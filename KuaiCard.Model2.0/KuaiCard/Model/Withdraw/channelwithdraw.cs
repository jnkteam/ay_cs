namespace KuaiCard.Model.Withdraw
{
    using System;

    [Serializable]
    public class channelwithdraw
    {
        private string _bankcode;
        private string _bankname;
        private int _id;
        private int? _sort;
        private int _supplier = 0;
        private string _bankenname;

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

        public string bankEnName
        {
            get
            {
                return this._bankenname;
            }
            set
            {
                this._bankenname = value;
            }
        }
    }
}

