namespace OriginalStudio.WebUI.User.validate
{
    using OriginalStudio.WebComponents.Web;
    using System;

    public class set : UserPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.CurrentUser.IsEmailPass == 1)
            {
                base.AlertAndRedirect("邮箱已经认证", "/User/");
            }
        }
    }
}

