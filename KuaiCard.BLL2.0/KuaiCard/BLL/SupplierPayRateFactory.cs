namespace KuaiCard.BLL
{
    using DBAccess;
    using KuaiCard.BLL.Channel;
    using KuaiCard.Cache;
    using KuaiCard.Model;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using KuaiCard.BLL.Sys;

    public sealed class SupplierPayRateFactory
    {
        public static string CACHE_KEY = (Constant.Cache_Mark + "SUPPPAYRATE");
        internal const string SQL_TABLE = "supplierpayrate";
        internal const string SQL_TABLE_FIELD = "id,supplierid,p980,p990,p1020,p98,p99,p100,p101,p102,p103,p104,p105,p106,p107,p108,p109,p110,p111,p112,p113,p114,p115,p116,p117,p118,p119,p300,p200,p201,p202,p203,p204,p205,p206,p207,p208,p209,p210";

        public static int Add(SupplierPayRateInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@supplierid", SqlDbType.Int, 10), new SqlParameter("@p98", SqlDbType.Decimal, 9), new SqlParameter("@p99", SqlDbType.Decimal, 9), new SqlParameter("@p100", SqlDbType.Decimal, 9), new SqlParameter("@p101", SqlDbType.Decimal, 9), new SqlParameter("@p102", SqlDbType.Decimal, 9), new SqlParameter("@p103", SqlDbType.Decimal, 9), new SqlParameter("@p104", SqlDbType.Decimal, 9), new SqlParameter("@p105", SqlDbType.Decimal, 9), new SqlParameter("@p106", SqlDbType.Decimal, 9), new SqlParameter("@p107", SqlDbType.Decimal, 9), new SqlParameter("@p108", SqlDbType.Decimal, 9), new SqlParameter("@p109", SqlDbType.Decimal, 9), new SqlParameter("@p110", SqlDbType.Decimal, 9), new SqlParameter("@p111", SqlDbType.Decimal, 9), new SqlParameter("@p112", SqlDbType.Decimal, 9), 
                    new SqlParameter("@p113", SqlDbType.Decimal, 9), new SqlParameter("@p114", SqlDbType.Decimal, 9), new SqlParameter("@p115", SqlDbType.Decimal, 9), new SqlParameter("@p116", SqlDbType.Decimal, 9), new SqlParameter("@p117", SqlDbType.Decimal, 9), new SqlParameter("@p118", SqlDbType.Decimal, 9), new SqlParameter("@p119", SqlDbType.Decimal, 9), new SqlParameter("@p300", SqlDbType.Decimal, 9), new SqlParameter("@p200", SqlDbType.Decimal, 9), new SqlParameter("@p201", SqlDbType.Decimal, 9), new SqlParameter("@p202", SqlDbType.Decimal, 9), new SqlParameter("@p203", SqlDbType.Decimal, 9), new SqlParameter("@p204", SqlDbType.Decimal, 9), new SqlParameter("@p205", SqlDbType.Decimal, 9), new SqlParameter("@p206", SqlDbType.Decimal, 9), new SqlParameter("@p207", SqlDbType.Decimal, 9), 
                    new SqlParameter("@p208", SqlDbType.Decimal, 9), new SqlParameter("@p209", SqlDbType.Decimal, 9), new SqlParameter("@p210", SqlDbType.Decimal, 9), new SqlParameter("@p980", SqlDbType.Decimal, 9), new SqlParameter("@p990", SqlDbType.Decimal, 9), new SqlParameter("@p1020", SqlDbType.Decimal, 9)
                 };
                commandParameters[0].Value = model.supplierid;
                commandParameters[1].Value = model.p98;
                commandParameters[2].Value = model.p99;
                commandParameters[3].Value = model.p100;
                commandParameters[4].Value = model.p101;
                commandParameters[5].Value = model.p102;
                commandParameters[6].Value = model.p103;
                commandParameters[7].Value = model.p104;
                commandParameters[8].Value = model.p105;
                commandParameters[9].Value = model.p106;
                commandParameters[10].Value = model.p107;
                commandParameters[11].Value = model.p108;
                commandParameters[12].Value = model.p109;
                commandParameters[13].Value = model.p110;
                commandParameters[14].Value = model.p111;
                commandParameters[15].Value = model.p112;
                commandParameters[0x10].Value = model.p113;
                commandParameters[0x11].Value = model.p114;
                commandParameters[0x12].Value = model.p115;
                commandParameters[0x13].Value = model.p116;
                commandParameters[20].Value = model.p117;
                commandParameters[0x15].Value = model.p118;
                commandParameters[0x16].Value = model.p119;
                commandParameters[0x17].Value = model.p300;
                commandParameters[0x18].Value = model.p200;
                commandParameters[0x19].Value = model.p201;
                commandParameters[0x1a].Value = model.p202;
                commandParameters[0x1b].Value = model.p203;
                commandParameters[0x1c].Value = model.p204;
                commandParameters[0x1d].Value = model.p205;
                commandParameters[30].Value = model.p206;
                commandParameters[0x1f].Value = model.p207;
                commandParameters[0x20].Value = model.p208;
                commandParameters[0x21].Value = model.p209;
                commandParameters[0x22].Value = model.p210;
                commandParameters[0x23].Value = model.p980;
                commandParameters[0x24].Value = model.p990;
                commandParameters[0x25].Value = model.p1020;
                int num = Convert.ToInt32(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_supplierpayrate_add", commandParameters));
                if (num > 0)
                {
                    ClearCache();
                }
                return num;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        private static void ClearCache()
        {
            string key = KuaiCard.BLL.Channel.Channel.CHANEL_CACHEKEY;
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
                SqlDependency dependency = DataBase.AddSqlDependency(objId, "supplierpayrate", "id,supplierid,p980,p990,p1020,p98,p99,p100,p101,p102,p103,p104,p105,p106,p107,p108,p109,p110,p111,p112,p113,p114,p115,p116,p117,p118,p119,p300,p200,p201,p202,p203,p204,p205,p206,p207,p208,p209,p210", "", null);
                o = GetList(string.Empty);
                WebCache.GetCacheService().AddObject(objId, o);
            }
            return o.Tables[0];
        }

        public static DataSet GetList(string strWhere)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select id,supplierid,p980,p990,p1020,p98,p99,p100,p101,p102,p103,p104,p105,p106,p107,p108,p109,p110,p111,p112,p113,p114,p115,p116,p117,p118,p119,p300,p200,p201,p202,p203,p204,p205,p206,p207,p208,p209,p210 ");
            builder.Append(" FROM supplierpayrate ");
            if (strWhere.Trim() != "")
            {
                builder.Append(" where " + strWhere);
            }
            return DataBase.ExecuteDataset(CommandType.Text, builder.ToString());
        }

        public static SupplierPayRateInfo GetModel(int supplierid)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@supplierid", SqlDbType.Int, 10) };
            commandParameters[0].Value = supplierid;
            SupplierPayRateInfo info = new SupplierPayRateInfo();
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_supplierpayrate_GetModel", commandParameters);
            if (set.Tables[0].Rows.Count > 0)
            {
                if (set.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    info.id = int.Parse(set.Tables[0].Rows[0]["id"].ToString());
                }
                if (set.Tables[0].Rows[0]["supplierid"].ToString() != "")
                {
                    info.supplierid = int.Parse(set.Tables[0].Rows[0]["supplierid"].ToString());
                }
                if (set.Tables[0].Rows[0]["p980"].ToString() != "")
                {
                    info.p980 = decimal.Parse(set.Tables[0].Rows[0]["p980"].ToString());
                }
                if (set.Tables[0].Rows[0]["p990"].ToString() != "")
                {
                    info.p990 = decimal.Parse(set.Tables[0].Rows[0]["p990"].ToString());
                }
                if (set.Tables[0].Rows[0]["p1020"].ToString() != "")
                {
                    info.p1020 = decimal.Parse(set.Tables[0].Rows[0]["p1020"].ToString());
                }
                if (set.Tables[0].Rows[0]["p98"].ToString() != "")
                {
                    info.p98 = decimal.Parse(set.Tables[0].Rows[0]["p98"].ToString());
                }
                if (set.Tables[0].Rows[0]["p99"].ToString() != "")
                {
                    info.p99 = decimal.Parse(set.Tables[0].Rows[0]["p99"].ToString());
                }
                if (set.Tables[0].Rows[0]["p100"].ToString() != "")
                {
                    info.p100 = decimal.Parse(set.Tables[0].Rows[0]["p100"].ToString());
                }
                if (set.Tables[0].Rows[0]["p101"].ToString() != "")
                {
                    info.p101 = decimal.Parse(set.Tables[0].Rows[0]["p101"].ToString());
                }
                if (set.Tables[0].Rows[0]["p102"].ToString() != "")
                {
                    info.p102 = decimal.Parse(set.Tables[0].Rows[0]["p102"].ToString());
                }
                if (set.Tables[0].Rows[0]["p103"].ToString() != "")
                {
                    info.p103 = decimal.Parse(set.Tables[0].Rows[0]["p103"].ToString());
                }
                if (set.Tables[0].Rows[0]["p104"].ToString() != "")
                {
                    info.p104 = decimal.Parse(set.Tables[0].Rows[0]["p104"].ToString());
                }
                if (set.Tables[0].Rows[0]["p105"].ToString() != "")
                {
                    info.p105 = decimal.Parse(set.Tables[0].Rows[0]["p105"].ToString());
                }
                if (set.Tables[0].Rows[0]["p106"].ToString() != "")
                {
                    info.p106 = decimal.Parse(set.Tables[0].Rows[0]["p106"].ToString());
                }
                if (set.Tables[0].Rows[0]["p107"].ToString() != "")
                {
                    info.p107 = decimal.Parse(set.Tables[0].Rows[0]["p107"].ToString());
                }
                if (set.Tables[0].Rows[0]["p108"].ToString() != "")
                {
                    info.p108 = decimal.Parse(set.Tables[0].Rows[0]["p108"].ToString());
                }
                if (set.Tables[0].Rows[0]["p109"].ToString() != "")
                {
                    info.p109 = decimal.Parse(set.Tables[0].Rows[0]["p109"].ToString());
                }
                if (set.Tables[0].Rows[0]["p110"].ToString() != "")
                {
                    info.p110 = decimal.Parse(set.Tables[0].Rows[0]["p110"].ToString());
                }
                if (set.Tables[0].Rows[0]["p111"].ToString() != "")
                {
                    info.p111 = decimal.Parse(set.Tables[0].Rows[0]["p111"].ToString());
                }
                if (set.Tables[0].Rows[0]["p112"].ToString() != "")
                {
                    info.p112 = decimal.Parse(set.Tables[0].Rows[0]["p112"].ToString());
                }
                if (set.Tables[0].Rows[0]["p113"].ToString() != "")
                {
                    info.p113 = decimal.Parse(set.Tables[0].Rows[0]["p113"].ToString());
                }
                if (set.Tables[0].Rows[0]["p114"].ToString() != "")
                {
                    info.p114 = decimal.Parse(set.Tables[0].Rows[0]["p114"].ToString());
                }
                if (set.Tables[0].Rows[0]["p115"].ToString() != "")
                {
                    info.p115 = decimal.Parse(set.Tables[0].Rows[0]["p115"].ToString());
                }
                if (set.Tables[0].Rows[0]["p116"].ToString() != "")
                {
                    info.p116 = decimal.Parse(set.Tables[0].Rows[0]["p116"].ToString());
                }
                if (set.Tables[0].Rows[0]["p117"].ToString() != "")
                {
                    info.p117 = decimal.Parse(set.Tables[0].Rows[0]["p117"].ToString());
                }
                if (set.Tables[0].Rows[0]["p118"].ToString() != "")
                {
                    info.p118 = decimal.Parse(set.Tables[0].Rows[0]["p118"].ToString());
                }
                if (set.Tables[0].Rows[0]["p119"].ToString() != "")
                {
                    info.p119 = decimal.Parse(set.Tables[0].Rows[0]["p119"].ToString());
                }
                if (set.Tables[0].Rows[0]["p300"].ToString() != "")
                {
                    info.p300 = decimal.Parse(set.Tables[0].Rows[0]["p300"].ToString());
                }
                if ((set.Tables[0].Rows[0]["p200"] != null) && (set.Tables[0].Rows[0]["p200"].ToString() != ""))
                {
                    info.p200 = decimal.Parse(set.Tables[0].Rows[0]["p200"].ToString());
                }
                if ((set.Tables[0].Rows[0]["p201"] != null) && (set.Tables[0].Rows[0]["p201"].ToString() != ""))
                {
                    info.p201 = decimal.Parse(set.Tables[0].Rows[0]["p201"].ToString());
                }
                if ((set.Tables[0].Rows[0]["p202"] != null) && (set.Tables[0].Rows[0]["p202"].ToString() != ""))
                {
                    info.p202 = decimal.Parse(set.Tables[0].Rows[0]["p202"].ToString());
                }
                if ((set.Tables[0].Rows[0]["p203"] != null) && (set.Tables[0].Rows[0]["p203"].ToString() != ""))
                {
                    info.p203 = decimal.Parse(set.Tables[0].Rows[0]["p203"].ToString());
                }
                if ((set.Tables[0].Rows[0]["p204"] != null) && (set.Tables[0].Rows[0]["p204"].ToString() != ""))
                {
                    info.p204 = decimal.Parse(set.Tables[0].Rows[0]["p204"].ToString());
                }
                if ((set.Tables[0].Rows[0]["p205"] != null) && (set.Tables[0].Rows[0]["p205"].ToString() != ""))
                {
                    info.p205 = decimal.Parse(set.Tables[0].Rows[0]["p205"].ToString());
                }
                if ((set.Tables[0].Rows[0]["p206"] != null) && (set.Tables[0].Rows[0]["p206"].ToString() != ""))
                {
                    info.p206 = decimal.Parse(set.Tables[0].Rows[0]["p206"].ToString());
                }
                if ((set.Tables[0].Rows[0]["p207"] != null) && (set.Tables[0].Rows[0]["p207"].ToString() != ""))
                {
                    info.p207 = decimal.Parse(set.Tables[0].Rows[0]["p207"].ToString());
                }
                if ((set.Tables[0].Rows[0]["p208"] != null) && (set.Tables[0].Rows[0]["p208"].ToString() != ""))
                {
                    info.p208 = decimal.Parse(set.Tables[0].Rows[0]["p208"].ToString());
                }
                if ((set.Tables[0].Rows[0]["p209"] != null) && (set.Tables[0].Rows[0]["p209"].ToString() != ""))
                {
                    info.p209 = decimal.Parse(set.Tables[0].Rows[0]["p209"].ToString());
                }
                if ((set.Tables[0].Rows[0]["p210"] != null) && (set.Tables[0].Rows[0]["p210"].ToString() != ""))
                {
                    info.p210 = decimal.Parse(set.Tables[0].Rows[0]["p210"].ToString());
                }
                return info;
            }
            return null;
        }

        public static decimal GetRate(int supplierId, int typeId)
        {
            try
            {
                return Convert.ToDecimal(GetCacheList().Select("supplierid=" + supplierId.ToString())[0]["p" + typeId.ToString()]);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0M;
            }
        }
    }
}

