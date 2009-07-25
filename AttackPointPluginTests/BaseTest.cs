using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using GK.AttackPoint;
using GK.SportTracks.AttackPoint;

namespace AttackPointPluginTests
{
    public class BaseTest
    {
        protected ApMetadata _metadata;
        protected ApConfig _config;
        protected ApProfile _profile;

        protected BaseTest() {
            using (var reader = new StreamReader("ap-metadata.xml")) {
                _metadata = (ApMetadata)new XmlSerializer(typeof(ApMetadata)).Deserialize(reader);
            }

            using (var reader = new StreamReader("ap-configuration.xml")) {
                _config = (ApConfig)new XmlSerializer(typeof(ApConfig)).Deserialize(reader);
            }

            using (var reader = new StreamReader("ap-constant-data.xml")) {
                var ser = new XmlSerializer(typeof(ApConstantData));
                _config.Profile.ConstantData = (ApConstantData)ser.Deserialize(reader);
            }

            _profile = _config.Profile;
        }


    }
}
