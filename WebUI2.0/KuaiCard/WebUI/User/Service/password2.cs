namespace KuaiCard.WebUI.User.Service
{
    using KuaiCard.BLL.User;
    using System;
    using System.Web;
    using System.Web.SessionState;
    using KuaiCard.Model.User;

    public class password2 : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string msg = "";
            string result = "false";
            string ico = "";
            string url = "";

            if (this.currentUser == null)
            {
                //未登录
                msg = "未登录!";
                result = "no";
                ico = "error";
                goto Label_Exit;
            }

            string oldpassword = context.Request["oldpassword"];
            string password = context.Request["password"];
            string password2 = context.Request["password2"];

            //if ((UserFactory.CurrentMember.Password2 == "") || (UserFactory.CurrentMember.Password2 == null))
            if (String.IsNullOrEmpty(UserFactory.CurrentMember.Password2))
            {
                if (password != password2)
                {
                    msg = "两次密码不一致";
                    result = "false";
                    ico = "error";
                }
                else if (string.IsNullOrEmpty(password))
                {
                    msg = "密码不能为空";
                    result = "false";
                    ico = "error";
                }
                else if (string.IsNullOrEmpty(password2))
                {
                    msg = "确认密码不能为空";
                    result = "false";
                    ico = "error";
                }
            }
            else if (oldpassword != UserFactory.CurrentMember.Password2)
            {
                msg = "旧密码不正确";
                result = "false";
                ico = "error";
            }
            else if (string.IsNullOrEmpty(password))
            {
                msg = "新密码不能为空";
                result = "false";
                ico = "error";
            }
            else if (string.IsNullOrEmpty(password2))
            {
                msg = "确认密码不能为空";
                result = "false";
                ico = "error";
            }
            else if (password != password2)
            {
                msg = "两次密码不一致";
                result = "false";
                ico = "error";
            }
            else if (oldpassword == password)
            {
                msg = "新密码不能为新一样";
                result = "false";
                ico = "error";
            }
            if (string.IsNullOrEmpty(msg))
            {
                UserFactory.CurrentMember.Password2 = password;
                if (UserFactory.Update(UserFactory.CurrentMember, null))
                {
                    msg = "更新成功！";
                    result = "true";
                    ico = "success";
                }
                else
                {
                    msg = "更新失败！";
                    result = "false";
                    ico = "error";
                }
            }

        Label_Exit:
            context.Response.ContentType = "application/json";
            context.Response.Write("{\"result\":\"" + result + "\",\"ico\":\"" + ico + "\",\"msg\":\"" + msg + "\",\"url\":\"" + url + "\"}");

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

