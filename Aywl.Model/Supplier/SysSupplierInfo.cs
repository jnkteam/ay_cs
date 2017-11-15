namespace OriginalStudio.Model.Supplier
{
    using System;

    /// <summary>
    /// 通道供应商
    /// </summary>
    [Serializable]
    public class SysSupplierInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SysSupplierInfo()
        {
        }

        #region 字段

        private System.Int32 _id = 0;
        private System.Int32 _suppliercode = 0;
        private System.String _suppliername = string.Empty;
        private System.String _logourl = string.Empty;
        private System.Boolean _isbank = false;
        private System.Boolean _isalipay = false;
        private System.Boolean _isqq = false;
        private System.Boolean _isweixin = false;
        private System.Boolean _isjd = false;
        private System.String _puserid = string.Empty;
        private System.String _puserkey = string.Empty;
        private System.String _pusername = string.Empty;
        private System.String _puserparm1 = string.Empty;
        private System.String _puserparm2 = string.Empty;
        private System.String _puserparm3 = string.Empty;
        private System.String _puserparm4 = string.Empty;
        private System.Boolean _active = false;
        private System.Boolean _isdebug = false;
        private System.String _bankposturl = string.Empty;
        private System.String _banknotifyurl = string.Empty;
        private System.String _bankreturnurl = string.Empty;
        private System.String _banksearchurl = string.Empty;
        private System.String _bankjumurl = string.Empty;
        private System.String _distributionurl = string.Empty;
        private System.String _distributionnotifyurl = string.Empty;
        private System.String _distributionsearchurl = string.Empty;
        private System.String _spdesc = string.Empty;
        private System.Int32 _listorder = 0;
        private System.Boolean _isdistribution = false;

        #endregion 字段

        #region 公开属性
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 ID
        {
            set { this._id = value; }
            get { return this._id; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 SupplierCode
        {
            set { this._suppliercode = value; }
            get { return this._suppliercode; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String SupplierName
        {
            set { this._suppliername = value; }
            get { return this._suppliername.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String LogoUrl
        {
            set { this._logourl = value; }
            get { return this._logourl.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Boolean IsBank
        {
            set { this._isbank = value; }
            get { return this._isbank; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Boolean IsAlipay
        {
            set { this._isalipay = value; }
            get { return this._isalipay; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Boolean IsQQ
        {
            set { this._isqq = value; }
            get { return this._isqq; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Boolean IsWeiXin
        {
            set { this._isweixin = value; }
            get { return this._isweixin; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Boolean IsJD
        {
            set { this._isjd = value; }
            get { return this._isjd; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String PUserID
        {
            set { this._puserid = value; }
            get { return this._puserid.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String PUserKey
        {
            set { this._puserkey = value; }
            get { return this._puserkey.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String PUserName
        {
            set { this._pusername = value; }
            get { return this._pusername.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String PUserParm1
        {
            set { this._puserparm1 = value; }
            get { return this._puserparm1.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String PUserParm2
        {
            set { this._puserparm2 = value; }
            get { return this._puserparm2.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String PUserParm3
        {
            set { this._puserparm3 = value; }
            get { return this._puserparm3.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String PUserParm4
        {
            set { this._puserparm4 = value; }
            get { return this._puserparm4.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Boolean Active
        {
            set { this._active = value; }
            get { return this._active; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Boolean IsDebug
        {
            set { this._isdebug = value; }
            get { return this._isdebug; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String BankPostUrl
        {
            set { this._bankposturl = value; }
            get { return this._bankposturl.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String BankNotifyUrl
        {
            set { this._banknotifyurl = value; }
            get { return this._banknotifyurl.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String BankReturnUrl
        {
            set { this._bankreturnurl = value; }
            get { return this._bankreturnurl.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String BankSearchUrl
        {
            set { this._banksearchurl = value; }
            get { return this._banksearchurl.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String BankJumUrl
        {
            set { this._bankjumurl = value; }
            get { return this._bankjumurl.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String DistributionUrl
        {
            set { this._distributionurl = value; }
            get { return this._distributionurl.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String DistributionNotifyUrl
        {
            set { this._distributionnotifyurl = value; }
            get { return this._distributionnotifyurl.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String DistributionSearchUrl
        {
            set { this._distributionsearchurl = value; }
            get { return this._distributionsearchurl.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String SpDesc
        {
            set { this._spdesc = value; }
            get { return this._spdesc.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 ListOrder
        {
            set { this._listorder = value; }
            get { return this._listorder; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Boolean IsDistribution
        {
            set { this._isdistribution = value; }
            get { return this._isdistribution; }
        }

        #endregion 公开属性
    }
}

