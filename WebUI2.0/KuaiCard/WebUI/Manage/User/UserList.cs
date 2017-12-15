namespace OriginalStudio.WebUI.Manage.User
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Channel;
    using OriginalStudio.BLL.Payment;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model;
    using OriginalStudio.Model.User;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Wuqi.Webdiyer;

    public class UserList : ManagePageBase
    {
        protected Button btn_allgetmoney;
        protected Button btn_Msg;
        protected Button btnAdd;
        protected Button btnCashTo;
        protected Button btnDelete;
        protected Button btnSearch;
        protected DropDownList ddlisSpecialPayRate;
        protected DropDownList ddlpayrate;
        protected DropDownList ddlSpecial;
        protected DropDownList ddlUserType;
        protected HtmlForm form1;
        protected HtmlHead Head1;
        protected AspNetPager Pager1;
        protected Repeater rptUsers;
        protected HtmlInputHidden selectedUsers;
        protected DropDownList StatusList;
        protected TextBox txtagent;
        protected TextBox txtfullname;
        protected TextBox txtMail;
        protected TextBox txtQQ;
        protected TextBox txtTel;
        protected TextBox txtMerchantName;
        protected TextBox txtuserName;
        protected string wzfmoney = string.Empty;
        protected string yzfmoney = string.Empty;

        protected void btn_Msg_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["chkItem"];
            if (string.IsNullOrEmpty(str))
            {
                base.AlertAndRedirect("选择商户");
            }
            else
            {
                base.Response.Redirect("SendMsg.aspx?uid=" + str);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            base.Response.Redirect("UserAdd.aspx", true);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string str = base.Request.Form["chkItem"];
                foreach (string str2 in str.Split(new char[] { ',' }))
                {
                    MchUserFactory.Delete(int.Parse(str2));
                }
            }
            catch
            {
            }
            this.LoadData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }

        private void DoCmd()
        {
            if (!string.IsNullOrEmpty(this.cmd) && (this.UserID > 0))
            {
                List<UsersUpdateLog> changeList = new List<UsersUpdateLog>();
                MchUserBaseInfo model = MchUserFactory.GetUserBaseByUserID(this.UserID);
                if (this.cmd == "ok")
                {
                    //changeList.Add(this.newUpdateLog("Status", model.Status.ToString(), "2", "审核"));
                    model.Status = 2;
                }
                else if (this.cmd == "del")
                {
                    //changeList.Add(this.newUpdateLog("Status", model.Status.ToString(), "4", "锁定"));
                    model.Status = 4;
                }
                else if (this.cmd == "pok")
                {
                    //changeList.Add(this.newUpdateLog("UserType", ((int) model.UserType).ToString(), "2", "设为代理"));
                    //changeList.Add(this.newUpdateLog("UserLevel", ((int) model.UserLevel).ToString(), "1", "设为代理"));
                    model.UserType = UserTypeEnum.代理;
                    model.UserLevel = UserLevelEnum.普通代理;
                }
                else if (this.cmd == "pdel")
                {
                    //changeList.Add(this.newUpdateLog("UserType", ((int) model.UserType).ToString(), "1", "取消代理"));
                    //changeList.Add(this.newUpdateLog("UserLevel", ((int) model.UserLevel).ToString(), "100", "取消代理"));
                    model.UserType = UserTypeEnum.商户;
                    model.UserLevel = UserLevelEnum.初级代理;
                }
                /* 此处在修改后  需要打开*/
                if (MchUserFactory.Update(model))
                {
                    base.AlertAndRedirect("操作成功", "UserList.aspx");
                }
                else
                {
                    base.AlertAndRedirect("操作失败", "UserList.aspx");
                }
                
            }
        }

        protected string getpassview(object obj)
        {
            if ((obj == null) || (obj == DBNull.Value))
            {
                return string.Empty;
            }
            if (Convert.ToInt32(obj) > 0)
            {
                return "<a title='√' style='color:#1db283' href='javascript:void(0)'><i class='fa  fa-check-circle'></i></a>";
            }
            return "<a title='×'  style='color:#ff4a4a' href='javascript:void(0)'><i class='fa  fa-times-circle'></i></a>";
        }

        public string isSpecialChannel(object userid)
        {
            if (MchUsersChannelTypeFactory.Exists(Convert.ToInt32(userid)) == 1)
            {
                return "(独)";
            }
            return string.Empty;
        }

        private void LoadData()
        {
            List<SearchParam> searchParams = new List<SearchParam>();
            if (!base.isSuperAdmin)
            {
                searchParams.Add(new SearchParam("manageId", base.ManageId));
            }
            else if (this.manageid > 0)
            {
                searchParams.Add(new SearchParam("manageId", this.manageid));
            }
            if (this.proid > 0)
            {
                searchParams.Add(new SearchParam("proid", this.proid));
            }
            if (!string.IsNullOrEmpty(this.StatusList.SelectedValue))
            {
                searchParams.Add(new SearchParam("status", int.Parse(this.StatusList.SelectedValue)));
            }
            if (!string.IsNullOrEmpty(this.ddlUserType.SelectedValue))
            {
                searchParams.Add(new SearchParam("usertype", int.Parse(this.ddlUserType.SelectedValue)));
            }
            if (!string.IsNullOrEmpty(this.txtuserName.Text.Trim()))
            {
                searchParams.Add(new SearchParam("userName", this.txtuserName.Text.Trim()));
            }
            if (!string.IsNullOrEmpty(this.txtMerchantName.Text.Trim()))
            {
                searchParams.Add(new SearchParam("merchantname", txtMerchantName.Text.Trim()));
            }            /*
            if (!string.IsNullOrEmpty(this.ddlisSpecialPayRate.SelectedValue))
            {
                searchParams.Add(new SearchParam("special", int.Parse(this.ddlisSpecialPayRate.SelectedValue)));
            }
            */
            /*if (!string.IsNullOrEmpty(this.ddlpayrate.SelectedValue))
            {
                searchParams.Add(new SearchParam("userlevel", int.Parse(this.ddlpayrate.SelectedValue)));
            }
            
            if (!string.IsNullOrEmpty(this.txtQQ.Text.Trim()))
            {
                searchParams.Add(new SearchParam("qq", this.txtQQ.Text.Trim()));
            }
            if (!string.IsNullOrEmpty(this.txtMail.Text.Trim()))
            {
                searchParams.Add(new SearchParam("email", this.txtMail.Text.Trim()));
            }
            if (!string.IsNullOrEmpty(this.txtTel.Text.Trim()))
            {
                searchParams.Add(new SearchParam("tel", this.txtTel.Text.Trim()));
            }
            if (!string.IsNullOrEmpty(this.txtfullname.Text.Trim()))
            {
                searchParams.Add(new SearchParam("ContactName", this.txtfullname.Text.Trim()));
            }
            if (!string.IsNullOrEmpty(this.ddlSpecial.SelectedValue))
            {
                searchParams.Add(new SearchParam("specialchannel", this.ddlSpecial.SelectedValue));
            }
            */
            string orderby = this.orderBy + " " + this.orderByType;
            DataSet set = MchUserFactory.PageSearch(searchParams, this.Pager1.PageSize, this.Pager1.CurrentPageIndex, orderby);
            this.Pager1.RecordCount = Convert.ToInt32(set.Tables[0].Rows[0][0]);
            this.rptUsers.DataSource = set.Tables[1];
            this.rptUsers.DataBind();
        }

        private UsersUpdateLog newUpdateLog(string f, string n, string o, string desc)
        {
            UsersUpdateLog log = new UsersUpdateLog();
            log.userid = this.UserID;
            log.Addtime = DateTime.Now;
            log.field = f;
            log.newvalue = n;
            log.oldValue = o;
            log.Editor = ManageFactory.CurrentManage.username;
            log.OIp = ServerVariables.TrueIP;
            log.Desc = desc;
            return log;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            this.DoCmd();
            if (!base.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.UserStatus))
                {
                    this.StatusList.SelectedValue = this.UserStatus;
                }
                /*
                DataTable userlevData = PayRateFactory.GetUserlevData();
                foreach (DataRow row in userlevData.Rows)
                {
                    this.ddlpayrate.Items.Add(new ListItem(row["RateName"].ToString(), row["userLevel"].ToString()));
                }
                */
                if (this.proid > 0)
                {
                    this.txtagent.Text = this.proid.ToString();
                }
                this.LoadData();
            }
        }

        protected void Pager1_PageChanged(object sender, EventArgs e)
        {
            this.LoadData();
        }

        protected void rptUsersItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                string orderBy = this.orderBy;
                if ((orderBy != null) && (orderBy == "balance"))
                {
                    HyperLink link = (HyperLink) e.Item.FindControl("hlinkOrderby");
                    if (this.orderByType == "asc")
                    {
                        link.Text = "余额 <i class='fa fa-sort-amount-asc'></i>";
                        link.NavigateUrl = "?orderby=balance&type=desc&sign=19&menuId=20";
                    }
                    else
                    {
                        link.Text = "余额 <i class='fa fa-sort-amount-desc'></i>";
                        link.NavigateUrl = "?orderby=balance&type=asc&sign=19&menuId=20";
                    }
                }
            }
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string manageId = DataBinder.Eval(e.Item.DataItem, "manageId").ToString();

                string status = DataBinder.Eval(e.Item.DataItem, "status").ToString();

                Label label4 = (Label) e.Item.FindControl("lblUserSettle");
                //label4.Text = "T+" + str5;
                string str7 = DataBinder.Eval(e.Item.DataItem, "userid").ToString();
                string agcmd = string.Empty;
                if (status != null)
                {
                    if (!(status == "1"))
                    {
                        if (status == "2")
                        {
                            agcmd = string.Format("<a onclick=\"return confirm('你确定要锁定该用户吗？')\" href=\"?cmd=del&ID={0}\" style=\"color:red;\">锁定</a>", str7);
                        }
                        else if (status == "4")
                        {
                            agcmd = string.Format("<a onclick=\"return confirm('你确定要恢复该用户吗？')\" href=\"?cmd=ok&ID={0}\">恢复</a>", str7);
                        }
                    }
                    else
                    {
                        agcmd = string.Format("<a onclick=\"return confirm('你确定要通过该用户吗？')\" href=\"?cmd=ok&ID={0}\" style=\"color:Green;\">通过</a> <a onclick=\"return confirm('你确定要锁定该用户吗？')\" href=\"?cmd=del&ID={0}\" style=\"color:red;\">锁定</a>", str7);
                    }
                }
                Label label6 = (Label)e.Item.FindControl("labagcmd");
                label6.Text = agcmd;

                string relname = string.Empty;
                if (!string.IsNullOrEmpty(manageId))
                {
                    Manage model = ManageFactory.GetModel(int.Parse(manageId));
                    if (model != null)
                    {
                        relname = model.relname;
                    }
                }
                //Label label6 = (Label) e.Item.FindControl("labagcmd");
                //label6.Text = relname;
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

        public string cmd
        {
            get
            {
                return WebBase.GetQueryStringString("cmd", "");
            }
        }

        public int manageid
        {
            get
            {
                return WebBase.GetQueryStringInt32("manageid", 0);
            }
        }

        public string orderBy
        {
            get
            {
                return WebBase.GetQueryStringString("orderby", "balance");
            }
        }

        public string orderByType
        {
            get
            {
                return WebBase.GetQueryStringString("type", "asc");
            }
        }

        public int proid
        {
            get
            {
                return WebBase.GetQueryStringInt32("agentid", 0);
            }
        }

        public int UserID
        {
            get
            {
                return WebBase.GetQueryStringInt32("ID", 0);
            }
        }

        public string UserStatus
        {
            get
            {
                return WebBase.GetQueryStringString("UserStatus", "");
            }
        }
    }
}

