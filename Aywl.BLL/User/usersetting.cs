namespace OriginalStudio.BLL.Settled
{
    using DBAccess;
    using OriginalStudio.Model.User;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class usersetting
    {
        public usersettingInfo GetModel(int userid)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userid", SqlDbType.Int, 10) };
            commandParameters[0].Value = userid;
            usersettingInfo info = new usersettingInfo();
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_usersetting_GetModel", commandParameters);
            if (set.Tables[0].Rows.Count > 0)
            {
                if ((set.Tables[0].Rows[0]["userid"] != null) && (set.Tables[0].Rows[0]["userid"].ToString() != ""))
                {
                    info.userid = int.Parse(set.Tables[0].Rows[0]["userid"].ToString());
                }
                if ((set.Tables[0].Rows[0]["defaultpay"] != null) && (set.Tables[0].Rows[0]["defaultpay"].ToString() != ""))
                {
                    info.defaultpay = int.Parse(set.Tables[0].Rows[0]["defaultpay"].ToString());
                }
                if ((set.Tables[0].Rows[0]["special"] != null) && (set.Tables[0].Rows[0]["special"].ToString() != ""))
                {
                    info.special = int.Parse(set.Tables[0].Rows[0]["special"].ToString());
                }
                if ((set.Tables[0].Rows[0]["istransfer"] != null) && (set.Tables[0].Rows[0]["istransfer"].ToString() != ""))
                {
                    info.istransfer = int.Parse(set.Tables[0].Rows[0]["istransfer"].ToString());
                }
                if ((set.Tables[0].Rows[0]["isRequireAgentDistAudit"] != null) && (set.Tables[0].Rows[0]["isRequireAgentDistAudit"].ToString() != ""))
                {
                    info.isRequireAgentDistAudit = byte.Parse(set.Tables[0].Rows[0]["isRequireAgentDistAudit"].ToString());
                }
                info.payrate = set.Tables[0].Rows[0]["payrate"].ToString();
                return info;
            }
            info.userid = userid;
            return info;
        }

        public bool Insert(usersettingInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@userid", SqlDbType.Int, 10), 
                    new SqlParameter("@defaultpay", SqlDbType.Int, 10), 
                    new SqlParameter("@payrate", SqlDbType.VarChar, 1000), 
                    new SqlParameter("@special", SqlDbType.Int, 10), 
                    new SqlParameter("@istransfer", SqlDbType.TinyInt, 1), 
                    new SqlParameter("@isRequireAgentDistAudit", SqlDbType.TinyInt, 1) 
                };
                commandParameters[0].Value = model.userid;
                commandParameters[1].Value = model.defaultpay;
                commandParameters[2].Value = model.payrate;
                commandParameters[3].Value = model.special;
                commandParameters[4].Value = model.istransfer;
                commandParameters[5].Value = model.isRequireAgentDistAudit;
                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_usersetting_Insert", commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }
    }
}

