namespace KuaiCard.WebUI.register
{
    using KuaiCard.BLL;
    using KuaiCard.BLL.User;
    using KuaiCard.Cache;
    using KuaiCard.Model.User;
    using KuaiCardLib.Security;
    using KuaiCardLib.Web;
    using System;
    using System.Web;
    using System.Web.UI;

    public class reg1 : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string s = "0";
            string str2 = HttpContext.Current.Request["mobile"];
            string str3 = HttpContext.Current.Request["password"];
            string str4 = HttpContext.Current.Request["password2"];
            string str5 = HttpContext.Current.Request["mobilecode"];
            string str6 = HttpContext.Current.Request["agreement"];
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
            else if (str5 != str8)
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
                        s = " {\"result\":true, \"text\":\"注册成功！\", \"id\":\"1\"}";
                    }
                    else
                    {
                        s = " {\"result\":true, \"text\":\"注册成功,请等待审核！\", \"id\":\"2\"}";
                    }
                }
                else
                {
                    s = " {\"result\":true, \"text\":\"注册失败！\", \"id\":\"3\"}";
                }
            }
            HttpContext.Current.Response.ContentType = "text/plain";
            HttpContext.Current.Response.Write(s);
        }
    }
}

