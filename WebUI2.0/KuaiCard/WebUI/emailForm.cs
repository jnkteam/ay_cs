namespace OriginalStudio.WebUI
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Settled;
    using OriginalStudio.Model.User;
    using OriginalStudio.Lib.Text;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using OriginalStudio.BLL.News;
    using OriginalStudio.Model.News;

    public class emailForm : Page
    {
        private UserInfo _user = null;
        protected HtmlHead Head1;
        protected string mobile = "";
        protected string mobile2 = "";
        protected Repeater rptNews1;

        protected void Page_Load(object sender, EventArgs e)
        {
            List<NewsInfo> list = NewsFactory.GetCacheList(4, 1, 6);
            if (list != null)
            {
                this.rptNews1.DataSource = list;
                this.rptNews1.DataBind();
            }
            if (!base.IsPostBack)
            {
                if (this.userInfo == null)
                {
                    base.Response.Redirect("/getPassword.html");
                }
                this.mobile = this.userInfo.Email;
                this.mobile2 = Strings.Mark(this.userInfo.Email);
            }
        }

        public UserInfo userInfo
        {
            get
            {
                if (!(string.IsNullOrEmpty(this.username) || (this._user != null)))
                {
                    this._user = UserFactory.GetModelByName(this.username);
                }
                return this._user;
            }
        }

        public string username
        {
            get
            {
                if (this.Session["findpwduser"] != null)
                {
                    return this.Session["findpwduser"].ToString();
                }
                return string.Empty;
            }
        }
    }
}

