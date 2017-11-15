namespace KuaiCard.Model.APP
{
    using System;

    [Serializable]
    public class APPRechargeInfo
    {
        private string _account;
        private DateTime? _addtime;
        private string _field1;
        private string _field2;
        private int _id;
        private string _orderid;
        private int? _processstatus;
        private DateTime? _processtime;
        private decimal? _realpayamt;
        private decimal _rechargeamt;
        private int? _rechtype;
        private string _remark;
        private bool _smsnotification;
        private int? _status;
        private int _userid;

        public string account
        {
            get
            {
                return this._account;
            }
            set
            {
                this._account = value;
            }
        }

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

        public string field1
        {
            get
            {
                return this._field1;
            }
            set
            {
                this._field1 = value;
            }
        }

        public string field2
        {
            get
            {
                return this._field2;
            }
            set
            {
                this._field2 = value;
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

        public string orderid
        {
            get
            {
                return this._orderid;
            }
            set
            {
                this._orderid = value;
            }
        }

        public int? processstatus
        {
            get
            {
                return this._processstatus;
            }
            set
            {
                this._processstatus = value;
            }
        }

        public DateTime? processtime
        {
            get
            {
                return this._processtime;
            }
            set
            {
                this._processtime = value;
            }
        }

        public decimal? realPayAmt
        {
            get
            {
                return this._realpayamt;
            }
            set
            {
                this._realpayamt = value;
            }
        }

        public decimal rechargeAmt
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

        public int? rechtype
        {
            get
            {
                return this._rechtype;
            }
            set
            {
                this._rechtype = value;
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

        public bool smsnotification
        {
            get
            {
                return this._smsnotification;
            }
            set
            {
                this._smsnotification = value;
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

