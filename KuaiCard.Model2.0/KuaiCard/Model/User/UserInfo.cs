namespace KuaiCard.Model.User
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class UserInfo
    {
        private int _id = 0;

        private string _username = string.Empty;

        private string _password = string.Empty;

        private string _password2 = string.Empty;

        private string _msn = string.Empty;

        private string _qq = string.Empty;

        private string _tel = string.Empty;

        private string _fax = string.Empty;

        private string _email = string.Empty;

        private string _lastloginip = string.Empty;

        private string _lastloginaddress = string.Empty;

        private string _lastloginremark = string.Empty;

        private string _payeebank = string.Empty;

        private string _payeename = string.Empty;

        private string _account;

        private string _bankAddress = string.Empty;

        private string _bankProvince = string.Empty;

        private string _bankCity = string.Empty;

        private string _siteName = string.Empty;

        private string _siteUrl = string.Empty;

        private Int64 _idCardType = 1;

        private string _idCard = string.Empty;

        private Int64 _status = 0;

        private Int64 _cpsdrate = 0;

        private Int64 _cvsnrate = 0;

        private Int64 _integral;

        private Int64 _agentId = 0;

        private Int64 _pmode = 0;

        private Int32 _settles = 1;

        private Int64 _isdebug = 1;

        private DateTime _regtime;

        private DateTime _lastlogintime;

        private decimal _balance;

        private decimal _payment;

        private decimal _unpayment;

        private long _apiaccount = 0L;

        private string _apikey = string.Empty;

        private int _maxdayToCashTimes = 0;

        private UserLevelEnum _userLevel = UserLevelEnum.普通商家;

        private UserTypeEnum _userType = UserTypeEnum.会员;

        private string _desc = string.Empty;

        private int _accouttype = 0;

        private string _bankcode = string.Empty;

        private string _provincecode = string.Empty;

        private string _citycode = string.Empty;

        private Int64 _isagentDistribution = 0;

        private Int64 _agentDistscheme = 0;

        private byte _cardversion = 1;

        private string _frontPic;

        private string _versoPic;

        private byte _loginType = 0;

        private decimal _enableAmt = 0m;

        private string _full_name = string.Empty;

        private string _male = string.Empty;

        private string _province = string.Empty;

        private string _city = string.Empty;

        private string _zip = string.Empty;

        private string _addtress = string.Empty;

        private string _field1 = string.Empty;

        private int _settles_type = 0;      //结算途径。2017.2.13增加

        private decimal _bank_limit = 0m;       //限额  2017.2.15add
        private decimal _wx_limit = 0m;       //限额  2017.2.15add
        private decimal _ali_limit = 0m;       //限额  2017.2.15add
        private decimal _qq_limit = 0m;       //限额  2017.2.15add

        private int _random_subject = 0;        //2017.3.26随机指定商品信息
        private int _service_channel = 0;

        private string _login_mac = "";         //2017.6.29添加
        //=============================

        private string _default_themes = string.Empty;

        private UserFromPartners _Partners = UserFromPartners.网站;

        private string _linkman = string.Empty;

        public string Account
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

        public string addtress
        {
            get
            {
                return this._addtress;
            }
            set
            {
                this._addtress = value;
            }
        }

        public Int64 agentDistscheme
        {
            get
            {
                return this._agentDistscheme;
            }
            set
            {
                this._agentDistscheme = value;
            }
        }

        public Int64 AgentId
        {
            get
            {
                return this._agentId;
            }
            set
            {
                this._agentId = value;
            }
        }

        public string answer
        {
            get;
            set;
        }

        public long APIAccount
        {
            get
            {
                return this._apiaccount;
            }
            set
            {
                this._apiaccount = value;
            }
        }

        public string APIKey
        {
            get
            {
                return this._apikey;
            }
            set
            {
                this._apikey = value;
            }
        }

        public decimal Balance
        {
            get
            {
                return this._balance;
            }
            set
            {
                this._balance = value;
            }
        }

        public string BankAddress
        {
            get
            {
                return this._bankAddress;
            }
            set
            {
                this._bankAddress = value;
            }
        }

        public string BankCity
        {
            get
            {
                return this._bankCity;
            }
            set
            {
                this._bankCity = value;
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

        public string BankProvince
        {
            get
            {
                return this._bankProvince;
            }
            set
            {
                this._bankProvince = value;
            }
        }

        public DateTime birthday
        {
            get;
            set;
        }

        public byte cardversion
        {
            get
            {
                return this._cardversion;
            }
            set
            {
                this._cardversion = value;
            }
        }

        public string city
        {
            get
            {
                return this._city;
            }
            set
            {
                this._city = value;
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

        public Int64 classid
        {
            get;
            set;
        }

        public Int64 CPSDrate
        {
            get
            {
                return this._cpsdrate;
            }
            set
            {
                this._cpsdrate = value;
            }
        }

        public Int64 CVSNrate
        {
            get
            {
                return this._cvsnrate;
            }
            set
            {
                this._cvsnrate = value;
            }
        }

        public string Desc
        {
            get
            {
                return this._desc;
            }
            set
            {
                this._desc = value;
            }
        }

        public string Email
        {
            get
            {
                return this._email;
            }
            set
            {
                this._email = value;
            }
        }

        public decimal enableAmt
        {
            get
            {
                return this._enableAmt;
            }
            set
            {
                this._enableAmt = value;
            }
        }

        public string fax
        {
            get
            {
                return this._fax;
            }
            set
            {
                this._fax = value;
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

        public string frontPic
        {
            get
            {
                return this._frontPic;
            }
            set
            {
                this._frontPic = value;
            }
        }

        public string full_name
        {
            get
            {
                return this._full_name;
            }
            set
            {
                this._full_name = value;
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

        public string IdCard
        {
            get
            {
                return this._idCard;
            }
            set
            {
                this._idCard = value;
            }
        }

        public Int64 IdCardType
        {
            get
            {
                return this._idCardType;
            }
            set
            {
                this._idCardType = value;
            }
        }

        public Int64 Integral
        {
            get
            {
                return this._integral;
            }
            set
            {
                this._integral = value;
            }
        }

        public Int64 isagentDistribution
        {
            get
            {
                return this._isagentDistribution;
            }
            set
            {
                this._isagentDistribution = value;
            }
        }

        public Int64 isdebug
        {
            get
            {
                return this._isdebug;
            }
            set
            {
                this._isdebug = value;
            }
        }

        public Int64 IsEmailPass
        {
            get;
            set;
        }

        public Int64 IsPhonePass
        {
            get;
            set;
        }

        public Int64 IsRealNamePass
        {
            get;
            set;
        }

        public string LastLoginAddress
        {
            get
            {
                return this._lastloginaddress;
            }
            set
            {
                this._lastloginaddress = value;
            }
        }

        public string LastLoginIp
        {
            get
            {
                return this._lastloginip;
            }
            set
            {
                this._lastloginip = value;
            }
        }

        public string LastLoginRemark
        {
            get
            {
                return this._lastloginremark;
            }
            set
            {
                this._lastloginremark = value;
            }
        }

        public DateTime LastLoginTime
        {
            get
            {
                return this._lastlogintime;
            }
            set
            {
                this._lastlogintime = value;
            }
        }

        public string LinkMan
        {
            get
            {
                return this._linkman;
            }
            set
            {
                this._linkman = value;
            }
        }

        public byte loginType
        {
            get
            {
                return this._loginType;
            }
            set
            {
                this._loginType = value;
            }
        }

        public string male
        {
            get
            {
                return this._male;
            }
            set
            {
                this._male = value;
            }
        }

        public int? manageId
        {
            get;
            set;
        }

        public int MaxDayToCashTimes
        {
            get
            {
                return this._maxdayToCashTimes;
            }
            set
            {
                this._maxdayToCashTimes = value;
            }
        }

        public string msn
        {
            get
            {
                return this._msn;
            }
            set
            {
                this._msn = value;
            }
        }

        public noRefCheckStatusEnum noRefCheckStatus
        {
            get;
            set;
        }

        public UserFromPartners Partners
        {
            get
            {
                return this._Partners;
            }
            set
            {
                this._Partners = value;
            }
        }

        public string Password
        {
            get
            {
                return this._password;
            }
            set
            {
                this._password = value;
            }
        }

        public string Password2
        {
            get
            {
                return this._password2;
            }
            set
            {
                this._password2 = value;
            }
        }

        public string PayeeBank
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

        public string PayeeName
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

        public decimal Payment
        {
            get
            {
                return this._payment;
            }
            set
            {
                this._payment = value;
            }
        }

        public Int64 PMode
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

        public string PModeName
        {
            get
            {
                string _name = string.Empty;
                switch (this.PMode)
                {
                    case 1:
                        _name = "银行帐户";
                        break;
                    case 2:
                        _name = "支付宝";
                        break;
                    case 3:
                        _name = "财付通";
                        break;
                }
                return _name;
            }
        }

        public string province
        {
            get
            {
                return this._province;
            }
            set
            {
                this._province = value;
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

        public string QQ
        {
            get
            {
                return this._qq;
            }
            set
            {
                this._qq = value;
            }
        }

        public string question
        {
            get;
            set;
        }

        public DateTime RegTime
        {
            get
            {
                return this._regtime;
            }
            set
            {
                this._regtime = value;
            }
        }

        public Int32 Settles
        {
            get
            {
                return this._settles;
            }
            set
            {
                this._settles = value;
            }
        }

        public string SiteName
        {
            get
            {
                return this._siteName;
            }
            set
            {
                this._siteName = value;
            }
        }

        public string SiteUrl
        {
            get
            {
                return this._siteUrl;
            }
            set
            {
                this._siteUrl = value;
            }
        }

        public string smsNotifyUrl
        {
            get;
            set;
        }

        public Int64 Status
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

        public string Tel
        {
            get
            {
                return this._tel;
            }
            set
            {
                this._tel = value;
            }
        }

        public decimal Unpayment
        {
            get
            {
                return this._unpayment;
            }
            set
            {
                this._unpayment = value;
            }
        }

        public UserLevelEnum UserLevel
        {
            get
            {
                return this._userLevel;
            }
            set
            {
                this._userLevel = value;
            }
        }

        public string UserName
        {
            get
            {
                return this._username;
            }
            set
            {
                this._username = value;
            }
        }

        public UserTypeEnum UserType
        {
            get
            {
                return this._userType;
            }
            set
            {
                this._userType = value;
            }
        }

        public string versoPic
        {
            get
            {
                return this._versoPic;
            }
            set
            {
                this._versoPic = value;
            }
        }

        public string zip
        {
            get
            {
                return this._zip;
            }
            set
            {
                this._zip = value;
            }
        }

        /// <summary>
        /// 结算途径。0：前台；1：接口；2：前台+接口
        /// </summary>
        public int settles_type
        {
            get
            {
                return this._settles_type;
            }
            set
            {
                this._settles_type = value;
            }
        }

        /// <summary>
        /// 网银限额
        /// </summary>
        public decimal bank_limit
        {
            get
            {
                return this._bank_limit;
            }
            set
            {
                this._bank_limit = value;
            }
        }

        /// <summary>
        /// 微信限额
        /// </summary>
        public decimal wx_limit
        {
            get
            {
                return this._wx_limit;
            }
            set
            {
                this._wx_limit = value;
            }
        }

        /// <summary>
        /// 支付宝限额
        /// </summary>
        public decimal ali_limit
        {
            get
            {
                return this._ali_limit;
            }
            set
            {
                this._ali_limit = value;
            }
        }

        /// <summary>
        /// QQ钱包限额
        /// </summary>
        public decimal qq_limit
        {
            get
            {
                return this._qq_limit;
            }
            set
            {
                this._qq_limit = value;
            }
        }

        /// <summary>
        /// 随机指定商品信息。1:随机指定;0:不随机
        /// </summary>
        public int random_subject
        {
            get
            {
                return this._random_subject;
            }
            set
            {
                this._random_subject = value;
            }
        }

        /// <summary>
        /// 是否启用供应商独立通道。1:独立通道;0:公共通道
        /// </summary>
        public int service_channel
        {
            get
            {
                return this._service_channel;
            }
            set
            {
                this._service_channel = value;
            }
        }


        public string default_themes
        {
            get { return this._default_themes; }
            set { this._default_themes = value; }
        }

        public string login_mac 
        {
            get { return this._login_mac; }
            set { this._login_mac = value; }
        }
    }
}

