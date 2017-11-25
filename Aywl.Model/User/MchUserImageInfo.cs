namespace OriginalStudio.Model.User
{
    using OriginalStudio.Model.Enum;
    using System;

    /// <summary>
    /// 商户图片信息
    /// </summary>
    public class MchUserImageInfo
    {
        public MchUserImageInfo()
        { }
        #region 字段

        private System.Int32 _id = 0;
        private System.Int32 _userid = 0;
        private System.String _imagefile = string.Empty;
        private System.Byte[] _imagestream = null;
        private System.Int32 _imagetype = 0;
        private System.String _imagedesc = string.Empty;
        private IdImageStatus _status = IdImageStatus.未知;
        private System.DateTime _addtime = System.DateTime.Now;
        private System.Int32 _checkuser = 0;
        private System.DateTime _checktime = System.DateTime.Now;

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
        /// 设置或获取UserID
        /// </summary>
        public System.Int32 UserID
        {
            set { this._userid = value; }
            get { return this._userid; }
        }

        /// <summary>
        /// 设置或获取文件名。在服务器上的相对目录
        /// </summary>
        public System.String ImageFile
        {
            set { this._imagefile = value; }
            get { return this._imagefile.Trim(); }
        }

        /// <summary>
        /// 设置或获取图片字节流。图片要用byte字节流存在数据库中
        /// </summary>
        public System.Byte[] ImageStream
        {
            set { this._imagestream = value; }
            get { return this._imagestream; }
        }

        /// <summary>
        /// 设置或获取图片类型
        /// </summary>
        public System.Int32 ImageType
        {
            set { this._imagetype = value; }
            get { return this._imagetype; }
        }

        /// <summary>
        /// 设置或获取图片描述
        /// </summary>
        public System.String ImageDesc
        {
            set { this._imagedesc = value; }
            get { return this._imagedesc.Trim(); }
        }

        /// <summary>
        /// 设置或获取状态图片状态
        /// </summary>
        public IdImageStatus Status
        {
            set { this._status = value; }
            get { return this._status; }
        }

        /// <summary>
        /// 设置或获取图片添加时间
        /// </summary>
        public System.DateTime AddTime
        {
            set { this._addtime = value; }
            get { return this._addtime; }
        }

        /// <summary>
        /// 设置或获取图片审核人
        /// </summary>
        public System.Int32 CheckUser
        {
            set { this._checkuser = value; }
            get { return this._checkuser; }
        }

        /// <summary>
        /// 设置或获取图片审核时间
        /// </summary>
        public System.DateTime CheckTime
        {
            set { this._checktime = value; }
            get { return this._checktime; }
        }

        #endregion 公开属性
    }
}

