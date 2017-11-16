namespace OriginalStudio.WebUI.Manage
{
    using System;
    using System.Web.UI;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.TimeControl;
    using System.Data;
    using OriginalStudio.BLL;

    public class Default : ManagePageBase
    {
       
        protected DataTable menuTable;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.loginip = base.currentManage.lastLoginIp;
            //this.logintime = FormatConvertor.DateTimeToTimeString(base.currentManage.lastLoginTime.Value);
            //this.username = base.currentManage.username;
            this.DefaultThemes = this.currentManage.DefaultThemes;

            //this.menuTable = ExMenuFactory.getExMenuList();
            
            //this.treeView = ExMenuFactory.getTreeView(0);
            this.DataBind();
            
        }
    }
}

