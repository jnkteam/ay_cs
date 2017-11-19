namespace OriginalStudio.WebUI.Manage
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Settled;
    using OriginalStudio.BLL.Tools;
    using OriginalStudio.BLL.User;
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

    public class Pay : ManagePageBase
    {
        private SettledInfo _ItemInfo = null;
        private UserInfo _userInfo = null;
        protected Label AccountLabel;
        protected Label AddTimeLabel;
        protected Label BankLabel;
        protected Button btnSave;
        protected Button btnSure;
        protected TextBox ChargesBox;
        protected DropDownList ddlSupplier;
        protected Label errLabel;
        protected HtmlForm form1;
        protected HtmlHead Head1;
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
                decimal num = decimal.Parse(this.TaxBox.Text.Trim());
                decimal num2 = decimal.Parse(this.ChargesBox.Text.Trim());
                this.ItemInfo.Tax = num;
                this.ItemInfo.Charges = num2;
                this.ItemInfo.PayTime = DateTime.Now;
                if (!string.IsNullOrEmpty(this.ddlSupplier.SelectedValue))
                {
                    this.ItemInfo.Suppid = int.Parse(this.ddlSupplier.SelectedValue);
                }
                if (SettledFactory.Update(this.ItemInfo))
                {
                    base.AlertAndRedirect("修改成功", "Pays.aspx");
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
            base.AlertAndRedirect(msg, "Pays.aspx");
        }

        private string DoPay()
        {
            try
            {
                decimal num = decimal.Parse(this.TaxBox.Text.Trim());
                decimal num2 = decimal.Parse(this.ChargesBox.Text.Trim());
                this.ItemInfo.PayTime = DateTime.Now;
                this.ItemInfo.Tax = num;
                this.ItemInfo.Charges = num2;
                this.ItemInfo.Status =  SettledStatusEnum.已支付;
                this.ItemInfo.AppType = AppTypeEnum.t0;
                if (!string.IsNullOrEmpty(this.ddlSupplier.SelectedValue))
                {
                    //接口商
                    this.ItemInfo.Suppid = int.Parse(this.ddlSupplier.SelectedValue);
                }

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
                }
                return "err";
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
                    this.ddlSupplier.Items.Add(new ListItem(row["name"].ToString(), row["code"].ToString()));
                    //mark:畅捷code:6001，这里只返回一个
                }
                if (((this.ItemInfoId > 0) && (this.ItemInfo != null)) && (this.userInfo != null))
                {
                    this.PayMoneyLabel.Text = this.ItemInfo.Amount.ToString("c2");
                    this.AddTimeLabel.Text = FormatConvertor.DateTimeToTimeString(this.ItemInfo.AddTime);
                    this.lblPayeeName.Text = this.ItemInfo.PayeeName;
                    this.lblBank.Text = SettledFactory.GetSettleBankName(this.ItemInfo.PayeeBank);
                    this.lblPayeeaddress.Text = this.ItemInfo.PayeeAddress;
                    this.lblAccount.Text = this.ItemInfo.Account;
                    if (this.ItemInfo.Charges > 0)
                    {
                        this.ChargesBox.Text = this.ItemInfo.Charges.ToString("f2");
                    }
                    else
                    {
                        TocashSchemeInfo modelByUser = OriginalStudio.BLL.Settled.TocashScheme.GetModelByUser(1, this.ItemInfo.UserID);
                        decimal chargeleastofeach = modelByUser.chargerate * this.ItemInfo.Amount;
                        if (chargeleastofeach < modelByUser.chargeleastofeach)
                        {
                            chargeleastofeach = modelByUser.chargeleastofeach;
                        }
                        else if (chargeleastofeach > modelByUser.chargemostofeach)
                        {
                            chargeleastofeach = modelByUser.chargemostofeach;
                        }
                        this.ChargesBox.Text = chargeleastofeach.ToString("f2");
                    }
                    this.ddlSupplier.SelectedValue = this.ItemInfo.Suppid.ToString();
                    this.UidLabel.Text = this.userInfo.ID.ToString();
                    this.UserNameLabel.Text = this.userInfo.UserName;
                    this.MoneyLabel.Text = this.userInfo.Balance.ToString("c2");
                    this.PayeeNameLabel.Text = this.userInfo.PayeeName;
                    this.PayeeaddressLabel.Text = this.userInfo.BankAddress;
                    this.AccountLabel.Text = this.userInfo.Account;
                    this.BankLabel.Text = this.userInfo.PayeeBank;
                    this.UserStatusLabel.Text = Enum.GetName(typeof(UserStatusEnum), this.userInfo.Status);
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

        public UserInfo userInfo
        {
            get
            {
                if ((this._userInfo == null) && (this.ItemInfo != null))
                {
                    this._userInfo = UserFactory.GetModel(this.ItemInfo.UserID);
                }
                return this._userInfo;
            }
        }
    }
}

