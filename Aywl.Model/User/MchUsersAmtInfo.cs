namespace OriginalStudio.Model.User
{
    using System;

    [Serializable]
    public class MchUsersAmtInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MchUsersAmtInfo()
        {
        }

        #region 字段

        private System.Int32 _id = 0;
        private System.Int32 _userid = 0;
        private System.Int64 _integral = 0;
        private System.Decimal _freeze = 0M;
        private System.Decimal _balance = 0M;
        private System.Decimal _payment = 0M;
        private System.Decimal _unpayment = 0M;
        private System.Decimal _unpayment2 = 0M;
        private System.Decimal _enableamt = 0M;

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
        public System.Int32 UserID
        {
            set { this._userid = value; }
            get { return this._userid; }
        }

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

        #endregion 公开属性
    }
}

