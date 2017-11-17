namespace OriginalStudio.WebComponents.Web
{
    using OriginalStudio.BLL;
    using OriginalStudio.Model;
    using OriginalStudio.Lib.SysConfig;
    using System;
    using System.Web;
    using OriginalStudio.Lib.Configuration;
    using OriginalStudio.Lib.TimeControl;

    public class ManagePageBase : PageBase
    {
        private Manage _currentManage = null;
        public string[] AllowHosts = new string[] { "long-bao.com", "localhost", "127.0.0.1" };
        public DateTime eDate = Convert.ToDateTime("2115-01-10");
        public DateTime sDate = Convert.ToDateTime("2012-01-01");
        /// <summary>
        /// 默认主题
        /// </summary>
        protected string DefaultThemes = "";

        public string ADMIN_URI = ConfigHelper.GetConfig("runtimeSettings", "ADMIN_URI");
        public string username = string.Empty;
        public string loginip = string.Empty;
        public string logintime = string.Empty;
        public string treeView = string.Empty;
        public void checkLogin()
        {
            string str = string.Format("/{0}/Login.aspx", RuntimeSetting.ManagePagePath);
            try
            {
                this.username = ManageFactory.CurrentManage.username;
                this.loginip = ManageFactory.CurrentManage.lastLoginIp;
                this.logintime = FormatConvertor.DateTimeToTimeString(ManageFactory.CurrentManage.lastLoginTime.Value);
                this.treeView = ExMenuFactory.getTreeView(0);
            }
            catch
            {

            }

            if ((DateTime.Now < this.sDate) || (DateTime.Now > this.eDate))
            {
                HttpContext.Current.Response.Write(string.Format("\r\n<SCRIPT LANGUAGE='javascript'><!--\r\nalert('试用过期请联系管理员');top.location.href=\"{0}\";\r\n//--></SCRIPT>", str));
                HttpContext.Current.Response.End();
            }
            if (!this.IsLogin)
            {
                HttpContext.Current.Response.Write(string.Format("\r\n<SCRIPT LANGUAGE='javascript'><!--\r\ntop.location.href=\"{0}\";\r\n//--></SCRIPT>", str));
                HttpContext.Current.Response.End();
            }
        }

        public static string GetViewStatusName(object isRealNamePass)
        {
            if (isRealNamePass == DBNull.Value)
            {
                return string.Empty;
            }
            if (Convert.ToInt32(isRealNamePass) == 3)
            {
                return "<font style=\"color: #F40;\">审核失败</font>";
            }
            if (Convert.ToInt32(isRealNamePass) == 2)
            {
                return "<font style=\"color: #2254F3;\">待审核</font>";
            }
            return ((Convert.ToInt32(isRealNamePass) == 1) ? "<font style=\"color: #F39C2D;\">审核成功</font>" : "<font style=\"color: #2254F3;\">无提交认证</font>");
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.checkLogin();
        }

        public bool CheckHost
        {
            get
            {
                string host = HttpContext.Current.Request.Url.Host;
                foreach (string str2 in this.AllowHosts)
                {
                    if (host.ToLower().Contains(str2))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public Manage currentManage
        {
            get
            {
                if (this._currentManage == null)
                {
                    this._currentManage = ManageFactory.CurrentManage;
                }
                return this._currentManage;
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

