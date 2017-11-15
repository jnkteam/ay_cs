namespace KuaiCard.SysConfig
{
    using KuaiCardLib.Configuration;
    using System;

    public class CardFaceValue
    {
        private static readonly string _group = "cardfacevalue";

        public static string S0001
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "0001");
            }
        }

        public static string S0001FJ
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "0001FJ");
            }
        }

        public static string S0001GD
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "0001ZJ");
            }
        }

        public static string S0001LN
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "0001LN");
            }
        }

        public static string S0001ZJ
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "0001ZJ");
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

