﻿namespace OriginalStudio.WebUI.Manage.User
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Settled;
    using OriginalStudio.BLL.Settled;
    using OriginalStudio.Model;
    using OriginalStudio.Model.Settled;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Wuqi.Webdiyer;
    using OriginalStudio.Model.User;

    public class Freeze : ManagePageBase
    {
        protected Button btnSearch;
        protected HtmlForm form1;
        protected HtmlHead Head1;
        protected AspNetPager Pager1;
        protected Repeater rptUsers;
        protected HtmlInputHidden selectedUsers;
        protected TextBox txtMerchantName;
        protected string wzfmoney = string.Empty;
        protected string yzfmoney = string.Empty;

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }

        protected string GetParm(object userid, object balance, object Freeze, object unpayment)
        {
            try
            {
                decimal num;
                decimal num2;
                decimal num3;
                if (balance == DBNull.Value)
                {
                    num = 0M;
                }
                else
                {
                    num = Convert.ToDecimal(balance);
                }
                if (Freeze == DBNull.Value)
                {
                    num2 = 0M;
                }
                else
                {
                    num2 = Convert.ToDecimal(Freeze);
                }
                if (unpayment == DBNull.Value)
                {
                    num3 = 0M;
                }
                else
                {
                    num3 = Convert.ToDecimal(unpayment);
                }
                return string.Format("{0}${1}${2}${3}", new object[] { userid, num, num2, num3 });
            }
            catch
            {
                return string.Format("{0}${1}${2}${3}", new object[] { "0.00", "0.00", "0.00", "0.00" });
            }
        }

        private void LoadData()
        {
            List<SearchParam> searchParams = new List<SearchParam>();
            searchParams.Add(new SearchParam("balance", ">", 0M));
            if (!base.isSuperAdmin)
            {
                searchParams.Add(new SearchParam("manageId", base.ManageId));
            }
            if (this.proid > 0)
            {
                searchParams.Add(new SearchParam("proid", this.proid));
            }
            string str = this.txtMerchantName.Text.Trim();
            if (!string.IsNullOrEmpty(str))
            {
                searchParams.Add(new SearchParam("merchantname", str));
            }
            string orderby = this.orderBy + " " + this.orderByType;
            DataSet set = MchUserFactory.PageSearch(searchParams, this.Pager1.PageSize, this.Pager1.CurrentPageIndex, orderby);
            this.Pager1.RecordCount = Convert.ToInt32(set.Tables[0].Rows[0][0]);
            this.rptUsers.DataSource = set.Tables[1];
            this.rptUsers.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            if (!base.IsPostBack)
            {
                this.LoadData();
            }
        }

        protected void Pager1_PageChanging(object src, PageChangingEventArgs e)
        {
        }

        protected void Pager1_PageChanging1(object src, PageChangingEventArgs e)
        {
            this.LoadData();
        }

        protected void rptUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "freeze")
            {
                string[] strArray = e.CommandArgument.ToString().Split(new char[] { '$' });
                int num = Convert.ToInt32(strArray[0]);
                decimal result = 0M;
                decimal.TryParse(strArray[1], out result);
                decimal num3 = 0M;
                decimal.TryParse(strArray[3], out num3);
                decimal num4 = 0M;
                decimal.TryParse(strArray[2], out num4);
                TextBox box = e.Item.FindControl("txtFreezeMoney") as TextBox;
                decimal FreezeMoney = decimal.Parse(box.Text.Trim());
                if (FreezeMoney <= 0M)
                {
                    base.AlertAndRedirect("请输入正确的金额");
                }
                else if (FreezeMoney > ((result - num3) - num4))
                {
                    base.AlertAndRedirect("冻结的金额大于余额 操作有误");
                }
                else
                {
                    TextBox box2 = (TextBox) e.Item.FindControl("txtWhy");
                    UsersAmtFreezeInfo model = new UsersAmtFreezeInfo();
                    model.UserID = num;
                    model.Addtime = DateTime.Now;
                    model.FreezeAmt = FreezeMoney;
                    model.ManageId = base.ManageId;
                    model.Status = AmtFreezeInfoStatus.否;
                    model.Why = box2.Text.Trim();
                    model.UnFreezeMode = AmtunFreezeMode.未处理;
                    if (UsersAmtFreeze.Freeze(model))
                    {
                        base.AlertAndRedirect("操作成功");
                    }
                    else
                    {
                        base.AlertAndRedirect("操作失败");
                    }
                }
            }
        }

        protected void rptUsersItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                TextBox box = (TextBox) e.Item.FindControl("txtFreezeMoney");
                box.Attributes["onkeypress"] = "if (event.keyCode < 45 || event.keyCode > 57) event.returnValue = false;";
            }
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

