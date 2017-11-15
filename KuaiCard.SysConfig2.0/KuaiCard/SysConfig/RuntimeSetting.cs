namespace KuaiCard.SysConfig
{
    using KuaiCardLib.Configuration;
    using System;
    using System.IO;

    public sealed class RuntimeSetting
    {
        private static readonly string _group = "runtimeSettings";

        private RuntimeSetting()
        {
        }

        public static bool CheckUrlReferrer
        {
            get
            {
                string config = ConfigHelper.GetConfig(SettingGroup, "CheckUrlReferrer");
                return (string.IsNullOrEmpty(config) || (config == "1"));
            }
        }

        public static bool CheckUserOrderNo
        {
            get
            {
                string config = ConfigHelper.GetConfig(SettingGroup, "CheckUserOrderNo");
                if (string.IsNullOrEmpty(config))
                {
                    return false;
                }
                return (config == "1");
            }
        }

        public static string ConnectString
        {
            get
            {
                string tmp = ConfigHelper.GetConfig(SettingGroup, "ConnectString");
                try
                {
                    //return KuaiCardLib.Security.Cryptography.RijndaelDecrypt(tmp);
                    return tmp;
                }
                catch
                {
                    return "";
                }
            }
        }

        public static string CSSDomain
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "CSSDomain");
            }
        }

        public static double DeductSafetyTime
        {
            get
            {
                try
                {
                    return double.Parse(ConfigHelper.GetConfig(SettingGroup, "DeductSafetyTime"));
                }
                catch
                {
                    return 0.0;
                }
            }
        }

        public static string Domain
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "domain");
            }
        }

        public static string Firstpage
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "firstpage");
            }
        }

        public static string HostName
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "HostName");
            }
        }

        public static string HostPort
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "HostPort");
            }
        }

        public static string JXTURL
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "JXTURL");
            }
        }

        public static string ManagePagePath
        {
            get
            {
                string config = ConfigHelper.GetConfig(SettingGroup, "ManagePagePath");
                if (config == string.Empty)
                {
                    return "Console";
                }
                return config;
            }
        }

        public static int MaxDayToCashTimes
        {
            get
            {
                try
                {
                    return int.Parse(ConfigHelper.GetConfig(SettingGroup, "MaxDayToCashTimes"));
                }
                catch
                {
                    return 1;
                }
            }
        }

        public static string OrderCardStrategyAssembly
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "OrderCardStrategyAssembly");
            }
        }

        public static string OrderCardStrategyClass
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "OrderCardStrategyClass");
            }
        }

        public static string OrdersDAL
        {
            get
            {
                //KuaiCard.SQLServerDAL
                return ConfigHelper.GetConfig(SettingGroup, "OrdersDAL");
            }
        }

        public static string OrderSmsStrategyAssembly
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "OrderSmsStrategyAssembly");
            }
        }

        public static string OrderSmsStrategyClass
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "OrderSmsStrategyClass");
            }
        }

        public static string OrderStrategyAssembly
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "OrderStrategyAssembly");
            }
        }

        public static string OrderStrategyClass
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "OrderStrategyClass");
            }
        }

        public static string Paycompletpage
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "Paycompletpage");
            }
        }

        public static string Paydomain
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "Paydomain");
            }
        }

        public static int ServerId
        {
            get
            {
                try
                {
                    return int.Parse(ConfigHelper.GetConfig(SettingGroup, "ServerId"));
                }
                catch
                {
                    return 0;
                }
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

        public static string SMPWD
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "SMPWD");
            }
        }

        public static string SMSCOM
        {
            get
            {
                return "COM/";
            }
        }

        public static string SMSKEY
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "SMSKEY");
            }
        }

        public static string SMSKEYS
        {
            get
            {
                return "DESHUNSOFT.";
            }
        }

        public static string SMSMAI
        {
            get
            {
                return "WWW.";
            }
        }

        public static string SMSPassword
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "SMSPassword");
            }
        }

        public static string SMSSN
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "SMSSN");
            }
        }

        public static string SMSUser
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "SMSUser");
            }
        }

        public static string SMUID
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "SMUID");
            }
        }

        public static string SqlDataUser
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "SqlDataUser");
            }
        }

        public static string SystemName
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "SystemName");
            }
        }

        public static string UrlManagerConfigPath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Configurations\urlmanagerconfiguration.config");
            }
        }

        public static string[] UserAgent
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "UserAgent").Split(new char[] { '|' });
            }
        }

        public static string WebDAL
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "WebDAL");
            }
        }

        public static string WebSiteKeywords
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "WebSiteKeywords");
            }
        }

        public static int Xiaoka_time_interval
        {
            get
            {
                try
                {
                    return int.Parse(ConfigHelper.GetConfig(SettingGroup, "xiaoka_time_interval"));
                }
                catch
                {
                    return 1;
                }
            }
        }

        public static string YesSite
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "YesSite");
            }
        }


        //=================2017.1.22记录=============
        /// <summary>
        /// 是否记录通知地址。调试使用
        /// </summary>
        public static bool RecordReturnUrl
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "RecordReturnUrl") == "1";
            }
        }

        /// <summary>
        /// 使用单独程序通知客户。
        /// </summary>
        public static bool SingleNoticeUser
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "SingleNoticeUser") == "1";
            }
        }

        /// <summary>
        /// 网站用户名称
        /// </summary>
        public static string SiteUser
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "SiteUser").ToLower();
            }
        }
    }
}

