namespace OriginalStudio.BLL.Supplier
{
    using DBAccess;
    using OriginalStudio.Cache;
    using OriginalStudio.Model;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using OriginalStudio.BLL.Sys;
    using OriginalStudio.Model.Supplier;
    using OriginalStudio.Lib.Utils;

    public sealed class SysSupplierFactory
    {
        public static string CACHE_KEY = (Constant.Cache_Mark + "SUPPLIER_{0}");
        internal const string SQL_TABLE = "sys_supplier";
        internal const string SQL_TABLE_FIELD = "[ID],[SupplierCode],[SupplierName],[LogoUrl],[IsBank],[IsAlipay],[IsQQ],[IsWeiXin],[IsJD],[PUserID],[PUserKey],[PUserName],[pUserParm1],[pUserParm2],[pUserParm3],[pUserParm4],[Active],[IsDebug],[BankPostUrl],[BankNotifyUrl],[BankReturnUrl],[BankSearchUrl],[BankJumUrl],[DistributionUrl],[DistributionNotifyUrl],[DistributionSearchUrl],[SpDesc],[ListOrder],[IsDistribution]";

        #region 缓存操作

        private static void ClearCache(int code)
        {
            string objId = string.Format(CACHE_KEY, code);
            WebCache.GetCacheService().RemoveObject(objId);
        }

        #endregion

        #region 获取对象

        public static SysSupplierInfo GetCacheSupplierModelByCode(int code)
        {
            SysSupplierInfo o = new SysSupplierInfo();
            string objId = string.Format(CACHE_KEY, code);

            //不要从缓存取，防止数据库更新，缓存没更新
            o = GetSupplierModelByCode(code);

            //o = (SysSupplierInfo)WebCache.GetCacheService().RetrieveObject(objId);
            //if (o == null)
            //{
            //    WebCache.GetCacheService().AddObject(objId, o);
            //}
            return o;
        }

        public static SysSupplierInfo GetSupplierModelById(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] 
                {
                    new SqlParameter("@id", SqlDbType.Int, 10),
                    new SqlParameter("@suppliercode", SqlDbType.Int, 10)
                };
                commandParameters[0].Value = id;
                commandParameters[1].Value = DBNull.Value;
                return GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_sys_supplier_GetModelByIdCode", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static SysSupplierInfo GetSupplierModelByCode(int code)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] 
                {
                    new SqlParameter("@id", SqlDbType.Int, 10),
                    new SqlParameter("@suppliercode", SqlDbType.Int, 10)
                };
                commandParameters[0].Value = DBNull.Value;
                commandParameters[1].Value = code;

