namespace OriginalStudio.Model.Order
{
    using System;

    public class callbackInfo
    {
        private bool _isnotify = false;
        private string _message = string.Empty;
        private string _opstate = string.Empty;
        private decimal _realmoney = 0M;
        private string _resultcode = string.Empty;
        private int _seq = 0;
        private int _status = 0;
        private int _suppid = 0;
        private decimal _suppmoney = 0M;
        private string _supporderid = string.Empty;
        private string _sysorderid = string.Empty;
        private string _transactionNo = string.Empty;

        public callbackInfo(int supp, string no)
        {
            this._suppid = supp;
            this._transactionNo = no;
            this._sysorderid = no;
            this._seq = 0;
            if (no.IndexOf('_') > 0)
            {
                this._sysorderid = no.Substring(0, (no.Length - no.IndexOf('_')) - 1);
                this._seq = Convert.ToInt32(no.Substring(no.IndexOf('_') - 1));
            }
        }

        public bool isnotify
        {
            get
            {
                return this._isnotify;
            }
            set
            {
                this._isnotify = value;
            }
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

        public decimal realmoney
        {
            get
            {
                return this._realmoney;
            }
            set
            {
                this._realmoney = value;
            }
        }

        public string resultcode
        {
            get
            {
                return this._resultcode;
            }
            set
            {
                this._resultcode = value;
            }
        }

        public int seq
        {
            get
            {
                return this._seq;
            }
            set
            {
                this._seq = value;
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

        public decimal suppmoney
        {
            get
            {
                return this._suppmoney;
            }
            set
            {
                this._suppmoney = value;
            }
        }

        public string supporderid
        {
            get
            {
                return this._supporderid;
            }
            set
            {
                this._supporderid = value;
            }
        }

        public string sysorderid
        {
            get
            {
                return this._sysorderid;
            }
            set
            {
                this._sysorderid = value;
            }
        }

        public string transactionNo
        {
            get
            {
                return this._transactionNo;
            }
            set
            {
                this._transactionNo = value;
            }
        }
    }
}

