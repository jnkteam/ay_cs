namespace OriginalStudio.BLL.Order
{
    using OriginalStudio.DAL.Order;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;

    public class Helper
    {
        private readonly OriginalStudio.DAL.Order.Helper dal = new OriginalStudio.DAL.Order.Helper();

        public int search_check(int o_userid, string userorderid, out DataRow row)
        {
            row = null;
            try
            {
                return this.dal.search_check(o_userid, userorderid, out row);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0x63;
            }
        }
    }
}

