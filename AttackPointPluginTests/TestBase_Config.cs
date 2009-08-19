using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using GK.AttackPoint;
using System.Xml.Serialization;
using GK.SportTracks.AttackPoint;

namespace AttackPointPluginTests
{
    /// <summary>
    /// Base class for test suites that use configuration and metadata.
    /// </summary>
    public class TestBase_Config : TestBase
    {
        protected ApMetadata _metadata;
        protected ApConfig _config;
        protected ApProfile _profile;

        protected TestBase_Config() {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            using (var reader = new StreamReader(Path.Combine(path, "ap-metadata.xml"))) {
                _metadata = (ApMetadata)new XmlSerializer(typeof(ApMetadata)).Deserialize(reader);
            }

            using (var reader = new StreamReader(Path.Combine(path, "ap-configuration.xml"))) {
                _config = (ApConfig)new XmlSerializer(typeof(ApConfig)).Deserialize(reader);
            }

            using (var reader = new StreamReader(Path.Combine(path, "ap-constant-data.xml"))) {
                var ser = new XmlSerializer(typeof(ApConstantData));
                _config.Profile.ConstantData = (ApConstantData)ser.Deserialize(reader);
            }

            _profile = _config.Profile;
        }

    }
}
