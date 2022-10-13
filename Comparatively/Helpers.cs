using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comparatively
{
public static    class Helpers
    {        
        
        ///<remarks>Source: https://stackoverflow.com/a/11841175/6614025 </remarks>
        public static bool SetAppSetting(string Key, string Value)
        {
            bool result = false;
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                config.AppSettings.Settings.Remove(Key);
                var kvElem = new KeyValueConfigurationElement(Key, Value);
                config.AppSettings.Settings.Add(kvElem);

                // Save the configuration file.
                config.Save(ConfigurationSaveMode.Modified);

                // Force a reload of a changed section.
                ConfigurationManager.RefreshSection("appSettings");

                result = true;
            }
            finally
            { }
            return result;
        }

        public static string GetAppSetting(string key)
        {
            string Value = "";
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationElement Element = config.AppSettings.Settings[key];
                Value = Element.Value;
            }
            catch { }
            return Value;
        }
    }
}
