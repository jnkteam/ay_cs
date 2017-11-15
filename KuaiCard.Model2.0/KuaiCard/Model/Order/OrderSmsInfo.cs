namespace KuaiCard.Model.Order
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class OrderSmsInfo
    {
        private int _id;

        private string _orderid;

        private string _userorder;

        private int _supplierid;

        private int _userid;

        private string _mobile;

        private decimal _fee;

        private string _message;

        private string _servicenum;

        private string _linkid;

        private string _gwid;

        private decimal _payrate;

        private decimal _supplierrate;

        private decimal _promrate;

        private decimal _payamt;

        private decimal _promamt;

        private decimal _supplieramt;

        private decimal _profits;

        private int _server;

        private DateTime _addtime;

        private DateTime _completetime;

        private string _notifyurl;

        private string _againnotifyurl;

        private int _notifycount = 0;

        private int _notifystat = 1;

        private string _notifycontext;

        private bool _issucc = false;

        public string _errcode = string.Empty;

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

        public string againNotifyUrl
        {
            get
            {
                return this._againnotifyurl;
            }
            set
            {
                this._againnotifyurl = value;
            }
        }

        public string Cmd
        {
            get;
            set;
        }

        public decimal? commission
        {
            get;
            set;
        }

        public DateTime completetime
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

        public string errcode
        {
            get
            {
                return this._errcode;
            }
            set
            {
                this._errcode = value;
            }
        }

        public decimal fee
        {
            get
            {
                return this._fee;
            }
            set
            {
                this._fee = value;
            }
        }

        public string gwid
        {
            get
            {
                return this._gwid;
            }
            set
            {
                this._gwid = value;
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

        public bool issucc
        {
            get
            {
                return this._issucc;
            }
            set
            {
                this._issucc = value;
            }
        }

        public string linkid
        {
            get
            {
                return this._linkid;
            }
            set
            {
                this._linkid = value;
            }
        }

        public int? manageId
        {
            get;
            set;
        }

        public string message
        {
            get
            {
                return this._message;
            }
            set
            {
                this._message = value;
            }
        }

        public string mobile
        {
            get
            {
                return this._mobile;
            }
            set
            {
                this._mobile = value;
            }
        }

        public string msg
        {
            get;
            set;
        }

        public string notifycontext
        {
            get
            {
                return this._notifycontext;
            }
            set
            {
                this._notifycontext = value;
            }
        }

        public int notifycount
        {
            get
            {
                return this._notifycount;
            }
            set
            {
                this._notifycount = value;
            }
        }

        public int notifystat
        {
            get
            {
                return this._notifystat;
            }
            set
            {
                this._notifystat = value;
            }
        }

        public string notifyurl
        {
            get
            {
                return this._notifyurl;
            }
            set
            {
                this._notifyurl = value;
            }
        }

        public string opstate
        {
            get;
            set;
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

        public decimal payAmt
        {
            get
            {
                return this._payamt;
            }
            set
            {
                this._payamt = value;
            }
        }

        public decimal payRate
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

        public decimal profits
        {
            get
            {
                return this._profits;
            }
            set
            {
                this._profits = value;
            }
        }

        public decimal promAmt
        {
            get
            {
                return this._promamt;
            }
            set
            {
                this._promamt = value;
            }
        }

        public decimal promRate
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

        public int server
        {
            get
            {
                return this._server;
            }
            set
            {
                this._server = value;
            }
        }

        public string servicenum
        {
            get
            {
                return this._servicenum;
            }
            set
            {
                this._servicenum = value;
            }
        }

        public int status
        {
            get;
            set;
        }

        public decimal supplierAmt
        {
            get
            {
                return this._supplieramt;
            }
            set
            {
                this._supplieramt = value;
            }
        }

        public int supplierId
        {
            get
            {
                return this._supplierid;
            }
            set
            {
                this._supplierid = value;
            }
        }

        public decimal supplierRate
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

        public string userMsgContenct
        {
            get;
            set;
        }

        public string userorder
        {
            get
            {
                return this._userorder;
            }
            set
            {
                this._userorder = value;
            }
        }
    }
}

