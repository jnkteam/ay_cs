namespace KuaiCard.SysConfig
{
    using KuaiCardLib.Configuration;
    using System;
    using System.IO;

    public class MemCachedConfig
    {
        private static readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Configurations\memcached.config");
        private static readonly string _group = "MemCachedConfig";

        public static string GetConfig(string key)
        {
            return ConfigHelper.GetConfig(_filePath, _group, key);
        }

        public static bool ApplyBase64
        {
            get
            {
                return Convert.ToBoolean(GetConfig("ApplyBase64"));
            }
        }

        public static bool ApplyMemCached
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(GetConfig("ApplyMemCached"));
                }
                catch
                {
                    return false;
                }
            }
        }

        public static string AuthCode
        {
            get
            {
                return GetConfig("AuthCode");
            }
        }

        public static bool FailOver
        {
            get
            {
                return Convert.ToBoolean(GetConfig("FailOver"));
            }
        }

        public string HashingAlgorithm
        {
            get
            {
                return GetConfig("HashingAlgorithm");
            }
        }

        public static int IntConnections
        {
            get
            {
                int num = Convert.ToInt32(GetConfig("IntConnections"));
                return ((num > 0) ? num : 3);
            }
        }

        public static int LocalCacheTime
        {
            get
            {
                return Convert.ToInt32(GetConfig("LocalCacheTime"));
            }
        }

        public static int MaintenanceSleep
        {
            get
            {
                int num = Convert.ToInt32(GetConfig("MaintenanceSleep"));
                return ((num > 0) ? num : 30);
            }
        }

        public static int MaxConnections
        {
            get
            {
                int num = Convert.ToInt32(GetConfig("MaxConnections"));
                return ((num > 0) ? num : 5);
            }
        }

        public static int MemCacheTime
        {
            get
            {
                return Convert.ToInt32(GetConfig("MemCacheTime"));
            }
        }

        public static int MinConnections
        {
            get
            {
                int num = Convert.ToInt32(GetConfig("MinConnections"));
                return ((num > 0) ? num : 3);
            }
        }

        public static bool Nagle
        {
            get
            {
                return Convert.ToBoolean(GetConfig("Nagle"));
            }
        }

        public static string PoolName
        {
            get
            {
                return GetConfig("PoolName");
            }
        }

        public static bool RecordeLog
        {
            get
            {
                return Convert.ToBoolean(GetConfig("RecordeLog"));
            }
        }

        public static string ServerList
        {
            get
            {
                return GetConfig("ServerList");
            }
        }

        public static string SettingGroup
        {
            get
            {
                return _group;
            }
        }

        public static int SocketConnectTimeout
        {
            get
            {
                int num = Convert.ToInt32(GetConfig("SocketConnectTimeout"));
                return ((num > 0x3e8) ? num : 0x3e8);
            }
        }

        public static int SocketTimeout
        {
            get
            {
                return ((Convert.ToInt32(GetConfig("SocketTimeout")) > 0x3e8) ? MaintenanceSleep : 0xbb8);
            }
        }

        public static string SyncCacheUrl
        {
            get
            {
                return GetConfig("SyncCacheUrl");
            }
        }

        public static string Weights
        {
            get
            {
                return GetConfig("Weights");
            }
        }
    }
}

