namespace OriginalStudio.Cache
{
    using System;
    using System.Configuration;

    public class BaseConfigs
    {
        private static string webPath = "";

        static BaseConfigs()
        {
            webPath = System.Configuration.ConfigurationSettings.AppSettings["WebPath"];
        }

        public static string GetPath
        {
            get
            {
                return webPath;
            }
        }
    }
}

