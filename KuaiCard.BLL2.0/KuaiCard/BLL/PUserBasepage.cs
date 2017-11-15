namespace KuaiCard.BLL
{
    using KuaiCard.Model.User;
    using KuaiCardLib;
    using System;
    using System.Web;
    using System.Web.UI;

    public class PUserBasepage : Page
    {
        private int _userid = 0;
        private UserStatusEnum _userstatus = UserStatusEnum.待审核;

        public void CheckLogin()
        {
            if (!this.IsLogin())
            {
                string url = string.Empty;
                if (!HttpContext.Current.Request.Url.ToString().ToLower().Contains("Logout.aspx"))
                {
                    url = XRequest.GetUrl();
                }
                else
                {
                    url = "index.html";
                }
                HttpContext.Current.Response.Write("<script>window.location='/Pman/Login.aspx';</script>");
                HttpContext.Current.Response.End();
            }
        }

        public bool IsLogin()
        {
            bool flag = false;
            if (((HttpContext.Current.Session["UserId"] != null) && (HttpContext.Current.Session["UserStatus"] != null)) && (int.Parse(HttpContext.Current.Session["group"].ToString()) == 2))
            {
                flag = true;
            }
            return flag;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.IsLogin())
            {
                this._userid = int.Parse(HttpContext.Current.Session["UserId"].ToString());
                this._userstatus = (UserStatusEnum) int.Parse(HttpContext.Current.Session["UserStatus"].ToString());
            }
            else
            {
                this.CheckLogin();
            }
        }

        public int UserId
        {
            get
            {
                return this._userid;
            }
            set
            {
                this._userid = value;
            }
        }

        public UserStatusEnum UserStatus
        {
            get
            {
                return this._userstatus;
            }
            set
            {
                this._userstatus = value;
            }
        }
    }
}

