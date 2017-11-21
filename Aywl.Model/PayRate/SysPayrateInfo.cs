namespace OriginalStudio.Model.PayRate
{
    using System;
    using OriginalStudio.Model.PayRate;

    /// <summary>
    /// 系统定义费率。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class SysPayRateInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SysPayRateInfo()
        {
        }

        #region 字段

        private System.Int32 _id = 0;
        private System.String _ratename = string.Empty;
        private RateTypeEnum _ratetype = 0;
        private System.Int32 _active = 0;
        private System.Int32 _createuserid = 0;
        private System.DateTime _createtime = System.DateTime.Now;
        private System.String _payratexml = "";
        private System.Int32 _userlevel = 0;

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
        public System.String RateName
        {
            set { this._ratename = value; }
            get { return this._ratename.Trim(); }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public RateTypeEnum RateType
        {
            set { this._ratetype = value; }
            get { return this._ratetype; }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 Active
        {
            set { this._active = value; }
            get { return this._active; }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 CreateUserID
        {
            set { this._createuserid = value; }
            get { return this._createuserid; }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.DateTime CreateTime
        {
            set { this._createtime = value; }
            get { return this._createtime; }
        }
        
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String  PayrateXML
        {
            set { this._payratexml = value; }
            get { return this._payratexml; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 UserLevel
        {
            set { this._userlevel = value; }
            get { return this._userlevel; }
        }
        #endregion 公开属性

    }
}

