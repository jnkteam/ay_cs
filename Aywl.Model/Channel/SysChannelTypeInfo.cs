namespace OriginalStudio.Model.Channel
{
    using System;

    /// <summary>
    /// 通道类型对象。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class SysChannelTypeInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SysChannelTypeInfo()
        {
        }

        #region 字段

        private System.Int32 _id = 0;
        private System.Int32 _typeid = 0;
        private System.String _typecode = string.Empty;
        private System.Int32 _typeclassid = 0;
        private System.String _typename = string.Empty;
        private SysChannelTypeOpenEnum _isopen = SysChannelTypeOpenEnum.None;
        private System.Int32 _suppliercode = 0;
        private System.Decimal _supplierrate = 0M;
        private System.DateTime _addtime = System.DateTime.Now;
        private System.Int32 _listorder = 0;
        private System.Boolean _release = false;
        private System.Int32 _runmode = 0;
        private System.String _runmodeset = string.Empty;

        #endregion 字段

        #region 公开属性
        /// <summary>
        /// 设置或获取序号
        /// </summary>
        public System.Int32 ID
        {
            set { this._id = value; }
            get { return this._id; }
        }

        /// <summary>
        /// 设置或获取通道编号（自定义）
        /// </summary>
        public System.Int32 TypeID
        {
            set { this._typeid = value; }
            get { return this._typeid; }
        }

        /// <summary>
        /// 设置或获取通道名称代码
        /// </summary>
        public System.String TypeCode
        {
            set { this._typecode = value; }
            get { return this._typecode.Trim(); }
        }

        /// <summary>
        /// 设置或获取通道类别
        /// </summary>
        public System.Int32 TypeClassID
        {
            set { this._typeclassid = value; }
            get { return this._typeclassid; }
        }

        /// <summary>
        /// 设置或获取类型名称
        /// </summary>
        public System.String TypeName
        {
            set { this._typename = value; }
            get { return this._typename.Trim(); }
        }

        /// <summary>
        /// 设置或获取是否开启
        /// </summary>
        public SysChannelTypeOpenEnum IsOpen
        {
            set { this._isopen = value; }
            get { return this._isopen; }
        }

        /// <summary>
        /// 设置或获取默认供应商通道
        /// </summary>
        public System.Int32 SupplierCode
        {
            set { this._suppliercode = value; }
            get { return this._suppliercode; }
        }

        /// <summary>
        /// 设置或获取费率
        /// </summary>
        public System.Decimal SupplierRate
        {
            set { this._supplierrate = value; }
            get { return this._supplierrate; }
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
        /// 设置或获取序号
        /// </summary>
        public System.Int32 ListOrder
        {
            set { this._listorder = value; }
            get { return this._listorder; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Boolean Release
        {
            set { this._release = value; }
            get { return this._release; }
        }

        /// <summary>
        /// 设置或获取运行模式。0:单独;   1:轮询
        /// </summary>
        public System.Int32 RunMode
        {
            set { this._runmode = value; }
            get { return this._runmode; }
        }

        /// <summary>
        /// 设置或获取轮训模式配置
        /// </summary>
        public System.String RunModeSet
        {
            set { this._runmodeset = value; }
            get { return this._runmodeset.Trim(); }
        }

        #endregion 公开属性

    }
}

