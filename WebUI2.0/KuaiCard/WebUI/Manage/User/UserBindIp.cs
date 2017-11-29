namespace OriginalStudio.WebUI.Manage.User
{
    using OriginalStudio.BLL;
    using OriginalStudio.Model;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Security;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using OriginalStudio.BLL.User;
    using System.Data;

    public class UserBindIp : ManagePageBase
    {

       
        protected Button btnAdd;
        protected Repeater bindIpRepeater;
       
        protected DropDownList ipType;
        protected TextBox IP;
        protected void btnAdd_Click(object sender, EventArgs e)
        {

            bool flag = false;
            if (this.isUpdate)
            {
               

                if (MchUserFactory.EditUserBindIp(this.ItemInfoId,int.Parse(this.ipType.SelectedValue),this.IP.Text.ToString()) > 0)
                {
                    flag = true;
                }
                
            }

            if (flag)
            {
                base.AlertAndRedirect("绑定成功");
            }
            else
            {
                base.AlertAndRedirect("绑定成功");
            }


        }
       
        private void InitForm()
        {

            if (this.isUpdate)
            {
                DataSet set = MchUserFactory.GetUserBindIpList(this.ItemInfoId);
                this.bindIpRepeater.DataSource = set.Tables[0];
                this.bindIpRepeater.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            this.setPower();
            if (!base.IsPostBack)
            {
                this.InitForm();
            }
            string cmd = WebBase.GetQueryStringString("cmd", string.Empty);
            int ipid = WebBase.GetQueryStringInt32("ipid", 0);
            int userid = WebBase.GetQueryStringInt32("userid", 0);

            if (!string.IsNullOrEmpty(cmd) && cmd == "delete")
            {
                if (MchUserFactory.DeleteUserBindIp(ipid) > 0)
                {
                    base.AlertAndRedirect("删除成功", "UserBindIp.aspx?id=" + userid);
                }
                else
                {
                    base.AlertAndRedirect("删除失败", "UserBindIp.aspx?id=" + userid);
                }
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

       
        public int ItemInfoId
        {
            get
            {
                return WebBase.GetQueryStringInt32("id", 0);
            }
        }
    }
}

