﻿namespace KuaiCard.WebUI.Manage.User.distribution
{
    using Aspose.Cells;
    using OriginalStudio.BLL;
    using OriginalStudio.Model;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Wuqi.Webdiyer;

    public class Distributions2 : ManagePageBase
    {
        protected Button btnExport;
        protected Button btnSearch;
        protected HtmlSelect ddlbankName;
        protected TextBox EtimeBox;
        protected HtmlForm form1;
        protected HtmlHead Head1;
        protected AspNetPager Pager1;
        protected Repeater rptTrades;
        protected HtmlInputHidden selectedUsers;
        protected TextBox StimeBox;
        protected string total_amount = "0.00元";
        protected TextBox txtbankAccount;
        protected TextBox txttrade_no;
        protected TextBox txtuserId;

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
                        row["sName"] = KuaiCard.BLL.distribution.GetStatusText(row["status"]);
                    }
                    dataTable.AcceptChanges();
                    dataTable.TableName = "Rpt";
                    string file = base.Server.MapPath("~/common/template/xls/Distributions.xls");
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
            this.LoadData();
        }

        private DataSet GetData()
        {
            List<SearchParam> searchParams = new List<SearchParam>();
            if (base.currentManage.isSuperAdmin <= 0)
            {
            }
            string s = this.txtuserId.Text.Trim();
            int result = 0;
            if (int.TryParse(s, out result))
            {
                searchParams.Add(new SearchParam("userid", result));
            }
            if (!string.IsNullOrEmpty(this.txtbankAccount.Text))
            {
                searchParams.Add(new SearchParam("bankAccount", this.txtbankAccount.Text.Trim()));
            }
            if (!string.IsNullOrEmpty(this.txttrade_no.Text))
            {
                searchParams.Add(new SearchParam("trade_no", this.txttrade_no.Text.Trim()));
            }
            if (!string.IsNullOrEmpty(this.ddlbankName.Value))
            {
                searchParams.Add(new SearchParam("bankcode", this.ddlbankName.Value));
            }
            DateTime minValue = DateTime.MinValue;
            if ((!string.IsNullOrEmpty(this.StimeBox.Text.Trim()) && DateTime.TryParse(this.StimeBox.Text.Trim(), out minValue)) && (minValue > DateTime.MinValue))
            {
                searchParams.Add(new SearchParam("stime", this.StimeBox.Text.Trim()));
            }
            if ((!string.IsNullOrEmpty(this.EtimeBox.Text.Trim()) && DateTime.TryParse(this.EtimeBox.Text.Trim(), out minValue)) && (minValue > DateTime.MinValue))
            {
                searchParams.Add(new SearchParam("etime", minValue.AddDays(1.0)));
            }
            string orderby = string.Empty;
            return KuaiCard.BLL.distribution.PageSearch(searchParams, this.Pager1.PageSize, this.Pager1.CurrentPageIndex, orderby);
        }

        private void LoadData()
        {
            List<SearchParam> searchParams = new List<SearchParam>();
            if (base.currentManage.isSuperAdmin <= 0)
            {
            }
            string s = this.txtuserId.Text.Trim();
            int result = 0;
            if (int.TryParse(s, out result))
            {
                searchParams.Add(new SearchParam("userid", result));
            }
            if (!string.IsNullOrEmpty(this.txtbankAccount.Text))
            {
                searchParams.Add(new SearchParam("bankAccount", this.txtbankAccount.Text.Trim()));
            }
            if (!string.IsNullOrEmpty(this.txttrade_no.Text))
            {
                searchParams.Add(new SearchParam("trade_no", this.txttrade_no.Text.Trim()));
            }
            if (!string.IsNullOrEmpty(this.ddlbankName.Value))
            {
                searchParams.Add(new SearchParam("bankcode", this.ddlbankName.Value));
            }
            DateTime minValue = DateTime.MinValue;
            if ((!string.IsNullOrEmpty(this.StimeBox.Text.Trim()) && DateTime.TryParse(this.StimeBox.Text.Trim(), out minValue)) && (minValue > DateTime.MinValue))
            {
                searchParams.Add(new SearchParam("stime", this.StimeBox.Text.Trim()));
            }
            if ((!string.IsNullOrEmpty(this.EtimeBox.Text.Trim()) && DateTime.TryParse(this.EtimeBox.Text.Trim(), out minValue)) && (minValue > DateTime.MinValue))
            {
                searchParams.Add(new SearchParam("etime", minValue.AddDays(1.0)));
            }
            string orderby = string.Empty;
            DataSet set = KuaiCard.BLL.distribution.PageSearch(searchParams, this.Pager1.PageSize, this.Pager1.CurrentPageIndex, orderby);
            this.Pager1.RecordCount = Convert.ToInt32(set.Tables[0].Rows[0][0]);
            this.rptTrades.DataSource = set.Tables[1];
            this.rptTrades.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            if (!base.IsPostBack)
            {
                this.StimeBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.EtimeBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.StimeBox.Attributes.Add("onFocus", "WdatePicker()");
                this.EtimeBox.Attributes.Add("onFocus", "WdatePicker()");
                this.LoadData();
            }
        }

        protected void Pager1_PageChanged(object sender, EventArgs e)
        {
            this.LoadData();
        }

        protected void rptTrades_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "query")
            {
                string str = Convert.ToString(e.CommandArgument);
            }
            else if (e.CommandName == "Reissue")
            {
                base.Response.Redirect(string.Format("Reissue.aspx?id={0}", e.CommandArgument));
            }
        }

        protected void rptTrades_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                int num = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "status"));
                Button button = (Button) e.Item.FindControl("btnquery");
                Button button2 = (Button) e.Item.FindControl("btnReissue");
                button2.Visible = num == 0xff;
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

