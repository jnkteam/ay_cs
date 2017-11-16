namespace KuaiCard.WebUI.register
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Cache;
    using OriginalStudio.Model.User;
    using OriginalStudio.Lib.Security;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Web;

    public class reg : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string s = "0";
            string str2 = context.Request["mobile"];
            string str3 = context.Request["password"];
            string str4 = context.Request["password2"];
            string str5 = context.Request["mobilecode"];
            string str6 = context.Request["agreement"];
            string objId = "PHONE_VALID_" + str2;
            string str8 = (string) WebCache.GetCacheService().RetrieveObject(objId);
            if (string.IsNullOrEmpty(str2))
            {
                s = " {\"result\":false, \"text\":\"手机号不能为空\"}";
            }
            else if (string.IsNullOrEmpty(str3))
            {
                s = " {\"result\":false, \"text\":\"手机号不能为空！\"}";
            }
            else if (string.IsNullOrEmpty(str4))
            {
                s = " {\"result\":false, \"text\":\"手机号不能为空！\"}";
            }
            else if (string.IsNullOrEmpty(str5))
            {
                s = " {\"result\":false, \"text\":\"短信效验码不能为空！\"}";
            }
            else if (str2 != str8)
            {
                s = " {\"result\":false, \"text\":\"短信效验码不能为空！\"}";
            }
            else
            {
                UserInfo info = new UserInfo();
                info.UserName = str2;
                info.Password = Cryptography.MD5(str3);
                info.Email = string.Empty;
                info.QQ = string.Empty;
                info.Tel = str2;
                info.SiteName = string.Empty;
                info.SiteUrl = string.Empty;
                info.IdCard = string.Empty;
                info.full_name = string.Empty;
                info.Status = SysConfig.IsAudit ? 1 : 2;
                info.PMode = 1;
                info.PayeeBank = string.Empty;
                info.BankProvince = string.Empty;
                info.BankCity = string.Empty;
                info.BankAddress = string.Empty;
                info.Account = string.Empty;
                info.PayeeName = string.Empty;
                info.LastLoginIp = ServerVariables.TrueIP;
                info.LastLoginTime = DateTime.Now;
                info.RegTime = DateTime.Now;
                info.Settles = SysConfig.DefaultSettledMode;
                info.CPSDrate = SysConfig.DefaultCPSDrate;
                info.IsPhonePass = 1;
                info.AgentId = 0;
                info.APIAccount = 0L;
                info.APIKey = Guid.NewGuid().ToString("N");
                info.question = string.Empty;
                info.answer = string.Empty;
                if (UserFactory.Add(info) > 0)
                {
                    if (!SysConfig.IsAudit)
                    {
                        HttpContext.Current.Response.Redirect("/register.aspx?id=1");
                    }
                    else
                    {
                        HttpContext.Current.Response.Redirect("/register.aspx?id=2");
                    }
                }
                else
                {
                    HttpContext.Current.Response.Redirect("/register.aspx?id=3");
                }
                return;
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

