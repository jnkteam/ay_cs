using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OriginalStudio.Model.User
{
    /// <summary>
    /// 商户自定义通道账号。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    public class MchUserSupplier
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MchUserSupplier()
        {
        }

        #region 字段

        private System.Int32 _id = 0;
        private System.Int32 _userid = 0;
        private System.Int32 _suppliercode = 0;
        private System.String _puserid = string.Empty;
        private System.String _puserkey = string.Empty;
        private System.String _pusername = string.Empty;
        private System.String _puserparm1 = string.Empty;
        private System.String _puserparm2 = string.Empty;
        private System.Int32 _issingle = 0;
        private System.Int32 _istransfer = 0;
        private System.String _transferurl = string.Empty;

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
        public System.Int32 SupplierCode
        {
            set { this._suppliercode = value; }
            get { return this._suppliercode; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String PUserID
        {
            set { this._puserid = value; }
            get { return this._puserid.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String PUserKey
        {
            set { this._puserkey = value; }
            get { return this._puserkey.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String PUserName
        {
            set { this._pusername = value; }
            get { return this._pusername.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String PUserParm1
        {
            set { this._puserparm1 = value; }
            get { return this._puserparm1.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String PUserParm2
        {
            set { this._puserparm2 = value; }
            get { return this._puserparm2.Trim(); }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 IsSingle
        {
            set { this._issingle = value; }
            get { return this._issingle; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Int32 IsTransfer
        {
            set { this._istransfer = value; }
            get { return this._istransfer; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String TransferUrl
        {
            set { this._transferurl = value; }
            get { return this._transferurl.Trim(); }
        }

        #endregion 公开属性
    }
}
