﻿namespace OriginalStudio.Model.Order
{
    using System;
    using System.Runtime.CompilerServices;
    using OriginalStudio.Lib.TimeControl;

    [Serializable]
    public class OrderBase
    {
        private long _id;

        private string _orderid;

        private int _ordertype;

        private int _userid;

        private int _channetypeid;

        private string _channelcode;

        private string _userorder;

        private decimal _refervalue;

        private decimal? _realvalue;

        private string _notifyurl;

        private string _againnotifyurl;

        private int _notifycount = 0;

        private int _notifystat = 1;

        private string _notifycontext;

        private string _returnurl;

        private string _attach;

        private string _payerip;

        private string _clientip;

        private string _referurl = string.Empty;

        private DateTime _addtime;

        private int _supplierid;

        private string _supplierorder;

        private int _status = 1;

        private DateTime? _processingtime;

        private DateTime? _completetime;

        private decimal _payrate = 0m;

        private decimal _supplierrate = 0m;

        private decimal _promrate = 0m;

        private decimal _payamt = 0m;

        private decimal _promamt = 0m;

        private decimal _supplieramt = 0m;

        private decimal _profits = 0m;

        private int? _server = new int?(1);

        private string _version = string.Empty;

        private string _cus_subject = string.Empty;

        private string _cus_price = string.Empty;

        private string _cus_quantity = string.Empty;

        private string _cus_description = string.Empty;

        private string _cus_field1 = string.Empty;

        private string _cus_field2 = string.Empty;

        private string _cus_field3 = string.Empty;

        private string _cus_field4 = string.Empty;

        private string _cus_field5 = string.Empty;

        private string _errtype = string.Empty;

        private int _agent = 0;

        private DateTime _notifytime = FormatConvertor.SqlDateTimeMinValue;

        private string _msg = string.Empty;

        private string _merchantName = string.Empty;

        private string _ipaddress = string.Empty;



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

        public string MerchantName {
            get
            {
                return this._merchantName;
            }
            set
            {
                this._merchantName = value;
            }
        }
        public string ipaddress
        {
            get
            {
                return this._ipaddress;
            }
            set
            {
                this._ipaddress = value;
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

        public string attach
        {
            get
            {
                return this._attach;
            }
            set
            {
                this._attach = value;
            }
        }

        public string clientip
        {
            get
            {
                return this._clientip;
            }
            set
            {
                this._clientip = value;
            }
        }

        public decimal? commission
        {
            get;
            set;
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

        public string cus_description
        {
            get
            {
                return this._cus_description;
            }
            set
            {
                this._cus_description = value;
            }
        }

        public string cus_field1
        {
            get
            {
                return this._cus_field1;
            }
            set
            {
                this._cus_field1 = value;
            }
        }

        public string cus_field2
        {
            get
            {
                return this._cus_field2;
            }
            set
            {
                this._cus_field2 = value;
            }
        }

        public string cus_field3
        {
            get
            {
                return this._cus_field3;
            }
            set
            {
                this._cus_field3 = value;
            }
        }

        public string cus_field4
        {
            get
            {
                return this._cus_field4;
            }
            set
            {
                this._cus_field4 = value;
            }
        }

        public string cus_field5
        {
            get
            {
                return this._cus_field5;
            }
            set
            {
                this._cus_field5 = value;
            }
        }

        public string cus_price
        {
            get
            {
                return this._cus_price;
            }
            set
            {
                this._cus_price = value;
            }
        }

        public string cus_quantity
        {
            get
            {
                return this._cus_quantity;
            }
            set
            {
                this._cus_quantity = value;
            }
        }

        public string cus_subject
        {
            get
            {
                return this._cus_subject;
            }
            set
            {
                this._cus_subject = value;
            }
        }

        public string errtype
        {
            get
            {
                return this._errtype;
            }
            set
            {
                this._errtype = value;
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

        public int? manageId
        {
            get;
            set;
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

        public DateTime notifytime
        {
            get
            {
                return this._notifytime;
            }
            set
            {
                this._notifytime = value;
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

        public int ordertype
        {
            get
            {
                return this._ordertype;
            }
            set
            {
                this._ordertype = value;
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

        public string payerip
        {
            get
            {
                return this._payerip;
            }
            set
            {
                this._payerip = value;
            }
        }

        public string channelcode
        {
            get
            {
                return this._channelcode;
            }
            set
            {
                this._channelcode = value;
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

        public DateTime? processingtime
        {
            get
            {
                return this._processingtime;
            }
            set
            {
                this._processingtime = value;
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

        public decimal? realvalue
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

        public string referUrl
        {
            get
            {
                return this._referurl;
            }
            set
            {
                this._referurl = value;
            }
        }

        public decimal refervalue
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

        public string returnurl
        {
            get
            {
                return this._returnurl;
            }
            set
            {
                this._returnurl = value;
            }
        }

        public int? server
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

        public int channeltypeId
        {
            get
            {
                return this._channetypeid;
            }
            set
            {
                this._channetypeid = value;
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

        public string version
        {
            get
            {
                return this._version;
            }
            set
            {
                this._version = value;
            }
        }
    }
}

