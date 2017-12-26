﻿namespace OriginalStudio.WebUI.Manage.Withdraw
{
    using Aspose.Cells;
    using OriginalStudio.BLL;
    using OriginalStudio.Model;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Wuqi.Webdiyer;
    using OriginalStudio.BLL.Supplier;
    using OriginalStudio.BLL.Settled;
    using OriginalStudio.Model.Settled;

    public class Historys : ManagePageBase
    {
        protected Button btnExport;
        protected Button btnSearch;
        protected DropDownList ddlbankName;
        protected DropDownList ddlmode;
        protected DropDownList ddlStatusList;
        protected DropDownList ddlSupplier;
        protected HtmlForm form1;
        protected HtmlHead Head1;
        protected AspNetPager Pager1;
        protected Repeater rptList;
        protected string TotalMoney = "0.00元";
        protected TextBox txtAccount;
        protected TextBox txtEtimeBox;
        protected TextBox txtItemInfoId;
        protected TextBox txtpayeeName;
        protected TextBox txtStimeBox;
        protected TextBox txtMerchantName;

        private void BindData()
        {
            List<SearchParam> searchParams = new List<SearchParam>();
            if (!string.IsNullOrEmpty(this.ddlStatusList.SelectedValue))
            {
                searchParams.Add(new SearchParam("status", int.Parse(this.ddlStatusList.SelectedValue)));
            }
            int result = 0;
            if (!(string.IsNullOrEmpty(this.txtItemInfoId.Text.Trim()) || !int.TryParse(this.txtItemInfoId.Text.Trim(), out result)))
            {
                searchParams.Add(new SearchParam("id", result));
            }
            if (!string.IsNullOrEmpty(this.txtMerchantName.Text.Trim()))
            {
                searchParams.Add(new SearchParam("merchantname", this.txtMerchantName.Text.Trim()));
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
            DateTime minValue = DateTime.MinValue;
            if ((!string.IsNullOrEmpty(this.txtStimeBox.Text.Trim()) && DateTime.TryParse(this.txtStimeBox.Text.Trim(), out minValue)) && (minValue > DateTime.MinValue))
            {
                searchParams.Add(new SearchParam("saddtime", this.txtStimeBox.Text.Trim()));
            }
            if ((!string.IsNullOrEmpty(this.txtEtimeBox.Text.Trim()) && DateTime.TryParse(this.txtEtimeBox.Text.Trim(), out minValue)) && (minValue > DateTime.MinValue))
            {
                searchParams.Add(new SearchParam("eaddtime", this.txtEtimeBox.Text.Trim()));
            }
            DataSet set = SettledFactory.PageSearch(searchParams, this.Pager1.PageSize, this.Pager1.CurrentPageIndex, string.Empty);
            this.Pager1.RecordCount = Convert.ToInt32(set.Tables[0].Rows[0][0]);
            DataTable table = set.Tables[1];
            if (table != null)
            {
                table.Columns.Add("StatusText");
                foreach (DataRow row in table.Rows)
                {                    
                    switch (((SettledStatusEnum) row["Status"]))
                    {
                        case SettledStatusEnum.审核中:
                            row["StatusText"] = "<font color='#66CC00'>审核中</font>";
                            break;

                        case SettledStatusEnum.支付中:
                            row["StatusText"] = "<a href=\"Pay.aspx?id=" + row["ID"].ToString() + "\">进行支付</a>";
                            break;

                        case SettledStatusEnum.无效:
                            row["StatusText"] = "<font color='red'>无效申请</font>";
                            break;

                        case SettledStatusEnum.已支付:
                            row["StatusText"] = "<font color='blue'>已支付</font>";
                            break;
                    }
                }
            }
            this.rptList.DataSource = table;
            this.rptList.DataBind();
            this.TotalMoney = Convert.ToString(set.Tables[2].Rows[0][0]);
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            if (!base.IsPostBack)
            {
                this.ddlmode.Items.Add(new ListItem("--提现方式--", ""));
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
                this.txtStimeBox.Attributes.Add("onFocus", "WdatePicker()");
                this.txtEtimeBox.Attributes.Add("onFocus", "WdatePicker()");
                this.txtStimeBox.Text = DateTime.Now.ToString("yyyy-MM-01");
                this.txtEtimeBox.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                this.ddlStatusList.Items.Add(new ListItem("--状态--", ""));
                foreach (int num in Enum.GetValues(typeof(SettledStatusEnum)))
                {
                    string text = Enum.GetName(typeof(SettledStatusEnum), num);
                    this.ddlStatusList.Items.Add(new ListItem(text, num.ToString()));
                }
                this.ddlStatusList.SelectedValue = 8.ToString();


                DataTable dtBank = OriginalStudio.BLL.Withdraw.ChannelWithdraw.GetChannelWithdrawList().Tables[0];
                this.ddlbankName.Items.Add(new ListItem("--收款银行--", ""));
                foreach (DataRow row in dtBank.Rows)
                    this.ddlbankName.Items.Add(new ListItem(row["bankName"].ToString(), row["bankCode"].ToString()));



                this.BindData();
            }
        }

        protected void Pager1_PageChanged(object sender, EventArgs e)
        {
            this.BindData();
        }

        private void setPower()
        {
            if (!ManageFactory.CheckCurrentPermission(false, ManageRole.Financial))
            {
                base.Response.Write("Sorry,No authority!");
                base.Response.End();
            }
        }

        public string action
        {
            get
            {
                return WebBase.GetQueryStringString("action", "");
            }
        }

        public int userId
        {
            get
            {
                return WebBase.GetQueryStringInt32("userid", 0);
            }
        }

        #region 导出


        private DataSet GetData()
        {
            //导出使用
            List<SearchParam> searchParams = new List<SearchParam>();
            if (!string.IsNullOrEmpty(this.ddlStatusList.SelectedValue))
            {
                searchParams.Add(new SearchParam("status", int.Parse(this.ddlStatusList.SelectedValue)));
            }
            int result = 0;
            if (!(string.IsNullOrEmpty(this.txtItemInfoId.Text.Trim()) || !int.TryParse(this.txtItemInfoId.Text.Trim(), out result)))
            {
                searchParams.Add(new SearchParam("id", result));
            }
            if (!(string.IsNullOrEmpty(this.txtMerchantName.Text.Trim()) || !int.TryParse(this.txtMerchantName.Text.Trim(), out result)))
            {
                searchParams.Add(new SearchParam("merchantname", result));
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
        #endregion
    }
}

