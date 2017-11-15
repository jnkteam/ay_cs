namespace KuaiCard.Model.order
{
    using System;

    [Serializable]
    public class reconciliation_temp
    {
        private int? _count = 0;
        private int _id;
        private string _orderid = string.Empty;
        private string _serverid = string.Empty;

        public int? count
        {
            get
            {
                return this._count;
            }
            set
            {
                this._count = value;
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

        public string orderid
        {
            get
            {
                return this._orderid;
            }
            set
            {
                this._orderid = value;
            }
        }

        public string serverid
        {
            get
            {
                return this._serverid;
            }
            set
            {
                this._serverid = value;
            }
        }
    }
}

