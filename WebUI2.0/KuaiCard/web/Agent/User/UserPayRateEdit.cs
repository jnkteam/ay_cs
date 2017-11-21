namespace OriginalStudio.web.Agent.User
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Payment;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model;
    using OriginalStudio.Model.PayRate;
    using OriginalStudio.Model.User;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Text;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class UserPayRateEdit : AgentPageBase
    {
        public usersettingInfo _model = null;
        protected Button btnCopy;
        protected Button btnSave;
        protected CheckBox ckb_isopen;
        protected HtmlForm form1;
        protected Label lblUserId;
        protected TextBox txtp100;
        protected TextBox txtp1000;
        protected TextBox txtp1001;
        protected TextBox txtp101;
        protected TextBox txtp1010;
        protected TextBox txtp1011;
        protected TextBox txtp102;
        protected TextBox txtp1020;
        protected TextBox txtp1021;
        protected TextBox txtp103;
        protected TextBox txtp1030;
        protected TextBox txtp1031;
        protected TextBox txtp104;
        protected TextBox txtp1040;
        protected TextBox txtp1041;
        protected TextBox txtp105;
        protected TextBox txtp1050;
        protected TextBox txtp1051;
        protected TextBox txtp106;
        protected TextBox txtp1060;
        protected TextBox txtp1061;
        protected TextBox txtp107;
        protected TextBox txtp1070;
        protected TextBox txtp1071;
        protected TextBox txtp108;
        protected TextBox txtp1080;
        protected TextBox txtp1081;
        protected TextBox txtp109;
        protected TextBox txtp1090;
        protected TextBox txtp1091;
        protected TextBox txtp110;
        protected TextBox txtp1100;
        protected TextBox txtp1101;
        protected TextBox txtp111;
        protected TextBox txtp1110;
        protected TextBox txtp1111;
        protected TextBox txtp112;
        protected TextBox txtp1120;
        protected TextBox txtp1121;
        protected TextBox txtp113;
        protected TextBox txtp1130;
        protected TextBox txtp1131;
        protected TextBox txtp114;
        protected TextBox txtp1140;
        protected TextBox txtp1141;
        protected TextBox txtp115;
        protected TextBox txtp1150;
        protected TextBox txtp1151;
        protected TextBox txtp116;
        protected TextBox txtp1160;
        protected TextBox txtp1161;
        protected TextBox txtp117;
        protected TextBox txtp1170;
        protected TextBox txtp1171;
        protected TextBox txtp118;
        protected TextBox txtp1180;
        protected TextBox txtp1181;
        protected TextBox txtp119;
        protected TextBox txtp1190;
        protected TextBox txtp1191;
        protected TextBox txtp200;
        protected TextBox txtp2000;
        protected TextBox txtp2001;
        protected TextBox txtp201;
        protected TextBox txtp2010;
        protected TextBox txtp2011;
        protected TextBox txtp202;
        protected TextBox txtp2020;
        protected TextBox txtp2021;
        protected TextBox txtp203;
        protected TextBox txtp2030;
        protected TextBox txtp2031;
        protected TextBox txtp204;
        protected TextBox txtp2040;
        protected TextBox txtp2041;
        protected TextBox txtp205;
        protected TextBox txtp2050;
        protected TextBox txtp2051;
        protected TextBox txtp208;
        protected TextBox txtp2080;
        protected TextBox txtp2081;
        protected TextBox txtp209;
        protected TextBox txtp2090;
        protected TextBox txtp2091;
        protected TextBox txtp300;
        protected TextBox txtp3000;
        protected TextBox txtp3001;

        protected void btnCopy_Click(object sender, EventArgs e)
        {
            PayRate modelByUser = PayRateFactory.GetModelByUser(base.UserId);
            if (modelByUser != null)
            {
                decimal num = Convert.ToDecimal(modelByUser.p100) * 100M;
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
                this.txtp209.Text = (Convert.ToDecimal(modelByUser.p209) * 100M).ToString("0.00");
                this.txtp300.Text = (Convert.ToDecimal(modelByUser.p300) * 100M).ToString("0.00");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp100.Text))
            //{
            //    msg = msg + "p100格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp101.Text))
            //{
            //    msg = msg + "p101格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp102.Text))
            //{
            //    msg = msg + "p102格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp103.Text))
            //{
            //    msg = msg + "p103格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp104.Text))
            //{
            //    msg = msg + "p104格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp105.Text))
            //{
            //    msg = msg + "p105格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp106.Text))
            //{
            //    msg = msg + "p106格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp107.Text))
            //{
            //    msg = msg + "p107格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp108.Text))
            //{
            //    msg = msg + "p108格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp109.Text))
            //{
            //    msg = msg + "p109格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp110.Text))
            //{
            //    msg = msg + "p110格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp111.Text))
            //{
            //    msg = msg + "p111格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp112.Text))
            //{
            //    msg = msg + "p112格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp113.Text))
            //{
            //    msg = msg + "p113格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp114.Text))
            //{
            //    msg = msg + "p114格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp115.Text))
            //{
            //    msg = msg + "p115格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp116.Text))
            //{
            //    msg = msg + "p116格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp117.Text))
            //{
            //    msg = msg + "p117格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp118.Text))
            //{
            //    msg = msg + "p118格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp119.Text))
            //{
            //    msg = msg + "p119格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp200.Text))
            //{
            //    msg = msg + "p200格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp201.Text))
            //{
            //    msg = msg + "p201格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp202.Text))
            //{
            //    msg = msg + "p202格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp203.Text))
            //{
            //    msg = msg + "p203格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp204.Text))
            //{
            //    msg = msg + "p204格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp205.Text))
            //{
            //    msg = msg + "p205格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp208.Text))
            //{
            //    msg = msg + "p208格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp209.Text))
            //{
            //    msg = msg + "p209格式错误！\n";
            //}
            //if (!OriginalStudio.Lib.Text.Validate.IsNumber(this.txtp300.Text))
            //{
            //    msg = msg + "p300格式错误！\n";
            //}
            if (msg != "")
            {
                base.AlertAndRedirect(msg);
            }
            else if (!this.CheckOK())
            {
                base.AlertAndRedirect("费率不在指定的范围 内.请重新设置");
            }
            else
            {
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
                decimal num27 = decimal.Parse(this.txtp208.Text) / 100M;
                decimal num28 = decimal.Parse(this.txtp209.Text) / 100M;
                decimal num29 = decimal.Parse(this.txtp300.Text) / 100M;
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat("{0}:{1}", 100, num);
                builder.AppendFormat("|{0}:{1}", 0x65, num2);
                builder.AppendFormat("|{0}:{1}", 0x66, num3);
                builder.AppendFormat("|{0}:{1}", 0x67, num4);
                builder.AppendFormat("|{0}:{1}", 0x68, num5);
                builder.AppendFormat("|{0}:{1}", 0x69, num6);
                builder.AppendFormat("|{0}:{1}", 0x6a, num7);
                builder.AppendFormat("|{0}:{1}", 0x6b, num8);
                builder.AppendFormat("|{0}:{1}", 0x6c, num9);
                builder.AppendFormat("|{0}:{1}", 0x6d, num10);
                builder.AppendFormat("|{0}:{1}", 110, num11);
                builder.AppendFormat("|{0}:{1}", 0x6f, num12);
                builder.AppendFormat("|{0}:{1}", 0x70, num13);
                builder.AppendFormat("|{0}:{1}", 0x71, num14);
                builder.AppendFormat("|{0}:{1}", 0x72, num15);
                builder.AppendFormat("|{0}:{1}", 0x73, num16);
                builder.AppendFormat("|{0}:{1}", 0x74, num17);
                builder.AppendFormat("|{0}:{1}", 0x75, num18);
                builder.AppendFormat("|{0}:{1}", 0x76, num19);
                builder.AppendFormat("|{0}:{1}", 0x77, num20);
                builder.AppendFormat("|{0}:{1}", 200, num21);
                builder.AppendFormat("|{0}:{1}", 0xc9, num22);
                builder.AppendFormat("|{0}:{1}", 0xca, num23);
                builder.AppendFormat("|{0}:{1}", 0xcb, num24);
                builder.AppendFormat("|{0}:{1}", 0xcc, num25);
                builder.AppendFormat("|{0}:{1}", 0xcd, num26);
                builder.AppendFormat("|{0}:{1}", 0xd0, num27);
                builder.AppendFormat("|{0}:{1}", 0xd1, num28);
                builder.AppendFormat("|{0}:{1}", 300, num29);
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

        private bool CheckOK()
        {
            string str = WebInfoFactory.GetAgent_Payrate_Setconfig();
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            decimal num = 0M;
            decimal num2 = 0M;
            decimal num3 = 0M;
            string[] strArray = str.Split(new char[] { '|' });
            foreach (string str2 in strArray)
            {
                string[] strArray2 = str2.Split(new char[] { ':' });
                if (strArray2[0] == "100")
                {
                    num3 = Convert.ToDecimal(this.txtp100.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "101")
                {
                    num3 = Convert.ToDecimal(this.txtp101.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "102")
                {
                    num3 = Convert.ToDecimal(this.txtp102.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "103")
                {
                    num3 = Convert.ToDecimal(this.txtp103.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "104")
                {
                    num3 = Convert.ToDecimal(this.txtp104.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "105")
                {
                    num3 = Convert.ToDecimal(this.txtp105.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "106")
                {
                    num3 = Convert.ToDecimal(this.txtp106.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "107")
                {
                    num3 = Convert.ToDecimal(this.txtp107.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "108")
                {
                    num3 = Convert.ToDecimal(this.txtp108.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "109")
                {
                    num3 = Convert.ToDecimal(this.txtp109.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "110")
                {
                    num3 = Convert.ToDecimal(this.txtp110.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "111")
                {
                    num3 = Convert.ToDecimal(this.txtp111.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "112")
                {
                    num3 = Convert.ToDecimal(this.txtp112.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "113")
                {
                    num3 = Convert.ToDecimal(this.txtp113.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "114")
                {
                    num3 = Convert.ToDecimal(this.txtp114.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "115")
                {
                    num3 = Convert.ToDecimal(this.txtp115.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "116")
                {
                    num3 = Convert.ToDecimal(this.txtp116.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "117")
                {
                    num3 = Convert.ToDecimal(this.txtp117.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "118")
                {
                    num3 = Convert.ToDecimal(this.txtp118.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "119")
                {
                    num3 = Convert.ToDecimal(this.txtp119.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "200")
                {
                    num3 = Convert.ToDecimal(this.txtp200.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "201")
                {
                    num3 = Convert.ToDecimal(this.txtp201.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "202")
                {
                    num3 = Convert.ToDecimal(this.txtp202.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "203")
                {
                    num3 = Convert.ToDecimal(this.txtp203.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "204")
                {
                    num3 = Convert.ToDecimal(this.txtp204.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "205")
                {
                    num3 = Convert.ToDecimal(this.txtp205.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "208")
                {
                    num3 = Convert.ToDecimal(this.txtp208.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "209")
                {
                    num3 = Convert.ToDecimal(this.txtp209.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
                else if (strArray2[0] == "300")
                {
                    num3 = Convert.ToDecimal(this.txtp300.Text.Trim());
                    num = Convert.ToDecimal(strArray2[1]) * 100M;
                    num2 = Convert.ToDecimal(strArray2[2]) * 100M;
                    if ((num3 < num) || (num3 > num2))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                if (string.IsNullOrEmpty(WebInfoFactory.GetAgent_Payrate_Setconfig()))
                {
                    base.Response.Write("未配置费率范围，不能操作");
                    base.Response.End();
                }
                this.ShowConfig();
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

        private void ShowConfig()
        {
            string str = WebInfoFactory.GetAgent_Payrate_Setconfig();
            if (!string.IsNullOrEmpty(str))
            {
                string[] strArray = str.Split(new char[] { '|' });
                foreach (string str2 in strArray)
                {
                    decimal num;
                    string[] strArray2 = str2.Split(new char[] { ':' });
                    if (strArray2[0] == "100")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp1000.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1001.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "101")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp1010.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1011.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "102")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp1020.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1021.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "103")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp1030.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1031.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "104")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp1040.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1041.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "105")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp1050.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1051.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "106")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp1060.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1061.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "107")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp1070.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1071.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "108")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp1080.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1081.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "109")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp1090.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1091.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "110")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp1100.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1101.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "111")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp1110.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1111.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "112")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp1120.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1121.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "113")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp1130.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1131.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "114")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp1140.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1141.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "115")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp1150.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1151.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "116")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp1160.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1161.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "117")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp1170.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1171.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "118")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp1180.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1181.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "119")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp1190.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1191.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "200")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp2000.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp2001.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "201")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp2010.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp2011.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "202")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp2020.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp2021.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "203")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp2030.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp2031.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "204")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp2040.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp2041.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "205")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp2050.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp2051.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "208")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp2080.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp2081.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "209")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp2090.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp2091.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "300")
                    {
                        this.txtp3000.Text = (Convert.ToDecimal(strArray2[1]) * 100M).ToString("0.00");
                        this.txtp3001.Text = (Convert.ToDecimal(strArray2[2]) * 100M).ToString("0.00");
                    }
                }
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
                        if (strArray2[0] == "100")
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
                            this.txtp209.Text = (Convert.ToDecimal(strArray2[1]) * 100M).ToString("0.00");
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

