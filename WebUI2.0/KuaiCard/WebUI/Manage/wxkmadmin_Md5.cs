namespace KuaiCard.WebUI.Manage
{
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Security;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class wxkmadmin_Md5 : ManagePageBase
    {
        protected Button btn_Update;
        protected DropDownList ddlencode;
        protected HtmlForm form1;
        protected TextBox txtMd5Str;
        protected TextBox txtresult;

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtMd5Str.Text.Trim()))
            {
                base.AlertAndRedirect("请输入要加密的字符串");
            }
            else
            {
                string strToEncrypt = this.txtMd5Str.Text.Trim();
                this.txtresult.Text = Cryptography.MD5(strToEncrypt, this.ddlencode.SelectedValue);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}

