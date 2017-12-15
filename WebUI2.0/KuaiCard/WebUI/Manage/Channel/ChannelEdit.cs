namespace OriginalStudio.WebUI.Manage.Channel
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

    public class ChannelEdit : ManagePageBase
    {
        public SysChannelInfo _ItemInfo = null;
        public SysChannelTypeInfo _typeInfo = null;
        protected Button btnSave;
        protected DropDownList ddlType;
        protected DropDownList ddlTypeSupp;
        protected HtmlForm form1;
        protected RadioButtonList rblOpen;
        protected RadioButtonList rblTypeOpen;
        protected TextBox txtcode;
        protected TextBox txtenmodeName;
        protected TextBox txtfaceValue;
        protected TextBox txtmodeName;
        protected TextBox txtsort;

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string text = this.txtcode.Text;
            int result = -1;
            int.TryParse(this.rblOpen.SelectedValue, out result);
            int num2 = -1;
            int.TryParse(this.ddlTypeSupp.SelectedValue, out num2);
            string str2 = this.txtmodeName.Text;
            
            int num4 = 0;
            int.TryParse(this.ddlType.SelectedValue, out num4);
            int num5 = int.Parse(this.txtsort.Text);
            this.model.ChannelCode = text;
            this.model.ChannelTypeId = num4;
            this.model.ChannelName = str2;
          
            this.model.IsOpen = result == 1 ? true : false ;
                     
            this.model.SupplierCode = num2;
            
            this.model.AddTime = DateTime.Now;
            this.model.ListSort = num5;
            this.model.ChannelEnName = this.txtenmodeName.Text;
            string url = "ChannelList.aspx";
            if (this.Session["selecttype"] != null)
            {
                url = url + "?typeid=" + this.Session["selecttype"].ToString();
            }
            if (!this.isUpdate)
            {
                if (OriginalStudio.BLL.Channel.SysChannel.Add(this.model) > 0)
                {
                    showPageMsg("保存成功！");
                }
                else
                {
                    showPageMsg("保存失败！");
                }
            }
            else if (OriginalStudio.BLL.Channel.SysChannel.Update(this.model))
            {
                showPageMsg("更新成功！");
            }
            else
            {
                showPageMsg("更新失败！");
            }
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlTypeSupp.SelectedValue = "";
            int result = 0;
            int.TryParse(this.ddlType.SelectedValue, out result);
            if (result > 0)
            {
                SysChannelTypeInfo modelByTypeId = SysChannelType.GetModelByTypeId(result);
                if (modelByTypeId != null)
                {
                    this.ddlTypeSupp.SelectedValue = modelByTypeId.SupplierCode.ToString();
                    //this.rblTypeOpen.SelectedValue = ((int) modelByTypeId.IsOpen).ToString();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            //ManageFactory.CheckSecondPwd();
            if (!base.IsPostBack)
            {
                this.ddlType.Items.Add(new ListItem("---全部类别---", ""));
                DataTable table = SysChannelType.GetList(null).Tables[0];
                foreach (DataRow row in table.Rows)
                {
                    this.ddlType.Items.Add(new ListItem(row["TypeName"].ToString(), row["typeId"].ToString()));
                }
                DataTable table2 = SysSupplierFactory.GetList(string.Empty).Tables[0];
                this.ddlTypeSupp.Items.Add(new ListItem("--请选择--", ""));
                foreach (DataRow row in table2.Rows)
                {
                    this.ddlTypeSupp.Items.Add(new ListItem(row["SupplierName"].ToString(), row["SupplierCode"].ToString()));
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
            if (this.isUpdate && (this.model != null) && (this.model.ID > 0))
            {
                this.txtcode.Text = this.model.ChannelCode;
                this.ddlType.SelectedValue = this.model.ChannelTypeId.ToString();
                this.txtmodeName.Text = this.model.ChannelName;

                this.txtsort.Text = this.model.ListSort.ToString();
                this.txtenmodeName.Text = this.model.ChannelEnName;
                if (this.model.SupplierCode>0)
                    this.ddlTypeSupp.SelectedValue = this.model.SupplierCode.ToString();

                if (this.model.IsOpen)
                    this.rblOpen.SelectedValue = this.model.IsOpen ? "1" : "0" ;
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

        public SysChannelInfo model
        {
            get
            {
                if (this._ItemInfo == null)
                {
                    if (this.ItemInfoId > 0)
                    {
                        this._ItemInfo = OriginalStudio.BLL.Channel.SysChannel.GetChannelModelByID(this.ItemInfoId);
                    }
                    else
                    {
                        this._ItemInfo = new SysChannelInfo();
                    }
                }
                return this._ItemInfo;
            }
        }

        public SysChannelTypeInfo typeInfo
        {
            get
            {
                if (this._typeInfo == null)
                {
                    if (this.model != null)
                    {
                        this._typeInfo = SysChannelType.GetModelByTypeId(this.model.ChannelTypeId);
                    }
                    else
                    {
                        this._typeInfo = new SysChannelTypeInfo();
                    }
                }
                return this._typeInfo;
            }
        }
    }
}

