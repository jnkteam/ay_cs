namespace KuaiCardLib.WebComponents.UrlManager
{
    using KuaiCard.SysConfig;
    using System;
    using System.IO;
    using System.Web;
    using System.Web.UI;

    public class PageHandler : HandlerBase
    {
        public PageHandler()
        {
        }

        public PageHandler(HttpContext context, UrlManagerConfig config, string simplePath, string realPath, string filePath, string pathInfo, string queryString, string url) : base(context, config, simplePath, realPath, filePath, pathInfo, queryString, url)
        {
        }

        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            string path = context.Server.MapPath(base.RealPath);
            if (!File.Exists(path))
            {
                context.Response.Redirect(base.Error404Url, true);
            }
            else
            {
                string action = base.Config.Action;
                if (action != null)
                {
                    if (!(action == "none"))
                    {
                        if (action == "rewrite")
                        {
                            context.RewritePath(context.Request.Path, base.PathInfo, base.QueryString);
                            PageParser.GetCompiledPageInstance(base.RealPath, path, context).ProcessRequest(context);
                        }
                        else if (((action == "cache_static") || (action == "cache_static_news")) || (action == "cache_static_pub"))
                        {
                            string fileName = context.Server.MapPath(base.FilePath);
                            FileInfo info = new FileInfo(fileName);
                            FileInfo info2 = new FileInfo(path);
                            bool flag = false;
                            if ((!info.Exists || ((base.Config.TimeSpan.Ticks > 0L) && (info.LastWriteTime.Add(base.Config.TimeSpan) < DateTime.Now))) || (info2.LastWriteTime > info.LastWriteTime))
                            {
                                flag = true;
                            }
                            if (flag)
                            {
                                context.RewritePath(context.Request.Path, base.PathInfo, base.QueryString);
                                string text1 = base.Config.Action;
                                PageParser.GetCompiledPageInstance(base.RealPath, path, context).ProcessRequest(context);
                                base.SetStaticFilter(context, fileName);
                            }
                            else
                            {
                                context.Response.WriteFile(fileName);
                                context.Response.End();
                            }
                        }
                    }
                    else
                    {
                        PageParser.GetCompiledPageInstance(base.RealPath, path, context).ProcessRequest(context);
                    }
                }
            }
        }
    }
}

