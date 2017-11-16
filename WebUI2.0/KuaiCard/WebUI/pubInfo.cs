namespace KuaiCard.WebUI
{
    using OriginalStudio.BLL;
    using OriginalStudio.Model;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class pubInfo : Page
    {
        public NewsInfo _ItemInfo = null;
        protected string newscontenct = "";
        protected string newsid = "";
        protected string newstime = "";
        protected string newstitle = "";
        protected string newstype = "";
        protected Repeater rptNews1;

        private void InitForm()
        {
            List<NewsInfo> list = NewsFactory.GetCacheList(4, 1, 6);
            if (list != null)
            {
                this.rptNews1.DataSource = list;
                this.rptNews1.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.InitForm();
                if (this.ItemInfo != null)
                {
                    base.Title = this.ItemInfo.newstitle;
                    if (this.ItemInfo != null)
                    {
                        this.newstime = this.ItemInfo.addTime.ToString("yyyy年MM月dd日");
                        this.newstitle = base.Server.HtmlDecode(this.ItemInfo.newstitle);
                        this.newscontenct = this.ItemInfo.newscontent;
                        this.newsid = this.ItemInfo.newstype.ToString();
                    }
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

