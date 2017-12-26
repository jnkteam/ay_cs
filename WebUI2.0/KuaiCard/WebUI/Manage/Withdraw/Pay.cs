namespace OriginalStudio.WebUI.Manage.Withdraw
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Settled;
    using OriginalStudio.BLL.Tools;
    using OriginalStudio.BLL.Settled;
    using OriginalStudio.ETAPI;
    using OriginalStudio.Model;
    using OriginalStudio.Model.Settled;
    using OriginalStudio.Model.User;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.ExceptionHandling;
    using OriginalStudio.Lib.TimeControl;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using OriginalStudio.BLL.Supplier;
    using OriginalStudio.Model.Withdraw;
    using System.Collections.Generic;
    using System.Text;

    public class Pay : ManagePageBase
    {
        private SettledInfo _ItemInfo = null;
        private MchUserBaseInfo _userInfo = null;

        protected Label AccountLabel;
        protected Label AddTimeLabel;
        protected Label BankLabel;
        protected Button btnSave;
        protected Button btnSure;
        protected TextBox ChargesBox;
        protected DropDownList ddlSupplier;
        protected Label errLabel;
        protected HtmlForm form1;
        protected Label lblAccount;
        protected Label lblBank;
        protected Label lblPayeeaddress;
        protected Label lblPayeeName;
        protected Label MoneyLabel;
        protected Label PayeeaddressLabel;
        protected Label PayeeNameLabel;
        protected Label PayMoneyLabel;
        protected TextBox TaxBox;
        protected Label UidLabel;
        protected Label UserNameLabel;
        protected Label UserStatusLabel;

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                decimal tax = decimal.Parse(this.TaxBox.Text.Trim());
                decimal charge = decimal.Parse(this.ChargesBox.Text.Trim());
                this.ItemInfo.Tax = tax;
                this.ItemInfo.Charges = charge;
                this.ItemInfo.PayTime = DateTime.Now;
                if (!string.IsNullOrEmpty(this.ddlSupplier.SelectedValue))
                {
                    this.ItemInfo.Suppid = int.Parse(this.ddlSupplier.SelectedValue);
                }
                //这个地方只改  charge  tax，不改动settle_amount
                if (SettledFactory.Update(this.ItemInfo))
                {
                    base.AlertAndRedirect("修改成功");
                }
                else
                {
                    base.AlertAndRedirect("修改失败");
                }
            }
            catch (Exception exception)
            {
                base.AlertAndRedirect(exception.Message);
            }
        }

        protected void btnSure_Click(object sender, EventArgs e)
        {
            string msg = this.DoPay();
            base.AlertAndRedirect(msg);
        }

        private string DoPay()
        {
            try
            {
                if (!string.IsNullOrEmpty(this.ddlSupplier.SelectedValue))
                    this.ItemInfo.Suppid = int.Parse(this.ddlSupplier.SelectedValue);//接口商
                else
                    this.ItemInfo.Suppid = 0;

                string web_return = BLL.Settled.SettledFactory.InvokeSysDistribution(this.ItemInfo.ID, this.ItemInfo.Suppid);

                Dictionary<string, string> dicRtn = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(web_return);

                string respCode = dicRtn["result"].ToUpper();
                if (respCode == "OK")
                {
                    return "代付提交成功。";
                }
                else
                {
                    string msg = dicRtn["msg"].ToUpper();
                    //base.AlertAndRedirect("提现失败，原因：" + msg, "BankForUser.aspx");
                    return msg;
                }

                //********************************************************************//

                /*
                //Step1：先记录
                
                switch (SettledFactory.Pay(this.ItemInfo))
                {
                    case 0:
                    {
                        //未支付
                        string str = SysConfig.sms_temp_tocash;

                        //发送短信
                        //if (!(string.IsNullOrEmpty(str) || string.IsNullOrEmpty(this.userInfo.Tel)))
                        //{
                        //    str = str.Replace("{@username}", this.userInfo.UserName).Replace("{@settledmoney}", this.ItemInfo.amount.ToString("f2"));
                        //    SMS.Send(this.userInfo.Tel, str, "");
                        //}
                        //OriginalStudio.Lib.Logging.LogHelper.Write("提现:DoPay");
                        if (this.ItemInfo.Suppid > 0)
                        {
                            //Step1：走 网关代付。
                            OriginalStudio.ETAPI.Withdraw.InitDistribution(this.ItemInfo);
                        }
                        return "";
                    }
                    case 1:
                        return "状态不正确";

                    case 99:
                        return "未知错误";
                }*/
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return exception.Message;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            if (!base.IsPostBack)
            {
                DataTable table = SysSupplierFactory.GetList("isdistribution=1").Tables[0];
                this.ddlSupplier.Items.Add(new ListItem("不走接口", "0"));
                foreach (DataRow row in table.Rows)
                {
                    this.ddlSupplier.Items.Add(new ListItem(row["SupplierName"].ToString(), row["SupplierCode"].ToString()));
                    //mark:畅捷code:6001，这里只返回一个
                }

                if (((this.ItemInfoId > 0) && (this.ItemInfo != null)) && (this.UserInfo != null))
                {
                    this.PayMoneyLabel.Text = this.ItemInfo.SettleAmount.ToString("c2");
                    this.AddTimeLabel.Text = FormatConvertor.DateTimeToTimeString(this.ItemInfo.AddTime);
                    this.lblPayeeName.Text = this.ItemInfo.PayeeName;
                    this.lblBank.Text = SettledFactory.GetSettleBankName(this.ItemInfo.PayeeBank);
                    this.lblPayeeaddress.Text = this.ItemInfo.PayeeAddress;
                    this.lblAccount.Text = this.ItemInfo.Account;
                    this.TaxBox.Text = this.ItemInfo.Tax.ToString("f2");
                    if (this.ItemInfo.Charges > 0)
                    {
                        this.ChargesBox.Text = this.ItemInfo.Charges.ToString("f2");
                    }
                    else
                    {
                        WithdrawSchemeInfo model = this.UserInfo.WithdrawScheme;
                        decimal chargeleastofeach = model.ChargeRate * this.ItemInfo.SettleAmount;
                        if (chargeleastofeach < model.SingleMinCharge)
                        {
                            chargeleastofeach = model.SingleMinCharge;
                        }
                        else if (chargeleastofeach > model.SingleMaxCharge)
                        {
                            chargeleastofeach = model.SingleMaxCharge;
                        }
                        this.ChargesBox.Text = chargeleastofeach.ToString("f2");
                    }
                    this.ddlSupplier.SelectedValue = this.ItemInfo.Suppid.ToString();
                    this.UidLabel.Text = this.UserInfo.UserID.ToString();
                    this.UserNameLabel.Text = this.UserInfo.MerchantName;
                    this.MoneyLabel.Text = this.UserInfo.MchUsersAmtInfo.Balance.ToString("c2");
                    this.PayeeNameLabel.Text = this.ItemInfo.PayeeName;     //收款人
                    this.PayeeaddressLabel.Text = this.ItemInfo.PayeeAddress;
                    this.AccountLabel.Text = this.ItemInfo.Account;
                    this.BankLabel.Text = this.ItemInfo.PayeeBank;
                    this.UserStatusLabel.Text = Enum.GetName(typeof(UserStatusEnum), this.UserInfo.Status);
                    if (this.ItemInfo.Status !=  SettledStatusEnum.支付中)
                    {
                        this.TaxBox.Enabled = false;
                        this.TaxBox.ReadOnly = true;
                        this.ChargesBox.Enabled = false;
                        this.ChargesBox.ReadOnly = true;
                        this.btnSure.Text = "已支付";
                        this.btnSure.Enabled = false;
                    }
                    if (this.action == "pay")
                    {
                        this.TaxBox.ReadOnly = this.ChargesBox.ReadOnly = true;
                        this.btnSure.Visible = true;
                        this.btnSave.Visible = false;
                    }
                    else if (this.action == "modi")
                    {
                        this.btnSure.Visible = false;
                        this.btnSave.Visible = true;
                    }
                }
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

        public string action
        {
            get
            {
                string queryStringString = WebBase.GetQueryStringString("action", "");
                if (queryStringString == "")
                {
                    return "pay";
                }
                return queryStringString;
            }
        }

        public SettledInfo ItemInfo
        {
            get
            {
                if (this._ItemInfo == null)
                {
                    if (this.ItemInfoId > 0)
                    {
                        this._ItemInfo = SettledFactory.GetModel(this.ItemInfoId);
                    }
                    else
                    {
                        this._ItemInfo = new SettledInfo();
                    }
                }
                return this._ItemInfo;
            }
        }

        public int ItemInfoId
        {
            get
            {
                return WebBase.GetQueryStringInt32("ID", 0);
            }
        }

        public MchUserBaseInfo UserInfo
        {
            get
            {
                if ((this._userInfo == null) && (this.ItemInfo != null))
                {
                    this._userInfo = MchUserFactory.GetUserBaseByUserID(this.ItemInfo.UserID);
                }
                return this._userInfo;
            }
        }
    }
}

