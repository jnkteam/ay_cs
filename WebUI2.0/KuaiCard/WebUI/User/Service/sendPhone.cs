namespace KuaiCard.WebUI.User.Service
{
    using KuaiCard.BLL;
    using KuaiCard.BLL.Tools;
    using KuaiCard.BLL.User;
    using KuaiCard.Cache;
    using KuaiCard.Model.User;
    using System;
    using System.Web;
    using System.Web.SessionState;

    public class sendPhone : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
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

            string mobile = context.Request["template"];
            string name = context.Request["name"];

            if (string.IsNullOrEmpty(name) || (name != "getcode"))
            {
                if (this.currentUser.IsPhonePass == 1)
                {
                    s = "当前用户已经认证！";
                    str2 = "false";
                    goto Label_Exit;
                }
            }

            if (name == "getcode")
            {
                if (this.currentUser.IsPhonePass == 0)
                {
                    s = "手机号未认证！";
                    str2 = "false";
                    goto Label_Exit;
                }

                //2017.8.19 add ，说明纯粹取验证码，以用户绑定的手机号为准
                mobile = string.IsNullOrEmpty(mobile) ? this.currentUser.Tel : mobile;
            }

            if (mobile == "")
            {
                s = "绑定手机号码为空！";
                str2 = "false";
                goto Label_Exit;
            }

            string objId = "PHONE_VALID_" + mobile;
            string o = (string)WebCache.GetCacheService().RetrieveObject(objId);
            if (o == null)
            {
                o = new Random().Next(0x2710, 0x1869f).ToString();
                WebCache.GetCacheService().AddObject(objId, o);
            }
            string msg = SysConfig.sms_temp_Authenticate;

            msg = SysConfig.sms_temp_Modify.Replace("{@username}", UserFactory.CurrentMember.UserName).Replace("{@sitename}", WebInfoFactory.CurrentWebInfo.Name).Replace("{@authcode}", o);
            if (string.IsNullOrEmpty(SMS.SendSmsWithCheck(mobile, msg, "")))
            {
                s = "短信效验码已发送，请注意查收！";
                str2 = "true";
            }
            else
            {
                s = "短信效验码发送失败！";
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

