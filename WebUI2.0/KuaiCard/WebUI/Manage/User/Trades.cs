﻿namespace OriginalStudio.WebUI.Manage.User
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
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Wuqi.Webdiyer;

    public class Trades : ManagePageBase
    {
        protected Button btnSearch;
        protected TextBox EtimeBox;
        protected HtmlForm form1;
        protected HtmlHead Head1;
        protected AspNetPager Pager1;
        protected Repeater rptTrades;
        protected HtmlInputHidden selectedUsers;
        protected TextBox StimeBox;
        protected TextBox txtMerchantName;
        protected string wzfmoney = string.Empty;
        protected string yzfmoney = string.Empty;

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }

        private void LoadData()
        {
            List<SearchParam> searchParams = new List<SearchParam>();
            if (base.currentManage.isSuperAdmin <= 0)
            {
            }
            if (!String.IsNullOrEmpty(this.txtMerchantName.Text.Trim()))
            {
                searchParams.Add(new SearchParam("merchantname", this.txtMerchantName.Text.Trim()));
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
            DataSet set = TradeFactory.PageSearch(searchParams, this.Pager1.PageSize, this.Pager1.CurrentPageIndex, orderby);
            this.Pager1.RecordCount = Convert.ToInt32(set.Tables[0].Rows[0][0]);
            this.rptTrades.DataSource = set.Tables[1];
            this.rptTrades.DataBind();
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
            }
        }

        protected void Pager1_PageChanged(object sender, EventArgs e)
        {
            this.LoadData();
        }

        protected void Pager1_PageChanging(object src, PageChangingEventArgs e)
        {
            this.LoadData();
        }

        protected void rptUsersItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }

        private void setPower()
        {
            if (!ManageFactory.CheckCurrentPermission(false, ManageRole.Financial))
            {
                base.Response.Write("Sorry,No authority!");
                base.Response.End();
            }
        }

        public string cmd
        {
            get
            {
                return WebBase.GetQueryStringString("cmd", "");
            }
        }

        public string orderBy
        {
            get
            {
                return WebBase.GetQueryStringString("orderby", "balance");
            }
        }

        public string orderByType
        {
            get
            {
                return WebBase.GetQueryStringString("type", "asc");
            }
        }

        public int proid
        {
            get
            {
                return WebBase.GetQueryStringInt32("proid", 0);
            }
        }

        public int UserID
        {
            get
            {
                return WebBase.GetQueryStringInt32("ID", 0);
            }
        }

        public string UserStatus
        {
            get
            {
                return WebBase.GetQueryStringString("UserStatus", "");
            }
        }
    }
}

