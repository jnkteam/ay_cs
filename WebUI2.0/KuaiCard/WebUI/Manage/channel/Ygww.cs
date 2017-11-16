namespace OriginalStudio.WebUI.Manage.Channel
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Channel;
    using OriginalStudio.Model;
    using OriginalStudio.Model.Channel;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Ygww : ManagePageBase
    {
        protected HtmlForm Form1;
        protected Button btnGetBalance;

        protected void Page_Load(object sender, EventArgs e)
        {
            //ManageFactory.CheckSecondPwd();
            //this.setPower();
        }

        private void setPower()
        {
            if (!ManageFactory.CheckCurrentPermission(false, ManageRole.Interfaces))
            {
                base.Response.Write("Sorry,No authority!");
                base.Response.End();
            }
        }

        protected void btnGetBalance_Click(object sender, EventArgs e)
        {
        }

    }
}

