namespace OriginalStudio.WebComponents.Web
{
    using OriginalStudio.BLL;
    using OriginalStudio.Model;
    using OriginalStudio.Lib.SysConfig;
    using OriginalStudio.Lib.Security;
    using System;
    using System.Web;
    using System.Web.UI;

    public class PageBase : Page
    {
        private WebInfo _webinfo = null;

        public void AlertAndRedirect(string msg)
        {
            this.AlertAndRedirect(this, msg, null);
        }

        public void AlertAndRedirect(string msg, string url)
        {
            this.AlertAndRedirect(this, msg, url);
        }

        public void AlertAndRedirect(Page P, string msg)
        {
            this.AlertAndRedirect(P, msg, null);
        }

        public void AlertAndRedirect(Page P, string msg, string url)
        {
            string script = "";
            if (string.IsNullOrEmpty(msg))
            {
                if (string.IsNullOrEmpty(url))
                    script = "<SCRIPT LANGUAGE='javascript'><!--\r\nlocation.href=location.href;\r\n//--></SCRIPT>";
                else
                    script = string.Format("<SCRIPT LANGUAGE='javascript'><!--\r\nlocation.href=\"{0}\";\r\n//--></SCRIPT>", url);

                //string.Format("\r\n<SCRIPT LANGUAGE='javascript'><!--\r\nalert({0});\r\nlocation.href=\"{1}\";\r\n//--></SCRIPT>\r\n", AntiXss.JavaScriptEncode(msg), url) : (((msg != null) && (msg.Length != 0)) ? string.Format("\r\n<SCRIPT LANGUAGE='javascript'><!--\r\nalert({0});\r\nlocation.href=location.href;\r\n//--></SCRIPT>\r\n", AntiXss.JavaScriptEncode(msg)) : string.Format("\r\n<SCRIPT LANGUAGE='javascript'><!--\r\nlocation.href=\"{0}\";\r\n//--></SCRIPT>\r\n", url))) : "\r\n<SCRIPT LANGUAGE='javascript'><!--\r\nlocation.href=location.href;\r\n//--></SCRIPT>\r\n";
            }
            else
            {
                if (string.IsNullOrEmpty(url))
                    script = string.Format("\r\n<SCRIPT LANGUAGE='javascript'><!--\r\nalert({0});\r\nlocation.href=location.href;\r\n//--></SCRIPT>\r\n", AntiXss.JavaScriptEncode(msg));
                else
                    script = string.Format("\r\n<SCRIPT LANGUAGE='javascript'><!--\r\nalert({0});\r\nlocation.href=\"{1}\";\r\n//--></SCRIPT>\r\n", AntiXss.JavaScriptEncode(msg), url);

            }
            //script = (((!string.IsNullOrEmpty(msg)) && (msg.Length != 0)) ||
            //       ((!string.IsNullOrEmpty(url)) && (url.Length != 0))) ? (((!string.IsNullOrEmpty(url)) && (url.Length != 0)) ? string.Format("\r\n<SCRIPT LANGUAGE='javascript'><!--\r\nalert({0});\r\nlocation.href=\"{1}\";\r\n//--></SCRIPT>\r\n", AntiXss.JavaScriptEncode(msg), url) : (((msg != null) && (msg.Length != 0)) ? string.Format("\r\n<SCRIPT LANGUAGE='javascript'><!--\r\nalert({0});\r\nlocation.href=location.href;\r\n//--></SCRIPT>\r\n", AntiXss.JavaScriptEncode(msg)) : string.Format("\r\n<SCRIPT LANGUAGE='javascript'><!--\r\nlocation.href=\"{0}\";\r\n//--></SCRIPT>\r\n", url))) : "\r\n<SCRIPT LANGUAGE='javascript'><!--\r\nlocation.href=location.href;\r\n//--></SCRIPT>\r\n";

            P.ClientScript.RegisterStartupScript(P.GetType(), "AlertAndRedirect", script);
        }

        public string CutWord(object _str)
        {
            if (_str == null)
            {
                return string.Empty;
            }
            return this.CutWord(HttpUtility.HtmlEncode(_str.ToString()), 30);
        }

        public string CutWord(string _str)
        {
            return this.CutWord(_str, 30);
        }

        public string CutWord(string _str, int len)
        {
            if (string.IsNullOrEmpty(_str))
            {
                return string.Empty;
            }
            if (_str.Length > len)
            {
                return (_str.Substring(0, len) + "...");
            }
            return _str;
        }

        public string getTitle(string subPageTitle)
        {
            return string.Format("{1} {0}-{2}", this.SiteName, subPageTitle, SysConfig.WebSiteTitleSuffix);
        }

        public string Description
        {
            get
            {
                return ("\"" + SysConfig.WebSitedescription + "\"");
            }
        }

        public DateTime FirstDayOfMonth
        {
            get
            {
                return Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-01 00:00:00"));
            }
        }

        public string firstPage
        {
            get
            {
                string firstpage = RuntimeSetting.firstpage;
                if (string.IsNullOrEmpty(firstpage))
                {
                    firstpage = "index.aspx";
                }
                return firstpage;
            }
        }

        public string KeyWords
        {
            get
            {
                return ("\"" + SysConfig.WebSiteKey + "\"");
            }
        }

        public string SiteName
        {
            get
            {
                if (this.webInfo == null)
                {
                    return string.Empty;
                }
                return this.webInfo.Name;
            }
        }

        public string statJs
        {
            get
            {
                if (this.webInfo != null)
                {
                    return HttpUtility.HtmlDecode(this.webInfo.Code);
                }
                return string.Empty;
            }
        }

        public DateTime ToDayFirstTime
        {
            get
            {
                return Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
            }
        }

        public WebInfo webInfo
        {
            get
            {
                if (this._webinfo == null)
                {
                    this._webinfo = WebInfoFactory.CurrentWebInfo;
                }
                return this._webinfo;
            }
        }

        public string WebSiteTitleSuffix
        {
            get
            {
                return SysConfig.WebSiteTitleSuffix;
            }
        }
    }
}

