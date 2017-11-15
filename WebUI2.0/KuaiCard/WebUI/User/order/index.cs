﻿namespace KuaiCard.WebUI.user.order
{
    using KuaiCard.BLL;
    using KuaiCard.WebComponents.Web;
    using KuaiCardLib.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Wuqi.Webdiyer;

    public class index : UserPageBase
    {
        protected Button b_search;
        protected HtmlSelect channelId;
        protected DropDownList ddlNotifyStatus;
        protected HtmlInputText edate;
        protected HtmlInputText okey;
        protected string pageordersucctotal = "0";
        protected string pageordertotal = "0";
        protected AspNetPager Pager1;
        protected string pagerealvalue = "0";
        protected string pagerefervalue = "0";
        protected Repeater rptOrders;
        protected HtmlInputText sdate;
        protected HtmlSelect select_field;
        protected HtmlSelect Success;
        protected string totalordertotal = "0";
        protected string totalrealvalue = "0";
        protected string totalrefervalue = "0";
        protected string totalsuccordertotal = "0";

        protected void b_search_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }

        public string builderdate(string date, string hour, string m, string s)
        {
            return string.Format("{0} {1}:{2}:{3}", new object[] { date, hour, m, s });
        }

        private void InitForm()
        {
            this.sdate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            this.edate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            this.channelId.Value = "0";
            this.okey.Value = string.Empty;
        }

        private void LoadData()
        {
            List<SearchParam> searchParams = new List<SearchParam>();
            searchParams.Add(new SearchParam("userid", base.UserId));
            searchParams.Add(new SearchParam("deduct", 0));
            int result = 0;
            if ((!string.IsNullOrEmpty(this.channelId.Value) && int.TryParse(this.channelId.Value, out result)) && (result > 0))
            {
                searchParams.Add(new SearchParam("typeId", result));
            }
            if (int.TryParse(this.select_field.Value, out result))
            {
                string str = this.okey.Value.Trim();
                if (!string.IsNullOrEmpty(str))
                {
                    switch (result)
                    {
                        case 1:
                            searchParams.Add(new SearchParam("userorder", str));
                            break;

                        case 2:
                            searchParams.Add(new SearchParam("cardno", str));
                            break;

                        case 3:
                            searchParams.Add(new SearchParam("orderid", str));
                            break;
                    }
                }
            }
            if ((!string.IsNullOrEmpty(this.Success.Value) && int.TryParse(this.Success.Value, out result)) && (result >= 0))
            {
                if (result == 0)
                {
                    searchParams.Add(new SearchParam("status", "!=", 8));   //把扣单的去掉！！！！。不能用字符串"8"，必须用数字8
                    //searchParams.Add(new SearchParam("status", "!=", 16));       //平台扣量
                    //searchParams.Add(new SearchParam("status", "!=", 32));       //部份成功
                    //searchParams.Add(new SearchParam("status", "!=", 64));       //转单
                }
                else if (result != 4)
                {
                    //
                    searchParams.Add(new SearchParam("status", result));
                }
                else
                {
                    //失败状态的。一定把扣单排除掉    2017.2.12处理
                    searchParams.Add(new SearchParam("statusallfail", result));
                    searchParams.Add(new SearchParam("status", "!=", 8));       //扣量
                    //searchParams.Add(new SearchParam("status", "!=", 16));       //平台扣量
                    //searchParams.Add(new SearchParam("status", "!=", 32));       //部份成功
                    //searchParams.Add(new SearchParam("status", "!=", 64));       //转单
                }
            }
            DateTime minValue = DateTime.MinValue;
            if (DateTime.TryParse(this.builderdate(this.sdate.Value, "00", "00", "00"), out minValue) && (minValue > DateTime.MinValue))
            {
                searchParams.Add(new SearchParam("stime", minValue.ToString()));
            }
            if (DateTime.TryParse(this.builderdate(this.edate.Value, "23", "59", "59"), out minValue) && (minValue > DateTime.MinValue))
            {
                searchParams.Add(new SearchParam("etime", minValue.ToString()));
            }
            if ((!string.IsNullOrEmpty(this.ddlNotifyStatus.SelectedValue) && int.TryParse(this.ddlNotifyStatus.SelectedValue, out result)) && (result > 0))
            {
                searchParams.Add(new SearchParam("notifystat", result));
            }
            string orderby = string.Empty;
            //DataSet set = new OrderBank().PageSearch(searchParams, this.Pager1.PageSize, this.Pager1.CurrentPageIndex, orderby);
            DataSet set = new OrderBank().UserPageSearch(searchParams, this.Pager1.PageSize, this.Pager1.CurrentPageIndex, orderby);
            this.Pager1.RecordCount = Convert.ToInt32(set.Tables[0].Rows[0][0]);
            this.rptOrders.DataSource = set.Tables[1];
            this.rptOrders.DataBind();
            DataTable table = set.Tables[2];
            if ((table != null) && (table.Rows.Count > 0))
            {
                try
                {
                    this.totalrefervalue = Convert.ToDecimal(table.Rows[0]["refervalue"]).ToString("f2");
                    this.totalrealvalue = Convert.ToDecimal(table.Rows[0]["realvalue"]).ToString("f2");
                    this.totalordertotal = Convert.ToDecimal(table.Rows[0]["ordtotal"]).ToString("f0");
                    this.totalsuccordertotal = Convert.ToDecimal(table.Rows[0]["succordtotal"]).ToString("f0");
                }
                catch
                {
                }
            }
            table = set.Tables[1];
            try
            {
                this.pagerefervalue = Convert.ToDecimal(table.Compute("sum(refervalue)", "")).ToString("f2");
                this.pagerealvalue = Convert.ToDecimal(table.Compute("sum(realvalue)", "")).ToString("f2");
                this.pageordertotal = table.Rows.Count.ToString();
                DataRow[] rowArray = table.Select("status=2");
                this.pageordersucctotal = rowArray.Length.ToString();
            }
            catch
            {
            }
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
            if ((e.Item.ItemType == ListItemType.AlternatingItem) || (e.Item.ItemType == ListItemType.Item))
            {
                Literal literal = (Literal) e.Item.FindControl("litdo");
                string str = DataBinder.Eval(e.Item.DataItem, "status").ToString();
                string str2 = DataBinder.Eval(e.Item.DataItem, "orderid").ToString();
            }
            if ((e.Item.ItemType == ListItemType.Footer) && (this.rptOrders.Items.Count == 0))
            {
                Literal literal2 = (Literal) e.Item.FindControl("litfoot");
                literal2.Text = " <tfoot>\r\n                        <tr>\r\n                            <td colspan=\"10\" class=\"nomsg\">\r\n                                －_－^..暂无记录\r\n                            </td>\r\n                        </tr>\r\n                     </tfoot>     ";
            }
        }
    }
}

