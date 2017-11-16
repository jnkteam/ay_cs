namespace OriginalStudio.WebComponents.UrlManager
{
    using OriginalStudio.Lib.SysConfig;
    using System;
    using System.IO;
    using System.Web;

    public class StaticFileHandler : HandlerBase
    {
        public StaticFileHandler()
        {
        }

        public StaticFileHandler(HttpContext context, UrlManagerConfig config, string simplePath, string realPath, string filePath, string pathInfo, string queryString, string url) : base(context, config, simplePath, realPath, filePath, pathInfo, queryString, url)
        {
        }

        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            context.Response.Clear();
            string path = context.Server.MapPath(base.RealPath);
            if (File.Exists(path))
            {
                context.Response.WriteFile(path);
                context.Response.End();
            }
            else
            {
                context.Response.Redirect(base.Error404Url, true);
            }
        }
    }
}

