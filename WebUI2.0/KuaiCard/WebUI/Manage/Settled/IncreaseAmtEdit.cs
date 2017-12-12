namespace OriginalStudio.WebUI.Manage.Settled
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model;
    using OriginalStudio.Model.Settled;
    using OriginalStudio.Model.User;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class IncreaseAmtEdit : ManagePageBase
    {
        public IncreaseAmtInfo _ItemInfo = null;
        protected Button btnAdd;
        protected CustomValidator CustomValidator1;
        protected HtmlForm form1;
        protected Label lblbalance;
        protected RadioButtonList rbl_optype;
        protected TextBox txtdesc;
        protected TextBox txtincreaseAmt;
        protected TextBox txtMerchantName;

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (String.IsNullOrEmpty(this.txtMerchantName.Text.Trim())) return;

            int result = 0;
            MchUserBaseInfo m = MchUserFactory.GetUserBaseByMerchantName(this.txtMerchantName.Text.Trim());
            if (m.UserID == 0)
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.MchName != "")
            {
                string s = string.Empty;
                MchUsersAmtInfo model = MchUserFactory.GetUserBaseByMerchantName(this.MchName).MchUsersAmtInfo;
                if (model == null)
                {
                    s = "用户不存在!";
                }
                else
                {
                    s = (model.Balance - model.Freeze - model.UnPayment).ToString("f2");
                }
                base.Response.Write(s);
                base.Response.End();
            }

            //ManageFactory.CheckCurrentPermission();
            //this.setPower();
            if (!base.IsPostBack)
            {
                this.ShowInfo();
            }
        }

        private void Save()
        {
            if (base.IsValid)
            {
                int result = 0;
                if (String.IsNullOrEmpty(this.txtMerchantName.Text.Trim()))
                {
                    base.AlertAndRedirect("请输入正确的用户ID");
                }
                else
                {
                    decimal increaseAmt = 0M;
                    if (!decimal.TryParse(this.txtincreaseAmt.Text, out increaseAmt))
                    {
                        base.AlertAndRedirect("请输入正确的金额");
                    }
                    else
                    {
                        MchUserBaseInfo m = MchUserFactory.GetUserBaseByMerchantName(this.txtMerchantName.Text.Trim());

                        string text = this.txtdesc.Text;
                        this.model.UserID = m.UserID;
                        this.model.IncreaseAmt = increaseAmt;
                        this.model.AddTime = DateTime.Now;
                        this.model.MangeId = base.ManageId;
                        this.model.MangeName = base.currentManage.username;
                        this.model.Status = 1;
                        this.model.optype = (SettleTypeEnum) int.Parse(this.rbl_optype.SelectedValue);
                        this.model.Desc = text;
                        if (!this.isUpdate)
                        {
                            if (IncreaseAmt.Add(this.model) > 0)
                            {
                                base.AlertAndRedirect("操作成功！", "IncreaseAmts.aspx");
                            }
                            else
                            {
                                base.AlertAndRedirect("操作失败！");
                            }
                        }
                    }
                }
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

        private void ShowInfo()
        {
            if (this.isUpdate && (this.model != null))
            {
                this.txtMerchantName.Text = this.model.MerchantName.ToString();
                this.txtincreaseAmt.Text = this.model.IncreaseAmt.ToString();
                this.txtdesc.Text = this.model.Desc;
            }
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

        public IncreaseAmtInfo model
        {
            get
            {
                if (this._ItemInfo == null)
                {
                    if (this.ItemInfoId > 0)
                    {
                        this._ItemInfo = IncreaseAmt.GetModel(this.ItemInfoId);
                    }
                    else
                    {
                        this._ItemInfo = new IncreaseAmtInfo();
                    }
                }
                return this._ItemInfo;
            }
        }

        public string MchName
        {
            get
            {
                return WebBase.GetQueryStringString("user", "");
            }
        }
    }
}

