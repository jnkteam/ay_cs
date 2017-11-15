namespace OriginalStudio.Lib.SysConfig
{
    using OriginalStudio.Lib.Configuration;
    using System;
    using System.Xml;

    /// <summary>
    /// 支付配置。
    /// </summary>
    public sealed class PaymentSetting
    {
        private static readonly string _group = "paymentSettings";

        public static string SettingGroup
        {
            get
            {
                return _group;
            }
        }

        private PaymentSetting()
        {
        }

        /// <summary>
        /// 获取支付配置键值。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static string GetKeyValue(string key, string defaultVal = "")
        {
            string tmp = ConfigHelper.GetConfig(OriginalStudio.Lib.Configuration.ConfigPath.RuntimeConfigPath, _group, key);

            return string.IsNullOrEmpty(tmp) ? defaultVal : tmp;
        }

        #region 固定配置

        public static string alipay_body
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "alipay_body");
            }
        }

        public static string alipay_subject
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "alipay_subject");
            }
        }

        public static string weixin_body
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "weixin_body");
            }
        }

        public static string weixin_subject
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "weixin_subject");
            }
        }

        #endregion
    }

}

