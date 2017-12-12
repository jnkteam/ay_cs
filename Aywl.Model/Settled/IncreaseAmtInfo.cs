namespace OriginalStudio.Model.Settled
{
    using System;

    public class IncreaseAmtInfo
    {
        private DateTime _addtime = DateTime.Now;
        private string _desc;
        private int _id = 0;
        private decimal _increaseamt = 0M;
        private int _mangeid = 0;
        private string _mangename;
        private SettleTypeEnum _optype;
        private int _status = 0;
        private int _userid = 0;
        private string _merchantname = "";

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

        public string Desc
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

        public decimal IncreaseAmt
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

        public int MangeId
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

        public string MangeName
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

        public int UserID
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

        public string MerchantName
        {
            get
            {
                return this._merchantname;
            }
            set
            {
                this._merchantname = value;
            }
        }
    }
}

