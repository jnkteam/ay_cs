namespace THPayHelper
{
    using System;
    using System.Text;
    using System.Web;

    public class URLUtils
    {
        public static void appendParam(StringBuilder sb, string name, string val)
        {
            appendParam(sb, name, val, true);
        }

        public static void appendParam(StringBuilder sb, string name, string val, bool and)
        {
            appendParam(sb, name, val, and, null);
        }

        public static void appendParam(StringBuilder sb, string name, string val, string charset)
        {
            appendParam(sb, name, val, true, charset);
        }

        public static void appendParam(StringBuilder sb, string name, string val, bool and, string charset)
        {
            if (and)
            {
                sb.Append("&");
            }
            else
            {
                sb.Append("?");
            }
            sb.Append(name);
            sb.Append("=");
            if (val == null)
            {
                val = "";
            }
            if (string.IsNullOrEmpty(charset))
            {
                sb.Append(val);
            }
            else
            {
                sb.Append(encode(val, charset));
            }
        }

        public static string decode(string str, string charset)
        {
            try
            {
                return HttpUtility.UrlDecode(str, Encoding.GetEncoding(charset));
            }
            catch (Exception)
            {
                return str;
            }
        }

        public static string encode(string str, string charset)
        {
            try
            {
                return HttpUtility.UrlEncode(str, Encoding.GetEncoding(charset));
            }
            catch (Exception)
            {
                return str;
            }
        }
    }
}

