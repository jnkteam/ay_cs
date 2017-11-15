namespace KuaiCard.WebUI.Manage.User
{
    using KuaiCard.BLL;
    using KuaiCard.BLL.Payment;
    using KuaiCard.Model;
    using KuaiCard.Model.Payment;
    using KuaiCard.WebComponents.Web;
    using KuaiCardLib.Text;
    using KuaiCardLib.Web;
    using System;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class agentratelimit : ManagePageBase
    {
        protected Button btnSave;
        protected HtmlForm form1;
        protected TextBox txtp100;
        protected TextBox txtp1001;
        protected TextBox txtp101;
        protected TextBox txtp1011;
        protected TextBox txtp102;
        protected TextBox txtp1021;
        protected TextBox txtp103;
        protected TextBox txtp1031;
        protected TextBox txtp104;
        protected TextBox txtp1041;
        protected TextBox txtp105;
        protected TextBox txtp1051;
        protected TextBox txtp106;
        protected TextBox txtp1061;
        protected TextBox txtp107;
        protected TextBox txtp1071;
        protected TextBox txtp108;
        protected TextBox txtp1081;
        protected TextBox txtp109;
        protected TextBox txtp1091;
        protected TextBox txtp110;
        protected TextBox txtp1101;
        protected TextBox txtp111;
        protected TextBox txtp1111;
        protected TextBox txtp112;
        protected TextBox txtp1121;
        protected TextBox txtp113;
        protected TextBox txtp1131;
        protected TextBox txtp114;
        protected TextBox txtp1141;
        protected TextBox txtp115;
        protected TextBox txtp1151;
        protected TextBox txtp116;
        protected TextBox txtp1161;
        protected TextBox txtp117;
        protected TextBox txtp1171;
        protected TextBox txtp118;
        protected TextBox txtp1181;
        protected TextBox txtp119;
        protected TextBox txtp1191;
        protected TextBox txtp200;
        protected TextBox txtp2001;
        protected TextBox txtp201;
        protected TextBox txtp2011;
        protected TextBox txtp202;
        protected TextBox txtp2021;
        protected TextBox txtp203;
        protected TextBox txtp2031;
        protected TextBox txtp204;
        protected TextBox txtp2041;
        protected TextBox txtp205;
        protected TextBox txtp2051;
        protected TextBox txtp208;
        protected TextBox txtp2081;
        protected TextBox txtp209;
        protected TextBox txtp2091;
        protected TextBox txtp300;
        protected TextBox txtp3001;

        protected void btnCopy_Click(object sender, EventArgs e)
        {
            PayRate modelByUser = PayRateFactory.GetModelByUser(this.ItemInfoId);
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
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp1001.Text))
            {
                msg = msg + "p1001格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp1011.Text))
            {
                msg = msg + "p1011格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp1021.Text))
            {
                msg = msg + "p1021格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp1031.Text))
            {
                msg = msg + "p1031格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp1041.Text))
            {
                msg = msg + "p1041格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp1051.Text))
            {
                msg = msg + "p1051格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp1061.Text))
            {
                msg = msg + "p1061格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp1071.Text))
            {
                msg = msg + "p1071格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp1081.Text))
            {
                msg = msg + "p1081格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp1091.Text))
            {
                msg = msg + "p1091格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp1101.Text))
            {
                msg = msg + "p1101格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp1111.Text))
            {
                msg = msg + "p1111格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp1121.Text))
            {
                msg = msg + "p1121格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp1131.Text))
            {
                msg = msg + "p1131格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp1141.Text))
            {
                msg = msg + "p1141格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp1151.Text))
            {
                msg = msg + "p1151格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp1161.Text))
            {
                msg = msg + "p1161格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp1171.Text))
            {
                msg = msg + "p11711格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp1181.Text))
            {
                msg = msg + "p1181格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp1191.Text))
            {
                msg = msg + "p1191格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp2001.Text))
            {
                msg = msg + "p2001格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp2011.Text))
            {
                msg = msg + "p2011格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp2021.Text))
            {
                msg = msg + "p2021格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp2031.Text))
            {
                msg = msg + "p2031格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp2041.Text))
            {
                msg = msg + "p2041格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp2051.Text))
            {
                msg = msg + "p2051格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp2081.Text))
            {
                msg = msg + "p2081格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp2091.Text))
            {
                msg = msg + "p2091格式错误！\n";
            }
            if (!KuaiCardLib.Text.Validate.IsNumber(this.txtp3001.Text))
            {
                msg = msg + "p3001格式错误！\n";
            }
            if (msg != "")
            {
                base.AlertAndRedirect(msg);
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
                decimal num30 = decimal.Parse(this.txtp1001.Text) / 100M;
                decimal num31 = decimal.Parse(this.txtp1011.Text) / 100M;
                decimal num32 = decimal.Parse(this.txtp1021.Text) / 100M;
                decimal num33 = decimal.Parse(this.txtp1031.Text) / 100M;
                decimal num34 = decimal.Parse(this.txtp1041.Text) / 100M;
                decimal num35 = decimal.Parse(this.txtp1051.Text) / 100M;
                decimal num36 = decimal.Parse(this.txtp1061.Text) / 100M;
                decimal num37 = decimal.Parse(this.txtp1071.Text) / 100M;
                decimal num38 = decimal.Parse(this.txtp1081.Text) / 100M;
                decimal num39 = decimal.Parse(this.txtp1091.Text) / 100M;
                decimal num40 = decimal.Parse(this.txtp1101.Text) / 100M;
                decimal num41 = decimal.Parse(this.txtp1111.Text) / 100M;
                decimal num42 = decimal.Parse(this.txtp1121.Text) / 100M;
                decimal num43 = decimal.Parse(this.txtp1131.Text) / 100M;
                decimal num44 = decimal.Parse(this.txtp1141.Text) / 100M;
                decimal num45 = decimal.Parse(this.txtp1151.Text) / 100M;
                decimal num46 = decimal.Parse(this.txtp1161.Text) / 100M;
                decimal num47 = decimal.Parse(this.txtp1171.Text) / 100M;
                decimal num48 = decimal.Parse(this.txtp1181.Text) / 100M;
                decimal num49 = decimal.Parse(this.txtp1191.Text) / 100M;
                decimal num50 = decimal.Parse(this.txtp2001.Text) / 100M;
                decimal num51 = decimal.Parse(this.txtp2011.Text) / 100M;
                decimal num52 = decimal.Parse(this.txtp2021.Text) / 100M;
                decimal num53 = decimal.Parse(this.txtp2031.Text) / 100M;
                decimal num54 = decimal.Parse(this.txtp2041.Text) / 100M;
                decimal num55 = decimal.Parse(this.txtp2051.Text) / 100M;
                decimal num56 = decimal.Parse(this.txtp2081.Text) / 100M;
                decimal num57 = decimal.Parse(this.txtp2091.Text) / 100M;
                decimal num58 = decimal.Parse(this.txtp3001.Text) / 100M;
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat("{0}:{1}:{2}", 100, num, num30);
                builder.AppendFormat("|{0}:{1}:{2}", 0x65, num2, num31);
                builder.AppendFormat("|{0}:{1}:{2}", 0x66, num3, num32);
                builder.AppendFormat("|{0}:{1}:{2}", 0x67, num4, num33);
                builder.AppendFormat("|{0}:{1}:{2}", 0x68, num5, num34);
                builder.AppendFormat("|{0}:{1}:{2}", 0x69, num6, num35);
                builder.AppendFormat("|{0}:{1}:{2}", 0x6a, num7, num36);
                builder.AppendFormat("|{0}:{1}:{2}", 0x6b, num8, num37);
                builder.AppendFormat("|{0}:{1}:{2}", 0x6c, num9, num38);
                builder.AppendFormat("|{0}:{1}:{2}", 0x6d, num10, num39);
                builder.AppendFormat("|{0}:{1}:{2}", 110, num11, num40);
                builder.AppendFormat("|{0}:{1}:{2}", 0x6f, num12, num41);
                builder.AppendFormat("|{0}:{1}:{2}", 0x70, num13, num42);
                builder.AppendFormat("|{0}:{1}:{2}", 0x71, num14, num43);
                builder.AppendFormat("|{0}:{1}:{2}", 0x72, num15, num44);
                builder.AppendFormat("|{0}:{1}:{2}", 0x73, num16, num45);
                builder.AppendFormat("|{0}:{1}:{2}", 0x74, num17, num46);
                builder.AppendFormat("|{0}:{1}:{2}", 0x75, num18, num47);
                builder.AppendFormat("|{0}:{1}:{2}", 0x76, num19, num48);
                builder.AppendFormat("|{0}:{1}:{2}", 0x77, num20, num49);
                builder.AppendFormat("|{0}:{1}:{2}", 200, num21, num50);
                builder.AppendFormat("|{0}:{1}:{2}", 0xc9, num22, num51);
                builder.AppendFormat("|{0}:{1}:{2}", 0xca, num23, num52);
                builder.AppendFormat("|{0}:{1}:{2}", 0xcb, num24, num53);
                builder.AppendFormat("|{0}:{1}:{2}", 0xcc, num25, num54);
                builder.AppendFormat("|{0}:{1}:{2}", 0xcd, num26, num55);
                builder.AppendFormat("|{0}:{1}:{2}", 0xd0, num27, num56);
                builder.AppendFormat("|{0}:{1}:{2}", 0xd1, num28, num57);
                builder.AppendFormat("|{0}:{1}:{2}", 300, num29, num58);
                if (WebInfoFactory.SetAgent_Payrate_Setconfig(builder.ToString()))
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
                        this.txtp100.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1001.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "101")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp101.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1011.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "102")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp102.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1021.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "103")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp103.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1031.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "104")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp104.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1041.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "105")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp105.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1051.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "106")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp106.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1061.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "107")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp107.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1071.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "108")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp108.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1081.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "109")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp109.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1091.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "110")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp110.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1101.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "111")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp111.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1111.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "112")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp112.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1121.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "113")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp113.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1131.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "114")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp114.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1141.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "115")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp115.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1151.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "116")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp116.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1161.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "117")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp117.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1171.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "118")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp118.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1181.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "119")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp119.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp1191.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "200")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp200.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp2001.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "201")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp201.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp2011.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "202")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp202.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp2021.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "203")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp203.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp2031.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "204")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp204.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp2041.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "205")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp205.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp2051.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "208")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp208.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp2081.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "209")
                    {
                        num = Convert.ToDecimal(strArray2[1]) * 100M;
                        this.txtp209.Text = num.ToString("0.00");
                        num = Convert.ToDecimal(strArray2[2]) * 100M;
                        this.txtp2091.Text = num.ToString("0.00");
                    }
                    else if (strArray2[0] == "300")
                    {
                        this.txtp300.Text = (Convert.ToDecimal(strArray2[1]) * 100M).ToString("0.00");
                        this.txtp3001.Text = (Convert.ToDecimal(strArray2[2]) * 100M).ToString("0.00");
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
    }
}

