namespace com.todaynic.ScpClient
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    internal class Utility
    {
        private static System.Text.Encoding m_Encoding = System.Text.Encoding.Default;

        public static string getBase64ToString(string data)
        {
            byte[] bytes = Convert.FromBase64String(data);
            return System.Text.Encoding.GetEncoding("gbk").GetString(bytes);
        }

        public static string getFileTime()
        {
            return DateTime.Now.ToFileTime().ToString();
        }

        public static string getMd5Hash(string data)
        {
            byte[] bytes = System.Text.Encoding.Default.GetBytes(data);
            byte[] buffer2 = new MD5CryptoServiceProvider().ComputeHash(bytes);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < buffer2.Length; i++)
            {
                builder.Append(buffer2[i].ToString("x2"));
            }
            return builder.ToString().ToLower();
        }

        public static string getStringToBase64(string data)
        {
            return Convert.ToBase64String(System.Text.Encoding.GetEncoding("gbk").GetBytes(data));
        }

        public static System.Text.Encoding Encoding
        {
            get
            {
                return m_Encoding;
            }
            set
            {
                m_Encoding = value;
            }
        }
    }
}

