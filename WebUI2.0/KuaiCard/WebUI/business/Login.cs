namespace OriginalStudio.WebUI.business
{
    using OriginalStudio.BLL;
    using OriginalStudio.Model;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.WebUI;
    using OriginalStudio.Lib;
    using OriginalStudio.Lib.Security;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Web.UI.HtmlControls;

    public class Login : PageBase
    {
        protected HtmlForm form1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (XRequest.IsPost())
            {
                string str = XRequest.GetString("txtUser");
                string str2 = Cryptography.MD5(XRequest.GetString("txtPwd"));
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

