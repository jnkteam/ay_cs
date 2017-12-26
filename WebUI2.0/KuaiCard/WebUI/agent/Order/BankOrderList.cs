﻿namespace OriginalStudio.WebUI.agent.Order
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Settled;
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

    public class BankOrderList : AgentPageBase
    {
        protected Button btn_Search;
        protected DropDownList ddlChannelType;
        protected DropDownList ddlmange;
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
        protected TextBox txtuserid;
        protected TextBox txtUserOrder;

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }

        public double GetDifftime(int userId, object completeTime)
        {
            DateTime minValue = DateTime.MinValue;
            if (UserAccessTime.GetModel(userId) == null)
            {
                return 1000.0;
            }
            DateTime? nullable = new DateTime?(UserAccessTime.GetModel(userId).lastAccesstime);
            if (nullable.HasValue)
            {
                minValue = nullable.Value;
            }
            return Convert.ToDateTime(completeTime).Subtract(minValue).TotalMinutes;
        }

        protected string GetParm(object orderid, object supp, object amt)
        {
            try
            {
                return string.Format("{0}${1}${2}", orderid, supp, amt);
            }
            catch
            {
                return string.Format("{0}${1}${2}", "", "", "0.00");
            }
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
                this.txtuserid.Text = this.UserId.ToString();
            }
            if (this.ctype > -1)
            {
                this.ddlChannelType.SelectedValue = this.ctype.ToString();
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

        private void LoadData()
        {
            List<SearchParam> searchParams = new List<SearchParam>();
            int result = 0;
            searchParams.Add(new SearchParam("agentid", base.CurrentUser.ID));
            if (!string.IsNullOrEmpty(this.txtOrderId.Text.Trim()))
            {
                searchParams.Add(new SearchParam("orderId_like", this.txtOrderId.Text));
            }
            if (!(string.IsNullOrEmpty(this.txtuserid.Text.Trim()) || !int.TryParse(this.txtuserid.Text.Trim(), out result)))
            {
                searchParams.Add(new SearchParam("userid", result));
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
            searchParams.Add(new SearchParam("status", 2));
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
            DataSet set = new OrderBank().PageSearch(searchParams, this.Pager1.PageSize, this.Pager1.CurrentPageIndex, orderby);
            this.Pager1.RecordCount = Convert.ToInt32(set.Tables[0].Rows[0][0]);
            this.rptOrders.DataSource = set.Tables[1];
            this.rptOrders.DataBind();
            if (this.currPage > -1)
            {
                this.Pager1.CurrentPageIndex = this.currPage;
            }
            DataTable table = set.Tables[2];
            if (table.Rows.Count >= 0)
            {
                if (table.Rows[0]["realvalue"] != DBNull.Value)
                {
                    this.TotalTranATM = Convert.ToDecimal(table.Rows[0]["realvalue"]).ToString("f2");
                }
                if (table.Rows[0]["payAmt"] != DBNull.Value)
                {
                    this.TotalUserATM = Convert.ToDecimal(table.Rows[0]["payAmt"]).ToString("f2");
                }
                if (table.Rows[0]["promAmt"] != DBNull.Value)
                {
                    this.TotalCommission = Convert.ToDecimal(table.Rows[0]["promAmt"]).ToString("f2");
                }
                if (table.Rows[0]["profits"] != DBNull.Value)
                {
                    this.TotalProfit = Convert.ToDecimal(table.Rows[0]["profits"]).ToString("f2");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
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
            string url = "BankOrderList.aspx?status=2";
            url = string.Concat(new object[] { 
                url, "&ctype=", this.ddlChannelType.SelectedValue, "&userid=", this.txtuserid.Text, "&stime=", this.StimeBox.Text, "&etime=", this.EtimeBox.Text, "&mid=", this.ddlmange.SelectedValue, "&orderid=", this.txtOrderId.Text, "&userorder=", this.txtUserOrder.Text, "&supporder=", 
                this.txtSuppOrder.Text, "&currpage=", this.Pager1.CurrentPageIndex
             });
            try
            {
                string str2;
                if (e.CommandName == "Reissue")
                {
                    str2 = e.CommandArgument.ToString();
                    if (!string.IsNullOrEmpty(str2))
                    {
                        base.AlertAndRedirect("返回：" + new OrderBankNotify().SynchronousNotify(str2), url);
                    }
                }
                else if (e.CommandName == "ResetOrder")
                {
                    string str4 = e.CommandArgument.ToString();
                    if (!string.IsNullOrEmpty(str4))
                    {
                        string[] strArray = str4.Split(new char[] { '$' });
                        base.Response.Redirect(string.Format("ResetOrder.aspx?orderid={0}&oclass=1&supp={1}&amt={2}", strArray[0], strArray[1], strArray[2]));
                    }
                }
                else
                {
                    OrderBank bank;
                    if (e.CommandName == "Deduct")
                    {
                        str2 = e.CommandArgument.ToString();
                        bank = new OrderBank();
                        if (bank.Deduct(str2))
                        {
                            base.AlertAndRedirect("扣量成功", url);
                        }
                        else
                        {
                            base.AlertAndRedirect("扣量失败，可能是余额不足", url);
                        }
                        this.LoadData();
                    }
                    else if (e.CommandName == "ReDeduct")
                    {
                        str2 = e.CommandArgument.ToString();
                        bank = new OrderBank();
                        if (bank.ReDeduct(str2))
                        {
                            base.AlertAndRedirect("还单成功", url);
                        }
                        else
                        {
                            base.AlertAndRedirect("还单失败", url);
                        }
                        this.LoadData();
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

