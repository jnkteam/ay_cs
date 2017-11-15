namespace KuaiCard.BLL
{
    using DBAccess;
    using KuaiCard.Cache;
    using KuaiCard.Model;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using KuaiCard.BLL.Sys;

    public sealed class SupplierFactory
    {
        public static string CACHE_KEY = (Constant.Cache_Mark + "SUPPLIER_{0}");
        internal const string SQL_TABLE = "supplier";
        internal const string SQL_TABLE_FIELD = "[id]\r\n      ,[code]\r\n      ,[name]\r\n      ,[name1]\r\n      ,[logourl]\r\n      ,[isbank]\r\n      ,[iscard]\r\n      ,[issms]\r\n      ,[issx],[isdistribution]\r\n      ,[puserid]\r\n      ,[puserkey]\r\n      ,[pusername]\r\n      ,[puserid1]\r\n      ,[puserkey1]\r\n      ,[puserid2]\r\n      ,[puserkey2]\r\n      ,[puserid3]\r\n      ,[puserkey3]\r\n      ,[puserid4]\r\n      ,[puserkey4]\r\n      ,[puserid5]\r\n      ,[puserkey5]\r\n      ,[purl]\r\n      ,[pbakurl]\r\n      ,[jumpUrl]\r\n      ,[pcardbakurl]\r\n      ,[postBankUrl]\r\n      ,[postCardUrl]\r\n      ,[postSMSUrl],[distributionUrl]\r\n      ,[desc]\r\n      ,[sort]\r\n      ,[release]\r\n      ,[issys],[iswap],[isali],[iswx]";

        public static int Add(SupplierInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@id", SqlDbType.Int, 10), 
                    new SqlParameter("@code", SqlDbType.Int, 10), 
                    new SqlParameter("@name", SqlDbType.VarChar, 50), 
                    new SqlParameter("@logourl", SqlDbType.VarChar, 50), 
                    new SqlParameter("@isbank", SqlDbType.Bit, 1), 
                    new SqlParameter("@iscard", SqlDbType.Bit, 1), 
                    new SqlParameter("@issms", SqlDbType.Bit, 1), 
                    new SqlParameter("@issx", SqlDbType.Bit, 1), 
                    new SqlParameter("@puserid", SqlDbType.VarChar, 100), 
                    new SqlParameter("@puserkey", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@pusername", SqlDbType.VarChar, 50), 
                    new SqlParameter("@puserid1", SqlDbType.VarChar, 100), 
                    new SqlParameter("@puserkey1", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@puserid2", SqlDbType.VarChar, 100), 
                    new SqlParameter("@puserkey2", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@puserid3", SqlDbType.VarChar, 100), 
                    new SqlParameter("@puserkey3", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@puserid4", SqlDbType.VarChar, 100), 
                    new SqlParameter("@puserkey4", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@puserid5", SqlDbType.VarChar, 100), 
                    new SqlParameter("@puserkey5", SqlDbType.VarChar, 2000),
                    new SqlParameter("@purl", SqlDbType.VarChar, 50), 
                    new SqlParameter("@pbakurl", SqlDbType.VarChar, 50), 
                    new SqlParameter("@postBankUrl", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@postCardUrl", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@postSMSUrl", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@desc", SqlDbType.NVarChar, 0x7d0), 
                    new SqlParameter("@sort", SqlDbType.Int, 10), 
                    new SqlParameter("@release", SqlDbType.Bit, 1), 
                    new SqlParameter("@issys", SqlDbType.Bit, 1), 
                    new SqlParameter("@pcardbakurl", SqlDbType.VarChar, 50), 
                    new SqlParameter("@name1", SqlDbType.VarChar, 100), 
                    new SqlParameter("@jumpUrl", SqlDbType.NVarChar, 0xff), 
                    new SqlParameter("@isdistribution", SqlDbType.Bit, 1), 
                    new SqlParameter("@distributionUrl", SqlDbType.VarChar, 0xff), 
                    new SqlParameter("@queryCardUrl", SqlDbType.VarChar, 0xff), 
                    new SqlParameter("@iswap", SqlDbType.Bit, 1), 
                    new SqlParameter("@isali", SqlDbType.Bit, 1), 
                    new SqlParameter("@iswx", SqlDbType.Bit, 1)
                 };
                commandParameters[0].Direction = ParameterDirection.Output;
                commandParameters[1].Value = model.code;
                commandParameters[2].Value = model.name;
                commandParameters[3].Value = model.logourl;
                commandParameters[4].Value = model.isbank;
                commandParameters[5].Value = model.iscard;
                commandParameters[6].Value = model.issms;
                commandParameters[7].Value = model.issx;
                commandParameters[8].Value = model.puserid;
                commandParameters[9].Value = model.puserkey;
                commandParameters[10].Value = model.pusername;
                commandParameters[11].Value = model.puserid1;
                commandParameters[12].Value = model.puserkey1;
                commandParameters[13].Value = model.puserid2;
                commandParameters[14].Value = model.puserkey2;
                commandParameters[15].Value = model.puserid3;
                commandParameters[0x10].Value = model.puserkey3;
                commandParameters[0x11].Value = model.puserid4;
                commandParameters[0x12].Value = model.puserkey4;
                commandParameters[0x13].Value = model.puserid5;
                commandParameters[20].Value = model.puserkey5;
                commandParameters[0x15].Value = model.purl;
                commandParameters[0x16].Value = model.pbakurl;
                commandParameters[0x17].Value = model.postBankUrl;
                commandParameters[0x18].Value = model.postCardUrl;
                commandParameters[0x19].Value = model.postSMSUrl;
                commandParameters[0x1a].Value = model.desc;
                commandParameters[0x1b].Value = model.sort;
                commandParameters[0x1c].Value = model.release;
                commandParameters[0x1d].Value = model.issys;
                commandParameters[30].Value = model.pcardbakurl;
                commandParameters[0x1f].Value = model.name1;
                commandParameters[0x20].Value = model.jumpUrl;
                commandParameters[0x21].Value = model.isdistribution;
                commandParameters[0x22].Value = model.distributionUrl;
                commandParameters[0x23].Value = model.queryCardUrl;
                commandParameters[0x24].Value = model.iswap;
                commandParameters[0x25].Value = model.isali;
                commandParameters[0x26].Value = model.iswx;
                int num = DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_supplier_add", commandParameters);
                ClearCache(model.code.Value);
                return (int) commandParameters[0].Value;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        private static void ClearCache(int code)
        {
            string objId = string.Format(CACHE_KEY, code);
            WebCache.GetCacheService().RemoveObject(objId);
        }

        public static SupplierInfo GetCacheModel(int code)
        {
            SupplierInfo o = new SupplierInfo();
            string objId = string.Format(CACHE_KEY, code);

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("code", code);
            SqlDependency dependency = DataBase.AddSqlDependency(objId, "supplier", "[id]\r\n      ,[code]\r\n      ,[name]\r\n      ,[name1]\r\n      ,[logourl]\r\n      ,[isbank]\r\n      ,[iscard]\r\n      ,[issms]\r\n      ,[issx],[isdistribution]\r\n      ,[puserid]\r\n      ,[puserkey]\r\n      ,[pusername]\r\n      ,[puserid1]\r\n      ,[puserkey1]\r\n      ,[puserid2]\r\n      ,[puserkey2]\r\n      ,[puserid3]\r\n      ,[puserkey3]\r\n      ,[puserid4]\r\n      ,[puserkey4]\r\n      ,[puserid5]\r\n      ,[puserkey5]\r\n      ,[purl]\r\n      ,[pbakurl]\r\n      ,[jumpUrl]\r\n      ,[pcardbakurl]\r\n      ,[postBankUrl]\r\n      ,[postCardUrl]\r\n      ,[postSMSUrl],[distributionUrl]\r\n      ,[desc]\r\n      ,[sort]\r\n      ,[release]\r\n      ,[issys],[iswap],[isali],[iswx]", "[code]=@code", parameters);
            o = GetModelByCode(code);
            return o;
        }

        public static DataSet GetList()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select id,code,name,name1,logourl,isbank,iscard,issms,issx,puserid,puserkey,pusername,puserid1,puserkey1,puserid2,puserkey2,puserid3,puserkey3,puserid4,puserkey4,puserid5,puserkey5,purl,pbakurl,postBankUrl,postCardUrl,postSMSUrl,[desc],sort,release,issys,pcardbakurl,iswap,isali,iswx ");
            builder.Append(" FROM supplier ");
            builder.Append(" Order by sort ");
            return DataBase.ExecuteDataset(CommandType.Text, builder.ToString());
        }

        public static DataSet GetList(string where)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select id,code,name,name1,logourl,isbank,iscard,issms,issx,puserid,puserkey,pusername,puserid1,puserkey1,puserid2,puserkey2,puserid3,puserkey3,puserid4,puserkey4,puserid5,puserkey5,purl,pbakurl,postBankUrl,postCardUrl,postSMSUrl,[desc],sort,release,issys,pcardbakurl,iswap,isali,iswx ");
            builder.Append(" FROM supplier ");
            if (!string.IsNullOrEmpty(where))
            {
                builder.AppendFormat(" where {0}", where);
            }
            builder.Append(" Order by sort ");
            return DataBase.ExecuteDataset(CommandType.Text, builder.ToString());
        }

        public static SupplierInfo GetModel(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = id;
                return GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_supplier_GetModel", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static SupplierInfo GetModelByCode(int code)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@code", SqlDbType.Int, 10) 
                };
                commandParameters[0].Value = code;
                return GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_supplier_GetModelBycode", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static SupplierInfo GetModelFromDs(DataSet ds)
        {
            SupplierInfo info = new SupplierInfo();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    info.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["code"].ToString() != "")
                {
                    info.code = new int?(int.Parse(ds.Tables[0].Rows[0]["code"].ToString()));
                }
                info.name = ds.Tables[0].Rows[0]["name"].ToString();
                info.name1 = ds.Tables[0].Rows[0]["name1"].ToString();
                info.logourl = ds.Tables[0].Rows[0]["logourl"].ToString();
                info.pcardbakurl = ds.Tables[0].Rows[0]["pcardbakurl"].ToString();
                if (ds.Tables[0].Rows[0]["isbank"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["isbank"].ToString() == "1") || (ds.Tables[0].Rows[0]["isbank"].ToString().ToLower() == "true"))
                    {
                        info.isbank = true;
                    }
                    else
                    {
                        info.isbank = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["iscard"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["iscard"].ToString() == "1") || (ds.Tables[0].Rows[0]["iscard"].ToString().ToLower() == "true"))
                    {
                        info.iscard = true;
                    }
                    else
                    {
                        info.iscard = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["issms"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["issms"].ToString() == "1") || (ds.Tables[0].Rows[0]["issms"].ToString().ToLower() == "true"))
                    {
                        info.issms = true;
                    }
                    else
                    {
                        info.issms = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["issx"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["issx"].ToString() == "1") || (ds.Tables[0].Rows[0]["issx"].ToString().ToLower() == "true"))
                    {
                        info.issx = true;
                    }
                    else
                    {
                        info.issx = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["isdistribution"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["isdistribution"].ToString() == "1") || (ds.Tables[0].Rows[0]["isdistribution"].ToString().ToLower() == "true"))
                    {
                        info.isdistribution = true;
                    }
                    else
                    {
                        info.isdistribution = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["iswap"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["iswap"].ToString() == "1") || (ds.Tables[0].Rows[0]["iswap"].ToString().ToLower() == "true"))
                    {
                        info.iswap = true;
                    }
                    else
                    {
                        info.iswap = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["isali"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["isali"].ToString() == "1") || (ds.Tables[0].Rows[0]["isali"].ToString().ToLower() == "true"))
                    {
                        info.isali = true;
                    }
                    else
                    {
                        info.isali = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["iswx"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["iswx"].ToString() == "1") || (ds.Tables[0].Rows[0]["iswx"].ToString().ToLower() == "true"))
                    {
                        info.iswx = true;
                    }
                    else
                    {
                        info.iswx = false;
                    }
                }
                info.puserid = ds.Tables[0].Rows[0]["puserid"].ToString();
                info.puserkey = ds.Tables[0].Rows[0]["puserkey"].ToString();
                info.pusername = ds.Tables[0].Rows[0]["pusername"].ToString();
                info.puserid1 = ds.Tables[0].Rows[0]["puserid1"].ToString();
                info.puserkey1 = ds.Tables[0].Rows[0]["puserkey1"].ToString();
                info.puserid2 = ds.Tables[0].Rows[0]["puserid2"].ToString();
                info.puserkey2 = ds.Tables[0].Rows[0]["puserkey2"].ToString();
                info.puserid3 = ds.Tables[0].Rows[0]["puserid3"].ToString();
                info.puserkey3 = ds.Tables[0].Rows[0]["puserkey3"].ToString();
                info.puserid4 = ds.Tables[0].Rows[0]["puserid4"].ToString();
                info.puserkey4 = ds.Tables[0].Rows[0]["puserkey4"].ToString();
                info.puserid5 = ds.Tables[0].Rows[0]["puserid5"].ToString();
                info.puserkey5 = ds.Tables[0].Rows[0]["puserkey5"].ToString();
                info.purl = ds.Tables[0].Rows[0]["purl"].ToString();
                info.pbakurl = ds.Tables[0].Rows[0]["pbakurl"].ToString();
                info.postBankUrl = ds.Tables[0].Rows[0]["postBankUrl"].ToString();
                info.jumpUrl = ds.Tables[0].Rows[0]["jumpUrl"].ToString();
                info.postCardUrl = ds.Tables[0].Rows[0]["postCardUrl"].ToString();
                info.postSMSUrl = ds.Tables[0].Rows[0]["postSMSUrl"].ToString();
                info.distributionUrl = ds.Tables[0].Rows[0]["distributionUrl"].ToString();
                info.queryCardUrl = ds.Tables[0].Rows[0]["queryCardUrl"].ToString();
                info.desc = ds.Tables[0].Rows[0]["desc"].ToString();
                if (ds.Tables[0].Rows[0]["sort"].ToString() != "")
                {
                    info.sort = new int?(int.Parse(ds.Tables[0].Rows[0]["sort"].ToString()));
                }
                if (ds.Tables[0].Rows[0]["release"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["release"].ToString() == "1") || (ds.Tables[0].Rows[0]["release"].ToString().ToLower() == "true"))
                    {
                        info.release = true;
                    }
                    else
                    {
                        info.release = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["issys"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["issys"].ToString() == "1") || (ds.Tables[0].Rows[0]["issys"].ToString().ToLower() == "true"))
                    {
                        info.issys = true;
                        return info;
                    }
                    info.issys = false;
                }
                return info;
            }
            return null;
        }

        public static bool Update(SupplierInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@id", SqlDbType.Int, 10), 
                    new SqlParameter("@code", SqlDbType.Int, 10), 
                    new SqlParameter("@name", SqlDbType.VarChar, 50), 
                    new SqlParameter("@logourl", SqlDbType.VarChar, 50), 
                    new SqlParameter("@isbank", SqlDbType.Bit, 1), 
                    new SqlParameter("@iscard", SqlDbType.Bit, 1), 
                    new SqlParameter("@issms", SqlDbType.Bit, 1), 
                    new SqlParameter("@issx", SqlDbType.Bit, 1), 
                    new SqlParameter("@puserid", SqlDbType.VarChar, 100), 
                    new SqlParameter("@puserkey", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@pusername", SqlDbType.VarChar, 50), 
                    new SqlParameter("@puserid1", SqlDbType.VarChar, 100), 
                    new SqlParameter("@puserkey1", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@puserid2", SqlDbType.VarChar, 100), 
                    new SqlParameter("@puserkey2", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@puserid3", SqlDbType.VarChar, 1000), 
                    new SqlParameter("@puserkey3", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@puserid4", SqlDbType.VarChar, 100), 
                    new SqlParameter("@puserkey4", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@puserid5", SqlDbType.VarChar, 100), 
                    new SqlParameter("@puserkey5", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@purl", SqlDbType.VarChar, 50), 
                    new SqlParameter("@pbakurl", SqlDbType.VarChar, 50), 
                    new SqlParameter("@postBankUrl", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@postCardUrl", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@postSMSUrl", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@desc", SqlDbType.NVarChar, 0x7d0), 
                    new SqlParameter("@sort", SqlDbType.Int, 10), 
                    new SqlParameter("@release", SqlDbType.Bit, 1), 
                    new SqlParameter("@issys", SqlDbType.Bit, 1), 
                    new SqlParameter("@pcardbakurl", SqlDbType.VarChar, 50), 
                    new SqlParameter("@name1", SqlDbType.VarChar, 100), 
                    new SqlParameter("@jumpUrl", SqlDbType.NVarChar, 0xff), 
                    new SqlParameter("@isdistribution", SqlDbType.Bit, 1), 
                    new SqlParameter("@distributionUrl", SqlDbType.VarChar, 0xff), 
                    new SqlParameter("@queryCardUrl", SqlDbType.VarChar, 0xff), 
                    new SqlParameter("@iswap", SqlDbType.Bit, 1), 
                    new SqlParameter("@isali", SqlDbType.Bit, 1), 
                    new SqlParameter("@iswx", SqlDbType.Bit, 1)
                 };
                commandParameters[0].Value = model.id;
                commandParameters[1].Value = model.code;
                commandParameters[2].Value = model.name;
                commandParameters[3].Value = model.logourl;
                commandParameters[4].Value = model.isbank;
                commandParameters[5].Value = model.iscard;
                commandParameters[6].Value = model.issms;
                commandParameters[7].Value = model.issx;
                commandParameters[8].Value = model.puserid;
                commandParameters[9].Value = model.puserkey;
                commandParameters[10].Value = model.pusername;
                commandParameters[11].Value = model.puserid1;
                commandParameters[12].Value = model.puserkey1;
                commandParameters[13].Value = model.puserid2;
                commandParameters[14].Value = model.puserkey2;
                commandParameters[15].Value = model.puserid3;
                commandParameters[0x10].Value = model.puserkey3;
                commandParameters[0x11].Value = model.puserid4;
                commandParameters[0x12].Value = model.puserkey4;
                commandParameters[0x13].Value = model.puserid5;
                commandParameters[20].Value = model.puserkey5;
                commandParameters[0x15].Value = model.purl;
                commandParameters[0x16].Value = model.pbakurl;
                commandParameters[0x17].Value = model.postBankUrl;
                commandParameters[0x18].Value = model.postCardUrl;
                commandParameters[0x19].Value = model.postSMSUrl;
                commandParameters[0x1a].Value = model.desc;
                commandParameters[0x1b].Value = model.sort;
                commandParameters[0x1c].Value = model.release;
                commandParameters[0x1d].Value = model.issys;
                commandParameters[30].Value = model.pcardbakurl;
                commandParameters[0x1f].Value = model.name1;
                commandParameters[0x20].Value = model.jumpUrl;
                commandParameters[0x21].Value = model.isdistribution;
                commandParameters[0x22].Value = model.distributionUrl;
                commandParameters[0x23].Value = model.queryCardUrl;
                commandParameters[0x24].Value = model.iswap;
                commandParameters[0x25].Value = model.isali;
                commandParameters[0x26].Value = model.iswx;
                if (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_supplier_Update", commandParameters) > 0)
                {
                    ClearCache(model.code.Value);
                    return true;
                }
                return false;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        #region 2017.4.2 增加服务商通道设置窗口

        /// <summary>
        /// 获取服务商通道列表
        /// </summary>
        /// <param name="p_userid"></param>
        /// <returns></returns>
        public static DataSet GetUserSupplierList(int p_userid)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@userid", SqlDbType.Int, 10) 
                };
                commandParameters[0].Value = p_userid;
                return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_user_supplier_getList", commandParameters);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        /// <summary>
        /// 获取商户对应对立服务商信息
        /// </summary>
        /// <param name="p_userid"></param>
        /// <param name="p_supplier_code"></param>
        /// <param name="p_typeId"></param>
        /// <returns></returns>
        public static UserSupplierInfo GetUserSupplierInfo(int p_userid, int p_supplier_code, int p_typeId)
        {
            UserSupplierInfo obj = new UserSupplierInfo();
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] 
                {
                    new SqlParameter("@supplier_code", SqlDbType.Int, 10) ,
                    new SqlParameter("@userid", SqlDbType.Int, 10) ,
                    new SqlParameter("@typeId", SqlDbType.Int, 10) 
                };
                commandParameters[0].Value = p_supplier_code;
                commandParameters[1].Value = p_userid;
                commandParameters[2].Value = p_typeId;
                DataSet ds = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_user_supplier_GetModel", commandParameters);

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        obj.ID = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                        obj.PUserID = dt.Rows[0]["puserid"].ToString();
                        obj.PUserKey = dt.Rows[0]["puserkey"].ToString();
                        obj.PUserName = dt.Rows[0]["pusername"].ToString();
                        obj.PUserID1 = dt.Rows[0]["puserid1"].ToString();
                        obj.PUserKey1 = dt.Rows[0]["puserkey1"].ToString();
                        obj.PUserID2 = dt.Rows[0]["puserid2"].ToString();
                        obj.PUserKey2 = dt.Rows[0]["puserkey2"].ToString();
                        obj.ExtParm1 = dt.Rows[0]["extparm1"].ToString();
                        obj.ExtParm2 = dt.Rows[0]["extparm2"].ToString();
                        obj.ExtParm3 = dt.Rows[0]["extparm3"].ToString();
                        obj.ExtParm4 = dt.Rows[0]["extparm4"].ToString();
                    }
                }                
                return obj;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }

        }

        /// <summary>
        /// 根据订单获取服务商信息。用在订单处理的地方。
        /// </summary>
        /// <param name="p_orderid"></param>
        /// <param name="p_supplier_code"></param>
        /// <returns></returns>
        public static UserSupplierInfo GetUserSupplierInfo(string p_orderid, int p_supplier_code)
        {
            UserSupplierInfo obj = new UserSupplierInfo();
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] 
                {
                    new SqlParameter("@supplier_code", SqlDbType.Int, 10) ,
                    new SqlParameter("@orderid", SqlDbType.VarChar, 30)
                };
                commandParameters[0].Value = p_supplier_code;
                commandParameters[1].Value = p_orderid;
                DataSet ds = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_user_order_supplier_GetModel", commandParameters);

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        obj.ID = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                        obj.PUserID = dt.Rows[0]["puserid"].ToString();
                        obj.PUserKey = dt.Rows[0]["puserkey"].ToString();
                        obj.PUserName = dt.Rows[0]["pusername"].ToString();
                        obj.PUserID1 = dt.Rows[0]["puserid1"].ToString();
                        obj.PUserKey1 = dt.Rows[0]["puserkey1"].ToString();
                        obj.PUserID2 = dt.Rows[0]["puserid2"].ToString();
                        obj.PUserKey2 = dt.Rows[0]["puserkey2"].ToString();
                        obj.ExtParm1 = dt.Rows[0]["extparm1"].ToString();
                        obj.ExtParm2 = dt.Rows[0]["extparm2"].ToString();
                        obj.ExtParm3 = dt.Rows[0]["extparm3"].ToString();
                        obj.ExtParm4 = dt.Rows[0]["extparm4"].ToString();
                    }
                }

                return obj;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        /// <summary>
        /// 代付服务商账号信息
        /// </summary>
        /// <param name="p_trade_no"></param>
        /// <param name="p_supplier_code"></param>
        /// <returns></returns>
        public static UserSupplierInfo GetTradeUserSupplierInfo(string p_trade_no, int p_supplier_code)
        {
            UserSupplierInfo obj = new UserSupplierInfo();
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] 
                {
                    new SqlParameter("@supplier_code", SqlDbType.Int, 10) ,
                    new SqlParameter("@trade_no", SqlDbType.VarChar, 30)
                };
                commandParameters[0].Value = p_supplier_code;
                commandParameters[1].Value = p_trade_no;
                DataSet ds = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_user_settled_supplier_GetModel", commandParameters);

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        obj.ID = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                        obj.PUserID = dt.Rows[0]["puserid"].ToString();
                        obj.PUserKey = dt.Rows[0]["puserkey"].ToString();
                        obj.PUserName = dt.Rows[0]["pusername"].ToString();
                        obj.PUserID1 = dt.Rows[0]["puserid1"].ToString();
                        obj.PUserKey1 = dt.Rows[0]["puserkey1"].ToString();
                        obj.PUserID2 = dt.Rows[0]["puserid2"].ToString();
                        obj.PUserKey2 = dt.Rows[0]["puserkey2"].ToString();
                        obj.ExtParm1 = dt.Rows[0]["extparm1"].ToString();
                        obj.ExtParm2 = dt.Rows[0]["extparm2"].ToString();
                        obj.ExtParm3 = dt.Rows[0]["extparm3"].ToString();
                        obj.ExtParm4 = dt.Rows[0]["extparm4"].ToString();
                    }
                }

                return obj;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        /// <summary>
        /// 保存用户服务商通道信息
        /// </summary>
        /// <param name="p_obj"></param>
        /// <returns></returns>
        public static Int32 SaveUserSupplierInfo(UserSupplierInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@id", SqlDbType.Int, 10), 
                    new SqlParameter("@userid", SqlDbType.Int, 10), 
                    new SqlParameter("@supplier_code", SqlDbType.Int, 10), 
                    new SqlParameter("@name", SqlDbType.VarChar, 50), 
                    new SqlParameter("@puserid", SqlDbType.VarChar, 100), 
                    new SqlParameter("@puserkey", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@pusername", SqlDbType.VarChar, 50), 
                    new SqlParameter("@puserid1", SqlDbType.VarChar, 100), 
                    new SqlParameter("@puserkey1", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@puserid2", SqlDbType.VarChar, 100), 
                    new SqlParameter("@puserkey2", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@postBankUrl", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@desc", SqlDbType.NVarChar, 2000), 
                    new SqlParameter("@sort", SqlDbType.Int, 10), 
                    new SqlParameter("@jumpUrl", SqlDbType.NVarChar, 1000), 
                    new SqlParameter("@active", SqlDbType.Int, 10), 
                    new SqlParameter("@extparm1", SqlDbType.NVarChar, 1000), 
                    new SqlParameter("@extparm2", SqlDbType.NVarChar, 1000), 
                    new SqlParameter("@extparm3", SqlDbType.NVarChar, 1000), 
                    new SqlParameter("@extparm4", SqlDbType.NVarChar, 1000), 
                    new SqlParameter("@distributionUrl", SqlDbType.VarChar, 2000)
                 };
                commandParameters[0].Direction = ParameterDirection.Output;
                commandParameters[1].Value = model.UserID;
                commandParameters[2].Value = model.SupplierCode;
                commandParameters[3].Value = model.Name;
                commandParameters[4].Value = model.PUserID;
                commandParameters[5].Value = model.PUserKey;
                commandParameters[6].Value = model.PUserName;
                commandParameters[7].Value = model.PUserID1;
                commandParameters[8].Value = model.PUserKey1;
                commandParameters[9].Value = model.PUserID2;
                commandParameters[10].Value = model.PUserKey2;
                commandParameters[11].Value = model.PostBankUrl;
                commandParameters[12].Value = model.Desc;
                commandParameters[13].Value = model.Sort;
                commandParameters[14].Value = model.JumpUrl;
                commandParameters[15].Value = model.Active;
                commandParameters[16].Value = model.ExtParm1;
                commandParameters[17].Value = model.ExtParm2;
                commandParameters[18].Value = model.ExtParm3;
                commandParameters[19].Value = model.ExtParm4;                
                commandParameters[20].Value = model.distributionUrl;
                int num = DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_user_supplier_add", commandParameters);
                int ID =  (int)commandParameters[0].Value;

                if (model.ChannelTypeList.Count > 0)
                {
                    SqlParameter[] commandParameters2 = new SqlParameter[] { 
                        new SqlParameter("@id", SqlDbType.Int, 10), 
                        new SqlParameter("@channeltypeId", SqlDbType.Int, 10)
                    };
                    foreach (string channelType in model.ChannelTypeList)
                    {
                        commandParameters2[0].Value = ID;
                        commandParameters2[1].Value = Convert.ToInt32(channelType);

                        DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_user_supplier_channeltype_add", commandParameters2);
                    }
                }

                return ID;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        /// <summary>
        /// 更新服务商通道信息。
        /// </summary>
        public static Int32 UpdateUserSupplierInfo(UserSupplierInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@id", SqlDbType.Int, 10), 
                    new SqlParameter("@name", SqlDbType.VarChar, 50), 
                    new SqlParameter("@puserid", SqlDbType.VarChar, 100), 
                    new SqlParameter("@puserkey", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@pusername", SqlDbType.VarChar, 50), 
                    new SqlParameter("@puserid1", SqlDbType.VarChar, 100), 
                    new SqlParameter("@puserkey1", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@puserid2", SqlDbType.VarChar, 100), 
                    new SqlParameter("@puserkey2", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@postBankUrl", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@desc", SqlDbType.NVarChar, 2000), 
                    new SqlParameter("@sort", SqlDbType.Int, 10), 
                    new SqlParameter("@jumpUrl", SqlDbType.NVarChar, 1000), 
                    new SqlParameter("@extparm1", SqlDbType.NVarChar, 1000), 
                    new SqlParameter("@extparm2", SqlDbType.NVarChar, 1000), 
                    new SqlParameter("@extparm3", SqlDbType.NVarChar, 1000), 
                    new SqlParameter("@extparm4", SqlDbType.NVarChar, 1000), 
                    new SqlParameter("@distributionUrl", SqlDbType.VarChar, 2000)
                 };
                commandParameters[0].Value = model.ID;
                commandParameters[1].Value = model.Name;
                commandParameters[2].Value = model.PUserID;
                commandParameters[3].Value = model.PUserKey;
                commandParameters[4].Value = model.PUserName;
                commandParameters[5].Value = model.PUserID1;
                commandParameters[6].Value = model.PUserKey1;
                commandParameters[7].Value = model.PUserID2;
                commandParameters[8].Value = model.PUserKey2;
                commandParameters[9].Value = model.PostBankUrl;
                commandParameters[10].Value = model.Desc;
                commandParameters[11].Value = model.Sort;
                commandParameters[12].Value = model.JumpUrl;
                commandParameters[13].Value = model.ExtParm1;
                commandParameters[14].Value = model.ExtParm2;
                commandParameters[15].Value = model.ExtParm3;
                commandParameters[16].Value = model.ExtParm4;
                commandParameters[17].Value = model.distributionUrl;
                int num = DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_user_supplier_update", commandParameters);
                int ID = (int)commandParameters[0].Value;

                if (model.ChannelTypeList.Count > 0)
                {
                    SqlParameter[] commandParameters2 = new SqlParameter[] { 
                        new SqlParameter("@id", SqlDbType.Int, 10), 
                        new SqlParameter("@channeltypeId", SqlDbType.Int, 10)
                    };
                    foreach (string channelType in model.ChannelTypeList)
                    {
                        commandParameters2[0].Value = ID;
                        commandParameters2[1].Value = Convert.ToInt32(channelType);

                        DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_user_supplier_channeltype_add", commandParameters2);
                    }
                }

                return ID;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        /// <summary>
        /// 删除服务商通道设置。
        /// </summary>
        /// <param name="p_id"></param>
        /// <returns></returns>
        public static Int32 DeleteUserSupplierInfo(int p_id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@id", SqlDbType.Int, 10), 
                 };
                commandParameters[0].Value = p_id;
                int num = DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_user_supplier_delete", commandParameters);
                return 1;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }
        #endregion
    }
}

