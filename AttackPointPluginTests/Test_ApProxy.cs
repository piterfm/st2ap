using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using GK.AttackPoint;
using System.Reflection;
using System.IO;
using Moq;
using System.Net;
using GK.Utils;

namespace AttackPointPluginTests
{
    public class Test_ApProxy// : TestBase
    {
        private string _path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private Mock<ILogger> _logger;

        public Test_ApProxy() {
            // Setup logger
            _logger = new Mock<ILogger>();
            LogManager.Logger = _logger.Object;
        }

        [Fact]
        public void Connect_GetRequestStreamThrowsException() {
            var error = "Can't open request stream.";
            var status = WebExceptionStatus.NameResolutionFailure;
            var wex = new WebException(error, status);

            var request = GetRequest();
            request.Setup(r => r.GetRequestStream()).Throws(wex);
            var cp = new Mock<IConnectionProvider>();
            cp.Setup(x => x.GetRequest(It.IsAny<string>())).Returns(request.Object);

            _logger.Setup(x => x.LogMessage(GetLogMessage(status, "/dologin.jsp"), wex)).Verifiable();

            var ex = Assert.Throws<ApplicationException>(() => ApProxy.Connect(cp.Object, _path, "user", "password"));

            Assert.Equal(GetErrorMessage(status, error), ex.Message);
            _logger.Verify();
        }

        [Fact]
        public void Connect_GetResponseThrowsException() {
            var error = "Connection failure.";
            var status = WebExceptionStatus.ConnectFailure;
            var wex = new WebException(error, status);

            var request = GetRequest();
            request.Setup(r => r.GetResponse()).Throws(wex);

            var cp = new Mock<IConnectionProvider>();
            cp.Setup(x => x.GetRequest(It.IsAny<string>())).Returns(request.Object);

            _logger.Setup(x => x.LogMessage(GetLogMessage(status, "/dologin.jsp"), wex)).Verifiable();

            var ex = Assert.Throws<ApplicationException>(() => ApProxy.Connect(cp.Object, _path, "user", "password"));

            Assert.Equal(GetErrorMessage(status, error), ex.Message);
            _logger.Verify();
        }

        [Fact]
        public void Connect_ResponseWithNotOkStatus() {
            var error = "Resource not found";
            var status = HttpStatusCode.NotFound;

            var response = new Mock<IHttpResponseWrapper>();
            response.SetupGet(r => r.StatusCode).Returns(status);
            response.SetupGet(r => r.StatusDescription).Returns(error);

            var request = GetRequest();
            request.Setup(r => r.GetResponse()).Returns(response.Object);

            var cp = new Mock<IConnectionProvider>();
            cp.Setup(x => x.GetRequest(It.IsAny<string>())).Returns(request.Object);

            _logger.Setup(x => x.LogMessage(GetLogMessage(status, error, "/dologin.jsp"))).Verifiable();
            _logger.Setup(x => x.LogWebResponse("/dologin.jsp", response.Object)).Verifiable();

            var ex = Assert.Throws<ApplicationException>(() => ApProxy.Connect(cp.Object, _path, "user", "password"));

            Assert.Equal(GetErrorMessage(status, error), ex.Message);
            _logger.Verify();
        }

        [Fact]
        public void Connect_ResponseWithNullLoginCookie() {
            var response = new Mock<IHttpResponseWrapper>();
            response.SetupGet(r => r.StatusCode).Returns(HttpStatusCode.OK);

            var request = GetRequest();
            request.Setup(r => r.GetResponse()).Returns(response.Object);
            request.SetupGet(r => r.RequestUri).Returns(new Uri("http://attackpoint.org/dologin.jsp"));

            var cp = new Mock<IConnectionProvider>();
            cp.Setup(x => x.GetRequest(It.IsAny<string>())).Returns(request.Object);

            _logger.Setup(x => x.LogMessage("Unable to authenticate on AttackPoint. Login cookie not found.")).Verifiable();
            _logger.Setup(x => x.LogWebResponse("/dologin.jsp", response.Object)).Verifiable();

            var ex = Assert.Throws<ApplicationException>(() => ApProxy.Connect(cp.Object, _path, "user", "password"));

            Assert.Equal("Unable to authenticate on AttackPoint. See log file for the response.", ex.Message);
            _logger.Verify();
        }

