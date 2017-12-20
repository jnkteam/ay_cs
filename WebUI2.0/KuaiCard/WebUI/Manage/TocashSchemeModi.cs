namespace OriginalStudio.WebUI.Manage
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model;
    using OriginalStudio.Model.Settled;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Text;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using OriginalStudio.BLL.Withdraw;
    using OriginalStudio.Model.Withdraw;
    using OriginalStudio.BLL.Supplier;
    using System.Data;

    public class TocashSchemeModi : ManagePageBase
    {
        public WithdrawSchemeInfo _ItemInfo = null;
        protected Button btnAdd;
        protected HtmlForm form1;
        protected RadioButtonList rblisdefault;
        protected RadioButtonList rbltranRequiredAudit;
        protected RadioButtonList rblVaiInterface;
        protected TextBox txtalipaydetentiondays;
        protected TextBox txtbankdetentiondays;
        protected TextBox txtcarddetentiondays;
        protected TextBox txtchargeleastofeach;
        protected TextBox txtchargemostofeach;
        protected TextBox txtchargerate;
        protected TextBox txtdailymaxamt;
        protected TextBox txtdailymaxtimes;
        protected TextBox txtmaxamtlimitofeach;
        protected TextBox txtminamtlimitofeach;
        protected TextBox txtotherdetentiondays;
        protected TextBox txtschemename;
        protected TextBox txtweixindetentiondays;

        protected DropDownList SupplierDrop; //结算通道



        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string msg = "";
            if (this.txtschemename.Text.Trim().Length == 0)
            {
                msg = msg + "方案名称不能为空！\n";
            }
            if (!this.isNum(this.txtminamtlimitofeach.Text))
            {
                msg = msg + "最低提现金额限制(每笔)格式错误！\n";
            }
            if (!this.isNum(this.txtmaxamtlimitofeach.Text))
            {
                msg = msg + "最大提现金额限制(每笔)格式错误！\n";
            }
            if (!PageValidate.IsNumber(this.txtdailymaxtimes.Text))
            {
                msg = msg + "每天最多可提现次数格式错误！\n";
            }
            if (!this.isNum(this.txtdailymaxamt.Text))
            {
                msg = msg + "每天最多可限额格式错误！\n";
            }
            if (!this.isNum(this.txtchargerate.Text))
            {
                msg = msg + "提现手续费格式错误！\n";
            }
            if (!this.isNum(this.txtchargeleastofeach.Text))
            {
                msg = msg + "提现手续费最少每笔格式错误！\n";
            }
            if (!this.isNum(this.txtchargemostofeach.Text))
            {
                msg = msg + "提现手续费最高每笔格式错误！\n";
            }
            if (msg != "")
            {
                base.AlertAndRedirect(msg);
            }
            else
            {
                string text = this.txtschemename.Text;
                decimal num = decimal.Parse(this.txtminamtlimitofeach.Text);
                decimal num2 = decimal.Parse(this.txtmaxamtlimitofeach.Text);
                int num3 = int.Parse(this.txtdailymaxtimes.Text);
                decimal num4 = decimal.Parse(this.txtdailymaxamt.Text);
                decimal num5 = decimal.Parse(this.txtchargerate.Text);
                decimal num6 = decimal.Parse(this.txtchargeleastofeach.Text);
                decimal num7 = decimal.Parse(this.txtchargemostofeach.Text);
                int num8 = int.Parse(this.rblisdefault.SelectedValue);
                int num9 = int.Parse(this.rblVaiInterface.SelectedValue);
                int result = 0;
                //int num11 = 0;
                int num12 = 0;
                int num13 = 0;
                int num14 = 0;
                int.TryParse(this.txtbankdetentiondays.Text.Trim(), out result);
                //int.TryParse(this.txtcarddetentiondays.Text.Trim(), out num11);
                int.TryParse(this.txtotherdetentiondays.Text.Trim(), out num12);
                int.TryParse(this.txtalipaydetentiondays.Text.Trim(), out num13);
                int.TryParse(this.txtweixindetentiondays.Text.Trim(), out num14);
                this.model.SchemeName = text;
                this.model.SingleMinAmtLimit = num;
                this.model.SingleMaxAmtLimit = num2;
                this.model.DailyMaxTimes = num3;
                this.model.DailyMaxAmt = num4;
                this.model.ChargeRate = num5;
                this.model.SingleMinCharge = num6;
                this.model.SingleMaxCharge = num7;
                this.model.IsDefault = num8;
                this.model.IsTranApi = num9;
                this.model.BankDetentionDays = result;
                //this.model.carddetentiondays = num11;
                this.model.AlipayDetentionDays = num13;
                this.model.WeiXinDetentionDays = num14;
                this.model.OtherDetentionDays = num12;
                this.model.IsTranRequiredAudit = Convert.ToByte(this.rbltranRequiredAudit.SelectedValue);//需要审核
                this.model.TranSupplier = Convert.ToInt32(this.SupplierDrop.SelectedValue);//通道

                bool flag = false;
                if (this.isUpdate)
                {
                    if (WithdrawSchemeFactory.Update(this.model))
                    {
                        flag = true;
                    }
                }
                else
                {
                    this.model.Type = 1;
                    if (WithdrawSchemeFactory.Add(this.model) > 0)
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    showPageMsg("操作成功");
                }
                else
                {
                    showPageMsg("操作失败");
                }
            }
        }

        private void InitForm()
        {

            //通道类型
            DataTable SupplierTable = SysSupplierFactory.GetList("IsDistribution  = 1").Tables[0];
            
            foreach (DataRow row in SupplierTable.Rows)
            {
                this.SupplierDrop.Items.Add(new ListItem(row["SupplierName"].ToString(), row["SupplierCode"].ToString()));
            }
            if (this.isUpdate)
            {
                this.txtschemename.Text = this.model.SchemeName;
                this.txtminamtlimitofeach.Text = this.model.SingleMinAmtLimit.ToString();
                this.txtmaxamtlimitofeach.Text = this.model.SingleMaxAmtLimit.ToString();
                this.txtdailymaxtimes.Text = this.model.DailyMaxTimes.ToString();
                this.txtdailymaxamt.Text = this.model.DailyMaxAmt.ToString();
                this.txtchargerate.Text = this.model.ChargeRate.ToString();
                this.txtchargeleastofeach.Text = this.model.SingleMinCharge.ToString();
                this.txtchargemostofeach.Text = this.model.SingleMaxCharge.ToString();
                this.rblisdefault.SelectedValue = this.model.IsDefault.ToString();
                this.rblVaiInterface.SelectedValue = this.model.IsTranApi.ToString(); //是否走接口
                this.txtbankdetentiondays.Text = this.model.BankDetentionDays.ToString();
                //this.txtcarddetentiondays.Text = this.model.carddetentiondays.ToString();
                this.txtotherdetentiondays.Text = this.model.OtherDetentionDays.ToString();
                this.rbltranRequiredAudit.SelectedValue = this.model.IsTranRequiredAudit.ToString();//是否需要审核
                this.txtalipaydetentiondays.Text = this.model.AlipayDetentionDays.ToString();
                this.txtweixindetentiondays.Text = this.model.WeiXinDetentionDays.ToString();


                this.SupplierDrop.SelectedValue = this.model.TranSupplier.ToString();//通道
            }
        }

        private bool isNum(string _input)
        {
            return (PageValidate.IsNumber(_input) || PageValidate.IsDecimal(_input));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            if (!base.IsPostBack)
            {
                this.InitForm();
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

        public string Action
        {
            get
            {
                return WebBase.GetQueryStringString("cmd", "");
            }
        }

        public bool isUpdate
        {
            get
            {
                return ((this.ItemInfoId > 0) && (this.Action == "edit"));
            }
        }

        public int ItemInfoId
        {
            get
            {
                return WebBase.GetQueryStringInt32("id", 0);
            }
        }

        public WithdrawSchemeInfo model
        {
            get
            {
                if (this._ItemInfo == null)
                {
                    if (this.isUpdate)
                    {
                        this._ItemInfo = WithdrawSchemeFactory.GetModel(this.ItemInfoId);
                    }
                    else
                    {
                        this._ItemInfo = new WithdrawSchemeInfo();
                    }
                }
                return this._ItemInfo;
            }
        }
    }
}

