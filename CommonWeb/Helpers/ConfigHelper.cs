using System;
using System.Collections.Specialized;

namespace CommonWeb.Helpers
{
    public static class ConfigHelper
    {
        public static T GetConfigSectionValue<T>(string section, string property)
        {
            T value = default(T);

            NameValueCollection configSection = (NameValueCollection)System.Configuration.ConfigurationManager.GetSection(section);
            if (configSection == null)
            {
                throw new Exception("La sezione " + section + " non è presente sul file di configurazione");
            }

            try
            {
                value = (T)Convert.ChangeType(configSection[property], typeof(T));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return value;
        }
    }
}