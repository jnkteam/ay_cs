namespace KuaiCard.WebUI.Manage
{
    using KuaiCard.BLL;
    using KuaiCard.BLL.Settled;
    using KuaiCard.BLL.Tools;
    using KuaiCard.BLL.User;
    using KuaiCard.ETAPI;
    using KuaiCard.Model;
    using KuaiCard.Model.Settled;
    using KuaiCard.Model.User;
    using KuaiCard.WebComponents.Web;
    using KuaiCardLib.ExceptionHandling;
    using KuaiCardLib.TimeControl;
    using KuaiCardLib.Web;
    using System;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

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
                this.ItemInfo.tax = new decimal?(num);
                this.ItemInfo.charges = new decimal?(num2);
                this.ItemInfo.paytime = DateTime.Now;
                if (!string.IsNullOrEmpty(this.ddlSupplier.SelectedValue))
                {
                    this.ItemInfo.suppid = int.Parse(this.ddlSupplier.SelectedValue);
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
                this.ItemInfo.paytime = DateTime.Now;
                this.ItemInfo.tax = new decimal?(num);
                this.ItemInfo.charges = new decimal?(num2);
                this.ItemInfo.status = SettledStatus.已支付;
                this.ItemInfo.AppType = AppTypeEnum.t0;
                if (!string.IsNullOrEmpty(this.ddlSupplier.SelectedValue))
                {
                    //接口商
                    this.ItemInfo.suppid = int.Parse(this.ddlSupplier.SelectedValue);
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
                        //KuaiCardLib.Logging.LogHelper.Write("提现:DoPay");
                        if (this.ItemInfo.suppid > 0)
                        {
                            //Step1：走 网关代付。
                            KuaiCard.ETAPI.Withdraw.InitDistribution(this.ItemInfo);
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
                DataTable table = SupplierFactory.GetList("isdistribution=1").Tables[0];
                this.ddlSupplier.Items.Add(new ListItem("不走接口", "0"));
                foreach (DataRow row in table.Rows)
                {
                    this.ddlSupplier.Items.Add(new ListItem(row["name"].ToString(), row["code"].ToString()));
                    //mark:畅捷code:6001，这里只返回一个
                }
                if (((this.ItemInfoId > 0) && (this.ItemInfo != null)) && (this.userInfo != null))
                {
                    this.PayMoneyLabel.Text = this.ItemInfo.amount.ToString("c2");
                    this.AddTimeLabel.Text = FormatConvertor.DateTimeToTimeString(this.ItemInfo.addtime);
                    this.lblPayeeName.Text = this.ItemInfo.payeeName;
                    this.lblBank.Text = SettledFactory.GetSettleBankName(this.ItemInfo.PayeeBank);
                    this.lblPayeeaddress.Text = this.ItemInfo.Payeeaddress;
                    this.lblAccount.Text = this.ItemInfo.Account;
                    if (this.ItemInfo.charges.HasValue)
                    {
                        this.ChargesBox.Text = this.ItemInfo.charges.Value.ToString("f2");
                    }
                    else
                    {
                        TocashSchemeInfo modelByUser = KuaiCard.BLL.Settled.TocashScheme.GetModelByUser(1, this.ItemInfo.userid);
                        decimal chargeleastofeach = modelByUser.chargerate * this.ItemInfo.amount;
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
                    this.ddlSupplier.SelectedValue = this.ItemInfo.suppid.ToString();
                    this.UidLabel.Text = this.userInfo.ID.ToString();
                    this.UserNameLabel.Text = this.userInfo.UserName;
                    this.MoneyLabel.Text = this.userInfo.Balance.ToString("c2");
                    this.PayeeNameLabel.Text = this.userInfo.PayeeName;
                    this.PayeeaddressLabel.Text = this.userInfo.BankAddress;
                    this.AccountLabel.Text = this.userInfo.Account;
                    this.BankLabel.Text = this.userInfo.PayeeBank;
                    this.UserStatusLabel.Text = Enum.GetName(typeof(UserStatusEnum), this.userInfo.Status);
                    if (this.ItemInfo.status != SettledStatus.支付中)
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
                    this._userInfo = UserFactory.GetModel(this.ItemInfo.userid);
                }
                return this._userInfo;
            }
        }
    }
}

