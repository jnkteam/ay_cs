namespace KuaiCard.WebUI.Manage
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Channel;
    using OriginalStudio.Model;
    using OriginalStudio.Model.Channel;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Collections.Generic;

    public class UserSupplierConfig : ManagePageBase
    {
        protected HtmlForm form1;
        protected TextBox txtUserID;
        protected TextBox txtUserName;
        protected DropDownList ddlsupp;
        protected CheckBoxList chkChannelType;
        protected TextBox txtpuserid;
        protected TextBox txtName;
        protected TextBox txtpuserkey;
        protected TextBox txtpusername;
        protected TextBox txtpuserid1;
        protected TextBox txtpuserkey1;
        protected TextBox txtpuserid2;
        protected TextBox txtpuserkey2;
        protected TextBox txtpostBankUrl;
        protected TextBox txtdesc;
        protected TextBox txtsort;
        protected TextBox txtJumpUrl;
        protected TextBox txtdistributionUrl;
        protected Button btnSave;
        protected HiddenField hidID;
        protected TextBox txtExtParm1;
        protected TextBox txtExtParm2;
        protected Repeater rptList;

        public KuaiCard.Model.UserSupplierInfo ItemModel = new UserSupplierInfo();

        public int UserID
        {
            get
            {
                return WebBase.GetQueryStringInt32("userid", 0);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.hidID.Value))
                ItemModel.ID = Convert.ToInt32(hidID.Value);
            else
                ItemModel.ID = 0;
            this.ItemModel.UserID = this.UserID;
            this.ItemModel.Name = "";
            this.ItemModel.SupplierCode = Convert.ToInt32(this.ddlsupp.SelectedValue);
            this.ItemModel.PUserID = this.txtpuserid.Text.Trim();
            this.ItemModel.PUserKey = this.txtpuserkey.Text.Trim();
            this.ItemModel.PUserName = this.txtpusername.Text.Trim();
            this.ItemModel.PUserID1 = this.txtpuserid1.Text.Trim();
            this.ItemModel.PUserKey1 = this.txtpuserkey1.Text.Trim();
            this.ItemModel.PUserID2 = this.txtpuserid2.Text.Trim();
            this.ItemModel.PUserKey2 = this.txtpuserkey2.Text.Trim();
            this.ItemModel.PostBankUrl = this.txtpostBankUrl.Text.Trim();
            this.ItemModel.Desc = this.txtdesc.Text.Trim();
            this.ItemModel.Sort = KuaiCardLib.Utils.Utils.StrToInt(this.txtsort.Text.Trim(), 0);
            this.ItemModel.JumpUrl = this.txtJumpUrl.Text.Trim();
            this.ItemModel.distributionUrl = this.txtdistributionUrl.Text.Trim();
            this.ItemModel.Active = 1;
            this.ItemModel.ExtParm1 = this.txtExtParm1.Text.Trim();
            this.ItemModel.ExtParm2 = this.txtExtParm2.Text.Trim();
            this.ItemModel.ExtParm3 = "";
            this.ItemModel.ExtParm4 = "";
            for (int i = 0; i < this.chkChannelType.Items.Count; i++)
            {
                if (this.chkChannelType.Items[i].Selected)
                {
                    this.ItemModel.ChannelTypeList.Add(this.chkChannelType.Items[i].Value.ToString());
                }
            }

            if (this.ItemModel.ID == 0)
            {
                if (KuaiCard.BLL.SysSupplierFactory.SaveUserSupplierInfo(this.ItemModel) > 0)
                {
                    base.AlertAndRedirect("保存成功！", "UserSupplierConfig.aspx?userid=" + this.ItemModel.UserID.ToString());
                }
                else
                {
                    base.AlertAndRedirect("保存失败！");
                }
            }
            else if (KuaiCard.BLL.SysSupplierFactory.UpdateUserSupplierInfo(this.ItemModel) > 0)
            {
                this.hidID.Value = "";
                base.AlertAndRedirect("更新成功！", "UserSupplierConfig.aspx?userid=" + this.ItemModel.UserID.ToString());
            }
            else
            {
                base.AlertAndRedirect("更新失败！");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                LoadInfo();

                LoadList();

                ShowInfo();
            }
        }

        private void setPower()
        {
            if (!ManageFactory.CheckCurrentPermission(false, ManageRole.Interfaces))
            {
                base.Response.Write("Sorry,No authority!");
                base.Response.End();
            }
        }

        private void ShowInfo()
        {
            string p_cmd = KuaiCardLib.Web.WebBase.GetQueryStringString("cmd","").ToLower();
            int p_id = KuaiCardLib.Web.WebBase.GetQueryStringInt32("id", 0);
            if (p_cmd == "edit" && p_id > 0)
            {
                DataSet ds = KuaiCard.BLL.SysSupplierFactory.GetUserSupplierList(this.UserID);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataRow[] dr = ds.Tables[0].Select("id = " + p_id.ToString());
                    if (dr.Length == 0) return;

                    this.hidID.Value = p_id.ToString();
                    this.ddlsupp.SelectedValue = dr[0]["supplier_code"].ToString();
                    this.txtpuserid.Text = dr[0]["puserid"].ToString();
                    this.txtpuserkey.Text = dr[0]["puserkey"].ToString();
                    this.txtpusername.Text = dr[0]["pusername"].ToString();
                    this.txtpuserid1.Text = dr[0]["puserid1"].ToString();
                    this.txtpuserkey1.Text = dr[0]["puserkey1"].ToString();
                    this.txtpuserid2.Text = dr[0]["puserid2"].ToString();
                    this.txtpuserkey2.Text = dr[0]["puserkey2"].ToString();
                    this.txtpostBankUrl.Text = dr[0]["postBankUrl"].ToString();
                    this.txtdesc.Text = dr[0]["desc"].ToString();
                    this.txtsort.Text = dr[0]["sort"].ToString();
                    this.txtJumpUrl.Text = dr[0]["jumpurl"].ToString();
                    this.txtdistributionUrl.Text = dr[0]["distributionUrl"].ToString();
                    this.txtExtParm1.Text = dr[0]["extparm1"].ToString();
                    this.txtExtParm2.Text = dr[0]["extparm2"].ToString();

                    string typelist = dr[0]["channeltypelistid"].ToString();
                    List<string> list = new List<string>(typelist.Split(','));
                    for (int i = 0; i < this.chkChannelType.Items.Count; i++)
                    {
                        string t = this.chkChannelType.Items[i].Value.ToString();
                        this.chkChannelType.Items[i].Selected = list.IndexOf(t) >= 0;
                    }
                }
            }
        }

        public void LoadInfo()
        {
            KuaiCard.Model.User.UserInfo userInfo = KuaiCard.BLL.User.UserFactory.GetModel(this.UserID);
            if (userInfo == null) return;

            this.txtUserID.Text = userInfo.ID.ToString();
            this.txtUserName.Text = userInfo.UserName;

            DataTable table2 = SysSupplierFactory.GetList(string.Empty).Tables[0];
            foreach (DataRow row in table2.Rows)
            {
                this.ddlsupp.Items.Add(new ListItem(row["name"].ToString() + "【" + row["code"].ToString() + "】", row["code"].ToString()));
            }
        }

        public void LoadList()
        {
            this.rptList.DataSource = KuaiCard.BLL.SysSupplierFactory.GetUserSupplierList(this.UserID);
            this.rptList.DataBind();
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                string id = e.CommandArgument.ToString();
                if (!string.IsNullOrEmpty(id))
                {
                    //删除服务商通道设置
                    KuaiCard.BLL.SysSupplierFactory.DeleteUserSupplierInfo(Convert.ToInt32(id));
                    base.AlertAndRedirect("删除成功！", "UserSupplierConfig.aspx?userid=" + this.UserID.ToString());
                }
            }
        }
    }
}

