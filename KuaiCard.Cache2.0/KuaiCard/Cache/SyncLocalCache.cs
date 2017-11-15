namespace KuaiCard.Cache
{
    using KuaiCard.SysConfig;
    using KuaiCardLib.Security;
    using System;
    using System.Web;
    using System.Web.Services;

    [WebService(Namespace="http://tempuri.org/"), WebServiceBinding(ConformsTo=WsiProfiles.BasicProfile1_1)]
    public class SyncLocalCache : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string str = context.Request.QueryString["cacheKey"];
            string str2 = context.Request.QueryString["passKey"];
            if (string.IsNullOrEmpty(str))
            {
                context.Response.Write("CacheKey is not null!");
            }
            else if (str2 != Cryptography.MD5(str + MemCachedConfig.AuthCode))
            {
                context.Response.Write("AuthCode is not valid!");
            }
            else
            {
                WebCache.LoadCacheStrategy(new DefaultCacheStrategy());
                WebCache.GetCacheService().RemoveObject(str);
                WebCache.LoadDefaultCacheStrategy();
                context.Response.Write("OK");
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

