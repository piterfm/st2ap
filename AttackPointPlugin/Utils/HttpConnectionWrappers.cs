using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace GK.Utils
{

    public class HttpConnectionProvider : IConnectionProvider
    {
        private int _timeout;
        private IWebProxy _webProxy;
        private string _userAgent;
        private CookieContainer _cookieContainer;

        public HttpConnectionProvider(int timeout, IWebProxy webProxy, string userAgent) {
            _timeout = timeout;
            _webProxy = webProxy;
            _userAgent = userAgent;
            _cookieContainer = new CookieContainer();
        }

        public IHttpRequestWrapper GetRequest(string url) {
            var request = (HttpWebRequest)WebRequest.Create(url);

            if (Environment.OSVersion.Platform != PlatformID.Unix) {
                request.Credentials = CredentialCache.DefaultCredentials;
            }
            request.Proxy = GetWebProxy();
            request.UserAgent = _userAgent;
            request.Timeout = _timeout;
            request.CookieContainer = _cookieContainer;

            return new HttpRequestWrapper(request);
        }

        public IHttpResponseWrapper GetResponse(WebException ex) {
            return ex.Response != null ? new HttpResponseWrapper((HttpWebResponse)ex.Response) : null;
        }

        private IWebProxy GetWebProxy() {
            if (_webProxy != null)
                return _webProxy;

            var proxy = WebRequest.DefaultWebProxy;
            if (Environment.OSVersion.Platform != PlatformID.Unix) {
                proxy.Credentials = CredentialCache.DefaultCredentials;
            }
            return proxy;
        }

    }

    public class HttpRequestWrapper : IHttpRequestWrapper
    {
        private HttpWebRequest _request;

        public HttpRequestWrapper(HttpWebRequest request) {
            _request = request;
        }

        public string ContentType { set { _request.ContentType = value; } }
        public string Method { set { _request.Method = value; } }
        public long ContentLength { set { _request.ContentLength = value; } }
        public Uri RequestUri { get { return _request.RequestUri; } }
        public CookieCollection Cookies { get { return _request.CookieContainer.GetCookies(RequestUri); } }
        public Stream GetRequestStream() { return _request.GetRequestStream(); }

        public IHttpResponseWrapper GetResponse() {
            return new HttpResponseWrapper((HttpWebResponse)_request.GetResponse());
        }

    }

    public class HttpResponseWrapper : IHttpResponseWrapper
    {
        private HttpWebResponse _response;

        public HttpResponseWrapper(HttpWebResponse response) {
            _response = response;
        }

        public HttpStatusCode StatusCode { get { return _response.StatusCode; } }
        public string StatusDescription { get { return _response.StatusDescription; } }
        public Version ProtocolVersion { get { return _response.ProtocolVersion; } }
        public string Method { get { return _response.Method; } }
        public Uri ResponseUri { get { return _response.ResponseUri; } }
        public string CharacterSet { get { return _response.CharacterSet; } }
        public string ContentEncoding { get { return _response.ContentEncoding; } }
        public WebHeaderCollection Headers { get { return _response.Headers; } }
        public Stream GetResponseStream() { return _response.GetResponseStream(); }
        void IDisposable.Dispose() { ((IDisposable)_response).Dispose(); }

    }


}
