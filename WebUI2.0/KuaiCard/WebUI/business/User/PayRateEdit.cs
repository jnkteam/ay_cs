﻿namespace KuaiCard.WebUI.business.User
{
    using KuaiCard.BLL;
    using KuaiCard.BLL.Payment;
    using KuaiCard.Model;
    using KuaiCard.Model.Payment;
    using KuaiCard.WebComponents.Web;
    using KuaiCardLib.Text;
    using KuaiCardLib.Web;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class PayRateEdit : BusinessPageBase
    {
        public PayRate _model = null;
        protected Button btnSave;
        protected HtmlForm form1;
        protected TextBox txtlevName;
        protected TextBox txtp100;
        protected TextBox txtp101;
        protected TextBox txtp102;
        protected TextBox txtp103;
        protected TextBox txtp104;
        protected TextBox txtp105;
        protected TextBox txtp106;
        protected TextBox txtp107;
        protected TextBox txtp108;
        protected TextBox txtp109;
        protected TextBox txtp110;
        protected TextBox txtp111;
        protected TextBox txtp112;
        protected TextBox txtp113;
        protected TextBox txtp114;
        protected TextBox txtp115;
        protected TextBox txtp116;
        protected TextBox txtp117;
        protected TextBox txtp118;
        protected TextBox txtp119;
        protected TextBox txtp200;
        protected TextBox txtp201;
        protected TextBox txtp202;
        protected TextBox txtp203;
        protected TextBox txtp204;
        protected TextBox txtp205;
        protected TextBox txtp300;

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string msg = "";
            if (this.txtlevName.Text.Trim().Length == 0)
            {
                msg = msg + "levName不能为空！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp100.Text))
            {
                msg = msg + "p100格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp101.Text))
            {
                msg = msg + "p101格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp102.Text))
            {
                msg = msg + "p102格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp103.Text))
            {
                msg = msg + "p103格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp104.Text))
            {
                msg = msg + "p104格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp105.Text))
            {
                msg = msg + "p105格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp106.Text))
            {
                msg = msg + "p106格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp107.Text))
            {
                msg = msg + "p107格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp108.Text))
            {
                msg = msg + "p108格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp109.Text))
            {
                msg = msg + "p109格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp110.Text))
            {
                msg = msg + "p110格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp111.Text))
            {
                msg = msg + "p111格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp112.Text))
            {
                msg = msg + "p112格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp113.Text))
            {
                msg = msg + "p113格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp114.Text))
            {
                msg = msg + "p114格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp115.Text))
            {
                msg = msg + "p115格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp116.Text))
            {
                msg = msg + "p116格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp117.Text))
            {
                msg = msg + "p117格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp118.Text))
            {
                msg = msg + "p118格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp119.Text))
            {
                msg = msg + "p119格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp200.Text))
            {
                msg = msg + "p200格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp201.Text))
            {
                msg = msg + "p201格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp202.Text))
            {
                msg = msg + "p202格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp203.Text))
            {
                msg = msg + "p203格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp204.Text))
            {
                msg = msg + "p204格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp205.Text))
            {
                msg = msg + "p205格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp300.Text))
            {
                msg = msg + "p300格式错误！\n";
            }
            if (msg != "")
            {
                base.AlertAndRedirect(msg);
            }
            else
            {
                string text = this.txtlevName.Text;
                decimal num = decimal.Parse(this.txtp100.Text) / 100M;
                decimal num2 = decimal.Parse(this.txtp101.Text) / 100M;
                decimal num3 = decimal.Parse(this.txtp102.Text) / 100M;
                decimal num4 = decimal.Parse(this.txtp103.Text) / 100M;
                decimal num5 = decimal.Parse(this.txtp104.Text) / 100M;
                decimal num6 = decimal.Parse(this.txtp105.Text) / 100M;
                decimal num7 = decimal.Parse(this.txtp106.Text) / 100M;
                decimal num8 = decimal.Parse(this.txtp107.Text) / 100M;
                decimal num9 = decimal.Parse(this.txtp108.Text) / 100M;
                decimal num10 = decimal.Parse(this.txtp109.Text) / 100M;
                decimal num11 = decimal.Parse(this.txtp110.Text) / 100M;
                decimal num12 = decimal.Parse(this.txtp111.Text) / 100M;
                decimal num13 = decimal.Parse(this.txtp112.Text) / 100M;
                decimal num14 = decimal.Parse(this.txtp113.Text) / 100M;
                decimal num15 = decimal.Parse(this.txtp114.Text) / 100M;
                decimal num16 = decimal.Parse(this.txtp115.Text) / 100M;
                decimal num17 = decimal.Parse(this.txtp116.Text) / 100M;
                decimal num18 = decimal.Parse(this.txtp117.Text) / 100M;
                decimal num19 = decimal.Parse(this.txtp118.Text) / 100M;
                decimal num20 = decimal.Parse(this.txtp119.Text) / 100M;
                decimal num21 = decimal.Parse(this.txtp200.Text) / 100M;
                decimal num22 = decimal.Parse(this.txtp201.Text) / 100M;
                decimal num23 = decimal.Parse(this.txtp202.Text) / 100M;
                decimal num24 = decimal.Parse(this.txtp203.Text) / 100M;
                decimal num25 = decimal.Parse(this.txtp204.Text) / 100M;
                decimal num26 = decimal.Parse(this.txtp205.Text) / 100M;
                decimal num27 = decimal.Parse(this.txtp300.Text) / 100M;
                this.model.levName = text;
                this.model.p100 = num;
                this.model.p101 = num2;
                this.model.p102 = num3;
                this.model.p103 = num4;
                this.model.p104 = num5;
                this.model.p105 = num6;
                this.model.p106 = num7;
                this.model.p107 = num8;
                this.model.p108 = num9;
                this.model.p109 = num10;
                this.model.p110 = num11;
                this.model.p111 = num12;
                this.model.p112 = num13;
                this.model.p113 = num14;
                this.model.p114 = num15;
                this.model.p115 = num16;
                this.model.p116 = num17;
                this.model.p117 = num18;
                this.model.p118 = num19;
                this.model.p119 = num20;
                this.model.p200 = num21;
                this.model.p201 = num22;
                this.model.p202 = num23;
                this.model.p203 = num24;
                this.model.p204 = num25;
                this.model.p205 = num26;
                this.model.p300 = num27;
                if (this.isUpdate)
                {
                    if (PayRateFactory.Update(this.model))
                    {
                        base.AlertAndRedirect("修改成功", "PayRate.aspx");
                    }
                    else
                    {
                        base.AlertAndRedirect("修改失败");
                    }
                }
                else if (PayRateFactory.Add(this.model) > 0)
                {
                    base.AlertAndRedirect("新增成功", "PayRate.aspx");
                }
                else
                {
                    base.AlertAndRedirect("新增失败");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ManageFactory.CheckSecondPwd();
            this.setPower();
            if (!base.IsPostBack)
            {
                this.ShowInfo();
            }
        }

        private void setPower()
        {
            if (!ManageFactory.CheckCurrentPermission(false, ManageRole.Interfaces))
            {
                base.Response.Write("Sorry,No authority!");
                base.Response.End();
            }
        }

        private void ShowInfo()
        {
            if (this.isUpdate && (this.model != null))
            {
                this.txtlevName.Text = this.model.levName;
                decimal num = Convert.ToDecimal(this.model.p100) * 100M;
                this.txtp100.Text = num.ToString("0.00");
                num = Convert.ToDecimal(this.model.p101) * 100M;
                this.txtp101.Text = num.ToString("0.00");
                num = Convert.ToDecimal(this.model.p102) * 100M;
                this.txtp102.Text = num.ToString("0.00");
                num = Convert.ToDecimal(this.model.p103) * 100M;
                this.txtp103.Text = num.ToString("0.00");
                num = Convert.ToDecimal(this.model.p104) * 100M;
                this.txtp104.Text = num.ToString("0.00");
                num = Convert.ToDecimal(this.model.p105) * 100M;
                this.txtp105.Text = num.ToString("0.00");
                num = Convert.ToDecimal(this.model.p106) * 100M;
                this.txtp106.Text = num.ToString("0.00");
                num = Convert.ToDecimal(this.model.p107) * 100M;
                this.txtp107.Text = num.ToString("0.00");
                num = Convert.ToDecimal(this.model.p108) * 100M;
                this.txtp108.Text = num.ToString("0.00");
                num = Convert.ToDecimal(this.model.p109) * 100M;
                this.txtp109.Text = num.ToString("0.00");
                num = Convert.ToDecimal(this.model.p110) * 100M;
                this.txtp110.Text = num.ToString("0.00");
                num = Convert.ToDecimal(this.model.p111) * 100M;
                this.txtp111.Text = num.ToString("0.00");
                num = Convert.ToDecimal(this.model.p112) * 100M;
                this.txtp112.Text = num.ToString("0.00");
                num = Convert.ToDecimal(this.model.p113) * 100M;
                this.txtp113.Text = num.ToString("0.00");
                num = Convert.ToDecimal(this.model.p114) * 100M;
                this.txtp114.Text = num.ToString("0.00");
                num = Convert.ToDecimal(this.model.p115) * 100M;
                this.txtp115.Text = num.ToString("0.00");
                num = Convert.ToDecimal(this.model.p116) * 100M;
                this.txtp116.Text = num.ToString("0.00");
                num = Convert.ToDecimal(this.model.p117) * 100M;
                this.txtp117.Text = num.ToString("0.00");
                num = Convert.ToDecimal(this.model.p118) * 100M;
                this.txtp118.Text = num.ToString("0.00");
                num = Convert.ToDecimal(this.model.p119) * 100M;
                this.txtp119.Text = num.ToString("0.00");
                num = Convert.ToDecimal(this.model.p200) * 100M;
                this.txtp200.Text = num.ToString("0.00");
                num = Convert.ToDecimal(this.model.p201) * 100M;
                this.txtp201.Text = num.ToString("0.00");
                num = Convert.ToDecimal(this.model.p202) * 100M;
                this.txtp202.Text = num.ToString("0.00");
                num = Convert.ToDecimal(this.model.p203) * 100M;
                this.txtp203.Text = num.ToString("0.00");
                num = Convert.ToDecimal(this.model.p204) * 100M;
                this.txtp204.Text = num.ToString("0.00");
                this.txtp205.Text = (Convert.ToDecimal(this.model.p205) * 100M).ToString("0.00");
                this.txtp300.Text = (Convert.ToDecimal(this.model.p300) * 100M).ToString("0.00");
            }
        }

        public bool isUpdate
        {
            get
            {
                return (this.ItemInfoId > 0);
            }
        }

        public int ItemInfoId
        {
            get
            {
                return WebBase.GetQueryStringInt32("id", 0);
            }
        }

        public PayRate model
        {
            get
            {
                if (this._model == null)
                {
                    if (this.isUpdate)
                    {
                        this._model = PayRateFactory.GetModel(this.ItemInfoId);
                    }
                    else
                    {
                        this._model = new PayRate();
                        this._model.rateType = RateTypeEnum.Member;
                    }
                }
                return this._model;
            }
        }
    }
}

