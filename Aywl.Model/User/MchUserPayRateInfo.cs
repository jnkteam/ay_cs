using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OriginalStudio.Model.User
{
    /// <summary>
    /// 商户自定义费率对象。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class MchUserPayRateInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MchUserPayRateInfo()
        {
        }

        #region 字段

        private System.Int32 _userid = 0;
        private System.Int32 _defaultpay = 0;
        private System.Int32 _special = 0;
        private System.String _payratexml = "";
        private System.Boolean _istransfer = false;
        private System.Boolean _isrequireagentdistaudit = false;
        private UserLevelEnum _userlevel = UserLevelEnum.普通商家; 

        #endregion 字段

        #region 公开属性
        /// <summary>
        /// 设置或获取商户ID
        /// </summary>
        public System.Int32 UserID
        {
            set { this._userid = value; }
            get { return this._userid; }
        }

        /// <summary>
        /// 设置或获取（暂时不用）
        /// </summary>
        public System.Int32 DefaultPay
        {
            set { this._defaultpay = value; }
            get { return this._defaultpay; }
        }
        
        /// <summary>
        /// 设置或获取是启用。   不启用0，启用1
        /// </summary>
        public System.Int32 Special
        {
            set { this._special = value; }
            get { return this._special; }
        }
        
        /// <summary>
        /// 设置或获取XML配置
        /// </summary>
        public System.String  PayrateXML
        {
            set { this._payratexml = value; }
            get { return this._payratexml; }
        }
        
        /// <summary>
        /// 设置或获取（暂时不用）
        /// </summary>
        public System.Boolean Istransfer
        {
            set { this._istransfer = value; }
            get { return this._istransfer; }
        }

        /// <summary>
        /// 设置或获取（暂时不用）
        /// </summary>
        public System.Boolean IsRequireAgentDistAudit
        {
            set { this._isrequireagentdistaudit = value; }
            get { return this._isrequireagentdistaudit; }
        }

        /// <summary>
        /// 设置或获取（暂时不用）
        /// </summary>
        public UserLevelEnum UserLevel
        {
            set { this._userlevel = value; }
            get { return this._userlevel; }
        }

        #endregion 公开属性

    }
}
