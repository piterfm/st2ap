﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Net;
using System.Web;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using GK.Utils;

namespace GK.AttackPoint
{
    public class ApProxy
    {
        private Dictionary<string, ApOperation> _apOperations = new Dictionary<string, ApOperation>();
        private Cookie _loginCookie;
        private IConnectionProvider _connectionProvider;

        private ApProxy(IConnectionProvider connectionProvider, ApMetadata metadata) {
            _connectionProvider = connectionProvider;
            Metadata = metadata;
        }

        public ApMetadata Metadata { get; private set; }
        public string UserId { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }

        public bool Expired {
            get { return _loginCookie == null || _loginCookie.Expired; }
        }

        public static ApProxy Connect(IConnectionProvider connectionProvider, ApMetadata metadata, string username, string password) {
            var proxy = new ApProxy(connectionProvider, metadata);

            // Authenticate on attackpoint website
            var parameters = new Dictionary<string, string>();
            parameters.Add(metadata.UsernameProperyName, username);
            parameters.Add(metadata.PasswordPropertyName, password);

            using (var response = proxy.PostRequest(metadata.LogingUrl, parameters, true)) {
                if (LogManager.Logger.IsDebug) {
                    LogManager.Logger.LogWebResponse(metadata.LogingUrl, response);
                }
            }

            proxy.Username = username;
            proxy.Password = password;
            return proxy;
        }

        public string Upload(ApNote note) {
            string id = null;

            ApOperation operation = null;
            foreach (var op in Metadata.Operations) {
                if (note.GetType().Name == op.ClassName) {
                    operation = op;
                }
            }

            if (operation == null) {
                LogManager.Logger.LogMessage("Invalid metadata. Operation not found for class type: " + note.GetType().FullName);
                throw new ApplicationException("AttackPoint plugin setup is corrupted. Unable to export.");
            }

            var parameters = note.Pack(operation);
            using (var response = PostRequest(operation.PageUrl, parameters)) {
                if (LogManager.Logger.IsDebug) {
                    LogManager.Logger.LogWebResponse(operation.PageUrl, response);
                }
            }

            return id; // not used.
        }

        public void ScrapeApData(ApProfile profile) {
            var ser = new JsonSerializer();
            var json = FormatJson(RetrievePage(Metadata.ScrapeActivitiesUrl));
            using (var reader = new JsonReader(new StringReader(json))) {
                profile.Activities = (List<ApActivity>)ser.Deserialize(reader, typeof(List<ApActivity>));
            }

            json = FormatJson(RetrievePage(Metadata.ScrapeShoesUrl));
            using (var reader = new JsonReader(new StringReader(json))) {
                profile.Shoes = (List<ApShoes>)ser.Deserialize(reader, typeof(List<ApShoes>));
            }

            var html = RetrievePage(Metadata.ScrapeUserSettingsUrl);
            // I'd go with a JSON call. But there is no JSON call
            profile.AdvancedFeaturesEnabled = html.Contains("paymenthistory");
        }

        private string FormatJson(string json) {
            // Return empty array if nothing came back from the server
            return string.IsNullOrEmpty(json) ? "[]" : json;
        }

        private IHttpResponseWrapper PostRequest(string url, Dictionary<string, string> parameters) {
            return PostRequest(url, parameters, false);
        }

        private IHttpResponseWrapper PostRequest(string url, Dictionary<string, string> parameters, bool authenticate)
        {
            var payload = new StringBuilder();
            foreach (var p in parameters) {
                payload.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(p.Key), EncodingUtils.UrlEncode(p.Value));
            }

            payload.Length = payload.Length - 1;
            byte[] bytes = new ASCIIEncoding().GetBytes(payload.ToString());
            if (LogManager.Logger.IsDebug && !authenticate) {
                LogManager.Logger.LogMessage(payload.ToString(), false);
            }

            var request = _connectionProvider.GetRequest(Metadata.BaseUrl + url);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.ContentLength = bytes.Length;

            Stream stream = null;
            try {
                stream = request.GetRequestStream();
                stream.Write(bytes, 0, bytes.Length);
            }
            catch (WebException ex) {
                ProcessWebException(url, ex);
            }
            finally {
                if (stream != null)
                    stream.Close();
            }

            IHttpResponseWrapper response = null;
            try {
                response = request.GetResponse();
            }
            catch (WebException ex) {
                ProcessWebException(url, ex);
            }

            EnsureResponseIsOk(url, response);

            // Check cookie container for authentication cookie
            if (authenticate) {
                _loginCookie = request.Cookies[Metadata.LoginCookieName];
                if (_loginCookie == null) {
                    LogManager.Logger.LogMessage("Unable to authenticate on AttackPoint. Login cookie not found.");
                    LogManager.Logger.LogWebResponse(url, response);
                    throw new ApplicationException("Unable to authenticate on AttackPoint. See log file for the response.");
                }
                
                var k = _loginCookie.Value.IndexOf(':');
                if (k <= 0) {
                    LogManager.Logger.LogMessage("Can't find user ID in the cookie: " + _loginCookie.Value);
                    throw new ApplicationException("Unable to retrieve AP user ID. Perhaps the format of authentication has been changed. See log file for details.");
                }

                UserId = _loginCookie.Value.Substring(0, k);
            }

            return response;
        }

        private string RetrievePage(string url) {
            string page = null;
            var request = _connectionProvider.GetRequest(Metadata.BaseUrl + url);
            request.Method = "GET";

            IHttpResponseWrapper response = null;
            try {
                response = request.GetResponse();
                EnsureResponseIsOk(url, response);
                using (var stream = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"))) {
                    var sb = new StringBuilder();
                    var writer = new StringWriter(sb);
                    var read = new char[256];
                    // Reads 256 characters at a time.    
                    var count = stream.Read(read, 0, 256);
                    while (count > 0) {
                        var str = new string(read, 0, count);
                        writer.Write(str);
                        count = stream.Read(read, 0, 256);
                    }

                    page = sb.ToString().Trim();
                }
            }
            catch (WebException ex) {
                ProcessWebException(url, ex);
            }
            finally {
                if (response != null)
                    response.Dispose();
            }

            return page;
        }

        private void EnsureResponseIsOk(string url, IHttpResponseWrapper response) {
            if (response == null || response.StatusCode == HttpStatusCode.OK) return;

            LogManager.Logger.LogMessage(string.Format("Request to URL '{2}' failed with status {0} ({1}).", response.StatusCode, response.StatusDescription, url));
            LogManager.Logger.LogWebResponse(url, response);
            throw new ApplicationException(string.Format("Request failed with status {0} ({1}). See log file for the full response.", response.StatusCode, response.StatusDescription));
        }

        private void ProcessWebException(string url, WebException ex) {
            LogManager.Logger.LogMessage(string.Format("Request to URL '{1}' failed with status {0}.", ex.Status, url), ex);
            
            var response = _connectionProvider.GetResponse(ex);
            if (response != null) {
                LogManager.Logger.LogWebResponse(url, response);
            }

            throw new ApplicationException(string.Format("Request failed with status {0}: {1} See log file for details.", ex.Status, ex.Message), ex);
        }

    }

}
