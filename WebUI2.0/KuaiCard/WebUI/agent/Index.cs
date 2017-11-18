﻿namespace OriginalStudio.WebUI.agent
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Order;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.TimeControl;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Index : AgentPageBase
    {
        protected string balance = "0.00";
        protected HtmlForm form1;
        protected Literal litlinks;
        protected string loginip;
        protected string logintime;
        protected string monthcommission = "0.00";
        protected string monthtotalAmt = "0.00";
        protected HtmlGenericControl paysouid;
        protected string todaytotalAmt = "0.00";
        protected string username;
        protected string userscount;
        protected string yeartotalAmt = "0.00";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                try
                {
                    this.litlinks.Text = "?agent=" + base.CurrentUser.ID.ToString();
                    this.userscount = PromotionUserFactory.GetUserNum(base.UserId).ToString();
                    DateTime today = DateTime.Today;
                    DateTime edt = DateTime.Today.AddDays(1.0);
                    decimal num = Dal.GetAgentTotalAmt(base.UserId, today, edt);
                    this.todaytotalAmt = num.ToString("f2");
                    today = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-01 00:00:00"));
                    num = Dal.GetAgentTotalAmt(base.UserId, today, edt);
                    decimal num2 = Dal.GetAgentIncome(base.UserId, today, edt);
                    this.monthtotalAmt = num.ToString("f2");
                    this.monthcommission = num2.ToString("f2");
                    today = Convert.ToDateTime(DateTime.Now.ToString("yyyy-01-01 00:00:00"));
                    this.yeartotalAmt = Dal.GetAgentTotalAmt(base.UserId, today, edt).ToString("f2");
                    try
                    {
                        this.balance = ((base.CurrentUserAmt.Balance - base.CurrentUserAmt.Freeze) - base.CurrentUserAmt.UnPayment).ToString("f2");
                    }
                    catch
                    {
                    }
                    this.loginip = base.CurrentUser.LastLoginIp;
                    this.logintime = FormatConvertor.DateTimeToTimeString(base.CurrentUser.LastLoginTime);
                    this.username = base.CurrentUser.UserName;
                }
                catch
                {
                }
            }
        }
    }
}

