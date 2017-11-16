namespace KuaiCard.WebUI.User.Service
{
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model.User;
    using System;
    using System.Web;
    using System.Web.SessionState;

    public class user : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string s = "";
            string str2 = context.Request["name"];
            string str3 = "";
            string str4 = "";
            if ((str2 == "email") && (this.currentUser.IsEmailPass == 1))
            {
                s = "邮箱已认证！";
                str3 = "error";
                str4 = "ok";
            }
            s = "{\"result\":\"" + str4 + "\",\"ico\":\"" + str3 + "\",\"msg\":\"" + s + "\"}";
            context.Response.ContentType = "application/json";
            context.Response.Write(s);
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

