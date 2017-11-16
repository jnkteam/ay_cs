namespace OriginalStudio.WebUI.merchant.ajax
{
    using OriginalStudio.BLL.Tools;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model.User;
    using System;
    using System.Web;
    using System.Web.SessionState;

    public class DPI3 : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string s = "";
            if (this.currentUser == null)
            {
                s = "登录信息失效，请重新登录";
            }
            else
            {
                IdcardInfo info = context.Session["IDCard_" + this.currentUser.ID.ToString()] as IdcardInfo;
                if (info != null)
                {
                    this.currentUser.full_name = info.fullname;
                    this.currentUser.IdCard = info.code;
                    this.currentUser.addtress = info.location;
                    this.currentUser.male = info.gender;
                    this.currentUser.IsRealNamePass = 1;
                    if (UserFactory.Update(this.currentUser, null))
                    {
                        s = "true";
                    }
                    else
                    {
                        s = "认证失败";
                    }
                }
                context.Response.ContentType = "text/plain";
                context.Response.Write(s);
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

