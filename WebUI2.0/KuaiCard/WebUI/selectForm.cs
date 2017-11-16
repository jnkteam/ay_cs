namespace OriginalStudio.WebUI
{
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model.User;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class selectForm : Page
    {
        private UserInfo _user = null;
        protected Label lituserName;

        private void InitForm()
        {
            string str = string.Empty;
            if (this.userInfo != null)
            {
                if ((this.userInfo.IsPhonePass == 1) && (this.userInfo.IsEmailPass == 1))
                {
                    str = "<tr>\r\n\t\t\t\t\t<td class=\"email\">\r\n\t\t\t\t\t\t<div>\r\n\t\t\t\t\t\t\t<a href=\"/emailForm.aspx\">通过邮箱找回</a>\r\n\t\t\t\t\t\t</div>\r\n\t\t\t\t\t</td>\r\n\t\t\t\t\t<td class=\"mobile\">\r\n\t\t\t\t\t\t<div>\r\n\t\t\t\t\t\t\t<a href=\"/mobileForm.aspx\">通过短信找回</a>\r\n\t\t\t\t\t\t</div>\r\n\t\t\t\t\t</td>\r\n\t\t\t\t</tr>";
                }
                else if (this.userInfo.IsPhonePass == 1)
                {
                    str = "<tr>\r\n\t\t\t\t\t\r\n\t\t\t\t\t<td class=\"mobile\">\r\n\t\t\t\t\t\t<div>\r\n\t\t\t\t\t\t\t<a href=\"/mobileForm.aspx\">通过短信找回</a>\r\n\t\t\t\t\t\t</div>\r\n\t\t\t\t\t</td>\r\n\t\t\t\t</tr>";
                }
                else if (this.userInfo.IsEmailPass == 1)
                {
                    str = "<tr>\r\n\t\t\t\t\t<td class=\"email\">\r\n\t\t\t\t\t\t<div>\r\n\t\t\t\t\t\t\t<a href=\"/emailForm.aspx\">通过邮箱找回</a>\r\n\t\t\t\t\t\t</div>\r\n\t\t\t\t\t</td>\r\n\t\t\t\t</tr>";
                }
                this.lituserName.Text = str;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                if (this.userInfo == null)
                {
                    base.Response.Redirect("getPassword.html");
                }
                this.InitForm();
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

