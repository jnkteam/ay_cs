namespace OriginalStudio.Model.Settled
{
    using System;

    /// <summary>
    /// 结算对象。
    /// </summary>
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
        private SettledModeEnum _settmode = SettledModeEnum.手动提现;
        private SettledStatusEnum _status = SettledStatusEnum.审核中;
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

        public DateTime AddTime
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

        public decimal Amount
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

        public decimal? Charges
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

        public int ID
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

        public string PayeeAddress
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

        public string PayeeName
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

        public DateTime PayTime
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

        public int PayType
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

        public DateTime Required
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

        public SettledModeEnum SettledMode
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

        public SettledStatusEnum Status
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

        public int Suppid
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

        public int SuppStatus
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

        public decimal? Tax
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

        public int UserID
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

