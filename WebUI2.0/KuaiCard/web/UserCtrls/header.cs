namespace KuaiCard.web.UserCtrls
{
    using System;
    using System.Web.UI;

    public class header : UserControl
    {
        protected string contactclass = string.Empty;
        protected string contactclass1 = string.Empty;
        protected string indexclass = string.Empty;
        protected string indexclass1 = string.Empty;
        protected string introdclass = string.Empty;
        protected string newsclass = string.Empty;
        protected string productclass = string.Empty;
        protected string productclass1 = string.Empty;
        protected string searchclass = string.Empty;
        protected string solutionclass = string.Empty;
        protected string solutionclass1 = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.showtype == "index")
            {
                this.indexclass = "class=\"b-over-font\"";
                this.indexclass1 = "class=\"b-over\"";
            }
            else if (this.showtype == "product")
            {
                this.productclass = "class=\"b-over-font\"";
                this.productclass1 = "class=\"b-over\"";
            }
            else if (this.showtype == "solution")
            {
                this.solutionclass = "class=\"b-over-font\"";
                this.solutionclass1 = "class=\"b-over\"";
            }
            else if (this.showtype == "contact")
            {
                this.contactclass = "class=\"b-over-font\"";
                this.contactclass1 = "class=\"b-over\"";
            }
            else if (this.showtype == "news")
            {
                this.newsclass = "class=\"b-over-font\"";
            }
            else if (this.showtype == "introd")
            {
                this.introdclass = "class=\"b-over-font\"";
            }
            else if (this.showtype == "search")
            {
                this.searchclass = "class=\"b-over-font\"";
            }
        }

        public string showtype
        {
            get
            {
                if (this.ViewState["UserCtrls_header_showtype"] == null)
                {
                    return "index";
                }
                return this.ViewState["UserCtrls_header_showtype"].ToString();
            }
            set
            {
                this.ViewState["UserCtrls_header_showtype"] = value;
            }
        }
    }
}

