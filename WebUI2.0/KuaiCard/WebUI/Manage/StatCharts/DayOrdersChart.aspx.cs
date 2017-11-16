namespace KuaiCard.WebUI.Manage.Stat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.BLL;
    using OriginalStudio.Model;

    public class DayOrdersChart : ManagePageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ManageFactory.CheckSecondPwd();
            setPower();
        }

        private void setPower()
        {
            if (!ManageFactory.CheckCurrentPermission(false, ManageRole.Interfaces))
            {
                base.Response.Write("Sorry,No authority!");
                base.Response.End();
            }
        }
    }
}