namespace OriginalStudio.WebUI.User.Service
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Tools;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Cache;
    using System;
    using System.Web;
    using System.Web.SessionState;
    using OriginalStudio.Model.User;

    public class sendtixianCode : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string str = "失败";
            string str2 = "false";
            if (this.currentUser == null)
            {
                str = "未登录！";
                str2 = "false";
            }
            else
            {
                bool flag = UserFactory.CurrentMember.IsPhonePass == 1;
                string objId = "PHONE_VALID_" + UserFactory.CurrentMember.APIKey;
                string o = (string)WebCache.GetCacheService().RetrieveObject(objId);
                if (o == null)
                {
                    o = new Random().Next(0x2710, 0x1869f).ToString();
                    WebCache.GetCacheService().AddObject(objId, o);
                }
                string msg = SysConfig.sms_temp_Authenticate;
                if (flag)
                {
                    msg = SysConfig.sms_temp_Modify
                        .Replace("{@username}", UserFactory.CurrentMember.UserName)
                        .Replace("{@sitename}", WebInfoFactory.CurrentWebInfo.Name)
                        .Replace("{@authcode}", o);
                    if (string.IsNullOrEmpty(SMS.SendSmsWithCheck(UserFactory.CurrentMember.Tel, msg, "")))
                    {
                        str = "短信效验码已发送，请注意查收！";
                        str2 = "true";
                    }
                    else
                    {
                        str = "短信效验码发送失败！";
                        str2 = "false";
                    }
                }
                else
                {
                    str = "当前手机号码没有认证！";
                    str2 = "false";
                }
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

        private UserInfo _currentUser = null;

        public UserInfo currentUser
        {
            get
            {
                if (this._currentUser == null)
                {
                    this._currentUser = UserFactory.CurrentMember;
                }
                return this._currentUser;
            }
        }
    }
}

