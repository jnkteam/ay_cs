namespace KuaiCard.WebUI.Manage.Tools
{
    using KuaiCard.BLL.User;
    using KuaiCard.WebComponents.Web;
    using KuaiCardLib.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web;

    public class Supplier1 : ManagePageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string s = "";
            List<SearchParam> searchParams = new List<SearchParam>();
            searchParams.Add(new SearchParam("isrealnamepass", 2));
            string orderby = string.Empty;
            DataTable table = UserFactory.PageSearch(searchParams, 20, 1, orderby).Tables[1];
            if ((table != null) && (table.Rows.Count > 0))
            {
                if (table.Rows[0]["isrealnamepass"].ToString() == "2")
                {
                    s = "1";
                }
            }
            else
            {
                s = "0";
            }
            HttpContext.Current.Response.ContentType = "text/plain";
            HttpContext.Current.Response.Write(s);
        }
    }
}

