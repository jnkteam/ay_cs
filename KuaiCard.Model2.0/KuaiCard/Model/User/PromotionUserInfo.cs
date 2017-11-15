namespace KuaiCard.Model.User
{
    using System;

    public class PromotionUserInfo
    {
        private int _pid;
        private decimal _prices;
        private int _promId = 0;
        private int _promstatus;
        private DateTime _promtime;
        private int _regid;

        public int PID
        {
            get
            {
                return this._pid;
            }
            set
            {
                this._pid = value;
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

        public int PromId
        {
            get
            {
                return this._promId;
            }
            set
            {
                this._promId = value;
            }
        }

        public int PromStatus
        {
            get
            {
                return this._promstatus;
            }
            set
            {
                this._promstatus = value;
            }
        }

        public DateTime PromTime
        {
            get
            {
                return this._promtime;
            }
            set
            {
                this._promtime = value;
            }
        }

        public int RegId
        {
            get
            {
                return this._regid;
            }
            set
            {
                this._regid = value;
            }
        }
    }
}

