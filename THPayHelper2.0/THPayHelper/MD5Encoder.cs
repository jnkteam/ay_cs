namespace THPayHelper
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class MD5Encoder
    {
        public static string encode(string str, string charset)
        {
            byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.GetEncoding(charset).GetBytes(str));
            string str2 = "";
            for (int i = 0; i < buffer.Length; i++)
            {
                str2 = str2 + buffer[i].ToString("x2");
            }
            return str2;
        }
    }
}

