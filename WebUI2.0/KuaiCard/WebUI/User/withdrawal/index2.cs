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

    public class index2 : UserPageBase
    {
        private TocashSchemeInfo _scheme = null;
        private WebInfo _webinfo = null;
        protected decimal alipay = 0M;
        protected int alipayT = 0;
        public string balancea = "0";
        protected decimal dianka = 0M;
        protected int diankaT = 0;
        protected int qqT = 0;
        protected string id = string.Empty;
        protected string LabelBank = string.Empty;
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

        protected bool gHasBankFlag = false;   //
        protected string gBankList = "";
        protected string glimitTodayIncome = "0.00";

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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.currentUser.settles_type == 1)
            {
                //接口方式，不允许通过前台操作
                this.Response.Redirect("~/User/", true);
            }

            DataSet list = UserPayBankApp.GetList("userid=" + base.currentUser.ID);
            if (list != null)
            {
                //提现银行卡列表
                gHasBankFlag = list.Tables[0].Rows.Count > 0;
                foreach (DataRow row in list.Tables[0].Rows)
                {
                    //<li class=\"cashier-bank\"><label><input type=\"radio\" value=\"1002\" name=\"bankName\"/><img class=\"bank\" src=\"/img/banks/bank-ICBC.png\" /></label></li>
                    gBankList = gBankList + string.Format("<li class=\"cashier-bank\"><label><input type=\"radio\" value=\"{0}\" name=\"bankName\"/><img class=\"bank\" src=\"/images/bank-{1}.png\" />{2}</label></li>",
                                                                                                    row["id"].ToString(), 
                                                                                                    row["BankCode"].ToString(), 
                                                                                                    this.GetLastStr(row["account"].ToString(), 4)
                                                                                                    );
                }
            }

            //提现方案
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
                    glimitTodayIncome = decimal.Round(Convert.ToDecimal(tmp), 2).ToString();

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
                    glimitTodayIncome = decimal.Round(d, 2).ToString();
                    //=====原来的方式=====
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

