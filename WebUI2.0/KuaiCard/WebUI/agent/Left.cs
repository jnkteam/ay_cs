﻿namespace KuaiCard.WebUI.agent
{
    using OriginalStudio.WebComponents.Web;
    using System;
    using System.Web.UI.HtmlControls;

    public class Left : AgentPageBase
    {
        protected HtmlAnchor a1;
        protected HtmlAnchor a2;
        protected HtmlAnchor a3;
        protected HtmlAnchor a6;
        protected HtmlAnchor a7;
        protected HtmlAnchor a9;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
        }

        private void setPower()
        {
        }
    }
}

