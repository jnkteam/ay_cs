﻿namespace KuaiCard.WebUI.Manage.User
{
    using KuaiCard.BLL;
    using KuaiCard.BLL.User;
    using KuaiCard.Model;
    using KuaiCard.WebComponents.Web;
    using KuaiCardLib.Data;
    using KuaiCardLib.Web;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Wuqi.Webdiyer;

    public class UserLoginLog : ManagePageBase
    {
        protected Button btnDel;
        protected Button btnSearch;
        protected TextBox EtimeBox;
        protected HtmlForm form1;
        protected HtmlHead Head1;
        protected TextBox KeyWordBox;
        protected AspNetPager Pager1;
        protected Repeater rptUsers;
        protected DropDownList SeachType;
        protected HtmlInputHidden selectedUsers;
        protected TextBox StimeBox;
        protected string wzfmoney = string.Empty;
        protected string yzfmoney = string.Empty;

        protected void btnDel_Click(object sender, EventArgs e)
        {
            string formString = WebBase.GetFormString("chkItem", "");
            if (!string.IsNullOrEmpty(formString))
            {
                foreach (string str2 in formString.Split(new char[] { ',' }))
                {
                    int result = 0;
                    if (int.TryParse(str2, out result))
                    {
                        UserLoginLogFactory.Del(result);
                    }
                }
            }
            this.LoadData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }

        private void LoadData()
        {
            List<SearchParam> searchParams = new List<SearchParam>();
            string str = this.KeyWordBox.Text.Trim();
            if (!string.IsNullOrEmpty(this.SeachType.SelectedValue) && !string.IsNullOrEmpty(str))
            {
                if (this.SeachType.SelectedValue.ToLower() == "userid")
                {
                    int result = 0;
                    int.TryParse(str, out result);
                    searchParams.Add(new SearchParam("id", result));
                }
                else if (this.SeachType.SelectedValue == "UserName")
                {
                    searchParams.Add(new SearchParam("userName", str));
                }
            }
            if (!string.IsNullOrEmpty(this.StimeBox.Text))
            {
                searchParams.Add(new SearchParam("starttime", this.StimeBox.Text.Trim()));
            }
            if (!string.IsNullOrEmpty(this.EtimeBox.Text))
            {
                searchParams.Add(new SearchParam("endtime", Convert.ToDateTime(this.EtimeBox.Text.Trim()).AddDays(1.0)));
            }
            DataSet set = UserLoginLogFactory.PageSearch(searchParams, this.Pager1.PageSize, this.Pager1.CurrentPageIndex, string.Empty);
            this.Pager1.RecordCount = Convert.ToInt32(set.Tables[0].Rows[0][0]);
            this.rptUsers.DataSource = set.Tables[1];
            this.rptUsers.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            if (!base.IsPostBack)
            {
                this.StimeBox.Attributes.Add("onFocus", "WdatePicker()");
                this.EtimeBox.Attributes.Add("onFocus", "WdatePicker()");
                this.StimeBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.EtimeBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.LoadData();
            }
        }

        protected void Pager1_PageChanging(object src, PageChangingEventArgs e)
        {
            this.LoadData();
        }

        private void setPower()
        {
            if (!ManageFactory.CheckCurrentPermission(false, ManageRole.Merchant))
            {
                base.Response.Write("Sorry,No authority!");
                base.Response.End();
            }
        }
    }
}

