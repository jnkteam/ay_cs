namespace KuaiCard.WebUI.Manage
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Withdraw;
    using OriginalStudio.ETAPI;
    using OriginalStudio.Model;
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

    public class settledAgentSummarys : ManagePageBase
    {
        protected settledAgentSummary _bll = new settledAgentSummary();
        protected Button btnallfail;
        protected Button btnAllPass;
        protected Button btnPass;
        protected Button btnSearch;
        protected DropDownList ddlaudit_status;
        protected DropDownList ddlstatus;
        protected TextBox EtimeBox;
        protected HtmlForm form1;
        protected HtmlHead Head1;
        protected AspNetPager Pager1;
        protected Repeater rptApply;
        protected TextBox StimeBox;
        protected settledAgent stlAgtBLL = new settledAgent();
        protected TextBox txtLotno;
        protected TextBox txtUserId;

        private void BindData()
        {
            List<SearchParam> searchParams = new List<SearchParam>();
            int result = 0;
            if (!(string.IsNullOrEmpty(this.txtUserId.Text.Trim()) || !int.TryParse(this.txtUserId.Text.Trim(), out result)))
            {
                searchParams.Add(new SearchParam("userid", result));
            }
            if (!string.IsNullOrEmpty(this.txtLotno.Text.Trim()))
            {
                searchParams.Add(new SearchParam("lotno", this.txtLotno.Text.Trim()));
            }
            if (!(string.IsNullOrEmpty(this.ddlstatus.Text.Trim()) || !int.TryParse(this.ddlstatus.Text.Trim(), out result)))
            {
                searchParams.Add(new SearchParam("status", result));
            }
            DateTime minValue = DateTime.MinValue;
            if ((!string.IsNullOrEmpty(this.StimeBox.Text.Trim()) && DateTime.TryParse(this.StimeBox.Text.Trim(), out minValue)) && (minValue > DateTime.MinValue))
            {
                searchParams.Add(new SearchParam("saddtime", this.StimeBox.Text.Trim()));
            }
            if ((!string.IsNullOrEmpty(this.EtimeBox.Text.Trim()) && DateTime.TryParse(this.EtimeBox.Text.Trim(), out minValue)) && (minValue > DateTime.MinValue))
            {
                searchParams.Add(new SearchParam("eaddtime", minValue.AddDays(1.0)));
            }
            DataSet set = this._bll.PageSearch(searchParams, this.Pager1.PageSize, this.Pager1.CurrentPageIndex, string.Empty, false);
            this.Pager1.RecordCount = Convert.ToInt32(set.Tables[0].Rows[0][0]);
            DataTable table = set.Tables[1];
            this.rptApply.DataSource = table;
            this.rptApply.DataBind();
        }

        protected void btnallfail_Click(object sender, EventArgs e)
        {
            if (SettledFactory.Allfails())
            {
                base.AlertAndRedirect("操作成功!");
                this.BindData();
            }
            else
            {
                base.AlertAndRedirect("操作失败!");
            }
        }

        protected void btnAllPass_Click(object sender, EventArgs e)
        {
            string batchNo = Guid.NewGuid().ToString("N");
            if (SettledFactory.AllPass(batchNo))
            {
                DataTable listWithdrawByApi = SettledFactory.GetListWithdrawByApi(batchNo);
                if ((listWithdrawByApi != null) && (listWithdrawByApi.Rows.Count > 0))
                {
                    List<SettledInfo> list = SettledFactory.DataTableToList(listWithdrawByApi);
                    foreach (SettledInfo info in list)
                    {
                        KuaiCard.ETAPI.Withdraw.InitDistribution(info);
                    }
                }
                base.AlertAndRedirect("审核成功!");
                this.BindData();
            }
            else
            {
                base.AlertAndRedirect("审核失败!");
            }
        }

        protected void btnPass_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["ischecked"];
            DataTable withdrawListByApi = null;
            string batchNo = Guid.NewGuid().ToString("N");
            if (!string.IsNullOrEmpty(str))
            {
                if (SettledFactory.BatchPass(str, batchNo, out withdrawListByApi))
                {
                    if ((withdrawListByApi != null) && (withdrawListByApi.Rows.Count > 0))
                    {
                        List<SettledInfo> list = SettledFactory.DataTableToList(withdrawListByApi);
                        foreach (SettledInfo info in list)
                        {
                            KuaiCard.ETAPI.Withdraw.InitDistribution(info);
                        }
                    }
                    base.AlertAndRedirect("审核成功!");
                    this.BindData();
                }
                else
                {
                    base.AlertAndRedirect("审核失败!");
                }
            }
            else
            {
                base.AlertAndRedirect("请选择要审核的申请!");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        protected string GetTranApiName(object obj)
        {
            if ((obj == null) || (obj == DBNull.Value))
            {
                return "不走接口";
            }
            if (Convert.ToInt32(obj) == 100)
            {
                return "财付通";
            }
            return "";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            if (!base.IsPostBack)
            {
                this.StimeBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.EtimeBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.StimeBox.Attributes.Add("onFocus", "WdatePicker()");
                this.EtimeBox.Attributes.Add("onFocus", "WdatePicker()");
                this.BindData();
            }
        }

        protected void Pager1_PageChanged(object sender, EventArgs e)
        {
            this.BindData();
        }

        protected void Pager1_PageChanging(object src, PageChangingEventArgs e)
        {
            this.BindData();
        }

        protected void rptApply_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string str = e.CommandArgument.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    int status = -1;
                    if (e.CommandName == "Pass")
                    {
                        status = 2;
                    }
                    else if (e.CommandName == "Refuse")
                    {
                        status = 4;
                    }
                    if (((status != -1) && SettledFactory.Audit(int.Parse(str), status)) && (status == 2))
                    {
                        SettledInfo model = SettledFactory.GetModel(int.Parse(str));
                        if (model.status == SettledStatus.付款接口支付中)
                        {
                            KuaiCard.ETAPI.Withdraw.InitDistribution(model);
                        }
                    }
                }
            }
            this.BindData();
        }

        protected void rptApply_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                HtmlTableRow row = e.Item.FindControl("tr_detail") as HtmlTableRow;
                Literal literal = e.Item.FindControl("litimg") as Literal;
                literal.Text = string.Format("<img src=\"../style/images/folder_close.gif\" style=\"cursor: hand\" onclick=\"collapse(this, '{0}')\" alt=\"\" />", row.ClientID);
                string str = DataBinder.Eval(e.Item.DataItem, "lotno").ToString();
                string str2 = DataBinder.Eval(e.Item.DataItem, "status").ToString();
                Repeater repeater = (Repeater) e.Item.FindControl("rptList");
                DataSet list = this.stlAgtBLL.GetList("lotno=" + str);
                repeater.DataSource = list;
                repeater.DataBind();
            }
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
        }

        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
            }
        }

        private void setPower()
        {
            if (!ManageFactory.CheckCurrentPermission(false, ManageRole.Financial))
            {
                base.Response.Write("Sorry,No authority!");
                base.Response.End();
            }
        }

        public int ItemInfoId
        {
            get
            {
                return WebBase.GetQueryStringInt32("ID", 0);
            }
        }

        public int ItemInfoStatus
        {
            get
            {
                return WebBase.GetQueryStringInt32("status", 0);
            }
        }
    }
}

