namespace KuaiCard.WebUI.Manage
{
    using Aspose.Cells;
    using OriginalStudio.BLL;
    using OriginalStudio.ETAPI;
    using OriginalStudio.Model;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Wuqi.Webdiyer;

    public class Audits : ManagePageBase
    {
        protected Button btnallfail;
        protected Button btnAllPass;
        protected Button btnExport;
        protected Button btnPass;
        protected Button btnSearch;
        protected DropDownList ddlbankName;
        protected DropDownList ddlmode;
        protected DropDownList ddlSupplier;
        protected HtmlForm form1;
        protected HtmlHead Head1;
        protected AspNetPager Pager1;
        protected Repeater rptApply;
        protected TextBox txtAccount;
        protected TextBox txtItemInfoId;
        protected TextBox txtpayeeName;
        protected TextBox txtUserId;

        private void BindData()
        {
            DataSet data = this.GetData();
            this.Pager1.RecordCount = Convert.ToInt32(data.Tables[0].Rows[0][0]);
            DataTable table = data.Tables[1];
            this.rptApply.DataSource = table;
            this.rptApply.DataBind();
        }

        protected void btnallfail_Click(object sender, EventArgs e)
        {
            if (SettledFactory.Allfails())
            {
                base.AlertAndRedirect("操作成功!");
                this.BindData();
            }
            else
            {
                base.AlertAndRedirect("操作失败!");
            }
        }

