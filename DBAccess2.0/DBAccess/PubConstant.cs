namespace DBAccess
{
    using KuaiCard.SysConfig;
    using KuaiCardLib.Security;
    using System;
    using System.Configuration;

    public class PubConstant
    {
        public static string GetConnectionString(string configName)
        {
            string strToDecrypt = ConfigurationManager.AppSettings[configName];
            if (ConfigurationManager.AppSettings["ConStringEncrypt"] == "true")
            {
                strToDecrypt = Cryptography.RijndaelDecrypt(strToDecrypt);
            }
            return strToDecrypt;
        }

        public static string ConnectionString
        {
            get
            {
                //return Cryptography.RijndaelDecrypt(RuntimeSetting.ConnectString);
                return RuntimeSetting.ConnectString;
            }
        }
    }
}

