namespace OriginalStudio.WebUI.User.Service
{
    using OriginalStudio.BLL.User;
    using System;
    using System.Web;
    using System.Web.SessionState;
    using OriginalStudio.Model.User;

    public class delbank : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        private string ico = string.Empty;
        private string msg = string.Empty;
        private string reload = string.Empty;
        private string results = string.Empty;

        public void ProcessRequest(HttpContext context)
        {
            if (this.currentUser == null)
            {
                //未登录
                msg = "未登录!";
                results = "false";
                goto Label_Exit;
            }

            string str = HttpContext.Current.Request["id"];

            int id = 0;
            if (Int32.TryParse(str, out id))
            {
                if (false) // UserPayBankApp.Delete(id))
                {
                    this.msg = "删除提现银行信息成功";
                    this.results = "true";
                    this.reload = "true";
                }
                else
                {
                    this.msg = "删除失败";
                    this.results = "false";
                    this.reload = "false";
                }
            }
            else
            {
                this.msg = "删除失败";
                this.results = "false";
                this.reload = "false";
            }

        Label_Exit:
            context.Response.ContentType = "application/json";
            context.Response.Write("{\"result\":\"" + this.results + "\",\"text\":\"" + this.msg + "\",\"time\":\"2\",\"reload\":\"" + this.reload + "\"}");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public UserInfo currentUser
        {
            get
            {
                return UserFactory.CurrentMember;
            }
        }

    }
}

