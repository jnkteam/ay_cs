namespace OriginalStudio.WebUI.Manage
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Supplier;
    using OriginalStudio.Model;
    using OriginalStudio.WebComponents.Web;
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
            this.GVSupplier.DataSource = SysSupplierFactory.GetList(where);
            this.GVSupplier.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //ManageFactory.CheckSecondPwd();
            //this.setPower();
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

