namespace OriginalStudio.WebUI.Business
{
    using OriginalStudio.WebComponents.Web;
    using System;
    using System.Web.UI.HtmlControls;

    public class Left : BusinessPageBase
    {
        protected HtmlAnchor a1;
        protected HtmlAnchor a10;
        protected HtmlAnchor a2;
        protected HtmlAnchor a3;
        protected HtmlAnchor a4;
        protected HtmlAnchor a5;
        protected HtmlAnchor a8;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
        }

        private void setPower()
        {
        }
    }
}

