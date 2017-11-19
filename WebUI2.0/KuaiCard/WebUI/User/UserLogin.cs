namespace OriginalStudio.WebUI.User
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model.User;
    using OriginalStudio.Lib.Configuration;
    using OriginalStudio.Lib.Web;
    using System;
   
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
   

    public class UserLogin : System.Web.UI.Page
	{
        protected Button btnLogin;
        protected HtmlInputPassword j_password;
        protected HtmlInputText j_username;
        protected HtmlInputText j_captcha_response;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.IsPostBack)
            {
                this.SignIn();
            }
        }

        private void SignIn()
        {
            string str = this.j_username.Value.Trim();
            string str2 = this.j_password.Value.Trim();
            string str3 = this.j_captcha_response.Value.Trim();
            if (string.IsNullOrEmpty(str3))
            {
                WebUtility.ShowMessage(this, "请输入验证码!");
            }
            else if (string.IsNullOrEmpty(str))
            {
                WebUtility.ShowMessage(this, "请输入商户名!");
            }
            else if (string.IsNullOrEmpty(str2))
            {
                WebUtility.ShowMessage(this, "请输入商户密码!");
            }
            else if (this.Session["CCode"] == null)
            {
                WebUtility.ShowMessage(this, "验证码失效!请刷新页面");
            }
            else if (this.Session["CCode"].ToString() != str3.ToUpper())
            {
                WebUtility.ShowMessage(this, "验证码不正确");
            }
            else
            {
                MchUserBaseInfo userinfo = new MchUserBaseInfo()
                {
                    UserName = str,
                    UserPayPwd = str2,
                    LastLoginIP = ServerVariables.TrueIP,
                    LastLoginTime = DateTime.Now
                };
                userinfo.LastLoginAddress = WebUtility.GetIPAddress(userinfo.LastLoginIP);
                userinfo.LastLoginRemark = WebUtility.GetIPAddressInfo(userinfo.LastLoginIP);
                userinfo.LastLoginMAC = "";// GetCustomerMac(ServerVariables.TrueIP);
                if (SysConfig.isUserloginByEmail == "1")
                {
                    userinfo.EMail = str;
                }
                string error = MchUserFactory.SignIn(userinfo);
                if ((userinfo.UserID > 0) && SysConfig.RegistrationActivationByEmail)
                {
                    //error = "您的账号尚未激活，请进入你注册时使用的邮箱激活账号。";
                    //error = "您的账号尚未激活，请联系商务激活账号。";
                }
                if (error != "登录成功")
                {
                    WebUtility.ShowMessage(this, error);
                }
                else
                {
                    base.Response.Redirect("/User/", false);
                }
            }
        }
	}
}
