namespace OriginalStudio.WebUI.Manage.Channel
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

    public class ChannelList : ManagePageBase
    {
        protected Button btnAdd;
        protected Button btnSearch;
        protected DropDownList ddlType;
        protected HtmlForm Form1;
        protected GridView GVChannel;

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            base.Response.Redirect("ChannelEdit.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Session["selecttype"] = this.ddlType.SelectedValue;
            this.LoadData();
        }

        protected void GVChannel_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "del")
                {
                    //Code 对应Rules表中name值 进行权限判定
                    if (!this.checkAuthCode("ChannelListDelete")) {
                        base.AlertAndRedirect("权限不足");return;                    
                    }

                    OriginalStudio.BLL.Channel.SysChannel.Delete(Convert.ToInt32(e.CommandArgument));
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
                Literal literal = e.Row.FindControl("litopen") as Literal;
                Literal literalChannelTypeName = e.Row.FindControl("ChannelTypeName") as Literal;

                string str = "<a title='？' style='color:#000' href='javascript:void(0)'> <i class='fa   fa-question-circle'></i></a>";
                if (dataItem["isOpen"] != DBNull.Value)
                {
                    if (Convert.ToInt32(dataItem["isOpen"]) == 0)
                    {
                        str = "<a title='关闭'  style='color:#ff4a4a' href='javascript:void(0)'> <i class='fa  fa-times-circle'></i></a>";
                    }
                    else
                    {
                        str = "<a title='开启' style='color:#1db283' href='javascript:void(0)'> <i class='fa  fa-check-circle'></i></a>";
                    }
                }
                literal.Text =  str;
                string channelTypeName = SysChannelType.GetModelByTypeId(Convert.ToInt32(dataItem["ChannelTypeId"])).TypeName;
                literalChannelTypeName.Text = channelTypeName;

            }
        }

        private void LoadData()
        {
            int result = 0;
            if (!string.IsNullOrEmpty(this.ddlType.SelectedValue))
            {
                int.TryParse(this.ddlType.SelectedValue, out result);
            }
            this.GVChannel.DataSource = OriginalStudio.BLL.Channel.SysChannel.GetList(result);
            this.GVChannel.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //ManageFactory.CheckSecondPwd();
            this.setPower();
            if (!base.IsPostBack)
            {
                this.ddlType.Items.Add(new ListItem("---全部类型---", ""));
                DataTable table = SysChannelType.GetList(null).Tables[0];
                foreach (DataRow row in table.Rows)
                {
                    this.ddlType.Items.Add(new ListItem(row["typename"].ToString(), row["typeId"].ToString()));
                }
                if (!string.IsNullOrEmpty(this.ptypeId))
                {
                    this.ddlType.SelectedValue = this.ptypeId;
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

