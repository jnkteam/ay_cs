namespace OriginalStudio.WebUI.User.Service
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model;
    using OriginalStudio.Model.User;
    using System;
    using System.IO;
    using System.Web;
    using System.Web.SessionState;

    public class upload2 : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        private WebInfo _webinfo = null;

        public void ProcessRequest(HttpContext context)
        {
            string s = string.Empty;
            HttpPostedFile file = context.Request.Files[0];
            string str2 = context.Request["fileUp2"];
            string str3 = context.Request["fileUp1"];
            if (file != null)
            {
                try
                {
                    string path = string.Concat(new object[] { "/Upload/", this.currentUser.ID, "/", this.currentUser.frontPic });
                    if (File.Exists(HttpContext.Current.Server.MapPath(path)))
                    {
                        File.Delete(HttpContext.Current.Server.MapPath(path));
                    }
                    string str5 = context.Server.MapPath("/Upload/" + this.currentUser.ID);
                    string str6 = file.FileName.Substring(file.FileName.LastIndexOf('.'));
                    string str7 = Guid.NewGuid().ToString() + str6;
                    if (!Directory.Exists(str5))
                    {
                        Directory.CreateDirectory(str5);
                    }
                    string filename = str5 + "/" + str7;
                    string str9 = this.currentUser.ID + "/" + str7;
                    file.SaveAs(filename);
                    s = s = "{result:true,text:'提交已经受理，请等待审核！',time:2,url:'/user/withdrawal/'}";
                }
                catch
                {
                    s = "{result:false,text:'提交失败',ok:true}";
                }
            }
            context.Response.Write(s);
        }

        public UserInfo currentUser
        {
            get
            {
                return UserFactory.CurrentMember;
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

