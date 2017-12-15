namespace OriginalStudio.WebUI.Manage.OrderStat
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Order;
    using OriginalStudio.BLL.Supplier;
    using OriginalStudio.Model;
    using OriginalStudio.WebComponents.Web;
    using System;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class OrderReport2 : ManagePageBase
    {
        protected Button btn_Search;
        protected DropDownList ddlSupplier;
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
            int suppid = 0;
            if (!string.IsNullOrEmpty(this.ddlSupplier.SelectedValue))
            {
                suppid = int.Parse(this.ddlSupplier.SelectedValue);
            }
            else
            {
                suppid = 0;
            }
            DateTime dtbegin = Lib.Utils.Utils.StrToDateTime(this.StimeBox.Text.Trim());
            DateTime dtend = Lib.Utils.Utils.StrToDateTime(this.EtimeBox.Text.Trim());

            DataSet ds = OriginalStudio.BLL.Stat.OrderReport.统计通道商订单金额利润(suppid, dtbegin, dtend);

            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                this.rep_report.DataSource = dt;
                this.rep_report.DataBind();
                try
                {
                    this.TotalRealvalue = Convert.ToDecimal(dt.Compute("sum(订单总额)", "")).ToString("f2");
                    this.TotalSupplierAmt = Convert.ToDecimal(dt.Compute("sum(平台总额)", "")).ToString("f2");
                    this.TotalPromAmt = Convert.ToDecimal(dt.Compute("sum(代理总额)", "")).ToString("f2");
                    this.TotalPayAmtATM = Convert.ToDecimal(dt.Compute("sum(商户总额)", "")).ToString("f2");
                    this.TotalProfit = Convert.ToDecimal(dt.Compute("sum(利润)", "")).ToString("f2");
                }
                catch
                {
                }
            }
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
                DataTable table = SysSupplierFactory.GetList(string.Empty).Tables[0];
                this.ddlSupplier.Items.Add(new ListItem("--请选择--", ""));
                foreach (DataRow row in table.Rows)
                {
                    this.ddlSupplier.Items.Add(new ListItem(row["SupplierName"].ToString(), row["SupplierCode"].ToString()));
                }
                //this.LoadData();
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

