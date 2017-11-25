using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OriginalStudio.Model.User
{
    [Serializable]
    public class MchUserBindIP
    {
        private System.Int32 _userid = 0;
        private System.String _ip = string.Empty;
        private System.Int32 _ip_type = 0;
        private System.Int32 _id = 0;
        private System.DateTime _bind_date = System.DateTime.Now;

        public MchUserBindIP()
        {

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
        /// 设置或获取IP
        /// </summary>
        public System.String IP
        {
            set { this._ip = value; }
            get { return this._ip.Trim(); }
        }

        /// <summary>
        /// 设置或获取IP类型。1:登录IP    2: API下方IP
        /// </summary>
        public System.Int32 IpType
        {
            set { this._ip_type = value; }
            get { return this._ip_type; }
        }

        /// <summary>
        /// 设置或获取序号
        /// </summary>
        public System.Int32 ID
        {
            set { this._id = value; }
            get { return this._id; }
        }

        /// <summary>
        /// 设置或获取绑定时间
        /// </summary>
        public System.DateTime BindDate
        {
            set { this._bind_date = value; }
            get { return this._bind_date; }
        }


    }
}