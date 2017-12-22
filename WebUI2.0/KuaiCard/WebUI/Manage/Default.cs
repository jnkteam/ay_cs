namespace OriginalStudio.WebUI.Manage
{
    using System;
    using System.Web.UI;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.TimeControl;
    using System.Data;
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Stat;

    public class Default : ManagePageBase
    {
        public string[] 合计 = {"0","0","0","0"};
        public string[] 网银 = { "0", "0", "0", "0" };
        public string[] 支付宝 = { "0", "0", "0", "0" };
        public string[] 微信 = { "0", "0", "0", "0" };

        protected DataTable menuTable;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
            this.DefaultThemes = this.currentManage.DefaultThemes;


            DateTime start = DateTime.Now.AddMonths(-1); //正式上线 需要把次参数改为DateTime.Today
            DateTime end = DateTime.Now;
            DataTable tableInfo = OrderReport.统计通道类型总数金额利润(start, end).Tables[0];
                foreach (DataRow row in tableInfo.Rows)
                {

                if (row["通道类型"].ToString() == "合计") {
                    if (!string.IsNullOrEmpty(row["订单总数"].ToString())){
                        合计[0] = row["订单总数"].ToString();
                    }
                    if (!string.IsNullOrEmpty(row["成功订单数"].ToString()))
                    {
                        合计[1] = row["成功订单数"].ToString();
                    }
                    if (!string.IsNullOrEmpty(row["订单总额"].ToString()))
                    {
                        合计[2] = row["订单总额"].ToString();
                    }
                    if (!string.IsNullOrEmpty(row["利润"].ToString()))
                    {
                        合计[3] = row["利润"].ToString();
                    }

                }
                else if (row["通道类型"].ToString() == "网银") {
                    if (!string.IsNullOrEmpty(row["订单总数"].ToString()))
                    {
                        网银[0] = row["订单总数"].ToString();
                    }
                    if (!string.IsNullOrEmpty(row["成功订单数"].ToString()))
                    {
                        网银[1] = row["成功订单数"].ToString();
                    }
                    if (!string.IsNullOrEmpty(row["订单总额"].ToString()))
                    {
                        网银[2] = row["订单总额"].ToString();
                    }
                    if (!string.IsNullOrEmpty(row["利润"].ToString()))
                    {
                        网银[3] = row["利润"].ToString();
                    }
                }
                else if (row["通道类型"].ToString() == "支付宝")
                {
                    if (!string.IsNullOrEmpty(row["订单总数"].ToString()))
                    {
                        支付宝[0] = row["订单总数"].ToString();
                    }
                    if (!string.IsNullOrEmpty(row["成功订单数"].ToString()))
                    {
                        支付宝[1] = row["成功订单数"].ToString();
                    }
                    if (!string.IsNullOrEmpty(row["订单总额"].ToString()))
                    {
                        支付宝[2] = row["订单总额"].ToString();
                    }
                    if (!string.IsNullOrEmpty(row["利润"].ToString()))
                    {
                        支付宝[3] = row["利润"].ToString();
                    }
                }
                else if (row["通道类型"].ToString() == "微信")
                {
                    if (!string.IsNullOrEmpty(row["订单总数"].ToString()))
                    {
                        微信[0] = row["订单总数"].ToString();
                    }
                    if (!string.IsNullOrEmpty(row["成功订单数"].ToString()))
                    {
                        微信[1] = row["成功订单数"].ToString();
                    }
                    if (!string.IsNullOrEmpty(row["订单总额"].ToString()))
                    {
                        微信[2] = row["订单总额"].ToString();
                    }
                    if (!string.IsNullOrEmpty(row["利润"].ToString()))
                    {
                        微信[3] = row["利润"].ToString();
                    }
                }

            }
                
            



            this.DataBind();
            
        }
    }
}

