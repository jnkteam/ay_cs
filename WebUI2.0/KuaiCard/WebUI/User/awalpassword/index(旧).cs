namespace OriginalStudio.WebUI.User.awalpassword
{
    using OriginalStudio.WebComponents.Web;
    using System;
    using System.Web.UI.WebControls;

    public class index_old : UserPageBase
    {
        protected string gUserID = string.Empty;
        protected Label pwdcss;
        protected HiddenField PWDID;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.gUserID = base.CurrentUser.ID.ToString();
            if ((base.CurrentUser.Password2 != "") || (base.CurrentUser.Password2 != null))
            {
                this.PWDID.Value = "xiugai";
                this.pwdcss.Text = "<tr>\r\n        <th><font style=\"color:#F00\">*</font>原密码</th>\r\n        <td><div class=\"col-lg-2\"><input type=\"password\" name=\"oldpassword\" class=\"form-control m-b-10\" /></div></td>\r\n      </tr>\r\n      <tr>\r\n        <th><font style=\"color:#F00\">*</font>新密码</th>\r\n        <td><div class=\"col-lg-2\"><input type=\"password\" name=\"password\" class=\"form-control m-b-10\" /></div></td>\r\n      </tr>\r\n      <tr class=\"td1\">\r\n        <th><font style=\"color:#F00\">*</font>确认新密码</th>\r\n        <td><div class=\"col-lg-2\"><input type=\"password\" name=\"password2\" class=\"form-control m-b-10\" /></div></td>\r\n      </tr>";
            }
            if ((base.CurrentUser.Password2 == "") || (base.CurrentUser.Password2 == null))
            {
                this.PWDID.Value = "zengja";
                this.pwdcss.Text = "<tr>\r\n        <th><font style=\"color:#F00\">*</font>密码</th>\r\n        <td><div class=\"col-lg-2\"><input type=\"password\" name=\"password\" class=\"form-control m-b-10\" /></div></td>\r\n      </tr>\r\n      \r\n      <tr>\r\n        <th><font style=\"color:#F00\">*</font>确认密码</th>\r\n        <td><div class=\"col-lg-2\"><input type=\"password\" name=\"password2\" class=\"form-control m-b-10\" /></div></td>\r\n      </tr>";
            }
        }
    }
}

