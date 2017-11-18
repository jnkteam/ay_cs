namespace OriginalStudio.WebUI.User.profile
{
    using OriginalStudio.WebComponents.Web;
    using System;
    using System.Web.UI.WebControls;

    public class index : UserPageBase
    {
        protected string hostName = string.Empty;
        protected string hostUrl = "http://";
        protected string litFullName = string.Empty;
        protected Literal litphone;
        protected string litqq = string.Empty;
        protected string littext = string.Empty;
        protected string littextmai = string.Empty;
        protected Literal litUserEmail;
        protected Literal userid;

        private void InitForm()
        {
            this.userid.Text = base.CurrentUser.ID.ToString();
            this.litqq = base.CurrentUser.QQ;
            this.litUserEmail.Text = base.UserViewEmail;
            this.litFullName = base.CurrentUser.full_name;
            if (base.CurrentUser.IsPhonePass == 0)
            {
                this.litphone.Text = "未绑定";
                this.littext = "绑定";
            }
            else
            {
                this.litphone.Text = base.CurrentUser.Tel;
                this.littext = "修改";
            }
            if (base.CurrentUser.IsEmailPass == 0)
            {
                this.litUserEmail.Text = "未绑定";
                this.littextmai = "绑定";
            }
            else
            {
                this.litUserEmail.Text = base.UserViewEmail;
                this.littextmai = "修改";
            }
            this.hostName = base.CurrentUser.SiteName;
            if (!string.IsNullOrEmpty(base.CurrentUser.SiteUrl))
            {
                this.hostUrl = base.CurrentUser.SiteUrl;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.InitForm();
            }
        }
    }
}

