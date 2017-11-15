namespace OriginalStudio.Model.APP
{
    using System;

    [Serializable]
    public class apprecharge
    {
        private string _account = string.Empty;
        private DateTime _addtime = DateTime.Now;
        private string _field1 = string.Empty;
        private string _field2 = string.Empty;
        private int _id = 0;
        private string _orderid = string.Empty;
        private int _paytype = 0;
        private int _processstatus = 0;
        private DateTime? _processtime = new DateTime?(DateTime.Now);
        private decimal? _realpayamt = 0;
        private decimal _rechargeamt = 0M;
        private int _rechtype = 0;
        private string _remark = string.Empty;
        private bool _smsnotification = false;
        private int _status = 0;
        private int _suppid = 0;
        private int _userid = 0;

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

        public int paytype
        {
            get
            {
                return this._paytype;
            }
            set
            {
                this._paytype = value;
            }
        }

        public int processstatus
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

        public int rechtype
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

        public int suppid
        {
            get
            {
                return this._suppid;
            }
            set
            {
                this._suppid = value;
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

