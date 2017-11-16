namespace KuaiCard.web.Merchant.Ajax
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Channel;
    using OriginalStudio.BLL.User;
    using OriginalStudio.ETAPI;
    using OriginalStudio.Model;
    using OriginalStudio.Model.Channel;
    using OriginalStudio.Model.Order;
    using OriginalStudio.Model.User;
    using OriginalStudio.Lib.SysConfig;
    using OriginalStudio.Lib.ExceptionHandling;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Web;
    using System.Web.SessionState;

    public class smartcardsell : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        private ChannelTypeInfo _typeInfo = null;
        private static readonly object obj = "KuaiCardsoft_QQ_1780493978_xiaoka_ProcessOrder";

        protected string ProcessOrder(HttpContext context)
        {
            return this.SendToSupp();
        }

        public void ProcessRequest(HttpContext context)
        {
            string message = "";
            if (this.currentUser == null)
            {
                message = "登录信息失效，请重新登录";
            }
            else if (this.typeInfo == null)
            {
                message = "无效卡";
            }
            else if (string.IsNullOrEmpty(this.cardNo))
            {
                message = "请输入卡号";
            }
            else if (string.IsNullOrEmpty(this.cardPwd))
            {
                message = "请输入卡密";
            }
            else if (this.faceValue <= 0)
            {
                message = "面值不正确";
            }
            else if (this.typeId <= 0)
            {
                message = "通道不正确";
            }
            if (string.IsNullOrEmpty(message))
            {
                try
                {
                    message = this.ProcessOrder(context);
                }
                catch (Exception exception)
                {
                    message = exception.Message;
                    ExceptionHandler.HandleException(exception);
                }
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(message);
        }

        private string SendToSupp()
        {
            try
            {
                int? nullable;
                int iD = this.currentUser.ID;
                string str = string.Empty;
                int suppId = 0;
                bool flag = this.typeInfo.runmode == 1;
                ChannelInfo info = KuaiCard.BLL.Channel.Channel.GetModel(this.typeId, this.faceValue, iD, flag);
                if (((info == null) || !info.supplier.HasValue) || (((nullable = info.supplier).GetValueOrDefault() == 0) && nullable.HasValue))
                {
                    str = "找不到通道,请联系商务或系统管理员处理。";
                }
                else
                {
                    suppId = info.supplier.Value;
                }
                if ((suppId > 0) && string.IsNullOrEmpty(str))
                {
                    OrderCard card = new OrderCard();
                    OrderCardInfo order = new OrderCardInfo();
                    order.orderid = card.GenerateUniqueOrderId(this.typeId);
                    order.addtime = DateTime.Now;
                    order.attach = "cardsell";
                    order.notifycontext = string.Empty;
                    order.notifycount = 0;
                    order.notifystat = 0;
                    order.notifyurl = string.Empty;
                    order.clientip = ServerVariables.TrueIP;
                    order.completetime = new DateTime?(DateTime.Now);
                    order.ordertype = 8;
                    order.typeId = this.typeId;
                    order.paymodeId = this.typeId.ToString("0000") + this.faceValue.ToString();
                    order.payRate = 0M;
                    order.supplierId = suppId;
                    order.supplierOrder = string.Empty;
                    order.userid = iD;
                    order.userorder = order.orderid + iD.ToString();
                    order.refervalue = this.faceValue;
                    order.referUrl = string.Empty;
                    order.cardNo = this.cardNo;
                    order.cardPwd = this.cardPwd;
                    order.server = new int?(RuntimeSetting.ServerId);
                    order.cardnum = 1;
                    order.version = SystemApiHelper.vcmyapi10;
                    order.agentId = UserFactory.GetPromID(iD);
                    order.method = 0;
                    order.faceValue = 0M;
                    order.manageId = this.currentUser.manageId;
                    card.Insert(order);
                    string supporderid = string.Empty;
                    string errormsg = string.Empty;
                    string supperrorcode = string.Empty;
                    str = "true";
                    SupplierCode supp = (SupplierCode) suppId;
                }
                return str;
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        public string cardNo
        {
            get
            {
                return WebBase.GetQueryStringString("CardId", "").Trim();
            }
        }

        public string cardPwd
        {
            get
            {
                return WebBase.GetQueryStringString("CardPass", "").Trim();
            }
        }

        public UserInfo currentUser
        {
            get
            {
                return UserFactory.CurrentMember;
            }
        }

        public int faceValue
        {
            get
            {
                return WebBase.GetQueryStringInt32("FaceValue", 0);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public int typeId
        {
            get
            {
                int num = 0;
                if ((this.cardNo.Length == 0x11) && (this.cardPwd.Length == 0x12))
                {
                    num = 0x67;
                }
                if ((this.cardNo.Length == 15) && (this.cardPwd.Length == 0x13))
                {
                    num = 0x6c;
                }
                if ((this.cardNo.Length == 0x13) && (this.cardPwd.Length == 0x12))
                {
                    num = 0x71;
                }
                if ((this.cardNo.Length == 10) && (this.cardPwd.Length == 15))
                {
                    num = 0x6f;
                }
                if ((this.cardNo.Length == 13) && (this.cardPwd.Length == 10))
                {
                    num = 0x69;
                }
                if ((this.cardNo.Length == 0x10) && (this.cardPwd.Length == 0x10))
                {
                    num = 0x6a;
                }
                if ((this.cardNo.Length == 13) && (this.cardPwd.Length == 9))
                {
                    num = 110;
                }
                if ((this.cardNo.Length == 20) && (this.cardPwd.Length == 12))
                {
                    num = 0x70;
                }
                if ((this.cardNo.Length == 9) && (this.cardPwd.Length == 12))
                {
                    num = 0x6b;
                }
                if (((this.cardNo.Length == 13) && (this.cardPwd.Length == 10)) || ((this.cardNo.Length == 12) && (this.cardPwd.Length == 6)))
                {
                    num = 0x6d;
                }
                if (((this.cardNo.Length == 10) && (this.cardPwd.Length == 10)) || ((this.cardNo.Length == 12) && (this.cardPwd.Length == 15)))
                {
                    num = 0x77;
                }
                if ((this.cardNo.Length == 20) && (this.cardPwd.Length == 8))
                {
                    num = 0x73;
                }
                if ((this.cardNo.Length == 15) && (this.cardPwd.Length == 15))
                {
                    num = 0x75;
                }
                if (((this.cardNo.Length == 15) || (this.cardNo.Length == 0x10)) && (this.cardPwd.Length == 8))
                {
                    string[] strArray = new string[] { "CSC5", "CS", "S", "CA", "CSB", "YA", "YB", "YC", "YD", "801335" };
                    foreach (string str in strArray)
                    {
                        if (this.cardNo.StartsWith(str))
                        {
                            return 0x68;
                        }
                    }
                }
                return num;
            }
        }

        public ChannelTypeInfo typeInfo
        {
            get
            {
                if ((this._typeInfo == null) && (this.typeId > 0))
                {
                    this._typeInfo = ChannelType.GetCacheModel(this.typeId);
                }
                return this._typeInfo;
            }
        }
    }
}

