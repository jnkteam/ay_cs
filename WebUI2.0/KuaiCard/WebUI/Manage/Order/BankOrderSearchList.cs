namespace KuaiCard.WebUI.Manage.Order
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

    public class BankOrderSearchList : ManagePageBase
    {
        protected Button btn_Search;
        protected HtmlForm form1;
        protected AspNetPager Pager1;
        protected Repeater rptOrders;
        protected HtmlGenericControl spangmmoney;
        protected TextBox txtOrderId;
        protected TextBox txtSuppOrder;
        protected TextBox txtUserOrder;

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }

        private DataSet GetData()
        {
            List<SearchParam> searchParams = new List<SearchParam>();
            int result = 0;
            if (!(string.IsNullOrEmpty(this.txtOrderId.Text.Trim()) || !int.TryParse(this.txtOrderId.Text.Trim(), out result)))
            {
                searchParams.Add(new SearchParam("orderid", result));
            }
            if (!(string.IsNullOrEmpty(this.txtUserOrder.Text.Trim()) || !int.TryParse(this.txtUserOrder.Text.Trim(), out result)))
            {
                searchParams.Add(new SearchParam("userorder", result));
            }
            if (!(string.IsNullOrEmpty(this.txtSuppOrder.Text.Trim()) || !int.TryParse(this.txtSuppOrder.Text.Trim(), out result)))
            {
                searchParams.Add(new SearchParam("supplierOrder", result));
            }
            string orderby = string.Empty;
            OrderBank bank = new OrderBank();
            return bank.PageSearch(searchParams, 0x2710, 1, orderby);
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

        private void LoadData()
        {
            if (this.txtOrderId.Text.Trim() == "" && this.txtSuppOrder.Text.Trim() == "" && this.txtUserOrder.Text.Trim() == "")
                return;

            List<SearchParam> searchParams = new List<SearchParam>();
            if (!string.IsNullOrEmpty(this.txtOrderId.Text.Trim()))
            {
                searchParams.Add(new SearchParam("orderId_like", this.txtOrderId.Text));
            }
            if (!string.IsNullOrEmpty(this.txtUserOrder.Text.Trim()))
            {
                searchParams.Add(new SearchParam("userorder", this.txtUserOrder.Text.Trim()));
            }
            if (!string.IsNullOrEmpty(this.txtSuppOrder.Text.Trim()))
            {
                searchParams.Add(new SearchParam("supplierOrder", this.txtSuppOrder.Text.Trim()));
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

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
        }

        protected void rptOrders_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string url = string.Concat(new object[] { 
                "BankOrderSearchList.aspx?&orderid=", this.txtOrderId.Text, 
                "&userorder=", this.txtUserOrder.Text, "&supporder=", this.txtSuppOrder.Text, "&currpage=", this.Pager1.CurrentPageIndex
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
            }
            catch (Exception exception)
            {
                base.AlertAndRedirect(exception.Message, url);
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

        protected int currPage
        {
            get
            {
                return WebBase.GetQueryStringInt32("currpage", -1);
            }
        }

        protected string kano
        {
            get
            {
                return WebBase.GetQueryStringString("ka", string.Empty);
            }
        }

        protected string sysorderid
        {
            get
            {
                return WebBase.GetQueryStringString("orderid", string.Empty);
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

