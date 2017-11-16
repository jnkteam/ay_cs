namespace OriginalStudio.Lib.SysConfig
{
    using OriginalStudio.Lib.Configuration;
    using System;
    using System.IO;

    /// <summary>
    /// 运行配置
    /// </summary>
    public sealed class RuntimeSetting
    {
        private static readonly string _group = "runtimeSettings";

        private RuntimeSetting()
        {
        }

        public static string SettingGroup
        {
            get
            {
                return _group;
            }
        }

        /// <summary>
        /// 获取运行配置键值。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static string GetKeyValue(string key, string defaultVal = "")
        {
            string tmp = ConfigHelper.GetConfig(OriginalStudio.Lib.Configuration.ConfigPath.RuntimeConfigPath, _group, key);

            return string.IsNullOrEmpty(tmp) ? defaultVal : tmp;
        }

        #region 固定的配置

        public static string Domain
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "domain");
            }
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string ConnectString
        {
            get
            {
                return OriginalStudio.Lib.Security.Cryptography.RijndaelDecrypt(GetKeyValue("ConnectString", "")).Trim();
            }
        }

        //=========OrderBank DAL==================
        public static string OrderBankDALStrategyAssembly
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "OrderBankDALStrategyAssembly");
            }
        }

        public static string OrderBankDALStrategyClass
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "OrderBankDALStrategyClass");
            }
        }
        //=========OrderBank DAL==================

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

        public static string SiteDomain
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "Sitedomain");
            }
        }

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
        
        public static string firstpage
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "firstpage");
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

        #endregion

        #region 短信配置

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

        #endregion

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


/*
 
        public static bool CheckUrlReferrer
        {
            get
            {
                string config = ConfigHelper.GetConfig(SettingGroup, "CheckUrlReferrer");
                return (string.IsNullOrEmpty(config) || (config == "1"));
            }
        }


        public static string ConnectString
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "ConnectString");
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

        public static int xiaoka_time_interval
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
        /// 使用单独程序通知客户。
        /// </summary>
        public static bool SingleNoticeUser
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "SingleNoticeUser") == "1";
            }
        }


 */

