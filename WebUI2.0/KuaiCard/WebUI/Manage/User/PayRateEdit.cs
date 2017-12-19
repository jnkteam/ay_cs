namespace OriginalStudio.WebUI.Manage.User
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Payment;
    using OriginalStudio.Model;
    using OriginalStudio.Model.PayRate;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Text;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using OriginalStudio.BLL.PayRate;
    using System.Data;
    using OriginalStudio.BLL.Channel;

    public class PayRateEdit : ManagePageBase
    {
        public SysPayRateInfo _model = null;
        protected Button btnSave;
        protected HtmlForm form1;
        protected RadioButtonList rbl_type;
        protected Repeater PayRateRepeater;
        protected HiddenField hideValue;
        protected TextBox ratename;
       
        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool flag = false;
            this.model.RateName = this.ratename.Text.Trim();
            this.model.PayrateXML = this.hideValue.Value;
            this.model.RateType = (RateTypeEnum)Convert.ToInt32(this.rbl_type.SelectedValue);
            


            if (this.isUpdate)
            {
                this.model.ID = this.ItemInfoId;
                if (SysPayRateFactory.Update(this.model))
                {
                    flag = true;
                }
            }
            else if (SysPayRateFactory.Add(this.model) > 0)
            {
                
                flag = true;
            }
            if (flag)
            {
                showPageMsg("操作成功");
            }
            else
            {
                base.AlertAndRedirect("操作失败");
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
            this.setPower();
            if (!base.IsPostBack)
            {
                this.ShowInfo();
                this.rbl_type.Enabled = !this.isUpdate;
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
            DataSet set = SysChannelType.GetList(true);
            DataTable table = set.Tables[0];
            table.Columns.Add("rateValue");
            if (this.isUpdate && (this.model != null))
            {
                this.ratename.Text = this.model.RateName;
                SysPayRateInfo sysPayRate = SysPayRateFactory.GetModel(this.ItemInfoId);
                foreach (DataRow row in table.Rows)
                {

                    decimal speRate = SysPayRateFactory.GetSysChannelTypePayRate(this.ItemInfoId, Convert.ToInt32(row["typeid"]));

                    row["rateValue"] = speRate >= 0 ? speRate * 100 : 0;


                }
            }
            this.PayRateRepeater.DataSource = table;
            this.PayRateRepeater.DataBind();
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

        public SysPayRateInfo model
        {
            get
            {
                if (this._model == null)
                {
                    if (this.isUpdate)
                    {
                        this._model = SysPayRateFactory.GetModel(this.ItemInfoId);
                    }
                    else
                    {
                        this._model = new SysPayRateInfo();
                    }
                }
                return this._model;
            }
        }
    }
}

