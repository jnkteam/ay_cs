namespace KuaiCard.SysConfig
{
    using KuaiCardLib.Configuration;
    using System;
    using System.IO;

    public sealed class MongoDBSetting
    {
        private static readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Configurations\memcached.config");
        private static readonly string _group = "MongoDB";

        private MongoDBSetting()
        {
        }

        public static string GetConfig(string group, string key)
        {
            return ConfigHelper.GetConfig(_filePath, group, key);
        }

        public static string CollectionName
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "collectionName");
            }
        }

        public static string Connstring
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "connStr");
            }
        }

        public static string DefaultDB
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "defaultdb");
            }
        }

        public static string SettingGroup
        {
            get
            {
                return _group;
            }
        }

        public static string SiteDomain
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "Sitedomain");
            }
        }

        public static string WebSiteDescription
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "WebSiteDescription");
            }
        }
    }
}

