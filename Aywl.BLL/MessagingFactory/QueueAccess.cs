namespace OriginalStudio.BLL.MessagingFactory
{
    using OriginalStudio.IMessaging;
    using System;
    using System.Reflection;

    public sealed class QueueAccess
    {
        private static readonly string orderAssembly = OriginalStudio.Lib.SysConfig.MSMQSetting.OrderMessagingAssembly;  //Aywl.MSMQMessaging
        private static readonly string orderClass = OriginalStudio.Lib.SysConfig.MSMQSetting.OrderMessagingClass;  //OriginalStudio.MSMQMessaging

        private QueueAccess()
        {
        }

        public static IOrderBank CreateBankOrder()
        {
            try
            {
                string typeName = orderClass + ".OrderBank";
                return (IOrderBank)Assembly.Load(orderAssembly).CreateInstance(typeName);
            }
            catch (Exception err)
            {
                OriginalStudio.Lib.Logging.LogHelper.Write("OriginalStudio.MessagingFactory.CreateBankOrder加载失败：" + err.Message.ToString());
                return null;
            }
        }

        public static IOrderBankNotify OrderBankNotify()
        {
            try
            {
                string typeName = orderClass + ".OrderBankNotify";      //OriginalStudio.MSMQMessaging.OrderBankNotify
                return (IOrderBankNotify)Assembly.Load(orderAssembly).CreateInstance(typeName);
            }
            catch (Exception err)
            {
                OriginalStudio.Lib.Logging.LogHelper.Write("OriginalStudio.MessagingFactory.OrderBankNotify加载失败：" + err.Message.ToString());
                return null;
            }
        }
    }
}

