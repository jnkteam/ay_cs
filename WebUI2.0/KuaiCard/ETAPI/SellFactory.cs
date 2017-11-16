namespace OriginalStudio.ETAPI
{
    public class SellFactory
    {

        /// <summary>
        /// 代付款
        /// </summary>
        /// <param name="info"></param>
        public static void ReqDistribution(OriginalStudio.Model.Settled.Distribution info)
        {
            Lib.Logging.LogHelper.Write("代付通道：" + info.suppid.ToString());
        }
    }
}

