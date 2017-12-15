namespace OriginalStudio.WebUI.Manage.OrderStat
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Order;
    using OriginalStudio.Model;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Wuqi.Webdiyer;

    public class UsersOrderIncomes : ManagePageBase
    {
        protected Button btn_Search;
        protected DropDownList ddlChannelType;
        protected TextBox EtimeBox;
        protected HtmlForm form1;
        protected Repeater gv_data;
        protected AspNetPager Pager1;
        protected TextBox StimeBox;
        protected TextBox txtMerchantName;
        protected TextBox txtvaluefrom;
        protected TextBox txtvalueto;

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }

        private void LoadData()
        {
            List<SearchParam> searchParams = new List<SearchParam>();
            if (!String.IsNullOrEmpty(txtMerchantName.Text.Trim()))
            {
                searchParams.Add(new SearchParam("merchantname", txtMerchantName.Text.Trim()));
            }
            if (!string.IsNullOrEmpty(this.ddlChannelType.SelectedValue))
            {
                searchParams.Add(new SearchParam("typeid", this.ddlChannelType.SelectedValue));
            }
            DateTime dtbegin = Lib.Utils.Utils.StrToDateTime(this.StimeBox.Text.Trim());
            searchParams.Add(new SearchParam("stime", dtbegin.ToString("yyyy-MM-dd")));

            DateTime dtend = Lib.Utils.Utils.StrToDateTime(this.EtimeBox.Text.Trim());
            searchParams.Add(new SearchParam("etime", dtend.ToString("yyyy-MM-dd")));

            decimal num2 = 0M;
            if (!(string.IsNullOrEmpty(this.txtvaluefrom.Text.Trim()) || !decimal.TryParse(this.txtvaluefrom.Text.Trim(), out num2)))
            {
                searchParams.Add(new SearchParam("fvaluefrom", num2));
            }
            if (!(string.IsNullOrEmpty(this.EtimeBox.Text.Trim()) || !decimal.TryParse(this.EtimeBox.Text.Trim(), out num2)))
            {
                searchParams.Add(new SearchParam("fvalueto", num2));
            }
            string orderby = " OrderDate asc, UserID asc";
            DataSet set = OriginalStudio.BLL.Stat.OrderReport.统计商户收益统计(searchParams, this.Pager1.PageSize, this.Pager1.CurrentPageIndex, orderby);
            this.Pager1.RecordCount = Convert.ToInt32(set.Tables[0].Rows[0][0]);
            this.gv_data.DataSource = set.Tables[1];
            this.gv_data.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            if (!base.IsPostBack)
            {
                this.StimeBox.Text = DateTime.Now.AddDays(-30.0).ToString("yyyy-MM-dd");
                this.EtimeBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.StimeBox.Attributes.Add("onFocus", "WdatePicker()");
                this.EtimeBox.Attributes.Add("onFocus", "WdatePicker()");

                DataTable dt = OriginalStudio.BLL.Channel.SysChannelType.GetCacheList();
                this.ddlChannelType.Items.Add(new ListItem("--请选择--", ""));
                foreach (DataRow row in dt.Rows)
                {
                    this.ddlChannelType.Items.Add(new ListItem(row["TypeName"].ToString(), row["TypeID"].ToString()));
                }

            }
        }

        protected void Pager1_PageChanged(object sender, EventArgs e)
        {
            this.LoadData();
        }

        private void setPower()
        {
            if (!ManageFactory.CheckCurrentPermission(false, ManageRole.Report))
            {
                base.Response.Write("Sorry,No authority!");
                base.Response.End();
            }
        }
    }
}

