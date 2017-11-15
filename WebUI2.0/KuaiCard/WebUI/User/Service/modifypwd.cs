namespace KuaiCard.WebUI.User.Service
{
    using KuaiCard.BLL.User;
    using KuaiCardLib.Security;
    using System;
    using System.Web;
    using System.Web.SessionState;

    public class modifypwd : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string str = "失败";
            string str2 = "false";
            string str3 = context.Request["oldpassword"];
            string strToEncrypt = context.Request["password"];
            string str5 = context.Request["password2"];
            string str6 = context.Request["PWDID"];
            if (str6 != null)
            {
                if (!(str6 == "xiugai"))
                {
                    if (str6 == "zengjia")
                    {
                        if ((strToEncrypt == "") || (strToEncrypt == null))
                        {
                            str = "密码不能为空!";
                            str2 = "false";
                        }
                        else if ((str5 == "") || (str5 == null))
                        {
                            str = "确认密码不能为空1";
                            str2 = "false";
                        }
                        else if (strToEncrypt != str5)
                        {
                            str = "两次密码不一致";
                            str2 = "false";
                        }
                        UserFactory.CurrentMember.Password2 = Cryptography.MD5(strToEncrypt);
                        if (UserFactory.CurrentMember.Password2 == UserFactory.CurrentMember.Password)
                        {
                            str = "登录密码与提现密码不能相同";
                        }
                        else if (UserFactory.Update(UserFactory.CurrentMember, null))
                        {
                            str = "设置成功！";
                            str2 = "true";
                        }
                        else
                        {
                            str = "更新失败！";
                            str2 = "false";
                        }
                    }
                }
                else
                {
                    if (strToEncrypt != UserFactory.CurrentMember.Password2)
                    {
                        str = "旧密码不正确";
                        str2 = "false";
                        return;
                    }
                    if ((strToEncrypt == "") || (strToEncrypt == null))
                    {
                        str = "密码不能为空!";
                        str2 = "false";
                        return;
                    }
                    str6 = str5;
                    if ((str6 == null) || (str6 == ""))
                    {
                        str = "确认密码不能为空1";
                        str2 = "false";
                        return;
                    }
                    if (strToEncrypt != str5)
                    {
                        str = "两次密码不一致";
                        str2 = "false";
                        return;
                    }
                    UserFactory.CurrentMember.Password2 = strToEncrypt;
                    if (UserFactory.CurrentMember.Password2 == UserFactory.CurrentMember.Password)
                    {
                        str = "登录密码与提现密码不能相同";
                    }
                    else if (UserFactory.Update(UserFactory.CurrentMember, null))
                    {
                        str = "设置成功！";
                        str2 = "true";
                    }
                    else
                    {
                        str = "更新失败！";
                        str2 = "false";
                    }
                }
            }
            context.Response.ContentType = "application/json";
            context.Response.Write("{\"result\":" + str2 + ",\"text\":\"" + str + "\"}");
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

