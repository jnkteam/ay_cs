﻿using System;

namespace OriginalStudio.Model.User
{
    /// <summary>
    /// 用户基本信息。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class MchUserBaseInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MchUserBaseInfo()
        {
        }

        #region 字段

        private System.Int32 _userid = 0;
        private System.Int32 _classid = 0;
        private System.String _username = string.Empty;
        private System.String _userpwd = string.Empty;
        private System.String _userpaypwd = string.Empty;
        private System.String _merchantname = string.Empty;
        private System.String _apikey = string.Empty;
        private System.String _contactname = string.Empty;
        private System.String _idcard = string.Empty;
        private System.String _phone = string.Empty;
        private System.String _email = string.Empty;
        private System.String _qq = string.Empty;
        private System.Boolean _isphone = false;
        private System.Boolean _isemail = false;
        private System.Boolean _isrealname = false;
        private System.Int32 _withdrawschemeid = 0;
        private System.Int32 _payrateid = 0;
        private System.Int32 _maxdaywithdrawtimes = 0;
        private System.String _firstloginip = string.Empty;
        private System.String _firstloginmac = string.Empty;
        private System.String _lastloginip = string.Empty;
        private System.String _lastloginmac = string.Empty;
        private System.DateTime _firstlogintime = System.DateTime.Now;
        private System.DateTime _lastlogintime = System.DateTime.Now;
        private System.String _sessionid = string.Empty;
        private System.Int32 _status = 0;
        private System.DateTime _addtime = System.DateTime.Now;
        private System.String _company = string.Empty;
        private System.String _linkman = string.Empty;
        private System.Int32 _withdrawtype = 0;
        private System.Int32 _randomproduct = 0;
        private System.Int32 _manageid = 0;
        private System.String _siteurl = string.Empty;
        private System.String _frontpic = string.Empty;
        private System.String _versopic = string.Empty;
        private System.String _defaultthemes = string.Empty;
        private System.Int32 _isdebug = 0;
        private System.Int32 _agentid = 0;
        private System.Int32 _cpsdrate = 0;
        private System.Int64 _integral = 0;
        private System.Decimal _freeze = 0M;
        private System.Decimal _balance = 0M;
        private System.Decimal _payment = 0M;
        private System.Decimal _unpayment = 0M;
        private System.Decimal _unpayment2 = 0M;
        private System.Decimal _enableamt = 0M;
        private UserTypeEnum _usertype = UserTypeEnum.会员;
        private UserLevelEnum _userlevel = UserLevelEnum.普通商家;
        private String _lastloginaddress = "";
        private String _lastloginremark = "";
        private System.Int32 _logintype = 0;
        private String _token = "";
        private System.Int32 _channeltypeid = 0;
        private System.Decimal _minmoney = 0M;
        private System.Decimal _maxmoney = 0M;

        private System.Int32 _schemenetype = 0;
        private System.String _schemename = string.Empty;
        private System.Decimal _singleminamtlimit = 0M;
        private System.Decimal _singlemaxamtlimit = 0M;
        private System.Int32 _dailymaxtimes = 0;
        private System.Decimal _dailymaxamt = 0M;
        private System.Decimal _chargerate = 0M;
        private System.Decimal _singlemincharge = 0M;
        private System.Decimal _singlemaxcharge = 0M;
        private System.Int32 _istranapi = 0;
        private System.Int32 _isdefault = 0;
        private System.Int32 _issys = 0;
        private System.Int32 _bankdetentiondays = 0;
        private System.Int32 _qqdetentiondays = 0;
        private System.Int32 _jddetentiondays = 0;
        private System.Int32 _istranrequiredaudit = 0;
        private System.Int32 _alipaydetentiondays = 0;
        private System.Int32 _weixindetentiondays = 0;
        private System.Int32 _otherdetentiondays = 0;

        #endregion 字段

