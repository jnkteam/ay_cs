namespace OriginalStudio.WebUI
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model.User;
    using OriginalStudio.Lib.Security;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using OriginalStudio.Model.News;
    using OriginalStudio.BLL.News;

    public class setForm : Page
    {
        private UserInfo _user = null;
        protected HtmlHead Head1;
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
                if (this.username == "")
                {
                    base.Response.Redirect("getPassword.html");
                }
                else if (this.userInfo == null)
                {
                    base.Response.Redirect("getPassword.html");
                }
                else if (this.renzheng == "")
                {
                    base.Response.Redirect("getPassword.html");
                }
                else if (this.Session["findpwduserok"].ToString() != Cryptography.MD5("yanzhengtongguook", "GB2312"))
                {
                    base.Response.Redirect("getPassword.html");
                }
            }
        }

        public string renzheng
        {
            get
            {
                if (this.Session["findpwduserok"] != null)
                {
                    return this.Session["findpwduserok"].ToString();
                }
                return string.Empty;
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

