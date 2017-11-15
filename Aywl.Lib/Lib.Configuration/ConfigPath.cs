using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

namespace OriginalStudio.Lib.Configuration
{
    public class ConfigPath
    {

        #region 配置文件路径

        /// <summary>
        /// urlmanagerconfiguration.config 配置文件路径
        /// </summary>
        public static string UrlManagerConfigPath
        {
            get
            {
                try
                {
                    string p_config = System.Configuration.ConfigurationManager.AppSettings["urlmanagerconfiguration"];
                    if (HttpContext.Current != null)
                    {
                        return HttpContext.Current.Server.MapPath(p_config);
                    }
                    else
                    {
                        //非web程序引用  
                        p_config = p_config.Replace("/", "\\");
                        if (p_config.StartsWith("\\") || p_config.StartsWith("~"))
                        {
                            p_config = p_config.Substring(p_config.IndexOf('\\', 1)).TrimStart('\\');
                        }
                        return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, p_config);
                    }  
                }
                catch
                {

                    return "";
                }
            }
        }

        /// <summary>
        /// runtimeconfiguration.config 配置文件路径
        /// </summary>
        public static string RuntimeConfigPath
        {
            get
            {
                try
                {
                    string p_config = System.Configuration.ConfigurationManager.AppSettings["runtimeconfiguration"];
                    if (HttpContext.Current != null)
                    {
                        return HttpContext.Current.Server.MapPath(p_config);
                    }
                    else
                    {
                        //非web程序引用  
                        p_config = p_config.Replace("/", "\\");
                        if (p_config.StartsWith("\\") || p_config.StartsWith("~"))
                        {
                            p_config = p_config.Substring(p_config.IndexOf('\\', 1)).TrimStart('\\');
                        }
                        return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, p_config);
                    }  
                }
                catch
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// memcached.config 配置文件路径
        /// </summary>
        public static string MemcachedConfigPath
        {
            get
            {
                try
                {
                    string p_config = System.Configuration.ConfigurationManager.AppSettings["memcached"];
                    if (HttpContext.Current != null)
                    {
                        return HttpContext.Current.Server.MapPath(p_config);
                    }
                    else
                    {
                        //非web程序引用  
                        p_config = p_config.Replace("/", "\\");
                        if (p_config.StartsWith("\\") || p_config.StartsWith("~"))
                        {
                            p_config = p_config.Substring(p_config.IndexOf('\\', 1)).TrimStart('\\');
                        }
                        return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, p_config);
                    }  
                }
                catch
                {
                    return "";
                }
            }
        }

        #endregion
    }
}