        protected void btnAllPass_Click(object sender, EventArgs e)
        {
            string batchNo = Guid.NewGuid().ToString("N");
            if (SettledFactory.AllPass(batchNo))
            {
                DataTable listWithdrawByApi = SettledFactory.GetListWithdrawByApi(batchNo);
                if ((listWithdrawByApi != null) && (listWithdrawByApi.Rows.Count > 0))
                {
                    List<SettledInfo> list = SettledFactory.DataTableToList(listWithdrawByApi);
                    foreach (SettledInfo info in list)
                    {
                        KuaiCard.ETAPI.Withdraw.InitDistribution(info);
                    }
                }
                base.AlertAndRedirect("审核成功!");
                this.BindData();
            }
            else
            {
                base.AlertAndRedirect("审核失败!");
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet data = this.GetData();
                if (data != null)
                {
                    DataTable dataTable = data.Tables[1];
                    dataTable.Columns.Add("sName", typeof(string));
                    foreach (DataRow row in dataTable.Rows)
                    {
                        row["sName"] = Enum.GetName(typeof(SettledStatus), row["status"]);
                        row["PayeeBank"] = SettledFactory.GetSettleBankName(row["PayeeBank"].ToString());
                    }
                    dataTable.AcceptChanges();
                    dataTable.TableName = "Rpt";
                    string file = base.Server.MapPath("~/common/template/xls/settle.xls");
                    WorkbookDesigner designer = new WorkbookDesigner();
                    designer.Workbook = new Workbook(file);
                    designer.SetDataSource(dataTable);
                    designer.Process();
                    designer.Workbook.Save(base.Response, DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", ContentDisposition.Attachment, designer.Workbook.SaveOptions);
                }
            }
            catch (Exception exception)
            {
                base.AlertAndRedirect(exception.Message);
            }
        }

        protected void btnPass_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["ischecked"];
            DataTable withdrawListByApi = null;
            string batchNo = Guid.NewGuid().ToString("N");
            if (!string.IsNullOrEmpty(str))
            {
                if (SettledFactory.BatchPass(str, batchNo, out withdrawListByApi))
                {
                    if ((withdrawListByApi != null) && (withdrawListByApi.Rows.Count > 0))
                    {
                        List<SettledInfo> list = SettledFactory.DataTableToList(withdrawListByApi);
                        foreach (SettledInfo info in list)
                        {
                            KuaiCard.ETAPI.Withdraw.InitDistribution(info);
                        }
                    }
                    base.AlertAndRedirect("审核成功!");
                    this.BindData();
                }
                else
                {
                    base.AlertAndRedirect("审核失败!");
                }
            }
            else
            {
                base.AlertAndRedirect("请选择要审核的申请!");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        private DataSet GetData()
        {
            List<SearchParam> searchParams = new List<SearchParam>();
            searchParams.Add(new SearchParam("status", 1));
            int result = 0;
            if (!(string.IsNullOrEmpty(this.txtItemInfoId.Text.Trim()) || !int.TryParse(this.txtItemInfoId.Text.Trim(), out result)))
            {
                searchParams.Add(new SearchParam("id", result));
            }
            if (!(string.IsNullOrEmpty(this.txtUserId.Text.Trim()) || !int.TryParse(this.txtUserId.Text.Trim(), out result)))
            {
                searchParams.Add(new SearchParam("userid", result));
            }
            if (!string.IsNullOrEmpty(this.ddlSupplier.SelectedValue))
            {
                searchParams.Add(new SearchParam("tranapi", int.Parse(this.ddlSupplier.SelectedValue)));
            }
            if (!string.IsNullOrEmpty(this.ddlbankName.SelectedValue))
            {
                searchParams.Add(new SearchParam("payeebank", this.ddlbankName.SelectedValue));
            }
            if (!string.IsNullOrEmpty(this.txtAccount.Text.Trim()))
            {
                searchParams.Add(new SearchParam("account", this.txtAccount.Text.Trim()));
            }
            if (!string.IsNullOrEmpty(this.txtpayeeName.Text.Trim()))
            {
                searchParams.Add(new SearchParam("payeename", this.txtpayeeName.Text.Trim()));
            }
            return SettledFactory.PageSearch(searchParams, this.Pager1.PageSize, this.Pager1.CurrentPageIndex, string.Empty);
        }

        protected string GetTranApiName(object obj)
        {
            if ((obj == null) || (obj == DBNull.Value))
            {
                return "不走接口";
            }
            if (Convert.ToInt32(obj) == 100)
            {
                return "财付通";
            }
            return "";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            if (!base.IsPostBack)
            {
                this.ddlmode.Items.Add(new ListItem("--提现方式--", ""));
                foreach (int num in Enum.GetValues(typeof(SettledmodeEnum)))
                {
                    string name = Enum.GetName(typeof(SettledmodeEnum), num);
                    this.ddlmode.Items.Add(new ListItem(name, num.ToString()));
                }
                DataTable table = SysSupplierFactory.GetList("isdistribution=1").Tables[0];
                this.ddlSupplier.Items.Add(new ListItem("--付款接口--", ""));
                this.ddlSupplier.Items.Add(new ListItem("不走接口", "0"));
                foreach (DataRow row in table.Rows)
                {
                    this.ddlSupplier.Items.Add(new ListItem(row["name"].ToString(), row["code"].ToString()));
                }
                this.BindData();
            }
        }

        protected void Pager1_PageChanged(object sender, EventArgs e)
        {
            this.BindData();
        }

        protected void Pager1_PageChanging(object src, PageChangingEventArgs e)
        {
            this.BindData();
        }

        protected void rptApply_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string str = e.CommandArgument.ToString();
            if (!string.IsNullOrEmpty(str))
            {
                int status = -1;
                if (e.CommandName == "Pass")
                {
                    status = 2;
                }
                else if (e.CommandName == "Refuse")
                {
                    status = 4;
                }
                if (((status != -1) && SettledFactory.Audit(int.Parse(str), status)) && (status == 2))
                {
                    SettledInfo model = SettledFactory.GetModel(int.Parse(str));
                    if (model.status == SettledStatus.付款接口支付中)
                    {
                        KuaiCard.ETAPI.Withdraw.InitDistribution(model);
                    }
                }
            }
            this.BindData();
        }

        protected void rptApply_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string str = DataBinder.Eval(e.Item.DataItem, "status").ToString();
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

        public int ItemInfoId
        {
            get
            {
                return WebBase.GetQueryStringInt32("ID", 0);
            }
        }

        public int ItemInfoStatus
        {
            get
            {
                return WebBase.GetQueryStringInt32("status", 0);
            }
        }
    }
}

