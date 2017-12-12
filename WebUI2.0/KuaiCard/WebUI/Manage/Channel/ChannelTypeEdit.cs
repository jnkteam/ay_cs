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
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using OriginalStudio.BLL.Supplier;

    public class ChannelTypeEdit : ManagePageBase
    {
        public SysChannelTypeInfo _ItemInfo = null;
        protected Button btnSave;
        protected CheckBoxList cblSupplier;
        protected DropDownList ddlOpen;
        protected DropDownList ddlSupplier;
        protected HtmlForm form1;
        protected HtmlHead Head1;
        protected RadioButtonList rblRelease;
        protected RadioButtonList rblrunmode;
        protected RadioButtonList rblType;
        protected Repeater rptsupp;
        protected HtmlTableRow tr_runmode_0;
        protected HtmlTableRow tr_runmode_1;
        protected TextBox txtCode;
        protected TextBox txtmodetypename;
        protected TextBox txtsort;
        protected TextBox txttypeId;

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string text = this.txtmodetypename.Text;
            int num = int.Parse(this.txttypeId.Text);
            int result = 0;
            int.TryParse(this.ddlOpen.SelectedValue, out result);
            int num3 = 0;
            int.TryParse(this.ddlSupplier.SelectedValue, out num3);
            int num4 = int.Parse(this.txtsort.Text);
            bool flag = this.rblRelease.SelectedValue == "1";
            int num5 = Convert.ToInt32(this.rblType.SelectedValue);
            if (!this.isUpdate)
            {
                this.ItemInfo.TypeName = text;
                this.ItemInfo.TypeID = num;
                this.ItemInfo.TypeClassID = num5;
                this.ItemInfo.AddTime = DateTime.Now;
            }
            this.ItemInfo.IsOpen = (SysChannelTypeOpenEnum)result;
            this.ItemInfo.SupplierCode = num3;
            this.ItemInfo.ListOrder = num4;
            this.ItemInfo.Release = flag;
            this.ItemInfo.RunMode = int.Parse(this.rblrunmode.SelectedValue);
            StringBuilder builder = new StringBuilder();
            int num6 = 1;
            foreach (RepeaterItem item in this.rptsupp.Items)
            {
                if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
                {
                    HtmlInputCheckBox box = item.FindControl("chkItem") as HtmlInputCheckBox;
                    HiddenField field = item.FindControl("hfsuppid") as HiddenField;
                    TextBox box2 = item.FindControl("txtweight") as TextBox;
                    if ((((box != null) && (field != null)) && (box2 != null)) && box.Checked)
                    {
                        int num7 = 1;
                        try
                        {
                            num7 = Convert.ToInt32(box2.Text);
                        }
                        catch
                        {
                            num7 = 1;
                        }
                        if (num6 > 1)
                        {
                            builder.AppendFormat("|{0}:{1}", field.Value, num7);
                        }
                        else
                        {
                            builder.AppendFormat("{0}:{1}", field.Value, num7);
                        }
                        num6++;
                    }
                }
            }
            this.ItemInfo.RunModeSet = builder.ToString();
            if (!this.isUpdate)
            {
                if (SysChannelType.Add(this.ItemInfo) > 0)
                {
                    showPageMsg("保存成功！");
                }
                else
                {
                    showPageMsg("保存失败！");
                }
            }
            else if (SysChannelType.Update(this.ItemInfo))
            {
                showPageMsg("更新成功！");
            }
            else
            {
                showPageMsg("更新失败！");
            }
        }

        private void LoadData()
        {
            
            DataTable table = SysSupplierFactory.GetList(null).Tables[0];
            table.Columns.Add("weight", typeof(int));
            foreach (DataRow row in table.Rows)
            {
                row["weight"] = "0";
            }
            this.rptsupp.DataSource = table;
            this.rptsupp.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
            this.setPower();
            if (!base.IsPostBack)
            {
                this.txtmodetypename.Enabled = false;
                this.txttypeId.Enabled = false;
                this.rblType.Enabled = false;
                this.txtCode.Enabled = false;
                this.ShowInfo();
                this.LoadData();
            }
        }

        protected void rblrunmode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.rblrunmode.SelectedValue == "1")
            {
                this.tr_runmode_1.Visible = true;
                this.tr_runmode_0.Visible = false;
            }
            else
            {
                this.tr_runmode_1.Visible = false;
                this.tr_runmode_0.Visible = true;
            }
        }

        protected void rptsupp_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
        }

        protected void rptsupp_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (((this.ItemInfo != null) && (this.ItemInfo.RunMode == 1)) && ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem)))
            {
                HtmlInputCheckBox box = e.Item.FindControl("chkItem") as HtmlInputCheckBox;
                TextBox box2 = e.Item.FindControl("txtweight") as TextBox;
                string str = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "SupplierCode"));
                if (!string.IsNullOrEmpty(this.ItemInfo.RunModeSet))
                {
                    foreach (string str2 in this.ItemInfo.RunModeSet.Split(new char[] { '|' }))
                    {
                        string[] strArray = str2.Split(new char[] { ':' });
                        if (strArray[0] == str)
                        {
                            box.Checked = true;
                            box2.Text = strArray[1];
                            break;
                        }
                    }
                }
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

           
            if (this.isUpdate && (this.ItemInfo != null))
            {
               
                this.rblType.SelectedValue = ((int) this.ItemInfo.TypeClassID).ToString();
                this.txtmodetypename.Text = this.ItemInfo.TypeName;
                this.txttypeId.Text = this.ItemInfo.TypeID.ToString();
                this.txtCode.Text = this.ItemInfo.TypeCode;
                this.ddlOpen.SelectedValue = ((int) this.ItemInfo.IsOpen).ToString();
                this.txtsort.Text = this.ItemInfo.ListOrder.ToString();
                this.rblRelease.SelectedValue = this.ItemInfo.Release ? "1" : "0";
                this.rblrunmode.SelectedValue = this.ItemInfo.RunMode.ToString();
                this.tr_runmode_1.Visible = this.ItemInfo.RunMode == 1;
                this.tr_runmode_0.Visible = this.ItemInfo.RunMode == 0;
                string where = string.Empty;


                /*
                if (this.ItemInfo.Class == ChannelClassEnum.网银)
                {
                    where = where + " and IsBank=1";
                }
                else if (this.ItemInfo.Class == ChannelClassEnum.支付宝)
                {
                    where = where + "and IsAlipay=1";
                }
                else if (this.ItemInfo.Class == ChannelClassEnum.微信)
                {
                    where = where + "and IsWeiXin=1";
                }
                else if (this.ItemInfo.Class == ChannelClassEnum.QQ)
                {
                    where = where + " and IsQQ=1";
                }
                else if (this.ItemInfo.Class == ChannelClassEnum.京东)
                {
                    where = where + " and IsJD=1";
                }
                */




                DataTable table = SysSupplierFactory.GetList(where).Tables[0];
                this.ddlSupplier.Items.Add(new ListItem("--请选择--", ""));
                foreach (DataRow row in table.Rows)
                {
                    this.ddlSupplier.Items.Add(new ListItem(row["SupplierName"].ToString(), row["SupplierCode"].ToString()));
                }
                this.ddlSupplier.SelectedValue = this.ItemInfo.SupplierCode.ToString();

               
            }
        }

        public bool isUpdate
        {
            get
            {
                return (this.ItemInfoId > 0);
            }
        }

        public SysChannelTypeInfo ItemInfo
        {
            get
            {
                if (this._ItemInfo == null)
                {
                    if (this.ItemInfoId > 0)
                    {
                        this._ItemInfo = SysChannelType.GetModel(this.ItemInfoId);
                    }
                    else
                    {
                        this._ItemInfo = new SysChannelTypeInfo();
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

