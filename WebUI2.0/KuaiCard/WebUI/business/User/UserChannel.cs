namespace OriginalStudio.WebUI.business.User
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Channel;
    using OriginalStudio.BLL.Payment;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model;
    using OriginalStudio.Model.Channel;
    using OriginalStudio.Model.PayRate;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using OriginalStudio.BLL.Supplier;

    public class UserChannel : BusinessPageBase
    {
        protected Button btnAllColse;
        protected Button btnAllOpen;
        protected Button btnReSet;
        protected Button btnSave;
        protected HtmlForm form1;
        protected HtmlHead Head1;
        protected Label lblInfo;
        protected HiddenField puser;
        protected Repeater rpt_paymode;
        protected HtmlInputHidden selectedUsers;

        private void bind(DropDownList ddlctrl, int suppId)
        {
            string where = "isbank=1";
            DataTable table = SysSupplierFactory.GetList(where).Tables[0];
            ddlctrl.Items.Add(new ListItem("--默认--", "0"));
            foreach (DataRow row in table.Rows)
            {
                ddlctrl.Items.Add(new ListItem(row["name"].ToString(), row["code"].ToString()));
            }
            ddlctrl.SelectedValue = suppId.ToString();
        }

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

        protected void btnReSet_Click(object sender, EventArgs e)
        {
            ChannelTypeUsers.Setting(this.UserID, 3);
            this.LoadData();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in this.rpt_paymode.Items)
            {
                if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
                {
                    HiddenField field = item.FindControl("hftypeId") as HiddenField;
                    if (field != null)
                    {
                        int num = Convert.ToInt32(field.Value);
                        if (num == 0x66)
                        {
                            DropDownList list = item.FindControl("ddlsupp") as DropDownList;
                            if (list != null)
                            {
                                ChannelTypeUserInfo model = new ChannelTypeUserInfo();
                                model.updateTime = new DateTime?(DateTime.Now);
                                model.typeId = num;
                                model.updateTime = new DateTime?(DateTime.Now);
                                model.userId = this.UserID;
                                model.suppid = new int?(int.Parse(list.SelectedValue));
                                ChannelTypeUsers.AddSupp(model);
                            }
                        }
                    }
                }
            }
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
            DataTable table = ChannelType.GetList(new bool?(true)).Tables[0];
            if (!table.Columns.Contains("type_status"))
            {
                table.Columns.Add("type_status", typeof(string));
            }
            if (!table.Columns.Contains("sys_setting"))
            {
                table.Columns.Add("sys_setting", typeof(string));
            }
            if (!table.Columns.Contains("user_setting"))
            {
                table.Columns.Add("user_setting", typeof(string));
            }
            if (!table.Columns.Contains("payrate"))
            {
                table.Columns.Add("payrate", typeof(double));
            }
            if (!table.Columns.Contains("suppid"))
            {
                table.Columns.Add("suppid", typeof(int));
            }
            foreach (DataRow row in table.Rows)
            {
                int typeId = int.Parse(row["typeId"].ToString());
                bool flag = false;
                ChannelTypeUserInfo model = ChannelTypeUsers.GetModel(this.UserID, typeId);
                switch (ChannelType.GetModelByTypeId(typeId).isOpen)
                {
                    case OpenEnum.AllClose:
                    case OpenEnum.Close:
                        flag = false;
                        break;

                    case OpenEnum.AllOpen:
                    case OpenEnum.Open:
                        flag = true;
                        break;
                }
                row["type_status"] = flag ? "right" : "wrong";
                row["sys_setting"] = "Unknown";
                row["user_setting"] = "Unknown";
                row["suppid"] = 0;
                if (model != null)
                {
                    if (model.sysIsOpen.HasValue)
                    {
                        row["sys_setting"] = model.sysIsOpen.Value ? "right" : "wrong";
                    }
                    if (model.userIsOpen.HasValue)
                    {
                        row["user_setting"] = model.userIsOpen.Value ? "right" : "wrong";
                    }
                    if (model.suppid.HasValue)
                    {
                        row["suppid"] = model.suppid.Value;
                    }
                }
                row["payrate"] = 100M * PayRateFactory.GetPayRate(RateTypeEnum.会员, (int) UserFactory.GetModel(this.UserID).UserLevel, Convert.ToInt32(row["typeId"]));
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
            ChannelTypeUserInfo model = new ChannelTypeUserInfo();
            model.updateTime = new DateTime?(DateTime.Now);
            model.typeId = int.Parse(e.CommandArgument.ToString());
            model.updateTime = new DateTime?(DateTime.Now);
            model.userId = this.UserID;
            model.userIsOpen = null;
            if (e.CommandName == "open")
            {
                model.sysIsOpen = true;
            }
            else if (e.CommandName == "close")
            {
                model.sysIsOpen = false;
            }
            ChannelTypeUsers.Add(model);
            this.LoadData();
        }

        protected void rpt_paymode_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DataRowView dataItem = e.Item.DataItem as DataRowView;
                int typeId = int.Parse(dataItem["typeId"].ToString());
                ChannelTypeUserInfo model = ChannelTypeUsers.GetModel(this.UserID, typeId);
                if ((model != null) && model.sysIsOpen.HasValue)
                {
                    Button button = e.Item.FindControl("btn_open") as Button;
                    Button button2 = e.Item.FindControl("btn_close") as Button;
                    button.Enabled = !model.sysIsOpen.Value;
                    button2.Enabled = model.sysIsOpen.Value;
                }
                DropDownList ddlctrl = e.Item.FindControl("ddlsupp") as DropDownList;
                if (ddlctrl != null)
                {
                    ddlctrl.Visible = typeId == 0x66;
                    if (typeId == 0x66)
                    {
                        int suppId = int.Parse(dataItem["suppid"].ToString());
                        this.bind(ddlctrl, suppId);
                    }
                }
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

