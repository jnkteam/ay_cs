using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OriginalStudio.Model.User
{
    [Serializable]
    public class MchUserPayLimit
    {
        private System.Int32 _userid = 0;
        private System.Int32 _typeid = 0;
        private System.Decimal _minmoney = 0M;
        private System.Decimal _maxmoney = 0M;

        public MchUserPayLimit()
        {

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
        public System.Int32 TypeID
        {
            set { this._typeid = value; }
            get { return this._typeid; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Decimal MinMoney
        {
            set { this._minmoney = value; }
            get { return this._minmoney; }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        public System.Decimal MaxMoney
        {
            set { this._maxmoney = value; }
            get { return this._maxmoney; }
        }

    }
}