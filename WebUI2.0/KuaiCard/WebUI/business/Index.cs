namespace OriginalStudio.WebUI.Business
{
    using OriginalStudio.BLL;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.TimeControl;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Index : BusinessPageBase
    {
        protected HtmlForm form1;
        protected Literal litlinks;
        protected string loginip;
        protected string logintime;
        protected string monthcommission = "0.00";
        protected string monthtotalAmt = "0.00";
        protected HtmlGenericControl paysouid;
        protected string todaytotalAmt = "0.00";
        protected string username;
        protected string userscount;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            if (!base.IsPostBack)
            {
                try
                {
                    decimal totalAmt;
                    decimal commission;
                    this.litlinks.Text = "?s=" + base.currentManage.id.ToString();
                    this.userscount = ManageFactory.GetManageUsers(base.currentManage.id).ToString();
                    if (ManageFactory.GetManagePerformance(base.currentManage.id, 
                        Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")), 
                        DateTime.Now.AddDays(1.0), 
                        out totalAmt, out commission))
                    {
                        this.todaytotalAmt = totalAmt.ToString("f2");
                    }
                    if (ManageFactory.GetManagePerformance(base.currentManage.id, Convert.ToDateTime(DateTime.Now.AddDays(-7.0).ToString("yyyy-MM-dd 00:00:00")), DateTime.Now.AddDays(1.0), out totalAmt, out commission))
                    {
                        this.monthtotalAmt = totalAmt.ToString("f2");
                        this.monthcommission = commission.ToString("f2");
                    }
                    this.loginip = base.currentManage.lastLoginIp;
                    this.logintime = FormatConvertor.DateTimeToTimeString(base.currentManage.lastLoginTime.Value);
                    this.username = base.currentManage.username;
                }
                catch
                {
                }
            }
        }

        private void setPower()
        {
        }
    }
}

