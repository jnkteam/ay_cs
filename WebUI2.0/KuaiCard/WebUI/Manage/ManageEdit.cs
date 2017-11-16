namespace OriginalStudio.WebUI.Manage
{
    using OriginalStudio.BLL;
    using OriginalStudio.Model;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Security;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ManageEdit : ManagePageBase
    {
        public OriginalStudio.Model.Manage _ItemInfo = null;
        protected Button btnAdd;
        protected CheckBoxList cbl_roles;
        protected CheckBox ckb_Agent;
        protected CheckBox ckb_SuperAdmin;
        protected DropDownList ddlCommissionType;
        protected DropDownList ddlStus;
        protected HtmlForm form1;
        protected HiddenField hf_isupdate;
        protected TextBox lblbalance;
        protected Label lbllastloginip;
        protected Label lbllastlogintime;
        protected TextBox qq;
        protected TextBox tel;
        protected TextBox txtCardCommission;
        protected TextBox txtCommission;
        protected TextBox txtpassword;
        protected TextBox txtrelname;
        protected TextBox txtsecondpwd;
        protected TextBox txtusername;

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string str = this.txtusername.Text.Trim();
            string str2 = this.txtpassword.Text.Trim();
            string str3 = this.txtsecondpwd.Text.Trim();
            string str4 = this.txtrelname.Text.Trim();
            string str5 = this.qq.Text.Trim();
            string str6 = this.tel.Text.Trim();
            ManageRole none = ManageRole.None;
            foreach (ListItem item in this.cbl_roles.Items)
            {
                if (item.Selected)
                {
                    if (none == ManageRole.None)
                    {
                        none = (ManageRole) Convert.ToInt32(item.Value);
                    }
                    else
                    {
                        //none |= Convert.ToInt32(item.Value);
                        none |= (ManageRole)Convert.ToInt32(item.Value);
                    }
                }
            }
            this.ItemInfo.username = str;
            if (!string.IsNullOrEmpty(str2))
            {
                this.ItemInfo.password = Cryptography.MD5(str2);
            }
            if (!string.IsNullOrEmpty(str3))
            {
                this.ItemInfo.secondpwd = Cryptography.MD5(str3);
            }
            this.ItemInfo.relname = str4;
            this.ItemInfo.role = none;
            this.ItemInfo.commissiontype = new int?(int.Parse(this.ddlCommissionType.SelectedValue));
            decimal result = 0M;
            if (!decimal.TryParse(this.txtCommission.Text.Trim(), out result))
            {
                base.AlertAndRedirect("请输入数字");
            }
            else
            {
                decimal num2 = 0M;
                if (!decimal.TryParse(this.txtCardCommission.Text.Trim(), out num2))
                {
                    base.AlertAndRedirect("请输入数字");
                }
                else
                {
                    this.ItemInfo.cardcommission = new decimal?(num2);
                    this.ItemInfo.commission = new decimal?(result);
                    this.ItemInfo.status = new int?(int.Parse(this.ddlStus.SelectedValue));
                    this.ItemInfo.isSuperAdmin = this.ckb_SuperAdmin.Checked ? 1 : 0;
                    this.ItemInfo.isAgent = this.ckb_Agent.Checked ? 1 : 0;
                    this.ItemInfo.qq = str5;
                    this.ItemInfo.tel = str6;
                    bool flag = false;
                    if (this.isUpdate)
                    {
                        if (ManageFactory.Update(this.ItemInfo))
                        {
                            flag = true;
                        }
                    }
                    else if (ManageFactory.Add(this.ItemInfo) > 0)
                    {
                        flag = true;
                    }
                    if (flag)
                    {
                        base.AlertAndRedirect("操作成功", "Manage.aspx");
                    }
                    else
                    {
                        base.AlertAndRedirect("操作失败");
                    }
                }
            }
        }

        private void InitForm()
        {
            foreach (ManageRole role in Enum.GetValues(typeof(ManageRole)))
            {
                if (role != ManageRole.None)
                {
                    int num = (int) role;
                    ListItem item = new ListItem(ManageFactory.GetManageRoleView(role), num.ToString());
                    if ((this.ItemInfo.role & role) == role)
                    {
                        item.Selected = true;
                    }
                    this.cbl_roles.Items.Add(item);
                }
            }
            if (this.isUpdate)
            {
                this.hf_isupdate.Value = "1";
                this.txtusername.Text = this.ItemInfo.username;
                this.txtrelname.Text = this.ItemInfo.relname;
                this.ddlCommissionType.SelectedValue = this.ItemInfo.commissiontype.ToString();
                this.ddlStus.SelectedValue = this.ItemInfo.status.ToString();
                this.lbllastloginip.Text = this.ItemInfo.lastLoginIp;
                this.qq.Text = this.ItemInfo.qq;
                this.tel.Text = this.ItemInfo.tel;
                if (this.ItemInfo.lastLoginTime.HasValue)
                {
                    this.lbllastlogintime.Text = this.ItemInfo.lastLoginTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (this.ItemInfo.commission.HasValue)
                {
                    this.txtCommission.Text = this.ItemInfo.commission.Value.ToString("f4");
                }
                if (this.ItemInfo.cardcommission.HasValue)
                {
                    this.txtCardCommission.Text = this.ItemInfo.cardcommission.Value.ToString("f4");
                }
                if (this.ItemInfo.balance.HasValue)
                {
                    this.lblbalance.Text = this.ItemInfo.balance.Value.ToString("f2");
                }
                this.ckb_SuperAdmin.Checked = this.ItemInfo.isSuperAdmin > 0;
                this.ckb_Agent.Checked = this.ItemInfo.isAgent > 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            if (!base.IsPostBack)
            {
                this.InitForm();
            }
        }

        private void setPower()
        {
            if (!ManageFactory.CheckCurrentPermission(true, ManageRole.None))
            {
                base.Response.Write("Sorry,No authority!");
                base.Response.End();
            }
        }

        public string Action
        {
            get
            {
                return WebBase.GetQueryStringString("cmd", "");
            }
        }

        public bool isUpdate
        {
            get
            {
                return ((this.ItemInfoId > 0) && (this.Action == "edit"));
            }
        }

        public OriginalStudio.Model.Manage ItemInfo
        {
            get
            {
                if (this._ItemInfo == null)
                {
                    if (this.isUpdate)
                    {
                        this._ItemInfo = ManageFactory.GetModel(this.ItemInfoId);
                    }
                    else
                    {
                        this._ItemInfo = new OriginalStudio.Model.Manage();
                    }
                }
                return this._ItemInfo;
            }
        }

        public int ItemInfoId
        {
            get
            {
                return WebBase.GetQueryStringInt32("id", 0);
            }
        }
    }
}

