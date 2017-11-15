namespace KuaiCard.Model.APP
{
    using System;

    [Serializable]
    public class EntrustInfo
    {
        private DateTime _addtime;
        private decimal _amount;
        private string _bankcardnum;
        private string _bankname;
        private int? _cadmin;
        private DateTime? _cdate;
        private int _id;
        private string _payee;
        private decimal _rate;
        private string _remark;
        private decimal _remittancefee;
        private int _status;
        private decimal _totalamt;
        private int _userid;

        public DateTime addtime
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

        public decimal amount
        {
            get
            {
                return this._amount;
            }
            set
            {
                this._amount = value;
            }
        }

        public string bankcardnum
        {
            get
            {
                return this._bankcardnum;
            }
            set
            {
                this._bankcardnum = value;
            }
        }

        public string bankname
        {
            get
            {
                return this._bankname;
            }
            set
            {
                this._bankname = value;
            }
        }

        public int? cadmin
        {
            get
            {
                return this._cadmin;
            }
            set
            {
                this._cadmin = value;
            }
        }

        public DateTime? cdate
        {
            get
            {
                return this._cdate;
            }
            set
            {
                this._cdate = value;
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

        public string payee
        {
            get
            {
                return this._payee;
            }
            set
            {
                this._payee = value;
            }
        }

        public decimal rate
        {
            get
            {
                return this._rate;
            }
            set
            {
                this._rate = value;
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

        public decimal remittancefee
        {
            get
            {
                return this._remittancefee;
            }
            set
            {
                this._remittancefee = value;
            }
        }

        public int status
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

        public decimal totalAmt
        {
            get
            {
                return this._totalamt;
            }
            set
            {
                this._totalamt = value;
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
    }
}

