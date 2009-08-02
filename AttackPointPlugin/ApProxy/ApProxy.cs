using System;
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

namespace GK.AttackPoint
{
    public class ApProxy
    {
        private const string ApMetadataFileName = "ap-metadata.xml";
        private Dictionary<string, ApOperation> _apOperations = new Dictionary<string, ApOperation>();
        private CookieContainer _cookieContainer;
        private Cookie _loginCookie;

        public ApMetadata Metadata { get; private set; }
        public string UserId { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }

        public bool Expired {
            get { return _loginCookie == null || _loginCookie.Expired; }
        }

        public static ApProxy Connect(string path, string username, string password) {
            var proxy = new ApProxy();

            // Read metadata
            var ser = new XmlSerializer(typeof(ApMetadata));
            ApMetadata m;
            using (var reader = new StreamReader(Path.Combine(path, ApMetadataFileName))) {
                proxy.Metadata = m = (ApMetadata)ser.Deserialize(reader);
                foreach (var op in m.Operations) {
                    var page = new Dictionary<string, string>();
                    proxy._apOperations.Add(op.ClassName, op);
                }
            }

            // Authenticate on attackpoint website
            var parameters = new Dictionary<string, string>();
            parameters.Add(m.UsernameProperyName, username);
            parameters.Add(m.PasswordPropertyName, password);

            using (var response = proxy.PostRequest(m.LogingUrl, parameters, true)) {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new WebException(string.Format("Unable to authenticate on AttackPoint. HTTP status code: {0} - {1}", response.StatusCode, response.StatusDescription));
            }

            proxy.Username = username;
            proxy.Password = password;
            return proxy;
        }

        public string Upload(ApNote note) {
            string id = null;

            var operation = _apOperations[note.GetType().Name];
            var parameters = note.Pack(operation);

            using (var response = PostRequest(operation.PageUrl, parameters)) {
                LogManager.Logger.LogWebResponse(response);
            }

            return id;
        }

        public void ScrapeApData(ApProfile data) {
            var ser = new JsonSerializer();
            var json = FormatJson(RetrievePage(Metadata.ScrapeActivitiesUrl));
            using (var reader = new JsonReader(new StringReader(json))) {
                data.Activities = (List<ApActivity>)ser.Deserialize(reader, typeof(List<ApActivity>));
            }

            json = FormatJson(RetrievePage(Metadata.ScrapeShoesUrl));
            using (var reader = new JsonReader(new StringReader(json))) {
                data.Shoes = (List<ApShoes>)ser.Deserialize(reader, typeof(List<ApShoes>));
            }

            var html = RetrievePage(Metadata.ScrapeUserSettingsUrl);
            // I'd go with a JSON call. But there is no JSON call
            data.AdvancedFeaturesEnabled = html.Contains("paymenthistory");

            /* Alternative code that scrapes data from HTML using regular expressions */

            /*
            var html = RetrievePage(_apMetadata.ScrapeEntitiesUrl);

            data.Activities = PopulateEntities(GetDropDownList(html, "activitytypeid"), "-1");

            // The scraping works fine for the workouts and technical intensities
            // However, I decided to hard-code them to avoid potential problems
            //var cd = new ApConstantData();
            //cd.Workouts = PopulateEntities(GetDropDownList(html, "workouttypeid"), null);
            //cd.TechnicalIntensities = PopulateEntities(GetDropDownList(html, "map"), null);
            //data.Intensities = PopulateEntities(GetDropDownList(html, "intensity"), null); 
            
            html = RetrievePage(string.Format(_apMetadata.ScrapeShoesUrl, UserId));

            data.Shoes = new List<ApShoes>();

            for (var match = ShoesRegex.Match(html); match.Success; match = ShoesRegex.Match(html, match.Index + 1)) {
                var shoeId = match.Groups[1].Value;
                var cutoffIndex = html.IndexOf("</tr>", match.Index);
                var shoes = html.Substring(match.Index, cutoffIndex - match.Index);

                var name = Regex.Match(shoes, "<td[^>]*>(.*?)</td>");
                bool retired = shoes.Contains("shoes_row_retired");

                var title = Normalize(name.Groups[1].Value);
                Debug.WriteLine(string.Format("{0} - {1} {2}", shoeId, title, retired));

                data.Shoes.Add(new ApShoes() { Id = shoeId, Title = title, Retired = retired });
            }
            */

            // Scrape units
            //var html = RetrievePage(Metadata.ScrapeUnitsUrl);
            //data.WeightDistanceUnits = ScrapeUnits(html, "units") == "m" ? Units.Metric : Units.English;
            //data.ClimbUnits = ScrapeUnits(html, "cunits") == "0" ? Units.Metric : Units.English;
        }

        private string FormatJson(string json) {
            // Return empty array if nothing came back from the server
            return string.IsNullOrEmpty(json) ? "[]" : json;
        }

