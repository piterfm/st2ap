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
        private string _path;
        private Mock<ILogger> _logger;
        private ApMetadata _metadata;

        public Test_ApProxy() {
            _logger = new Mock<ILogger>();
            LogManager.Logger = _logger.Object;

            _path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _metadata = ApMetadata.LoadMetadata(_path);
        }

        [Fact]
        public void Connect_GetRequestStreamThrowsException() {
            var error = "Can't open request stream.";
            var status = WebExceptionStatus.NameResolutionFailure;
            var wex = new WebException(error, status);

            var request = new Mock<IHttpRequestWrapper>();
            request.Setup(r => r.GetRequestStream()).Throws(wex);
            var cp = new Mock<IConnectionProvider>();
            cp.Setup(x => x.GetRequest(It.IsAny<string>())).Returns(request.Object);

            _logger.Setup(x => x.LogMessage(GetLogMessage(status, "/dologin.jsp"), wex)).Verifiable();
            var ex = Assert.Throws<ApplicationException>(() => ApProxy.Connect(cp.Object, _metadata, "user", "password"));
            Assert.Equal(GetErrorMessage(status, error), ex.Message);
            _logger.Verify();
        }

        [Fact]
        public void Connect_GetResponseThrowsException() {
            var error = "Connection failure.";
            var status = WebExceptionStatus.ConnectFailure;
            var wex = new WebException(error, status);

            var request = new Mock<IHttpRequestWrapper>();
            request.Setup(r => r.GetRequestStream()).Returns(new MemoryStream());
            request.Setup(r => r.GetResponse()).Throws(wex);

            var cp = new Mock<IConnectionProvider>();
            cp.Setup(x => x.GetRequest(It.IsAny<string>())).Returns(request.Object);

            _logger.Setup(x => x.LogMessage(GetLogMessage(status, "/dologin.jsp"), wex)).Verifiable();

            var ex = Assert.Throws<ApplicationException>(() => ApProxy.Connect(cp.Object, _metadata, "user", "password"));

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

            var request = new Mock<IHttpRequestWrapper>();
            request.Setup(r => r.GetRequestStream()).Returns(new MemoryStream());
            request.Setup(r => r.GetResponse()).Returns(response.Object);

            var cp = new Mock<IConnectionProvider>();
            cp.Setup(x => x.GetRequest(It.IsAny<string>())).Returns(request.Object);

            _logger.Setup(x => x.LogMessage(GetLogMessage(status, error, "/dologin.jsp"))).Verifiable();
            _logger.Setup(x => x.LogWebResponse("/dologin.jsp", response.Object)).Verifiable();

            var ex = Assert.Throws<ApplicationException>(() => ApProxy.Connect(cp.Object, _metadata, "user", "password"));

            Assert.Equal(GetErrorMessage(status, error), ex.Message);
            _logger.Verify();
        }

        [Fact]
        public void Connect_ResponseWithNullLoginCookie() {
            var response = new Mock<IHttpResponseWrapper>();
            response.SetupGet(r => r.StatusCode).Returns(HttpStatusCode.OK);

            var request = new Mock<IHttpRequestWrapper>();
            request.Setup(r => r.GetRequestStream()).Returns(new MemoryStream());
            request.Setup(r => r.GetResponse()).Returns(response.Object);
            request.SetupGet(r => r.RequestUri).Returns(new Uri("http://attackpoint.org/dologin.jsp"));
            request.SetupGet(r => r.Cookies).Returns(new CookieCollection());

            var cp = new Mock<IConnectionProvider>();
            cp.Setup(x => x.GetRequest(It.IsAny<string>())).Returns(request.Object);

            _logger.Setup(x => x.LogMessage("Unable to authenticate on AttackPoint. Login cookie not found.")).Verifiable();
            _logger.Setup(x => x.LogWebResponse("/dologin.jsp", response.Object)).Verifiable();

            var ex = Assert.Throws<ApplicationException>(() => ApProxy.Connect(cp.Object, _metadata, "user", "password"));

            Assert.Equal("Unable to authenticate on AttackPoint. See log file for the response.", ex.Message);
            _logger.Verify();
        }

        [Fact]
        public void Connect_ResponseWithLoginCookieWithoutUserId() {
            var response = new Mock<IHttpResponseWrapper>();
            response.SetupGet(r => r.StatusCode).Returns(HttpStatusCode.OK);

            var request = new Mock<IHttpRequestWrapper>();
            request.Setup(r => r.GetRequestStream()).Returns(new MemoryStream());
            request.Setup(r => r.GetResponse()).Returns(response.Object);
            request.SetupGet(r => r.RequestUri).Returns(new Uri("http://attackpoint.org/dologin.jsp"));
            var cookies = new CookieCollection();
            var cookieValue = "value-which-does-not-have-colon-in-it";
            cookies.Add(new Cookie("login", cookieValue));
            request.SetupGet(r => r.Cookies).Returns(cookies);

            var cp = new Mock<IConnectionProvider>();
            cp.Setup(x => x.GetRequest(It.IsAny<string>())).Returns(request.Object);

            _logger.Setup(x => x.LogMessage("Can't find user ID in the cookie: " + cookieValue)).Verifiable();

            var ex = Assert.Throws<ApplicationException>(() => ApProxy.Connect(cp.Object, _metadata, "user", "password"));

            Assert.Equal("Unable to retrieve AP user ID. Perhaps the format of authentication has been changed. See log file for details.", ex.Message);
            _logger.Verify();
        }

        [Fact]
        public void Connect_PostedPayload() {
            var response = new Mock<IHttpResponseWrapper>();
            response.SetupGet(r => r.StatusCode).Returns(HttpStatusCode.OK);

            var request = new Mock<IHttpRequestWrapper>();
            request.Setup(r => r.GetResponse()).Returns(response.Object);
            request.SetupGet(r => r.RequestUri).Returns(new Uri("http://attackpoint.org/dologin.jsp"));
            var rstream = new MemoryStream();
            request.Setup(r => r.GetRequestStream()).Returns(rstream);

            var cookies = new CookieCollection();
            var cookieValue = "1234:auth-ticket";
            var cookie = new Cookie("login", cookieValue);
            cookie.Expired = false;
            cookies.Add(cookie);
            request.SetupGet(r => r.Cookies).Returns(cookies);

            var cp = new Mock<IConnectionProvider>();
            cp.Setup(x => x.GetRequest(It.IsAny<string>())).Returns(request.Object);

            var proxy = ApProxy.Connect(cp.Object, _metadata, "greg", "password");
            Assert.Equal("1234", proxy.UserId);
            Assert.Equal("greg", proxy.Username);
            Assert.Equal("password", proxy.Password);
            Assert.False(proxy.Expired);

            cookie.Expired = true;
            Assert.True(proxy.Expired);

            var bytes = rstream.ToArray();
            request.VerifySet(r => r.Method = "POST");
            request.VerifySet(r => r.ContentType = "application/x-www-form-urlencoded");
            request.VerifySet(r => r.ContentLength = 31);

            Assert.Equal(31, bytes.Length);
            Assert.Equal("username=greg&password=password", new ASCIIEncoding().GetString(bytes));
        }

        [Fact]
        public void Upload_PostedPayload() {
            Mock<IConnectionProvider> cp;
            Mock<IHttpRequestWrapper> request;
            Mock<IHttpResponseWrapper> response;

            var proxy = GetProxy(out cp, out request, out response);

            var note = new ApNote()
            {
                Date = new DateTime(2009, 7, 29),
                Description = "This is a note",
                RestingHeartRate = "61"
            };

            var rstream = new MemoryStream();
            cp.Setup(c => c.GetRequest("http://www.attackpoint.org/addcomment.jsp")).Returns(request.Object);
            request.Setup(r => r.GetRequestStream()).Returns(rstream);
            _logger.SetupGet(x => x.IsDebug).Returns(true);
            _logger.Setup(x => x.LogWebResponse("/addcomment.jsp", response.Object)).Verifiable();

            proxy.Upload(note);

            request.VerifySet(r => r.Method = "POST");
            request.VerifySet(r => r.ContentType = "application/x-www-form-urlencoded");
            request.VerifySet(r => r.ContentLength = 98);

            var bytes = rstream.ToArray();
            Assert.Equal(98, bytes.Length);
            Assert.Equal("session-month=07&session-day=29&session-year=2009&rhr=61&sleep=&weight=&description=This+is+a+note", new ASCIIEncoding().GetString(bytes));
            _logger.Verify();
            
        }

        [Fact]
        public void ScrapeData_GetResponseThrowsException() {
            Mock<IConnectionProvider> cp;
            Mock<IHttpRequestWrapper> request;
            Mock<IHttpResponseWrapper> response;

            var proxy = GetProxy(out cp, out request, out response);
            var profile = new ApProfile();

            var error = "Connection failure.";
            var status = WebExceptionStatus.ConnectFailure;
            var wex = new WebException(error, status);

            request = new Mock<IHttpRequestWrapper>();
            request.Setup(r => r.GetResponse()).Throws(wex);
            cp.Setup(x => x.GetRequest("http://www.attackpoint.org/jsonactivitytypes.jsp")).Returns(request.Object);

            _logger.Setup(x => x.LogMessage(GetLogMessage(status, "/jsonactivitytypes.jsp"), wex)).Verifiable();
            var ex = Assert.Throws<ApplicationException>(() => proxy.ScrapeApData(profile));
            Assert.Equal(GetErrorMessage(status, error), ex.Message);
            _logger.Verify();
        }

        [Fact]
        public void ScrapeData_ResponseWithNotOkStatus() {
            Mock<IConnectionProvider> cp;
            Mock<IHttpRequestWrapper> request;
            Mock<IHttpResponseWrapper> response;

            var proxy = GetProxy(out cp, out request, out response);
            var profile = new ApProfile();

            var error = "Resource not found";
            var status = HttpStatusCode.NotFound;

            response = new Mock<IHttpResponseWrapper>();
            response.SetupGet(r => r.StatusCode).Returns(status);
            response.SetupGet(r => r.StatusDescription).Returns(error);

            request = new Mock<IHttpRequestWrapper>();
            request.Setup(r => r.GetResponse()).Returns(response.Object);
            cp.Setup(x => x.GetRequest("http://www.attackpoint.org/jsonactivitytypes.jsp")).Returns(request.Object);

            _logger.Setup(x => x.LogMessage(GetLogMessage(status, error, "/jsonactivitytypes.jsp"))).Verifiable();
            _logger.Setup(x => x.LogWebResponse("/jsonactivitytypes.jsp", response.Object)).Verifiable();
            var ex = Assert.Throws<ApplicationException>(() => proxy.ScrapeApData(profile));
            Assert.Equal(GetErrorMessage(status, error), ex.Message);
            _logger.Verify();
        }

        [Fact]
        public void ScrapeData_GetResponseStreamThrowsException() {
            Mock<IConnectionProvider> cp;
            Mock<IHttpRequestWrapper> request;
            Mock<IHttpResponseWrapper> response;

            var proxy = GetProxy(out cp, out request, out response);
            var profile = new ApProfile();

            var error = "Not found.";
            var status = WebExceptionStatus.ProtocolError;
            var wex = new WebException(error, status);

            response = new Mock<IHttpResponseWrapper>();
            response.Setup(r => r.GetResponseStream()).Throws(wex);
            response.SetupGet(r => r.StatusCode).Returns(HttpStatusCode.OK);

            request = new Mock<IHttpRequestWrapper>();
            request.Setup(r => r.GetResponse()).Returns(response.Object);
            cp.Setup(x => x.GetRequest("http://www.attackpoint.org/jsonactivitytypes.jsp")).Returns(request.Object);

            _logger.Setup(x => x.LogMessage(GetLogMessage(status, "/jsonactivitytypes.jsp"), wex)).Verifiable();
            var ex = Assert.Throws<ApplicationException>(() => proxy.ScrapeApData(profile));
            Assert.Equal(GetErrorMessage(status, error), ex.Message);
            _logger.Verify();
        }

        [Fact]
        public void ScrapeData_RetrievedPage() {
            Mock<IConnectionProvider> cp;
            Mock<IHttpRequestWrapper> request;
            Mock<IHttpResponseWrapper> response;

            var proxy = GetProxy(out cp, out request, out response);
            var profile = new ApProfile();

            Setup(cp, "jsonactivitytypes.jsp", "activities.json");
            Setup(cp, "jsonshoes.jsp", "shoes.json");
            Setup(cp, "usermenu.jsp", "usersettings.htm");

            proxy.ScrapeApData(profile);

            Assert.True(profile.AdvancedFeaturesEnabledSpecified);
            Assert.True(profile.AdvancedFeaturesEnabled);
            Assert.Equal(3, profile.Activities.Count);
            Assert.Equal(3, profile.Shoes.Count);
        }

        private void Setup(Mock<IConnectionProvider> cp, string url, string filename) {
            var response = new Mock<IHttpResponseWrapper>();
            response.SetupGet(r => r.StatusCode).Returns(HttpStatusCode.OK);
            var stream = new FileStream(Path.Combine(_path, filename), FileMode.Open);
            response.Setup(r => r.GetResponseStream()).Returns(stream);

            var request = new Mock<IHttpRequestWrapper>();
            request.Setup(r => r.GetResponse()).Returns(response.Object);
            cp.Setup(x => x.GetRequest("http://www.attackpoint.org/" + url)).Returns(request.Object);
        }

        private ApProxy GetProxy(out Mock<IHttpRequestWrapper> request, out Mock<IHttpResponseWrapper> response) {
            Mock<IConnectionProvider> cp;
            return GetProxy(out cp, out request, out response);
        }

        private ApProxy GetProxy(out Mock<IConnectionProvider> cp, out Mock<IHttpRequestWrapper> request, out Mock<IHttpResponseWrapper> response) {
            cp = new Mock<IConnectionProvider>();
            response = new Mock<IHttpResponseWrapper>();
            request = new Mock<IHttpRequestWrapper>();
            response.SetupGet(r => r.StatusCode).Returns(HttpStatusCode.OK);

            request.Setup(r => r.GetResponse()).Returns(response.Object);
            request.SetupGet(r => r.RequestUri).Returns(new Uri("http://www.attackpoint.org/dologin.jsp"));
            var rstream = new MemoryStream();
            request.Setup(r => r.GetRequestStream()).Returns(rstream);

            var cookies = new CookieCollection();
            var cookieValue = "1234:auth-ticket";
            var cookie = new Cookie("login", cookieValue);
            cookie.Expired = false;
            cookies.Add(cookie);
            request.SetupGet(r => r.Cookies).Returns(cookies);

            cp.Setup(x => x.GetRequest("http://www.attackpoint.org/dologin.jsp")).Returns(request.Object);

            return ApProxy.Connect(cp.Object, _metadata, "greg", "password");
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
