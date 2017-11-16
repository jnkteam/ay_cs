namespace OriginalStudio.WebUI.User.Service
{
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model.User;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Web;
    using System.Web.SessionState;

    public class UserAgent : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string str = string.Empty;
            string str2 = "";
            string str3 = "";

            if (this.currentUser == null)
            {
                str = "未登录";
                str2 = "no";
                str3 = "error";
                context.Response.ContentType = "application/json";
                context.Response.Write("{\"result\":\"" + str2 + "\",\"ico\":\"" + str3 + "\",\"msg\":\"" + str + "\"}");

                return;
            }

            string formString = WebBase.GetFormString("m", "");
            string str5 = WebBase.GetFormString("c", "");
            string str6 = WebBase.GetFormString("a", "");
            if (formString == "user")
            {
                if (str5 == "profile")
                {
                    string str7 = WebBase.GetFormString("name", "");
                    string str8 = WebBase.GetFormString("qq", "");
                    if (string.IsNullOrEmpty(str7))
                    {
                        str = "姓名不能为空";
                        str2 = "no";
                        str3 = "error";
                        context.Response.ContentType = "application/json";
                        context.Response.Write("{\"result\":\"" + str2 + "\",\"ico\":\"" + str3 + "\",\"msg\":\"" + str + "\"}");
                        return;
                    }
                    if (string.IsNullOrEmpty(str8))
                    {
                        str = "QQ不能为空";
                        str2 = "no";
                        str3 = "error";
                        context.Response.ContentType = "application/json";
                        context.Response.Write("{\"result\":\"" + str2 + "\",\"ico\":\"" + str3 + "\",\"msg\":\"" + str + "\"}");
                        return;
                    }
                    this.currentUser.QQ = str8;
                    this.currentUser.full_name = str7;
                }
                if (string.IsNullOrEmpty(str))
                {
                    if (UserFactory.Update(this.currentUser, null))
                    {
                        str = "修改成功";
                        str2 = "ok";
                        str3 = "success";
                    }
                    else
                    {
                        str = "修改失败";
                        str2 = "ok";
                        str3 = "fail";
                    }
                }
            }
            context.Response.ContentType = "application/json";
            context.Response.Write("{\"result\":\"" + str2 + "\",\"ico\":\"" + str3 + "\",\"msg\":\"" + str + "\"}");
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

