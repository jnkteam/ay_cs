﻿namespace KuaiCard.WebUI.User
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model.User;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using System.Text.RegularExpressions;

    public class UserLoginV2 : System.Web.UI.Page
	{
        protected Button btnLogin;
        protected HtmlInputText j_username;
        protected HtmlInputText j_captcha_response;
        protected HiddenField hidPwd;
        protected HiddenField hidDesc;

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
            string str2 = this.hidPwd.Value.ToString(); //this.j_password.Value.Trim();
            string str3 = this.j_captcha_response.Value.Trim();
            if (string.IsNullOrEmpty(str3))
            {
                KuaiCard.WebUI.WebUtility.ShowMessage(this, "请输入验证码!");
            }
            else if (string.IsNullOrEmpty(str))
            {
                KuaiCard.WebUI.WebUtility.ShowMessage(this, "请输入商户名!");
            }
            else if (string.IsNullOrEmpty(str2))
            {
                KuaiCard.WebUI.WebUtility.ShowMessage(this, "请输入商户密码!");
            }
            else if (this.Session["CCode"] == null)
            {
                KuaiCard.WebUI.WebUtility.ShowMessage(this, "验证码失效!请刷新页面");
            }
            else if (this.Session["CCode"].ToString() != str3.ToUpper())
            {
                KuaiCard.WebUI.WebUtility.ShowMessage(this, "验证码不正确");
            }
            else
            {
                //KuaiCardLib.Logging.LogHelper.Write("登录:" + str2 + ",MAC：" + this.hidDesc.Value.ToString());

                UserInfo userinfo = new UserInfo();
                userinfo.UserName = str;
                userinfo.Password = str2;
                userinfo.LastLoginIp = ServerVariables.TrueIP;
                userinfo.LastLoginTime = DateTime.Now;
                userinfo.LastLoginAddress = WebUtility.GetIPAddress(userinfo.LastLoginIp);
                userinfo.LastLoginRemark = WebUtility.GetIPAddressInfo(userinfo.LastLoginIp);
                userinfo.login_mac = this.hidDesc.Value.ToString();
                if (SysConfig.isUserloginByEmail == "1")
                {
                    userinfo.Email = str;
                }
                string error = UserFactory.SignIn(userinfo);
                if (((userinfo.ID > 0) && SysConfig.RegistrationActivationByEmail) && (userinfo.IsEmailPass == 0))
                {
                    error = "您的账号尚未激活，请进入你注册时使用的邮箱激活账号。";
                }
                if (error != "登录成功")
                {
                    KuaiCard.WebUI.WebUtility.ShowMessage(this, error);
                    //this.ShowErrorInfo(error);
                }
                else
                {
                    base.Response.Redirect("/User/", false);
                }
            }
        }
	}
}
