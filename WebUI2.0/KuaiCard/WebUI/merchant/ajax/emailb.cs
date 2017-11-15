namespace KuaiCard.WebUI.merchant.ajax
{
    using KuaiCard.BLL;
    using KuaiCard.BLL.User;
    using KuaiCard.Model;
    using KuaiCard.Model.User;
    using KuaiCard.WebComponents;
    using KuaiCard.WebComponents.Template;
    using KuaiCardLib.Security;
    using KuaiCardLib.Text;
    using KuaiCardLib.Web;
    using System;
    using System.Text;
    using System.Web;
    using System.Web.SessionState;

    public class emailb : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        private WebInfo _webinfo = null;

        public string GetVerifyUrl(string parms)
        {
            return (this.webInfo.Domain + "/merchant/ajax/mailcheckreceive.ashx?parms=" + parms);
        }

        public void ProcessRequest(HttpContext context)
        {
            string s = "";
            if (UserFactory.CurrentMember == null)
            {
                s = "登录信息失效，请重新登录";
            }
            else
            {
                string emailCheckTemp = Helper.GetEmailCheckTemp();
                if (!string.IsNullOrEmpty(emailCheckTemp))
                {
                    string queryStringString = WebBase.GetQueryStringString("email", string.Empty);
                    string inputData = WebBase.GetQueryStringString("newemail", string.Empty);
                    if (queryStringString != UserFactory.CurrentMember.Email)
                    {
                        s = "当前邮件账号输入不正确 请重新输入";
                    }
                    else if (!PageValidate.IsEmail(inputData))
                    {
                        s = "新邮箱格式不正确;";
                    }
                    else
                    {
                        EmailCheckInfo model = new EmailCheckInfo();
                        model.userid = UserFactory.CurrentMember.ID;
                        model.status = EmailCheckStatus.提交中;
                        model.addtime = new DateTime?(DateTime.Now);
                        model.checktime = new DateTime?(DateTime.Now);
                        model.email = inputData;
                        model.typeid = EmailCheckType.认证;
                        model.Expired = DateTime.Now.AddDays(7.0);
                        int num = new EmailCheck().Add(model);
                        if (num > 0)
                        {
                            string parms = HttpUtility.UrlEncode(Cryptography.RijndaelEncrypt(string.Format("id={0}&", num)));
                            string verifyUrl = this.GetVerifyUrl(parms);
                            emailCheckTemp = emailCheckTemp.Replace("{#username#}", UserFactory.CurrentMember.UserName).Replace("{#useremail#}", queryStringString).Replace("{#sitename#}", this.SiteName).Replace("{#sitedomain#}", this.webInfo.Domain).Replace("{#verify_email#}", verifyUrl);
                            EmailHelper helper = new EmailHelper(string.Empty, queryStringString, queryStringString + "邮件验证", emailCheckTemp, true, Encoding.GetEncoding("gbk"));
                            if (helper.Send())
                            {
                                s = "true";
                            }
                            else
                            {
                                s = "邮件发送失败，请联系管理员";
                            }
                        }
                    }
                }
                context.Response.ContentType = "text/plain";
                context.Response.Write(s);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
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

