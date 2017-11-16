namespace KuaiCard.WebUI.User.Service
{
    using OriginalStudio.BLL.User;
    using System;
    using System.Web;
    using System.Web.SessionState;

    public class password : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string str = "";
            string str2 = "false";
            string str3 = "";
            string str4 = context.Request["oldpassword"];
            string str5 = context.Request["password"];
            string str6 = context.Request["password2"];
            if (str4 != UserFactory.CurrentMember.Password)
            {
                str = "旧密码不正确";
                str2 = "false";
                str3 = "error";
            }
            else if (str5 != str6)
            {
                str = "两次密码不一致";
                str2 = "false";
                str3 = "error";
            }
            else if (str6 == str4)
            {
                str = "新密码不能与旧密码一样";
                str2 = "false";
                str3 = "error";
            }
            if (string.IsNullOrEmpty(str))
            {
                UserFactory.CurrentMember.Password = str5;
                if (UserFactory.Update(UserFactory.CurrentMember, null))
                {
                    str = "修改成功！";
                    str2 = "true";
                    str3 = "success";
                }
                else
                {
                    str = "更新失败！";
                    str2 = "false";
                    str3 = "error";
                }
            }
            context.Response.ContentType = "application/json";
            context.Response.Write("{\"result\":" + str2 + ",\"text\":\"" + str + "\",\"ico\":\"" + str3 + "\"}");
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

