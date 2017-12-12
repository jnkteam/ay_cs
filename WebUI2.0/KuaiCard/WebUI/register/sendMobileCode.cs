namespace OriginalStudio.WebUI.register
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.SMS;
    using OriginalStudio.Cache;
    using System;
    using System.Web;
    using System.Web.SessionState;

    public class sendMobileCode : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string s = "0";
            string newValue = context.Request.QueryString["mobile"];
            string str3 = context.Request.QueryString["captcha"];
            string objId = "PHONE_VALID_" + newValue;
            if (HttpContext.Current.Session["CCode"].ToString() == str3.ToUpper())
            {
                string o = (string) WebCache.GetCacheService().RetrieveObject(objId);
                if (o == null)
                {
                    o = new Random().Next(0x186a0, 0xf423f).ToString();
                    WebCache.GetCacheService().AddObject(objId, o);
                }
                string msg = SysConfig.sms_temp_Authenticate;
                msg = SysConfig.sms_temp_Modify.Replace("{@username}", newValue).Replace("{@sitename}", WebInfoFactory.CurrentWebInfo.Name).Replace("{@authcode}", o);
                if (string.IsNullOrEmpty(SMS.SendSmsWithCheck(newValue, msg, "")))
                {
                    s = " {\"result\":true, \"text\":\"短信效验码已发送，请注意查收！\"}";
                }
                else
                {
                    s = " {\"result\":false, \"text\":\"短信效验码发送失败\"}";
                }
            }
            else
            {
                s = " {\"result\":false, \"text\":\"验证码填写错误\"}";
            }
            context.Response.ContentType = "text/html";
            context.Response.Write(s);
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

