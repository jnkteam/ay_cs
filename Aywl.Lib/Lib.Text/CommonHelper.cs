namespace OriginalStudio.Lib
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;

    public class CommonHelper
    {
        public static string[] BubbleSort(string[] r)
        {
            for (int i = 0; i < r.Length; i++)
            {
                bool flag = false;
                for (int j = r.Length - 2; j >= i; j--)
                {
                    if (string.CompareOrdinal(r[j + 1], r[j]) < 0)
                    {
                        string str = r[j + 1];
                        r[j + 1] = r[j];
                        r[j] = str;
                        flag = true;
                    }
                }
                if (!flag)
                {
                    return r;
                }
            }
            return r;
        }

        public static string BuildParamString(string[] s)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                if (i == (s.Length - 1))
                {
                    builder.Append(s[i]);
                }
                else
                {
                    builder.Append(s[i] + "&");
                }
            }
            return builder.ToString();
        }

        public static string BuildRequest(SortedDictionary<string, string> sParaTemp, string strMethod, string strButtonValue, string GATEWAY_NEW)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<form id='ebatongsubmit' name='alipaysubmit' action='" + GATEWAY_NEW + "' method='" + strMethod.ToLower().Trim() + "'>");
            foreach (KeyValuePair<string, string> pair in sParaTemp)
            {
                builder.Append("<input type='hidden' name='" + pair.Key + "' value='" + pair.Value + "'/>");
            }
            builder.Append("<input type='submit' value='" + strButtonValue + "' style='display:none;'></form>");
            builder.Append("<script>document.forms['ebatongsubmit'].submit();</script>");
            return builder.ToString();
        }

        public static string md5(string input_charset, string plainText)
        {
            MD5 md = new MD5CryptoServiceProvider();
            return ToHexStr(md.ComputeHash(Encoding.GetEncoding(input_charset).GetBytes(plainText)));
        }

        public static byte[] ToByteArray(string HexString)
        {
            int length = HexString.Length;
            byte[] buffer = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
            {
                buffer[i / 2] = Convert.ToByte(HexString.Substring(i, 2), 0x10);
            }
            return buffer;
        }

        public static string ToHexStr(byte[] bytes)
        {
            if (bytes == null)
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("X2"));
            }
            return builder.ToString();
        }
    }
}

