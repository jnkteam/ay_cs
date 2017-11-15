namespace KuaiCard.WebUI.merchant.ajax
{
    using KuaiCard.BLL.Channel;
    using KuaiCard.BLL.User;
    using KuaiCard.Model.Channel;
    using KuaiCard.Model.User;
    using KuaiCardLib.ExceptionHandling;
    using KuaiCardLib.Web;
    using System;
    using System.Data;
    using System.Web;
    using System.Web.SessionState;

    public class GetCardFaceValue : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        private ChannelTypeInfo _typeInfo = null;

        public void ProcessRequest(HttpContext context)
        {
            string str = string.Empty;
            try
            {
                if (((this.typeInfo != null) && (this.currentUser != null)) && (this.typeid > 0))
                {
                    DataRow[] rowArray = KuaiCard.BLL.Channel.Channel.GetCacheList().Select("salecard=1 and typeId=" + this.typeid.ToString());
                    for (int i = 0; i < rowArray.Length; i++)
                    {
                        int? nullable;
                        ChannelInfo info = KuaiCard.BLL.Channel.Channel.GetModel(Convert.ToString(rowArray[i]["code"]), this.currentUser.ID, true);
                        if ((info != null) && (((nullable = info.isOpen).GetValueOrDefault() == 1) && nullable.HasValue))
                        {
                            if (string.IsNullOrEmpty(str))
                            {
                                str = str + info.faceValue.ToString();
                            }
                            else
                            {
                                str = str + "," + info.faceValue.ToString();
                            }
                        }
                    }
                    str = str + "|" + this.typeInfo.runmode.ToString();
                }
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(str);
        }

        public UserInfo currentUser
        {
            get
            {
                return UserFactory.CurrentMember;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public int typeid
        {
            get
            {
                int result = 0;
                string formString = WebBase.GetFormString("typeid", string.Empty);
                if (!string.IsNullOrEmpty(formString))
                {
                    int.TryParse(formString, out result);
                }
                return result;
            }
        }

        public ChannelTypeInfo typeInfo
        {
            get
            {
                if ((this._typeInfo == null) && (this.typeid > 0))
                {
                    this._typeInfo = ChannelType.GetCacheModel(this.typeid);
                }
                return this._typeInfo;
            }
        }
    }
}

