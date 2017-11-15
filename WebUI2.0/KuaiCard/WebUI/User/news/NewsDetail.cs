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
using KuaiCard.Model;

    public class NewsDetail : System.Web.UI.Page
	{
        protected HtmlForm form1;
        protected AspNetPager PagerFooter;
        protected Repeater rptList;

        protected string NewsTitle { get; set; }
        protected string NewsTime { get; set; }
        protected string NewsContent { get; set; }


        protected int NewsID
        {
            get
            {
                return WebBase.GetQueryStringInt32("ID", -1);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ShowDetail();
            }
        }


        private void ShowDetail()
        {
            NewsInfo m = NewsFactory.GetCacheModel(this.NewsID);

            this.NewsTitle = m.newstitle;
            this.NewsTime = m.addTime.ToShortDateString();
            this.NewsContent = m.newscontent;

            this.DataBind();
        }
	}
}
