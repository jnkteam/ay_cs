namespace OriginalStudio.WebUI.Manage.Order
{
    using OriginalStudio.BLL;
    using OriginalStudio.Model;
    using OriginalStudio.WebComponents.Web;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class Reconciliation : ManagePageBase
    {
        protected Button btn_search;
        protected HtmlForm form1;
        protected TextBox txtorders;
        protected TextBox txtResult;

        protected void btn_search_Click(object sender, EventArgs e)
        {
            string orderId = this.txtorders.Text.Trim();
            if (string.IsNullOrEmpty(orderId))
            {
                base.AlertAndRedirect("请输入订单号！");
                return;
            }

            string InterfaceKey = Lib.SysConfig.RuntimeSetting.GetKeyValue("InterfaceKey", "");
            string checkSign = Lib.Security.Cryptography.MD5(orderId + InterfaceKey).ToUpper();

            string gateSrv = OriginalStudio.Lib.SysConfig.RuntimeSetting.GateWayServer;
            if (gateSrv == "")
            {
                base.AlertAndRedirect("配置查单网关地址！");
                return;
            }
            string payUrl = gateSrv + "/SysSearchSupplierOrder.ashx";

            payUrl = payUrl + "?orderid=" + orderId + "&sign=" + checkSign;

            string invokeResult = Lib.Web.WebClientHelper.GetString(payUrl, "", "get", Encoding.UTF8, 10000);

            this.txtResult.Text = invokeResult;

            //返回json
            Dictionary<string, string> dicRtn = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(invokeResult);
            StringBuilder sb = new StringBuilder();
            if (dicRtn["result"].ToString() == "SUCCESS")
            {
                sb.Append("结果:" + dicRtn["result"].ToString()).Append("；")
                                .Append("上游单号:" + dicRtn["supplierorder"].ToString()).Append("；")
                                .Append("支付金额:" + dicRtn["payamt"].ToString()).Append("；")
                                .Append("状态:" + dicRtn["msg"].ToString());
            }
            else
            {
                sb.Append("结果:" + dicRtn["result"].ToString()).Append("；").Append("状态:" + dicRtn["msg"].ToString());
            }
            this.txtResult.Text = sb.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            if (!base.IsPostBack)
            {
            }
        }

        private void setPower()
        {
            if (!ManageFactory.CheckCurrentPermission(false, ManageRole.System))
            {
                base.Response.Write("Sorry,No authority!");
                base.Response.End();
            }
        }
    }
}