                return GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_sys_supplier_GetModelByIdCode", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static SysSupplierInfo GetModelFromDs(DataSet ds)
        {
            SysSupplierInfo modle = new SysSupplierInfo();
            if (ds.Tables[0].Rows.Count == 0) return modle;

            DataRow dr = ds.Tables[0].Rows[0];
            modle.ID = Convert.ToInt32(dr["id"]);
            modle.SupplierCode = Convert.ToInt32(dr["suppliercode"]);
            modle.SupplierName = Convert.ToString(dr["suppliername"]);
            modle.LogoUrl = Convert.ToString(dr["logourl"]);
            modle.IsBank = Utils.StrToInt(dr["isbank"].ToString(), 0) == 1;
            modle.IsAlipay = Utils.StrToInt(dr["isalipay"].ToString(), 0) == 1;
            modle.IsQQ = Utils.StrToInt(dr["isqq"].ToString(), 0) == 1;
            modle.IsWeiXin = Utils.StrToInt(dr["isweixin"].ToString(), 0) == 1;
            modle.IsJD = Utils.StrToInt(dr["isjd"].ToString(), 0) == 1;
            modle.IsDistribution = Utils.StrToInt(dr["isdistribution"].ToString(), 0) == 1;
            modle.PUserID = Convert.ToString(dr["puserid"]);
            modle.PUserKey = Convert.ToString(dr["puserkey"]);
            modle.PUserName = Convert.ToString(dr["pusername"]);
            modle.PUserParm1 = Convert.ToString(dr["puserparm1"]);
            modle.PUserParm2 = Convert.ToString(dr["puserparm2"]);
            modle.PUserParm3 = Convert.ToString(dr["puserparm3"]);
            modle.PUserParm4 = Convert.ToString(dr["puserparm4"]);
            modle.Active = Utils.StrToInt(dr["active"].ToString(), 1) == 1;
            modle.IsDebug = Utils.StrToInt(dr["isdebug"].ToString(), 0) == 1;
            modle.BankPostUrl = Convert.ToString(dr["bankposturl"]);
            modle.BankNotifyUrl = Convert.ToString(dr["banknotifyurl"]);
            modle.BankReturnUrl = Convert.ToString(dr["bankreturnurl"]);
            modle.BankSearchUrl = Convert.ToString(dr["banksearchurl"]);
            modle.BankJumUrl = Convert.ToString(dr["bankjumurl"]);
            modle.DistributionUrl = Convert.ToString(dr["distributionurl"]);
            modle.DistributionNotifyUrl = Convert.ToString(dr["distributionnotifyurl"]);
            modle.DistributionSearchUrl = Convert.ToString(dr["distributionsearchurl"]);
            modle.SpDesc = Convert.ToString(dr["spdesc"]);
            modle.ListOrder = Utils.StrToInt(dr["listorder"].ToString(), 0);

            return modle;
            //SysSupplierInfo info = new SysSupplierInfo();
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    if (ds.Tables[0].Rows[0]["id"].ToString() != "")
            //    {
            //        info.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
            //    }
            //    if (ds.Tables[0].Rows[0]["code"].ToString() != "")
            //    {
            //        info.code = new int?(int.Parse(ds.Tables[0].Rows[0]["code"].ToString()));
            //    }
            //    info.name = ds.Tables[0].Rows[0]["name"].ToString();
            //    info.name1 = ds.Tables[0].Rows[0]["name1"].ToString();
            //    info.logourl = ds.Tables[0].Rows[0]["logourl"].ToString();
            //    info.pcardbakurl = ds.Tables[0].Rows[0]["pcardbakurl"].ToString();
            //    if (ds.Tables[0].Rows[0]["isbank"].ToString() != "")
            //    {
            //        if ((ds.Tables[0].Rows[0]["isbank"].ToString() == "1") || (ds.Tables[0].Rows[0]["isbank"].ToString().ToLower() == "true"))
            //        {
            //            info.isbank = true;
            //        }
            //        else
            //        {
            //            info.isbank = false;
            //        }
            //    }
            //    if (ds.Tables[0].Rows[0]["iscard"].ToString() != "")
            //    {
            //        if ((ds.Tables[0].Rows[0]["iscard"].ToString() == "1") || (ds.Tables[0].Rows[0]["iscard"].ToString().ToLower() == "true"))
            //        {
            //            info.iscard = true;
            //        }
            //        else
            //        {
            //            info.iscard = false;
            //        }
            //    }
            //    if (ds.Tables[0].Rows[0]["issms"].ToString() != "")
            //    {
            //        if ((ds.Tables[0].Rows[0]["issms"].ToString() == "1") || (ds.Tables[0].Rows[0]["issms"].ToString().ToLower() == "true"))
            //        {
            //            info.issms = true;
            //        }
            //        else
            //        {
            //            info.issms = false;
            //        }
            //    }
            //    if (ds.Tables[0].Rows[0]["issx"].ToString() != "")
            //    {
            //        if ((ds.Tables[0].Rows[0]["issx"].ToString() == "1") || (ds.Tables[0].Rows[0]["issx"].ToString().ToLower() == "true"))
            //        {
            //            info.issx = true;
            //        }
            //        else
            //        {
            //            info.issx = false;
            //        }
            //    }
            //    if (ds.Tables[0].Rows[0]["isdistribution"].ToString() != "")
            //    {
            //        if ((ds.Tables[0].Rows[0]["isdistribution"].ToString() == "1") || (ds.Tables[0].Rows[0]["isdistribution"].ToString().ToLower() == "true"))
            //        {
            //            info.isdistribution = true;
            //        }
            //        else
            //        {
            //            info.isdistribution = false;
            //        }
            //    }
            //    if (ds.Tables[0].Rows[0]["iswap"].ToString() != "")
            //    {
            //        if ((ds.Tables[0].Rows[0]["iswap"].ToString() == "1") || (ds.Tables[0].Rows[0]["iswap"].ToString().ToLower() == "true"))
            //        {
            //            info.iswap = true;
            //        }
            //        else
            //        {
            //            info.iswap = false;
            //        }
            //    }
            //    if (ds.Tables[0].Rows[0]["isali"].ToString() != "")
            //    {
            //        if ((ds.Tables[0].Rows[0]["isali"].ToString() == "1") || (ds.Tables[0].Rows[0]["isali"].ToString().ToLower() == "true"))
            //        {
            //            info.isali = true;
            //        }
            //        else
            //        {
            //            info.isali = false;
            //        }
            //    }
            //    if (ds.Tables[0].Rows[0]["iswx"].ToString() != "")
            //    {
            //        if ((ds.Tables[0].Rows[0]["iswx"].ToString() == "1") || (ds.Tables[0].Rows[0]["iswx"].ToString().ToLower() == "true"))
            //        {
            //            info.iswx = true;
            //        }
            //        else
            //        {
            //            info.iswx = false;
            //        }
            //    }
            //    info.puserid = ds.Tables[0].Rows[0]["puserid"].ToString();
            //    info.puserkey = ds.Tables[0].Rows[0]["puserkey"].ToString();
            //    info.pusername = ds.Tables[0].Rows[0]["pusername"].ToString();
            //    info.puserid1 = ds.Tables[0].Rows[0]["puserid1"].ToString();
            //    info.puserkey1 = ds.Tables[0].Rows[0]["puserkey1"].ToString();
            //    info.puserid2 = ds.Tables[0].Rows[0]["puserid2"].ToString();
            //    info.puserkey2 = ds.Tables[0].Rows[0]["puserkey2"].ToString();
            //    info.puserid3 = ds.Tables[0].Rows[0]["puserid3"].ToString();
            //    info.puserkey3 = ds.Tables[0].Rows[0]["puserkey3"].ToString();
            //    info.puserid4 = ds.Tables[0].Rows[0]["puserid4"].ToString();
            //    info.puserkey4 = ds.Tables[0].Rows[0]["puserkey4"].ToString();
            //    info.puserid5 = ds.Tables[0].Rows[0]["puserid5"].ToString();
            //    info.puserkey5 = ds.Tables[0].Rows[0]["puserkey5"].ToString();
            //    info.purl = ds.Tables[0].Rows[0]["purl"].ToString();
            //    info.pbakurl = ds.Tables[0].Rows[0]["pbakurl"].ToString();
            //    info.postBankUrl = ds.Tables[0].Rows[0]["postBankUrl"].ToString();
            //    info.jumpUrl = ds.Tables[0].Rows[0]["jumpUrl"].ToString();
            //    info.postCardUrl = ds.Tables[0].Rows[0]["postCardUrl"].ToString();
            //    info.postSMSUrl = ds.Tables[0].Rows[0]["postSMSUrl"].ToString();
            //    info.distributionUrl = ds.Tables[0].Rows[0]["distributionUrl"].ToString();
            //    info.queryCardUrl = ds.Tables[0].Rows[0]["queryCardUrl"].ToString();
            //    info.desc = ds.Tables[0].Rows[0]["desc"].ToString();
            //    if (ds.Tables[0].Rows[0]["sort"].ToString() != "")
            //    {
            //        info.sort = new int?(int.Parse(ds.Tables[0].Rows[0]["sort"].ToString()));
            //    }
            //    if (ds.Tables[0].Rows[0]["release"].ToString() != "")
            //    {
            //        if ((ds.Tables[0].Rows[0]["release"].ToString() == "1") || (ds.Tables[0].Rows[0]["release"].ToString().ToLower() == "true"))
            //        {
            //            info.release = true;
            //        }
            //        else
            //        {
            //            info.release = false;
            //        }
            //    }
            //    if (ds.Tables[0].Rows[0]["issys"].ToString() != "")
            //    {
            //        if ((ds.Tables[0].Rows[0]["issys"].ToString() == "1") || (ds.Tables[0].Rows[0]["issys"].ToString().ToLower() == "true"))
            //        {
            //            info.issys = true;
            //            return info;
            //        }
            //        info.issys = false;
            //    }
            //    return info;
            //}
            //return null;
        }

