namespace OriginalStudio.WebUI.Manage.User
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
    using OriginalStudio.BLL.Supplier;
    using OriginalStudio.BLL.Settled;
    using OriginalStudio.Model.User;

    public class UserSupplierEdit : ManagePageBase
    {
        
        protected Button btnSave;
        protected DropDownList UserID; //商户
        protected DropDownList SupplierCode;//通道
        protected HtmlForm form1;
        protected RadioButtonList IsSingle; //是否开启
        protected RadioButtonList IsTransfer;//是否跳转
        protected TextBox PUserID; //账号
        protected TextBox PUserKey;//秘钥
        protected TextBox PUserName;//用户名
        protected TextBox PUserParm1; //参数1
        protected TextBox PUserParm2; //参数2
        protected TextBox TransferUrl; //url


        protected void btnSave_Click(object sender, EventArgs e)
        {
            MchUserSupplier mchUserSupplier = new MchUserSupplier();
            mchUserSupplier.UserID          = Convert.ToInt32(this.UserID.SelectedValue);
            mchUserSupplier.SupplierCode    = Convert.ToInt32(this.SupplierCode.SelectedValue);
            mchUserSupplier.IsSingle        = Convert.ToInt32(this.IsSingle.SelectedValue);
            mchUserSupplier.IsTransfer      = Convert.ToInt32(this.IsTransfer.SelectedValue);
            mchUserSupplier.PUserID         = this.PUserID.Text;
            mchUserSupplier.PUserKey        = this.PUserKey.Text;
            mchUserSupplier.PUserName       = this.PUserName.Text;      
            mchUserSupplier.PUserParm1      = this.PUserParm1.Text;
            mchUserSupplier.PUserParm2      = this.PUserParm2.Text;
            mchUserSupplier.TransferUrl     = this.TransferUrl.Text;






            if (!this.isUpdate)
            {
                if (MchUserSupplierFactory.EditUserSupplier(mchUserSupplier) > 0)
                {
                    showPageMsg("保存成功！");
                }
                else
                {
                    showPageMsg("保存失败！");
                }
            }
            else if (MchUserSupplierFactory.EditUserSupplier(mchUserSupplier) > 0)
            {
                showPageMsg("更新成功！");
            }
            else
            {
                showPageMsg("更新失败！");
            }
        }

        

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            //ManageFactory.CheckSecondPwd();
            if (!base.IsPostBack)
            {
               
                DataTable table = MchUserFactory.GetAllUserList();
                foreach (DataRow row in table.Rows)
                {
                    this.UserID.Items.Add(new ListItem(row["username"].ToString(), row["userid"].ToString()));
                }
                DataTable table2 = SysSupplierFactory.GetList(string.Empty).Tables[0];
                
                foreach (DataRow row in table2.Rows)
                {
                    this.SupplierCode.Items.Add(new ListItem(row["SupplierName"].ToString(), row["SupplierCode"].ToString()));
                }

                this.ShowInfo();
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
            if (this.isUpdate )
            {
                DataTable infoTable = MchUserSupplierFactory.GetUserSupplier(this.ItemInfoId).Tables[0];
                foreach (DataRow dr in infoTable.Rows) {
                   
                    this.UserID.SelectedValue = dr["userid"].ToString();
                    this.UserID.Enabled = false;
                    this.SupplierCode.SelectedValue = dr["SupplierCode"].ToString();
                    this.IsSingle.SelectedValue  =  dr["IsSingle"].ToString();
                    this.IsTransfer.SelectedValue = dr["IsTransfer"].ToString();
                    this.PUserID.Text = dr["PUserID"].ToString();
                    this.PUserKey.Text = dr["PUserKey"].ToString();
                    this.PUserName.Text = dr["PUserName"].ToString();
                    this.PUserParm1.Text = dr["PUserParm1"].ToString();
                    this.PUserParm2.Text = dr["PUserParm2"].ToString();
                    this.TransferUrl.Text = dr["TransferUrl"].ToString();




                }
                    
            }
            //this.ddlTypeSupp.Enabled = false;
            //this.rblTypeOpen.Enabled = false;
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

        
    }
}

