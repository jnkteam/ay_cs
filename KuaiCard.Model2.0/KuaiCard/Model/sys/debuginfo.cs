namespace KuaiCard.Model.Sys
{
    using System;

    [Serializable]
    public class DebugInfo
    {
        private DateTime? _addtime;
        private debugtypeenum _bugtype;
        private string _detail;
        private string _errorcode;
        private string _errorinfo;
        private int _id;
        private string _url;
        private int? _userid;
        private string _userorder;

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

        public debugtypeenum bugtype
        {
            get
            {
                return this._bugtype;
            }
            set
            {
                this._bugtype = value;
            }
        }

        public string detail
        {
            get
            {
                return this._detail;
            }
            set
            {
                this._detail = value;
            }
        }

        public string errorcode
        {
            get
            {
                return this._errorcode;
            }
            set
            {
                this._errorcode = value;
            }
        }

        public string errorinfo
        {
            get
            {
                return this._errorinfo;
            }
            set
            {
                this._errorinfo = value;
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

        public string url
        {
            get
            {
                return this._url;
            }
            set
            {
                this._url = value;
            }
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

        public string userorder
        {
            get
            {
                return this._userorder;
            }
            set
            {
                this._userorder = value;
            }
        }
    }
}

