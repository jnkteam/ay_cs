namespace KuaiCard.WebUI.User.userrate
{
    using OriginalStudio.BLL.Channel;
    using OriginalStudio.BLL.Payment;
    using OriginalStudio.Model.Channel;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Data;
    using System.Web.UI.WebControls;

    public class index : UserPageBase
    {
        protected Repeater rpt_paymode;

        private void docmd()
        {
            if ((this.typeId > 0) && !string.IsNullOrEmpty(this.cmd))
            {
                ChannelTypeUserInfo model = new ChannelTypeUserInfo();
                model.userId = base.UserId;
                model.typeId = this.typeId;
                model.userIsOpen = new bool?(this.cmd != "off");
                model.addTime = new DateTime?(DateTime.Now);
                model.sysIsOpen = null;
                ChannelTypeInfo cacheModel = ChannelType.GetCacheModel(this.typeId);
                string str = cacheModel.modetypename + "已关闭";
                if (model.userIsOpen.Value)
                {
                    str = cacheModel.modetypename + "已开启";
                }
                if (ChannelTypeUsers.Add(model) > 0)
                {
                    string script = string.Format("<script type=\"text/javascript\">$(document).ready(function(){{$.dialog({{title:lktitle,resize:false,content: '{0}！',ok:function () {{location.href ='/Merchant/order/channel.aspx';return false;}},close:function () {{location.href ='/Merchant/order/channel.aspx';return false;}},icon: 'succeed',width:'250px',height:'90px',time:5}});}})</script>", str);
                    this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "msg", script);
                }
            }
        }

        private void LoadData()
        {
            DataTable table = ChannelType.GetCacheList().Copy();
            if (!table.Columns.Contains("payrate"))
            {
                table.Columns.Add("payrate", typeof(double));
            }
            if (!table.Columns.Contains("plmodestatus"))
            {
                table.Columns.Add("plmodestatus", typeof(string));
            }
            if (!table.Columns.Contains("usermodestatus"))
            {
                table.Columns.Add("usermodestatus", typeof(string));
            }
            if (!table.Columns.Contains("flag"))
            {
                table.Columns.Add("flag", typeof(string));
            }
            foreach (DataRow row in table.Rows)
            {
                int typeId = int.Parse(row["typeId"].ToString());
                bool flag = true;
                bool flag2 = false;
                ChannelTypeUserInfo cacheModel = ChannelTypeUsers.GetCacheModel(base.UserId, typeId);
                if ((cacheModel != null) && cacheModel.userIsOpen.HasValue)
                {
                    flag = cacheModel.userIsOpen.Value;
                }
                switch (ChannelType.GetCacheModel(typeId).isOpen)
                {
                    case OpenEnum.AllClose:
                        flag2 = false;
                        break;

                    case OpenEnum.AllOpen:
                        flag2 = true;
                        break;

                    case OpenEnum.Close:
                        flag2 = false;
                        if ((cacheModel != null) && cacheModel.sysIsOpen.HasValue)
                        {
                            flag2 = cacheModel.sysIsOpen.Value;
                        }
                        break;

                    case OpenEnum.Open:
                        flag2 = true;
                        if (((cacheModel != null) && cacheModel.sysIsOpen.HasValue) && cacheModel.sysIsOpen.HasValue)
                        {
                            flag2 = cacheModel.sysIsOpen.Value;
                        }
                        break;
                }
                row["payrate"] = PayRateFactory.GetUserPayRate(base.UserId, Convert.ToInt32(row["typeId"]));
                if (flag)
                {
                    row["usermodestatus"] = "right";
                    row["flag"] = string.Format("<button class=\"button green\" disabled=\"disabled\" type=\"button\" onclick=\"javascipt:switchstate('on','{1}','{0}')\">\r\n                                启用</button>\r\n                            <button class=\"button\" type=\"button\" onclick=\"javascipt:switchstate('off','{1}','{0}')\"\r\n                                style=\"margin-right: 0\">\r\n                                关闭</button>", row["typeid"], base.UserId);
                }
                else
                {
                    row["usermodestatus"] = "wrong";
                    row["flag"] = string.Format("<button class=\"button\"  type=\"button\" onclick=\"javascipt:switchstate('on','{1}','{0}')\">\r\n                                启用</button>\r\n                            <button class=\"button\" type=\"button green\" disabled=\"disabled\" onclick=\"javascipt:switchstate('off','{1}','{0}')\"\r\n                                style=\"margin-right: 0\">\r\n                                关闭</button>", row["typeid"], base.UserId);
                }
                if (flag2)
                {
                    row["plmodestatus"] = "right";
                }
                else
                {
                    row["plmodestatus"] = "wrong";
                }
            }
            this.rpt_paymode.DataSource = table;
            this.rpt_paymode.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.docmd();
            if (!base.IsPostBack)
            {
                this.LoadData();
            }
        }

        public string cmd
        {
            get
            {
                return WebBase.GetQueryStringString("flag", string.Empty);
            }
        }

        public int typeId
        {
            get
            {
                return WebBase.GetQueryStringInt32("id", 0);
            }
        }
    }
}

