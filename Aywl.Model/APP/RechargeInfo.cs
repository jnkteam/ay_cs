namespace OriginalStudio.Model.APP
{
    using System;

    [Serializable]
    public class RechargeInfo
    {
        private DateTime? _addtime;
        private decimal? _balance;
        private int _id;
        private string _orderno;
        private DateTime? _paytime;
        private decimal? _rechargeamt;
        private string _remark;
        private int? _status;
        private string _transno;
        private int? _userid;

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

        public decimal? balance
        {
            get
            {
                return this._balance;
            }
            set
            {
                this._balance = value;
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

        public string orderno
        {
            get
            {
                return this._orderno;
            }
            set
            {
                this._orderno = value;
            }
        }

        public DateTime? paytime
        {
            get
            {
                return this._paytime;
            }
            set
            {
                this._paytime = value;
            }
        }

        public decimal? rechargeAmt
        {
            get
            {
                return this._rechargeamt;
            }
            set
            {
                this._rechargeamt = value;
            }
        }

        public string remark
        {
            get
            {
                return this._remark;
            }
            set
            {
                this._remark = value;
            }
        }

        public int? status
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

        public string transNo
        {
            get
            {
                return this._transno;
            }
            set
            {
                this._transno = value;
            }
        }

        public int? userid
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
    }
}

