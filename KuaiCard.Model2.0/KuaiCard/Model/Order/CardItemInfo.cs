namespace KuaiCard.Model.Order
{
    using System;

    [Serializable]
    public class CardItemInfo
    {
        private DateTime _addtime;
        private int _agent = 0;
        private string _cardno;
        private string _cardpwd;
        private int _cardtype;
        private decimal _commission = 0M;
        private DateTime? _completetime;
        private long _id;
        private string _msg;
        private string _opstate;
        private decimal? _payrate;
        private string _porderid;
        private decimal _promrate = 0M;
        private decimal _realvalue;
        private decimal? _refervalue;
        private int _serial;
        private int _status;
        private int _suppid;
        private string _supplierorder;
        private decimal _supplierrate = 0M;
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

        public int agentId
        {
            get
            {
                return this._agent;
            }
            set
            {
                this._agent = value;
            }
        }

        public string cardno
        {
            get
            {
                return this._cardno;
            }
            set
            {
                this._cardno = value;
            }
        }

        public string cardpwd
        {
            get
            {
                return this._cardpwd;
            }
            set
            {
                this._cardpwd = value;
            }
        }

        public int cardtype
        {
            get
            {
                return this._cardtype;
            }
            set
            {
                this._cardtype = value;
            }
        }

        public decimal commission
        {
            get
            {
                return this._commission;
            }
            set
            {
                this._commission = value;
            }
        }

        public DateTime? completetime
        {
            get
            {
                return this._completetime;
            }
            set
            {
                this._completetime = value;
            }
        }

        public long id
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

        public string msg
        {
            get
            {
                return this._msg;
            }
            set
            {
                this._msg = value;
            }
        }

        public string opstate
        {
            get
            {
                return this._opstate;
            }
            set
            {
                this._opstate = value;
            }
        }

        public decimal? payrate
        {
            get
            {
                return this._payrate;
            }
            set
            {
                this._payrate = value;
            }
        }

        public string porderid
        {
            get
            {
                return this._porderid;
            }
            set
            {
                this._porderid = value;
            }
        }

        public decimal promrate
        {
            get
            {
                return this._promrate;
            }
            set
            {
                this._promrate = value;
            }
        }

        public decimal realvalue
        {
            get
            {
                return this._realvalue;
            }
            set
            {
                this._realvalue = value;
            }
        }

        public decimal? refervalue
        {
            get
            {
                return this._refervalue;
            }
            set
            {
                this._refervalue = value;
            }
        }

        public int serial
        {
            get
            {
                return this._serial;
            }
            set
            {
                this._serial = value;
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

        public string supplierOrder
        {
            get
            {
                return this._supplierorder;
            }
            set
            {
                this._supplierorder = value;
            }
        }

        public decimal supplierrate
        {
            get
            {
                return this._supplierrate;
            }
            set
            {
                this._supplierrate = value;
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

