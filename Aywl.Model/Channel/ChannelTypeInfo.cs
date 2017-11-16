namespace OriginalStudio.Model.Channel
{
    using System;

    /// <summary>
    /// 作废！！！！
    /// </summary>
    [Serializable]
    public class ChannelTypeInfo
    {
        private DateTime _addtime;
        private ChannelClassEnum _class;
        private string _code = string.Empty;
        private int _id;
        private OpenEnum _isopen;
        private string _modetypename;
        private bool _release;
        private int _runmode = 0;
        private string _runset = string.Empty;
        private int? _sort;
        private int _supplier;
        private decimal _supprate = 0M;
        private int _typeId;

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

        public ChannelClassEnum Class
        {
            get
            {
                return this._class;
            }
            set
            {
                this._class = value;
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

        public OpenEnum isOpen
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

        public string modetypename
        {
            get
            {
                return this._modetypename;
            }
            set
            {
                this._modetypename = value;
            }
        }

        public bool release
        {
            get
            {
                return this._release;
            }
            set
            {
                this._release = value;
            }
        }

        public int runmode
        {
            get
            {
                return this._runmode;
            }
            set
            {
                this._runmode = value;
            }
        }

        public string runset
        {
            get
            {
                return this._runset;
            }
            set
            {
                this._runset = value;
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

        public int supplier
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
                return this._typeId;
            }
            set
            {
                this._typeId = value;
            }
        }
    }
}

