namespace KuaiCard.WebUI.User.withdrawal
{
    using KuaiCard.BLL;
    using KuaiCard.BLL.Settled;
    using KuaiCard.BLL.User;
    using KuaiCard.Model;
    using KuaiCard.Model.Settled;
    using KuaiCard.WebComponents.Web;
    using System;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using KuaiCard.Model.User;

    public class index : UserPageBase
    {
        private TocashSchemeInfo _scheme = null;
        private WebInfo _webinfo = null;
        protected decimal alipay = 0M;
        protected int alipayT = 0;
        public string balancea = "0";
        protected decimal dianka = 0M;
        protected int diankaT = 0;
        protected int qqT = 0;
        public int getmsgcount;
        public string getnid = "";
        public string getnm = "";
        protected string id = string.Empty;
        protected string LabelBank = string.Empty;
        protected Label banktex;
        protected Label litTodayIncome;
        public string shouji = "";
        protected decimal shouxufei = 0M;
        protected decimal tixianedu = 0M;
        protected decimal wangyin = 0M;
        protected int wangyinT = 0;
        protected decimal weixin = 0M;
        protected int weixinT = 0;
        public string youxiang = "";
        protected string zongyue = "0";
        protected decimal zuigaoshouxu = 0M;
        protected decimal zuixiaoshouxu = 0M;

        /// <summary>
        /// 是否可以前台结算。2017.2.13增加
        /// </summary>
        public bool CanSettlesInFront = false;
        //2017.4.11 修改用变量绑定的方式
        protected string UserLastLoginTime = "";
        protected string UserLastLoginIp = "";
        protected string UserBalance = "0";
        protected string UserName = "";
        protected string UserMsgCount = "0";
        protected string UserDefaultThemes = "";
        protected string mUserFullName = "";


        public string GetLastStr(string str, int num)
        {
            int startIndex = 0;
            if (str.Length > num)
            {
                startIndex = str.Length - num;
                str = str.Substring(startIndex, num);
            }
            return str;
        }

        private void InitForm()
        {
            this.getnid = base.currentUser.ID.ToString();
            this.getnm = base.currentUser.UserName;
            try
            {
                this.getmsgcount = IMSGFactory.GetUserMsgCount(base.currentUser.ID);
            }
            catch
            {
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.currentUser == null)
            {
                //session过期，直接跳出
                this.Response.Redirect("~/User/loginout.aspx", true);
            }

            if (this.currentUser.settles_type == 1)
            {
                //接口方式，不允许通过前台操作
                this.Response.Redirect("~/User/", true);
            }

            this.UserLastLoginTime = this.currentUser.LastLoginTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.UserLastLoginIp = this.currentUser.LastLoginIp;
            this.UserBalance = ((this.balance - this.unpayment) - this.Freeze).ToString("f2");
            this.UserDefaultThemes = this.currentUser.default_themes;
            this.mUserFullName = this.currentUser.full_name;
            if (this.currentUser.IsRealNamePass == 1)
            {
                this.UserName = this.currentUser.UserName;
            }
            else
            {
                this.UserName = "平台商户";
            }

            this.CanSettlesInFront = (this.currentUser.settles_type == 0) || (this.currentUser.settles_type == 2);


            DataSet list = UserPayBankApp.GetList("userid=" + base.currentUser.ID);
            if (list != null)
            {
                foreach (DataRow row in list.Tables[0].Rows)
                {
                    this.banktex.Text = this.banktex.Text + string.Format("<div class=\"payment-choices card-{0}\" data-id=\"{1}\"><span>{2}</span></div>", 
                                                                                                    row["BankCode"].ToString(), row["id"].ToString(), this.GetLastStr(row["account"].ToString(), 4));
                }
            }
            TocashSchemeInfo modelByUser = TocashScheme.GetModelByUser(1, UserFactory.CurrentMember.ID);
            if (modelByUser == null)
            {
                base.AlertAndRedirect("未设置提现方案，请联系客服人员!!");
            }
            else
            {
                this.shouxufei = modelByUser.chargerate;                        //提现手续费
                this.tixianedu = modelByUser.minamtlimitofeach;             //最低提现金额限制(每笔)
                this.zuixiaoshouxu = modelByUser.chargeleastofeach;     //提现手续费最少每笔
                this.zuigaoshouxu = modelByUser.chargemostofeach;       //提现手续费最高每笔
                this.wangyinT = modelByUser.bankdetentiondays;              //网银T+几
                this.diankaT = modelByUser.carddetentiondays;               //点卡T+几 
                this.qqT = modelByUser.qqdetentiondays;                         //QQ钱包T+几 
                this.weixinT = modelByUser.weixindetentiondays;             //微信T+几 
                this.alipayT = modelByUser.alipaydetentiondays;             //支付宝T+几 
                /*

                 */

                DataSet ds = KuaiCard.BLL.Settled.Trade.GetUserLeftBalance(modelByUser.id, 
                                                            base.currentUser.ID, 
                                                            Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")),
                                                            DateTime.Now.AddDays(1.0));
                if (ds != null)
                {
                    string tmp = "";
                    tmp = ds.Tables[0].Rows[0]["balance"].ToString();     //总余额
                    this.zongyue = decimal.Round(Convert.ToDecimal(tmp), 2).ToString();

                    tmp = ds.Tables[0].Rows[0]["todayincome"].ToString();     //当天收入
                    this.litTodayIncome.Text = decimal.Round(Convert.ToDecimal(tmp), 2).ToString();

                    tmp = ds.Tables[0].Rows[0]["left_balance"].ToString();        //可提现
                    this.balancea =  decimal.Round(Convert.ToDecimal(tmp), 2).ToString();                   
                }
                else
                {
                    //=====原来的方式=====
                    decimal d = 0M;
                    if (modelByUser.bankdetentiondays > 0)
                    {
                        d += base.TodayIncomewangyin;
                    }
                    if (modelByUser.qqdetentiondays > 0)
                    {
                        d += base.TodayIncomeQQ;        //17.1.1修改为 QQ钱包收入。QQ钱包设置用点卡的设置
                    }
                    if (modelByUser.carddetentiondays > 0)
                    {
                        d += base.TodayIncomedianka;
                    }
                    if (modelByUser.otherdetentiondays > 0)
                    {
                        d += base.TodayIncomeqita;
                    }
                    if (modelByUser.alipaydetentiondays > 0)
                    {
                        d += base.TodayIncomealipay;
                    }
                    if (modelByUser.weixindetentiondays > 0)
                    {
                        d += base.TodayIncomeweixin;
                    }
                    if (((base.balance - base.unpayment) - base.Freeze) - d < 0)
                        this.balancea = ((base.balance - base.unpayment) - base.Freeze).ToString("f2");
                    else
                        this.balancea = (((base.balance - base.unpayment) - base.Freeze) - d).ToString("f2");
                    this.zongyue = ((base.balance - base.unpayment) - base.Freeze).ToString("f2");
                    this.litTodayIncome.Text = decimal.Round(d, 2).ToString();
                    //=====原来的方式=====
                }

                if (!base.IsPostBack)
                {
                    this.InitForm();
                }
            }

            this.DataBind();
        }

        protected TocashSchemeInfo scheme
        {
            get
            {
                if (this._scheme == null)
                {
                    this._scheme = TocashScheme.GetModelByUser(1, base.UserId);
                }
                return this._scheme;
            }
        }

        public WebInfo webInfo
        {
            get
            {
                if (this._webinfo == null)
                {
                    this._webinfo = WebInfoFactory.CurrentWebInfo;
                }
                return this._webinfo;
            }
        }
    }
}

