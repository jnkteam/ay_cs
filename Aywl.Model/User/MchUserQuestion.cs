using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OriginalStudio.Model.User
{
    /// <summary>
    /// 实体类mch_userQuestion。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class MchUserQuestion
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MchUserQuestion()
        {
        }

        #region 字段

        private System.Int32 _id = 0;
        private System.Int32 _userid = 0;
        private System.Int32 _questionid = 0;
        private System.String _answer = string.Empty;
        private System.Int32 _qtype = 0;

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
        public System.Int32 QuestionID
        {
            set { this._questionid = value; }
            get { return this._questionid; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.String Answer
        {
            set { this._answer = value; }
            get { return this._answer.Trim(); }
        }

        /// <summary>
        /// 设置或获取问题类型。1：登录密码问题；2：支付密码问题
        /// </summary>
        public System.Int32 QType
        {
            set { this._qtype = value; }
            get { return this._qtype; }
        }

        #endregion 公开属性

    }
}
