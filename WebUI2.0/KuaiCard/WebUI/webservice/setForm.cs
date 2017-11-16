namespace OriginalStudio.WebUI.webservice
{
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model.User;
    using OriginalStudio.Lib.Security;
    using System;
    using System.Web;
    using System.Web.SessionState;

    public class setForm : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        private UserInfo _user = null;

        public void ProcessRequest(HttpContext context)
        {
            string s = "";
            string str2 = context.Request["password"];
            string str3 = context.Request["password2"];
            if (this.username == "")
            {
                s = "{\"result\":\"no\",\"ico\":\"error\",\"msg\":\"验证超时！\",\"url\":\"/getPassword.html\"}";
            }
            else if (this.userInfo == null)
            {
                s = "{\"result\":\"no\",\"ico\":\"error\",\"msg\":\"验证超时！\",\"url\":\"/getPassword.html\"}";
            }
            if (this.renzheng == "")
            {
                s = "{\"result\":\"no\",\"ico\":\"error\",\"msg\":\"验证超时！\",\"url\":\"/getPassword.html\"}";
            }
            else if (HttpContext.Current.Session["findpwduserok"].ToString() != Cryptography.MD5("yanzhengtongguook", "GB2312"))
            {
                s = "{\"result\":\"no\",\"ico\":\"error\",\"msg\":\"验证超时！\",\"url\":\"/getPassword.html\"}";
            }
            else if (string.IsNullOrEmpty(str2))
            {
                s = "{\"result\":\"no\",\"ico\":\"error\",\"msg\":\"密码不能为空！\",\"url\":\"/getPassword.html\"}";
            }
            else if (string.IsNullOrEmpty(str3))
            {
                s = "{\"result\":\"no\",\"ico\":\"error\",\"msg\":\"两次密码不能为空！\",\"url\":\"/getPassword.html\"}";
            }
            else if (str2 != str3)
            {
                s = "{\"result\":\"no\",\"ico\":\"error\",\"msg\":\"两次密码不一致！请重新输入\",\"url\":\"/getPassword.html\"}";
            }
            else
            {
                this.userInfo.Password = str2;
                if (UserFactory.Update(this.userInfo, null))
                {
                    HttpContext.Current.Session["findpwduserok"] = null;
                    HttpContext.Current.Session["findpwduser"] = null;
                    s = "{\"result\":\"ok\",\"ico\":\"success\",\"msg\":\"修改成功！\", \"url\":\"/index.aspx\"}";
                }
                else
                {
                    s = "{\"result\":\"no\",\"ico\":\"error\",\"msg\":\"修改失败！\",\"url\":\"/index.aspx\"}";
                }
            }
            HttpContext.Current.Response.ContentType = "text/html";
            context.Response.Write(s);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public string renzheng
        {
            get
            {
                if (HttpContext.Current.Session["findpwduserok"] != null)
                {
                    return HttpContext.Current.Session["findpwduserok"].ToString();
                }
                return string.Empty;
            }
        }

        public UserInfo userInfo
        {
            get
            {
                if (!(string.IsNullOrEmpty(this.username) || (this._user != null)))
                {
                    this._user = UserFactory.GetModelByName(this.username);
                }
                return this._user;
            }
        }

        public string username
        {
            get
            {
                if (HttpContext.Current.Session["findpwduser"] != null)
                {
                    return HttpContext.Current.Session["findpwduser"].ToString();
                }
                return string.Empty;
            }
        }
    }
}

