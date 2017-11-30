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

    public class UserBankCard : ManagePageBase
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

            }

            if (flag)
            {
                base.AlertAndRedirect("绑定成功");
            }
            else
            {
                base.AlertAndRedirect("绑定失败");
            }


        }

        private void InitForm()
        {

            if (this.isUpdate)
            {
                DataSet set = MchUserFactory.GetUserPayBankList(this.ItemInfoId);

                DataTable table = set.Tables[0];
                table.Columns.Add("accountTypeName");
                table.Columns.Add("BankAccountHide");
                foreach (DataRow row in table.Rows)
                {
                    
                     row["accountTypeName"] = Convert.ToInt32(row["AccountType"]) == 1 ? "对公" : "对私";
                     string str = row["BankAccount"].ToString();
                     string hideString = str.Length > 10 ? str.Remove(10)+"*******" : str;
                     row["BankAccountHide"] = hideString;

                }


                this.bindIpRepeater.DataSource = table;
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
            int bankid = WebBase.GetQueryStringInt32("bankid", 0);
            int userid = WebBase.GetQueryStringInt32("userid", 0);

            if (!string.IsNullOrEmpty(cmd) && cmd == "delete")
            {
                if (MchUserFactory.DeleteUserPayBank(bankid) > 0)
                {
                    base.AlertAndRedirect("删除成功", "UserBankCard.aspx?id=" + userid);
                }
                else
                {
                    base.AlertAndRedirect("删除失败", "UserBankCard.aspx?id=" + userid);
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

