namespace OriginalStudio.WebUI.Manage.Order
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Order;
    using OriginalStudio.Model;
    using OriginalStudio.WebComponents.Web;
    using System;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Wuqi.Webdiyer;

    public class OrderReport4 : ManagePageBase
    {
        protected Button btn_Search;
        protected TextBox EtimeBox;
        protected HtmlForm form1;
        protected AspNetPager Pager1;
        protected Repeater rep_report;
        protected TextBox StimeBox;
        protected string TotalPayAmtATM = "0.00";
        protected string TotalProfit = "0.00";
        protected string TotalRealvalue = "0.00";
        protected string TotalSupplierAmt = "0.00";

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }

        private void LoadData()
        {
            DateTime dtbegin = Lib.Utils.Utils.StrToDateTime(this.StimeBox.Text.Trim());
            DateTime dtend = Lib.Utils.Utils.StrToDateTime(this.EtimeBox.Text.Trim());

            DataSet set = OriginalStudio.BLL.Stat.OrderReport.统计代理收益(dtbegin, dtend, 0, "");
            try
            {
                this.Pager1.RecordCount = set.Tables[0].Rows.Count;
                this.rep_report.DataSource = set.Tables[0];
            }
            catch {
                this.Pager1.RecordCount = 0;
            }
            
            this.rep_report.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            if (!base.IsPostBack)
            {
                this.StimeBox.Text = DateTime.Today.ToString("yyyy-MM-dd");
                this.EtimeBox.Text = DateTime.Today.AddDays(1.0).ToString("yyyy-MM-dd");
                this.StimeBox.Attributes.Add("onFocus", "WdatePicker()");
                this.EtimeBox.Attributes.Add("onFocus", "WdatePicker()");
                this.LoadData();
            }
        }

        protected void Pager1_PageChanged(object sender, EventArgs e)
        {
            this.LoadData();
        }

        protected void rep_report_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
        }

        protected void rptOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }

        private void setPower()
        {
            if (!ManageFactory.CheckCurrentPermission(false, ManageRole.Orders))
            {
                base.Response.Write("Sorry,No authority!");
                base.Response.End();
            }
        }
    }
}


/*
private void LoadData()
        {
            string orderby = "promAmt DESC";
            if (this.ViewState["Sort"] != null)
            {
                orderby = this.ViewState["Sort"].ToString();
            }
            DateTime minValue = DateTime.MinValue;
            if (!(string.IsNullOrEmpty(this.StimeBox.Text.Trim()) || !DateTime.TryParse(this.StimeBox.Text.Trim(), out minValue)))
            {
            }
            DateTime result = DateTime.MinValue;
            if (!(string.IsNullOrEmpty(this.EtimeBox.Text.Trim()) || !DateTime.TryParse(this.EtimeBox.Text.Trim(), out result)))
            {
            }
            DataSet set = Dal.AgentStat2(minValue, result, this.Pager1.CurrentPageIndex - 1, this.Pager1.PageSize, orderby);
            
            try
            {
                this.Pager1.RecordCount = Convert.ToInt32(set.Tables[0].Rows[0]["C"]);
                this.rep_report.DataSource = set.Tables[1];
            }
            catch {
                this.Pager1.RecordCount = 0;
            }
            
            this.rep_report.DataBind();
        } 
     */

