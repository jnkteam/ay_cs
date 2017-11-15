namespace OriginalStudio.Model.Settled
{
    using System;

    [Serializable]
    public class TocashSchemeInfo
    {
        private int _alipaydetentiondays = 0;
        private int _bankdetentiondays = 0;
        private int _carddetentiondays = 0;
        private int _qqdetentiondays = 0;
        private decimal _chargeleastofeach = 10M;
        private decimal _chargemostofeach = 50M;
        private decimal _chargerate = 0.01M;
        private decimal _dailymaxamt = 50000M;
        private int _dailymaxtimes = 10;
        private int _id;
        private int _isdefault;
        private decimal _maxamtlimitofeach = 50000M;
        private decimal _minamtlimitofeach = 100M;
        private int _otherdetentiondays = 0;
        private string _schemename;
        private byte _tranRequiredAudit = 1;
        private int _type = 1;
        private int _vaiInterface = 0;
        private int _weixindetentiondays = 0;

        public int alipaydetentiondays
        {
            get
            {
                return this._alipaydetentiondays;
            }
            set
            {
                this._alipaydetentiondays = value;
            }
        }

        public int bankdetentiondays
        {
            get
            {
                return this._bankdetentiondays;
            }
            set
            {
                this._bankdetentiondays = value;
            }
        }

        public int carddetentiondays
        {
            get
            {
                return this._carddetentiondays;
            }
            set
            {
                this._carddetentiondays = value;
            }
        }

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

        public int otherdetentiondays
        {
            get
            {
                return this._otherdetentiondays;
            }
            set
            {
                this._otherdetentiondays = value;
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

        public byte tranRequiredAudit
        {
            get
            {
                return this._tranRequiredAudit;
            }
            set
            {
                this._tranRequiredAudit = value;
            }
        }

        public int type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        public int vaiInterface
        {
            get
            {
                return this._vaiInterface;
            }
            set
            {
                this._vaiInterface = value;
            }
        }

        public int weixindetentiondays
        {
            get
            {
                return this._weixindetentiondays;
            }
            set
            {
                this._weixindetentiondays = value;
            }
        }

        public int qqdetentiondays
        {
            get
            {
                return this._qqdetentiondays;
            }
            set
            {
                this._qqdetentiondays = value;
            }
        }
    }
}

