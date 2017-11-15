namespace KuaiCard.WebUI.User.realname
{
    using KuaiCard.WebComponents.Web;
    using KuaiCardLib.Text;
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
                if (base.currentUser.IsRealNamePass == 0)
                {
                    HttpContext.Current.Response.Redirect("/user/realname/add.aspx");
                }
                else
                {
                    this.id = base.currentUser.ID.ToString();
                    this.name = base.currentUser.full_name;
                    this.zhengmian = base.currentUser.frontPic;     //"/Upload" + 
                    this.fanmian = base.currentUser.versoPic;       //"/Upload" + 
                    this.shenfenzhenghao = Strings.Mark(base.currentUser.IdCard);
                    this.shijan = base.currentUser.msn;
                    this.img1.Attributes["href"] = base.currentUser.frontPic;
                    this.img2.Attributes["href"] = base.currentUser.versoPic;
                    if (base.currentUser.IsRealNamePass == 1)
                    {
                        this.zhuangtai = "已认证";
                        this.img.Text = "<img src=\"/skin/user/images/auditok.png\" />";
                    }
                    if (base.currentUser.IsRealNamePass == 2)
                    {
                        this.zhuangtai = "审核中";
                        this.img.Text = "<img src=\"/skin/user/images/auditing.png\" />";
                    }
                    if (base.currentUser.IsRealNamePass == 3)
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

