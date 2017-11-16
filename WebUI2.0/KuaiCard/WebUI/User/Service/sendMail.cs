namespace OriginalStudio.WebUI.User.Service
{
    using OriginalStudio.BLL.User;
    using OriginalStudio.Cache;
    using OriginalStudio.Model.User;
    using OriginalStudio.WebComponents;
    using OriginalStudio.WebComponents.Template;
    using System;
    using System.Text;
    using System.Web;
    using System.Web.SessionState;

    public class sendMail : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string s = "失败";
            string str2 = "false";

            if (this.currentUser == null)
            {
                //未登录
                s = "未登录!";
                goto Label_Exit;
            }

            string str3 = context.Request["template"];
            if (this.currentUser.IsEmailPass == 0 ||
                    OriginalStudio.Lib.SysConfig.RuntimeSetting.SiteUser.ToLower() == "zft")
            {
                string emailCheckTemp = Helper.GetEmailCheckTemp();
                string to = str3;
                string objId = "PHONE_VALID_" + to;
                string o = (string)WebCache.GetCacheService().RetrieveObject(objId);
                if (o == null)
                {
                    o = new Random().Next(0x2710, 0x1869f).ToString();
                    WebCache.GetCacheService().AddObject(objId, o);
                }
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat("<p>亲爱的{0}:<p>", UserFactory.CurrentMember.UserName);
                builder.AppendFormat("<p style=\"font-size:14px\">您的验证码为：<font style=\"font-size:14px;font-weight:bold;color:blue\">{0}</font>，打死也不能告诉别人！</p>", o);
                EmailHelper helper = new EmailHelper(string.Empty, to, to + "验证码", builder.ToString(), true, Encoding.GetEncoding("gbk"));
                if (helper.Send())
                {
                    s = "邮件效验码已发送，请注意查收！";
                    str2 = "true";
                }
                else
                {
                    s = "邮件发送失败，请联系管理员";
                    str2 = "false";
                }
            }
            else
            {
                s = "当前用户已经认证！";
                str2 = "false";
            }

        Label_Exit:
            s = "{\"result\":" + str2 + ",\"text\":\"" + s + "\"}";
            context.Response.ContentType = "application/json";
            context.Response.Write(s);
        }

        public UserInfo currentUser
        {
            get
            {
                return UserFactory.CurrentMember;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

