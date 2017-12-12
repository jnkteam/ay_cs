namespace OriginalStudio.BLL.User
{
    using DBAccess;
    using OriginalStudio.Model.Settled;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class WithdrawScheme_del
    {
        public static int Add(TocashSchemeInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@id", SqlDbType.Int, 10), 
                    new SqlParameter("@schemename", SqlDbType.NVarChar, 50), 
                    new SqlParameter("@minamtlimitofeach", SqlDbType.Decimal, 9), 
                    new SqlParameter("@maxamtlimitofeach", SqlDbType.Decimal, 9), 
                    new SqlParameter("@dailymaxtimes", SqlDbType.Int, 10), 
                    new SqlParameter("@dailymaxamt", SqlDbType.Decimal, 9), 
                    new SqlParameter("@chargerate", SqlDbType.Decimal, 9), 
                    new SqlParameter("@chargeleastofeach", SqlDbType.Decimal, 9), 
                    new SqlParameter("@chargemostofeach", SqlDbType.Decimal, 9), 
                    new SqlParameter("@isdefault", SqlDbType.TinyInt, 1), 
                    new SqlParameter("@tranapi", SqlDbType.Int, 10), 
                    new SqlParameter("@bankdetentiondays", SqlDbType.Int, 10), 
                    new SqlParameter("@otherdetentiondays", SqlDbType.Int, 10), 
                    new SqlParameter("@carddetentiondays", SqlDbType.Int, 10), 
                    new SqlParameter("@tranRequiredAudit", SqlDbType.TinyInt, 1), 
                    new SqlParameter("@type", SqlDbType.TinyInt, 1), 
                    new SqlParameter("@alipaydetentiondays", SqlDbType.Int, 10), 
                    new SqlParameter("@weixindetentiondays", SqlDbType.Int, 10)
                 };
                commandParameters[0].Direction = ParameterDirection.Output;
                commandParameters[1].Value = model.schemename;
                commandParameters[2].Value = model.minamtlimitofeach;
                commandParameters[3].Value = model.maxamtlimitofeach;
                commandParameters[4].Value = model.dailymaxtimes;
                commandParameters[5].Value = model.dailymaxamt;
                commandParameters[6].Value = model.chargerate;
                commandParameters[7].Value = model.chargeleastofeach;
                commandParameters[8].Value = model.chargemostofeach;
                commandParameters[9].Value = model.isdefault;
                commandParameters[10].Value = model.vaiInterface;
                commandParameters[11].Value = model.bankdetentiondays;
                commandParameters[12].Value = model.otherdetentiondays;
                commandParameters[13].Value = model.carddetentiondays;
                commandParameters[14].Value = model.tranRequiredAudit;
                commandParameters[15].Value = model.type;
                commandParameters[0x10].Value = model.alipaydetentiondays;
                commandParameters[0x11].Value = model.weixindetentiondays;
                if (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_tocashscheme_Add", commandParameters) > 0)
                {
                    return (int) commandParameters[0].Value;
                }
                return 0;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        public static bool Delete(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = id;
                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_tocashscheme_Delete", commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static DataSet GetList(string strWhere)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("select id,tranRequiredAudit,schemename,minamtlimitofeach,maxamtlimitofeach,dailymaxtimes,dailymaxamt,chargerate,chargeleastofeach,chargemostofeach,isdefault,bankdetentiondays,otherdetentiondays,carddetentiondays,alipaydetentiondays,weixindetentiondays ");
                builder.Append(" FROM tocashscheme ");
                if (strWhere.Trim() != "")
                {
                    builder.Append(" where " + strWhere);
                }
                return DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), null);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static TocashSchemeInfo GetModel(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = id;
                return GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_tocashscheme_GetModel", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static TocashSchemeInfo GetModelByUser(int type, int userId)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@type", SqlDbType.TinyInt, 1), 
                    new SqlParameter("@userId", SqlDbType.Int, 10) };
                commandParameters[0].Value = type;
                commandParameters[1].Value = userId;
                return GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_tocashscheme_GetModelByUser", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static TocashSchemeInfo GetModelFromDs(DataSet ds)
        {
            TocashSchemeInfo info = new TocashSchemeInfo();
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                if (row["id"].ToString() != "")
                {
                    info.id = int.Parse(row["id"].ToString());
                }
                info.schemename = row["schemename"].ToString();
                if (row["minamtlimitofeach"].ToString() != "")
                {
                    info.minamtlimitofeach = decimal.Parse(row["minamtlimitofeach"].ToString());
                }
                if (row["maxamtlimitofeach"].ToString() != "")
                {
                    info.maxamtlimitofeach = decimal.Parse(row["maxamtlimitofeach"].ToString());
                }
                if (row["dailymaxtimes"].ToString() != "")
                {
                    info.dailymaxtimes = int.Parse(row["dailymaxtimes"].ToString());
                }
                if (row["dailymaxamt"].ToString() != "")
                {
                    info.dailymaxamt = decimal.Parse(row["dailymaxamt"].ToString());
                }
                if (row["chargerate"].ToString() != "")
                {
                    info.chargerate = decimal.Parse(row["chargerate"].ToString());
                }
                if (row["chargeleastofeach"].ToString() != "")
                {
                    info.chargeleastofeach = decimal.Parse(row["chargeleastofeach"].ToString());
                }
                if (row["chargemostofeach"].ToString() != "")
                {
                    info.chargemostofeach = decimal.Parse(row["chargemostofeach"].ToString());
                }
                if (row["isdefault"].ToString() != "")
                {
                    info.isdefault = int.Parse(row["isdefault"].ToString());
                }
                if (row["tranapi"].ToString() != "")
                {
                    info.vaiInterface = int.Parse(row["tranapi"].ToString());
                }
                if ((row["bankdetentiondays"] != null) && (row["bankdetentiondays"].ToString() != ""))
                {
                    info.bankdetentiondays = int.Parse(row["bankdetentiondays"].ToString());
                }
                if ((row["otherdetentiondays"] != null) && (row["otherdetentiondays"].ToString() != ""))
                {
                    info.otherdetentiondays = int.Parse(row["otherdetentiondays"].ToString());
                }
                if ((row["carddetentiondays"] != null) && (row["carddetentiondays"].ToString() != ""))
                {
                    info.carddetentiondays = int.Parse(row["carddetentiondays"].ToString());
                }
                if ((row["carddetentiondays"] != null) && (row["carddetentiondays"].ToString() != ""))
                {
                    info.qqdetentiondays = int.Parse(row["carddetentiondays"].ToString());
                }
                if ((row["tranRequiredAudit"] != null) && (row["tranRequiredAudit"].ToString() != ""))
                {
                    info.tranRequiredAudit = byte.Parse(row["tranRequiredAudit"].ToString());
                }
                if (row["type"].ToString() != "")
                {
                    info.type = int.Parse(row["type"].ToString());
                }
                if ((row["alipaydetentiondays"] != null) && (row["alipaydetentiondays"].ToString() != ""))
                {
                    info.alipaydetentiondays = int.Parse(row["alipaydetentiondays"].ToString());
                }
                if ((row["weixindetentiondays"] != null) && (row["weixindetentiondays"].ToString() != ""))
                {
                    info.weixindetentiondays = int.Parse(row["weixindetentiondays"].ToString());
                }
                return info;
            }
            return new TocashSchemeInfo();
        }

        public static bool Update(TocashSchemeInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@schemename", SqlDbType.NVarChar, 50), new SqlParameter("@minamtlimitofeach", SqlDbType.Decimal, 9), new SqlParameter("@maxamtlimitofeach", SqlDbType.Decimal, 9), new SqlParameter("@dailymaxtimes", SqlDbType.Int, 10), new SqlParameter("@dailymaxamt", SqlDbType.Decimal, 9), new SqlParameter("@chargerate", SqlDbType.Decimal, 9), new SqlParameter("@chargeleastofeach", SqlDbType.Decimal, 9), new SqlParameter("@chargemostofeach", SqlDbType.Decimal, 9), new SqlParameter("@isdefault", SqlDbType.TinyInt, 1), new SqlParameter("@tranapi", SqlDbType.Int, 10), new SqlParameter("@bankdetentiondays", SqlDbType.Int, 10), new SqlParameter("@otherdetentiondays", SqlDbType.Int, 10), new SqlParameter("@carddetentiondays", SqlDbType.Int, 10), new SqlParameter("@tranRequiredAudit", SqlDbType.TinyInt, 1), new SqlParameter("@type", SqlDbType.TinyInt, 1), 
                    new SqlParameter("@alipaydetentiondays", SqlDbType.Int, 10), new SqlParameter("@weixindetentiondays", SqlDbType.Int, 10)
                 };
                commandParameters[0].Value = model.id;
                commandParameters[1].Value = model.schemename;
                commandParameters[2].Value = model.minamtlimitofeach;
                commandParameters[3].Value = model.maxamtlimitofeach;
                commandParameters[4].Value = model.dailymaxtimes;
                commandParameters[5].Value = model.dailymaxamt;
                commandParameters[6].Value = model.chargerate;
                commandParameters[7].Value = model.chargeleastofeach;
                commandParameters[8].Value = model.chargemostofeach;
                commandParameters[9].Value = model.isdefault;
                commandParameters[10].Value = model.vaiInterface;
                commandParameters[11].Value = model.bankdetentiondays;
                commandParameters[12].Value = model.otherdetentiondays;
                commandParameters[13].Value = model.carddetentiondays;
                commandParameters[14].Value = model.tranRequiredAudit;
                commandParameters[15].Value = model.type;
                commandParameters[0x10].Value = model.alipaydetentiondays;
                commandParameters[0x11].Value = model.weixindetentiondays;
                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_tocashscheme_Update", commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }
    }
}

