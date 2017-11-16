namespace OriginalStudio.WebUI.register
{
    using OriginalStudio.Cache;
    using System;
    using System.Web;
    using System.Web.SessionState;

    public class ajaxcheckmobilecode : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string s = "0";
            string str2 = context.Request.QueryString["mobile"];
            string str3 = context.Request.QueryString["mobilecode"];
            string objId = "PHONE_VALID_" + str2;
            string str5 = (string) WebCache.GetCacheService().RetrieveObject(objId);
            if (str3 != str5)
            {
                s = "0";
            }
            else
            {
                s = "1";
            }
            context.Response.ContentType = "text/plain";
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

