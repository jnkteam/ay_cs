namespace OriginalStudio.WebUI.Manage.User
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Payment;
    using OriginalStudio.Model;
    using OriginalStudio.Model.PayRate;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Text;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using OriginalStudio.BLL.PayRate;
    using System.Data;
    using OriginalStudio.BLL.Channel;
    using System.Collections;

    public class PayRate : ManagePageBase
    {
        public SysPayRateInfo _model = null;
        
        protected HtmlForm form1;
        protected Repeater repRate;
        protected Repeater channelTypeNameRep;
        protected DataTable tableChannelType;
        protected RadioButtonList rbl_type;


        protected void Page_Load(object sender, EventArgs e)
        {
            //ManageFactory.CheckSecondPwd();
            this.setPower();
            if (!base.IsPostBack)
            {
                this.ShowInfo();
                
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

        private void ShowInfo()
        {
            DataTable tableSysPayRate  = SysPayRateFactory.GetAllList();


            this.tableChannelType = SysChannelType.GetList(true).Tables[0];
            channelTypeNameRep.DataSource = this.tableChannelType;
            channelTypeNameRep.DataBind();
            this.repRate.DataSource = tableSysPayRate;
            this.repRate.DataBind();
           
        }

        protected void repRate_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater channelTypeRep = e.Item.FindControl("channelType") as Repeater;
                DataRowView rowRate = (DataRowView)e.Item.DataItem;
              
                string typeName = Enum.Parse(typeof(RateTypeEnum), rowRate["RateType"].ToString(), true).ToString();
                Label label = e.Item.FindControl("ratetypename") as Label;
                label.Text = typeName;
                DataTable tableChannelType = SysChannelType.GetList(true).Tables[0];

                ArrayList listChannelTypeRate = new ArrayList();
                foreach (DataRow row in tableChannelType.Rows)
                {
                    
                    decimal speRate = SysPayRateFactory.GetSysChannelTypePayRate(Convert.ToInt32(rowRate["id"]), Convert.ToInt32(row["typeid"]));
                    if (speRate >= 0)
                    {
                        listChannelTypeRate.Add(speRate * 100);
                    }
                    else {
                        listChannelTypeRate.Add(0);
                    }
                    


                }

                channelTypeRep.DataSource = listChannelTypeRate;
                channelTypeRep.DataBind();
            }

        }




    }
}

