namespace OriginalStudio.Model.Channel
{
    using System;

    /// <summary>
    /// 用户自定义通道设置。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class MchUserChannelType
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MchUserChannelType()
        {
        }

        #region 字段

        private System.Int32 _id = 0;
        private System.Int32 _typeid = 0;
        private System.Int32 _suppliercode = 0;
        private System.Int32 _userid = 0;
        private System.Boolean _userisopen = false;
        private System.Boolean _sysisopen = false;
        private System.DateTime _addtime = System.DateTime.Now;
        private System.DateTime _updatetime = System.DateTime.Now;

        #endregion 字段

        #region 公开属性
        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 Id
        {
            set { this._id = value; }
            get { return this._id; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 TypeID
        {
            set { this._typeid = value; }
            get { return this._typeid; }
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
        public System.Int32 UserId
        {
            set { this._userid = value; }
            get { return this._userid; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Boolean UserIsOpen
        {
            set { this._userisopen = value; }
            get { return this._userisopen; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Boolean SysIsOpen
        {
            set { this._sysisopen = value; }
            get { return this._sysisopen; }
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
        public System.DateTime UpdateTime
        {
            set { this._updatetime = value; }
            get { return this._updatetime; }
        }

        #endregion 公开属性

    }
}

