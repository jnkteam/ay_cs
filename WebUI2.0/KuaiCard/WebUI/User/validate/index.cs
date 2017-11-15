namespace KuaiCard.WebUI.User.validate
{
    using KuaiCard.WebComponents.Web;
    using KuaiCardLib.Text;
    using System;
    using System.Web;
    using System.Web.UI.WebControls;

    public class index : UserPageBase
    {
        protected HiddenField sendtime;
        public string sign = string.Empty;
        protected HiddenField userid;
        public string yzname = string.Empty;
        protected Label yznameid;

        private void InitForm()
        {
            if (this.username == "mobile")
            {
                this.sign = "modifyShouji";
                this.yzname = "手机";
                this.yznameid.Text = Strings.Mark(base.currentUser.Tel);
                this.sendtime.Value = "mobile";
            }
            if (this.username == "mobilecode")
            {
                this.sign = "mobilecode";
                this.yzname = "手机";
                this.yznameid.Text = Strings.Mark(base.currentUser.Tel);
                this.sendtime.Value = "mobilecode";
            }
            if (this.username == "email")
            {
                this.sign = "modifyEmail";
                this.yzname = "邮箱";
                this.yznameid.Text = Strings.Mark(base.currentUser.Email);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.username))
            {
                HttpContext.Current.Response.Redirect("/user/");
            }
            else
            {
                this.userid.Value = base.currentUser.ID.ToString();
                if (!base.IsPostBack)
                {
                    this.InitForm();
                }
            }
        }

        public string username
        {
            get
            {
                if (HttpContext.Current.Session["codeSession"] != null)
                {
                    return HttpContext.Current.Session["codeSession"].ToString();
                }
                return string.Empty;
            }
        }
    }
}

