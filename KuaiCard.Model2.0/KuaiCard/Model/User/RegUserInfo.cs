namespace KuaiCard.Model.User
{
    using KuaiCard.Model;
    using System;

    public class RegUserInfo
    {
        private DateTime _addtime;
        private AdsTypeEnum _adstype;
        private int _cid;
        private int _id;
        private decimal _prices;
        private CPAInfo.RegUserStatusEnum _status;
        private int _uid;
        private int _userid;

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

        public AdsTypeEnum AdsType
        {
            get
            {
                return this._adstype;
            }
            set
            {
                this._adstype = value;
            }
        }

        public int Cid
        {
            get
            {
                return this._cid;
            }
            set
            {
                this._cid = value;
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

        public decimal Prices
        {
            get
            {
                return this._prices;
            }
            set
            {
                this._prices = value;
            }
        }

        public CPAInfo.RegUserStatusEnum Status
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

        public int UserId
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

