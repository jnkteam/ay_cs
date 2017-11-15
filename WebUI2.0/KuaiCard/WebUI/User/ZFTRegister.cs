namespace KuaiCard.WebUI.User
{
    using KuaiCard.BLL;
    using KuaiCard.BLL.User;
    using KuaiCard.Model.User;
    using KuaiCardLib.Web;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using System.Text.RegularExpressions;

    public class ZFTRegister : KuaiCard.WebComponents.Web.PageBase
	{
        protected Button btnReg;
        protected TextBox txtusername;
        protected TextBox txtpassword;
        protected TextBox txtpassword2;
        protected TextBox txtfullname;
        protected TextBox txtidCard;
        protected TextBox txttel;
        protected TextBox txtemail;
        protected TextBox txtqq;
        protected DropDownList dlsquestion;
        protected TextBox txtanswer;
        protected TextBox txtimgcode;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.dlsquestion.DataSource = new Question().GetCacheList();
                this.dlsquestion.DataBind();
            }
        }
        
        protected void btnReg_Click(object sender, EventArgs e)
        {
            if (!SysConfig.IsOpenRegistration)
            {
                ShowMessage("系统关闭注册功能！");
                return;
            }

            if (this.Session["CCode"] == null)
            {
                ShowMessage("验证码已过期，请重新刷新本页！");
                return;
            }
            string imgcode = this.txtimgcode.Text.Trim().ToLower();
            if (this.Session["CCode"].ToString().ToLower() != imgcode)
            {
                this.AlertAndRedirect("验证码错误！");
                return;
            }

            UserInfo info = new UserInfo();
            info.UserName = this.txtusername.Text.Trim();
            info.Password = this.txtpassword.Text.Trim();
            info.Email = this.txtemail.Text.Trim();
            info.QQ = this.txtqq.Text.Trim();
            info.Tel = "";
            info.SiteName = "";
            info.SiteUrl = "";
            info.IdCard = this.txtidCard.Text.Trim(); ;
            info.full_name = this.txtfullname.Text.Trim();
            info.Status = 1;        //新注册用户
            info.PMode = 1;
            info.PayeeBank = string.Empty;
            info.BankProvince = string.Empty;
            info.BankCity = string.Empty;
            info.BankAddress = string.Empty;
            info.Account = string.Empty;
            info.PayeeName = string.Empty;
            info.classid = 0;
            info.LastLoginIp = ServerVariables.TrueIP;
            info.LastLoginTime = DateTime.Now;
            info.RegTime = DateTime.Now;
            info.Settles = SysConfig.DefaultSettledMode;
            info.CPSDrate = SysConfig.DefaultCPSDrate;
            info.IsRealNamePass = 0;
            info.IsEmailPass = 0;
            info.IsPhonePass = 0;
            info.AgentId = 0;
            info.APIAccount = 0L;
            info.APIKey = Guid.NewGuid().ToString("N");
            info.question = dlsquestion.SelectedValue;
            info.answer = this.txtanswer.Text.Trim();
            info.manageId = 0;
            int num2 = UserFactory.Add(info);

            if (num2 > 0)
            {
                base.AlertAndRedirect("用户注册成功！", "/Index.aspx");
            }
        }

        private void BindQuestion()
        {

        }

        /// <summary>
        /// 弹出提示信息。
        /// </summary>
        /// <param name="page">引用脚本的Page页</param>
        /// <param name="strMessage">提示信息</param>
        private void ShowMessage(string strMessage)
        {
            strMessage = "alert('" + strMessage + "');";
            if (!this.ClientScript.IsStartupScriptRegistered("showmsg"))
                this.ClientScript.RegisterStartupScript(this.GetType(), "showmsg", strMessage, true);
        }
	}
}
