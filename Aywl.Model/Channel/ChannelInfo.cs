namespace OriginalStudio.Model.Channel
{
    using System;

    [Serializable]
    public class ChannelInfo
    {
        private DateTime _addtime;
        private string _code;
        private int _facevalue;
        private int _id;
        private int? _isopen;
        private string _modeenname;
        private string _modename;
        private int? _sort;
        private int? _supplier;
        private decimal _supprate = 0M;
        private int _typeid;

        public DateTime addtime
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

        public string code
        {
            get
            {
                return this._code;
            }
            set
            {
                this._code = value;
            }
        }

        public int faceValue
        {
            get
            {
                return this._facevalue;
            }
            set
            {
                this._facevalue = value;
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

        public int? isOpen
        {
            get
            {
                return this._isopen;
            }
            set
            {
                this._isopen = value;
            }
        }

        public string modeEnName
        {
            get
            {
                return this._modeenname;
            }
            set
            {
                this._modeenname = value;
            }
        }

        public string modeName
        {
            get
            {
                return this._modename;
            }
            set
            {
                this._modename = value;
            }
        }

        public int? sort
        {
            get
            {
                return this._sort;
            }
            set
            {
                this._sort = value;
            }
        }

        public int? supplier
        {
            get
            {
                return this._supplier;
            }
            set
            {
                this._supplier = value;
            }
        }

        public decimal supprate
        {
            get
            {
                return this._supprate;
            }
            set
            {
                this._supprate = value;
            }
        }

        public int typeId
        {
            get
            {
                return this._typeid;
            }
            set
            {
                this._typeid = value;
            }
        }
    }

}

