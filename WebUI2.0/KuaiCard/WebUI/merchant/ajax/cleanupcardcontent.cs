namespace OriginalStudio.WebUI.Merchant.ajax
{
    using System;
    using System.Text.RegularExpressions;
    using System.Web;

    public class cleanupcardcontent : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string str;
            context.Response.ContentType = "application/json";
            string str2 = context.Request.Form["cardcontent"];
            str2 = new Regex(@"\d{1,2}:\d{1,2}:\d{1,2}").Replace(str2.ToString(), "");
            if ((((((str2.Contains("S") || str2.Contains("CSC")) || (str2.Contains("CS") || str2.Contains("CA"))) || ((str2.Contains("CSB") || str2.Contains("YA")) || (str2.Contains("YB") || str2.Contains("YC")))) || (((str2.Contains("YD") || str2.Contains("s")) || (str2.Contains("csc") || str2.Contains("cs"))) || ((str2.Contains("ca") || str2.Contains("csb")) || (str2.Contains("ya") || str2.Contains("yb"))))) || str2.Contains("yc")) || str2.Contains("yd"))
            {
                str = "：/`，、。.~\x00b7…！!|,@#$%^&*()+_=]\"[￥-\\【】[]{}'“；;:？?><》《:'EFGHIJKLMNOPQRTUVWXZefghijklmnopqrstuvwxz";
                foreach (char ch in str)
                {
                    str2 = str2.Replace(ch.ToString(), " ");
                }
            }
            else
            {
                str = "：/`，、。.~\x00b7…！!|,@#$%^&*()+_=]\"[￥-\\【】[]{}'“；;:？?><》《:'ABCDEFGHIJKLMNOPQRTUYVWXZabcdefghijklmnopqrstuyvwxz";
                foreach (char ch in str)
                {
                    str2 = str2.Replace(ch.ToString(), " ");
                }
            }
            string[] strArray = str2.Split(new string[] { " ", "\t", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            string str3 = "";
            for (int i = 0; i < strArray.Length; i++)
            {
                if (!((strArray[i].Contains("-") || (strArray[i].Length < 8)) || strArray[i].Contains("000000")))
                {
                    str3 = str3 + strArray[i++] + " " + strArray[i] + @"\r\n";
                }
            }
            string s = "{\"result\":\"ok\",\"rescode\":\"1\",\"msg\":\"" + str3.Trim() + "\"}";
            context.Response.Write(s);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

