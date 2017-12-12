namespace OriginalStudio.WebUI.Manage.Supplier
{
    using OriginalStudio.BLL;
    using OriginalStudio.Model;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using OriginalStudio.BLL.Supplier;
    using OriginalStudio.Model.Supplier;

    public class SupplierEdit : ManagePageBase
    {
        public SysSupplierInfo _ItemInfo = null;
        protected Button btnSave;
        protected CheckBox chkiali;
        protected CheckBox chkisbank;
        protected CheckBox chkiscard;
        protected CheckBox chkisdistribution;
        protected CheckBox chkissms;
        protected CheckBox chkissx;
        protected CheckBox chkissys;
        protected CheckBox chkiwap;
        protected CheckBox chkiwx;
        protected HtmlForm form1;
        protected TextBox txtCardbakUrl;
        protected TextBox txtcode;
       
        protected TextBox txtdistributionUrl;
        protected TextBox txtJumpUrl;
        protected TextBox txtlogourl;
        protected TextBox txtname;
        protected TextBox txtpbakurl;
        protected TextBox txtpostBankUrl;
        protected TextBox txtpostCardUrl;
        protected TextBox txtpostSMSUrl;
        protected TextBox txtpurl;
        protected TextBox txtpuserid;
        protected TextBox PUserParm1;
        protected TextBox PUserParm2;
        protected TextBox PUserParm3;
        protected TextBox PUserParm4;
        protected RadioButtonList Active;
        protected RadioButtonList IsDebug;
        protected TextBox txtpuserkey;

        protected TextBox BankPostUrl;// 网关地址
        protected TextBox BankReturnUrl;// 异步通知地址
        protected TextBox BankNotifyUrl;// 同步返回地址
        protected TextBox BankSearchUrl;// 查单接口地址
        protected TextBox BankJumUrl;// 中转域名地址
        protected TextBox DistributionUrl;// 代付提交地址
        protected TextBox DistributionNotifyUrl;// 代付通知地址
        protected TextBox DistributionSearchUrl;//代付查询地址
        
        protected TextBox txtdesc;//说明
        protected TextBox txtsort;//排序

        protected TextBox txtpusername;
        protected TextBox txtQueryCardUrl;
        

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int num = int.Parse(this.txtcode.Text);
            string text = this.txtname.Text;
            bool flag5 = this.chkiwap.Checked; 
            bool flag6 = this.chkiali.Checked;
            bool flag7 = this.chkiwx.Checked;
            bool flag8 = this.chkisdistribution.Checked;
            string str5 = this.txtpuserid.Text;
            string str6 = this.txtpuserkey.Text;
            string str7 = this.txtpusername.Text;


            this.ItemInfo.SupplierCode = num;
            this.ItemInfo.SupplierName = text;

           
            this.ItemInfo.IsBank = flag5;
            this.ItemInfo.IsAlipay = flag6;
            this.ItemInfo.IsWeiXin = flag7;
            this.ItemInfo.IsDistribution = flag8;
            this.ItemInfo.PUserID = str5;
            this.ItemInfo.PUserKey = str6;
            this.ItemInfo.PUserName = str7;
            this.ItemInfo.PUserParm1 = this.PUserParm1.Text; //预留参数 1-4
            this.ItemInfo.PUserParm2 = this.PUserParm2.Text; //预留参数 1-4
            this.ItemInfo.PUserParm3 = this.PUserParm3.Text; //预留参数 1-4
            this.ItemInfo.PUserParm4 = this.PUserParm4.Text; //预留参数 1-4

            this.ItemInfo.Active = this.Active.SelectedValue == "1" ? true : false; //启停标记
            this.ItemInfo.IsDebug = this.IsDebug.SelectedValue == "1" ? true : false; //调试


            this.ItemInfo.BankPostUrl = this.BankPostUrl.Text;// 网关地址
            this.ItemInfo.BankReturnUrl = this.BankReturnUrl.Text;// 异步通知地址
            this.ItemInfo.BankNotifyUrl = this.BankNotifyUrl.Text;// 同步返回地址
            this.ItemInfo.BankSearchUrl = this.BankSearchUrl.Text;// 查单接口地址
            this.ItemInfo.BankJumUrl = this.BankJumUrl.Text;// 中转域名地址
            this.ItemInfo.DistributionUrl = this.DistributionUrl.Text;// 代付提交地址
            this.ItemInfo.DistributionNotifyUrl = this.DistributionNotifyUrl.Text;// 代付通知地址
            this.ItemInfo.DistributionSearchUrl = this.DistributionSearchUrl.Text;//代付查询地址

            this.ItemInfo.SpDesc = this.txtdesc.Text;
            this.ItemInfo.ListOrder = int.Parse(this.txtsort.Text);

            //this.ItemInfo.IsDistribution = flag8;
            //==================================

            if (!this.isUpdate)
            {
                if (SysSupplierFactory.Add(this.ItemInfo) > 0)
                {
                    showPageMsg("保存成功！");
                }
                else
                {
                    showPageMsg("保存失败！");
                }
            }
            else if (SysSupplierFactory.Update(this.ItemInfo))
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
            //ManageFactory.CheckSecondPwd();
            //this.setPower();
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
            if (this.isUpdate && (this.ItemInfo != null))
            {
                this.txtcode.Text = this.ItemInfo.SupplierCode.ToString(); //供应商编号
                this.txtname.Text = this.ItemInfo.SupplierName;  //供应商通道名称
                //this.txtlogourl.Text = this.ItemInfo.LogoUrl;  //logo图片
               
                this.chkiwap.Checked = this.ItemInfo.IsBank;     //支持网银
                this.chkiali.Checked = this.ItemInfo.IsAlipay;//支持支付宝
                this.chkiwx.Checked = this.ItemInfo.IsWeiXin;//支持微信
                this.chkisdistribution.Checked = this.ItemInfo.IsDistribution;//支持代付
                
                this.txtpuserid.Text = this.ItemInfo.PUserID;  //账号
                this.txtpuserkey.Text = this.ItemInfo.PUserKey; //秘钥
                this.txtpusername.Text = this.ItemInfo.PUserName; //商户名称

                this.PUserParm1.Text = this.ItemInfo.PUserParm1; //预留参数 1-4
                this.PUserParm2.Text = this.ItemInfo.PUserParm2; //预留参数 1-4
                this.PUserParm3.Text = this.ItemInfo.PUserParm3; //预留参数 1-4
                this.PUserParm4.Text = this.ItemInfo.PUserParm4; //预留参数 1-4

                this.Active.SelectedValue = this.ItemInfo.Active ? "1" : "0"; //启停标记
                this.IsDebug.SelectedValue = this.ItemInfo.IsDebug ? "1" : "0"; //调试


                this.BankPostUrl.Text = this.ItemInfo.BankPostUrl;// 网关地址
                this.BankReturnUrl.Text = this.ItemInfo.BankReturnUrl;// 异步通知地址
                this.BankNotifyUrl.Text = this.ItemInfo.BankNotifyUrl;// 同步返回地址
                this.BankSearchUrl.Text = this.ItemInfo.BankSearchUrl;// 查单接口地址
                this.BankJumUrl.Text = this.ItemInfo.BankJumUrl;// 中转域名地址
                this.DistributionUrl.Text = this.ItemInfo.DistributionUrl;// 代付提交地址
                this.DistributionNotifyUrl.Text = this.ItemInfo.DistributionNotifyUrl;// 代付通知地址
                this.DistributionSearchUrl.Text = this.ItemInfo.DistributionSearchUrl;//代付查询地址

                this.txtdesc.Text = this.ItemInfo.SpDesc;
                this.txtsort.Text = this.ItemInfo.ListOrder.ToString();
            }
        }

        public bool isUpdate
        {
            get
            {
                return (this.ItemInfoId > 0);
            }
        }

        public SysSupplierInfo ItemInfo
        {
            get
            {
                if (this._ItemInfo == null)
                {
                    if (this.ItemInfoId > 0)
                    {
                        this._ItemInfo = SysSupplierFactory.GetSupplierModelById(this.ItemInfoId); ; 
                    }
                    else
                    {
                        this._ItemInfo = new SysSupplierInfo();
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

