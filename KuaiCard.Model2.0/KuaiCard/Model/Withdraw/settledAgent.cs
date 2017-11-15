namespace KuaiCard.Model.Withdraw
{
    using System;

    [Serializable]
    public class settledAgent
    {
        private DateTime _addtime = DateTime.Now;
        private decimal _amount;
        private int _audit_status = 1;
        private DateTime? _audittime;
        private int? _audituser;
        private string _auditusername = string.Empty;
        private string _bankaccount;
        private string _bankaccountname;
        private string _bankbranch;
        private string _bankcode;
        private string _bankname;
        private string _callbacktext = string.Empty;
        private decimal _charge;
        private string _ext1 = string.Empty;
        private string _ext2 = string.Empty;
        private string _ext3 = string.Empty;
        private int _id;
        private string _input_charset = string.Empty;
        private bool _is_cancel = false;
        private byte _issure = 0;
        private string _lotno = string.Empty;
        private int _mode;
        private int _notifystatus = 0xff;
        private int _notifytimes = 0;
        private string _out_trade_no;
        private int _payment_status = 1;
        private DateTime _processingtime = DateTime.Now;
        private string _remark = string.Empty;
        private string _return_url;
        private int _serial = 1;
        private string _service;
        private string _sign_type;
        private int _suppid = 0;
        private int _suppstatus = 0;
        private string _sureclientip = string.Empty;
        private string _trade_no;
        private int _userid;

        public DateTime addTime
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

        public decimal amount
        {
            get
            {
                return this._amount;
            }
            set
            {
                this._amount = value;
            }
        }

        public int audit_status
        {
            get
            {
                return this._audit_status;
            }
            set
            {
                this._audit_status = value;
            }
        }

        public DateTime? auditTime
        {
            get
            {
                return this._audittime;
            }
            set
            {
                this._audittime = value;
            }
        }

        public int? auditUser
        {
            get
            {
                return this._audituser;
            }
            set
            {
                this._audituser = value;
            }
        }

        public string auditUserName
        {
            get
            {
                return this._auditusername;
            }
            set
            {
                this._auditusername = value;
            }
        }

        public string bankAccount
        {
            get
            {
                return this._bankaccount;
            }
            set
            {
                this._bankaccount = value;
            }
        }

        public string bankAccountName
        {
            get
            {
                return this._bankaccountname;
            }
            set
            {
                this._bankaccountname = value;
            }
        }

        public string bankBranch
        {
            get
            {
                return this._bankbranch;
            }
            set
            {
                this._bankbranch = value;
            }
        }

        public string bankCode
        {
            get
            {
                return this._bankcode;
            }
            set
            {
                this._bankcode = value;
            }
        }

        public string bankName
        {
            get
            {
                return this._bankname;
            }
            set
            {
                this._bankname = value;
            }
        }

        public string callbackText
        {
            get
            {
                return this._callbacktext;
            }
            set
            {
                this._callbacktext = value;
            }
        }

        public decimal charge
        {
            get
            {
                return this._charge;
            }
            set
            {
                this._charge = value;
            }
        }

        public string ext1
        {
            get
            {
                return this._ext1;
            }
            set
            {
                this._ext1 = value;
            }
        }

        public string ext2
        {
            get
            {
                return this._ext2;
            }
            set
            {
                this._ext2 = value;
            }
        }

        public string ext3
        {
            get
            {
                return this._ext3;
            }
            set
            {
                this._ext3 = value;
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

        public string input_charset
        {
            get
            {
                return this._input_charset;
            }
            set
            {
                this._input_charset = value;
            }
        }

        public bool is_cancel
        {
            get
            {
                return this._is_cancel;
            }
            set
            {
                this._is_cancel = value;
            }
        }

        public byte issure
        {
            get
            {
                return this._issure;
            }
            set
            {
                this._issure = value;
            }
        }

        public string lotno
        {
            get
            {
                return this._lotno;
            }
            set
            {
                this._lotno = value;
            }
        }

        public int mode
        {
            get
            {
                return this._mode;
            }
            set
            {
                this._mode = value;
            }
        }

        public int notifystatus
        {
            get
            {
                return this._notifystatus;
            }
            set
            {
                this._notifystatus = value;
            }
        }

        public int notifyTimes
        {
            get
            {
                return this._notifytimes;
            }
            set
            {
                this._notifytimes = value;
            }
        }

        public string out_trade_no
        {
            get
            {
                return this._out_trade_no;
            }
            set
            {
                this._out_trade_no = value;
            }
        }

        public int payment_status
        {
            get
            {
                return this._payment_status;
            }
            set
            {
                this._payment_status = value;
            }
        }

        public DateTime processingTime
        {
            get
            {
                return this._processingtime;
            }
            set
            {
                this._processingtime = value;
            }
        }

        public string remark
        {
            get
            {
                return this._remark;
            }
            set
            {
                this._remark = value;
            }
        }

        public string return_url
        {
            get
            {
                return this._return_url;
            }
            set
            {
                this._return_url = value;
            }
        }

        public int serial
        {
            get
            {
                return this._serial;
            }
            set
            {
                this._serial = value;
            }
        }

        public string service
        {
            get
            {
                return this._service;
            }
            set
            {
                this._service = value;
            }
        }

        public string sign_type
        {
            get
            {
                return this._sign_type;
            }
            set
            {
                this._sign_type = value;
            }
        }

        public int suppid
        {
            get
            {
                return this._suppid;
            }
            set
            {
                this._suppid = value;
            }
        }

        public int suppstatus
        {
            get
            {
                return this._suppstatus;
            }
            set
            {
                this._suppstatus = value;
            }
        }

        public string sureclientip
        {
            get
            {
                return this._sureclientip;
            }
            set
            {
                this._sureclientip = value;
            }
        }

        public string trade_no
        {
            get
            {
                return this._trade_no;
            }
            set
            {
                this._trade_no = value;
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

