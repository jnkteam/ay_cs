namespace OriginalStudio.WebUI.Manage.User
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Channel;
    using OriginalStudio.Model;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using OriginalStudio.BLL.Settled;
    using OriginalStudio.BLL.Supplier;

    public class UserSupplier : ManagePageBase
    {
        protected Button btnAdd;
        protected Button btnSearch;
        protected DropDownList ddlType;
        protected HtmlForm Form1;
        protected GridView GVChannel;
        protected DropDownList UserID; //商户
        protected DropDownList SupplierCode;//通道
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            //弹窗处理
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void GVChannel_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "del")
                {
                    MchUserSupplierFactory.DeleteUserSupplier(Convert.ToInt32(e.CommandArgument));
                    base.AlertAndRedirect("删除成功");
                }
            }
            catch (Exception exception)
            {
                base.AlertAndRedirect("Error:" + exception.Message);
            }
        }

        protected void GVChannel_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dataItem = e.Row.DataItem as DataRowView;
                Literal lissingle   = e.Row.FindControl("lissingle") as Literal;
                Literal listransfer = e.Row.FindControl("listransfer") as Literal;

                string str = "<a title='？' style='color:#000' href='javascript:void(0)'> <i class='fa   fa-question-circle'></i></a>";
                string str1 = "<a title='？' style='color:#000' href='javascript:void(0)'> <i class='fa   fa-question-circle'></i></a>";

                if (dataItem["issingle"] != DBNull.Value)
                {
                    if (Convert.ToInt32(dataItem["issingle"]) == 0)
                    {
                        str = "<a title='关闭'  style='color:#ff4a4a' href='javascript:void(0)'> <i class='fa  fa-times-circle'></i></a>";
                    }
                    else
                    {
                        str = "<a title='开启' style='color:#1db283' href='javascript:void(0)'> <i class='fa  fa-check-circle'></i></a>";
                    }
                }
                if (dataItem["isTransfer"] != DBNull.Value)
                {
                    if (Convert.ToInt32(dataItem["isTransfer"]) == 0)
                    {
                        str1 = "<a title='否'  style='color:#ff4a4a' href='javascript:void(0)'> <i class='fa  fa-times-circle'></i></a>";
                    }
                    else
                    {
                        str1 = "<a title='是' style='color:#1db283' href='javascript:void(0)'> <i class='fa  fa-check-circle'></i></a>";
                    }
                }
                lissingle.Text =  str;
                listransfer.Text = str1;

               

            }
        }

        private void LoadData()
        {

            int userId   = Convert.ToInt32(this.UserID.SelectedValue);
            int suppCode = Convert.ToInt32(this.SupplierCode.SelectedValue);

            this.GVChannel.DataSource = MchUserSupplierFactory.GetUserSupplierList(userId, suppCode);
            this.GVChannel.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            this.setPower();
            if (!base.IsPostBack)
            {
                
                this.UserID.Items.Add(new ListItem("---商户---", "0"));
                DataTable table = MchUserFactory.GetAllUserList();
                foreach (DataRow row in table.Rows)
                {
                    this.UserID.Items.Add(new ListItem(row["username"].ToString(), row["userid"].ToString()));
                }
                this.SupplierCode.Items.Add(new ListItem("---通道---", "0"));
                DataTable table2 = SysSupplierFactory.GetList(string.Empty).Tables[0];

                foreach (DataRow row in table2.Rows)
                {
                    this.SupplierCode.Items.Add(new ListItem(row["SupplierName"].ToString(), row["SupplierCode"].ToString()));
                }
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

        public string ptypeId
        {
            get
            {
                return WebBase.GetQueryStringString("typeId", "");
            }
        }
    }
}

