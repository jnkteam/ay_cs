using System;
using System.Web;
using System.Data;
using OriginalStudio.Lib.Web;
using System.Text;
using OriginalStudio.Lib.Security;
using OriginalStudio.Lib;
using OriginalStudio.Model.User;
using System.Web.SessionState;
using OriginalStudio.Lib.Data;
using Wuqi.Webdiyer;
using System.Collections.Generic;
using System.Data.SqlClient;
using OriginalStudio.DBAccess;
using OriginalStudio.BLL;
using OriginalStudio.Model.Order;
using OriginalStudio.BLL.Supplier;
using OriginalStudio.BLL.Settled;

namespace OriginalStudio.WebUI.Manage.Order
{
    public class SearchOrderHandler : System.Web.IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            Json json = new Json();

            List<SearchParam> searchParams = new List<SearchParam>();
            int result = 0;
            AspNetPager Pager1 = new AspNetPager();
            string txtOrderId = context.Request.Form["txtOrderId"];
            string txtuserid = context.Request.Form["txtuserid"];
            string ddlChannelType = context.Request.Form["ddlChannelType"];
            string ddlsupp = context.Request.Form["ddlsupp"];
            string txtUserOrder = context.Request.Form["txtUserOrder"];
            string txtSuppOrder = context.Request.Form["txtSuppOrder"];
            string ddlOrderStatus = context.Request.Form["ddlOrderStatus"];
            string ddlNotifyStatus = context.Request.Form["ddlNotifyStatus"];
            string StimeBox = context.Request.Form["StimeBox"];
            string EtimeBox = context.Request.Form["EtimeBox"];
            string page = context.Request.Form["page"];

            if (!string.IsNullOrEmpty(txtOrderId.Trim()))
            {
                searchParams.Add(new SearchParam("orderId_like", txtOrderId.Trim()));
            }
            if (!(string.IsNullOrEmpty(txtuserid.Trim()) || !int.TryParse(txtuserid.Trim(), out result)))
            {
                searchParams.Add(new SearchParam("userid", result));
            }
            if ((!string.IsNullOrEmpty(ddlChannelType.Trim()) && int.TryParse(ddlChannelType.Trim(), out result)) && (result > 0))
            {

                searchParams.Add(new SearchParam("channeltypeId", result));
            }
            if ((!string.IsNullOrEmpty(ddlsupp.Trim()) && int.TryParse(ddlsupp.Trim(), out result)) && (result > 0))
            {
                searchParams.Add(new SearchParam("supplierId", result));
            }
            if (!string.IsNullOrEmpty(txtUserOrder.Trim()))
            {
                searchParams.Add(new SearchParam("userorder", txtUserOrder.Trim()));
            }
            if (!string.IsNullOrEmpty(txtSuppOrder.Trim()))
            {
                searchParams.Add(new SearchParam("supplierOrder", txtSuppOrder.Trim()));
            }
            if ((!string.IsNullOrEmpty(ddlOrderStatus.Trim()) && int.TryParse(ddlOrderStatus.Trim(), out result)) && (result > 0))
            {
                searchParams.Add(new SearchParam("status", result));
            }
            if ((!string.IsNullOrEmpty(ddlNotifyStatus.Trim()) && int.TryParse(ddlNotifyStatus.Trim(), out result)) && (result > 0))
            {
                searchParams.Add(new SearchParam("notifystat", result));
            }
            DateTime minValue = DateTime.MinValue;
            if ((!string.IsNullOrEmpty(StimeBox.Trim()) && DateTime.TryParse(StimeBox.Trim(), out minValue)) && (minValue > DateTime.MinValue))
            {
                searchParams.Add(new SearchParam("stime", StimeBox.Trim()));
            }
            if ((!string.IsNullOrEmpty(EtimeBox.Trim()) && DateTime.TryParse(EtimeBox.Trim(), out minValue)) && (minValue > DateTime.MinValue))
            {
                searchParams.Add(new SearchParam("etime", minValue.AddDays(1.0)));
            }

            DataSet set = new OrderBank().AdminPageSearch(searchParams, Pager1.PageSize, Convert.ToInt32(page));

            DataTable table = null;
            if (set.Tables.Count != 0)
            {
                table = set.Tables[0];
                table.Columns.Add("orderTypeName"); //订单类型
                table.Columns.Add("supplierName"); //接口商
                table.Columns.Add("difftime");

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    table.Rows[i]["orderTypeName"] = Enum.GetName(typeof(OrderTypeEnum), table.Rows[i]["ordertype"]);
                    table.Rows[i]["supplierName"] = getsupplierName(table.Rows[i]["supplierId"]);
                    table.Rows[i]["difftime"] = this.GetDifftime(Convert.ToInt32(table.Rows[i]["userid"]), table.Rows[i]["completetime"]);
                }
            }

            if (table.Rows.Count > 0)
            {
                DataTable table1 = set.Tables[1];

                table.Columns.Remove("notifycontext");
                json.AddToJson("data", table);
                json.AddToJson("count", table1);
                json.AddToJson("index", page);
                json.AddToJson("success", true);
            }
            else
            {
                json.AddToJson("success", false);
            }

            //context.Response.Write(wheres);

            context.Response.Write(json.ToString());
        }

        public double GetDifftime(int userId, object completeTime)
        {
            DateTime minValue = DateTime.MinValue;
            if (UserAccessTime.GetModel(userId) == null)
            {
                return 1000.0;
            }
            DateTime? nullable = new DateTime?(UserAccessTime.GetModel(userId).lastAccesstime);
            if (nullable.HasValue)
            {
                minValue = nullable.Value;
            }
            return Convert.ToDateTime(completeTime == DBNull.Value ? DateTime.Now : completeTime).Subtract(minValue).TotalMinutes;
        }
        protected string getsupplierName(object obj)
        {
            if ((obj == DBNull.Value) || (obj == null))
            {
                return string.Empty;
            }
            return SysSupplierFactory.GetSupplierModelByCode(int.Parse(obj.ToString())).SupplierName;
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
