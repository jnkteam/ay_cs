using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OriginalStudio.Model.Withdraw
{
    /// <summary>
    /// 结算方式
    /// </summary>
    public class WithdrawSchemeInfo
    {
        #region 字段

        private System.Int32 _id = 0;
        private System.Int32 _type = 0;
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
        private System.Int32 _transupplier = 0;
        

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
        /// 设置或获取方案适用类型。1是商户；2是代理
        /// </summary>
        public System.Int32 Type
        {
            set { this._type = value; }
            get { return this._type; }
        }

        /// <summary>
        /// 设置或获取方案名称 
        /// </summary>
        public System.String SchemeName
        {
            set { this._schemename = value; }
            get { return this._schemename.Trim(); }
        }

        /// <summary>
        /// 设置或获取最低提现金额限制(每笔) 
        /// </summary>
        public System.Decimal SingleMinAmtLimit
        {
            set { this._singleminamtlimit = value; }
            get { return this._singleminamtlimit; }
        }

        /// <summary>
        /// 设置或获取最大提现金额限制(每笔)
        /// </summary>
        public System.Decimal SingleMaxAmtLimit
        {
            set { this._singlemaxamtlimit = value; }
            get { return this._singlemaxamtlimit; }
        }

        /// <summary>
        /// 设置或获取每天最多可提现次数
        /// </summary>
        public System.Int32 DailyMaxTimes
        {
            set { this._dailymaxtimes = value; }
            get { return this._dailymaxtimes; }
        }

        /// <summary>
        /// 设置或获取每天最多提现金额
        /// </summary>
        public System.Decimal DailyMaxAmt
        {
            set { this._dailymaxamt = value; }
            get { return this._dailymaxamt; }
        }

        /// <summary>
        /// 设置或获每笔取提现手续费
        /// </summary>
        public System.Decimal ChargeRate
        {
            set { this._chargerate = value; }
            get { return this._chargerate; }
        }

        /// <summary>
        /// 设置或获取每笔提现手续费最少
        /// </summary>
        public System.Decimal SingleMinCharge
        {
            set { this._singlemincharge = value; }
            get { return this._singlemincharge; }
        }

        /// <summary>
        /// 设置或获取每笔提现手续费最高
        /// </summary>
        public System.Decimal SingleMaxCharge
        {
            set { this._singlemaxcharge = value; }
            get { return this._singlemaxcharge; }
        }

        /// <summary>
        /// 设置或获取是否走接口
        /// </summary>
        public System.Int32 IsTranApi
        {
            set { this._istranapi = value; }
            get { return this._istranapi; }
        }

        /// <summary>
        /// 设置或获取是否默认
        /// </summary>
        public System.Int32 IsDefault
        {
            set { this._isdefault = value; }
            get { return this._isdefault; }
        }

        /// <summary>
        /// 设置或获取是否默认
        /// </summary>
        public System.Int32 IsSys
        {
            set { this._issys = value; }
            get { return this._issys; }
        }

        /// <summary>
        /// 设置或获取网银T+几 
        /// </summary>
        public System.Int32 BankDetentionDays
        {
            set { this._bankdetentiondays = value; }
            get { return this._bankdetentiondays; }
        }

        /// <summary>
        /// 设置或获取QQT+几 
        /// </summary>
        public System.Int32 QQDetentionDays
        {
            set { this._qqdetentiondays = value; }
            get { return this._qqdetentiondays; }
        }

        /// <summary>
        /// 设置或获取京东T+几 
        /// </summary>
        public System.Int32 JDDetentionDays
        {
            set { this._jddetentiondays = value; }
            get { return this._jddetentiondays; }
        }

        /// <summary>
        /// 设置或获取是否需要审核
        /// </summary>
        public System.Int32 IsTranRequiredAudit
        {
            set { this._istranrequiredaudit = value; }
            get { return this._istranrequiredaudit; }
        }

        /// <summary>
        /// 设置或获取支付宝T+几 
        /// </summary>
        public System.Int32 AlipayDetentionDays
        {
            set { this._alipaydetentiondays = value; }
            get { return this._alipaydetentiondays; }
        }

        /// <summary>
        /// 设置或获取微信T+几 
        /// </summary>
        public System.Int32 WeiXinDetentionDays
        {
            set { this._weixindetentiondays = value; }
            get { return this._weixindetentiondays; }
        }

        /// <summary>
        /// 设置或获取其他类型T+几 
        /// </summary>
        public System.Int32 OtherDetentionDays
        {
            set { this._otherdetentiondays = value; }
            get { return this._otherdetentiondays; }
        }

        /// <summary>
        /// 设置或获取代付默认通道商
        /// </summary>
        public System.Int32 TranSupplier
        {
            set { this._transupplier = value; }
            get { return this._transupplier; }
        }
        
        #endregion 公开属性
    }
}
