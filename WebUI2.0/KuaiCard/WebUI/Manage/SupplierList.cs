namespace KuaiCard.WebUI.Manage
{
    using KuaiCard.BLL;
    using KuaiCard.Model;
    using KuaiCard.WebComponents.Web;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class SupplierList : ManagePageBase
    {
        protected Button btnAdd;
        protected HtmlForm Form1;
        protected GridView GVSupplier;

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            base.Response.Redirect("SupplierEdit.aspx", true);
        }

        private void LoadData()
        {
            string where = "release = 1";
            this.GVSupplier.DataSource = SupplierFactory.GetList(where);
            this.GVSupplier.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ManageFactory.CheckSecondPwd();
            this.setPower();
            if (!base.IsPostBack)
            {
                this.LoadData();
            }
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

