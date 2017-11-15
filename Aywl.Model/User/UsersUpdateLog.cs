namespace OriginalStudio.Model.User
{
    using System;

    public class UsersUpdateLog
    {
        private DateTime _addtime;
        private string _desc = string.Empty;
        private string _editor = string.Empty;
        private string _field;
        private int _id;
        private string _newvalue;
        private string _oIp = string.Empty;
        private string _oldvalue;
        private int _userid;

        public DateTime Addtime
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

        public string Editor
        {
            get
            {
                return this._editor;
            }
            set
            {
                this._editor = value;
            }
        }

        public string field
        {
            get
            {
                return this._field;
            }
            set
            {
                this._field = value;
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

        public string newvalue
        {
            get
            {
                return this._newvalue;
            }
            set
            {
                this._newvalue = value;
            }
        }

        public string OIp
        {
            get
            {
                return this._oIp;
            }
            set
            {
                this._oIp = value;
            }
        }

        public string oldValue
        {
            get
            {
                return this._oldvalue;
            }
            set
            {
                this._oldvalue = value;
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

