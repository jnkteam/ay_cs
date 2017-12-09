namespace OriginalStudio.Model.Sys
{
    using System;

    [Serializable]
    public class SysRunLog
    {
        #region 字段

        private System.Int32 _id = 0;
        private System.Int32 _logtype = 0;
        private System.Int32 _userid = 0;
        private System.String _merchantname = string.Empty;
        private System.String _userorder = string.Empty;
        private System.String _url = string.Empty;
        private System.String _errorcode = string.Empty;
        private System.String _errorinfo = string.Empty;
        private System.String _detail = string.Empty;
        private System.DateTime _addtime = System.DateTime.Now;

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
        public System.Int32 Logtype
        {
            set { this._logtype = value; }
            get { return this._logtype; }
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
        public System.String MerchantName
        {
            set { this._merchantname = value; }
            get { return this._merchantname; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String UserOrder
        {
            set { this._userorder = value; }
            get { return this._userorder.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String Url
        {
            set { this._url = value; }
            get { return this._url.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String ErrorCode
        {
            set { this._errorcode = value; }
            get { return this._errorcode.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String ErrorInfo
        {
            set { this._errorinfo = value; }
            get { return this._errorinfo.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String Detail
        {
            set { this._detail = value; }
            get { return this._detail.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.DateTime AddTime
        {
            set { this._addtime = value; }
            get { return this._addtime; }
        }

        #endregion 公开属性
    }
}

