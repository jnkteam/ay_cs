﻿namespace KuaiCard.WebUI.Manage
{
    using KuaiCard.WebComponents.Web;
    using System;

    public class Top : ManagePageBase
    {
        protected string username;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            if (!base.IsPostBack)
            {
                this.username = base.currentManage.username;
            }
        }

        private void setPower()
        {
        }
    }
}

