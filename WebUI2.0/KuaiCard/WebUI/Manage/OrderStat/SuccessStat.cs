namespace OriginalStudio.WebUI.Manage.OrderStat
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Order;
    using OriginalStudio.Model;
    using OriginalStudio.WebComponents.Web;
    using System;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class SuccessStat : ManagePageBase
    {
        protected Button btn_Search;
        protected TextBox EtimeBox;
        protected HtmlForm form1;
        protected Repeater rep_report;
        protected TextBox StimeBox;
        protected string TotalRealvalue = "0.00";
        protected string TotalSupplierAmt = "0.00";
        protected string TotalPromAmt = "0.00";
        protected string TotalPayAmtATM = "0.00";
        protected string TotalProfit = "0.00";

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }

        private void LoadData()
        {
            DateTime dtbegin = Lib.Utils.Utils.StrToDateTime(this.StimeBox.Text.Trim());
            DateTime dtend = Lib.Utils.Utils.StrToDateTime(this.EtimeBox.Text.Trim());

            DataSet ds = OriginalStudio.BLL.Stat.OrderReport.统计通道商成功率(dtbegin, dtend);

            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                this.rep_report.DataSource = dt;
                this.rep_report.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            if (!base.IsPostBack)
            {
                this.StimeBox.Text = DateTime.Now.AddMinutes(-5).ToString("yyyy-MM-dd hh:mm:ss");
                this.EtimeBox.Text = DateTime.Now.Date.AddDays(1).ToString("yyyy-MM-dd hh:mm:ss");
                this.StimeBox.Attributes.Add("onFocus", "WdatePicker()");
                this.EtimeBox.Attributes.Add("onFocus", "WdatePicker()");
            }
        }

        protected void rptOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
            }
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

