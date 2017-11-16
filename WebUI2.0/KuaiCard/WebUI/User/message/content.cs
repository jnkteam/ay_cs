namespace OriginalStudio.WebUI.User.message
{
    using OriginalStudio.BLL;
    using OriginalStudio.Model;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Web.UI.WebControls;

    public class content : UserPageBase
    {
        private IMSG _item = null;
        protected string classa = string.Empty;
        protected Label lit_addtime;
        protected Label lit_content;
        protected Label lit_title;

        private void InitForm()
        {
            if (this.ItemInfo != null)
            {
                this.lit_title.Text = this.ItemInfo.msg_title;
                this.lit_addtime.Text = this.ItemInfo.msg_addtime.ToString();
                this.lit_content.Text = base.Server.HtmlDecode(this.ItemInfo.msg_content);
                if (!this.ItemInfo.isRead)
                {
                    this.ItemInfo.isRead = true;
                    //IMSGFactory.Update(this.ItemInfo);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.InitForm();
            }
        }

        public IMSG ItemInfo
        {
            get
            {
                if ((this._item == null) && (this.msgId > 0))
                {
                    this._item = new IMSG();    // IMSGFactory.GetModel(this.msgId);
                }
                return this._item;
            }
        }

        public int msgId
        {
            get
            {
                return WebBase.GetQueryStringInt32("id", 0);
            }
        }
    }
}

