namespace KuaiCard.WebUI.User.Pincard
{
    using KuaiCard.WebComponents.Web;
    using System;
    using System.Web.UI.WebControls;

    public class index : UserPageBase
    {
        public string defaultvalue = string.Empty;
        protected HiddenField xk_faceValue;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.defaultvalue = "100";
            this.xk_faceValue.Value = this.defaultvalue;
        }
    }
}

