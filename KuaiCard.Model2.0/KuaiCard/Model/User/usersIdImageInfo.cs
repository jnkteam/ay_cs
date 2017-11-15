namespace KuaiCard.Model.User
{
    using System;

    public class usersIdImageInfo
    {
        private DateTime? _addtime;
        private int? _admin;
        private DateTime? _checktime;
        private int? _filesize;
        private int? _filesize1;
        private int _id;
        private byte[] _image_down;
        private byte[] _image_on;
        private string _ptype;
        private string _ptype1;
        private IdImageStatus _status;
        private int? _userid;
        private string _why;

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

        public int? admin
        {
            get
            {
                return this._admin;
            }
            set
            {
                this._admin = value;
            }
        }

        public DateTime? checktime
        {
            get
            {
                return this._checktime;
            }
            set
            {
                this._checktime = value;
            }
        }

        public int? filesize
        {
            get
            {
                return this._filesize;
            }
            set
            {
                this._filesize = value;
            }
        }

        public int? filesize1
        {
            get
            {
                return this._filesize1;
            }
            set
            {
                this._filesize1 = value;
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

        public byte[] image_down
        {
            get
            {
                return this._image_down;
            }
            set
            {
                this._image_down = value;
            }
        }

        public byte[] image_on
        {
            get
            {
                return this._image_on;
            }
            set
            {
                this._image_on = value;
            }
        }

        public string ptype
        {
            get
            {
                return this._ptype;
            }
            set
            {
                this._ptype = value;
            }
        }

        public string ptype1
        {
            get
            {
                return this._ptype1;
            }
            set
            {
                this._ptype1 = value;
            }
        }

        public IdImageStatus status
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

        public int? userId
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

        public string why
        {
            get
            {
                return this._why;
            }
            set
            {
                this._why = value;
            }
        }
    }
}

