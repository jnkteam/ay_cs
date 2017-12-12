namespace OriginalStudio.Model.User
{
    using OriginalStudio.Model.Settled;
    using System;

    [Serializable]
    public class UsersAmtFreezeInfo
    {
        private DateTime _addtime = DateTime.Now;
        private DateTime _checktime = DateTime.Now;
        private decimal _freezeamt;
        private int _id;
        private int _manageid = 0;
        private AmtFreezeInfoStatus _status = AmtFreezeInfoStatus.否;
        private AmtunFreezeMode _unfreezemode = AmtunFreezeMode.未处理;
        private int _userid = 0;
        private string _why = "";

        public DateTime Addtime
        {
            get
            {
                return this._addtime;
            }
            set
            {
                this._addtime = value;
            }
        }

        public DateTime Checktime
        {
            get
            {
                return this._checktime;
            }
            set
            {
                this._checktime = value;
            }
        }

        public decimal FreezeAmt
        {
            get
            {
                return this._freezeamt;
            }
            set
            {
                this._freezeamt = value;
            }
        }

        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        public int ManageId
        {
            get
            {
                return this._manageid;
            }
            set
            {
                this._manageid = value;
            }
        }

        public AmtFreezeInfoStatus Status
        {
            get
            {
                return this._status;
            }
            set
            {
                this._status = value;
            }
        }

        public AmtunFreezeMode UnFreezeMode
        {
            get
            {
                return this._unfreezemode;
            }
            set
            {
                this._unfreezemode = value;
            }
        }

        public int UserID
        {
            get
            {
                return this._userid;
            }
            set
            {
                this._userid = value;
            }
        }

        public string Why
        {
            get
            {
                return this._why;
            }
            set
            {
                this._why = value;
            }
        }
    }
}

