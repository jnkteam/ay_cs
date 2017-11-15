﻿namespace KuaiCard.WebUI.User.IP
{
    using KuaiCard.WebComponents.Web;
    using KuaiCardLib.Web;
    using System;
    using System.Web;
    using System.Web.UI.WebControls;
    using KuaiCard.BLL.User;

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
            this.rptIPList.DataSource = UserFactory.GetUserBindIp(this.currentUser.ID).Tables[0];
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

                this.Response.Redirect("/User/IP/Delete.aspx?id=" + KuaiCardLib.Security.Cryptography.DESEncryptString(id, "aywl") +
                                                            "&ip=" + KuaiCardLib.Security.Cryptography.DESEncryptString(ip.ToString(), "aywl"));
            }

            //BindData();

        }

        protected string getMarkStr(string ip)
        {
            return KuaiCardLib.Text.Strings.Mark(ip);
        }
    }
}

