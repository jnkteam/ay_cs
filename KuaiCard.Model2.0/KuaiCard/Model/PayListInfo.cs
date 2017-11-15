namespace KuaiCard.Model
{
    using System;

    public class PayListInfo
    {
        private DateTime _addtime;
        private decimal _charges;
        private int _id;
        private decimal _money;
        private DateTime _paytime;
        private int _status;
        private decimal _tax;
        private int _uid;

        public DateTime AddTime
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

        public decimal Charges
        {
            get
            {
                return this._charges;
            }
            set
            {
                this._charges = value;
            }
        }

        public int ID
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

        public decimal Money
        {
            get
            {
                return this._money;
            }
            set
            {
                this._money = value;
            }
        }

        public DateTime PayTime
        {
            get
            {
                return this._paytime;
            }
            set
            {
                this._paytime = value;
            }
        }

        public int Status
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

        public decimal Tax
        {
            get
            {
                return this._tax;
            }
            set
            {
                this._tax = value;
            }
        }

        public int Uid
        {
            get
            {
                return this._uid;
            }
            set
            {
                this._uid = value;
            }
        }
    }
}

