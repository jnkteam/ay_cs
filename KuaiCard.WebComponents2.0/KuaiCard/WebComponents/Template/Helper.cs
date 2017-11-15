namespace KuaiCard.WebComponents.Template
{
    using KuaiCard.Cache;
    using KuaiCardLib.ExceptionHandling;
    using KuaiCardLib.IO;
    using System;
    using System.Web;
    using System.Web.Caching;

    public class Helper
    {
        public static string GetEmailCheckTemp()
        {
            return GetEmailTempCont("template_email_checkmail", @"email\checkemail.txt");
        }

        public static string GetEmailRegisterTemp()
        {
            return GetEmailTempCont("template_email_register", @"email\register.txt");
        }

        public static string GetEmailTempCont(string cacheKey, string path)
        {
            try
            {
                string str3;
                string filepath = BaseDir + path;
                object obj2 = DefaultCacheStrategy.GetWebCacheObj.Get(cacheKey);
                if (obj2 == null)
                {
                    str3 = File.ReadFile(filepath);
                    DefaultCacheStrategy.GetWebCacheObj.Insert(cacheKey, str3, new CacheDependency(filepath));
                }
                else
                {
                    str3 = obj2.ToString();
                }
                return str3;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return string.Empty;
            }
        }

        public static string BaseDir
        {
            get
            {
                return (HttpContext.Current.Request.PhysicalApplicationPath + @"\common\template\");
            }
        }
    }
}

