namespace OriginalStudio.Model.Settled
{
    using System;

    [Serializable]
    public class UsersAmtFreezeInfo
    {
        private DateTime? _addtime;
        private DateTime? _checktime;
        private decimal _freezeamt;
        private int _id;
        private int? _manageid;
        private AmtFreezeInfoStatus _status = AmtFreezeInfoStatus.否;
        private AmtunFreezeMode _unfreezemode = AmtunFreezeMode.未处理;
        private int _userid;
        private string _why;

        public DateTime? addtime
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

        public DateTime? checktime
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

        public decimal freezeAmt
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

        public int id
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

        public int? manageId
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

        public AmtFreezeInfoStatus status
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

        public AmtunFreezeMode unfreezemode
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

        public int userid
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

        public string why
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

