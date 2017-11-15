namespace KuaiCard.WebUI.User.verify
{
    using KuaiCard.BLL.User;
    using KuaiCard.WebComponents.Web;
    using KuaiCardLib.Text;
    using System;
    using System.Data;
    using System.Web.UI.WebControls;

    public class index : UserPageBase
    {
        protected string id = string.Empty;
        protected string LabelBank = string.Empty;

        protected Repeater rptList;
        protected string gTotalCount = "0";

        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet list = UserPayBankApp.GetList("userid=" + base.currentUser.ID);
            this.rptList.DataSource = list.Tables[0];
            this.rptList.DataBind();

            this.gTotalCount = list.Tables[0].Rows.Count.ToString();

            this.DataBind();
        }

        public string MarkString(string p_src)
        {
            return Strings.Mark(p_src);
        }
    }
}

