namespace OriginalStudio.WebUI.business.User
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Channel;
    using OriginalStudio.BLL.Payment;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model;
    using OriginalStudio.Model.Channel;
    using OriginalStudio.Model.Payment;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class UserChannels : BusinessPageBase
    {
        protected Button btnAllColse;
        protected Button btnAllOpen;
        protected HtmlForm form1;
        protected HtmlHead Head1;
        protected Label lblInfo;
        protected HiddenField puser;
        protected Repeater rpt_paymode;
        protected HtmlInputHidden selectedUsers;

        protected void btnAllColse_Click(object sender, EventArgs e)
        {
            ChannelTypeUsers.Setting(this.UserID, 0);
            this.LoadData();
        }

        protected void btnAllOpen_Click(object sender, EventArgs e)
        {
            ChannelTypeUsers.Setting(this.UserID, 1);
            this.LoadData();
        }

        private void DoCmd()
        {
            if (((this.typeId > 0) && !string.IsNullOrEmpty(this.cmd)) && (this.ajaxUserId > 0))
            {
                ChannelTypeUserInfo model = new ChannelTypeUserInfo();
                model.userId = this.ajaxUserId;
                model.typeId = this.typeId;
                model.sysIsOpen = new bool?(this.cmd != "close");
                model.addTime = new DateTime?(DateTime.Now);
                model.userIsOpen = null;
                string s = "error";
                if (ChannelTypeUsers.Add(model) > 0)
                {
                    s = "success";
                }
                base.Response.Write(s);
                base.Response.End();
            }
        }

        private void LoadData()
        {
            DataTable table = OriginalStudio.BLL.Channel.ChannelType.GetList(new bool?(true)).Tables[0];
            if (!table.Columns.Contains("payrate"))
            {
                table.Columns.Add("payrate", typeof(double));
            }
            if (!table.Columns.Contains("plmodestatus"))
            {
                table.Columns.Add("plmodestatus", typeof(string));
            }
            if (!table.Columns.Contains("usermodestatus"))
            {
                table.Columns.Add("usermodestatus", typeof(string));
            }
            foreach (DataRow row in table.Rows)
            {
                int typeId = int.Parse(row["typeId"].ToString());
                bool flag = true;
                bool flag2 = false;
                ChannelTypeUserInfo model = ChannelTypeUsers.GetModel(this.UserID, typeId);
                switch (ChannelType.GetModelByTypeId(typeId).isOpen)
                {
                    case OpenEnum.AllClose:
                        flag2 = false;
                        break;

                    case OpenEnum.AllOpen:
                        flag2 = true;
                        break;

                    case OpenEnum.Close:
                        flag2 = false;
                        if (model != null)
                        {
                            if (model.sysIsOpen.HasValue)
                            {
                                flag2 = model.sysIsOpen.Value;
                            }
                            if (model.userIsOpen.HasValue)
                            {
                                flag = model.userIsOpen.Value;
                            }
                        }
                        break;

                    case OpenEnum.Open:
                        flag2 = true;
                        if ((model != null) && model.sysIsOpen.HasValue)
                        {
                            if (model.sysIsOpen.HasValue)
                            {
                                flag2 = model.sysIsOpen.Value;
                            }
                            if (model.userIsOpen.HasValue)
                            {
                                flag = model.userIsOpen.Value;
                            }
                        }
                        break;
                }
                row["payrate"] = 100M * PayRateFactory.GetPayRate(RateTypeEnum.会员, (int) UserFactory.GetModel(this.UserID).UserLevel, Convert.ToInt32(row["typeId"]));
                if (flag)
                {
                    row["usermodestatus"] = "right";
                }
                else
                {
                    row["usermodestatus"] = "wrong";
                }
                if (flag2)
                {
                    row["plmodestatus"] = "right";
                }
                else
                {
                    row["plmodestatus"] = "wrong";
                }
            }
            this.rpt_paymode.DataSource = table;
            this.rpt_paymode.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            this.DoCmd();
            if (!base.IsPostBack)
            {
                this.puser.Value = this.UserID.ToString();
                this.lblInfo.Text = "当前用户ID：" + this.UserID.ToString();
                this.LoadData();
            }
        }

        protected void rpt_paymode_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
        }

        private void setPower()
        {
            if (!ManageFactory.CheckCurrentPermission(false, ManageRole.Merchant))
            {
                base.Response.Write("Sorry,No authority!");
                base.Response.End();
            }
        }

        public int ajaxUserId
        {
            get
            {
                return WebBase.GetQueryStringInt32("userId", 0);
            }
        }

        public string cmd
        {
            get
            {
                return WebBase.GetQueryStringString("cmd", string.Empty);
            }
        }

        public int typeId
        {
            get
            {
                return WebBase.GetQueryStringInt32("id", 0);
            }
        }

        public int UserID
        {
            get
            {
                return WebBase.GetQueryStringInt32("ID", 0);
            }
        }
    }
}

