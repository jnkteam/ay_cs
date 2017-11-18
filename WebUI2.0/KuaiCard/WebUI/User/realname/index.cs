namespace OriginalStudio.WebUI.User.realname
{
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Text;
    using System;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class index : UserPageBase
    {
        protected string fanmian = string.Empty;
        protected string id = string.Empty;
        protected Label img;
        protected HtmlAnchor img1;
        protected HtmlAnchor img2;
        protected Label Labeladd;
        protected string name = string.Empty;
        protected string shenfenzhenghao = string.Empty;
        protected string shijan = string.Empty;
        protected string zhengmian = string.Empty;
        protected string zhuangtai = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                if (base.CurrentUser.IsRealNamePass == 0)
                {
                    HttpContext.Current.Response.Redirect("/user/realname/add.aspx");
                }
                else
                {
                    this.id = base.CurrentUser.ID.ToString();
                    this.name = base.CurrentUser.full_name;
                    this.zhengmian = base.CurrentUser.frontPic;     //"/Upload" + 
                    this.fanmian = base.CurrentUser.versoPic;       //"/Upload" + 
                    this.shenfenzhenghao = Strings.Mark(base.CurrentUser.IdCard);
                    this.shijan = base.CurrentUser.msn;
                    this.img1.Attributes["href"] = base.CurrentUser.frontPic;
                    this.img2.Attributes["href"] = base.CurrentUser.versoPic;
                    if (base.CurrentUser.IsRealNamePass == 1)
                    {
                        this.zhuangtai = "已认证";
                        this.img.Text = "<img src=\"/skin/user/images/auditok.png\" />";
                    }
                    if (base.CurrentUser.IsRealNamePass == 2)
                    {
                        this.zhuangtai = "审核中";
                        this.img.Text = "<img src=\"/skin/user/images/auditing.png\" />";
                    }
                    if (base.CurrentUser.IsRealNamePass == 3)
                    {
                        this.zhuangtai = "审核失败";
                        this.img.Text = "<img src=\"/skin/user/images/auditing.png\" />";
                        this.Labeladd.Text = "<font style=\"padding-left:10px\">(<a href=\"/user/verify/add.aspx\">去重新认证</a>)</font>";
                    }
                }
            }
        }
    }
}

