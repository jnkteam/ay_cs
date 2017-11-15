namespace KuaiCard.WebUI.User.message
{
    using KuaiCard.BLL;
    using KuaiCard.WebComponents.Web;
    using KuaiCardLib.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI.WebControls;
    using Wuqi.Webdiyer;

    public class unread : UserPageBase
    {
        protected string classa = string.Empty;
        protected Repeater msg_data;
        protected AspNetPager Pager1;

        private void LoadData()
        {
            List<SearchParam> searchParams = new List<SearchParam>();
            searchParams.Add(new SearchParam("msg_to", base.UserId));
            searchParams.Add(new SearchParam("IsRead", 0));
            DataSet set = IMSGFactory.PageSearch(searchParams, this.Pager1.PageSize, this.Pager1.CurrentPageIndex, string.Empty);
            this.Pager1.RecordCount = Convert.ToInt32(set.Tables[0].Rows[0][0]);
            this.msg_data.DataSource = set.Tables[1];
            this.msg_data.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                base.Title = base.getUserTitle("站内消息");
                this.LoadData();
            }
        }

        protected void Pager1_PageChanged(object sender, EventArgs e)
        {
            this.LoadData();
        }

        protected void rptfeedback_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }
    }
}

