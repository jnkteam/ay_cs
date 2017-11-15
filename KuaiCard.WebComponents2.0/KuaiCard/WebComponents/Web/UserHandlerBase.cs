namespace KuaiCard.WebComponents.Web
{
    using KuaiCard.BLL;
    using KuaiCard.BLL.User;
    using KuaiCard.Model;
    using KuaiCard.Model.User;
    using System;
    using System.Web;
    using System.Web.SessionState;

    public class UserHandlerBase : IHttpHandler, IRequiresSessionState
    {
        private UserInfo _currentUser = null;
        private WebInfo _webinfo = null;

        public string GetValue(string param)
        {
            string str = string.Empty;
            try
            {
                str = (HttpContext.Current.Request.Form[param] == null) ? ((HttpContext.Current.Request.QueryString[param] == null) ? "" : HttpContext.Current.Request.QueryString[param]) : HttpContext.Current.Request.Form[param];
            }
            catch (Exception)
            {
            }
            return str;
        }

        public virtual void OnLoad(HttpContext context)
        {
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context.Session == null)
            {
                context.Response.StatusCode = 0x195;
                context.Response.End();
            }
            else if (!this.IsLogin)
            {
                context.Response.StatusCode = 0x195;
                context.Response.End();
            }
            this.OnLoad(context);
        }

        public UserInfo CurrentUser
        {
            get
            {
                if (this._currentUser == null)
                {
                    this._currentUser = UserFactory.CurrentMember;
                }
                return this._currentUser;
            }
        }

        public bool IsLogin
        {
            get
            {
                return (this.CurrentUser != null);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public int UserId
        {
            get
            {
                return ((this.CurrentUser == null) ? 0 : this.CurrentUser.ID);
            }
        }

        public WebInfo WebSiteInfo
        {
            get
            {
                if (this._webinfo == null)
                {
                    this._webinfo = WebInfoFactory.CurrentWebInfo;
                }
                return this._webinfo;
            }
        }
    }
}

