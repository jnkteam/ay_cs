namespace OriginalStudio.WebUI.merchant.ajax
{
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model.User;
    using OriginalStudio.Lib.Text;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.SessionState;

    public class DPI1 : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string message = "";
            if (this.currentUser == null)
            {
                message = "登录信息失效，请重新登录";
            }
            else
            {
                try
                {
                    string queryStringString = WebBase.GetQueryStringString("pernumber", string.Empty);
                    string str3 = WebBase.GetQueryStringString("personName", string.Empty);
                    string str4 = WebBase.GetQueryStringString("action", string.Empty);
                    string regularString = Regular.GetRegularString(RegularType.ChineseIDCard, 0x12);
                    if (!Regex.IsMatch(queryStringString, regularString))
                    {
                        message = "请输入正确的身份证号码 ";
                    }
                    else
                    {
                        string str6;
                        string str7;
                        IdcardInfo info = new IdcardInfo();
                        info.code = queryStringString;
                        if (OriginalStudio.BLL.basedata.identitycard.GetBirthdayAndSex(queryStringString, out str6, out str7))
                        {
                            info.birthday = str6;
                            info.gender = str7;
                            OriginalStudio.Model.basedata.identitycard model = OriginalStudio.BLL.basedata.identitycard.GetModel(queryStringString.Substring(0, 6));
                            if (model == null)
                            {
                                info = null;
                            }
                            else
                            {
                                info.location = model.DQ;
                            }
                        }
                        else
                        {
                            info = null;
                        }
                        if (info == null)
                        {
                            message = "无效身份信息。请重新输入。";
                        }
                        else
                        {
                            if (str4 == "modiname")
                            {
                                if (string.IsNullOrEmpty(str3))
                                {
                                    message = "请输入真实姓名";
                                }
                                else
                                {
                                    info.fullname = str3;
                                }
                            }
                            else
                            {
                                info.fullname = this.currentUser.full_name;
                            }
                            if (string.IsNullOrEmpty(message))
                            {
                                message = "true";
                                context.Session["IDCard_" + this.currentUser.ID.ToString()] = info;
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    message = exception.Message;
                }
                context.Response.ContentType = "text/plain";
                context.Response.Write(message);
            }
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

