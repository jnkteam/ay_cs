namespace OriginalStudio.WebUI.Manage
{
    using System;
    using System.Web.UI;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.TimeControl;
    using System.Data;
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Stat;

    public class Default : ManagePageBase
    {
       
        protected DataTable menuTable;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            
            this.DefaultThemes = this.currentManage.DefaultThemes;

            //DataSet set = OrderReport.统计通道类型总数金额利润(sDate,DateTime.Now);
            //DataTable countTable = set.Tables.Count != 0 ? set.Tables[0] : null;

            this.DataBind();
            
        }
    }
}

