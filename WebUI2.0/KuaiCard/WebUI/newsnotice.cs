namespace OriginalStudio.WebUI
{
    using OriginalStudio.BLL;
    using OriginalStudio.Model;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Web.UI;
    using OriginalStudio.Model.News;
    using OriginalStudio.BLL.News;

    public class newsnotice : Page
    {
        public NewsInfo _ItemInfo = null;
        protected string newscontenct = "";
        protected string newsid = "";
        protected string newstime = "";
        protected string newstitle = "";
        protected string newstype = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack && (this.ItemInfo != null))
            {
                base.Title = this.ItemInfo.newstitle;
                if (this.ItemInfo != null)
                {
                    if (this.ItemInfo.newstype == 1)
                    {
                        this.newstype = "站点新闻";
                    }
                    if (this.ItemInfo.newstype == 2)
                    {
                        this.newstype = "会员公告";
                    }
                    if (this.ItemInfo.newstype == 3)
                    {
                        this.newstype = "会员新闻";
                    }
                    if (this.ItemInfo.newstype == 4)
                    {
                        this.newstype = "站点公告";
                    }
                    if (this.ItemInfo.newstype == 6)
                    {
                        this.newstype = "行业新闻";
                    }
                    this.newstime = this.ItemInfo.addTime.ToString("yyyy年MM月dd日");
                    this.newstitle = base.Server.HtmlDecode(this.ItemInfo.newstitle);
                    this.newscontenct = this.ItemInfo.newscontent;
                    this.newsid = this.ItemInfo.newstype.ToString();
                }
            }
        }

        public NewsInfo ItemInfo
        {
            get
            {
                if ((this._ItemInfo == null) && (this.ItemInfoId > 0))
                {
                    this._ItemInfo = NewsFactory.GetCacheModel(this.ItemInfoId);
                }
                return this._ItemInfo;
            }
        }

        public int ItemInfoId
        {
            get
            {
                return WebBase.GetQueryStringInt32("id", 0);
            }
        }
    }
}

