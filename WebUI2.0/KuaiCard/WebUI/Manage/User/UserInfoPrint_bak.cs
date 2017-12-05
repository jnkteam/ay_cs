namespace OriginalStudio.WebUI.Manage.User
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model;
    using OriginalStudio.Model.User;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.WebUI;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class UserInfoPrint_bak : ManagePageBase
    {
        public UserInfo _userInfo = null;
        protected HtmlForm form1;

        protected string spid = "";
        protected string spuserName = "";
        protected string spuserclass = "";
        protected string spfullname = "";
        protected string spuserType = "";
        protected string sppromvip = "";
        protected string spagents = "";
        protected string spmange = "";
        protected string spemail = "";
        protected string spqq = "";
        protected string sptel = "";
        protected string spidCard = "";
        protected string spsettlemode = "";
        protected string sppayeeBank = "";
        protected string spaccount = "";
        protected string sppayeeName = "";
        protected string spregTime = "";
        protected string splastLoginIp = "";
        protected string spsettledmode = "";
        protected string spisRealNamePass = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.ShowInfo();
            }
        }

        private void ShowInfo()
        {
            if (this.model != null)
            {
                spid = this.model.ID.ToString();
                spuserName = this.model.UserName.ToString();
                spuserclass = this.model.classid == 0 ? "个人" : "企业";
                spfullname =  this.model.full_name.ToString();
                spuserType = Enum.GetName(typeof(UserTypeEnum), this.model.UserType);
                if (this.model.UserType == UserTypeEnum.商户)
                {
                    this.sppromvip = "";// ((int)this.model.UserLevel).ToString();//会员等级：
                }
                if (this.model.UserType == UserTypeEnum.代理)
                {
                    this.sppromvip = "";// Enum.GetName(typeof(UserTypeEnum), this.model.province); 
                }
                
                spagents =  this.model.AgentId.ToString();  //
                spmange =  this.model.UserName.ToString();
                spemail =  this.model.UserName.ToString();
                spqq =  this.model.UserName.ToString();
                sptel =  this.model.UserName.ToString();
                spidCard =  this.model.UserName.ToString();
                spsettlemode =  this.model.UserName.ToString();
                sppayeeBank =  this.model.UserName.ToString();
                spaccount =  this.model.UserName.ToString();
                sppayeeName =  this.model.UserName.ToString();
                spregTime =  this.model.UserName.ToString();
                splastLoginIp =  this.model.UserName.ToString();
                spsettledmode =  this.model.UserName.ToString();
                spisRealNamePass = this.model.UserName.ToString();
                //this.txtuserid.Text = ;
                //this.txttypeid.Text = Enum.GetName(typeof(feedbacktype), (int) this.ItemInfo.typeid);
                //this.txttitle.Text = this.ItemInfo.title;
                //this.txtcont.Text = this.ItemInfo.cont;
                //this.txtstatus.Text = Enum.GetName(typeof(feedbackstatus), (int) this.ItemInfo.status);
                //this.txtaddtime.Value = this.ItemInfo.addtime.ToString("yyyy-MM-dd HH:mm:ss");
                //this.txtreply.Text = this.ItemInfo.reply;
                //this.txtreplyer.Text = this.ItemInfo.replyer.ToString();
                //this.txtreplytime.Value = this.ItemInfo.replytime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                //this.txtuserid.Enabled = false;
                //this.txttypeid.Enabled = false;
                //this.txttitle.Enabled = false;
                //this.txtcont.Enabled = false;
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
                if ((this._userInfo == null) && (this.ItemInfoId != 0))
                {
                    if (this.ItemInfoId > 0)
                    {
                        this._userInfo = UserFactory.GetModel(this.ItemInfoId);
                    }
                    else
                    {
                        this._userInfo = new UserInfo();
                    }
                }
                return this._userInfo;
            }
        }
    }
}

