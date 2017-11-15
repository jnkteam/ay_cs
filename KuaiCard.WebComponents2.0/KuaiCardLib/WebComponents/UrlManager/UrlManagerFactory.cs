namespace KuaiCardLib.WebComponents.UrlManager
{
    using KuaiCard.SysConfig;
    using System;
    using System.IO;
    using System.Web;

    public class UrlManagerFactory : IHttpHandlerFactory
    {
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            UrlManagerConfig config = null;
            string simplePath = string.Empty;
            string realPath = string.Empty;
            string filePath = string.Empty;
            string pathInfo = string.Empty;
            string queryString = string.Empty;
            string str6 = string.Empty;
            UrlManagerConfig.IsMatch(context, ref config, ref simplePath, ref realPath, ref filePath, ref pathInfo, ref queryString, ref str6);
            if (config != null)
            {
                if (Path.GetExtension(context.Request.Path) == ".aspx")
                {
                    return new PageHandler(context, config, simplePath, realPath, filePath, pathInfo, queryString, str6);
                }
                return new KuaiCardLib.WebComponents.UrlManager.StaticFileHandler(context, config, simplePath, realPath, filePath, pathInfo, queryString, str6);
            }
            if (Path.GetExtension(context.Request.Path) == ".aspx")
            {
                return new PageHandler();
            }
            return new KuaiCardLib.WebComponents.UrlManager.StaticFileHandler();
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
        }
    }
}

