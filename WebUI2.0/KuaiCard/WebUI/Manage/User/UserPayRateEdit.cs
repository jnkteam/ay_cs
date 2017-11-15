namespace KuaiCard.WebUI.Manage.User
{
    using KuaiCard.BLL;
    using KuaiCard.BLL.Payment;
    using KuaiCard.BLL.User;
    using KuaiCard.Model;
    using KuaiCard.Model.Payment;
    using KuaiCard.Model.User;
    using KuaiCard.WebComponents.Web;
    using KuaiCardLib.Text;
    using KuaiCardLib.Web;
    using System;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class UserPayRateEdit : ManagePageBase
    {
        public usersettingInfo _model = null;
        protected Button btnCopy;
        protected Button btnSave;
        protected CheckBox ckb_isopen;
        protected HtmlForm form1;
        protected Label lblUserId;
        protected TextBox txtp100;
        protected TextBox txtp101;
        protected TextBox txtp102;
        protected TextBox txtp1020;
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
        protected TextBox txtp208;
        protected TextBox txtp209;
        protected TextBox txtp210;
        protected TextBox txtp300;
        protected TextBox txtp98;
        protected TextBox txtp980;
        protected TextBox txtp99;
        protected TextBox txtp990;

        protected void btnCopy_Click(object sender, EventArgs e)
        {
            PayRate modelByUser = PayRateFactory.GetModelByUser(this.ItemInfoId);
            if (modelByUser != null)
            {
                decimal num = Convert.ToDecimal(modelByUser.p980) * 100M;
                this.txtp980.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p990) * 100M;
                this.txtp990.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p1020) * 100M;
                this.txtp1020.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p98) * 100M;
                this.txtp98.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p99) * 100M;
                this.txtp99.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p100) * 100M;
                this.txtp100.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p101) * 100M;
                this.txtp101.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p102) * 100M;
                this.txtp102.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p103) * 100M;
                this.txtp103.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p104) * 100M;
                this.txtp104.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p105) * 100M;
                this.txtp105.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p106) * 100M;
                this.txtp106.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p107) * 100M;
                this.txtp107.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p108) * 100M;
                this.txtp108.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p109) * 100M;
                this.txtp109.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p110) * 100M;
                this.txtp110.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p111) * 100M;
                this.txtp111.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p112) * 100M;
                this.txtp112.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p113) * 100M;
                this.txtp113.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p114) * 100M;
                this.txtp114.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p115) * 100M;
                this.txtp115.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p116) * 100M;
                this.txtp116.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p117) * 100M;
                this.txtp117.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p118) * 100M;
                this.txtp118.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p119) * 100M;
                this.txtp119.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p200) * 100M;
                this.txtp200.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p201) * 100M;
                this.txtp201.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p202) * 100M;
                this.txtp202.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p203) * 100M;
                this.txtp203.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p204) * 100M;
                this.txtp204.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p205) * 100M;
                this.txtp205.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p208) * 100M;
                this.txtp208.Text = num.ToString("0.00");
                num = Convert.ToDecimal(modelByUser.p209) * 100M;
                this.txtp209.Text = num.ToString("0.00");
                this.txtp210.Text = (Convert.ToDecimal(modelByUser.p210) * 100M).ToString("0.00");
                this.txtp300.Text = (Convert.ToDecimal(modelByUser.p300) * 100M).ToString("0.00");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp980.Text))
            {
                msg = msg + "p980格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp990.Text))
            {
                msg = msg + "p990格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp1020.Text))
            {
                msg = msg + "p1020格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp98.Text))
            {
                msg = msg + "p98格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp99.Text))
            {
                msg = msg + "p99格式错误！\n";
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
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp208.Text))
            {
                msg = msg + "p208格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp209.Text))
            {
                msg = msg + "p209格式错误！\n";
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
                decimal num = decimal.Parse(this.txtp980.Text) / 100M;
                decimal num2 = decimal.Parse(this.txtp990.Text) / 100M;
                decimal num3 = decimal.Parse(this.txtp1020.Text) / 100M;
                decimal num4 = decimal.Parse(this.txtp98.Text) / 100M;
                decimal num5 = decimal.Parse(this.txtp99.Text) / 100M;
                decimal num6 = decimal.Parse(this.txtp100.Text) / 100M;
                decimal num7 = decimal.Parse(this.txtp101.Text) / 100M;
                decimal num8 = decimal.Parse(this.txtp102.Text) / 100M;
                decimal num9 = decimal.Parse(this.txtp103.Text) / 100M;
                decimal num10 = decimal.Parse(this.txtp104.Text) / 100M;
                decimal num11 = decimal.Parse(this.txtp105.Text) / 100M;
                decimal num12 = decimal.Parse(this.txtp106.Text) / 100M;
                decimal num13 = decimal.Parse(this.txtp107.Text) / 100M;
                decimal num14 = decimal.Parse(this.txtp108.Text) / 100M;
                decimal num15 = decimal.Parse(this.txtp109.Text) / 100M;
                decimal num16 = decimal.Parse(this.txtp110.Text) / 100M;
                decimal num17 = decimal.Parse(this.txtp111.Text) / 100M;
                decimal num18 = decimal.Parse(this.txtp112.Text) / 100M;
                decimal num19 = decimal.Parse(this.txtp113.Text) / 100M;
                decimal num20 = decimal.Parse(this.txtp114.Text) / 100M;
                decimal num21 = decimal.Parse(this.txtp115.Text) / 100M;
                decimal num22 = decimal.Parse(this.txtp116.Text) / 100M;
                decimal num23 = decimal.Parse(this.txtp117.Text) / 100M;
                decimal num24 = decimal.Parse(this.txtp118.Text) / 100M;
                decimal num25 = decimal.Parse(this.txtp119.Text) / 100M;
                decimal num26 = decimal.Parse(this.txtp200.Text) / 100M;
                decimal num27 = decimal.Parse(this.txtp201.Text) / 100M;
                decimal num28 = decimal.Parse(this.txtp202.Text) / 100M;
                decimal num29 = decimal.Parse(this.txtp203.Text) / 100M;
                decimal num30 = decimal.Parse(this.txtp204.Text) / 100M;
                decimal num31 = decimal.Parse(this.txtp205.Text) / 100M;
                decimal num32 = decimal.Parse(this.txtp208.Text) / 100M;
                decimal num33 = decimal.Parse(this.txtp209.Text) / 100M;
                decimal num34 = decimal.Parse(this.txtp210.Text) / 100M;
                decimal num35 = decimal.Parse(this.txtp300.Text) / 100M;
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat("{0}:{1}", 980, num);
                builder.AppendFormat("|{0}:{1}", 990, num2);
                builder.AppendFormat("|{0}:{1}", 0x3fc, num3);
                builder.AppendFormat("|{0}:{1}", 0x62, num4);
                builder.AppendFormat("|{0}:{1}", 0x63, num5);
                builder.AppendFormat("|{0}:{1}", 100, num6);
                builder.AppendFormat("|{0}:{1}", 0x65, num7);
                builder.AppendFormat("|{0}:{1}", 0x66, num8);
                builder.AppendFormat("|{0}:{1}", 0x67, num9);
                builder.AppendFormat("|{0}:{1}", 0x68, num10);
                builder.AppendFormat("|{0}:{1}", 0x69, num11);
                builder.AppendFormat("|{0}:{1}", 0x6a, num12);
                builder.AppendFormat("|{0}:{1}", 0x6b, num13);
                builder.AppendFormat("|{0}:{1}", 0x6c, num14);
                builder.AppendFormat("|{0}:{1}", 0x6d, num15);
                builder.AppendFormat("|{0}:{1}", 110, num16);
                builder.AppendFormat("|{0}:{1}", 0x6f, num17);
                builder.AppendFormat("|{0}:{1}", 0x70, num18);
                builder.AppendFormat("|{0}:{1}", 0x71, num19);
                builder.AppendFormat("|{0}:{1}", 0x72, num20);
                builder.AppendFormat("|{0}:{1}", 0x73, num21);
                builder.AppendFormat("|{0}:{1}", 0x74, num22);
                builder.AppendFormat("|{0}:{1}", 0x75, num23);
                builder.AppendFormat("|{0}:{1}", 0x76, num24);
                builder.AppendFormat("|{0}:{1}", 0x77, num25);
                builder.AppendFormat("|{0}:{1}", 200, num26);
                builder.AppendFormat("|{0}:{1}", 0xc9, num27);
                builder.AppendFormat("|{0}:{1}", 0xca, num28);
                builder.AppendFormat("|{0}:{1}", 0xcb, num29);
                builder.AppendFormat("|{0}:{1}", 0xcc, num30);
                builder.AppendFormat("|{0}:{1}", 0xcd, num31);
                builder.AppendFormat("|{0}:{1}", 0xd0, num32);
                builder.AppendFormat("|{0}:{1}", 0xd1, num33);
                builder.AppendFormat("|{0}:{1}", 210, num34);
                builder.AppendFormat("|{0}:{1}", 300, num35);
                usersetting usersetting = new usersetting();
                this.model.payrate = builder.ToString();
                this.model.special = this.ckb_isopen.Checked ? 1 : 0;
                if (usersetting.Insert(this.model))
                {
                    base.AlertAndRedirect("设置成功");
                }
                else
                {
                    base.AlertAndRedirect("设置失败");
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
            this.lblUserId.Text = this.ItemInfoId.ToString();
            if (this.model != null)
            {
                this.ckb_isopen.Checked = this.model.special > 0;
                string payrate = this.model.payrate;
                if (!string.IsNullOrEmpty(payrate))
                {
                    string[] strArray = payrate.Split(new char[] { '|' });
                    foreach (string str2 in strArray)
                    {
                        decimal num;
                        string[] strArray2 = str2.Split(new char[] { ':' });
                        if (strArray2[0] == "980")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp980.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "990")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp990.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "1020")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp1020.Text = num.ToString("0.00");
                        }
                        if (strArray2[0] == "98")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp98.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "99")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp99.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "100")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp100.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "101")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp101.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "102")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp102.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "103")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp103.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "104")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp104.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "105")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp105.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "106")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp106.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "107")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp107.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "108")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp108.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "109")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp109.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "110")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp110.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "111")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp111.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "112")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp112.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "113")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp113.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "114")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp114.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "115")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp115.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "116")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp116.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "117")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp117.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "118")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp118.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "119")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp119.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "200")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp200.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "201")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp201.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "202")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp202.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "203")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp203.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "204")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp204.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "205")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp205.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "208")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp208.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "209")
                        {
                            num = Convert.ToDecimal(strArray2[1]) * 100M;
                            this.txtp209.Text = num.ToString("0.00");
                        }
                        else if (strArray2[0] == "210")
                        {
                            this.txtp210.Text = (Convert.ToDecimal(strArray2[1]) * 100M).ToString("0.00");
                        }
                        else if (strArray2[0] == "300")
                        {
                            this.txtp300.Text = (Convert.ToDecimal(strArray2[1]) * 100M).ToString("0.00");
                        }
                    }
                }
            }
        }

        public int ItemInfoId
        {
            get
            {
                return WebBase.GetQueryStringInt32("id", 0);
            }
        }

        public usersettingInfo model
        {
            get
            {
                if (this._model == null)
                {
                    this._model = new usersetting().GetModel(this.ItemInfoId);
                }
                return this._model;
            }
        }
    }
}

