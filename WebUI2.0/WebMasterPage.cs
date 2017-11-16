using OriginalStudio.BLL;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using OriginalStudio.Model;
using OriginalStudio.Model.News;
using OriginalStudio.BLL.News;

public class WebMasterPage : MasterPage
{
    protected ContentPlaceHolder ContentPlaceHolder1;
    protected HtmlForm form1;
    protected ContentPlaceHolder head;
    protected HtmlHead Head1;
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
        }
    }
}

