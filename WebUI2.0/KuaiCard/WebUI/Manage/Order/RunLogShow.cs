namespace OriginalStudio.WebUI.Manage.Order
{
    using OriginalStudio.Model.Sys;
    using OriginalStudio.WebComponents.Web;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class RunLogShow : ManagePageBase
    {
        protected HtmlForm form1;
        protected Label lbladdtime;
        protected Label lblbugtype;
        protected Label lbldetail;
        protected Label lblerrorcode;
        protected Label lblerrorinfo;
        protected Label lblid;
        protected Label lblurl;
        protected Label lbluserid;
        protected Label lbluserorder;
        public string strid = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack || ((base.Request.Params["id"] == null) || !(base.Request.Params["id"].Trim() != ""))))
            {
                this.strid = base.Request.Params["id"];
                int id = Convert.ToInt32(this.strid);
                this.ShowInfo(id);
            }
        }

        private void ShowInfo(int id)
        {
            OriginalStudio.Model.Sys.SysRunLog model = OriginalStudio.BLL.Sys.SysRunLogFactory.GetModel(id);
            this.lblid.Text = model.Id.ToString();
            this.lblbugtype.Text = Enum.GetName(typeof(LogTypeEnum), model.Logtype);
            this.lbluserid.Text = model.UserID.ToString();
            this.lbluserorder.Text = model.UserOrder;
            this.lblurl.Text = model.Url;
            this.lblerrorcode.Text = model.ErrorCode;
            this.lblerrorinfo.Text = model.ErrorInfo;
            if (model.Detail.IndexOf("<xml>") >= 0)
                this.lbldetail.Text = "<xmp>" + model.Detail + "</xmp>";
            else
                this.lbldetail.Text = model.Detail;
            this.lbladdtime.Text = model.AddTime.ToString("yyyy-MM-dd HH:ss:mm");
        }
    }
}

