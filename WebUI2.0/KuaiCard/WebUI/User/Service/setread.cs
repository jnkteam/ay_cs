namespace KuaiCard.WebUI.User.Service
{
    using OriginalStudio.BLL;
    using OriginalStudio.Model;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Web;
    using System.Web.SessionState;

    public class setread : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        private string ids(string m_Id)
        {
            string str = "";
            foreach (string str2 in m_Id.Split(new char[] { ',' }))
            {
                int result = 0;
                if (int.TryParse(str2, out result))
                {
                    str = result.ToString();
                }
            }
            return str;
        }

        public void ProcessRequest(HttpContext context)
        {
            string s = "";
            string str2 = context.Request["id"];
            foreach (string str3 in str2.Split(new char[] { ',' }))
            {
                int result = 0;
                if (int.TryParse(str3, out result))
                {
                    IMSG model = IMSGFactory.GetModel(result);
                    model.ID = result;
                    model.isRead = true;
                    if (IMSGFactory.Update(model))
                    {
                        s = "{result:true, text:'标记成功', time:1.5, reload:true}";
                    }
                    else
                    {
                        s = "{result:false, text:'标记失败', time:1.5, reload:true}";
                    }
                }
            }
            context.Response.ContentType = "text/html";
            context.Response.Write(s);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public int m_Id
        {
            get
            {
                return WebBase.GetQueryStringInt32("id", 0);
            }
        }
    }
}