        #region 公开属性
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 UserID
        {
            set { this._userid = value; }
            get { return this._userid; }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 ClassID
        {
            set { this._classid = value; }
            get { return this._classid; }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String UserName
        {
            set { this._username = value; }
            get { return this._username.Trim(); }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String UserPwd
        {
            set { this._userpwd = value; }
            get { return this._userpwd.Trim(); }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String UserPayPwd
        {
            set { this._userpaypwd = value; }
            get { return this._userpaypwd.Trim(); }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String MerchantName
        {
            set { this._merchantname = value; }
            get { return this._merchantname.Trim(); }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String ApiKey
        {
            set { this._apikey = value; }
            get { return this._apikey.Trim(); }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String ContactName
        {
            set { this._contactname = value; }
            get { return this._contactname.Trim(); }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String IDCard
        {
            set { this._idcard = value; }
            get { return this._idcard.Trim(); }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String Phone
        {
            set { this._phone = value; }
            get { return this._phone.Trim(); }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String EMail
        {
            set { this._email = value; }
            get { return this._email.Trim(); }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String QQ
        {
            set { this._qq = value; }
            get { return this._qq.Trim(); }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Boolean IsPhone
        {
            set { this._isphone = value; }
            get { return this._isphone; }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Boolean IsEmail
        {
            set { this._isemail = value; }
            get { return this._isemail; }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Boolean IsRealName
        {
            set { this._isrealname = value; }
            get { return this._isrealname; }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 WithdrawSchemeID
        {
            set { this._withdrawschemeid = value; }
            get { return this._withdrawschemeid; }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 PayRateID
        {
            set { this._payrateid = value; }
            get { return this._payrateid; }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 MaxDayWithdrawTimes
        {
            set { this._maxdaywithdrawtimes = value; }
            get { return this._maxdaywithdrawtimes; }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String FirstLoginIP
        {
            set { this._firstloginip = value; }
            get { return this._firstloginip.Trim(); }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String FirstLoginMac
        {
            set { this._firstloginmac = value; }
            get { return this._firstloginmac.Trim(); }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String LastLoginIP
        {
            set { this._lastloginip = value; }
            get { return this._lastloginip.Trim(); }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String LastLoginMAC
        {
            set { this._lastloginmac = value; }
            get { return this._lastloginmac.Trim(); }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String SessionID
        {
            set { this._sessionid = value; }
            get { return this._sessionid.Trim(); }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 Status
        {
            set { this._status = value; }
            get { return this._status; }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.DateTime AddTime
        {
            set { this._addtime = value; }
            get { return this._addtime; }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String Company
        {
            set { this._company = value; }
            get { return this._company.Trim(); }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String LinkMan
        {
            set { this._linkman = value; }
            get { return this._linkman.Trim(); }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 WithdrawType
        {
            set { this._withdrawtype = value; }
            get { return this._withdrawtype; }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 RandomProduct
        {
            set { this._randomproduct = value; }
            get { return this._randomproduct; }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 ManageId
        {
            set { this._manageid = value; }
            get { return this._manageid; }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String SiteUrl
        {
            set { this._siteurl = value; }
            get { return this._siteurl.Trim(); }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String FrontPic
        {
            set { this._frontpic = value; }
            get { return this._frontpic.Trim(); }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String VersoPic
        {
            set { this._versopic = value; }
            get { return this._versopic.Trim(); }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String DefaultThemes
        {
            set { this._defaultthemes = value; }
            get { return this._defaultthemes.Trim(); }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 IsDebug
        {
            set { this._isdebug = value; }
            get { return this._isdebug; }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 AgentID
        {
            set { this._agentid = value; }
            get { return this._agentid; }
        }


        /// <summary>
        /// 设置或获取随机扣单几率
        /// </summary>
        public System.Int32 CPSDrate
        {
            set { this._cpsdrate = value; }
            get { return this._cpsdrate; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public UserTypeEnum UserType
        {
            set { this._usertype = value; }
            get { return this._usertype; }
        }

        /// <summary>
        /// 设置或获取会员等级
        /// </summary>
        public UserLevelEnum UserLevel
        {
            set { this._userlevel = value; }
            get { return this._userlevel; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String LastLoginAddress
        {
            set { this._lastloginaddress = value; }
            get { return this._lastloginaddress.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String LastLoginRemark
        {
            set { this._lastloginremark = value; }
            get { return this._lastloginremark.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 LoginType
        {
            get { return this._logintype; }
            set { this._logintype = value; }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.DateTime FirstLoginTime
        {
            set { this._firstlogintime = value; }
            get { return this._firstlogintime; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.DateTime LastLoginTime
        {
            set { this._lastlogintime = value; }
            get { return this._lastlogintime; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String Token
        {
            set { this._token = value; }
            get { return this._token.Trim(); }
        }

        #endregion 公开属性

        #region 账户金额属性


        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int64 Integral
        {
            set { this._integral = value; }
            get { return this._integral; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Decimal Freeze
        {
            set { this._freeze = value; }
            get { return this._freeze; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Decimal Balance
        {
            set { this._balance = value; }
            get { return this._balance; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Decimal Payment
        {
            set { this._payment = value; }
            get { return this._payment; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Decimal UnPayment
        {
            set { this._unpayment = value; }
            get { return this._unpayment; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Decimal UnPayment2
        {
            set { this._unpayment2 = value; }
            get { return this._unpayment2; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Decimal EnableAmt
        {
            set { this._enableamt = value; }
            get { return this._enableamt; }
        }

        #endregion

        #region 限额属性

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 ChannelTypeID
        {
            set { this._channeltypeid = value; }
            get { return this._channeltypeid; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Decimal MinMoney
        {
            set { this._minmoney = value; }
            get { return this._minmoney; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Decimal MaxMoney
        {
            set { this._maxmoney = value; }
            get { return this._maxmoney; }
        }

        #endregion

        #region 提现属性

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 SchemeneType
        {
            set { this._schemenetype = value; }
            get { return this._schemenetype; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String SchemeName
        {
            set { this._schemename = value; }
            get { return this._schemename.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Decimal SingleMinAmtLimit
        {
            set { this._singleminamtlimit = value; }
            get { return this._singleminamtlimit; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Decimal SingleMaxAmtLimit
        {
            set { this._singlemaxamtlimit = value; }
            get { return this._singlemaxamtlimit; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 DailyMaxTimes
        {
            set { this._dailymaxtimes = value; }
            get { return this._dailymaxtimes; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Decimal DailyMaxAmt
        {
            set { this._dailymaxamt = value; }
            get { return this._dailymaxamt; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Decimal ChargeRate
        {
            set { this._chargerate = value; }
            get { return this._chargerate; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Decimal SingleMinCharge
        {
            set { this._singlemincharge = value; }
            get { return this._singlemincharge; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Decimal SingleMaxCharge
        {
            set { this._singlemaxcharge = value; }
            get { return this._singlemaxcharge; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 IsTranApi
        {
            set { this._istranapi = value; }
            get { return this._istranapi; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 IsDefault
        {
            set { this._isdefault = value; }
            get { return this._isdefault; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 IsSys
        {
            set { this._issys = value; }
            get { return this._issys; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 BankDetentionDays
        {
            set { this._bankdetentiondays = value; }
            get { return this._bankdetentiondays; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 QQDetentionDays
        {
            set { this._qqdetentiondays = value; }
            get { return this._qqdetentiondays; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 JDDetentionDays
        {
            set { this._jddetentiondays = value; }
            get { return this._jddetentiondays; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 IsTranRequiredAudit
        {
            set { this._istranrequiredaudit = value; }
            get { return this._istranrequiredaudit; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 AlipayDetentionDays
        {
            set { this._alipaydetentiondays = value; }
            get { return this._alipaydetentiondays; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 WeiXinDetentionDays
        {
            set { this._weixindetentiondays = value; }
            get { return this._weixindetentiondays; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 OtherDetentionDays
        {
            set { this._otherdetentiondays = value; }
            get { return this._otherdetentiondays; }
        }


        #endregion
    }
}
