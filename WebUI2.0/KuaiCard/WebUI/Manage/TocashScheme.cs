namespace OriginalStudio.WebUI.Manage
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model;
    using OriginalStudio.Model.Settled;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using OriginalStudio.BLL.Withdraw;
    using OriginalStudio.Model.Withdraw;
    using OriginalStudio.BLL.Supplier;

    public class TocashScheme : ManagePageBase
    {
        public WithdrawSchemeInfo _ItemInfo = null;
        protected Button btnAdd;
        protected HtmlForm form1;
        protected GridView GridView1;
        protected HtmlHead Head1;

        private void BindView()
        {
            

            DataTable table = WithdrawSchemeFactory.GetList("type=1").Tables[0];
            this.GridView1.DataSource = table;
            this.GridView1.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            base.Response.Redirect("TocashSchemeModi.aspx");
        }
        protected void GVChannel_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dataItem = e.Row.DataItem as DataRowView;
                Literal transupplierNameLiteral = e.Row.FindControl("transupplierName") as Literal;
                Literal isdefaultIconLiteral = e.Row.FindControl("isdefaultIcon") as Literal;

                string str = "<a title='？' style='color:#000' href='javascript:void(0)'> <i class='fa   fa-question-circle'></i></a>";
                if (dataItem["IsDefault"] != DBNull.Value)
                {
                    if (Convert.ToInt32(dataItem["IsDefault"]) == 0)
                    {
                        str = "<a title=''  style='color:#ff4a4a' href='javascript:void(0)'> <i class='fa  fa-times-circle'></i></a>";
                    }
                    else
                    {
                        str = "<a title='' style='color:#1db283' href='javascript:void(0)'> <i class='fa  fa-check-circle'></i></a>";
                    }
                }
                isdefaultIconLiteral.Text = str;
                string Name = SysSupplierFactory.GetSupplierModelByCode(Convert.ToInt32(dataItem["TranSupplier"])).SupplierName;
                transupplierNameLiteral.Text = Name;

            }
        }
        private void DoCmd()
        {
            if (this.isDel)
            {
                if (WithdrawSchemeFactory.Delete(this.ItemInfoId))
                {
                    base.AlertAndRedirect("删除成功!", "TocashSchemes.aspx?sign=19&menuId=25");
                }
                else
                {
                    base.AlertAndRedirect("删除失败!", "TocashSchemes.aspx?sign=19&menuId=25");
                }
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.GridView1.PageIndex = e.NewPageIndex;
            this.BindView();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            this.DoCmd();
            if (!base.IsPostBack)
            {
                this.BindView();
            }
        }

        private void setPower()
        {
            if (!ManageFactory.CheckCurrentPermission(false, ManageRole.Financial))
            {
                base.Response.Write("Sorry,No authority!");
                base.Response.End();
            }
        }

        public string Action
        {
            get
            {
                return WebBase.GetQueryStringString("cmd", "");
            }
        }

        public bool isDel
        {
            get
            {
                return ((this.ItemInfoId > 0) && (this.Action == "del"));
            }
        }

        public bool isUpdate
        {
            get
            {
                return ((this.ItemInfoId > 0) && (this.Action == "edit"));
            }
        }

        public WithdrawSchemeInfo ItemInfo
        {
            get
            {
                if (this._ItemInfo == null)
                {
                    if (this.isUpdate)
                    {
                        this._ItemInfo = WithdrawSchemeFactory.GetModel(this.ItemInfoId);
                    }
                    else
                    {
                        this._ItemInfo = new WithdrawSchemeInfo();
                    }
                }
                return this._ItemInfo;
            }
        }

        public int ItemInfoId
        {
            get
            {
                return WebBase.GetQueryStringInt32("id", 0);
            }
        }
    }
}

