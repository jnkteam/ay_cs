using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OriginalStudio.Model.User
{
    [Serializable]
    public class UserPayLimit
    {
        private int _userid;
        private int _typeid;
        private decimal _minmoney;
        private decimal _maxmoney;

        public UserPayLimit()
        {

        }

        public int UserID
        {
            get { return _userid; }
            set { _userid = value; }
        }
        public int TypeID
        {
            get { return _typeid; }
            set { _typeid = value; }
        }
        public decimal MinMoney
        {
            get { return _minmoney; }
            set { _minmoney = value; }
        }
        public decimal MaxMoney
        {
            get { return _maxmoney; }
            set { _maxmoney = value; }
        }
    }
}