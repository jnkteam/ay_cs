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
        /// <summary>
        /// 公用方法  提示
        /// </summary>
        /// <param name="msg"></param>
        public void showPageMsg(string msg)
        {

            HttpContext.Current.Response.Write("<SCRIPT LANGUAGE='javascript'><!--\r\nvar index = parent.layer.getFrameIndex(window.name);parent.layer.close(index);parent.showPageMsg('" + msg + "');\r\n//--></SCRIPT>");
        }
        /// <summary>
        /// auth
        /// </summary>
        public void checkAuth() {
            string url = HttpContext.Current.Request.Path;
            //OriginalStudio.Lib.Logging.LogHelper.Write("url:"+url);
            int menuId = ExMenuFactory.getExMenuIdByControl(url);          
            if (menuId > 0) {                
                if (!ExMenuFactory.authContains(menuId.ToString(), "menu")) {
                    Response.Write("权限错误! CODE : 1244");
                    Response.End();
                }               
            }
            //---------/------------/----------------
            int rulesId = ExMenuFactory.getRulesIdByControl(url);
            if (rulesId > 0)
            {
                if (!ExMenuFactory.authContains(rulesId.ToString(), "rules"))
                {
                    Response.Write("没有当前项操作权限! CODE : 1245");
                    Response.End();
                }
            }
            // Response.Write("没有当前项操作权限! CODE : 1245");



        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.checkLogin();
            this.checkAuth();
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

