namespace OriginalStudio.WebUI.Manage.Supplier
{
    using OriginalStudio.BLL;
    using OriginalStudio.Model;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Text;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Data;
    using OriginalStudio.BLL.Channel;

    public class SupplierPayrate : ManagePageBase
    {
        public SysSupplierPayRateInfo _ItemInfo = null;
        protected Button btnSave;
        protected HtmlForm form1;
        protected HiddenField hideValue;
        protected Label lblName;
        protected Repeater PayRateRepeater;

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SysSupplierPayRateInfo model = new SysSupplierPayRateInfo();
            model.SupplierCode = this.SupplierId;
          
            model.PayrateXML = this.hideValue.Value;

            if (SysSupplierPayRateFactory.Edit(model) > 0)
                {
                    base.AlertAndRedirect("保存成功！");
                }
                else
                {
                    base.AlertAndRedirect("保存失败！");
                }
                
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            if (!base.IsPostBack)
            {
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
           
            
            if (this.ItemInfo != null)
            {
               
                
                DataSet set = SysChannelType.GetList(true);
                DataTable table =  set.Tables[0];
                table.Columns.Add("rateValue");

                SysSupplierPayRateInfo myRateInfo = SysSupplierPayRateFactory.GetModel(this.SupplierId);
                
                foreach (DataRow row in table.Rows)
                {
                    
                        decimal speRate  = SysSupplierPayRateFactory.GetSupplierChannelTypeRate(this.SupplierId, Convert.ToInt32(row["typeid"]));

                        row["rateValue"] = speRate >= 0 ? speRate * 100 : 0;
                    
                    
                }
                this.PayRateRepeater.DataSource = table;
                this.PayRateRepeater.DataBind();
            
            }
            
        }

        public SysSupplierPayRateInfo ItemInfo
        {
            get
            {
                if (this._ItemInfo == null)
                {
                    if (this.SupplierId > 0)
                    {
                        this._ItemInfo = SysSupplierPayRateFactory.GetModel(this.SupplierId);
                    }
                    if (this._ItemInfo == null)
                    {
                        this._ItemInfo = new SysSupplierPayRateInfo();
                    }
                }
                return this._ItemInfo;
            }
        }

        public int SupplierId
        {
            get
            {
                return WebBase.GetQueryStringInt32("supid", 0);
            }
        }

        public string SupplierName
        {
            get
            {
                return WebBase.GetQueryStringString("n", string.Empty);
            }
        }
    }
}

