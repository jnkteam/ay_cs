namespace KuaiCard.Model.Settled
{
    using System;

    [Serializable]
    public class transferscheme
    {
        private decimal _chargeleastofeach;
        private decimal _chargemostofeach;
        private decimal _chargerate;
        private decimal _dailymaxamt;
        private int _dailymaxtimes;
        private int _id = 0;
        private int _isdefault;
        private decimal _maxamtlimitofeach;
        private decimal _minamtlimitofeach;
        private decimal _monthmaxamt;
        private int _monthmaxtimes;
        private string _schemename;

        public decimal chargeleastofeach
        {
            get
            {
                return this._chargeleastofeach;
            }
            set
            {
                this._chargeleastofeach = value;
            }
        }

        public decimal chargemostofeach
        {
            get
            {
                return this._chargemostofeach;
            }
            set
            {
                this._chargemostofeach = value;
            }
        }

        public decimal chargerate
        {
            get
            {
                return this._chargerate;
            }
            set
            {
                this._chargerate = value;
            }
        }

        public decimal dailymaxamt
        {
            get
            {
                return this._dailymaxamt;
            }
            set
            {
                this._dailymaxamt = value;
            }
        }

        public int dailymaxtimes
        {
            get
            {
                return this._dailymaxtimes;
            }
            set
            {
                this._dailymaxtimes = value;
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

        public int isdefault
        {
            get
            {
                return this._isdefault;
            }
            set
            {
                this._isdefault = value;
            }
        }

        public decimal maxamtlimitofeach
        {
            get
            {
                return this._maxamtlimitofeach;
            }
            set
            {
                this._maxamtlimitofeach = value;
            }
        }

        public decimal minamtlimitofeach
        {
            get
            {
                return this._minamtlimitofeach;
            }
            set
            {
                this._minamtlimitofeach = value;
            }
        }

        public decimal monthmaxamt
        {
            get
            {
                return this._monthmaxamt;
            }
            set
            {
                this._monthmaxamt = value;
            }
        }

        public int monthmaxtimes
        {
            get
            {
                return this._monthmaxtimes;
            }
            set
            {
                this._monthmaxtimes = value;
            }
        }

        public string schemename
        {
            get
            {
                return this._schemename;
            }
            set
            {
                this._schemename = value;
            }
        }
    }
}

