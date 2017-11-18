namespace OriginalStudio.WebUI.Manage.Order
{
    using Aspose.Cells;
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model;
    using OriginalStudio.Lib.SysConfig;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Wuqi.Webdiyer;
    using OriginalStudio.BLL.Supplier;

    public class BankOrderList : ManagePageBase
    {
        protected Button btn_Search;
        protected Button btnExport;
        protected DropDownList ddlChannelType;
        protected DropDownList ddlmange;
        protected DropDownList ddlNotifyStatus;
        protected DropDownList ddlOrderStatus;
        protected DropDownList ddlsupp;
        protected TextBox EtimeBox;
        protected HtmlForm form1;
        protected AspNetPager Pager1;
        protected Repeater rptOrders;
        protected HtmlGenericControl spangmmoney;
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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet data = this.GetData();
                if (data != null)
                {
                    DataTable dataTable = data.Tables[1];
                    dataTable.TableName = "Rpt";
                    string file = base.Server.MapPath("~/common/template/xls/cards.xls");
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

        private DataSet GetData()
        {
            List<SearchParam> searchParams = new List<SearchParam>();
            int result = 0;
            if (!base.isSuperAdmin)
            {
                searchParams.Add(new SearchParam("manageId", base.ManageId));
            }
            else if (!(string.IsNullOrEmpty(this.ddlmange.SelectedValue) || !int.TryParse(this.ddlmange.SelectedValue, out result)))
            {
                searchParams.Add(new SearchParam("manageId", result));
            }
            if (!(string.IsNullOrEmpty(this.txtOrderId.Text.Trim()) || !int.TryParse(this.txtOrderId.Text.Trim(), out result)))
            {
                searchParams.Add(new SearchParam("orderid", result));
            }
            if (!(string.IsNullOrEmpty(this.txtuserid.Text.Trim()) || !int.TryParse(this.txtuserid.Text.Trim(), out result)))
            {
                searchParams.Add(new SearchParam("userid", result));
            }
            if ((!string.IsNullOrEmpty(this.ddlChannelType.SelectedValue) && int.TryParse(this.ddlChannelType.SelectedValue, out result)) && (result > 0))
            {
                searchParams.Add(new SearchParam("typeId", result));
            }
            if (!(string.IsNullOrEmpty(this.txtUserOrder.Text.Trim()) || !int.TryParse(this.txtUserOrder.Text.Trim(), out result)))
            {
                searchParams.Add(new SearchParam("userorder", result));
            }
            if (!(string.IsNullOrEmpty(this.txtSuppOrder.Text.Trim()) || !int.TryParse(this.txtSuppOrder.Text.Trim(), out result)))
            {
                searchParams.Add(new SearchParam("supplierOrder", result));
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
            OrderBank bank = new OrderBank();
            return bank.PageSearch(searchParams, 0x2710, 1, orderby);
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
            string where = "IsBank=1";
            DataTable table = SysSupplierFactory.GetList(where).Tables[0];
            this.ddlsupp.Items.Add(new ListItem("--接口商--", ""));
            foreach (DataRow row in table.Rows)
            {
                this.ddlsupp.Items.Add(new ListItem(row["SupplierName"].ToString(), row["SupplierCode"].ToString()));
            }
            this.ddlmange.Items.Add("--请选择业务员--");
            DataTable table2 = ManageFactory.GetList(" status =1").Tables[0];
            foreach (DataRow row in table2.Rows)
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
            if ((!string.IsNullOrEmpty(this.ddlsupp.SelectedValue) && int.TryParse(this.ddlsupp.SelectedValue, out result)) && (result > 0))
            {
                searchParams.Add(new SearchParam("supplierId", result));
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
            DataSet set = new OrderBank().AdminPageSearch(searchParams, this.Pager1.PageSize, this.Pager1.CurrentPageIndex, orderby);
            Response.Write(set);
            /*
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
                   this.TotalTranATM = Convert.ToDecimal(table.Rows[0]["realvalue"]).ToString("f2");   //总额：0.00 
               }
               if (table.Rows[0]["payAmt"] != DBNull.Value)
               {
                   this.TotalUserATM = Convert.ToDecimal(table.Rows[0]["payAmt"]).ToString("f2");  //商户所得：0.00 
               }
               if (table.Rows[0]["commission"] != DBNull.Value)
               {
                   this.TotalCommission = Convert.ToDecimal(table.Rows[0]["commission"]).ToString("f2");   //业务总提成：0.00 
               }
               if (table.Rows[0]["profits"] != DBNull.Value)
               {
                   this.TotalProfit = Convert.ToDecimal(table.Rows[0]["profits"]).ToString("f2");  //平台利润：0.00 
               }
           }
           */
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

        //增加数据 状态样式 方法
        protected string getStatusStyle(string status) {

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



        protected void rptOrders_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string url = string.Concat(new object[] { 
                "BankOrderList.aspx?status=", this.ddlOrderStatus.SelectedValue, "&ctype=", this.ddlChannelType.SelectedValue, "&userid=", this.txtuserid.Text, "&ns=", this.ddlNotifyStatus.SelectedValue, "&stime=", this.StimeBox.Text, "&etime=", this.EtimeBox.Text, "&mid=", this.ddlmange.SelectedValue, "&orderid=", this.txtOrderId.Text, 
                "&userorder=", this.txtUserOrder.Text, "&supporder=", this.txtSuppOrder.Text, "&currpage=", this.Pager1.CurrentPageIndex
             });
            try
            {
                string str2;
                if (e.CommandName == "Reissue")
                {
                    //手动回发
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
            HtmlTableCell cell;
            HtmlTableCell cell2;
           
            if ((e.Item.ItemType == ListItemType.Header) && !base.isSuperAdmin)
            {
                cell = e.Item.FindControl("th_profits") as HtmlTableCell;
                if (cell != null)
                {
                    cell.Visible = false;
                }
                cell2 = e.Item.FindControl("th_supplier") as HtmlTableCell;
                if (cell2 != null)
                {
                    cell2.Visible = false;
                }
            }
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                if (!base.isSuperAdmin)
                {
                    cell = e.Item.FindControl("tr_profits") as HtmlTableCell;
                    if (cell != null)
                    {
                        cell.Visible = false;
                    }
                    cell2 = e.Item.FindControl("tr_supplier") as HtmlTableCell;
                    if (cell2 != null)
                    {
                        cell2.Visible = false;
                    }
                }
                Button button = e.Item.FindControl("btnReissue") as Button;
                Button button2 = e.Item.FindControl("btnRest") as Button;
                Button button3 = e.Item.FindControl("btnDeduct") as Button;
                Button button4 = e.Item.FindControl("btnReDeduct") as Button;
                int userId = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "userid").ToString());
                switch (DataBinder.Eval(e.Item.DataItem, "status").ToString())
                {
                    case "1":
                        button.Visible = false;
                        button2.Visible = true;
                        button3.Visible = false;
                        button4.Visible = false;
                        
                        break;

                    case "2":
                    {
                        button.Visible = true;
                        button2.Visible = false;
                        button4.Visible = false;
                        button3.OnClientClick = "return confirm('是否确定扣量？')";
                        object completeTime = DataBinder.Eval(e.Item.DataItem, "CompleteTime");
                        double difftime = this.GetDifftime(userId, completeTime);
                        if (difftime > 0)   //RuntimeSetting.DeductSafetyTime)
                        {
                            button3.Text = "扣";
                        }
                        else if ((difftime > 0.0) && (difftime <= 0))    //RuntimeSetting.DeductSafetyTime))
                        {
                            button3.Text = "危险";
                        }
                        else
                        {
                            button3.Text = "不能";
                        }
                           
                            break;
                    }
                    case "4":
                        button.Visible = true;
                        button2.Visible = false;
                        button3.Visible = false;
                        button4.Visible = false;
                        
                        break;

                    case "8":
                        button.Visible = true;
                        button2.Visible = false;
                        button3.Visible = false;
                        button4.Visible = true;
                        break;
                }
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

        protected int ChannelType
        {
            get
            {
                return WebBase.GetQueryStringInt32("ChannelType", -1);
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

