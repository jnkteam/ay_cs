namespace OriginalStudio.Model
{
    using System;

    /// <summary>
    /// 供应商通道费率设置。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class SysSupplierPayRateInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SysSupplierPayRateInfo()
        {
        }

        #region 字段

        private System.Int32 _suppliercode = 0;
        private System.String _payratexml = "";
        
        #endregion 字段

        #region 公开属性
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
        public System.String PayrateXML
        {
            set { this._payratexml = value; }
            get { return this._payratexml; }
        }
        
        #endregion 公开属性
    }
}

