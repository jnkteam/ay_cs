namespace KuaiCard.WebUI.Merchant.Ajax
{
    using KuaiCard.BLL;
    using KuaiCard.BLL.Channel;
    using KuaiCard.BLL.User;
    using KuaiCard.ETAPI;
    using KuaiCard.Model;
    using KuaiCard.Model.Channel;
    using KuaiCard.Model.Order;
    using KuaiCard.Model.User;
    using KuaiCard.SysConfig;
    using KuaiCardLib.ExceptionHandling;
    using KuaiCardLib.Web;
    using System;
    using System.Threading;
    using System.Web;
    using System.Web.SessionState;

    public class CardSellOld : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        private ChannelTypeInfo _typeInfo = null;
        private static readonly object obj = "KuaiCardsoft_QQ_1780493978_xiaoka_ProcessOrder";

        protected string ProcessOrder(HttpContext context)
        {
            if ((((this.typeId == 0x67) || (this.typeId == 0x6c)) || (this.typeId == 0x71)) || ((this.typeId >= 200) && (this.typeId <= 0xcb)))
            {
                lock (obj)
                {
                    int millisecondsTimeout = 0;
                    try
                    {
                        millisecondsTimeout = Convert.ToInt32(decimal.Round(RuntimeSetting.Xiaoka_time_interval * 0x3e8, 0));
                    }
                    catch
                    {
                    }
                    if (millisecondsTimeout > 0)
                    {
                        Thread.Sleep(millisecondsTimeout);
                    }
                    return this.SendToSupp();
                }
            }
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
                int num = WebBase.GetQueryStringInt32("ctl00$ContentPlaceHolder1$xk_channelId", 0);
                if (num == 0)
                {
                    num = WebBase.GetQueryStringInt32("ChannelId", 0);
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

