namespace OKXR.Common
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    public class SMS
    {
        public static string Get_Http(string a_strUrl, int timeout)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(a_strUrl);
                request.Timeout = timeout;
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                StringBuilder builder = new StringBuilder();
                while (-1 != reader.Peek())
                {
                    builder.Append(reader.ReadLine());
                }
                return builder.ToString();
            }
            catch (Exception exception)
            {
                return ("错误：" + exception.Message);
            }
        }

        public static bool MobileNumValidate(string input)
        {
            string pattern = @"(86)*0*1\d{10}";
            return Regex.IsMatch(input, pattern);
        }

        public static bool SendSMS(string mobile, string content, string ext)
        {
            string str = ConfigurationManager.AppSettings["SMSSN"];
            string str2 = ConfigurationManager.AppSettings["SMSPWD"];
            string str3 = HttpUtility.UrlEncode(content, Encoding.GetEncoding("gb2312"));
            return (Get_Http(string.Format("http://sdk2.entinfo.cn/z_send.aspx?sn={0}&pwd={1}&mobile={2}&content={3}&ext={4}", new object[] { str, str2, mobile, str3, ext }), 0x1388) == "1");
        }
    }
}

