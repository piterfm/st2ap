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
    public class Test_ApProxy
    {
        private string _path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        [Fact]
        public void ConnectWithInvalidUsername() {
            var request = GetRequest();
            request.Setup(r => r.GetRequestStream()).Throws(new WebException("Can't open request stream", WebExceptionStatus.NameResolutionFailure));
            var cp = new Mock<IConnectionProvider>();
            cp.Setup(x => x.GetRequest(It.IsAny<string>())).Returns(request.Object);

            var ex = Assert.Throws<ApplicationException>(new Assert.ThrowsDelegate(
                () => ApProxy.Connect(cp.Object, _path, "user", "password")));

            Assert.Equal("Request failed with status NameResolutionFailure: Can't open request stream See log file for details.", ex.Message);
        }

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

    }
}
