namespace OriginalStudio.Model.Settled
{
    using System;

    public class IncreaseAmtInfo
    {
        private DateTime? _addtime;
        private string _desc;
        private int _id;
        private decimal? _increaseamt;
        private int? _mangeid;
        private string _mangename;
        private SettleTypeEnum _optype;
        private int? _status;
        private int? _userid;

        public DateTime? addtime
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

        public string desc
        {
            get
            {
                return this._desc;
            }
            set
            {
                this._desc = value;
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

        public decimal? increaseAmt
        {
            get
            {
                return this._increaseamt;
            }
            set
            {
                this._increaseamt = value;
            }
        }

        public int? mangeId
        {
            get
            {
                return this._mangeid;
            }
            set
            {
                this._mangeid = value;
            }
        }

        public string mangeName
        {
            get
            {
                return this._mangename;
            }
            set
            {
                this._mangename = value;
            }
        }

        public SettleTypeEnum optype
        {
            get
            {
                return this._optype;
            }
            set
            {
                this._optype = value;
            }
        }

        public int? status
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

        public int? userId
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

