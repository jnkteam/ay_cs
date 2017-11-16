namespace OriginalStudio.WebUI
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.News;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Wuqi.Webdiyer;

    public class news : Page
    {
        protected HtmlAnchor guanfang;
        protected HtmlAnchor hangye;
        protected HtmlAnchor huiyuan;
        protected AspNetPager Pager1;
        protected HtmlAnchor quanbu;
        protected Repeater rptNews1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.ItemInfoId.ToString() == "")
            {
                this.quanbu.Attributes["class"] = "news-tabs-link news-tab-current";
                this.guanfang.Attributes["class"] = "news-tabs-link";
                this.huiyuan.Attributes["class"] = "news-tabs-link";
                this.hangye.Attributes["class"] = "news-tabs-link";
            }
            if (this.ItemInfoId == "4")
            {
                this.quanbu.Attributes["class"] = "news-tabs-link";
                this.guanfang.Attributes["class"] = "news-tabs-link news-tab-current";
                this.huiyuan.Attributes["class"] = "news-tabs-link";
                this.hangye.Attributes["class"] = "news-tabs-link";
            }
            if (this.ItemInfoId == "2")
            {
                this.quanbu.Attributes["class"] = "news-tabs-link";
                this.guanfang.Attributes["class"] = "news-tabs-link";
                this.huiyuan.Attributes["class"] = "news-tabs-link news-tab-current";
                this.hangye.Attributes["class"] = "news-tabs-link";
            }
            if (this.ItemInfoId == "1")
            {
                this.quanbu.Attributes["class"] = "news-tabs-link";
                this.guanfang.Attributes["class"] = "news-tabs-link";
                this.huiyuan.Attributes["class"] = "news-tabs-link";
                this.hangye.Attributes["class"] = "news-tabs-link news-tab-current";
            }
            List<SearchParam> searchParams = new List<SearchParam>();
            if (this.ItemInfoId.ToString() != "")
            {
                searchParams.Add(new SearchParam("newstype", int.Parse(this.ItemInfoId)));
            }
            DataSet set = NewsFactory.PageSearch(searchParams, this.Pager1.PageSize, this.Pager1.CurrentPageIndex, string.Empty);
            this.Pager1.RecordCount = Convert.ToInt32(set.Tables[0].Rows[0][0]);
            this.rptNews1.DataSource = set.Tables[1];
            this.rptNews1.DataBind();
        }

        public string ItemInfoId
        {
            get
            {
                return WebBase.GetQueryStringString("id", "");
            }
        }
    }
}

