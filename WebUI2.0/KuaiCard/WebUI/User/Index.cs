namespace OriginalStudio.WebUI.User
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.Lib.Text;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using OriginalStudio.BLL.News;

    public class Index : UserPageBase
    {
        protected int bankcoe = 0;
        public string businessid = "";
        public string businessname = "";
        public string businessqq = "";
        public string businesstel = "";
        protected HtmlGenericControl classemail;
        protected HtmlGenericControl classshouji;
        protected HtmlGenericControl classtixian;

        public string getnid = "";
        public string getnm = "";
        protected HtmlAnchor linemail;
        protected HtmlAnchor linshouji;
        protected HtmlAnchor lintixian;
        protected Literal litbalance;
        protected Literal litEmai;
        public string LitLleixing = "";
        protected Literal LitUserName;
        //protected HtmlGenericControl nameclass;
        protected Repeater rptNews;
        protected HtmlGenericControl shiming;
        protected HtmlAnchor shimingtext;
        public string shouji = "";
        protected HtmlGenericControl Span1;
        protected string totalordertotal = "0";
        protected string totalordertotal1 = "0";
        protected string totalordertotal2 = "0";
        protected string totalrealvalue = "0";
        protected string totalrealvalue1 = "0";
        protected string totalrealvalue2 = "0";
        protected string totalrefervalue = "0";
        protected string totalrefervalue1 = "0";
        protected string totalrefervalue2 = "0";
        protected string totalpayamt1 = "0";

        protected string totalsuccordertotal = "0";
        protected string totalsuccordertotal1 = "0";
        protected string totalsuccordertotal2 = "0";
        protected string totalsuccorderpayamt = "0";
        public string youxiang = "";
        /// <summary>
        /// 实名认证标记
        /// </summary>
        public bool gblnRealName = false;

        protected bool gblnBindIP = false;

        public string builderdate(string date, string hour, string m, string s)
        {
            return string.Format("{0} {1}:{2}:{3}", new object[] { date, hour, m, s });
        }

        private void InitForm()
        {
            this.bankcoe = 0;// UserPayBankApp.GetIsRead(base.currentUser.ID);
            this.getnid = base.CurrentUser.ID.ToString();
            this.getnm = base.CurrentUser.UserName;
            this.litbalance.Text = ((base.Balance - base.Unpayment) - base.Freeze).ToString("f2");

            if (base.CurrentUser.manageId.HasValue)
            {
                Manage model = ManageFactory.GetModel(int.Parse(base.CurrentUser.manageId.ToString()));
                if (model != null)
                {
                    this.businessname = model.relname;
                    this.businessqq = model.qq;
                    this.businesstel = model.tel;
                    this.businessid = "800" + model.id.ToString();
                }
            }
            this.litEmai.Text = Strings.Mark(base.CurrentUser.Email);
            if (base.CurrentUser.IsEmailPass == 1)
            {
                this.classemail.Attributes["class"] = "color_ju";
                this.classemail.InnerText = "已绑定";
                this.linemail.InnerText = "修改";
                this.linemail.Attributes["href"] = "/user/email/";
            }
            else
            {
                this.classemail.Attributes["class"] = "color_fei";
                this.classemail.InnerText = "未绑定";
                this.linemail.InnerText = "绑定";
                if (OriginalStudio.Lib.SysConfig.RuntimeSetting.SiteUser.ToLower() == "zft")
                    this.linemail.Attributes["href"] = "/user/email/";
                else
                    this.linemail.Attributes["href"] = "/user/validate/set.aspx";
            }
            if (base.CurrentUser.IsPhonePass == 1)
            {
                this.classshouji.Attributes["class"] = "color_ju";
                this.classshouji.InnerText = "已绑定";
                this.linshouji.InnerText = "修改";
                this.linshouji.Attributes["href"] = "/user/mobile/";
            }
            else
            {
                this.classshouji.Attributes["class"] = "color_fei";
                this.classshouji.InnerText = "未绑定";
                this.linshouji.InnerText = "绑定";
                if (OriginalStudio.Lib.SysConfig.RuntimeSetting.SiteUser.ToLower() == "zft")
                    this.linshouji.Attributes["href"] = "/user/mobile/";
                else
                    this.linshouji.Attributes["href"] = "/user/validate/tel.aspx";
            }
            if (!string.IsNullOrEmpty(base.CurrentUser.Password2))
            {
                this.classtixian.Attributes["class"] = "color_ju";
                this.classtixian.InnerText = "已设置";
                this.lintixian.InnerText = "修改";
            }
            else
            {
                this.classtixian.Attributes["class"] = "color_fei";
                this.classtixian.InnerText = "未设置";
                this.lintixian.InnerText = "设置";
            }

            gblnRealName = base.CurrentUser.IsRealNamePass == 1;
            this.LitUserName.Text = gblnRealName ? base.CurrentUser.full_name : "平台商户";
            this.LitLleixing = base.CurrentUser.accoutType == 1?"企业":"个人";

            DataTable table = NewsFactory.GetNewsList(2, 4, 0);
            if ((table != null) && (table.Rows.Count > 0))
            {
                this.rptNews.DataSource = table;
                this.rptNews.DataBind();
            }
            List<SearchParam> searchParams = new List<SearchParam>();
            searchParams.Add(new SearchParam("userid", base.UserId));
            DateTime minValue = DateTime.MinValue;
            if (DateTime.TryParse(this.builderdate(DateTime.Now.ToString("yyyy-MM-dd"), "00", "00", "00"), out minValue) && (minValue > DateTime.MinValue))
            {
                searchParams.Add(new SearchParam("stime", minValue.ToString()));
            }
            if (DateTime.TryParse(this.builderdate(DateTime.Now.ToString("yyyy-MM-dd"), "23", "59", "59"), out minValue) && (minValue > DateTime.MinValue))
            {
                searchParams.Add(new SearchParam("etime", minValue.ToString()));
            }
            string orderby = string.Empty;
            OrderBank bank = new OrderBank();
            try
            {
                DataTable table2 = bank.UserPageSearch(searchParams, 20, 1, orderby).Tables[2];
                if ((table2 != null) && (table2.Rows.Count > 0))
                {
                    this.totalrefervalue1 = Convert.ToDecimal(table2.Rows[0]["refervalue"]).ToString("f0");
                    this.totalrealvalue1 = Convert.ToDecimal(table2.Rows[0]["realvalue"]).ToString("f0");
                    this.totalordertotal1 = Convert.ToDecimal(table2.Rows[0]["ordtotal"]).ToString("f0");
                    this.totalsuccordertotal1 = Convert.ToDecimal(table2.Rows[0]["succordtotal"]).ToString("f0");
                    this.totalsuccordertotal1 = Convert.ToDecimal(table2.Rows[0]["succordtotal"]).ToString("f0");
                    this.totalpayamt1 = Convert.ToDecimal(table2.Rows[0]["payAmt"]).ToString("f0");
                    //if (table2.Rows[0]["typeId"].ToString() == "98")
                    //{
                    //    //this.totalordertotal1 = Convert.ToDecimal(table2.Rows[0]["realvalue"]).ToString("f0");
                    //    this.totalordertotal1 = Convert.ToDecimal(table2.Rows[0]["ordtotal"]).ToString("f0");
                    //}
                    //if (table2.Rows[0]["typeId"].ToString() == "99")
                    //{
                    //    //this.totalsuccordertotal1 = Convert.ToDecimal(table2.Rows[0]["realvalue"]).ToString("f0");
                    //    this.totalsuccordertotal1 = Convert.ToDecimal(table2.Rows[0]["succordtotal"]).ToString("f0");
                    //}

                }
            }
            catch { }
            searchParams = new List<SearchParam>();
            searchParams.Add(new SearchParam("userid", base.UserId));
            minValue = DateTime.MinValue;
            if (DateTime.TryParse(this.builderdate(DateTime.Now.ToString("yyyy-MM-dd"), "00", "00", "00"), out minValue) && (minValue > DateTime.MinValue))
            {
                searchParams.Add(new SearchParam("stime", minValue.ToString()));
            }
            if (DateTime.TryParse(this.builderdate(DateTime.Now.ToString("yyyy-MM-dd"), "23", "59", "59"), out minValue) && (minValue > DateTime.MinValue))
            {
                searchParams.Add(new SearchParam("etime", minValue.ToString()));
            }
            orderby = string.Empty;
            this.totalordertotal = (Convert.ToDouble(this.totalordertotal1) + Convert.ToDouble(this.totalordertotal2)).ToString();
            this.totalsuccordertotal = (Convert.ToDouble(this.totalsuccordertotal1) + Convert.ToDouble(this.totalsuccordertotal2)).ToString();
            this.totalrefervalue = (Convert.ToDouble(this.totalrefervalue1) + Convert.ToDouble(this.totalrefervalue2)).ToString();
            this.totalrealvalue = (Convert.ToDouble(this.totalrealvalue1) + Convert.ToDouble(this.totalrealvalue2)).ToString();
            this.totalsuccorderpayamt = (Convert.ToDouble(this.totalpayamt1)).ToString();
            
            //绑定IP
            this.gblnBindIP = UserFactory.GetUserBindIp(base.CurrentUser.ID).Tables[0].Rows.Count > 0;

            this.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.CurrentUser == null)
            {
                //session过期，直接跳出
                this.Response.Redirect("~/User/loginout.aspx", true);
            }

            if (base.CurrentUser.IsPhonePass == 1)
            {
                this.shouji = "user/mobile/";
            }
            else
            {
                this.shouji = "user/validate/Tel.aspx";
            }
            if (base.CurrentUser.IsEmailPass == 1)
            {
                this.youxiang = "user/email/";
            }
            else
            {
                this.youxiang = "user/email/set.aspx";
            }
            if (!base.IsPostBack)
            {
                this.InitForm();
            }
        }
    }
}

