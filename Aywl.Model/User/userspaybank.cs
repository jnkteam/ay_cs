namespace OriginalStudio.Model.User
{
    using System;

    [Serializable]
    public class userspaybank
    {
        private string _account;
        private int _accouttype = 0;
        private DateTime _addtime;
        private string _bankaddress;
        private string _bankcity;
        private string _bankcode;
        private string _bankprovince;
        private string _citycode;
        private int _id;
        private string _payeebank;
        private string _payeename;
        private int _pmode = 1;
        private string _provincecode;
        private int? _status;
        private DateTime? _updatetime;
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

        public DateTime? updateTime
        {
            get
            {
                return this._updatetime;
            }
            set
            {
                this._updatetime = value;
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

