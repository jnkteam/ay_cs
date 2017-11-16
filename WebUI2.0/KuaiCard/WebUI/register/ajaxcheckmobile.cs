namespace OriginalStudio.WebUI.register
{
    using OriginalStudio.BLL.User;
    using System;
    using System.Web;

    public class ajaxcheckmobile : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string s = "0";
            string username = context.Request.QueryString["mobile"];
            if (UserFactory.Exists(username))
            {
                s = "0";
            }
            else
            {
                s = "1";
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(s);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

