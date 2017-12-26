﻿namespace OriginalStudio.WebUI.Manage.Withdraw
{
    using Aspose.Cells;
    using OriginalStudio.BLL;
    using OriginalStudio.Model;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Wuqi.Webdiyer;
    using OriginalStudio.BLL.Supplier;
    using OriginalStudio.BLL.Settled;
    using OriginalStudio.Model.Settled;

    public class PayingByApi : ManagePageBase
    {
        protected Button btnExport;
        protected Button btnSearch;
        protected DropDownList ddlbankName;
        protected DropDownList ddlmode;
        protected DropDownList ddlSupplier;
        protected HtmlForm form1;
        protected HtmlHead Head1;
        protected AspNetPager Pager1;
        protected Repeater rptList;
        protected TextBox txtAccount;
        protected TextBox txtItemInfoId;
        protected TextBox txtpayeeName;
        protected TextBox txtMerchantName;

        private void BindData()
        {
            List<SearchParam> searchParams = new List<SearchParam>();
            searchParams.Add(new SearchParam("status", 16));        //接口支付中，固定16
            int result = 0;
            if (!(string.IsNullOrEmpty(this.txtItemInfoId.Text.Trim()) || !int.TryParse(this.txtItemInfoId.Text.Trim(), out result)))
            {
                searchParams.Add(new SearchParam("id", result));
            }
            if (!string.IsNullOrEmpty(this.txtMerchantName.Text.Trim()))
            {
                searchParams.Add(new SearchParam("merchantname", txtMerchantName.Text.Trim()));
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
            if (ddlmode.SelectedIndex > 0)
            {
                searchParams.Add(new SearchParam("settmode", ddlmode.SelectedValue.ToString()));
            }
            DataSet set = SettledFactory.PageSearch(searchParams, this.Pager1.PageSize, this.Pager1.CurrentPageIndex, string.Empty);
            this.Pager1.RecordCount = Convert.ToInt32(set.Tables[0].Rows[0][0]);
            DataTable table = set.Tables[1];
            this.rptList.DataSource = table;
            this.rptList.DataBind();
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
                        row["sName"] = Enum.GetName(typeof(SettledStatusEnum), row["status"]);
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        private DataSet GetData()
        {
            List<SearchParam> searchParams = new List<SearchParam>();
            searchParams.Add(new SearchParam("status", 0x10));
            int result = 0;
            if (!(string.IsNullOrEmpty(this.txtItemInfoId.Text.Trim()) || !int.TryParse(this.txtItemInfoId.Text.Trim(), out result)))
            {
                searchParams.Add(new SearchParam("id", result));
            }
            if (!string.IsNullOrEmpty(this.txtMerchantName.Text.Trim()))
            {
                searchParams.Add(new SearchParam("merchantname", txtMerchantName.Text.Trim()));
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

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            if (!base.IsPostBack)
            {
                this.ddlmode.Items.Add(new ListItem("--提现方式--", ""));
                /**/
                foreach (int num in Enum.GetValues(typeof(SettledModeEnum)))
                {
                    string name = Enum.GetName(typeof(SettledModeEnum), num);
                    this.ddlmode.Items.Add(new ListItem(name, num.ToString()));
                }
                
                DataTable table = SysSupplierFactory.GetList("isdistribution=1").Tables[0];
                this.ddlSupplier.Items.Add(new ListItem("--付款接口--", ""));
                this.ddlSupplier.Items.Add(new ListItem("不走接口", "0"));
                foreach (DataRow row in table.Rows)
                {
                    this.ddlSupplier.Items.Add(new ListItem(row["SupplierName"].ToString(), row["SupplierCode"].ToString()));
                }

                //--收款银行--
                DataTable dtBank = OriginalStudio.BLL.Withdraw.ChannelWithdraw.GetChannelWithdrawList().Tables[0];
                this.ddlbankName.Items.Add(new ListItem("--收款银行--", ""));
                foreach (DataRow row in dtBank.Rows)
                    this.ddlbankName.Items.Add(new ListItem(row["bankName"].ToString(), row["bankCode"].ToString()));

                this.BindData();
            }
        }

        protected void Pager1_PageChanging(object src, PageChangingEventArgs e)
        {
            this.BindData();
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                SettledFactory.Cancel(Convert.ToInt32(e.CommandArgument));
                this.BindData();
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
    }
}

