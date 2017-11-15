namespace KuaiCard.WebUI.Manage.Stat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using KuaiCard.WebComponents.Web;
    using KuaiCard.BLL;
    using KuaiCard.Model;

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