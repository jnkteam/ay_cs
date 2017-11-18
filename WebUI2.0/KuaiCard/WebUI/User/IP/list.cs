namespace OriginalStudio.WebUI.User.IP
{
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Web;
    using System.Web.UI.WebControls;
    using OriginalStudio.BLL.User;

    public class list : UserPageBase
    {
        protected Repeater rptIPList;

        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();

            
        }

        private void BindData()
        {
            //绑定
            this.rptIPList.DataSource = UserFactory.GetUserBindIp(this.CurrentUser.ID).Tables[0];
            this.rptIPList.DataBind(); 
        }

        protected void rptIPList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                //int id = int.Parse(e.CommandArgument.ToString());
                //把sender由object转换成LinkButton后获取CommandArgument的值
                //UserFactory.DeleteUserBindIp(id);

                string id = e.CommandArgument.ToString();
                string ip = ((Label)(e.Item.FindControl("lblip"))).Text;

                this.Response.Redirect("/User/IP/Delete.aspx?id=" + OriginalStudio.Lib.Security.Cryptography.DESEncryptString(id, "aywl") +
                                                            "&ip=" + OriginalStudio.Lib.Security.Cryptography.DESEncryptString(ip.ToString(), "aywl"));
            }

            //BindData();

        }

        protected string getMarkStr(string ip)
        {
            return OriginalStudio.Lib.Text.Strings.Mark(ip);
        }
    }
}

