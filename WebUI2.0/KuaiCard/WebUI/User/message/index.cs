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

    public class index : UserPageBase
    {
        protected string classa = string.Empty;
        protected Repeater msg_data;
        protected AspNetPager PagerAll;

        protected Repeater msg_unread_data;
        protected AspNetPager PagerUnRead;

        protected Repeater msg_read_data;
        protected AspNetPager PagerRead;

        private void LoadAllData()
        {
            List<SearchParam> searchParams = new List<SearchParam>();
            searchParams.Add(new SearchParam("msg_to", base.UserId));
            DataSet set = IMSGFactory.PageSearch(searchParams, this.PagerAll.PageSize, this.PagerAll.CurrentPageIndex, string.Empty);
            this.PagerAll.RecordCount = Convert.ToInt32(set.Tables[0].Rows[0][0]);
            this.msg_data.DataSource = set.Tables[1];
            this.msg_data.DataBind();
            DataTable table = set.Tables[1];
            if ((table != null) && (table.Rows.Count > 0))
            {
                foreach (DataRow row in table.Rows)
                {
                    if (IMSGFactory.IsRead(int.Parse(row["id"].ToString())))
                    {
                        this.classa = "";
                    }
                    else
                    {
                        this.classa = "message-read";
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.LoadAllData();

                this.LoadUnReadData();

                this.LoadReadData();
            }
        }

        protected void PagerAll_PageChanged(object sender, EventArgs e)
        {
            this.LoadAllData();
        }

        protected void rptfeedback_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }

        #region 未读消息

        private void LoadUnReadData()
        {
            List<SearchParam> searchParams = new List<SearchParam>();
            searchParams.Add(new SearchParam("msg_to", base.UserId));
            searchParams.Add(new SearchParam("IsRead", 0));
            DataSet set = IMSGFactory.PageSearch(searchParams, this.PagerUnRead.PageSize, this.PagerUnRead.CurrentPageIndex, string.Empty);
            this.PagerUnRead.RecordCount = Convert.ToInt32(set.Tables[0].Rows[0][0]);
            this.msg_unread_data.DataSource = set.Tables[1];
            this.msg_unread_data.DataBind();
        }

        protected void PagerUnRead_PageChanged(object sender, EventArgs e)
        {
            this.LoadUnReadData();
        }
        
        #endregion

        #region 已读消息

        private void LoadReadData()
        {
            List<SearchParam> searchParams = new List<SearchParam>();
            searchParams.Add(new SearchParam("msg_to", base.UserId));
            searchParams.Add(new SearchParam("IsRead", 1));
            DataSet set = IMSGFactory.PageSearch(searchParams, this.PagerRead.PageSize, this.PagerRead.CurrentPageIndex, string.Empty);
            this.PagerRead.RecordCount = Convert.ToInt32(set.Tables[0].Rows[0][0]);
            this.msg_read_data.DataSource = set.Tables[1];
            this.msg_read_data.DataBind();
        }

        protected void PagerRead_PageChanged(object sender, EventArgs e)
        {
            this.LoadReadData();
        }
        
        #endregion
    }
}

