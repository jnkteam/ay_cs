namespace OriginalStudio.WebUI.User.Pincard.card
{
    using OriginalStudio.BLL.Channel;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model.Channel;
    using OriginalStudio.Model.User;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class index : UserPageBase
    {
        private usersettingInfo _usersetting = null;
        public string defaultvalue = string.Empty;
        protected HtmlTableCell facevaluelist;
        protected HtmlInputHidden g_channelId;
        protected HtmlAnchor link103;
        protected HtmlAnchor link104;
        protected HtmlAnchor link105;
        protected HtmlAnchor link106;
        protected HtmlAnchor link107;
        protected HtmlAnchor link108;
        protected HtmlAnchor link109;
        protected HtmlAnchor link110;
        protected HtmlAnchor link111;
        protected HtmlAnchor link112;
        protected HtmlAnchor link113;
        protected HtmlAnchor link115;
        protected HtmlAnchor link116;
        protected HtmlAnchor link117;
        protected HtmlAnchor link118;
        protected HtmlAnchor link119;
        protected HtmlAnchor link200;
        protected HtmlAnchor link201;
        protected HtmlAnchor link202;
        protected HtmlAnchor link203;
        protected HtmlAnchor link204;
        protected HtmlAnchor link205;
        protected HtmlAnchor link208;
        protected HtmlAnchor link209;
        protected HtmlAnchor link210;
        protected HtmlAnchor link999;
        protected string ordtotal = "0";
        protected string pagerealvalue = "0";
        protected string pagerefervalue = "0";
        protected string succordtotal = "0";
        protected string totalpayAmt = "0";
        protected string totalrealvalue = "0";
        protected string totalrefervalue = "0";
        private OriginalStudio.BLL.User.usersetting usersetDAL = new OriginalStudio.BLL.User.usersetting();
        protected HiddenField xk_faceValue;

        public void docmd()
        {
            if ((this.writein == "index") && (this.cid > 0))
            {
                string str = "首选页面设置失败";
                this.usersetting.userid = base.UserId;
                this.usersetting.defaultpay = this.cid;
                if (this.usersetDAL.Insert(this.usersetting))
                {
                    string modetypename = ChannelType.GetCacheModel(this.cid).modetypename;
                    str = string.Format("首选页面设置成功！<br>进入\"点卡消耗\"频道将直接进入设为<br>{0}首选的通道提交页面！", modetypename);
                }
                string script = string.Format("<script type=\"text/javascript\">$(document).ready(function(){{$.dialog({{title:lktitle,resize:false,content: '{0}！',ok:function () {{location.href ='/Merchant/excha.aspx';return false;}},close:function () {{location.href ='/Merchant/excha.aspx';return false;}},icon: 'succeed',width:'250px',height:'90px',time:5}});}})</script>", str);
                this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "msg", script);
            }
        }

        private void InitForm()
        {
            int cid = 0;
            bool flag = true;
            foreach (int num2 in Enum.GetValues(typeof(EnumChannelType)))
            {
                if (((num2 >= 0x67) && (num2 != 0x72)) && (num2 != 300))
                {
                    string str = string.Empty;
                    bool flag2 = ChannelType.IsOpen(num2, base.UserId);
                    if (num2 == this.cid)
                    {
                        flag = flag2;
                        str = "cur";
                    }
                    if (flag2 && (cid == 0))
                    {
                        cid = num2;
                    }
                    this.setCtrl(num2, flag2, str);
                }
            }
            if (!flag)
            {
                this.setCtrl(cid, true, "cur");
            }
            else
            {
                cid = this.cid;
            }
            if (cid > 0)
            {
                if (cid == this.usersetting.defaultpay)
                {
                }
                this.g_channelId.Value = cid.ToString();
                string name = Enum.GetName(typeof(EnumChannelType), cid);
                if (cid > 0)
                {
                    string str3 = string.Empty;
                    DataRow[] rowArray = OriginalStudio.BLL.Channel.Channel.GetCacheList().Select("typeId=" + cid.ToString());
                    for (int i = 0; i < rowArray.Length; i++)
                    {
                        int? nullable;
                        ChannelInfo info = OriginalStudio.BLL.Channel.Channel.GetModel(Convert.ToString(rowArray[i]["code"]), base.UserId, true);
                        if ((info != null) && (((nullable = info.isOpen).GetValueOrDefault() == 1) && nullable.HasValue))
                        {
                            if (info.faceValue != 100)
                            {
                                str3 = str3 + string.Format("<a href=\"#\"   name= \"{0}\" >{0}元</a>", rowArray[i]["faceValue"]);
                            }
                            else
                            {
                                str3 = str3 + string.Format("<a href=\"#\" class=\"cur\"  name= \"{0}\" >{0}元</a>", rowArray[i]["faceValue"]);
                                this.xk_faceValue.Value = this.defaultvalue = rowArray[i]["faceValue"].ToString();
                            }
                        }
                    }
                    this.facevaluelist.InnerHtml = str3;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.docmd();
            if (!base.IsPostBack)
            {
                this.InitForm();
            }
        }

        private void setCtrl(int _paytype, bool _visible, string _style)
        {
            HtmlAnchor anchor = null;
            if (_paytype == 0x67)
            {
                anchor = this.link103;
            }
            else if (_paytype == 0x68)
            {
                anchor = this.link104;
            }
            else if (_paytype == 210)
            {
                anchor = this.link210;
            }
            else if (_paytype == 0x69)
            {
                anchor = this.link105;
            }
            else if (_paytype == 0x6a)
            {
                anchor = this.link106;
            }
            else if (_paytype == 0x6b)
            {
                anchor = this.link107;
            }
            else if (_paytype == 0x6c)
            {
                anchor = this.link108;
            }
            else if (_paytype == 0x6d)
            {
                anchor = this.link109;
            }
            else if (_paytype == 110)
            {
                anchor = this.link110;
            }
            else if (_paytype == 0x6f)
            {
                anchor = this.link111;
            }
            else if (_paytype == 0x70)
            {
                anchor = this.link112;
            }
            else if (_paytype == 0x71)
            {
                anchor = this.link113;
            }
            else if (_paytype == 0x73)
            {
                anchor = this.link115;
            }
            else if (_paytype == 0x74)
            {
                anchor = this.link116;
            }
            else if (_paytype == 0x75)
            {
                anchor = this.link117;
            }
            else if (_paytype == 0x76)
            {
                anchor = this.link118;
            }
            else if (_paytype == 0x77)
            {
                anchor = this.link119;
            }
            else if (_paytype == 200)
            {
                anchor = this.link200;
            }
            else if (_paytype == 0xc9)
            {
                anchor = this.link201;
            }
            else if (_paytype == 0xca)
            {
                anchor = this.link202;
            }
            else if (_paytype == 0xcb)
            {
                anchor = this.link203;
            }
            else if (_paytype == 0xcc)
            {
                anchor = this.link204;
            }
            else if (_paytype == 0xcd)
            {
                anchor = this.link205;
            }
            else if (_paytype == 0xd0)
            {
                anchor = this.link208;
            }
            else if (_paytype == 0xd1)
            {
                anchor = this.link209;
            }
            if (anchor != null)
            {
                anchor.Visible = _visible;
                anchor.Attributes["class"] = _style;
            }
        }

        public int cid
        {
            get
            {
                return WebBase.GetQueryStringInt32("cid", this.usersetting.defaultpay);
            }
        }

        public bool isdefaultPay
        {
            get
            {
                return (this.cid == this.usersetting.defaultpay);
            }
        }

        public usersettingInfo usersetting
        {
            get
            {
                if (this._usersetting == null)
                {
                    this._usersetting = this.usersetDAL.GetModel(base.UserId);
                }
                return this._usersetting;
            }
        }

        public string writein
        {
            get
            {
                return WebBase.GetQueryStringString("writein", string.Empty);
            }
        }
    }
}

