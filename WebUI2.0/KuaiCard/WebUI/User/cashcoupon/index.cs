namespace OriginalStudio.WebUI.User.cashcoupon
{
    using OriginalStudio.BLL;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Wuqi.Webdiyer;
    using OriginalStudio.BLL.Settled;

    public class index : UserPageBase
    {
        protected Button b_search;
        protected HtmlSelect fState;
        protected AspNetPager Pager1;
        protected Repeater rptDetails;
        protected HtmlInputText sdate;
        protected HtmlInputText edate;
        protected HtmlInputText bankaccount;
        protected HtmlInputText accountname;
        protected HtmlInputText trade_no;

        protected string totalamt = "0";
        protected string successamt = "0";
        protected string totalcount = "0";
        protected string successcount = "0";
        protected string successcharges = "0";


        protected void b_search_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }

        private void InitForm()
        {
            this.sdate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            this.edate.Value = DateTime.Now.ToString("yyyy-MM-dd");
        }

        public string builderdate(string date, string hour, string m, string s)
        {
            return string.Format("{0} {1}:{2}:{3}", new object[] { date, hour, m, s });
        }

        private void LoadData()
        {
            List<SearchParam> searchParams = new List<SearchParam>();
            searchParams.Add(new SearchParam("userid", base.UserId));
            if (this.fState.Value != "-1")
            {
                searchParams.Add(new SearchParam("status", int.Parse(this.fState.Value)));
            }
            DateTime minValue = DateTime.MinValue;
            if (DateTime.TryParse(this.builderdate(this.sdate.Value, "00", "00", "00"), out minValue) && (minValue > DateTime.MinValue))
            {
                searchParams.Add(new SearchParam("begindate", minValue.ToString()));
            }
            if (DateTime.TryParse(this.builderdate(this.edate.Value, "23", "59", "59"), out minValue) && (minValue > DateTime.MinValue))
            {
                searchParams.Add(new SearchParam("enddate", minValue.ToString()));
            }
            string account = this.bankaccount.Value.ToString();
            if (!string.IsNullOrEmpty(account))
                searchParams.Add(new SearchParam("account", account));
            string str_trade_no = this.trade_no.Value.ToString();
            if (!string.IsNullOrEmpty(str_trade_no))
                searchParams.Add(new SearchParam("trade_no", str_trade_no));

            string accountname = this.accountname.Value.ToString();
            if (!string.IsNullOrEmpty(accountname))
                searchParams.Add(new SearchParam("payeename", accountname));
            

            string orderby = string.Empty;
            DataSet set = SettledFactory.PageSearch(searchParams, this.Pager1.PageSize, this.Pager1.CurrentPageIndex, orderby);
            this.Pager1.RecordCount = Convert.ToInt32(set.Tables[0].Rows[0][0]);
            this.rptDetails.DataSource = set.Tables[1];
            this.rptDetails.DataBind();

            DataTable table = set.Tables[2];
            if ((table != null) && (table.Rows.Count > 0))
            {
                try
                {
                    this.totalamt = Convert.ToDecimal(table.Rows[0]["totalamt"]).ToString("f2");
                    this.successamt = Convert.ToDecimal(table.Rows[0]["successamt"]).ToString("f2");
                    this.successcharges = Convert.ToDecimal(table.Rows[0]["successcharges"]).ToString("f2");
                    this.totalcount = Convert.ToDecimal(table.Rows[0]["totalcount"]).ToString("f0");
                    this.successcount = Convert.ToDecimal(table.Rows[0]["successcount"]).ToString("f0");
                    
                }
                catch
                {
                }
            }

            this.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.InitForm();
                this.LoadData();
            }
        }

        protected void Pager1_PageChanged(object sender, EventArgs e)
        {
            this.LoadData();
        }

        protected void rptDetails_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }
    }
}

