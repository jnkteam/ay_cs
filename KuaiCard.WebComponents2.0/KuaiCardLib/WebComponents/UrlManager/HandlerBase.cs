﻿namespace KuaiCardLib.WebComponents.UrlManager
{
    using KuaiCard.SysConfig;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using System.Web.SessionState;

    public class HandlerBase : IHttpHandler, IRequiresSessionState
    {
        private static List<string> _writingStaticFilePathes = new List<string>();
        protected UrlManagerConfig Config;
        protected string FilePath;
        protected string PathInfo;
        protected string QueryString;
        protected string RealPath;
        protected string SimplePath;
        protected string Url;

        public HandlerBase()
        {
        }

        public HandlerBase(HttpContext context, UrlManagerConfig config, string simplePath, string realPath, string filePath, string pathInfo, string queryString, string url) : this()
        {
            this.Config = config;
            this.SimplePath = (context.Request.ApplicationPath == "/") ? simplePath : (context.Request.ApplicationPath + simplePath);
            this.RealPath = (context.Request.ApplicationPath == "/") ? realPath : (context.Request.ApplicationPath + realPath);
            this.FilePath = (context.Request.ApplicationPath == "/") ? filePath : (context.Request.ApplicationPath + filePath);
            this.PathInfo = pathInfo;
            this.QueryString = queryString;
            this.Url = url;
        }

        public virtual void ProcessRequest(HttpContext context)
        {
            if (this.Config == null)
            {
                this.Config = new UrlManagerConfig();
                this.Config.Action = "none";
                this.SimplePath = context.Request.Path;
                this.RealPath = context.Request.Path;
                this.FilePath = context.Request.Path;
                this.PathInfo = context.Request.PathInfo;
                this.QueryString = context.Request.QueryString.ToString();
                this.Url = context.Request.Url.ToString();
            }
            else
            {
                string action = this.Config.Action;
                if (action != null)
                {
                    if (!(action == "none"))
                    {
                        if (action == "redirect")
                        {
                            context.Response.Redirect(this.Url, true);
                        }
                    }
                    else
                    {
                        this.SimplePath = context.Request.Path;
                        this.RealPath = context.Request.Path;
                        this.FilePath = context.Request.Path;
                        this.PathInfo = context.Request.PathInfo;
                        this.QueryString = context.Request.QueryString.ToString();
                        this.Url = context.Request.Url.ToString();
                    }
                }
            }
        }

        protected void SetStaticFilter(HttpContext context, string physicalPath)
        {
            string directoryName = Path.GetDirectoryName(physicalPath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            context.Response.Filter = new KuaiCardLib.WebComponents.UrlManager.Filter(context.Response.Filter, physicalPath);
        }

        protected string Error404Url
        {
            get
            {
                return "/Error404Url.aspx";
            }
        }

        public virtual bool IsReusable
        {
            get
            {
                return true;
            }
        }

        public static List<string> WritingStaticFilePathes
        {
            get
            {
                return _writingStaticFilePathes;
            }
        }
    }
}

