namespace OriginalStudio.BLL
{
    using System;
    using System.IO;
    using System.Text;
    using System.Web;

    public class BaseFactory
    {
        public static void WriteLogs(string title, string method, string parms, string description)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("-----------------------------------------------------------------------------------------------------------\r\n");
            builder.Append("Time:" + DateTime.Now.ToString() + "\r\n");
            builder.Append("Name:" + title + "\r\n");
            builder.Append("Method:" + method + "\r\n");
            builder.Append("Parms:" + parms + "\r\n");
            builder.Append("Description:" + description + "\r\n");
            string path = HttpContext.Current.Server.MapPath("/Logs/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.AppendAllText(path + DateTime.Now.ToString("yyyy-MM-dd") + ".log", builder.ToString(), Encoding.Default);
        }

        public string OtherInfo
        {
            get;
            set;
        }

        public int Page
        {
            get;
            set;
        }

        public int PageSize
        {
            get;
            set;
        }

        public int Total
        {
            get;
            set;
        }
    }
}

