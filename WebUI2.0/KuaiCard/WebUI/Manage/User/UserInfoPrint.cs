namespace KuaiCard.WebUI.Manage.User
{
    using KuaiCard.BLL;
    using KuaiCard.BLL.User;
    using KuaiCard.Model;
    using KuaiCard.Model.User;
    using KuaiCard.WebComponents.Web;
    using KuaiCard.WebUI;
    using KuaiCardLib.Web;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Data;
    using KuaiCard.BLL.Payment;
    using KuaiCard.Model.Payment;

    public class UserInfoPrint : ManagePageBase
    {
        public UserInfo _ItemInfo = null;
        private usersettingInfo _setting = null;
        protected HtmlForm form1;

        protected Label lblid;
        protected Label lblUserName;
        protected Label lbllastLoginIp;
        protected Label lblregTime;
        protected Label lblaccount;
        protected Label lblemail;
        protected Label lblfullname;
        protected Label lblidCard;
        protected Label lblpayeeName;
        protected Label lblqq;
        protected Label lbltel;
        protected Label lbluserType;
        protected Label lblagents;
        protected Label lblmange;
        protected Label lblsettlemode;
        protected Label lblsettledmode;
        protected Label lbluserclass;
        protected Label lblpayeeBank;
        protected Label lblmemvip;
        protected Label lblPass;
        protected Label lblfirstLoginIp;

        protected string frontPic = "";
        protected string backPic = "";
        protected int imgID = 0;

        private void InitForm()
        {
            if (!base.isSuperAdmin)
            {
                int? manageId = this.model.manageId;
                int id = base.currentManage.id;
                if (!((manageId.GetValueOrDefault() == id) && manageId.HasValue))
                {
                    base.Response.Write("Sorry,No authority!");
                    base.Response.End();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.InitForm();
                this.ShowInfo();
            }
        }

        private void ShowInfo()
        {
            try
            {
                if (this.model != null)
                {
                    this.lblid.Text = this.model.ID.ToString();
                    this.lblUserName.Text = this.model.UserName;
                    this.lblfullname.Text = this.model.full_name;
                    this.lblemail.Text = this.model.Email;
                    this.lblqq.Text = this.model.QQ;
                    this.lbltel.Text = this.model.Tel;
                    this.lblidCard.Text = this.model.IdCard;
                    this.lblregTime.Text = this.model.RegTime.ToString("yyyy-MM-dd HH:mm");
                    this.lbluserclass.Text = this.model.classid == 0 ? "个人" : "企业";
                    if (this.model.UserType == UserTypeEnum.会员)
                        lbluserType.Text = "会员";
                    if (this.model.UserType == UserTypeEnum.代理)
                        lbluserType.Text = "代理";
                    this.lbllastLoginIp.Text = this.model.LastLoginIp;
                    this.lblaccount.Text = this.model.Account;
                    this.lblpayeeName.Text = this.model.PayeeName;
                    //开户银行
                    if (this.model.PayeeBank != "--开户银行--")
                        this.lblpayeeBank.Text = this.model.PayeeBank;
                    /*    */
                    DataTable levName = new DataTable();
                    if (this.model.UserType == UserTypeEnum.会员)
                    {
                        lblmemvip.Text = this.model.UserLevel.ToString();
                    }
                    else if (this.model.UserType == UserTypeEnum.代理)
                    {
                        lblagents.Text = this.model.UserLevel.ToString();
                    }
                    //所属业务员
                    levName = ManageFactory.GetList(" status =1").Tables[0];
                    foreach (DataRow row2 in levName.Rows)
                    {
                        if (this.model.manageId.ToString() == row2["id"].ToString())
                            lblmange.Text = row2["username"].ToString();
                    }

                    //收款方式
                    if (this.model.PMode == 1)
                        this.lblsettlemode.Text = "银行帐户";
                    else if (this.model.PMode == 2)
                        this.lblsettlemode.Text = "支付宝";
                    else if (this.model.PMode == 3)
                        this.lblsettlemode.Text = "财付通";

                    //结算模式
                    if (this.model.Settles == 0)
                        this.lblsettledmode.Text = "T+0";
                    else if (this.model.Settles == 1)
                        this.lblsettledmode.Text = "T+1";

                    //认证信息
                    if (this.model.IsRealNamePass == 1)
                        lblPass.Text = "实名认证" + "　　";
                    if (this.model.IsEmailPass == 1)
                        lblPass.Text += "邮件认证" + "　　";
                    if (this.model.IsPhonePass == 1)
                        lblPass.Text += "手机认证" + "　　";
                    if (this.setting.istransfer == 1)
                        lblPass.Text += "开启转账" + "　　";
                    if (this.model.isagentDistribution == 1)
                        lblPass.Text += "对私代发";

                    //身份证照片
                    //usersIdImageInfo idImg = new usersIdImage().GetModelByUser(this.ItemInfoId);
                    //if (idImg != null)
                    //    imgID = idImg.id;
                    this.frontPic = this.model.frontPic;
                    this.backPic = this.model.versoPic;

                    //首次登陆ip
                    DataSet ds = UserFactory.GetUserFirstLogin(this.ItemInfoId);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        lblfirstLoginIp.Text = ds.Tables[0].Rows[0]["lastIP"].ToString();
                    }

                    this.DataBind();
                }
            }
            catch (Exception err)
            {
                this.Response.Write(err.Message.ToString());
            }
        }

        public int ItemInfoId
        {
            get
            {
                return WebBase.GetQueryStringInt32("id", 0);
            }
        }

        public UserInfo model
        {
            get
            {
                if (this._ItemInfo == null)
                {
                    if (this.ItemInfoId > 0)
                    {
                        this._ItemInfo = UserFactory.GetModel(this.ItemInfoId);
                    }
                    else
                    {
                        this._ItemInfo = new UserInfo();
                    }
                }
                return this._ItemInfo;
            }
        }

        public usersettingInfo setting
        {
            get
            {
                usersetting setbll = new usersetting();
                this._setting = setbll.GetModel(this.ItemInfoId);
                if (this._setting == null)
                {
                    this._setting = new usersettingInfo();
                }
                return this._setting;
            }
        }
    }
}

