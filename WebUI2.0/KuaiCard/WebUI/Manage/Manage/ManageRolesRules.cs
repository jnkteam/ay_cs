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

    public class ManageRolesRules : ManagePageBase
    {

        public Model.ManageRoles _ItemInfo = null;
        protected Button btnAdd;

        protected string rolesRules = string.Empty;
        private object context;
        protected HiddenField rolesRulesCheckBox;
        protected void btnAdd_Click(object sender, EventArgs e)
        {

            bool flag = false;
            if (this.isUpdate)
            {
                this.ItemInfo.Id = this.ItemInfoId;
                this.ItemInfo.Rules = this.rolesRulesCheckBox.Value;

                if (ManageRolesFactory.UpdateRules(this.ItemInfo))
                {
                    flag = true;
                }
            }

            if (flag)
            {
                showPageMsg("访问授权操作成功");
            }
            else
            {
                showPageMsg("操作失败");
            }


        }

        private void InitForm()
        {

            if (this.isUpdate)
            {
                this.rolesRules = ExMenuFactory.getRolesRules(this.ItemInfo);
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



        public bool isUpdate
        {
            get
            {
                return (this.ItemInfoId > 0);
            }
        }

        public OriginalStudio.Model.ManageRoles ItemInfo
        {
            get
            {
                if (this._ItemInfo == null)
                {
                    if (this.isUpdate)
                    {
                        this._ItemInfo = ManageRolesFactory.GetModelById(this.ItemInfoId);
                    }
                    else
                    {
                        this._ItemInfo = new OriginalStudio.Model.ManageRoles();
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

