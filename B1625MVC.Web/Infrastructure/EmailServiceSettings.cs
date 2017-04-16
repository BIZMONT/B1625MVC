using System.Configuration;
using System.Web.Configuration;
using System.Linq;

namespace B1625MVC.Web.Infrastructure
{
    public class EmailServiceSettings
    {
        private const string EmailEnable = "EnableEmailVerification";
        private const string SmtpHost = "SmtpHost";
        private const string SmtpPort = "SmtpPort";
        private const string SmtpEnableSsl = "SmtpEnableSsl";
        private const string SmtpLogin = "SmtpLogin";
        private const string SmtpPassword = "SmtpPassword";

        Configuration _configs = WebConfigurationManager.OpenWebConfiguration("~");

        static EmailServiceSettings()
        {
            Configuration configs = WebConfigurationManager.OpenWebConfiguration("~");
            if (!configs.AppSettings.Settings.AllKeys.Contains(EmailEnable) ||
                !configs.AppSettings.Settings.AllKeys.Contains(SmtpHost) ||
                !configs.AppSettings.Settings.AllKeys.Contains(SmtpPort) ||
                !configs.AppSettings.Settings.AllKeys.Contains(SmtpEnableSsl) ||
                !configs.AppSettings.Settings.AllKeys.Contains(SmtpLogin) ||
                !configs.AppSettings.Settings.AllKeys.Contains(SmtpPassword))
            {
                configs.AppSettings.Settings.Add(EmailEnable, false.ToString());
                configs.AppSettings.Settings.Add(SmtpHost, string.Empty);
                configs.AppSettings.Settings.Add(SmtpPort, 0.ToString());
                configs.AppSettings.Settings.Add(SmtpEnableSsl, false.ToString());
                configs.AppSettings.Settings.Add(SmtpLogin, string.Empty);
                configs.AppSettings.Settings.Add(SmtpPassword, string.Empty);
                configs.Save();
            }
        }

        public string Host
        {
            get
            {
                return _configs.AppSettings.Settings[SmtpHost].Value;
            }
            set
            {
                _configs.AppSettings.Settings[SmtpHost].Value = value;
                _configs.Save();
            }
        }
        public bool IsEmailServiceEnabled
        {
            get
            {
                return bool.Parse(_configs.AppSettings.Settings[EmailEnable].Value);
            }
            set
            {
                _configs.AppSettings.Settings[EmailEnable].Value = value.ToString();
                _configs.Save();
            }
        }
        public int Port
        {
            get
            {
                return int.Parse(_configs.AppSettings.Settings[SmtpPort].Value);
            }
            set
            {
                _configs.AppSettings.Settings[SmtpPort].Value = value.ToString();
                _configs.Save();
            }
        }
        public bool SslEnabled
        {
            get
            {
                return bool.Parse(_configs.AppSettings.Settings[SmtpEnableSsl].Value);
            }
            set
            {
                _configs.AppSettings.Settings[SmtpEnableSsl].Value = value.ToString();
                _configs.Save();
            }
        }
        public string Password
        {
            get
            {
                return _configs.AppSettings.Settings[SmtpPassword].Value;
            }
            set
            {
                _configs.AppSettings.Settings[SmtpPassword].Value = value;
                _configs.Save();
            }
        }
        public string Login
        {
            get
            {
                return _configs.AppSettings.Settings[SmtpLogin].Value;
            }
            set
            {
                _configs.AppSettings.Settings[SmtpLogin].Value = value;
                _configs.Save();
            }
        }
    }
}