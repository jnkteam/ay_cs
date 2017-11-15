namespace OriginalStudio.Lib.SysConfig
{
    using OriginalStudio.Lib.Configuration;
    using System;

    /// <summary>
    /// 交易额配置
    /// </summary>
    public sealed class TransactionSetting
    {
        private static readonly string _group = "TransactionSettings";

        public static string SettingGroup
        {
            get
            {
                return _group;
            }
        }

        public static int ExpiresTime
        {
            get
            {
                int result = 300;
                int.TryParse(ConfigHelper.GetConfig(SettingGroup, "orderCacheExpiresTime"), out result);
                return result;
            }
        }

        public static decimal MaxTranATM
        {
            get
            {
                decimal result = 5000M;
                decimal.TryParse(ConfigHelper.GetConfig(SettingGroup, "maxtransactionamount"), out result);
                return result;
            }
        }

        public static decimal MinTranATM
        {
            get
            {
                decimal result = 0.02M;
                decimal.TryParse(ConfigHelper.GetConfig(SettingGroup, "mintransactionamount"), out result);
                return result;
            }
        }

    }
}

