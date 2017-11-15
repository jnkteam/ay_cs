namespace OriginalStudio.Model.Channel
{
    using System;

    /// <summary>
    /// 接口通道实体类。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class SysChannelInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SysChannelInfo()
        {
        }

        #region 字段

        private System.Int32 _id = 0;
        private System.String _channelcode = string.Empty;
        private System.Int32 _channeltypeid = 0;
        private System.Int32 _suppliercode = 0;
        private System.Decimal _supplierrate = 0;
        private System.String _channelname = string.Empty;
        private System.String _channelenname = string.Empty;
        private System.Int32 _facevalue = 0;
        private System.Boolean _isopen = false;
        private System.DateTime _addtime = System.DateTime.Now;
        private System.Int32 _listsort = 0;
        private System.Int32 _createuserid = 0;
        private System.DateTime _createtime = System.DateTime.Now;

        #endregion 字段

        #region 公开属性
        /// <summary>
        /// 设置或获取ID。
        /// </summary>
        public System.Int32 ID
        {
            set { this._id = value; }
            get { return this._id; }
        }

        /// <summary>
        /// 设置或获取通道代码。
        /// </summary>
        public System.String ChannelCode
        {
            set { this._channelcode = value; }
            get { return this._channelcode.Trim(); }
        }

        /// <summary>
        /// 设置或获取通道类型。
        /// </summary>
        public System.Int32 ChannelTypeId
        {
            set { this._channeltypeid = value; }
            get { return this._channeltypeid; }
        }

        /// <summary>
        /// 设置或获取通道供应商。
        /// </summary>
        public System.Int32 SupplierCode
        {
            set { this._suppliercode = value; }
            get { return this._suppliercode; }
        }

        /// <summary>
        /// 设置或获取供应商费率。
        /// </summary>
        public System.Decimal SupplierRate
        {
            set { this._supplierrate = value; }
            get { return this._supplierrate; }
        }

        /// <summary>
        /// 设置或获取通道名称
        /// </summary>
        public System.String ChannelName
        {
            set { this._channelname = value; }
            get { return this._channelname.Trim(); }
        }

        /// <summary>
        /// 设置或获取通道英文名称
        /// </summary>
        public System.String ChannelEnName
        {
            set { this._channelenname = value; }
            get { return this._channelenname.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 FaceValue
        {
            set { this._facevalue = value; }
            get { return this._facevalue; }
        }

        /// <summary>
        /// 设置或获取是否开启通道
        /// </summary>
        public System.Boolean IsOpen
        {
            set { this._isopen = value; }
            get { return this._isopen; }
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
        /// 设置或获取排序
        /// </summary>
        public System.Int32 ListSort
        {
            set { this._listsort = value; }
            get { return this._listsort; }
        }

        /// <summary>
        /// 设置或获取创建人
        /// </summary>
        public System.Int32 CreateUserID
        {
            set { this._createuserid = value; }
            get { return this._createuserid; }
        }

        /// <summary>
        /// 设置或获取创建时间
        /// </summary>
        public System.DateTime CreateTime
        {
            set { this._createtime = value; }
            get { return this._createtime; }
        }

        #endregion 公开属性

    }
}

