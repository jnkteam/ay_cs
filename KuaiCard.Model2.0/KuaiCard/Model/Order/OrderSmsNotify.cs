namespace KuaiCard.Model.Order
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class OrderSmsNotify
    {
        public Timer tmr;

        public OrderSmsInfo orderInfo
        {
            get;
            set;
        }
    }
}

