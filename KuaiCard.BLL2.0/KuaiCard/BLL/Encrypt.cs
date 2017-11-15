namespace KuaiCard.BLL
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class Encrypt
    {
        public static string decodefortcp(string str)
        {
            string str2 = "";
            for (int i = 0; i < str.Length; i++)
            {
                int num2 = str[i];
                str2 = str2 + ((char) ~num2);
            }
            return str2;
        }

        public static string DecryptDES(string source, string key, string iv)
        {
            Encoding aSCII = Encoding.ASCII;
            byte[] buffer = Convert.FromBase64String(source);
            byte[] bytes = aSCII.GetBytes(key);
            byte[] rgbIV = aSCII.GetBytes(iv);
            MemoryStream stream = new MemoryStream(buffer);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Read);
            byte[] buffer4 = new byte[buffer.Length];
            stream2.Read(buffer4, 0, buffer4.Length);
            return aSCII.GetString(buffer4);
        }

        public static string DoDecrypt(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            byte[] buffer = new byte[pToDecrypt.Length / 2];
            for (int i = 0; i < (pToDecrypt.Length / 2); i++)
            {
                int num2 = Convert.ToInt32(pToDecrypt.Substring(i * 2, 2), 0x10);
                buffer[i] = (byte) num2;
            }
            provider.Key = Encoding.ASCII.GetBytes(sKey);
            provider.IV = Encoding.ASCII.GetBytes(sKey);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            StringBuilder builder = new StringBuilder();
            return Encoding.Default.GetString(stream.ToArray());
        }

        public static string encodefortcp(string str)
        {
            string str2 = "";
            for (int i = 0; i < str.Length; i++)
            {
                int num2 = str[i];
                str2 = str2 + ((char) ~num2);
            }
            return str2;
        }

        public static string EncrypotyDES(string source, string key, string iv)
        {
            Encoding aSCII = Encoding.ASCII;
            byte[] bytes = aSCII.GetBytes(source);
            byte[] rgbKey = aSCII.GetBytes(key);
            byte[] rgbIV = aSCII.GetBytes(iv);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            return Convert.ToBase64String(stream.ToArray());
        }

        public static string Getdekeystring(string txtkey)
        {
            return DoDecrypt(txtkey, "15684598");
        }

        public static bool PayTimeout()
        {
            return true;
        }
    }
}

