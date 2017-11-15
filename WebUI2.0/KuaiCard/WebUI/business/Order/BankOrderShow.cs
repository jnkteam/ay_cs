namespace KuaiCard.WebUI.business.Order
{
    using KuaiCard.BLL;
    using KuaiCard.BLL.Channel;
    using KuaiCard.BLL.User;
    using KuaiCard.Model;
    using KuaiCard.Model.Order;
    using KuaiCard.WebComponents.Web;
    using KuaiCard.WebUI;
    using KuaiCardLib.TimeControl;
    using KuaiCardLib.Web;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class BankOrderShow : BusinessPageBase
    {
        protected HtmlForm form1;
        protected Label lbladdtime;
        protected Label lblattach;
        protected Label lblclientip;
        protected Label lblid;
        protected Label lblnotifycontext;
        protected Label lblnotifycount;
        protected Label lblnotifystat;
        protected Label lblorderid;
        protected Label lblordertype;
        protected Label lblpayAmt;
        protected Label lblpayerip;
        protected Label lblpaymodeId;
        protected Label lblpayRate;
        protected Label lblprofits;
        protected Label lblpromAmt;
        protected Label lblpromRate;
        protected Label lblrealvalue;
        protected Label lblrefervalue;
        protected Label lblstatus;
        protected Label lblsupplierAmt;
        protected Label lblsupplierId;
        protected Label lblsupplierOrder;
        protected Label lblsupplierRate;
        protected Label lbltypeId;
        protected Label lbluserid;
        protected Label lbluserorder;
        protected Label lblversion;

        private string getChannelName(string code)
        {
            try
            {
                return KuaiCard.BLL.Channel.Channel.GetModelByCode(code).modeName;
            }
            catch
            {
                return string.Empty;
            }
        }

        private string getChannelTypeName(int id)
        {
            try
            {
                return ChannelType.GetModelByTypeId(id).modetypename;
            }
            catch
            {
                return string.Empty;
            }
        }

        private string getuserName(int uid)
        {
            try
            {
                return UserFactory.GetCacheModel(uid).UserName;
            }
            catch
            {
                return string.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            if (!this.Page.IsPostBack)
            {
                this.ShowInfo(this.Id);
            }
        }

        private void setPower()
        {
            if (!ManageFactory.CheckCurrentPermission(false, ManageRole.Orders))
            {
                base.Response.Write("Sorry,No authority!");
                base.Response.End();
            }
        }

        private void ShowInfo(long id)
        {
            if (this.Id > 0L)
            {
                OrderBankInfo model = new OrderBank().GetModel(this.Id);
                if (model != null)
                {
                    if (base.currentManage.isSuperAdmin <= 0)
                    {
                    }
                    this.lblid.Text = this.Id.ToString();
                    this.lblorderid.Text = model.orderid;
                    this.lblordertype.Text = model.ordertype.ToString();
                    this.lbluserid.Text = model.userid.ToString() + " (" + this.getuserName(model.userid) + ")";
                    this.lbltypeId.Text = this.getChannelTypeName(model.typeId);
                    this.lblpaymodeId.Text = this.getChannelName(model.paymodeId);
                    this.lbluserorder.Text = model.userorder;
                    this.lblrefervalue.Text = model.refervalue.ToString("f2");
                    if (model.realvalue.HasValue)
                    {
                        this.lblrealvalue.Text = model.realvalue.Value.ToString("f2");
                    }
                    this.lblnotifycount.Text = model.notifycount.ToString();
                    this.lblnotifystat.Text = Enum.GetName(typeof(OrderNofityStatusEnum), model.notifystat);
                    this.lblnotifycontext.Text = base.Server.HtmlEncode(model.notifycontext);
                    this.lblattach.Text = model.attach;
                    this.lblpayerip.Text = model.payerip;
                    this.lblclientip.Text = model.clientip;
                    this.lbladdtime.Text = FormatConvertor.DateTimeToTimeString(model.addtime);
                    this.lblsupplierId.Text = WebUtility.GetsupplierName(model.supplierId);
                    this.lblsupplierOrder.Text = model.supplierOrder;
                    this.lblstatus.Text = Enum.GetName(typeof(OrderStatusEnum), model.status);
                    this.lblpayRate.Text = model.payRate.ToString("p2");
                    this.lblsupplierRate.Text = model.supplierRate.ToString("p2");
                    this.lblpromRate.Text = model.promRate.ToString("p2");
                    this.lblpayAmt.Text = model.payAmt.ToString("f2");
                    this.lblpromAmt.Text = model.promAmt.ToString("f2");
                    this.lblsupplierAmt.Text = model.supplierAmt.ToString("f2");
                    this.lblprofits.Text = model.profits.ToString("f2");
                    this.lblversion.Text = SystemApiHelper.GetVersionName(model.version);
                }
            }
        }

        public long Id
        {
            get
            {
                return WebBase.GetQueryStringInt64("id", 0L);
            }
        }
    }
}

