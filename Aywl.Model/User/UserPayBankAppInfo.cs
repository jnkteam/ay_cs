namespace OriginalStudio.Model.User
{
    using System;

    [Serializable]
    public class UserPayBankAppInfo
    {
        private string _account = string.Empty;
        private int _accouttype = 0;
        private DateTime? _addtime = new DateTime?(DateTime.Now);
        private string _bankaddress = string.Empty;
        private string _bankcity = string.Empty;
        private string _bankcode = string.Empty;
        private string _bankprovince = string.Empty;
        private string _citycode = string.Empty;
        private int _id;
        private string _payeebank = string.Empty;
        private string _payeename = string.Empty;
        private int _pmode = 1;
        private string _provincecode = string.Empty;
        private AcctChangeEnum _status = AcctChangeEnum.待审核;
        private DateTime? _suretime = new DateTime?(DateTime.Now);
        private int? _sureuser = 0;
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

        public int accoutType
        {
            get
            {
                return this._accouttype;
            }
            set
            {
                this._accouttype = value;
            }
        }

        public DateTime? AddTime
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

        public string bankAddress
        {
            get
            {
                return this._bankaddress;
            }
            set
            {
                this._bankaddress = value;
            }
        }

        public string bankCity
        {
            get
            {
                return this._bankcity;
            }
            set
            {
                this._bankcity = value;
            }
        }

        public string BankCode
        {
            get
            {
                return this._bankcode;
            }
            set
            {
                this._bankcode = value;
            }
        }

        public string bankProvince
        {
            get
            {
                return this._bankprovince;
            }
            set
            {
                this._bankprovince = value;
            }
        }

        public string cityCode
        {
            get
            {
                return this._citycode;
            }
            set
            {
                this._citycode = value;
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

        public string payeeBank
        {
            get
            {
                return this._payeebank;
            }
            set
            {
                this._payeebank = value;
            }
        }

        public string payeeName
        {
            get
            {
                return this._payeename;
            }
            set
            {
                this._payeename = value;
            }
        }

        public int pmode
        {
            get
            {
                return this._pmode;
            }
            set
            {
                this._pmode = value;
            }
        }

        public string pmodeName
        {
            get
            {
                string str = string.Empty;
                switch (this.pmode)
                {
                    case 1:
                        return "银行帐户";

                    case 2:
                        return "支付宝";

                    case 3:
                        return "财付通";
                }
                return str;
            }
        }

        public string provinceCode
        {
            get
            {
                return this._provincecode;
            }
            set
            {
                this._provincecode = value;
            }
        }

        public AcctChangeEnum status
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

        public DateTime? SureTime
        {
            get
            {
                return this._suretime;
            }
            set
            {
                this._suretime = value;
            }
        }

        public int? SureUser
        {
            get
            {
                return this._sureuser;
            }
            set
            {
                this._sureuser = value;
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

