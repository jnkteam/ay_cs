namespace KuaiCard.WebUI.Manage
{
    using KuaiCard.BLL.User;
    using KuaiCard.Model.User;
    using KuaiCard.WebComponents.Web;
    using KuaiCardLib.Data;
    using KuaiCardLib.Web;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Wuqi.Webdiyer;

    public class Reviewname : ManagePageBase
    {
        protected Button btnSearch;
        protected HtmlForm form1;
        protected AspNetPager Pager1;
        protected Repeater rptUsers;
        protected HtmlInputHidden selectedUsers;
        protected DropDownList StatusList;
        protected TextBox txtuserId;

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }

        private void LoadData()
        {
            List<SearchParam> searchParams = new List<SearchParam>();
            searchParams.Add(new SearchParam("isrealnamepass", int.Parse(this.StatusList.SelectedValue)));
            if (!string.IsNullOrEmpty(this.txtuserId.Text.Trim()))
            {
                int result = 0;
                if (int.TryParse(this.txtuserId.Text.Trim(), out result))
                {
                    searchParams.Add(new SearchParam("id", result));
                }
            }
            string orderby = string.Empty;
            DataSet set = UserFactory.PageSearch(searchParams, this.Pager1.PageSize, this.Pager1.CurrentPageIndex, orderby);
            this.Pager1.RecordCount = Convert.ToInt32(set.Tables[0].Rows[0][0]);
            this.rptUsers.DataSource = set.Tables[1];
            this.rptUsers.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.LoadData();
            }
        }

        protected void Pager1_PageChanged(object sender, EventArgs e)
        {
            this.LoadData();
        }

        protected void rptApply_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string str = e.CommandArgument.ToString();
            if (!string.IsNullOrEmpty(str))
            {
                int num = 0;
                if (e.CommandName == "Pass")
                {
                    num = 1;
                }
                else if (e.CommandName == "Refuse")
                {
                    num = 3;
                }
                UserInfo model = UserFactory.GetModel(int.Parse(str));
                model.IsRealNamePass = num;
                if (UserFactory.Update1(model))
                {
                    base.AlertAndRedirect("保存成功！", "Reviewname.aspx");
                }
            }
            this.LoadData();
        }

        public string id
        {
            get
            {
                return WebBase.GetQueryStringString("id", "");
            }
        }
    }
}

