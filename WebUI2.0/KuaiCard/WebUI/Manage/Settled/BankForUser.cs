﻿namespace OriginalStudio.WebUI.Manage.Settled
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Settled;
    using OriginalStudio.ETAPI;
    using OriginalStudio.Model;
    using OriginalStudio.Model.Settled;
    using OriginalStudio.Model.User;
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

    public class BankForUser : ManagePageBase
    {
        protected Button btnAllSettle;
        protected Button btnBatchSettle;
        protected Button btnSearch;
        //private ChannelWithdraw chnlBLL = new ChannelWithdraw();
        protected HtmlForm form1;
        protected HtmlHead Head1;
        protected AspNetPager Pager1;
        protected Repeater rptUsers;
        protected HtmlInputHidden selectedUsers;
        protected TextBox txtbalance;
        protected TextBox txtPassWord;
        protected TextBox txtMerchantName;
        protected string wzfmoney = string.Empty;
        protected string yzfmoney = string.Empty;
        protected string TotalMoney = "";       //2017.2.13增加。目的是前台能看到总余额

        protected void btnAllSettle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtPassWord.Text))
            {
                base.AlertAndRedirect("请输入二级密码");
            }
            else if (!ManageFactory.SecPwdVaild(this.txtPassWord.Text.Trim()))
            {
                base.AlertAndRedirect("二级密码不正确");
            }
            else
            {
                decimal result = 0M;
                if (!string.IsNullOrEmpty(this.txtbalance.Text))
                {
                    decimal.TryParse(this.txtbalance.Text, out result);
                }
                if (SettledFactory.AutoSettled(result))
                {
                    base.AlertAndRedirect("结算成功", "SettledAudit.aspx");
                }
                else
                {
                    base.AlertAndRedirect("结算失败", "SettledAudit.aspx");
                }
            }
        }

        protected void btnBatchSettle_Click(object sender, EventArgs e)
        {
            int num = 0;
            int num2 = 0;
            decimal num3 = 0M;
            foreach (RepeaterItem item in this.rptUsers.Items)
            {
                if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
                {
                    HtmlInputCheckBox box = item.FindControl("chkItem") as HtmlInputCheckBox;
                    if (box.Checked)
                    {
                        HiddenField field = item.FindControl("hfuserid") as HiddenField;
                        num++;
                        int userId = 0;
                        decimal settleAmt = 0M;
                        Model.User.MchUserBaseInfo ub = new MchUserBaseInfo();
                        TextBox box2 = item.FindControl("txtpayAmt") as TextBox;
                        try
                        {
                            userId = Convert.ToInt32(field.Value);
                            settleAmt = Convert.ToDecimal(box2.Text.Trim());
                            ub = BLL.Settled.MchUserFactory.GetUserBaseByUserID(userId);

                        }
                        catch
                        {
                        }
                        if (userId > 0 && settleAmt > 0M
                            && !String.IsNullOrEmpty(ub.MchUserPayBankInfo.AccountName)
                            && !String.IsNullOrEmpty(ub.MchUserPayBankInfo.BankName)
                            && this.Settle(ub, settleAmt) == "OK")
                        {
                            num2++;
                            num3 += settleAmt;
                        }
                    }
                }
            }
            base.AlertAndRedirect(string.Format("总处理提现总条数{0} 其中成功条数{1} 成功金额{2:0.00}", num, num2, num3), "BankForUser.aspx");
        }

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
            decimal result = 0M;
            if (!string.IsNullOrEmpty(this.txtbalance.Text))
            {
                decimal.TryParse(this.txtbalance.Text, out result);
            }
            List<SearchParam> searchParams = new List<SearchParam>();
            searchParams.Add(new SearchParam("enableAmt", ">", result));
            if (!base.isSuperAdmin)
            {
                searchParams.Add(new SearchParam("manageId", base.ManageId));
            }
            if (this.proid > 0)
            {
                searchParams.Add(new SearchParam("proid", this.proid));
            }
            string merchantname = this.txtMerchantName.Text.Trim();
            if (!string.IsNullOrEmpty(merchantname))
            {
                searchParams.Add(new SearchParam("merchantname", merchantname));
            }
            string orderby = this.orderBy + " " + this.orderByType;
            DataSet set = MchUserFactory.PageSearch(searchParams, this.Pager1.PageSize, this.Pager1.CurrentPageIndex, orderby);
            this.Pager1.RecordCount = Convert.ToInt32(set.Tables[0].Rows[0][0]);
            this.TotalMoney = set.Tables[1].Compute("Sum(balance)", "").ToString(); //2017.2.13增加
            this.rptUsers.DataSource = set.Tables[1];
            this.rptUsers.DataBind();

            this.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            if (!base.IsPostBack)
            {
                this.txtbalance.Attributes["onkeypress"] = "if (event.keyCode < 45 || event.keyCode > 57) event.returnValue = false;";
                this.LoadData();
            }
        }

        protected void Pager1_PageChanged(object sender, EventArgs e)
        {
            this.LoadData();
        }

        protected void Pager1_PageChanging(object sender, EventArgs e)
        {
            this.LoadData();
        }

        protected void rptUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Settled")
            {
                int userId = 0;
                decimal settleAmt = 0M;
                Model.User.MchUserBaseInfo ub = new MchUserBaseInfo();
                TextBox box = e.Item.FindControl("txtpayAmt") as TextBox;
                try
                {
                    userId = Convert.ToInt32(e.CommandArgument);
                    settleAmt = Convert.ToDecimal(box.Text.Trim());
                    ub = BLL.Settled.MchUserFactory.GetUserBaseByUserID(userId);
                }
                catch
                {
                }
                if ((userId <= 0) || (settleAmt <= 0M) || ub.MchUserPayBankInfo.AccountName == "" ||
                    ub.MchUserPayBankInfo.BankName == "")
                {
                    base.AlertAndRedirect("参数不齐全!");
                }
                else
                {
                    string msg = this.Settle(ub, settleAmt);
                    if (msg == "OK")
                    {
                        base.AlertAndRedirect("提现成功", "BankForUser.aspx");
                    }
                    else
                    {
                        base.AlertAndRedirect("提现失败，原因：" + msg, "BankForUser.aspx");
                    }
                }
            }
        }

        protected void rptUsersItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                //OriginalStudio.Lib.Logging.LogHelper.Write("111111");

                TextBox box = (TextBox) e.Item.FindControl("txtpayAmt");
                Literal literal = (Literal) e.Item.FindControl("litTodayIncome");

                //OriginalStudio.Lib.Logging.LogHelper.Write("222222");

                box.Attributes["onkeypress"] = "if (event.keyCode < 45 || event.keyCode > 57) event.returnValue = false;";
                box.Text = "0";
                int userId = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "userid"));

                //OriginalStudio.Lib.Logging.LogHelper.Write("userId：" + userId.ToString());

                //6、检查可提现金额
                decimal allLimitIncome = BLL.Settled.TradeFactory.GetMerchantLimitDayIncome(userId);
                MchUserBaseInfo mchInfo = MchUserFactory.GetUserBaseByUserID(userId);
                decimal canPayMoney = mchInfo.MchUsersAmtInfo.Balance -
                                                        mchInfo.MchUsersAmtInfo.Freeze -
                                                        mchInfo.MchUsersAmtInfo.UnPayment -
                                                        allLimitIncome;

                //OriginalStudio.Lib.Logging.LogHelper.Write("Balance：" + mchInfo.MchUsersAmtInfo.Balance.ToString());
                //OriginalStudio.Lib.Logging.LogHelper.Write("Freeze：" + mchInfo.MchUsersAmtInfo.Freeze.ToString());
                //OriginalStudio.Lib.Logging.LogHelper.Write("UnPayment：" + mchInfo.MchUsersAmtInfo.UnPayment.ToString());
                //OriginalStudio.Lib.Logging.LogHelper.Write("canPayMoney：" + canPayMoney.ToString());

                //扣押金额
                literal.Text = allLimitIncome.ToString();
                box.Text = canPayMoney.ToString();
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

        /// <summary>
        /// 后台提现方法！！！！
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="settleAmt"></param>
        /// <returns></returns>
        private string Settle(Model.User.MchUserBaseInfo ub, decimal settleAmt)
        {
            string web_return = OriginalStudio.BLL.Settled.SettledFactory.InvokeSettleInterface(ub.MerchantName,
                                                        ub.MchUserPayBankInfo.BankCode.ToString(),
                                                        ub.MchUserPayBankInfo.BankAccount,
                                                        ub.MchUserPayBankInfo.BankName,
                                                        ub.MchUserPayBankInfo.AccountName,
                                                        SettlePayTypeEnum.管理后台,
                                                        SettledModeEnum.手动提现,
                                                        settleAmt, settleAmt * ub.WithdrawScheme.ChargeRate, 0, "/");

            Dictionary<string, string> dicRtn = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(web_return);

            string respCode = dicRtn["result"].ToUpper();
            if (respCode == "OK")
            {
                return respCode;//
            }
            else
            {
                string msg = dicRtn["msg"].ToUpper();
                //base.AlertAndRedirect("提现失败，原因：" + msg, "BankForUser.aspx");
                return msg;
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

