﻿namespace KuaiCard.BLL.Stat
{
    using DBAccess;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    public class OrderReport
    {
        public static DataTable ReportByUser(DateTime beginTime, DateTime endTime, int uid)
        {
            List<SqlParameter> list = new List<SqlParameter>();
            if (uid > 0)
            {
                list.Add(DataBase.MakeInParam("@userid", SqlDbType.Int, 10, uid));
            }
            list.Add(DataBase.MakeInParam("@beginTime", SqlDbType.DateTime, 8, beginTime));
            list.Add(DataBase.MakeInParam("@endTime", SqlDbType.DateTime, 8, endTime));
            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "Proc_Stat_ReportByUser", list.ToArray()).Tables[0];
        }
    }
}

