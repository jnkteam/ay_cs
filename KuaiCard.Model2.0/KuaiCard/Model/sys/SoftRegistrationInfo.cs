namespace KuaiCard.Model.Sys
{
    using System;

    [Serializable]
    public class SoftRegistrationInfo
    {
        private DateTime _etime = DateTime.Now;
        private string _hosts = string.Empty;
        private bool _islimithost = true;
        private bool _islimittime = true;
        private string _name = string.Empty;
        private DateTime _stime = DateTime.Now;

        public DateTime etime
        {
            get
            {
                return this._etime;
            }
            set
            {
                this._etime = value;
            }
        }

        public string hosts
        {
            get
            {
                return this._hosts;
            }
            set
            {
                this._hosts = value;
            }
        }

        public bool islimithost
        {
            get
            {
                return this._islimithost;
            }
            set
            {
                this._islimithost = value;
            }
        }

        public bool islimittime
        {
            get
            {
                return this._islimittime;
            }
            set
            {
                this._islimittime = value;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        public DateTime stime
        {
            get
            {
                return this._stime;
            }
            set
            {
                this._stime = value;
            }
        }
    }
}

