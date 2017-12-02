namespace OriginalStudio.WebUI.Manage.User
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
    using OriginalStudio.BLL.PayRate;

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
            string where = "";
            DataTable table = SysSupplierFactory.GetList(where).Tables[0];
            ddlctrl.Items.Add(new ListItem("--默认--", "0"));
            foreach (DataRow row in table.Rows)
            {
                ddlctrl.Items.Add(new ListItem(row["SupplierName"].ToString(), row["suppliercode"].ToString()));
            }
            ddlctrl.SelectedValue = suppId.ToString();
        }

        protected void btnAllColse_Click(object sender, EventArgs e)
        {
            MchUsersChannelTypeFactory.BatchSettingSupp(this.UserID, 0);
            this.LoadData();
        }

        protected void btnAllOpen_Click(object sender, EventArgs e)
        {
            MchUsersChannelTypeFactory.BatchSettingSupp(this.UserID, 1);
            this.LoadData();
        }

        protected void btnReSet_Click(object sender, EventArgs e)
        {
            MchUsersChannelTypeFactory.BatchSettingSupp(this.UserID, 3);
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
                        DropDownList list = item.FindControl("ddlsupp") as DropDownList;
                        if (list != null)
                        {
                            MchUserChannelType model = new MchUserChannelType();
                            
                            model.TypeID = num;
                            model.UpdateTime = DateTime.Now;
                            model.UserId = this.UserID;
                            model.SupplierCode = int.Parse(list.SelectedValue);
                            MchUsersChannelTypeFactory.AddSupp(model);
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
                SysChannelTypeInfo model = new SysChannelTypeInfo();
                /*
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
                */
                base.Response.Write("");
                base.Response.End();
            }
        }

        private void LoadData()
        {
            DataTable table = SysChannelType.GetList(new bool?(true)).Tables[0];
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
                
                MchUserChannelType model = MchUsersChannelTypeFactory.GetModel(this.UserID, typeId);
                switch (SysChannelType.GetModelByTypeId(typeId).IsOpen)
                {
                    case SysChannelTypeOpenEnum.AllClose:
                    case SysChannelTypeOpenEnum.Close:
                        flag = false;
                        break;

                    case SysChannelTypeOpenEnum.AllOpen:
                    case SysChannelTypeOpenEnum.Open:
                        flag = true;
                        break;
                }
               


                row["type_status"] = flag ? "<a title='√' style='color:#1db283' href='javascript:void(0)'> <i class='fa  fa-check-circle'></i></a>" : "<a title='×'  style='color:#ff4a4a' href='javascript:void(0)'> <i class='fa  fa-times-circle'></i></a>";
                row["sys_setting"] = "<a title='？' style='color:#000' href='javascript:void(0)'> <i class='fa   fa-question-circle'></i></a>";
                row["user_setting"] = "<a title='？' style='color:#000' href='javascript:void(0)'> <i class='fa   fa-question-circle'></i></a>";
                row["suppid"] = 0;
                if (model != null)
                {
                   
                    row["sys_setting"] = model.SysIsOpen ? "<a title='√' style='color:#1db283' href='javascript:void(0)'> <i class='fa  fa-check-circle'></i></a>" : "<a title='×'  style='color:#ff4a4a' href='javascript:void(0)'> <i class='fa  fa-times-circle'></i></a>";
                    
                   
                    row["user_setting"] = model.UserIsOpen ? "<a title='√' style='color:#1db283' href='javascript:void(0)'> <i class='fa  fa-check-circle'></i></a>" : "<a title='×'  style='color:#ff4a4a' href='javascript:void(0)'> <i class='fa  fa-times-circle'></i></a>";
                    
                    
                    if (model.SupplierCode > 0)
                    {
                        row["suppid"] = model.SupplierCode;
                    }
                    
            }
            row["payrate"] = 100M * SysPayRateFactory.GetSysChannelTypePayRate(RateTypeEnum.会员,  MchUserFactory.GetUserBaseByUserID(this.UserID).UserLevel, Convert.ToInt32(row["typeId"]));
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
            MchUserChannelType model = new MchUserChannelType();
            model.UpdateTime = DateTime.Now;
            model.TypeID = int.Parse(e.CommandArgument.ToString());
            
            model.UserId = this.UserID;
           
            model.UserIsOpen = false;
            if (e.CommandName == "open")
            {
                model.SysIsOpen = true;
            }
            else if (e.CommandName == "close")
            {
                model.SysIsOpen = false;
            }
            MchUsersChannelTypeFactory.Add(model);
            this.LoadData();
        }

        protected void rpt_paymode_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DataRowView dataItem = e.Item.DataItem as DataRowView;
                int typeId = int.Parse(dataItem["typeId"].ToString());
                MchUserChannelType model = MchUsersChannelTypeFactory.GetModel(this.UserID, typeId);
                if (model != null)
                {
                    Button button = e.Item.FindControl("btn_open") as Button;
                    Button button2 = e.Item.FindControl("btn_close") as Button;
                    button.Enabled = !model.SysIsOpen;
                    button2.Enabled = model.SysIsOpen;
                }
                DropDownList ddlctrl = e.Item.FindControl("ddlsupp") as DropDownList;
                if (ddlctrl != null)
                {
                    int suppId = int.Parse(dataItem["suppid"].ToString());
                    this.bind(ddlctrl, suppId);
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

