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
    using OriginalStudio.BLL.Channel;
    using OriginalStudio.BLL.BLL.User;
    using OriginalStudio.Model.User;

    public class UserPayRate : ManagePageBase
    {


        protected Button btnAdd;
        protected Repeater PayRateRepeater;
        protected Label labUserName;
        

        protected CheckBox Special;
        protected HiddenField hideValue;
        
            
        protected void btnAdd_Click(object sender, EventArgs e)
        {

            bool flag = false;
            if (this.isUpdate)
            {
                MchUserPayRateInfo model = new MchUserPayRateInfo();
                model.UserID        = this.ItemInfoId;
                model.Special       = this.Special.Checked ? 1 : 0;
                model.PayrateXML    = this.hideValue.Value;

                if (MchUserPayRateFactory.EditUserChannelPayRate(model))
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
                MchUserBaseInfo userInfo = MchUserFactory.GetUserBaseByUserID(this.ItemInfoId);
                this.labUserName.Text = userInfo.UserName.ToString(); 
                DataSet set = SysChannelType.GetList(true);
                DataTable table =  set.Tables[0];
                table.Columns.Add("rateValue");

                MchUserPayRateInfo myRateInfo = MchUserPayRateFactory.GetModel(this.ItemInfoId);
                this.Special.Checked = myRateInfo.Special == 1 ? true :false;
                foreach (DataRow row in table.Rows)
                {
                    if (myRateInfo.Special == 1)
                    {
                        decimal speRate  = MchUserPayRateFactory.GetUserChannelPayRate(this.ItemInfoId, Convert.ToInt32(row["typeid"]));
                        row["rateValue"] = speRate * 100;
                    }
                    else {
                        row["rateValue"] = Convert.ToDouble(row["supplierrate"])*100;
                    }
                    
                }
                this.PayRateRepeater.DataSource = table;
                this.PayRateRepeater.DataBind();
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

