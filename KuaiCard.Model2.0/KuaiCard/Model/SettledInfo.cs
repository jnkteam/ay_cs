﻿namespace KuaiCard.Model
{
    using System;

    public class SettledInfo
    {
        private string _Account = string.Empty;
        private DateTime _addtime;
        private decimal _amount;
        private AppTypeEnum _appType;
        private decimal? _charges = 0;
        private int _id;
        private string _BankCode = "";
        private string _Payeeaddress = string.Empty;
        private string _PayeeBank = string.Empty;
        private string _payeeName = string.Empty;
        private DateTime _paytime;
        private int _Paytype;
        private DateTime _required;
        private SettledmodeEnum _settmode = SettledmodeEnum.手动提现;
        private SettledStatus _status = SettledStatus.审核中;
        private int _suppid = 0;
        private int _suppstatus;
        private decimal? _tax = 0;
        private int _userid;

        public string Account
        {
            get
            {
                return this._Account;
            }
            set
            {
                this._Account = value;
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

        public AppTypeEnum AppType
        {
            get
            {
                return this._appType;
            }
            set
            {
                this._appType = value;
            }
        }

        public decimal? charges
        {
            get
            {
                return this._charges;
            }
            set
            {
                this._charges = value;
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

        public string Payeeaddress
        {
            get
            {
                return this._Payeeaddress;
            }
            set
            {
                this._Payeeaddress = value;
            }
        }

        public string PayeeBank
        {
            get
            {
                return this._PayeeBank;
            }
            set
            {
                this._PayeeBank = value;
            }
        }

        public string payeeName
        {
            get
            {
                return this._payeeName;
            }
            set
            {
                this._payeeName = value;
            }
        }

        public DateTime paytime
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

        public int Paytype
        {
            get
            {
                return this._Paytype;
            }
            set
            {
                this._Paytype = value;
            }
        }

        public DateTime required
        {
            get
            {
                return this._required;
            }
            set
            {
                this._required = value;
            }
        }

        public SettledmodeEnum settmode
        {
            get
            {
                return this._settmode;
            }
            set
            {
                this._settmode = value;
            }
        }

        public SettledStatus status
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

        public int suppstatus
        {
            get
            {
                return this._suppstatus;
            }
            set
            {
                this._suppstatus = value;
            }
        }

        public decimal? tax
        {
            get
            {
                return this._tax;
            }
            set
            {
                this._tax = value;
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

        public string BankCode
        {
            get
            {
                return this._BankCode;
            }
            set
            {
                this._BankCode = value;
            }
        }
    }
}

