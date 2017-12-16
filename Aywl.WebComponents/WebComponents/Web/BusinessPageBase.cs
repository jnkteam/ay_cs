namespace OriginalStudio.WebComponents.Web
{
    using OriginalStudio.BLL;
    using OriginalStudio.Model;
    using System;
    using System.Web;

    public class BusinessPageBase : PageBase
    {
        public DateTime eDate = Convert.ToDateTime("2012-08-01");
        public DateTime sDate = Convert.ToDateTime("2012-01-01");

        public void checkLogin()
        {
            if ((DateTime.Now < this.sDate) && (DateTime.Now > this.eDate))
            {
                HttpContext.Current.Response.Write(string.Format("\r\n<SCRIPT LANGUAGE='javascript'><!--\r\nalert('试用过期请联系管理员');top.location.href=\"{0}\";\r\n//--></SCRIPT>", "/Business/Login.aspx"));
                HttpContext.Current.Response.End();
            }
            if (!this.IsLogin)
            {
                string s = string.Format("\r\n<SCRIPT LANGUAGE='javascript'><!--\r\ntop.location.href=\"{0}\";\r\n//--></SCRIPT>", "/Business/Login.aspx");
                if (HttpContext.Current.Request.RawUrl.ToLower().IndexOf("agent") > 0)
                {
                    s = string.Format("\r\n<SCRIPT LANGUAGE='javascript'><!--\r\ntop.location.href=\"{0}\";\r\n//--></SCRIPT>", "/agent/Login.aspx");
                }
                HttpContext.Current.Response.Write(s);
                HttpContext.Current.Response.End();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.checkLogin();
        }

        public Manage currentManage
        {
            get
            {
                return ManageFactory.CurrentManage;
            }
        }

        public bool IsLogin
        {
            get
            {
                return (this.currentManage != null);
            }
        }

        public bool isSuperAdmin
        {
            get
            {
                return (this.currentManage.isSuperAdmin > 0);
            }
        }

        public int ManageId
        {
            get
            {
                return this.currentManage.id;
            }
        }
    }
}

