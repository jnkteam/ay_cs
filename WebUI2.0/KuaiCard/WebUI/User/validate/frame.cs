namespace OriginalStudio.WebUI.User.validate
{
    using OriginalStudio.BLL;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Text;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class frame : UserPageBase
    {
        protected string email = "";
        protected HtmlForm form1;
        protected string shouji = "";
        protected Label usertext;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.shouji = Strings.Mark(base.CurrentUser.Tel);
            this.email = Strings.Mark(base.CurrentUser.Email);
            if (SysConfig.radioButtonshouji)
            {
                this.usertext.Text = this.usertext.Text + " <li> <a id='#mobile' class='selected' name=\"mobile\"><span>短信验证</span></a></li>";
            }
            if (SysConfig.radioButtonemail)
            {
                this.usertext.Text = this.usertext.Text + "<li><a id='#email' class='selected' name=\"email\"><span>邮箱验证</span></a></li>";
            }
        }
    }
}

