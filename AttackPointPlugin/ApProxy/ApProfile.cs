using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Diagnostics;
using System.IO;

namespace GK.AttackPoint
{
    public class ApProfile
    {
        private const string ApConstantDataFileName = "ap-constant-data.xml";

        private string _username;
        private string _password;
        private string _oldUsername;
        private string _oldPassword;
        private bool _advancedFeaturesEnabled;

        public ApProfile() { }
        public ApProfile(string basePath) {
            ReloadConstantData(basePath);
        }

        internal void ReloadConstantData(string basePath) {
            var ser = new XmlSerializer(typeof(ApConstantData));
            using (var reader = new StreamReader(Path.Combine(basePath, ApConstantDataFileName))) {
                ConstantData = (ApConstantData)ser.Deserialize(reader);
            }
        }

        [XmlAttribute("username")]
        public string Username {
            get { return _username; }
            set {
                _oldUsername = _username ?? value;
                _username = value;
            }
        }
        
        [XmlIgnore]
        public string Password {
            get { return _password; }
            set {
                _oldPassword = _password ?? value;
                _password = value;
            }
        }

        [XmlAttribute("advanced")]
        public bool AdvancedFeaturesEnabled {
            get { return _advancedFeaturesEnabled; }
            set {
                _advancedFeaturesEnabled = value;
                AdvancedFeaturesEnabledSpecified = true;
            }
        }

        [XmlIgnore]
        public bool AdvancedFeaturesEnabledSpecified { get; set; }
        [XmlIgnore]
        public ApConstantData ConstantData;

        public List<ApActivity> Activities { get; set; }
        public List<ApShoes> Shoes { get; set; }

        [XmlIgnore]
        public List<ApEntity> Intensities { get { return ConstantData.Intensities; } }
        [XmlIgnore]
        public List<ApEntity> Workouts { get { return ConstantData.Workouts; } }
        [XmlIgnore]
        public List<ApEntity> TechnicalIntensities { get { return ConstantData.TechnicalIntensities; } }

        [XmlAttribute("password")]
        public byte[] ProtectedPassword {
            get {
                if (string.IsNullOrEmpty(Password)) return null;
                return ProtectedData.Protect(Encoding.UTF8.GetBytes(Password), null, DataProtectionScope.CurrentUser);
            }
            set {
                if (value == null || value.Length == 0) Password = null;
                try {
                    Password = Encoding.UTF8.GetString(ProtectedData.Unprotect(value, null, DataProtectionScope.CurrentUser));
                }
                catch (Exception ex) {
                    Password = null;
                    LogManager.Logger.LogMessage("Unable to decrypt password.", ex);
                }
            }
        }

        public bool UsernameChanged {
            get { return _oldUsername != _username; }
        }

        public bool CredentialsChanged {
            get { return _oldUsername != _username || _oldPassword != _password; }
        }

        public bool IsEmpty {
            get {
                return
                    !AdvancedFeaturesEnabledSpecified ||
                    string.IsNullOrEmpty(Username) ||
                    string.IsNullOrEmpty(Password) ||
                    Activities == null ||
                    Shoes == null;
            }
        }

        internal ApEntity GetActivity(string id) {
            if (Activities == null) return null;
            return Activities.Find(a => a.Id == id);
        }

        internal bool ContainsWorkoutId(string id) {
            return Workouts != null && Workouts.Find(w => w.Id == id) != null;
        }

        internal bool ContainsTechnicalIntensityId(string id) {
            return TechnicalIntensities != null && TechnicalIntensities.Find(t => t.Id == id) != null;
        }
    }

    public enum Units {
        None,
        Metric,
        English
    }

    public enum Quantity
    {
        Climb,
        Distance,
        Weight
    }
}
