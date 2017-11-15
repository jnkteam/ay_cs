namespace KuaiCard.WebUI.Manage.Withdraw
{
    using KuaiCard.BLL;
    using KuaiCard.BLL.Withdraw;
    using KuaiCard.Model;
    using KuaiCard.WebComponents.Web;
    using KuaiCardLib.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Wuqi.Webdiyer;

    public class AgentDistNotifys : ManagePageBase
    {
        protected Button btnSearch;
        protected DropDownList ddlnotifystatus;
        protected HtmlForm form1;
        protected HtmlHead Head1;
        protected settledAgentNotify notifyBLL = new settledAgentNotify();
        protected AspNetPager Pager1;
        protected Repeater rptList;
        protected string totalMoney = "0.00元";
        protected TextBox txtEtimeBox;
        protected TextBox txtout_trade_no;
        protected TextBox txtStimeBox;
        protected TextBox txttrade_no;
        protected TextBox txtUserId;

        private void BindData()
        {
            List<SearchParam> searchParams = new List<SearchParam>();
            int result = 0;
            if (!(string.IsNullOrEmpty(this.txtUserId.Text.Trim()) || !int.TryParse(this.txtUserId.Text.Trim(), out result)))
            {
                searchParams.Add(new SearchParam("userid", result));
            }
            if (!string.IsNullOrEmpty(this.txttrade_no.Text.Trim()))
            {
                searchParams.Add(new SearchParam("trade_no", this.txttrade_no.Text.Trim()));
            }
            if (!string.IsNullOrEmpty(this.txtout_trade_no.Text.Trim()))
            {
                searchParams.Add(new SearchParam("out_trade_no", this.txtout_trade_no.Text.Trim()));
            }
            if (!string.IsNullOrEmpty(this.ddlnotifystatus.SelectedValue))
            {
                searchParams.Add(new SearchParam("tranapi", int.Parse(this.ddlnotifystatus.SelectedValue)));
            }
            if (!string.IsNullOrEmpty(this.ddlnotifystatus.SelectedValue))
            {
                searchParams.Add(new SearchParam("notifystatus", int.Parse(this.ddlnotifystatus.SelectedValue)));
            }
            DataSet set = this.notifyBLL.PageSearch(searchParams, this.Pager1.PageSize, this.Pager1.CurrentPageIndex, string.Empty);
            this.Pager1.RecordCount = Convert.ToInt32(set.Tables[0].Rows[0][0]);
            DataTable table = set.Tables[1];
            this.rptList.DataSource = table;
            this.rptList.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            if (!base.IsPostBack)
            {
                this.txtStimeBox.Attributes.Add("onFocus", "WdatePicker()");
                this.txtEtimeBox.Attributes.Add("onFocus", "WdatePicker()");
                this.txtStimeBox.Text = DateTime.Now.ToString("yyyy-MM-01");
                this.txtEtimeBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.BindData();
            }
        }

        protected void Pager1_PageChanged(object sender, EventArgs e)
        {
            this.BindData();
        }

        private void setPower()
        {
            if (!ManageFactory.CheckCurrentPermission(false, ManageRole.Financial))
            {
                base.Response.Write("Sorry,No authority!");
                base.Response.End();
            }
        }
    }
}

