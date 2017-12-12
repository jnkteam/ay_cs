namespace OriginalStudio.WebUI.User.Service
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Tools;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Cache;
    using OriginalStudio.Model;
    using OriginalStudio.OKXR;
    using OriginalStudio.WebComponents;
    using OriginalStudio.WebComponents.Template;
    using System;
    using System.Text;
    using System.Web;
    using System.Web.SessionState;

    public class sendMobileCode : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        private WebInfo _webinfo = null;

        public void ProcessRequest(HttpContext context)
        {
            string str;
            string str2;
            StringBuilder builder;
            EmailHelper helper;
            string str4 = context.Request["template"];
            string to = context.Request["sendemail"];
            string s = "失败";
            string str7 = "false";
            string str10 = str4;
            if (str10 != null)
            {
                bool flag;
                string str3;
                if (!(str10 == "modifyShouji"))
                {
                    if (str10 == "mobilecode")
                    {
                        flag = UserFactory.CurrentMember.IsPhonePass == 1;
                        str = "PHONE_VALID_" + UserFactory.CurrentMember.Tel;
                        str2 = (string) WebCache.GetCacheService().RetrieveObject(str);
                        if (str2 == null)
                        {
                            str2 = new Random().Next(0x2710, 0x1869f).ToString();
                            WebCache.GetCacheService().AddObject(str, str2);
                        }
                        str3 = SysConfig.sms_temp_Authenticate;
                        if (flag)
                        {
                            str3 = SysConfig.sms_temp_Modify.Replace("{@username}", UserFactory.CurrentMember.UserName)
                                                                                    .Replace("{@sitename}", WebInfoFactory.CurrentWebInfo.Name)
                                                                                    .Replace("{@authcode}", str2);
                            if (string.IsNullOrEmpty(BLL.SMS.SMS.SendSmsWithCheck(UserFactory.CurrentMember.Tel, str3, "")))
                            {
                                s = "短信效验码已发送，请注意查收！";
                                str7 = "true";
                            }
                            else
                            {
                                s = "短信效验码发送失败！";
                                str7 = "false";
                            }
                        }
                        else
                        {
                            s = "当前手机号码没有认证！";
                            str7 = "false";
                        }
                    }
                    else if (str10 == "modifyEmail")
                    {
                        string emailCheckTemp = Helper.GetEmailCheckTemp();
                        string email = UserFactory.CurrentMember.Email;
                        str = "PHONE_VALID_" + email;
                        str2 = (string) WebCache.GetCacheService().RetrieveObject(str);
                        if (str2 == null)
                        {
                            str2 = new Random().Next(0x2710, 0x1869f).ToString();
                            WebCache.GetCacheService().AddObject(str, str2);
                        }
                        builder = new StringBuilder();
                        builder.AppendFormat("<p>亲爱的{0}:<p>", UserFactory.CurrentMember.UserName);
                        builder.AppendFormat("<p style=\"font-size:14px\">您的验证码为：<font style=\"font-size:14px;font-weight:bold;color:blue\">{0}</font>，打死也不能告诉别人！</p>", str2);
                        helper = new EmailHelper(string.Empty, email, email + "验证码", builder.ToString(), true, Encoding.GetEncoding("gbk"));
                        if (helper.Send())
                        {
                            s = "邮件效验码已发送，请注意查收！";
                            str7 = "true";
                        }
                        else
                        {
                            s = "邮件发送失败，请联系管理员";
                            str7 = "false";
                        }
                    }
                }
                else if (str10 == "userwithdraw")
                {
                    flag = UserFactory.CurrentMember.IsPhonePass == 1;
                    str = "PHONE_VALID_" + UserFactory.CurrentMember.Tel;
                    str2 = (string) WebCache.GetCacheService().RetrieveObject(str);
                    if (str2 == null)
                    {
                        str2 = new Random().Next(0x2710, 0x1869f).ToString();
                        WebCache.GetCacheService().AddObject(str, str2);
                    }
                    str3 = SysConfig.sms_temp_Authenticate;
                    if (flag)
                    {
                        str3 = SysConfig.sms_caiwu_tocash2.Replace("{@username}", UserFactory.CurrentMember.UserName)
                                                                                .Replace("{@sitename}", WebInfoFactory.CurrentWebInfo.Name)
                                                                                .Replace("{@authcode}", str2);
                        if (string.IsNullOrEmpty(BLL.SMS.SMS.SendSmsWithCheck(UserFactory.CurrentMember.Tel, str3, "")))
                        {
                            s = "短信效验码已发送，请注意查收！";
                            str7 = "true";
                        }
                        else
                        {
                            s = "短信效验码发送失败！";
                            str7 = "false";
                        }
                    }
                    else
                    {
                        s = "当前手机号码没有认证！";
                        str7 = "false";
                    }
                }
                else
                {
                    flag = UserFactory.CurrentMember.IsPhonePass == 1;
                    str = "PHONE_VALID_" + UserFactory.CurrentMember.Tel;
                    str2 = (string) WebCache.GetCacheService().RetrieveObject(str);
                    if (str2 == null)
                    {
                        str2 = new Random().Next(0x2710, 0x1869f).ToString();
                        WebCache.GetCacheService().AddObject(str, str2);
                    }
                    str3 = SysConfig.sms_temp_Authenticate;
                    if (flag)
                    {
                        str3 = SysConfig.sms_temp_Modify.Replace("{@username}", UserFactory.CurrentMember.UserName)
                                                                                .Replace("{@sitename}", WebInfoFactory.CurrentWebInfo.Name)
                                                                                .Replace("{@authcode}", str2);
                        if (string.IsNullOrEmpty(BLL.SMS.SMS.SendSmsWithCheck(UserFactory.CurrentMember.Tel, str3, "")))
                        {
                            s = "短信效验码已发送，请注意查收！";
                            str7 = "true";
                        }
                        else
                        {
                            s = "短信效验码发送失败！";
                            str7 = "false";
                        }
                    }
                    else
                    {
                        s = "当前手机号码没有认证！";
                        str7 = "false";
                    }
                }
            }
            if (to != null)
            {
                str = "PHONE_VALID_" + to;
                str2 = (string) WebCache.GetCacheService().RetrieveObject(str);
                if (str2 == null)
                {
                    str2 = new Random().Next(0x2710, 0x1869f).ToString();
                    WebCache.GetCacheService().AddObject(str, str2);
                }
                builder = new StringBuilder();
                builder.AppendFormat("<p>亲爱的{0}:<p>", UserFactory.CurrentMember.UserName);
                builder.AppendFormat("<p style=\"font-size:14px\">您的验证码为：<font style=\"font-size:14px;font-weight:bold;color:blue\">{0}</font>，打死也不能告诉别人！</p>", str2);
                helper = new EmailHelper(string.Empty, to, to + "验证码", builder.ToString(), true, Encoding.GetEncoding("gbk"));
                if (helper.Send())
                {
                    s = "邮件效验码已发送，请注意查收！";
                    str7 = "true";
                }
                else
                {
                    s = "邮件发送失败，请联系管理员";
                    str7 = "false";
                }
            }
            s = "{\"result\":" + str7 + ",\"text\":\"" + s + "\"}";
            context.Response.ContentType = "application/json";
            context.Response.Write(s);
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

