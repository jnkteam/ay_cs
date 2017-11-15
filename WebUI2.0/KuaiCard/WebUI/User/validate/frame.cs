namespace KuaiCard.WebUI.User.validate
{
    using KuaiCard.BLL;
    using KuaiCard.WebComponents.Web;
    using KuaiCardLib.Text;
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
            this.shouji = Strings.Mark(base.currentUser.Tel);
            this.email = Strings.Mark(base.currentUser.Email);
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

