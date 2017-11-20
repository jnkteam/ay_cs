namespace OriginalStudio.WebComponents.Web
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model;
    using OriginalStudio.Model.User;
    using System;
    using System.Web;
    using System.Web.SessionState;

    public class UserHandlerBase : IHttpHandler, IRequiresSessionState
    {
        private MchUserBaseInfo _currentUser = null;
        private WebInfo _webinfo = null;

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

        public MchUserBaseInfo CurrentUser
        {
            get
            {
                if (this._currentUser == null)
                {
                    this._currentUser = MchUserFactory.CurrentMember;
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
                return ((this.CurrentUser == null) ? 0 : this.CurrentUser.UserID);
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

