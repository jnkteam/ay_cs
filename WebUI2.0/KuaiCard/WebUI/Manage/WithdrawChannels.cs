﻿namespace OriginalStudio.WebUI.Manage
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Supplier;
    using OriginalStudio.BLL.Withdraw;
    using OriginalStudio.Model;
    using OriginalStudio.Model.Withdraw;
    using OriginalStudio.WebComponents.Web;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class WithdrawChannels : ManagePageBase
    {
        private OriginalStudio.BLL.Withdraw.ChannelWithdraw chnlsBLL = new OriginalStudio.BLL.Withdraw.ChannelWithdraw();
        protected HtmlForm Form1;
        protected Repeater rptChnls;

        private void LoadData()
        {
            DataSet allList = this.chnlsBLL.GetAllList();
            this.rptChnls.DataSource = allList;
            this.rptChnls.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //ManageFactory.CheckSecondPwd();
            this.setPower();
            if (!base.IsPostBack)
            {
                this.LoadData();
            }
        }

        protected void rptChnls_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DropDownList list = e.Item.FindControl("ddlsupp") as DropDownList;
                int id = Convert.ToInt32(e.CommandArgument);
                OriginalStudio.Model.Withdraw.ChannelWithdraw model = this.chnlsBLL.GetModel(id);
                model.supplier = int.Parse(list.SelectedValue);
                this.chnlsBLL.Update(model);
                this.LoadData();
            }
        }

        protected void rptChnls_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DropDownList ddl = e.Item.FindControl("ddlsupp") as DropDownList;
                string dfval = string.Empty;
                object obj2 = DataBinder.Eval(e.Item.DataItem, "supplier");
                if (obj2 != DBNull.Value)
                {
                    dfval = obj2.ToString();
                }
                this.SupBind(ddl, dfval);
            }
        }

        private void setPower()
        {
            if (!ManageFactory.CheckCurrentPermission(false, ManageRole.Interfaces))
            {
                base.Response.Write("Sorry,No authority!");
                base.Response.End();
            }
        }

        private void SupBind(DropDownList ddl, string dfval)
        {
            DataTable table = SysSupplierFactory.GetList("isdistribution=1").Tables[0];
            ddl.Items.Add(new ListItem("--请选择--", "0"));
            foreach (DataRow row in table.Rows)
            {
                ListItem item = new ListItem(row["name"].ToString(), row["code"].ToString());
                if (row["code"].ToString() == dfval)
                {
                    item.Selected = true;
                }
                ddl.Items.Add(item);
            }
        }
    }
}

