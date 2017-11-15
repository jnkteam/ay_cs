namespace KuaiCard.Model.Order
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class OrderCardNotify
    {
        public Timer tmr;

        public OrderCardInfo orderInfo
        {
            get;
            set;
        }
    }
}

