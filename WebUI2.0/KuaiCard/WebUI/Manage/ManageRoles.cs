namespace OriginalStudio.WebUI.Manage
{
    using OriginalStudio.BLL;
    using OriginalStudio.Model;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Security;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ManageRoles : ManagePageBase
    {
        protected DataTable ManageRolesTable = null;
        protected GridView GridView;
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet set = ManageRolesFactory.GetList(null);
            if (set.Tables.Count != 0)
            {
                ManageRolesTable = set.Tables[0];
            }
            ManageRolesTable.Columns.Add("statusStr");
            foreach (DataRow row in ManageRolesTable.Rows)
            {
                switch (row["status"].ToString())
                {
                    case "0":
                        row["statusStr"] = "<a title='禁用'  style='color:#ff4a4a' href='javascript:void(0)'> <i class='fa  fa-times-circle'></i></a>";
                        break;

                    case "1":
                        row["statusStr"] = "<a title='启用' style='color:#1db283' href='javascript:void(0)'> <i class='fa  fa-check-circle'></i></a>";
                        break;
                }
                
            }
            this.GridView.DataSource = ManageRolesTable;
            this.GridView.DataBind();

        }
       

    }
}

