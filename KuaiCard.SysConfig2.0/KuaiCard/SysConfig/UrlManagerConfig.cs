namespace KuaiCard.SysConfig
{
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Caching;
    using System.Xml;

    [Serializable]
    public class UrlManagerConfig
    {
        private string _action = string.Empty;
        private string _filePath = string.Empty;
        private string _host = string.Empty;
        private string _pathInfo = string.Empty;
        private string _pattern = string.Empty;
        private string _queryString = string.Empty;
        private string _realPath = string.Empty;
        private System.TimeSpan _timeSpan = new System.TimeSpan(0L);
        private string _url = string.Empty;
        protected static volatile Cache webCache = HttpRuntime.Cache;

        internal static string FormatFilePath(HttpContext context, string filePath)
        {
            int length = filePath.LastIndexOf(".");
            if (length > 0)
            {
                return (filePath.Substring(0, length) + "." + context.Request.Url.Host + filePath.Substring(length));
            }
            return filePath;
        }

        public static List<UrlManagerConfig> GetConfigs(string host)
        {
            if (webCache.Get(host) != null)
            {
                return (webCache.Get(host) as List<UrlManagerConfig>);
            }
            string urlManagerConfigPath = RuntimeSetting.UrlManagerConfigPath;
            if (File.Exists(urlManagerConfigPath))
            {
                try
                {
                    XmlTextReader reader = new XmlTextReader(urlManagerConfigPath);
                    int num = (int) reader.MoveToContent();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(reader.ReadOuterXml());
                    List<UrlManagerConfig> listFromXmlDocument = GetListFromXmlDocument(doc, host);
                    webCache.Insert(host, listFromXmlDocument, new CacheDependency(urlManagerConfigPath));
                    reader.Close();
                    return listFromXmlDocument;
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                }
            }
            return new List<UrlManagerConfig>(0);
        }

        public static UrlManagerConfig GetFromXmlNode(XmlNode node, string host)
        {
            UrlManagerConfig config = new UrlManagerConfig();
            config.Host = host;
            if (((node.Attributes["action"] != null) && (node.Attributes["action"].Value != null)) && (node.Attributes["action"].Value.Length > 0))
            {
                config.Action = node.Attributes["action"].Value;
            }
            if (((node.Attributes["pattern"] != null) && (node.Attributes["pattern"].Value != null)) && (node.Attributes["pattern"].Value.Length > 0))
            {
                config.Pattern = node.Attributes["pattern"].Value;
            }
            if (((node.Attributes["url"] != null) && (node.Attributes["url"].Value != null)) && (node.Attributes["url"].Value.Length > 0))
            {
                config.Url = node.Attributes["url"].Value;
            }
            if (((node.Attributes["realpath"] != null) && (node.Attributes["realpath"].Value != null)) && (node.Attributes["realpath"].Value.Length > 0))
            {
                config.RealPath = node.Attributes["realpath"].Value;
            }
            if (((node.Attributes["filepath"] != null) && (node.Attributes["filepath"].Value != null)) && (node.Attributes["filepath"].Value.Length > 0))
            {
                config.FilePath = node.Attributes["filepath"].Value;
            }
            if (((node.Attributes["pathinfo"] != null) && (node.Attributes["pathinfo"].Value != null)) && (node.Attributes["pathinfo"].Value.Length > 0))
            {
                config.PathInfo = node.Attributes["pathinfo"].Value;
            }
            if (((node.Attributes["querystring"] != null) && (node.Attributes["querystring"].Value != null)) && (node.Attributes["querystring"].Value.Length > 0))
            {
                config.QueryString = node.Attributes["querystring"].Value;
            }
            if (((node.Attributes["timespan"] != null) && (node.Attributes["timespan"].Value != null)) && (node.Attributes["timespan"].Value.Length > 0))
            {
                config.TimeSpan = System.TimeSpan.Parse(node.Attributes["timespan"].Value);
            }
            return config;
        }

        public static List<UrlManagerConfig> GetListFromXmlDocument(XmlDocument doc, string host)
        {
            XmlNodeList list = doc.SelectNodes("configs/" + host + "/location");
            List<UrlManagerConfig> list2 = new List<UrlManagerConfig>(list.Count);
            foreach (XmlNode node in list)
            {
                list2.Add(GetFromXmlNode(node, host));
            }
            return list2;
        }

        public static bool IsMatch(HttpContext context, ref UrlManagerConfig config, ref string simplePath, ref string realPath, ref string filePath, ref string pathInfo, ref string queryString, ref string url)
        {
            Match match;
            string[] strArray;
            int num;
            string input = context.Request.RawUrl.Substring((context.Request.ApplicationPath == "/") ? (context.Request.ApplicationPath.Length - 1) : context.Request.ApplicationPath.Length);
            simplePath = context.Request.Path.Substring((context.Request.ApplicationPath == "/") ? (context.Request.ApplicationPath.Length - 1) : context.Request.ApplicationPath.Length);
            List<UrlManagerConfig> configs = GetConfigs("none");
            if (configs != null)
            {
                foreach (UrlManagerConfig config2 in configs)
                {
                    match = new Regex(config2.Pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase).Match(input);
                    if (match.Success)
                    {
                        config = config2;
                        strArray = new string[match.Groups.Count];
                        num = 0;
                        while (num < match.Groups.Count)
                        {
                            strArray[num] = match.Groups[num].Value;
                            num++;
                        }
                        realPath = string.Format(config2.RealPath, (object[]) strArray);
                        filePath = FormatFilePath(context, string.Format(config2.FilePath, (object[]) strArray));
                        pathInfo = string.Format(config2.PathInfo, (object[]) strArray);
                        queryString = string.Format(config2.QueryString, (object[]) strArray);
                        url = string.Format(config2.Url, (object[]) strArray);
                        return true;
                    }
                }
            }
            List<UrlManagerConfig> list2 = GetConfigs(context.Request.Url.Host);
            if (list2 != null)
            {
                foreach (UrlManagerConfig config2 in list2)
                {
                    match = new Regex(config2.Pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase).Match(input);
                    if (match.Success)
                    {
                        config = config2;
                        strArray = new string[match.Groups.Count];
                        for (num = 0; num < match.Groups.Count; num++)
                        {
                            strArray[num] = match.Groups[num].Value;
                        }
                        realPath = string.Format(config2.RealPath, (object[]) strArray);
                        filePath = FormatFilePath(context, string.Format(config2.FilePath, (object[]) strArray));
                        pathInfo = string.Format(config2.PathInfo, (object[]) strArray);
                        queryString = string.Format(config2.QueryString, (object[]) strArray);
                        url = string.Format(config2.Url, (object[]) strArray);
                        return true;
                    }
                }
            }
            return false;
        }

        public string Action
        {
            get
            {
                return this._action;
            }
            set
            {
                this._action = value;
            }
        }

        public string FilePath
        {
            get
            {
                return this._filePath;
            }
            set
            {
                this._filePath = value;
            }
        }

        public string Host
        {
            get
            {
                return this._host;
            }
            set
            {
                this._host = value;
            }
        }

        public string PathInfo
        {
            get
            {
                return this._pathInfo;
            }
            set
            {
                this._pathInfo = value;
            }
        }

        public string Pattern
        {
            get
            {
                return this._pattern;
            }
            set
            {
                this._pattern = value;
            }
        }

        public string QueryString
        {
            get
            {
                return this._queryString;
            }
            set
            {
                this._queryString = value;
            }
        }

        public string RealPath
        {
            get
            {
                return this._realPath;
            }
            set
            {
                this._realPath = value;
            }
        }

        public System.TimeSpan TimeSpan
        {
            get
            {
                return this._timeSpan;
            }
            set
            {
                this._timeSpan = value;
            }
        }

        public string Url
        {
            get
            {
                return this._url;
            }
            set
            {
                this._url = value;
            }
        }
    }
}

