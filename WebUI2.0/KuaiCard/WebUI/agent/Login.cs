namespace KuaiCard.WebUI.agent
{
    using KuaiCard.BLL.User;
    using KuaiCard.Model.User;
    using KuaiCard.WebComponents.Web;
    using KuaiCard.WebUI;
    using KuaiCardLib;
    using KuaiCardLib.Web;
    using System;
    using System.Web.UI.HtmlControls;

    public class Login : PageBase
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
                    string str2 = XRequest.GetString("pas");
                    UserInfo userinfo = new UserInfo();
                    userinfo.UserName = str;
                    userinfo.Password = str2;
                    userinfo.LastLoginIp = ServerVariables.TrueIP;
                    userinfo.LastLoginTime = DateTime.Now;
                    userinfo.LastLoginAddress = WebUtility.GetIPAddress(userinfo.LastLoginIp);
                    userinfo.LastLoginRemark = WebUtility.GetIPAddressInfo(userinfo.LastLoginIp);
                    string msg = UserFactory.SignIn(userinfo);
                    if (userinfo.ID > 0)
                    {
                        if (userinfo.UserType == UserTypeEnum.代理)
                        {
                            base.AlertAndRedirect(string.Empty, "Default.aspx");
                        }
                        else
                        {
                            base.AlertAndRedirect("非代理权限，无法登录！");
                        }
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

