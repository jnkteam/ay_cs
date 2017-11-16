namespace KuaiCard.WebUI.User.Service
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model;
    using OriginalStudio.Model.User;
    using System;
    using System.IO;
    using System.Web;
    using System.Web.SessionState;

    public class upload : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        private WebInfo _webinfo = null;
        private UserInfo _currentUser = null;

        public void ProcessRequest(HttpContext context)
        {
            string s = string.Empty;

            if (this.CurrentUser == null)
            {
                s = "{result:false,text:'未登录',ok:true}";
                context.Response.ContentType = "text/html";
                context.Response.Write(s);
                return;
            }

            HttpPostedFile file = context.Request.Files[0];
            string str2 = context.Request["fileUp2"];   //反面
            string str3 = context.Request["fileUp1"];   //正面
            //KuaiCardLib.Logging.LogHelper.Write("文件名:" + file.FileName);
            if (file != null)
            {
                try
                {
                    string front = string.Concat(new object[] { "/Upload/", this.CurrentUser.ID, "/", this.CurrentUser.frontPic });
                    string verso = string.Concat(new object[] { "/Upload/", this.CurrentUser.ID, "/", this.CurrentUser.versoPic });
                    if (File.Exists(HttpContext.Current.Server.MapPath(front)))
                    {
                        File.Delete(HttpContext.Current.Server.MapPath(front));
                    }
                    if (File.Exists(HttpContext.Current.Server.MapPath(verso)))
                    {
                        File.Delete(HttpContext.Current.Server.MapPath(verso));
                    }
                    string str6 = context.Server.MapPath("/Upload/" + this.CurrentUser.ID);
                    string str7 = file.FileName.Substring(file.FileName.LastIndexOf('.'));
                    string str8 = Guid.NewGuid().ToString() + str7;
                    if (!Directory.Exists(str6))
                    {
                        Directory.CreateDirectory(str6);
                    }
                    string filename = str6 + "/" + str8;
                    file.SaveAs(filename);
                    string str10 = string.Concat(new object[] { "/Upload/", this.CurrentUser.ID, "/", str8 });
                    s = s = "{\"result\":true,\"url\":\"" + str10 + "\"}";
                }
                catch
                {
                    s = "{result:false,text:'提交失败',ok:true}";
                }
            }
            context.Response.ContentType = "text/html";
            context.Response.Write(s);
        }

        public UserInfo CurrentUser
        {
            get
            {
                if (this._currentUser == null)
                {
                    this._currentUser = UserFactory.CurrentMember;
                }
                return this._currentUser;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public WebInfo webInfo
        {
            get
            {
                if (this._webinfo == null)
                {
                    this._webinfo = WebInfoFactory.CurrentWebInfo;
                }
                return this._webinfo;
            }
        }
    }
}

