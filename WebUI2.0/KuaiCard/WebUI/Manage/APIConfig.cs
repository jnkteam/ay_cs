namespace OriginalStudio.WebUI.Manage
{
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.WebUI;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class APIConfig : ManagePageBase
    {
        protected DropDownList ddl5173;
        protected DropDownList ddlbankurl;
        protected DropDownList ddldx;
        protected DropDownList ddlisurl;
        protected DropDownList ddljw;
        protected DropDownList ddljy;
        protected DropDownList ddlking;
        protected DropDownList ddllt;
        protected DropDownList ddlmoko;
        protected DropDownList ddlonline;
        protected DropDownList ddlqq;
        protected DropDownList ddlrxk;
        protected DropDownList ddlsd;
        protected DropDownList ddlsh;
        protected DropDownList ddlsms;
        protected DropDownList ddlsxk;
        protected DropDownList ddlszx;
        protected DropDownList ddlwm;
        protected DropDownList ddlwy;
        protected DropDownList ddlzt;
        protected HtmlForm form1;
        protected Button purlok;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                WebUtility.BindBankSupplierDDL(this.ddlbankurl);
                WebUtility.BindCardSupplierDLL(this.ddlisurl);
                WebUtility.BindCardSupplierDLL(this.ddlszx);
                WebUtility.BindCardSupplierDLL(this.ddlsd);
                WebUtility.BindCardSupplierDLL(this.ddlzt);
                WebUtility.BindCardSupplierDLL(this.ddljw);
                WebUtility.BindCardSupplierDLL(this.ddlqq);
                WebUtility.BindCardSupplierDLL(this.ddllt);
                WebUtility.BindCardSupplierDLL(this.ddljy);
                WebUtility.BindCardSupplierDLL(this.ddlwy);
                WebUtility.BindCardSupplierDLL(this.ddlwm);
                WebUtility.BindCardSupplierDLL(this.ddlsh);
                WebUtility.BindCardSupplierDLL(this.ddlonline);
                WebUtility.BindCardSupplierDLL(this.ddlking);
                WebUtility.BindCardSupplierDLL(this.ddlmoko);
                WebUtility.BindCardSupplierDLL(this.ddl5173);
                WebUtility.BindCardSupplierDLL(this.ddlrxk);
                WebUtility.BindCardSupplierDLL(this.ddldx);
                WebUtility.BindSMSSupplierDLL(this.ddlsms);
                WebUtility.BindSXSupplierDLL(this.ddlsxk);
            }
        }

        protected void purlok_Click(object sender, EventArgs e)
        {
        }
    }
}

