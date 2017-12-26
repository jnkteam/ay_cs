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
    using OriginalStudio.BLL.Settled;
    using System.Data;
    using OriginalStudio.BLL.Channel;

    public class UserTypeLimit : ManagePageBase
    {


        protected Button btnAdd;
        protected Repeater typeLimitRepeater;

        protected DropDownList channelType;
        protected TextBox MinMoney;
        protected TextBox MaxMoney;

        protected void btnAdd_Click(object sender, EventArgs e)
        {

            bool flag = false;
            if (this.isUpdate)
            {


                if (MchUserFactory.EditUserChannelTypeLimit(this.ItemInfoId, int.Parse(this.channelType.SelectedValue), decimal.Parse(this.MinMoney.Text.ToString()), decimal.Parse(this.MaxMoney.Text.ToString())) > 0)
                {
                    flag = true;
                }
                

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
                DataTable table = SysChannelType.GetList(null).Tables[0];
                foreach (DataRow row in table.Rows)
                {
                    this.channelType.Items.Add(new ListItem(row["typename"].ToString(), row["typeId"].ToString()));
                }
                DataSet set = MchUserFactory.GetUserChannelTypeLimit(this.ItemInfoId);
                this.typeLimitRepeater.DataSource = set.Tables[0];
                this.typeLimitRepeater.DataBind();
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
            int typeid = WebBase.GetQueryStringInt32("typeid", 0);
            int userid = WebBase.GetQueryStringInt32("userid", 0);
            string min = WebBase.GetQueryStringString("min" , string.Empty);
            string max = WebBase.GetQueryStringString("max" , string.Empty);


            if (!string.IsNullOrEmpty(cmd) && cmd == "edit")
            {
                if (MchUserFactory.EditUserChannelTypeLimit(userid,typeid, decimal.Parse(min), decimal.Parse(max)) > 0)
                {
                    base.AlertAndRedirect("编辑成功", "UserTypeLimit.aspx?id=" + userid);
                }
                else
                {
                    base.AlertAndRedirect("编辑失败", "UserTypeLimit.aspx?id=" + userid);
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

