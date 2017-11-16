namespace KuaiCard.WebUI.User.awalpassword
{
    using OriginalStudio.WebComponents.Web;
    using System;
    using System.Web.UI.WebControls;

    public class index : UserPageBase
    {
        protected string gUserID = string.Empty;
        protected Boolean gUpdateMode = true;       //new:新增; update:修改

        protected HiddenField hidMode;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.gUserID = base.currentUser.ID.ToString();

            if ((base.currentUser.Password2 == "") || (base.currentUser.Password2 == null))
                this.gUpdateMode = false;
            else
                this.gUpdateMode = true;

            //if ((base.currentUser.Password2 != "") || (base.currentUser.Password2 != null))
            //{
            //    this.PWDID.Value = "xiugai";
            //    this.pwdcss.Text = "<tr>\r\n        <th><font style=\"color:#F00\">*</font>原密码</th>\r\n        <td><div class=\"col-lg-2\"><input type=\"password\" name=\"oldpassword\" class=\"form-control m-b-10\" /></div></td>\r\n      </tr>\r\n      <tr>\r\n        <th><font style=\"color:#F00\">*</font>新密码</th>\r\n        <td><div class=\"col-lg-2\"><input type=\"password\" name=\"password\" class=\"form-control m-b-10\" /></div></td>\r\n      </tr>\r\n      <tr class=\"td1\">\r\n        <th><font style=\"color:#F00\">*</font>确认新密码</th>\r\n        <td><div class=\"col-lg-2\"><input type=\"password\" name=\"password2\" class=\"form-control m-b-10\" /></div></td>\r\n      </tr>";
            //}
            //if ()
            //{
            //    this.PWDID.Value = "zengja";
            //    this.pwdcss.Text = "<tr>\r\n        <th><font style=\"color:#F00\">*</font>密码</th>\r\n        <td><div class=\"col-lg-2\"><input type=\"password\" name=\"password\" class=\"form-control m-b-10\" /></div></td>\r\n      </tr>\r\n      \r\n      <tr>\r\n        <th><font style=\"color:#F00\">*</font>确认密码</th>\r\n        <td><div class=\"col-lg-2\"><input type=\"password\" name=\"password2\" class=\"form-control m-b-10\" /></div></td>\r\n      </tr>";
            //}
        }
    }
}

