namespace KuaiCard.WebUI.Manage
{
    using OriginalStudio.BLL;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Web.UI.HtmlControls;

    public class login2 : ManagePageBase
    {
        protected HtmlInputButton btnOk;
        protected HtmlForm form1;
        protected HtmlGenericControl lblMessage;
        protected HtmlInputPassword txtPsec;

        protected void btnOk_ServerClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtPsec.Value.Trim()))
            {
                this.lblMessage.InnerText = "请输入密码";
            }
            else if (ManageFactory.SecPwdVaild(this.txtPsec.Value.Trim()))
            {
                base.Response.Redirect(this.RedirectUrl);
            }
            else
            {
                this.lblMessage.InnerText = "密码错误";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public string RedirectUrl
        {
            get
            {
                return WebBase.GetQueryStringString("RedirectUrl", "");
            }
        }
    }
}

