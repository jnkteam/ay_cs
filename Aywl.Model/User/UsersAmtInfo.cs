namespace OriginalStudio.Model.User
{
    using System;

    [Serializable]
    public class UsersAmtInfo
    {
        private decimal? _balance = 0;
        private decimal _enableAmt = 0M;
        private decimal? _Freeze = 0;
        private int _id;
        private int? _integral = 0;
        private decimal? _payment = 0;
        private decimal? _unpayment = 0;
        private int _userid;

        public decimal? balance
        {
            get
            {
                return this._balance;
            }
            set
            {
                this._balance = value;
            }
        }

        public decimal enableAmt
        {
            get
            {
                return this._enableAmt;
            }
            set
            {
                this._enableAmt = value;
            }
        }

        public decimal? Freeze
        {
            get
            {
                return this._Freeze;
            }
            set
            {
                this._Freeze = value;
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

        public int? Integral
        {
            get
            {
                return this._integral;
            }
            set
            {
                this._integral = value;
            }
        }

        public decimal? payment
        {
            get
            {
                return this._payment;
            }
            set
            {
                this._payment = value;
            }
        }

        public decimal? unpayment
        {
            get
            {
                return this._unpayment;
            }
            set
            {
                this._unpayment = value;
            }
        }

        public int userId
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
    }
}

