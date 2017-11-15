namespace OriginalStudio.Model.User
{
    using System;

    public class usersettingInfo
    {
        private int _defaultpay = 0x67;
        private byte _isRequireAgentDistAudit = 0;
        private int _istransfer = 0;
        private string _payrate = string.Empty;
        private int _special = 0;
        private int _userid;

        public int defaultpay
        {
            get
            {
                return this._defaultpay;
            }
            set
            {
                this._defaultpay = value;
            }
        }

        public byte isRequireAgentDistAudit
        {
            get
            {
                return this._isRequireAgentDistAudit;
            }
            set
            {
                this._isRequireAgentDistAudit = value;
            }
        }

        public int istransfer
        {
            get
            {
                return this._istransfer;
            }
            set
            {
                this._istransfer = value;
            }
        }

        public string payrate
        {
            get
            {
                return this._payrate;
            }
            set
            {
                this._payrate = value;
            }
        }

        public int special
        {
            get
            {
                return this._special;
            }
            set
            {
                this._special = value;
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

