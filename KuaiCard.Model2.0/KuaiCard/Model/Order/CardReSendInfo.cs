namespace KuaiCard.Model.Order
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class CardReSendInfo
    {
        public string cardno
        {
            get;
            set;
        }

        public string cardpwd
        {
            get;
            set;
        }

        public decimal cardvalue
        {
            get;
            set;
        }

        public int result
        {
            get;
            set;
        }

        public int seq
        {
            get;
            set;
        }

        public int suppid
        {
            get;
            set;
        }

        public string sysorderid
        {
            get;
            set;
        }

        public int typeid
        {
            get;
            set;
        }
    }
}

