namespace KuaiCard.Model.Settled
{
    using System;

    [Serializable]
    public class transfer
    {
        private DateTime _addtime;
        private decimal _amt;
        private decimal _charge;
        private int _id;
        private int? _month;
        private string _remark;
        private int _status;
        private int _touserid;
        private DateTime? _updatetime;
        private int _userid;
        private int? _year;

        public DateTime addtime
        {
            get
            {
                return this._addtime;
            }
            set
            {
                this._addtime = value;
            }
        }

        public decimal amt
        {
            get
            {
                return this._amt;
            }
            set
            {
                this._amt = value;
            }
        }

        public decimal charge
        {
            get
            {
                return this._charge;
            }
            set
            {
                this._charge = value;
            }
        }

        public int id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        public int? month
        {
            get
            {
                return this._month;
            }
            set
            {
                this._month = value;
            }
        }

        public string remark
        {
            get
            {
                return this._remark;
            }
            set
            {
                this._remark = value;
            }
        }

        public int status
        {
            get
            {
                return this._status;
            }
            set
            {
                this._status = value;
            }
        }

        public int touserid
        {
            get
            {
                return this._touserid;
            }
            set
            {
                this._touserid = value;
            }
        }

        public DateTime? updatetime
        {
            get
            {
                return this._updatetime;
            }
            set
            {
                this._updatetime = value;
            }
        }

        public int userid
        {
            get
            {
                return this._userid;
            }
            set
            {
                this._userid = value;
            }
        }

        public int? year
        {
            get
            {
                return this._year;
            }
            set
            {
                this._year = value;
            }
        }
    }
}

