﻿namespace KuaiCard.WebUI.Manage.Channel
{
    using KuaiCard.BLL;
    using KuaiCard.BLL.Channel;
    using KuaiCard.Model;
    using KuaiCard.Model.Channel;
    using KuaiCard.WebComponents.Web;
    using KuaiCardLib.Web;
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
            ManageFactory.CheckSecondPwd();
            this.setPower();
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

