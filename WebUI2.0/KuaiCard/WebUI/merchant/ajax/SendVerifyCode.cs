namespace KuaiCard.WebUI.merchant.ajax
{
    using KuaiCard.BLL;
    using KuaiCard.BLL.Tools;
    using KuaiCard.BLL.User;
    using KuaiCard.Cache;
    using KuaiCard.Model.User;
    using System;
    using System.Web;
    using System.Web.SessionState;

    public class SendVerifyCode : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string s = string.Empty;
            if (this.currentUser == null)
            {
                s = "登录信息失效，请重新登录";
            }
            else
            {
                string tel = UserFactory.CurrentMember.Tel;
                string objId = "PHONE_VALID_" + tel;
                string o = new Random().Next(0x2710, 0x1869f).ToString();
                WebCache.GetCacheService().AddObject(objId, o);
                string msg = SysConfig.sms_temp_Authenticate.Replace("{@username}", UserFactory.CurrentMember.UserName).Replace("{@sitename}", WebInfoFactory.CurrentWebInfo.Name).Replace("{@authcode}", o);
                string str6 = SMS.SendSmsWithCheck(tel, msg, "");
                if (string.IsNullOrEmpty(str6))
                {
                    s = "true";
                }
                else
                {
                    s = str6;
                }
            }
            context.Response.ContentType = "text/plain";
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

