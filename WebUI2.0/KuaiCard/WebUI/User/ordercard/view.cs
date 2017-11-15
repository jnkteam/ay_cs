namespace KuaiCard.WebUI.User.ordercard
{
    using KuaiCard.BLL;
    using KuaiCard.WebComponents.Web;
    using KuaiCardLib.Data;
    using KuaiCardLib.Web;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI.WebControls;

    public class view : UserPageBase
    {
        protected Repeater rptOrders;

        private void LoadData()
        {
            if (!string.IsNullOrEmpty(this.orderid))
            {
                List<SearchParam> searchParams = new List<SearchParam>();
                searchParams.Add(new SearchParam("userid", base.UserId));
                searchParams.Add(new SearchParam("orderid", this.orderid));
                string orderby = string.Empty;
                DataSet set = new OrderCard().PageSearch(searchParams, 20, 1, orderby);
                DataTable table = set.Tables[2];
                this.rptOrders.DataSource = set.Tables[1];
                this.rptOrders.DataBind();
            }
            else
            {
                base.AlertAndRedirect("非法访问", "User/order/");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.LoadData();
            }
        }

        public string orderid
        {
            get
            {
                return WebBase.GetQueryStringString("orderid", string.Empty);
            }
        }
    }
}

