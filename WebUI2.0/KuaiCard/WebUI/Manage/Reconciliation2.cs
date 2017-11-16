namespace KuaiCard.WebUI.Manage
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Order;
    using OriginalStudio.Model;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Reconciliation2 : ManagePageBase
    {
        protected Button btn_Reconciliation;
        protected Button btn_search;
        protected TextBox EtimeBox;
        protected HtmlForm form1;
        protected HtmlHead Head1;
        protected Repeater rptOrders;
        protected TextBox StimeBox;

        private void Bind()
        {
            DateTime sdt = DateTime.Parse(this.StimeBox.Text);
            DateTime edt = DateTime.Parse(this.EtimeBox.Text);
            DataTable table = Dal.GetFailOrders2(sdt, edt);
            this.rptOrders.DataSource = table;
            this.rptOrders.DataBind();
        }

        protected void btn_Reconciliation_Click(object sender, EventArgs e)
        {
            this.ProcessNotify();
            base.AlertAndRedirect("执行完成。");
            this.Bind();
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            this.Bind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            if (!base.IsPostBack)
            {
                this.StimeBox.Text = DateTime.Now.AddHours(-24.0).ToString("yyyy-MM-dd HH:mm:ss");
                this.EtimeBox.Text = DateTime.Now.AddSeconds(-2.0).ToString("yyyy-MM-dd HH:mm:ss");
                this.StimeBox.Attributes.Add("onFocus", "WdatePicker({startDate:'%y-%M-01 00:00:00',dateFmt:'yyyy-MM-dd HH:mm:ss',alwaysUseStartDate:true})");
                this.EtimeBox.Attributes.Add("onFocus", "WdatePicker({startDate:'%y-%M-01 00:00:00',dateFmt:'yyyy-MM-dd HH:mm:ss',alwaysUseStartDate:true})");
            }
        }

        private void ProcessNotify()
        {

        }

        private void setPower()
        {
            if (!ManageFactory.CheckCurrentPermission(false, ManageRole.System))
            {
                base.Response.Write("Sorry,No authority!");
                base.Response.End();
            }
        }
    }
}

