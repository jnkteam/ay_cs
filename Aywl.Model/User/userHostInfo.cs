namespace OriginalStudio.Model.User
{
    using OriginalStudio.Model.Trade;
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class userHostInfo
    {
        private int _id;

        private int? _userid;

        private string _siteip;

        private int? _sitetype;

        private string _hostname;

        private string _hosturl = "http://";

        private string _desc;

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

        public string hostName
        {
            get
            {
                return this._hostname;
            }
            set
            {
                this._hostname = value;
            }
        }

        public string hostUrl
        {
            get
            {
                return this._hosturl;
            }
            set
            {
                this._hosturl = value;
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

        public string siteip
        {
            get
            {
                return this._siteip;
            }
            set
            {
                this._siteip = value;
            }
        }

        public int? sitetype
        {
            get
            {
                return this._sitetype;
            }
            set
            {
                this._sitetype = value;
            }
        }

        public userHostStatus status
        {
            get;
            set;
        }

        public int? userid
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

