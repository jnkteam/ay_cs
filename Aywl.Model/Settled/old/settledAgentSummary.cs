namespace OriginalStudio.Model.Withdraw
{
    using System;

    [Serializable]
    public class settledAgentSummary
    {
        private DateTime _addtime = DateTime.Now;
        private decimal _amt;
        private int? _audit_status = 1;
        private DateTime? _audittime = new DateTime?(DateTime.Now);
        private int? _audituser = 0;
        private string _auditusername = "";
        private decimal _fee = 0M;
        private int _id;
        private string _lotno;
        private int _qty;
        private decimal _realfee = 0M;
        private string _remark;
        private int _status = 1;
        private decimal _succamt = 0M;
        private int _success = 1;
        private int _succqty = 0;
        private decimal? _totalamt;
        private decimal? _totalsuccamt;
        private DateTime _updatetime = DateTime.Now;
        private int _userid;

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

        public decimal amt
        {
            get
            {
                return this._amt;
            }
            set
            {
                this._amt = value;
            }
        }

        public int? audit_status
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

        public decimal fee
        {
            get
            {
                return this._fee;
            }
            set
            {
                this._fee = value;
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

        public int qty
        {
            get
            {
                return this._qty;
            }
            set
            {
                this._qty = value;
            }
        }

        public decimal realfee
        {
            get
            {
                return this._realfee;
            }
            set
            {
                this._realfee = value;
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

        public int status
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

        public decimal succamt
        {
            get
            {
                return this._succamt;
            }
            set
            {
                this._succamt = value;
            }
        }

        public int success
        {
            get
            {
                return this._success;
            }
            set
            {
                this._success = value;
            }
        }

        public int succqty
        {
            get
            {
                return this._succqty;
            }
            set
            {
                this._succqty = value;
            }
        }

        public decimal? totalamt
        {
            get
            {
                return this._totalamt;
            }
            set
            {
                this._totalamt = value;
            }
        }

        public decimal? totalsuccamt
        {
            get
            {
                return this._totalsuccamt;
            }
            set
            {
                this._totalsuccamt = value;
            }
        }

        public DateTime updatetime
        {
            get
            {
                return this._updatetime;
            }
            set
            {
                this._updatetime = value;
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