        //private static string DropDownPatternt = "<select[^>]*name\\s*\\=\\s*['\"]?{0}['\"]?[^>]*>(.*?)</select>";
        //private static Regex OptionsRegex = new Regex("<option[^>]*value\\s*=\\s*['\"]?([^'\">]*)['\"]?[^>]*>(.*?)<", RegexOptions.Singleline);
        //private static Regex NormalizationRegex = new Regex("\\s{2,}");
        //private static Regex ShoesRegex = new Regex("<a[^>]*href\\s*=\\s*['\"]/editshoes.jsp\\?shoesid=(\\d+?)['\"]\\s*>", RegexOptions.Singleline);
        //private static Regex UnitsRegex = new Regex("<option[^>]*value\\s*=\\s*['\"]?([^'\">\\s]*)['\"]?\\s*selected\\s*>", RegexOptions.Singleline);

        //private string GetDropDownList(string html, string id) {
        //    return Regex.Match(html, string.Format(DropDownPatternt, id), RegexOptions.Singleline).Value;
        //}

        //private List<ApEntity> PopulateEntities(string options, string ignoreId) {
        //    var entities = new List<ApEntity>();
        //    var match = OptionsRegex.Match(options);
        //    while (match.Success) {
        //        var id = match.Groups[1].Value.Trim();
        //        if (id != ignoreId) {
        //            var title = Normalize(match.Groups[2].Value);
        //            Debug.WriteLine(string.Format("{0} = {1} - '{2}'", id, title, match.Groups[2].Value));
        //            entities.Add(new ApEntity()
        //            {
        //                Id = id,
        //                Title = title
        //            });
        //        }
        //        match = OptionsRegex.Match(options, match.Index + 1);
        //    }
        //    return entities;
        //}

        //private string ScrapeUnits(string html, string id) {
        //    var dropDown = GetDropDownList(html, id);
        //    return UnitsRegex.Match(dropDown).Groups[1].Value;
        //}

        //private static string Normalize(string s) {
        //    return NormalizationRegex.Replace(s.Trim(), " ");
        //}

        private HttpWebResponse PostRequest(string url, Dictionary<string, string> parameters) {
            return PostRequest(url, parameters, false);
        }

        private HttpWebResponse PostRequest(string url, Dictionary<string, string> parameters, bool authenticate) {
            var payload = new StringBuilder();
            foreach (var p in parameters) {
                payload.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(p.Key), EncodingUtils.UrlEncodeForLatin1(p.Value));
            }

            LogManager.Logger.PrintMessage(payload.ToString());
            payload.Length = payload.Length - 1;
            byte[] bytes = new ASCIIEncoding().GetBytes(payload.ToString());

            var request = (HttpWebRequest)WebRequest.Create(Metadata.BaseUrl + url);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.ContentLength = bytes.Length;

            if (_cookieContainer == null) {
                _cookieContainer = new CookieContainer();
            }

            request.CookieContainer = _cookieContainer;

            using (var stream = request.GetRequestStream()) {
                stream.Write(bytes, 0, bytes.Length);
            }

            HttpWebResponse response = null;
            try {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex) {
                if (ex.Status == WebExceptionStatus.ProtocolError) {
                    LogManager.Logger.LogWebResponse((HttpWebResponse)ex.Response);
                    throw new ApplicationException(string.Format("Request failed with status {0}: {1}. See log file for the full response.", ex.Status, ex));
                }

                throw new ApplicationException(string.Format("Request failed with status {0}: {1}", ex.Status, ex));
            }

            // Check cookie container for authentication cookie
            if (authenticate) {
                var cookies = _cookieContainer.GetCookies(request.RequestUri);
                _loginCookie = cookies[Metadata.LoginCookieName];
                if (_loginCookie == null) {
                    LogManager.Logger.LogWebResponse(response);
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
            var request = (HttpWebRequest)WebRequest.Create(Metadata.BaseUrl + url);
            request.Method = "GET";
            request.CookieContainer = _cookieContainer;

            try {
                using (var stream = new StreamReader(request.GetResponse().GetResponseStream(), Encoding.GetEncoding("utf-8"))) {
                    var sb = new StringBuilder();
                    var writer = new StringWriter(sb);
                    try {
                        var read = new char[256];
                        // Reads 256 characters at a time.    
                        var count = stream.Read(read, 0, 256);
                        while (count > 0) {
                            var str = new string(read, 0, count);
                            writer.Write(str);
                            count = stream.Read(read, 0, 256);
                        }
                    }
                    finally {
                        if (stream != null) stream.Close();
                    }

                    return sb.ToString().Trim();
                }
            }
            catch (WebException ex) {
                if (ex.Status == WebExceptionStatus.ProtocolError) {
                    LogManager.Logger.LogWebResponse((HttpWebResponse)ex.Response);
                    throw new ApplicationException(string.Format("Request failed with status {0}: {1}. See log file for the response.", ex.Status, ex));
                }

                throw new ApplicationException(string.Format("Request failed with status {0}: {1}", ex.Status, ex));
            }
        }



    }

}
