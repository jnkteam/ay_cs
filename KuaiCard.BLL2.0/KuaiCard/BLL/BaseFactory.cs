namespace KuaiCard.BLL
{
    using System;
    using System.IO;
    using System.Text;
    using System.Web;

    public class BaseFactory
    {
        private string _otherinfo = string.Empty;
        private int _page = 0;
        private int _pagesize = 0x19;
        private int _total = 0;

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
            get
            {
                return this._otherinfo;
            }
            set
            {
                this._otherinfo = value;
            }
        }

        public int Page
        {
            get
            {
                return this._page;
            }
            set
            {
                this._page = value;
            }
        }

        public int PageSize
        {
            get
            {
                return this._pagesize;
            }
            set
            {
                this._pagesize = value;
            }
        }

        public int Total
        {
            get
            {
                return this._total;
            }
            set
            {
                this._total = value;
            }
        }
    }
}

