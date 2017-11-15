namespace KuaiCard.WebUI.News
{
    using KuaiCard.BLL;
    using KuaiCard.BLL.User;
    using KuaiCard.Model.User;
    using KuaiCardLib.Web;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Wuqi.Webdiyer;
    using System.Data;
    using System.Collections.Generic;
    using KuaiCardLib.Data;

    public class NewsList : System.Web.UI.Page
	{
        protected HtmlForm form1;
        protected AspNetPager PagerFooter;
        protected Repeater rptList;

        protected int currPage
        {
            get
            {
                return WebBase.GetQueryStringInt32("currpage", -1);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
                this.PagerFooter.CurrentPageIndex = 1;

            LoadData();
        }

        protected void PagerFooter_PageChanged(object sender, EventArgs e)
        {
            LoadData();
        }


        private void LoadData()
        {
            List<SearchParam> searchParams = new List<SearchParam>();
            searchParams.Add(new SearchParam("release", 1));
            string orderby = string.Empty;
            DataSet set = NewsFactory.PageSearch(searchParams, this.PagerFooter.PageSize, this.PagerFooter.CurrentPageIndex, orderby);
            this.PagerFooter.RecordCount = Convert.ToInt32(set.Tables[0].Rows[0][0]);
            this.rptList.DataSource = set.Tables[1];
            this.rptList.DataBind();
            if (this.currPage > -1)
            {
                this.PagerFooter.CurrentPageIndex = this.currPage;
            }
        }
	}
}
