namespace OriginalStudio.Model.User
{
    using System;

    [Serializable]
    public class MchUserPayBankInfo
    {
        #region 字段

        private System.Int32 _id = 0;
        private System.Int32 _userid = 0;
        private System.Int32 _accounttype = 0;
        private System.Int32 _pmode = 0;
        private System.String _bankaccount = string.Empty;
        private System.String _accountname = string.Empty;
        private System.String _bankname = string.Empty;
        private System.Int32 _bankcode = 0;
        private System.String _bankcity = string.Empty;
        private System.String _bankaddress = string.Empty;
        private System.Int32 _active = 0;
        private System.DateTime _addtime = System.DateTime.Now;
        private System.String _provincecode = string.Empty;
        private System.String _citycode = string.Empty;
        private System.Int32 _listorder = 0;
        private System.Int32 _isdefault = 0;

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
        /// 设置或获取商户ID
        /// </summary>
        public System.Int32 UserID
        {
            set { this._userid = value; }
            get { return this._userid; }
        }

        /// <summary>
        /// 设置或获取账户类型。1对公 0对私
        /// </summary>
        public System.Int32 AccountType
        {
            set { this._accounttype = value; }
            get { return this._accounttype; }
        }

        /// <summary>
        /// 设置或获取收款方式。（这个前台忽略掉） 1 银行帐户 2 支付宝 3 财付通
        /// </summary>
        public System.Int32 PMode
        {
            set { this._pmode = value; }
            get { return this._pmode; }
        }

        /// <summary>
        /// 设置或获取银行卡账户
        /// </summary>
        public System.String BankAccount
        {
            set { this._bankaccount = value; }
            get { return this._bankaccount.Trim(); }
        }

        /// <summary>
        /// 设置或获取账户姓名
        /// </summary>
        public System.String AccountName
        {
            set { this._accountname = value; }
            get { return this._accountname.Trim(); }
        }

        /// <summary>
        /// 设置或获取银行名称
        /// </summary>
        public System.String BankName
        {
            set { this._bankname = value; }
            get { return this._bankname.Trim(); }
        }

        /// <summary>
        /// 设置或获取银行代码
        /// </summary>
        public System.Int32 BankCode
        {
            set { this._bankcode = value; }
            get { return this._bankcode; }
        }

        /// <summary>
        /// 设置或获取银行城市
        /// </summary>
        public System.String BankCity
        {
            set { this._bankcity = value; }
            get { return this._bankcity.Trim(); }
        }

        /// <summary>
        /// 设置或获取开户行地址
        /// </summary>
        public System.String BankAddress
        {
            set { this._bankaddress = value; }
            get { return this._bankaddress.Trim(); }
        }

        /// <summary>
        /// 设置或获取是否启用
        /// </summary>
        public System.Int32 Active
        {
            set { this._active = value; }
            get { return this._active; }
        }

        /// <summary>
        /// 设置或获取添加时间
        /// </summary>
        public System.DateTime AddTime
        {
            set { this._addtime = value; }
            get { return this._addtime; }
        }

        /// <summary>
        /// 设置或获取省份代码
        /// </summary>
        public System.String ProvinceCode
        {
            set { this._provincecode = value; }
            get { return this._provincecode.Trim(); }
        }

        /// <summary>
        /// 设置或获取城市代码
        /// </summary>
        public System.String CityCode
        {
            set { this._citycode = value; }
            get { return this._citycode.Trim(); }
        }

        /// <summary>
        /// 设置或获取排序
        /// </summary>
        public System.Int32 ListOrder
        {
            set { this._listorder = value; }
            get { return this._listorder; }
        }

        /// <summary>
        /// 设置或获取默认银行卡
        /// </summary>
        public System.Int32 IsDefault
        {
            set { this._isdefault = value; }
            get { return this._isdefault; }
        }

        #endregion 公开属性
    }
}

