namespace KuaiCard.WebUI.merchant.ajax
{
    using KuaiCard.BLL.User;
    using KuaiCard.Cache;
    using KuaiCard.Model.User;
    using KuaiCardLib.Web;
    using System;
    using System.Web;
    using System.Web.SessionState;

    public class userinfo : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        private string GetParmValue(string caption)
        {
            return WebBase.GetFormString(caption, "");
        }

        public void ProcessRequest(HttpContext context)
        {
            string str = "";
            if (this.currentUser == null)
            {
                str = "登录信息失效，请重新登录";
            }
            else
            {
                string queryStringString = WebBase.GetQueryStringString("action", "");
                string str3 = WebBase.GetQueryStringString("yphone", "");
                string str4 = WebBase.GetQueryStringString("phone", "");
                string str5 = WebBase.GetQueryStringString("getdate", "");
                string str6 = WebBase.GetQueryStringString("phonecode", "");
                string str7 = WebBase.GetQueryStringString("hostName", "");
                string str8 = WebBase.GetQueryStringString("hostUrl", "");
                string objId = "PHONE_VALID_" + str4;
                string str10 = (string) WebCache.GetCacheService().RetrieveObject(objId);
                this.currentUser.SiteName = str7;
                this.currentUser.SiteUrl = str8;
                if (!string.IsNullOrEmpty(str4))
                {
                    this.currentUser.Tel = str4;
                }
                if (this.currentUser.IsPhonePass == 0)
                {
                    if (string.IsNullOrEmpty(str4))
                    {
                        str = "请输入手机号码!";
                    }
                    else if (string.IsNullOrEmpty(str4))
                    {
                        str = "请输入手机认验码!";
                    }
                    else if (str6 != str10)
                    {
                        str = "手机验证码不正确!";
                    }
                    else
                    {
                        this.currentUser.IsPhonePass = 1;
                        this.currentUser.Tel = str4;
                    }
                }
                else if (queryStringString == "modiphone")
                {
                    if (string.IsNullOrEmpty(str3))
                    {
                        str = "请输入原手机号码!";
                    }
                    else if (str3 != this.currentUser.Tel)
                    {
                        str = "原手机号码输入错误!";
                    }
                    else if (string.IsNullOrEmpty(str4))
                    {
                        str = "请输入新手机号码!";
                    }
                    else if (str3 == str4)
                    {
                        str = "原手机号码和新手机号码一样!";
                    }
                    else if (string.IsNullOrEmpty(str6))
                    {
                        str = "请输入手机认验码!";
                    }
                    else if (str6 != str10)
                    {
                        str = "手机验证码不正确!";
                    }
                    else
                    {
                        this.currentUser.Tel = str4;
                    }
                }
                if (string.IsNullOrEmpty(str))
                {
                    if (UserFactory.Update(this.currentUser, null))
                    {
                        str = "true";
                    }
                    else
                    {
                        this.currentUser.IsPhonePass = 0;
                        this.currentUser.Tel = string.Empty;
                        str = "修改失败";
                    }
                }
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(str);
        }

        public UserInfo currentUser
        {
            get
            {
                return UserFactory.CurrentMember;
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

