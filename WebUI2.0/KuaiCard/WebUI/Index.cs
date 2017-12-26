﻿namespace OriginalStudio.WebUI
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Settled;
    using OriginalStudio.Model.User;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Index : Page
    {
        protected Button Button1;
        protected HtmlInputText j_captcha_response;
        protected HtmlInputPassword j_password;
        protected HtmlInputText j_username;
        protected HtmlGenericControl messageBox;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.IsPostBack)
            {
                this.SignIn();
            }
        }

        private void ShowErrorInfo(string error)
        {
            this.messageBox.Style.Add("display", "block");
            this.messageBox.InnerHtml = error;
        }

        private void SignIn()
        {
            string str = this.j_username.Value.Trim();
            string str2 = this.j_password.Value.Trim();
            string str3 = this.j_captcha_response.Value.Trim();
            if (string.IsNullOrEmpty(str3))
            {
                OriginalStudio.WebUI.WebUtility.ShowMessage(this, "请输入验证码!");
            }
            else if (string.IsNullOrEmpty(str))
            {
                OriginalStudio.WebUI.WebUtility.ShowMessage(this, "请输入商户名!");
            }
            else if (string.IsNullOrEmpty(str2))
            {
                OriginalStudio.WebUI.WebUtility.ShowMessage(this, "请输入商户密码!");
            }
            else if (this.Session["CCode"] == null)
            {
                OriginalStudio.WebUI.WebUtility.ShowMessage(this, "验证码失效!请刷新页面");
            }
            else if (this.Session["CCode"].ToString() != str3.ToUpper())
            {
                OriginalStudio.WebUI.WebUtility.ShowMessage(this, "验证码不正确");
            }
            else
            {
                UserInfo userinfo = new UserInfo();
                userinfo.UserName = str;
                userinfo.Password = str2;
                userinfo.LastLoginIp = ServerVariables.TrueIP;
                userinfo.LastLoginTime = DateTime.Now;
                userinfo.LastLoginAddress = WebUtility.GetIPAddress(userinfo.LastLoginIp);
                userinfo.LastLoginRemark = WebUtility.GetIPAddressInfo(userinfo.LastLoginIp);
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
                    OriginalStudio.WebUI.WebUtility.ShowMessage(this, error);
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

