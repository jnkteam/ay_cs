namespace KuaiCard.WebUI.Manage.Tools
{
    using OriginalStudio.BLL;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web;

    public class Alarm : ManagePageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string s = "";
            List<SearchParam> searchParams = new List<SearchParam>();
            searchParams.Add(new SearchParam("status", 1));
            DataTable table = SettledFactory.PageSearch(searchParams, 8, 1, string.Empty).Tables[1];
            if ((table != null) && (table.Rows.Count > 0))
            {
                if (table.Rows[0]["status"].ToString() == "1")
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

