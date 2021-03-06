﻿namespace OriginalStudio.WebUI.Manage.User
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Settled;
    using OriginalStudio.Model;
    using OriginalStudio.Model.User;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Wuqi.Webdiyer;
    using OriginalStudio.Model.Trade;

    public class UserIdImgLists : ManagePageBase
    {
        protected Button btnCashTo;
        protected Button btnDelete;
        protected Button btnSearch;
        protected TextBox EtimeBox;
        protected HtmlForm form1;
        protected HtmlHead Head1;
        protected AspNetPager Pager1;
        protected Repeater rptIamges;
        protected HtmlInputHidden selectedUsers;
        protected DropDownList StatusList;
        protected TextBox StimeBox;
        protected TextBox txtUserId;
        protected TextBox txtUserName;

        protected TextBox merchantName;
        protected DropDownList status;


        protected void btnCashTo_Click(object sender, EventArgs e)
        {
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["chkItem"];
            foreach (string str2 in str.Split(new char[] { ',' }))
            {
                new usersIdImage().Delete(int.Parse(str2));
            }
            this.LoadData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }

        private void DoCmd()
        {
            if ((!string.IsNullOrEmpty(this.cmd) && (this.ItemID > 0)) && (this.UserId > 0))
            {
                MchUserImageInfo model = new MchUserImageInfo();

                model.ID = this.ItemID;
                if (this.cmd == "ok")
                {
                    model.Status = IdImageStatusEnum.审核成功;
                }
                if (this.cmd == "fail")
                {
                    model.Status = IdImageStatusEnum.审核失败;
                }
                //model.why = string.Empty;
                model.CheckTime = DateTime.Now;
                model.CheckUser = base.ManageId;
                model.UserID = this.UserId;
                

                if (MchUsersImageFactory.CheckUserImage(this.ItemID, base.ManageId, Convert.ToInt32(model.Status)) > 0)
                {
                    base.AlertAndRedirect("操作成功", "UserIdImgList.aspx?s=1");
                }
                else
                {
                    base.AlertAndRedirect("操作失败", "UserIdImgList.aspx?s=1");
                }
            }
        }

        protected string getpassview(object obj)
        {
            if ((obj == null) || (obj == DBNull.Value))
            {
                return string.Empty;
            }
            if (Convert.ToInt32(obj) > 0)
            {
                return "√";
            }
            return "\x00d7";
        }

        private void LoadData()
        {

            string merchantName = this.merchantName.Text;
            int status = Convert.ToInt32(this.status.SelectedValue);

            string orderby = string.Empty;
            DataSet set = MchUsersImageFactory.GetUserImages(merchantName, status);
            this.rptIamges.DataSource = set.Tables[0];
            this.rptIamges.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            this.DoCmd();
            if (!base.IsPostBack)
            {
                /*
                this.StimeBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.EtimeBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.StimeBox.Attributes.Add("onFocus", "WdatePicker()");
                this.EtimeBox.Attributes.Add("onFocus", "WdatePicker()");
                if (!string.IsNullOrEmpty(this.ItemStatus))
                {
                    this.StatusList.SelectedValue = this.ItemStatus;
                }
                */
                this.LoadData();
            }
        }

        protected void Pager1_PageChanging(object src, PageChangingEventArgs e)
        {
            this.LoadData();
        }

        public enum ImageType
        {
            unknown   = 0,
            身份证正面  = 1,
            身份证背面  = 2,
            营业执照    = 3
        }

        protected void rptUsersItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string str = DataBinder.Eval(e.Item.DataItem, "id").ToString();
                string str2 = DataBinder.Eval(e.Item.DataItem, "userid").ToString();
                string str3 = DataBinder.Eval(e.Item.DataItem, "status").ToString();
                string ImageType = DataBinder.Eval(e.Item.DataItem, "ImageType").ToString();



                string str4 = string.Empty;               
                    str4 = string.Format("<a onclick=\"return confirm('审核成功?')\" href=\"?cmd=ok&ID={0}&userid={1}\" style=\"color:#fff;\">通过</a>", str, str2);

                string str5 = string.Empty;
                str5 = string.Format("<a onclick=\"return confirm('审核失败？')\" href=\"?cmd=fail&ID={0}&userid={1}\" style=\"color:#fff;\">失败</a>", str, str2);



                string statusName = Enum.Parse(typeof(ImageStatus), str3, true).ToString();
                string imageTypeName = Enum.Parse(typeof(ImageType), ImageType, true).ToString();

                Label label = (Label) e.Item.FindControl("labagcmd");
                Label label1 = (Label)e.Item.FindControl("labagcmd1");
                Label labelStaus = (Label)e.Item.FindControl("labelStatus");
                Label labelImageTypeName = (Label)e.Item.FindControl("labelImageTypeName");


                label.Text = str4;
                label1.Text = str5;
                labelStaus.Text = statusName;
                labelImageTypeName.Text = imageTypeName;
            }
        }

        private void setPower()
        {
            if (!ManageFactory.CheckCurrentPermission(false, ManageRole.Merchant))
            {
                base.Response.Write("Sorry,No authority!");
                base.Response.End();
            }
        }

        public string cmd
        {
            get
            {
                return WebBase.GetQueryStringString("cmd", "");
            }
        }

        public int ItemID
        {
            get
            {
                return WebBase.GetQueryStringInt32("ID", 0);
            }
        }

        public string ItemStatus
        {
            get
            {
                return WebBase.GetQueryStringString("s", "");
            }
        }

        public string orderBy
        {
            get
            {
                return WebBase.GetQueryStringString("orderby", "balance");
            }
        }

        public string orderByType
        {
            get
            {
                return WebBase.GetQueryStringString("type", "asc");
            }
        }

        public int proid
        {
            get
            {
                return WebBase.GetQueryStringInt32("proid", 0);
            }
        }

        public int UserId
        {
            get
            {
                return WebBase.GetQueryStringInt32("userid", 0);
            }
        }
    }
}

