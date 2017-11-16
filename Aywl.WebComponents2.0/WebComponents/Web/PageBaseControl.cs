namespace OriginalStudio.WebComponents.Web
{
    using OriginalStudio.BLL;
    using OriginalStudio.Model;
    using OriginalStudio.Lib.Security;
    using System;
    using System.Web.UI;

    public class PageBaseControl : UserControl
    {
        private WebInfo _webinfo = null;

        public void AlertAndRedirect(string msg)
        {
            this.AlertAndRedirect(this.Page, msg, null);
        }

        public void AlertAndRedirect(string msg, string url)
        {
            this.AlertAndRedirect(this.Page, msg, url);
        }

        public void AlertAndRedirect(Page P, string msg, string url)
        {
            string script = (((msg != null) && (msg.Length != 0)) || ((url != null) && (url.Length != 0))) ? (((url != null) && (url.Length != 0)) ? string.Format("\r\n<SCRIPT LANGUAGE='javascript'><!--\r\nalert({0});\r\nlocation.href=\"{1}\";\r\n//--></SCRIPT>\r\n", AntiXss.JavaScriptEncode(msg), url) : (((msg != null) && (msg.Length != 0)) ? string.Format("\r\n<SCRIPT LANGUAGE='javascript'><!--\r\nalert({0});\r\nlocation.href=location.href;\r\n//--></SCRIPT>\r\n", AntiXss.JavaScriptEncode(msg)) : string.Format("\r\n<SCRIPT LANGUAGE='javascript'><!--\r\nlocation.href=\"{0}\";\r\n//--></SCRIPT>\r\n", url))) : "\r\n<SCRIPT LANGUAGE='javascript'><!--\r\nlocation.href=location.href;\r\n//--></SCRIPT>\r\n";
            P.ClientScript.RegisterClientScriptBlock(P.GetType(), "AlertAndRedirect", script);
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
    }
}

