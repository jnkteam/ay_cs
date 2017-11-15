namespace Aywl.DBAccess
{
    using OriginalStudio.Lib.SysConfig;
    using OriginalStudio.Lib.Security;
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
                return Cryptography.RijndaelDecrypt(RuntimeSetting.ConnectString);
            }
        }
    }
}

