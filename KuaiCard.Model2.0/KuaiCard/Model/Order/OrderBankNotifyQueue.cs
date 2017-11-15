using System;
using System.Collections.Generic;
using System.Text;

namespace KuaiCard.Model.Order
{
	public class OrderBankNotifyQueue
	{
        public Int64 ID { get; set; }

        public string OrderID { get; set; }

        public Int32 UserID { get; set; }

        public Int32 Status { get; set; }

        public DateTime NotifyTime { get; set; }

        public Int32 NotifyCount { get; set; }

        public string NotifyUrl { get; set; }
	}
}
