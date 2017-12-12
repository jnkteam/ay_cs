namespace OriginalStudio.WebUI.Manage.User
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Payment;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model.PayRate;
    using OriginalStudio.Model.User;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using OriginalStudio.BLL.PayRate;

    public class NewUser : ManagePageBase
    {
        protected Button Button1;
        protected CheckBox cb_isEmailPass;
        protected CheckBox cb_isPhonePass;
        protected CheckBox cb_isRealNamePass;
        protected DropDownList ddlagents;
        protected DropDownList ddlmange;
        protected DropDownList ddlStatus;
        protected TextBox email;
        protected HtmlForm form1;
        protected string loginip;
        protected string logintime;
        protected TextBox pwd;
        protected TextBox qq;
        protected RadioButtonList rbuserclass;
        protected TextBox shouji;
        protected TextBox TextBox2;
        protected TextBox txtfullname;
        protected TextBox txtGetPromSuperior;
        protected TextBox txtName;
        protected string username;

        protected void Button1_Click(object sender, EventArgs e)
        {
            MchUserBaseInfo info = new MchUserBaseInfo();
            //需要重新写，和前台对应
            /*
            info.UserName = this.txtName.Text;
            info.Password = this.pwd.Text;
            info.Email = this.email.Text;
            info.QQ = this.qq.Text;
            info.Tel = this.shouji.Text;
            info.SiteName = string.Empty;
            info.SiteUrl = this.TextBox2.Text;
            info.IdCard = string.Empty;
            info.full_name = this.txtfullname.Text;
            info.Status = SysConfig.IsAudit ? 1 : 2;
            info.PMode = 1;
            info.PayeeBank = string.Empty;
            info.BankProvince = string.Empty;
            info.BankCity = string.Empty;
            info.BankAddress = string.Empty;
            info.Account = string.Empty;
            info.PayeeName = string.Empty;
            info.classid = int.Parse(this.rbuserclass.SelectedValue);
            info.LastLoginIp = ServerVariables.TrueIP;
            info.LastLoginTime = DateTime.Now;
            info.RegTime = DateTime.Now;
            info.Status = int.Parse(this.ddlStatus.SelectedValue);
            info.Settles = SysConfig.DefaultSettledMode;
            info.CPSDrate = SysConfig.DefaultCPSDrate;
            info.IsRealNamePass = this.cb_isRealNamePass.Checked ? 1 : 0;
            info.IsEmailPass = this.cb_isEmailPass.Checked ? 1 : 0;
            info.IsPhonePass = this.cb_isPhonePass.Checked ? 1 : 0;
            info.AgentId = 0;
            info.APIAccount = 0L;
            info.APIKey = Guid.NewGuid().ToString("N");
            info.question = string.Empty;
            info.answer = string.Empty;
            if (!string.IsNullOrEmpty(this.ddlmange.SelectedValue))
            {
                info.manageId = new int?(int.Parse(this.ddlmange.SelectedValue));
            }
            else
            {
                info.manageId = 0;
            }
            */
            int num = 0;
            int num2 = MchUserFactory.Add(info);
            if (num2 > 0)
            {
                base.AlertAndRedirect("保存成功！", "UserList.aspx");

                //info.UserID = num2;
                //info.agentDistscheme = 0;
                //if (MchUserFactory.Update1(info))
                //{
                //    num = num2;
                //}
            }
            else
            {
                base.AlertAndRedirect("保存失败！");
            }
            if (!((num <= 0) || string.IsNullOrEmpty(this.ddlagents.SelectedValue)))
            {
                PromotionUserInfo promUser = new PromotionUserInfo();
                promUser.PID = int.Parse(this.ddlagents.SelectedValue);
                promUser.Prices = 0.5M;
                promUser.RegId = num;
                promUser.PromTime = DateTime.Now;
                promUser.PromStatus = 1;
                PromotionUserFactory.Insert(promUser);
            }
        }

        private void dldropDownList()
        {
            string s = this.ddlagents.SelectedValue.ToString();
            if (s != "")
            {
                //DataTable levName = SysPayRateFactory.GetLevName(RateTypeEnum.会员);
                //MchUserBaseInfo model = MchUserFactory.GetUserBaseByUserID(int.Parse(s));
                //if (model.ManageId > 0)
                //{
                //    this.ddlmange.SelectedValue = model.ManageId.ToString();
                //}
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                string str = new Random().Next(0x186a0, 0xf423f).ToString();
                this.pwd.Text = "888888";
                this.txtName.Text = str;
                foreach (int num in Enum.GetValues(typeof(UserStatusEnum)))
                {
                    string name = Enum.GetName(typeof(UserStatusEnum), num);
                    this.ddlStatus.Items.Add(new ListItem(name, num.ToString()));
                }
                DataTable levName = SysPayRateFactory.GetLevName(RateTypeEnum.会员);
                this.ddlmange.Items.Add(new ListItem("--请选择所属商务--", ""));
                levName = ManageFactory.GetList("status =1").Tables[0];
                foreach (DataRow row in levName.Rows)
                {
                    this.ddlmange.Items.Add(new ListItem(row["username"].ToString(), row["id"].ToString()));
                }
                this.ddlagents.Items.Add(new ListItem("--请选择代理员--", ""));
                levName = MchUserFactory.getAgentList();
                foreach (DataRow row in levName.Rows)
                {
                    this.ddlagents.Items.Add(new ListItem(row["username"].ToString(), row["userid"].ToString()));
                }
            }
            this.dldropDownList();
        }
    }
}

