using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCS.K2.NFLN.Helpers
{
    public static class SettingsHelper
    {
        public static void InitializeSettings()
        {
            var setting = ConfigurationManager.AppSettings;
            if (setting["VizIp"] == null)
            {
                SetOrCreateSetting("vizIp", "10.1.1.111");
            }
            if (setting["VizPort"] == null)
            {
                SetOrCreateSetting("vizPort", "6100");
            }
            if (setting["FeedbackPort"] == null)
            {
                SetOrCreateSetting("FeedbackPort", "666");
            }
            if (setting["K2Ip"] == null)
            {
                SetOrCreateSetting("K2Ip", "10.0.2.40");
            }
            if (setting["K2UserName"] == null)
            {
                SetOrCreateSetting("K2UserName", "administrator");
            }
            if (setting["K2Password"] == null)
            {
                SetOrCreateSetting("K2Password", "adminGV!");
            }
        }

        public static bool SetOrCreateSetting(string settingName, string value)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var change = false;
            if (ConfigurationManager.AppSettings[settingName] == null)
            {
                config.AppSettings.Settings.Add(settingName, value);
            }
            else
            {
                change = config.AppSettings.Settings[settingName].Value != value;
                config.AppSettings.Settings[settingName].Value = value;
            }
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
            return change;
        }
    }
}