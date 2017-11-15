namespace KuaiCard.SysConfig
{
    using KuaiCardLib.Configuration;
    using System;

    public sealed class TransactionSetting
    {
        private static readonly string _group = "TransactionSettings";

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

        public static decimal MaxTranATMali
        {
            get
            {
                decimal result = 5000M;
                decimal.TryParse(ConfigHelper.GetConfig(SettingGroup, "maxtransactionamountali"), out result);
                return result;
            }
        }

        public static decimal MaxTranATMwx
        {
            get
            {
                decimal result = 5000M;
                decimal.TryParse(ConfigHelper.GetConfig(SettingGroup, "maxtransactionamountwx"), out result);
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

        public static string SettingGroup
        {
            get
            {
                return _group;
            }
        }
    }
}

