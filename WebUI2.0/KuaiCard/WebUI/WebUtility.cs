namespace OriginalStudio.WebUI
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Supplier;
    using OriginalStudio.BLL.Settled;
    using OriginalStudio.Lib.Security;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class WebUtility
    {
        public static void AlertAndClose(Page P, string msg)
        {
            string script = string.Empty;
            if ((msg == null) || (msg.Length == 0))
            {
                script = "\r\n<SCRIPT LANGUAGE='javascript'><!--\r\nwindow.close();\r\n//--></SCRIPT>\r\n";
            }
            else
            {
                script = string.Format("\r\n<SCRIPT LANGUAGE='javascript'><!--\r\nalert({0});\r\nwindow.close();\r\n//--></SCRIPT>\r\n", AntiXss.JavaScriptEncode(msg));
            }
            P.ClientScript.RegisterClientScriptBlock(P.GetType(), "AlertAndClose", script);
        }

        public static void AlertAndRedirect(Page P, string msg)
        {
            AlertAndRedirect(P, msg, null);
        }

        public static void AlertAndRedirect(Page P, string msg, string url)
        {
            string script = string.Empty;
            if (((msg == null) || (msg.Length == 0)) && ((url == null) || (url.Length == 0)))
            {
                script = "\r\n<SCRIPT LANGUAGE='javascript'><!--\r\nlocation.href=location.href;\r\n//--></SCRIPT>\r\n";
            }
            else if ((msg == null) || (msg.Length == 0))
            {
                script = string.Format("\r\n<SCRIPT LANGUAGE='javascript'><!--\r\nlocation.href=\"{0}\";\r\n//--></SCRIPT>\r\n", url);
            }
            else if ((url == null) || (url.Length == 0))
            {
                script = string.Format("\r\n<SCRIPT LANGUAGE='javascript'><!--\r\nalert({0});\r\nlocation.href=location.href;\r\n//--></SCRIPT>\r\n", AntiXss.JavaScriptEncode(msg));
            }
            else
            {
                script = string.Format("\r\n<SCRIPT LANGUAGE='javascript'><!--\r\nalert({0});\r\nlocation.href=\"{1}\";\r\n//--></SCRIPT>\r\n", AntiXss.JavaScriptEncode(msg), url);
            }
            P.ClientScript.RegisterClientScriptBlock(P.GetType(), "AlertAndRedirect", script);
        }

        public static void BindBankSupplierDDL(DropDownList ddl)
        {
            ddl.Items.Clear();
            string where = "isbank =1";
            DataTable table = SysSupplierFactory.GetList(where).Tables[0];
            ddl.Items.Add(new ListItem("--请选择--", "0"));
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    ddl.Items.Add(new ListItem(row["name"].ToString(), row["code"].ToString()));
                }
            }
        }

        public static void BindBquestionDDL(DropDownList ddl)
        {
            ddl.Items.Clear();
            DataTable cacheList = new QuestionFactory().GetCacheList();
            ddl.Items.Add(new ListItem("--请选择--", "0"));
            if (cacheList != null)
            {
                foreach (DataRow row in cacheList.Rows)
                {
                    ddl.Items.Add(new ListItem(row["question"].ToString(), row["id"].ToString()));
                }
            }
        }

        public static void BindCardSupplierDLL(DropDownList ddl)
        {
            ddl.Items.Clear();
            string where = "iscard =1";
            DataTable table = SysSupplierFactory.GetList(where).Tables[0];
            ddl.Items.Add(new ListItem("--请选择--", "0"));
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    ddl.Items.Add(new ListItem(row["name"].ToString(), row["code"].ToString()));
                }
            }
        }

        public static void BindSMSSupplierDLL(DropDownList ddl)
        {
            ddl.Items.Clear();
            string where = "issms =1";
            DataTable table = SysSupplierFactory.GetList(where).Tables[0];
            ddl.Items.Add(new ListItem("--请选择--", "0"));
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    ddl.Items.Add(new ListItem(row["name"].ToString(), row["code"].ToString()));
                }
            }
        }

        public static void BindSXSupplierDLL(DropDownList ddl)
        {
            ddl.Items.Clear();
            string where = "issx =1";
            DataTable table = SysSupplierFactory.GetList(where).Tables[0];
            ddl.Items.Add(new ListItem("--请选择--", "0"));
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    ddl.Items.Add(new ListItem(row["name"].ToString(), row["code"].ToString()));
                }
            }
        }

        public static string GetIPAddress(string ip)
        {
            try
            {
                IpList list = new IpList();
                list.IP = ip;
                return list.IPLocation().Replace("本机地址", "局域网IP");
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetIPAddressInfo(string ip)
        {
            IpList list = new IpList();
            list.IP = ip;
            string str = list.IPAddInfo();
            if (str != null)
            {
                return str.Replace("CZ88.NET", "");
            }
            return "";
        }

        public static string GetPayModeViewName(int pmode)
        {
            string str = string.Empty;
            switch (pmode)
            {
                case 1:
                    return "银行帐户";

                case 2:
                    return "支付宝";

                case 3:
                    return "财付通";
            }
            return str;
        }

        public static string GetsupplierName(object obj)
        {
            try
            {
                if ((obj == DBNull.Value) || (obj == null))
                {
                    return string.Empty;
                }
                return SysSupplierFactory.GetSupplierModelByCode(int.Parse(obj.ToString())).SupplierName;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 弹出提示信息。
        /// </summary>
        /// <param name="page">引用脚本的Page页</param>
        /// <param name="strMessage">提示信息</param>
        public static void ShowMessage(System.Web.UI.Page page, string strMessage)
        {
            strMessage = "alert('" + strMessage + "');";
            if (!page.ClientScript.IsStartupScriptRegistered("showmsg"))
                page.ClientScript.RegisterStartupScript(page.GetType(), "showmsg", strMessage, true);
        }
    }
}

