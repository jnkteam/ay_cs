namespace OriginalStudio.WebUI.User.Service
{
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model.User;
    using System;
    using System.Web;
    using System.Web.SessionState;

    public class amt : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        private UserInfo _currentUser = null;
        public MchUsersAmtInfo _currentUserAmt = null;

        public void ProcessRequest(HttpContext context)
        {
            string s = "";
            if (this.currentUser == null)
            {
                s = "{\"result\":\"no\",\"ico\":\"error\"}";
            }
            else
            {
                string str2 = ((this.balance - this.Unpayment) - this.Freeze).ToString("f2");
                if ((str2 == "") || (str2 == null))
                {
                    s = "{\"result\":\"no\",\"ico\":\"error\"}";
                }
                else
                {
                    s = "{\"result\":\"ok\",\"amt\":\"" + str2 + "\"}";
                }
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(s);
        }

        public decimal balance
        {
            get
            {
                decimal num = 0M;
                if (this.currentUserAmt != null)
                {
                    num = this.currentUserAmt.Balance;
                }
                return num;
            }
        }

        public UserInfo currentUser
        {
            get
            {
                if (this._currentUser == null)
                {
                    this._currentUser = UserFactory.CurrentMember;
                }
                return this._currentUser;
            }
        }

        public MchUsersAmtInfo currentUserAmt
        {
            get
            {
                if ((this._currentUserAmt == null) && (this.UserId > 0))
                {
                    this._currentUserAmt = MchUsersAmt.GetModel(this.UserId);
                }
                return this._currentUserAmt;
            }
        }

        public decimal Freeze
        {
            get
            {
                decimal num = 0M;
                if (this.currentUserAmt != null)
                {
                    num = this.currentUserAmt.Freeze;
                }
                return num;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public decimal Unpayment
        {
            get
            {
                decimal num = 0M;
                if (this.currentUserAmt != null)
                {
                    num = this.currentUserAmt.UnPayment;
                }
                return num;
            }
        }

        public int UserId
        {
            get
            {
                if (this.currentUser == null)
                {
                    return 0;
                }
                return this.currentUser.ID;
            }
        }
    }
}

