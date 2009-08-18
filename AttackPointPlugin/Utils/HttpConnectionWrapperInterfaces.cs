using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace GK.Utils
{
    public interface IConnectionProvider
    {
        IHttpRequestWrapper GetRequest(string url);
        IHttpResponseWrapper GetResponse(WebException ex);
    }

    public interface IHttpRequestWrapper
    {
        string ContentType { set; }
        string Method { set; }
        long ContentLength { set; }
        CookieContainer CookieContainer { set; }
        Uri RequestUri { get; }

        Stream GetRequestStream();
        IHttpResponseWrapper GetResponse();
    }

    public interface IHttpResponseWrapper : IDisposable
    {
        HttpStatusCode StatusCode { get; }
        string StatusDescription { get; }
        Version ProtocolVersion { get; }
        string Method { get; }
        Uri ResponseUri { get; }
        string CharacterSet { get; }
        string ContentEncoding { get; }
        WebHeaderCollection Headers { get; }

        Stream GetResponseStream();
    }

}
