namespace KuaiCard.WebUI.User.Service
{
    using KuaiCard.BLL.User;
    using KuaiCard.Cache;
    using KuaiCard.WebComponents;
    using System;
    using System.Text;
    using System.Web;
    using System.Web.SessionState;

    public class sendtixianemail : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string str = "失败";
            string str2 = "false";
            bool flag = UserFactory.CurrentMember.IsEmailPass == 1;
            string aPIKey = UserFactory.CurrentMember.APIKey;
            string objId = "PHONE_VALID_" + aPIKey;
            string o = (string) WebCache.GetCacheService().RetrieveObject(objId);
            if (o == null)
            {
                o = new Random().Next(0x2710, 0x1869f).ToString();
                WebCache.GetCacheService().AddObject(objId, o);
            }
            if (flag)
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat("<p>亲爱的{0}:<p>", UserFactory.CurrentMember.UserName);
                builder.AppendFormat("<p style=\"font-size:14px\">您的验证码为：<font style=\"font-size:14px;font-weight:bold;color:blue\">{0}</font>，打死也不能告诉别人！</p>", o);
                EmailHelper helper = new EmailHelper(string.Empty, UserFactory.CurrentMember.Email, UserFactory.CurrentMember.Email + "验证码", builder.ToString(), true, Encoding.GetEncoding("gbk"));
                if (helper.Send())
                {
                    str = "邮件效验码已发送，请注意查收！";
                    str2 = "true";
                }
                else
                {
                    str = "邮件发送失败，请联系管理员";
                    str2 = "false";
                }
            }
            else
            {
                str = "当前邮箱没有认证！";
                str2 = "false";
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write("{\"result\":" + str2 + ",\"text\":\"" + str + "\"}");
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

