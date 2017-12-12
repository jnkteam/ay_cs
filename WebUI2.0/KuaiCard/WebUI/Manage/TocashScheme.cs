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

    public class TocashScheme : ManagePageBase
    {
        public TocashSchemeInfo _ItemInfo = null;
        protected Button btnAdd;
        protected HtmlForm form1;
        protected GridView GridView1;
        protected HtmlHead Head1;

        private void BindView()
        {
            DataTable table = OriginalStudio.BLL.User.TocashScheme.GetList("type=1").Tables[0];
            this.GridView1.DataSource = table.DefaultView;
            this.GridView1.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            base.Response.Redirect("TocashSchemeModi.aspx");
        }

        private void DoCmd()
        {
            if (this.isDel)
            {
                if (OriginalStudio.BLL.User.TocashScheme.Delete(this.ItemInfoId))
                {
                    base.AlertAndRedirect("删除成功!", "TocashSchemes.aspx");
                }
                else
                {
                    base.AlertAndRedirect("删除失败!", "TocashSchemes.aspx");
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

        public TocashSchemeInfo ItemInfo
        {
            get
            {
                if (this._ItemInfo == null)
                {
                    if (this.isUpdate)
                    {
                        this._ItemInfo = OriginalStudio.BLL.User.TocashScheme.GetModel(this.ItemInfoId);
                    }
                    else
                    {
                        this._ItemInfo = new TocashSchemeInfo();
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

