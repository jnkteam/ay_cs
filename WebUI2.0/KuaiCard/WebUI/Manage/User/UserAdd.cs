namespace OriginalStudio.WebUI.Manage.User
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Payment;
    using OriginalStudio.BLL.User;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model;
    using OriginalStudio.Model.PayRate;
    using OriginalStudio.Model.User;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Security;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using OriginalStudio.BLL.Withdraw;
    using OriginalStudio.BLL.PayRate;
    using System.Globalization;

    public class UserAdd : ManagePageBase
    {
        public MchUserBaseInfo _ItemInfo = null;
        protected Button btnAdd;
        protected HtmlForm form1;
        protected TextBox UserName;
        protected RadioButtonList ClassID; //签约属性
        protected TextBox ContactName;
        protected TextBox UserPwd;
        protected TextBox UserPayPwd; //提现密码
        protected TextBox MerchantName; //商户名
        protected TextBox ApiKey; //秘钥
        protected TextBox IDCard;
        protected RadioButtonList IsRealName;
        protected TextBox Phone;
        protected RadioButtonList IsPhone;
        protected TextBox EMail;
        protected RadioButtonList IsEmail;
        protected TextBox QQ;
        protected TextBox AddTime;
        protected DropDownList WithdrawSchemeID;
        protected DropDownList PayRateID;
        protected DropDownList manageId;//管理员
        protected TextBox SiteUrl;
        protected RadioButtonList WithdrawType;//结算方式
        protected TextBox RandomProduct;//随机商品名称
        protected TextBox LinkMan;//企业联系人
        protected DropDownList AgentID;
        protected DropDownList Status;
        protected TextBox LastLoginRemark;
        public string pwdDisplay;
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        private void InitForm()
        {


            DataTable WithdrawScheme = WithdrawSchemeFactory.GetList("type = 1").Tables[0];
            foreach (DataRow row in WithdrawScheme.Rows)
            {
                this.WithdrawSchemeID.Items.Add(new ListItem(row["SchemeName"].ToString(), row["id"].ToString()));
            }

            DataTable SysPayRate = SysPayRateFactory.GetList(RateTypeEnum.会员, 0);
            foreach (DataRow row in SysPayRate.Rows)
            {
                this.PayRateID.Items.Add(new ListItem(row["RateName"].ToString(), row["id"].ToString()));
            }
            DataTable manage = ManageFactory.GetList("status=1").Tables[0];
            foreach (DataRow row in manage.Rows)
            {
                this.manageId.Items.Add(new ListItem(row["userName"].ToString(), row["id"].ToString()));
            }

            DataTable MchUser = MchUserFactory.GetAgentList();
            foreach (DataRow row in MchUser.Rows)
            {
                this.AgentID.Items.Add(new ListItem(row["userName"].ToString(), row["userid"].ToString()));
            }
            foreach (int num2 in Enum.GetValues(typeof(UserStatusEnum)))
            {
                string name = Enum.GetName(typeof(UserStatusEnum), num2);
                this.Status.Items.Add(new ListItem(name, num2.ToString()));
            }




        }

        private UsersUpdateLog newUpdateLog(string f, string n, string o)
        {
            UsersUpdateLog log = new UsersUpdateLog();
            log.userid = this.model.UserID;
            log.Addtime = DateTime.Now;
            log.field = f;
            log.newvalue = n;
            log.oldValue = o;
            log.Editor = ManageFactory.CurrentManage.username;
            log.OIp = ServerVariables.TrueIP;
            return log;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            if (!base.IsPostBack)
            {
                this.InitForm();
                this.ShowInfo();

            }
        }

        private void Save()
        {
            List<UsersUpdateLog> changeList = new List<UsersUpdateLog>();


            this.model.UserName = this.UserName.Text;
            this.model.ClassID = int.Parse(this.ClassID.SelectedValue);
            this.model.ContactName = this.ContactName.Text;
            this.model.UserPwd = this.UserPwd.Text;
            this.model.UserPayPwd = this.UserPayPwd.Text;
            this.model.MerchantName = this.MerchantName.Text;
            this.model.ApiKey = this.ApiKey.Text;
            this.model.IDCard = this.IDCard.Text;
            this.model.IsRealName = this.IsRealName.SelectedValue == "1" ? true : false;
            this.model.Phone = this.Phone.Text;
            this.model.IsPhone = this.IsPhone.SelectedValue == "1" ? true : false;
            this.model.EMail = this.EMail.Text;
            this.model.IsEmail = this.IsEmail.SelectedValue == "1" ? true : false;
            this.model.QQ = this.QQ.Text;

            

            this.model.WithdrawSchemeID = int.Parse(this.WithdrawSchemeID.SelectedValue);
            this.model.PayRateID = int.Parse(this.PayRateID.SelectedValue);
            this.model.ManageId = int.Parse(this.manageId.SelectedValue);
            if (!string.IsNullOrEmpty(this.SiteUrl.Text.ToString()))
            {
                this.model.SiteUrl = this.SiteUrl.Text;
            }

            this.model.WithdrawType = int.Parse(this.WithdrawType.SelectedValue);
            if (!string.IsNullOrEmpty(this.RandomProduct.Text.ToString()))
            {
                this.model.RandomProduct = int.Parse(this.RandomProduct.Text); //类型未知
            }
            this.model.LinkMan = this.LinkMan.Text;
            this.model.AgentID = int.Parse(this.AgentID.SelectedValue);
            this.model.Status = int.Parse(this.Status.SelectedValue);
            this.model.LastLoginRemark = this.LastLoginRemark.Text;



            if (!this.isUpdate)
            {
                if (!string.IsNullOrEmpty(this.AddTime.Text.ToString()))
                {

                    this.model.AddTime = DateTime.ParseExact(this.AddTime.Text.ToString(), "yyyy/MM/dd hh:mm:ss", CultureInfo.InvariantCulture);
                }
                else
                {
                    this.model.AddTime = DateTime.Now;
                }
                if (MchUserFactory.Add(this.model) > 0)
                {
                    base.AlertAndRedirect("保存成功！");
                }
                else
                {
                    base.AlertAndRedirect("保存失败！");
                }
            }
            else if (MchUserFactory.Update(this.model))
            {
                base.AlertAndRedirect("更新成功！");
            }
            else
            {
                base.AlertAndRedirect("更新失败！");
            }
        }

        private void setPower()
        {
            if (!ManageFactory.CheckCurrentPermission(false, ManageRole.Merchant))
            {
                base.Response.Write("Sorry,No authority!");
                base.Response.End();
            }
        }

        private void ShowInfo()
        {
            this.pwdDisplay = "";
            if (this.isUpdate && (this.model != null))
            {
                this.pwdDisplay = "none";
                //MchUserBaseInfo promSuperior = MchUserFactory.GetPromSuperior(this.model.UserID);
                //OriginalStudio.Lib.Logging.LogHelper.Write(this.model.UserName);
                this.UserName.Text = this.model.UserName;
                this.ClassID.SelectedValue = this.model.ClassID.ToString();
                this.ContactName.Text = this.model.ContactName;
                this.UserPwd.Text = this.model.UserPwd;
                this.UserPayPwd.Text = this.model.UserPayPwd;
                this.MerchantName.Text = this.model.MerchantName;
                this.ApiKey.Text = this.model.ApiKey;
                this.IDCard.Text = this.model.IDCard;
                this.IsRealName.SelectedValue = this.model.IsRealName == true ? "1" : "0";
                this.Phone.Text = this.model.Phone;
                this.IsPhone.SelectedValue = this.model.IsPhone == true ? "1" : "0";
                this.EMail.Text = this.model.EMail;
                this.IsEmail.SelectedValue = this.model.IsEmail == true ? "1" : "0";
                this.QQ.Text = this.model.QQ;
                this.AddTime.Text = this.model.AddTime.ToString();
                this.SiteUrl.Text = this.model.SiteUrl;
                this.RandomProduct.Text = this.model.RandomProduct.ToString();

                this.LinkMan.Text = this.model.LinkMan;

                this.Status.SelectedValue = this.model.Status.ToString();
                this.LastLoginRemark.Text = this.model.LastLoginRemark;

                this.WithdrawSchemeID.SelectedValue = this._ItemInfo.WithdrawSchemeID.ToString();
                this.PayRateID.SelectedValue = this._ItemInfo.PayRateID.ToString();
                this.manageId.SelectedValue = this._ItemInfo.ManageId.ToString();
                this.AgentID.SelectedValue = this._ItemInfo.AgentID.ToString();
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

        public MchUserBaseInfo model
        {
            get
            {
                if (this._ItemInfo == null)
                {
                    if (this.ItemInfoId > 0)
                    {
                        this._ItemInfo = MchUserFactory.GetUserBaseByUserID(this.ItemInfoId);
                    }
                    else
                    {
                        this._ItemInfo = new MchUserBaseInfo();
                    }
                }
                return this._ItemInfo;
            }
        }
    }
}

