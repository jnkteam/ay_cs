namespace OriginalStudio.BLL
{
    using OriginalStudio.DBAccess;
    using OriginalStudio.BLL.Channel;
    using OriginalStudio.Cache;
    using OriginalStudio.Model;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using OriginalStudio.BLL.Sys;
    using OriginalStudio.Lib.Utils;

    public sealed class SysSupplierPayRateFactory
    {
        public static string CACHE_KEY = (Constant.Cache_Mark + "SUPPPAYRATE");
        internal const string SQL_TABLE = "sys_supplier_payrate";
        internal const string SQL_TABLE_FIELD = "[SupplierCode],[PayrateXML]";

        #region 获取供应商通道费率
        
        /// <summary>
        /// 获取供应商通道费率
        /// </summary>
        /// <param name="supplierCode"></param>
        /// <param name="channelTypeId"></param>
        /// <returns></returns>
        public static decimal GetSupplierChannelTypeRate(int supplierCode, int channelTypeId)
        {
            string sql = "select PayrateXML.query('(/PayRate/ChannelType[@ID=\"" + channelTypeId.ToString() + "\"])').value('(/ChannelType/@Rate)[1]','nvarchar(max)') from sys_supplier_payrate where SupplierCode = " + supplierCode.ToString();
            string tmp = Convert.ToString(DataBase.ExecuteScalar(CommandType.Text, sql));

            return string.IsNullOrEmpty(tmp) ? 0 : Utils.StrToDecimal(tmp);
        }

        #endregion

        public static int Edit(SysSupplierPayRateInfo model)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@suppliercode",SqlDbType.Int),
                    new SqlParameter("@payratexml",SqlDbType.VarChar,4000)
                };
                parameters[0].Value = model.SupplierCode;
                parameters[1].Value = model.PayrateXML;

                return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_sys_supplierpayrate_edit", parameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        private static void ClearCache()
        {
            string key = OriginalStudio.BLL.Channel.Channel.CHANEL_CACHEKEY;
            DefaultCacheStrategy.GetWebCacheObj.Remove(key);
            key = ChannelType.CHANNELTYPE_CACHEKEY;
            DefaultCacheStrategy.GetWebCacheObj.Remove(key);
            key = CACHE_KEY;
            DefaultCacheStrategy.GetWebCacheObj.Remove(key);
        }

        public static DataTable GetCacheList()
        {
            string objId = CACHE_KEY;
            DataSet o = new DataSet();
            o = (DataSet) WebCache.GetCacheService().RetrieveObject(objId);
            if (o == null)
            {
                SqlDependency dependency = DataBase.AddSqlDependency(objId, "sys_supplier_payrate", "SupplierCode,PayrateXML", "", null);
                o = GetList(string.Empty);
                WebCache.GetCacheService().AddObject(objId, o);
            }
            return o.Tables[0];
        }

        public static DataSet GetList(string strWhere)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select [SupplierCode],[PayrateXML] FROM sys_supplier_payrate ");
            if (strWhere.Trim() != "")
            {
                builder.Append(" where " + strWhere);
            }
            return DataBase.ExecuteDataset(CommandType.Text, builder.ToString());
        }

        public static SysSupplierPayRateInfo GetModel(int supplierid)
        {
            SqlParameter[] commandParameters = new SqlParameter[] {
                new SqlParameter("@SupplierCode", SqlDbType.Int, 10)
            };
            commandParameters[0].Value = supplierid;
            SysSupplierPayRateInfo model = new SysSupplierPayRateInfo();
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_sys_supplierpayrate_GetModel", commandParameters);
            if (set.Tables[0].Rows.Count > 0)
            {
                model.SupplierCode = int.Parse(set.Tables[0].Rows[0]["SupplierCode"].ToString());
                model.PayrateXML = Convert.ToString(set.Tables[0].Rows[0]["PayrateXML"].ToString());

                return model;
            }
            return null;
        }

    }
}

