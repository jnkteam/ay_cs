﻿namespace KuaiCard.Model
{
    using System;

    public class PhoneValidLog
    {
        private string _clientip;
        private string _code;
        private bool _enable = false;
        private int _id;
        private string _phone;
        private DateTime _sendtime;

        public string clientIP
        {
            get
            {
                return this._clientip;
            }
            set
            {
                this._clientip = value;
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

        public bool Enable
        {
            get
            {
                return this._enable;
            }
            set
            {
                this._enable = value;
            }
        }

        public int ID
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

        public string phone
        {
            get
            {
                return this._phone;
            }
            set
            {
                this._phone = value;
            }
        }

        public DateTime sendTime
        {
            get
            {
                return this._sendtime;
            }
            set
            {
                this._sendtime = value;
            }
        }
    }
}

