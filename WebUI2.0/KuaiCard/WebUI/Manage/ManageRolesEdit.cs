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

    public class ManageRolesEdit : ManagePageBase
    {
        
        public Model.ManageRoles _ItemInfo = null;
        protected Button btnAdd;
       
        protected DropDownList status;
        protected HtmlForm form1;
        protected HiddenField hf_isupdate;
        protected TextBox title;      
        protected TextBox module;      
        protected TextBox description;



        protected void btnAdd_Click(object sender, EventArgs e)
        {

            this.ItemInfo.Title = this.title.Text.Trim();
            this.ItemInfo.Module = this.module.Text.Trim();
            this.ItemInfo.Description = this.description.Text.Trim();
            this.ItemInfo.Status = int.Parse(this.status.SelectedValue.ToString());
            
            bool flag = false;
            if (this.isUpdate)
            {
                this.ItemInfo.Id = this.ItemInfoId;
                if (ManageRolesFactory.Update(this.ItemInfo))
                {
                    flag = true;
                }
            }
            else if (ManageRolesFactory.Add(this.ItemInfo) > 0)
            {
                flag = true;
            }
            if (flag)
            {
                base.AlertAndRedirect("操作成功");
            }
            else
            {
                base.AlertAndRedirect("操作失败");
            }

           
        }

        private void InitForm()
        {
           
            if (this.isUpdate)
            {
                this.title.Text = this.ItemInfo.Title;
                this.module.Text = this.ItemInfo.Module;
                this.description.Text = this.ItemInfo.Description ;
                this.status.SelectedValue = this.ItemInfo.Status.ToString();
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

