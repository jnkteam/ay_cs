namespace KuaiCard.WebUI.Business
{
    using OriginalStudio.BLL;
    using OriginalStudio.Model;
    using OriginalStudio.WebComponents.Web;
    using KuaiCard.WebUI;
    using OriginalStudio.Lib;
    using OriginalStudio.Lib.Security;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Web.UI.HtmlControls;

    public class Login1 : PageBase
    {
        protected HtmlForm form1;
        protected HtmlHead Head1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (XRequest.IsPost())
            {
                if (this.Session["CCode"] == null)
                {
                    base.AlertAndRedirect("验证码已失效!");
                }
                else if (XRequest.GetString("CCode").ToUpper() != this.Session["CCode"].ToString().ToUpper())
                {
                    base.AlertAndRedirect("验证码错误!");
                }
                else
                {
                    string str = XRequest.GetString("UserNameBox");
                    string str2 = Cryptography.MD5(XRequest.GetString("pas"));
                    Manage manage = new Manage();
                    manage.username = str;
                    manage.password = str2;
                    manage.lastLoginTime = new DateTime?(DateTime.Now);
                    manage.lastLoginIp = ServerVariables.TrueIP;
                    manage.LastLoginAddress = WebUtility.GetIPAddress(manage.lastLoginIp);
                    manage.LastLoginRemark = WebUtility.GetIPAddressInfo(manage.lastLoginIp);
                    string msg = ManageFactory.SignIn(manage);
                    if (manage.id > 0)
                    {
                        base.AlertAndRedirect(string.Empty, "Default.aspx");
                    }
                    else
                    {
                        base.AlertAndRedirect(msg);
                    }
                }
            }
        }
    }
}

