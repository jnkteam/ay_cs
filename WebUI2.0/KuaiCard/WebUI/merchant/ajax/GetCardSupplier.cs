namespace KuaiCard.WebUI.merchant.ajax
{
    using OriginalStudio.BLL.Channel;
    using OriginalStudio.BLL.Payment;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model.Channel;
    using OriginalStudio.Model.User;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Data;
    using System.Web;
    using System.Web.SessionState;

    public class GetCardSupplier : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        private ChannelTypeInfo _typeInfo = null;

        public void ProcessRequest(HttpContext context)
        {
            string str = string.Empty;
            if (((this.typeInfo != null) && (this.currentUser != null)) && (this.typeid > 0))
            {
                DataRow[] rowArray = Channelsupplier.GetList1(this.typeid, 0).Tables[0].Select();
                for (int i = 0; i < rowArray.Length; i++)
                {
                    decimal userPayRate = 0M;
                    try
                    {
                        userPayRate = Convert.ToDecimal(rowArray[i]["payrate"]);
                    }
                    catch
                    {
                    }
                    if (userPayRate == 0M)
                    {
                        userPayRate = PayRateFactory.GetUserPayRate(this.currentUser.ID, this.typeid);
                    }
                    if (string.IsNullOrEmpty(str))
                    {
                        str = str + string.Format("{0}|{3}|{1}({2:p2})", new object[] { rowArray[i]["suppid"], rowArray[i]["name1"], userPayRate, rowArray[i]["isdefault"] });
                    }
                    else
                    {
                        str = str + "," + string.Format("{0}|{3}|{1}({2:p2})", new object[] { rowArray[i]["suppid"], rowArray[i]["name1"], userPayRate, rowArray[i]["isdefault"] });
                    }
                }
                str = str + "&" + this.typeInfo.runmode.ToString();
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