        #endregion

        #region 获取列表

        public static DataSet GetList()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select " + SQL_TABLE_FIELD);
            builder.Append(" FROM " + SQL_TABLE);
            builder.Append(" Order by ListOrder ");
            return DataBase.ExecuteDataset(CommandType.Text, builder.ToString());
        }

        public static DataSet GetList(string where)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select " + SQL_TABLE_FIELD);
            builder.Append(" FROM " + SQL_TABLE);
            if (!string.IsNullOrEmpty(where))
            {
                builder.AppendFormat(" where {0}", where);
            }
            builder.Append(" Order by ListOrder ");
            return DataBase.ExecuteDataset(CommandType.Text, builder.ToString());
        }

        #endregion

        #region 增删改
        
        public static int Add(SysSupplierInfo model)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@id",SqlDbType.Int),
                    new SqlParameter("@suppliercode",SqlDbType.Int),
                    new SqlParameter("@suppliername",SqlDbType.VarChar,50),
                    new SqlParameter("@logourl",SqlDbType.VarChar,100),
                    new SqlParameter("@isbank",SqlDbType.Int),
                    new SqlParameter("@isalipay",SqlDbType.Int),
                    new SqlParameter("@isqq",SqlDbType.Int),
                    new SqlParameter("@isweixin",SqlDbType.Int),
                    new SqlParameter("@isjd",SqlDbType.Int),
                    new SqlParameter("@puserid",SqlDbType.VarChar,200),
                    new SqlParameter("@puserkey",SqlDbType.VarChar,200),
                    new SqlParameter("@pusername",SqlDbType.VarChar,200),
                    new SqlParameter("@puserparm1",SqlDbType.VarChar,200),
                    new SqlParameter("@puserparm2",SqlDbType.VarChar,200),
                    new SqlParameter("@puserparm3",SqlDbType.VarChar,200),
                    new SqlParameter("@puserparm4",SqlDbType.VarChar,200),
                    new SqlParameter("@isdebug",SqlDbType.Int),
                    new SqlParameter("@bankposturl",SqlDbType.VarChar,2000),
                    new SqlParameter("@banknotifyurl",SqlDbType.VarChar,2000),
                    new SqlParameter("@bankreturnurl",SqlDbType.VarChar,2000),
                    new SqlParameter("@banksearchurl",SqlDbType.VarChar,2000),
                    new SqlParameter("@bankjumurl",SqlDbType.VarChar,2000),
                    new SqlParameter("@distributionurl",SqlDbType.VarChar,2000),
                    new SqlParameter("@distributionnotifyurl",SqlDbType.VarChar,2000),
                    new SqlParameter("@distributionsearchurl",SqlDbType.VarChar,2000),
                    new SqlParameter("@spdesc",SqlDbType.NVarChar,4000),
                    new SqlParameter("@listorder",SqlDbType.Int),
                    new SqlParameter("@isdistribution",SqlDbType.Int),
                    new SqlParameter("@active",SqlDbType.Int)
                };
                parameters[0].Direction = ParameterDirection.InputOutput;
                parameters[1].Value = model.SupplierCode;
                parameters[2].Value = model.SupplierName;
                parameters[3].Value = model.LogoUrl;
                parameters[4].Value = model.IsBank;
                parameters[5].Value = model.IsAlipay;
                parameters[6].Value = model.IsQQ;
                parameters[7].Value = model.IsWeiXin;
                parameters[8].Value = model.IsJD;
                parameters[9].Value = model.PUserID;
                parameters[10].Value = model.PUserKey;
                parameters[11].Value = model.PUserName;
                parameters[12].Value = model.PUserParm1;
                parameters[13].Value = model.PUserParm2;
                parameters[14].Value = model.PUserParm3;
                parameters[15].Value = model.PUserParm4;
                parameters[16].Value = model.IsDebug;
                parameters[17].Value = model.BankPostUrl;
                parameters[18].Value = model.BankNotifyUrl;
                parameters[19].Value = model.BankReturnUrl;
                parameters[20].Value = model.BankSearchUrl;
                parameters[21].Value = model.BankJumUrl;
                parameters[22].Value = model.DistributionUrl;
                parameters[23].Value = model.DistributionNotifyUrl;
                parameters[24].Value = model.DistributionSearchUrl;
                parameters[25].Value = model.SpDesc;
                parameters[26].Value = model.ListOrder;
                parameters[27].Value = model.IsDistribution;
                parameters[28].Value = model.Active;

                int num = DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_sys_supplier_add", parameters);
                return (int)parameters[0].Value;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        public static bool Update(SysSupplierInfo model)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@id",SqlDbType.Int),
                    new SqlParameter("@suppliercode",SqlDbType.Int),
                    new SqlParameter("@suppliername",SqlDbType.VarChar,50),
                    new SqlParameter("@logourl",SqlDbType.VarChar,100),
                    new SqlParameter("@isbank",SqlDbType.Int),
                    new SqlParameter("@isalipay",SqlDbType.Int),
                    new SqlParameter("@isqq",SqlDbType.Int),
                    new SqlParameter("@isweixin",SqlDbType.Int),
                    new SqlParameter("@isjd",SqlDbType.Int),
                    new SqlParameter("@puserid",SqlDbType.VarChar,200),
                    new SqlParameter("@puserkey",SqlDbType.VarChar,200),
                    new SqlParameter("@pusername",SqlDbType.VarChar,200),
                    new SqlParameter("@puserparm1",SqlDbType.VarChar,200),
                    new SqlParameter("@puserparm2",SqlDbType.VarChar,200),
                    new SqlParameter("@puserparm3",SqlDbType.VarChar,200),
                    new SqlParameter("@puserparm4",SqlDbType.VarChar,200),
                    new SqlParameter("@isdebug",SqlDbType.Int),
                    new SqlParameter("@bankposturl",SqlDbType.VarChar,2000),
                    new SqlParameter("@banknotifyurl",SqlDbType.VarChar,2000),
                    new SqlParameter("@bankreturnurl",SqlDbType.VarChar,2000),
                    new SqlParameter("@banksearchurl",SqlDbType.VarChar,2000),
                    new SqlParameter("@bankjumurl",SqlDbType.VarChar,2000),
                    new SqlParameter("@distributionurl",SqlDbType.VarChar,2000),
                    new SqlParameter("@distributionnotifyurl",SqlDbType.VarChar,2000),
                    new SqlParameter("@distributionsearchurl",SqlDbType.VarChar,2000),
                    new SqlParameter("@spdesc",SqlDbType.NVarChar,4000),
                    new SqlParameter("@listorder",SqlDbType.Int),
                    new SqlParameter("@isdistribution",SqlDbType.Int),
                    new SqlParameter("@active",SqlDbType.Int)
                };
                parameters[0].Value = model.ID ;
                parameters[1].Value = model.SupplierCode;
                parameters[2].Value = model.SupplierName;
                parameters[3].Value = model.LogoUrl;
                parameters[4].Value = model.IsBank;
                parameters[5].Value = model.IsAlipay;
                parameters[6].Value = model.IsQQ;
                parameters[7].Value = model.IsWeiXin;
                parameters[8].Value = model.IsJD;
                parameters[9].Value = model.PUserID;
                parameters[10].Value = model.PUserKey;
                parameters[11].Value = model.PUserName;
                parameters[12].Value = model.PUserParm1;
                parameters[13].Value = model.PUserParm2;
                parameters[14].Value = model.PUserParm3;
                parameters[15].Value = model.PUserParm4;
                parameters[16].Value = model.IsDebug;
                parameters[17].Value = model.BankPostUrl;
                parameters[18].Value = model.BankNotifyUrl;
                parameters[19].Value = model.BankReturnUrl;
                parameters[20].Value = model.BankSearchUrl;
                parameters[21].Value = model.BankJumUrl;
                parameters[22].Value = model.DistributionUrl;
                parameters[23].Value = model.DistributionNotifyUrl;
                parameters[24].Value = model.DistributionSearchUrl;
                parameters[25].Value = model.SpDesc;
                parameters[26].Value = model.ListOrder;
                parameters[27].Value = model.IsDistribution;
                parameters[28].Value = model.Active;

                if (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_sys_supplier_update", parameters) > 0)
                    return true;
                return false;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        #endregion

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

