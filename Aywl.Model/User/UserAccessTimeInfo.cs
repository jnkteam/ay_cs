namespace OriginalStudio.Model.User
{
    using System;

    [Serializable]
    public class MchUserAccessTimeInfo
    {        
        private DateTime _lastaccesstime = DateTime.MinValue;
        private int _userid;

        public DateTime lastAccesstime
        {
            get
            {
                return this._lastaccesstime;
            }
            set
            {
                this._lastaccesstime = value;
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
    }
}