        [Fact(Skip="TODO: Implement")]
        public void Connect_ResponseWithLoginCookieWithoutUserId() {
            //var response = new Mock<IHttpResponseWrapper>();
            //response.SetupGet(r => r.StatusCode).Returns(HttpStatusCode.OK);

            //var uri = new Uri("http://attackpoint.org/dologin.jsp");
            //var request = GetRequest();
            //request.Setup(r => r.GetResponse()).Returns(response.Object);
            //request.SetupGet(r => r.RequestUri).Returns(uri);
            ////request.SetupSet(r => r.CookieContainer = c);

            //var cp = new Mock<IConnectionProvider>();
            //cp.Setup(x => x.GetRequest(It.IsAny<string>())).Returns(request.Object);
            //_logger.Setup(x => x.LogMessage("Can't find user ID in the cookie: cookievalue")).Verifiable();

            //var ex = Assert.Throws<ApplicationException>(() => ApProxy.Connect(cp.Object, _path, "user", "password"));
            //Assert.Equal("Unable to retrieve AP user ID. Perhaps the format of authentication has been changed. See log file for details.", ex.Message);
            //_logger.Verify();
        }

        [Fact]
        public void Connect_PostedPayload() {
            var response = new Mock<IHttpResponseWrapper>();
            response.SetupGet(r => r.StatusCode).Returns(HttpStatusCode.OK);

            var request = GetRequest();
            request.Setup(r => r.GetResponse()).Returns(response.Object);
            request.SetupGet(r => r.RequestUri).Returns(new Uri("http://attackpoint.org/dologin.jsp"));
            var rstream = new MemoryStream();
            request.Setup(r => r.GetRequestStream()).Returns(rstream);

            var cp = new Mock<IConnectionProvider>();
            cp.Setup(x => x.GetRequest(It.IsAny<string>())).Returns(request.Object);

            // Login cookie will be null, so the exception will be thrown
            // I don't care. I am interested in the stuff that happens before that
            Assert.Throws<ApplicationException>(() => ApProxy.Connect(cp.Object, _path, "user", "password"));

            var bytes = rstream.ToArray();
            request.VerifySet(r => r.Method = "POST");
            request.VerifySet(r => r.ContentType = "application/x-www-form-urlencoded");
            request.VerifySet(r => r.ContentLength = 31);

            Assert.Equal(31, bytes.Length);
            Assert.Equal("username=user&password=password", new ASCIIEncoding().GetString(bytes));
        }

        //[Fact]
        //public void Upload_PostedPayload() {
        //}

        //[Fact]
        //public void ScrapeData_GetResponseThrowsException() {
        //}

        //[Fact]
        //public void ScrapeData_GetResponseStreamThrowsException() {
        //}

        //[Fact]
        //public void ScrapeData_RetrievedPage() {
        //}

        private Mock<IHttpRequestWrapper> GetRequest() {
            var request = new Mock<IHttpRequestWrapper>();
            request.Setup(r => r.GetRequestStream()).Returns(new MemoryStream());
            return request;
        }

        private Mock<IHttpResponseWrapper> GetResponse(Stream stream) {
            var response = new Mock<IHttpResponseWrapper>();
            response.Setup(r => r.GetResponseStream()).Returns(stream);
            return response;
        }

        private Mock<IConnectionProvider> GetConnectionProvider(string url) {
            var cp = new Mock<IConnectionProvider>();
            cp.Setup(x => x.GetRequest(url)).Returns(GetRequest().Object);
            return cp;
        }

        private static string GetErrorMessage(WebExceptionStatus status, string error) {
            return string.Format("Request failed with status {0}: {1} See log file for details.", status, error);
        }

        private static string GetErrorMessage(HttpStatusCode status, string error) {
            return string.Format("Request failed with status {0} ({1}). See log file for the full response.", status, error);
        }

        private static string GetLogMessage(WebExceptionStatus status, string url) {
            return string.Format("Request to URL '{1}' failed with status {0}.", status, url);
        }

        private static string GetLogMessage(HttpStatusCode status, string error, string url) {
            return string.Format("Request to URL '{2}' failed with status {0} ({1}).", status, error, url);
        }
    }
}
