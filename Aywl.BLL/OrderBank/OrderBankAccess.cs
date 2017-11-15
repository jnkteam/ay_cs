namespace OriginalStudio.BLL
{
    using System;
    using System.Reflection;
    using OriginalStudio.Lib.SysConfig;
    using OriginalStudio.IDAL;

    public class OrderBankAccess
    {
        public static IOrderBank CreateOrderBank()
        {
            try
            {
                string orderAssembly = RuntimeSetting.OrderBankDALStrategyAssembly;    //Aywl.SQLServerDAL
                string orderClass = RuntimeSetting.OrderBankDALStrategyClass;                //OriginalStudio.SQLServerDAL

                string typeName = orderClass + ".OrderBank";
                return (IOrderBank)Assembly.Load(orderAssembly).CreateInstance(typeName);
            }
            catch (Exception err)
            {
                OriginalStudio.Lib.Logging.LogHelper.Write("OriginalStudio.DALFactory.CreateOrderBank加载失败：" + err.Message.ToString());
                return null;
            }
        }

    }
}

