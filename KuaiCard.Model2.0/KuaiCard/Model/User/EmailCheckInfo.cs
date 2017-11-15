﻿namespace KuaiCard.Model.User
{
    using System;
    using System.Runtime.CompilerServices;

    public class EmailCheckInfo
    {
        private int _id;

        private int _userid;

        private string _email;

        private DateTime? _addtime;

        private EmailCheckStatus _status;

        private DateTime? _checktime;

        private EmailCheckType _typeid;

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

        public string email
        {
            get
            {
                return this._email;
            }
            set
            {
                this._email = value;
            }
        }

        public DateTime Expired
        {
            get;
            set;
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

        public EmailCheckStatus status
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

        public EmailCheckType typeid
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

