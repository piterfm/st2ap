using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using GK.AttackPoint;
using System.Reflection;
using System.IO;

namespace AttackPointPluginTests
{
    public class Test_ApProxy
    {
        private string _path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        [Fact]
        public void ConnectWithInvalidUsername() {
            //var proxy = ApProxy.Connect(_path, "user", "password");
        }
    }
}
