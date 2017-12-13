﻿namespace OriginalStudio.WebUI.Manage.Order
{
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

    public class BankReportList : ManagePageBase
    {
        protected Button btn_Search;
        protected DropDownList ddlChannelType;
        protected DropDownList ddlmange;
        protected DropDownList ddlNotifyStatus;
        protected DropDownList ddlOrderStatus;
        protected TextBox EtimeBox;
        protected HtmlForm form1;
        protected AspNetPager Pager1;
        protected Repeater rptOrders;
        protected TextBox StimeBox;
        protected string TotalCommission = "0.00";
        protected string TotalProfit = "0.00";
        protected string TotalPromATM = "0.00";
        protected string TotalTranATM = "0.00";
        protected string TotalUserATM = "0.00";
        protected TextBox txtOrderId;
        protected TextBox txtSuppOrder;
        protected TextBox txtMerchantName;
        protected TextBox txtUserOrder;

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }

        protected string getsupplierName(object obj)
        {
            if ((obj == DBNull.Value) || (obj == null))
            {
                return string.Empty;
            }
            return SysSupplierFactory.GetSupplierModelByCode(int.Parse(obj.ToString())).SupplierName;
        }

        private void InitForm()
        {
            if (this.UserId > -1)
            {
                this.txtMerchantName.Text = this.UserId.ToString();
            }
            if (this.Status > -1)
            {
                this.ddlOrderStatus.SelectedValue = this.Status.ToString();
            }
            if (this.ctype > -1)
            {
                this.ddlChannelType.SelectedValue = this.ctype.ToString();
            }
            if (this.NotifyStatus > -1)
            {
                this.ddlNotifyStatus.SelectedValue = this.NotifyStatus.ToString();
            }
            if (!string.IsNullOrEmpty(this.stime))
            {
                this.StimeBox.Text = this.stime;
            }
            if (!string.IsNullOrEmpty(this.etime))
            {
                this.EtimeBox.Text = this.etime;
            }
            if (!string.IsNullOrEmpty(this.sysorderid))
            {
                this.txtOrderId.Text = this.sysorderid;
            }
            if (!string.IsNullOrEmpty(this.userorderid))
            {
                this.txtUserOrder.Text = this.userorderid;
            }
            if (!string.IsNullOrEmpty(this.supporderid))
            {
                this.txtSuppOrder.Text = this.supporderid;
            }
            this.ddlmange.Items.Add("--请选择业务员--");
            DataTable table = ManageFactory.GetList(" status =1").Tables[0];
            foreach (DataRow row in table.Rows)
            {
                this.ddlmange.Items.Add(new ListItem(row["username"].ToString(), row["id"].ToString()));
            }
            if (this.MID > -1)
            {
                this.ddlmange.SelectedValue = this.MID.ToString();
            }
        }


        //增加数据 状态样式 方法
        protected string getStatusStyle(string status)
        {

            string statusCss = string.Empty;
            switch (status)
            {
                case "1":
                    statusCss = "<a title='处理中' style='color:darkorange' href='javascript:void(0)'> <i class='fa   fa-hourglass-end'></i></a>";
                    break;

                case "2":

                    statusCss = "<a title='已完成' style='color:#1db283' href='javascript:void(0)'> <i class='fa  fa-check-circle'></i></a>";
                    break;

                case "4":

                    statusCss = "<a title='失败'  style='color:#ff4a4a' href='javascript:void(0)'> <i class='fa  fa-times-circle'></i></a>";
                    break;

                case "8":
                    statusCss = "<a title='扣量' style='color:#00c0ef' href='javascript:void(0)'> <i class='fa    fa-plus-circle'></i></a>";
                    break;
                default:
                    statusCss = "<a title='？' style='color:#000' href='javascript:void(0)'> <i class='fa   fa-question-circle'></i></a>";

                    break;
            }


            return statusCss;
        }



        //增加数据 状态样式 方法
        protected string getNotifystatStatusStyle(string status)
        {

            string statusCss = string.Empty;
            switch (status)
            {
                case "1":
                    statusCss = "<a title='处理中' style='color:darkorange' href='javascript:void(0)'> <i class='fa   fa-hourglass-end'></i></a>";
                    break;

                case "2":

                    statusCss = "<a title='已完成' style='color:#1db283' href='javascript:void(0)'> <i class='fa  fa-check-circle'></i></a>";
                    break;

                case "4":

                    statusCss = "<a title='失败'  style='color:#ff4a4a' href='javascript:void(0)'> <i class='fa  fa-times-circle'></i></a>";
                    break;


                default:
                    statusCss = "<a title='？' style='color:#000' href='javascript:void(0)'> <i class='fa   fa-question-circle'></i></a>";

                    break;
            }


            return statusCss;
        }




        private void LoadData()
        {
            try
            {
                List<SearchParam> searchParams = new List<SearchParam>();
                searchParams.Add(new SearchParam("ordertype", 1));
                searchParams.Add(new SearchParam("status", "<>", 1));
                int result = 0;
                if (!string.IsNullOrEmpty(this.txtOrderId.Text.Trim()))
                {
                    searchParams.Add(new SearchParam("orderId_like", this.txtOrderId.Text));
                }
                if (!string.IsNullOrEmpty(this.txtMerchantName.Text.Trim()))
                {
                    searchParams.Add(new SearchParam("MerchantName", this.txtMerchantName.Text.Trim()));
                }
                if ((!string.IsNullOrEmpty(this.ddlChannelType.SelectedValue) && int.TryParse(this.ddlChannelType.SelectedValue, out result)) && (result > 0))
                {
                    searchParams.Add(new SearchParam("typeId", result));
                }
                if (!string.IsNullOrEmpty(this.txtUserOrder.Text.Trim()))
                {
                    searchParams.Add(new SearchParam("userorder", this.txtUserOrder.Text.Trim()));
                }
                if (!string.IsNullOrEmpty(this.txtSuppOrder.Text.Trim()))
                {
                    searchParams.Add(new SearchParam("supplierOrder", this.txtSuppOrder.Text.Trim()));
                }
                if ((!string.IsNullOrEmpty(this.ddlOrderStatus.SelectedValue) && int.TryParse(this.ddlOrderStatus.SelectedValue, out result)) && (result > 0))
                {
                    searchParams.Add(new SearchParam("status", result));
                }
                if ((!string.IsNullOrEmpty(this.ddlNotifyStatus.SelectedValue) && int.TryParse(this.ddlNotifyStatus.SelectedValue, out result)) && (result > 0))
                {
                    searchParams.Add(new SearchParam("notifystat", result));
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

                DataSet set = new OrderBank().AdminPageSearch(searchParams, this.Pager1.PageSize, this.Pager1.CurrentPageIndex);
                DataTable table = set.Tables[0];

                this.rptOrders.DataSource = table;
                this.rptOrders.DataBind();

                this.Pager1.RecordCount = table.Rows.Count;

                if (this.currPage > -1)
                {
                    this.Pager1.CurrentPageIndex = this.currPage;
                }
                
                if (table.Rows.Count >= 0)
                {
                    this.TotalTranATM = Convert.ToDecimal(table.Compute("sum(realvalue)", "true").ToString()).ToString("f2");
                    this.TotalUserATM = Convert.ToDecimal(table.Compute("sum(payAmt)", "true").ToString()).ToString("f2");
                    this.TotalCommission = Convert.ToDecimal(table.Compute("sum(commission)", "true").ToString()).ToString("f2");
                    this.TotalProfit = Convert.ToDecimal(table.Compute("sum(profits)", "true").ToString()).ToString("f2");
                }
            }
            catch (Exception err)
            {
                OriginalStudio.Lib.Logging.LogHelper.Write("BankReportList页面错误：" + err.Message.ToString());
            }
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
                this.InitForm();
                this.LoadData();
            }
        }

        protected void Pager1_PageChanged(object sender, EventArgs e)
        {
            this.LoadData();
        }

        protected void rptOrders_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string url = string.Concat(new object[] { 
                "BankReportList.aspx?status=", this.ddlOrderStatus.SelectedValue, "&ctype=", this.ddlChannelType.SelectedValue, "&userid=", this.txtMerchantName.Text, "&ns=", this.ddlNotifyStatus.SelectedValue, "&stime=", this.StimeBox.Text, "&etime=", this.EtimeBox.Text, "&mid=", this.ddlmange.SelectedValue, "&orderid=", this.txtOrderId.Text, 
                "&userorder=", this.txtUserOrder.Text, "&supporder=", this.txtSuppOrder.Text, "&currpage=", this.Pager1.CurrentPageIndex
             });
            try
            {
                if (e.CommandName == "Reissue")
                {
                    string str2 = e.CommandArgument.ToString();
                    if (!string.IsNullOrEmpty(str2))
                    {
                        base.AlertAndRedirect("返回：" + new OrderBankNotify().SynchronousNotify(str2), url);
                    }
                }
            }
            catch (Exception exception)
            {
                base.AlertAndRedirect(exception.Message, url);
            }
        }

        protected void rptOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
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

        protected int ctype
        {
            get
            {
                return WebBase.GetQueryStringInt32("ctype", -1);
            }
        }

        protected int currPage
        {
            get
            {
                return WebBase.GetQueryStringInt32("currpage", -1);
            }
        }

        protected string etime
        {
            get
            {
                return WebBase.GetQueryStringString("etime", string.Empty);
            }
        }

        protected string kano
        {
            get
            {
                return WebBase.GetQueryStringString("ka", string.Empty);
            }
        }

        protected int MID
        {
            get
            {
                return WebBase.GetQueryStringInt32("mid", -1);
            }
        }

        protected int NotifyStatus
        {
            get
            {
                return WebBase.GetQueryStringInt32("ns", -1);
            }
        }

        protected int Status
        {
            get
            {
                return WebBase.GetQueryStringInt32("status", -1);
            }
        }

        protected string stime
        {
            get
            {
                return WebBase.GetQueryStringString("stime", string.Empty);
            }
        }

        protected string supporderid
        {
            get
            {
                return WebBase.GetQueryStringString("supporder", string.Empty);
            }
        }

        protected string sysorderid
        {
            get
            {
                return WebBase.GetQueryStringString("orderid", string.Empty);
            }
        }

        protected int UserId
        {
            get
            {
                return WebBase.GetQueryStringInt32("userid", -1);
            }
        }

        protected string userorderid
        {
            get
            {
                return WebBase.GetQueryStringString("userorder", string.Empty);
            }
        }
    }
}

