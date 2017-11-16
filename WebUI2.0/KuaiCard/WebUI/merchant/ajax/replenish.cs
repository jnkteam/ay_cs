namespace KuaiCard.WebUI.merchant.ajax
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model.User;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Web;
    using System.Web.SessionState;

    public class replenish : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string s = "";
            if (this.currentUser == null)
            {
                s = "登录信息失效，请重新登录";
            }
            else if ((this.type == 0) || string.IsNullOrEmpty(this.orderid))
            {
                s = "参数不正确";
            }
            else if (this.type == 1)
            {
                new OrderBankNotify().SynchronousNotify(this.orderid);
                s = "true";
            }
            else if (this.type == 2)
            {
                new OrderCardNotify().SynchronousNotify(this.orderid);
                s = "true";
            }
            context.Response.ContentType = "text/plain";
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

        public string orderid
        {
            get
            {
                return WebBase.GetQueryStringString("order", "");
            }
        }

        public int type
        {
            get
            {
                return WebBase.GetQueryStringInt32("type", 0);
            }
        }
    }
}

