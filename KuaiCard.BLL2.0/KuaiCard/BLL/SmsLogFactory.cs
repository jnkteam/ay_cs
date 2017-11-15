namespace KuaiCard.BLL
{
    using DBAccess;
    using OKXR.Entity;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;
    using System.Text;

    public class SmsLogFactory
    {
        public static void Add(Smslog model)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("insert into Smslog(");
            builder.Append("status,price,message,mobile,servicenum,linkid,siteid,spid,type,addtime)");
            builder.Append(" values (");
            builder.Append("@status,@price,@message,@mobile,@servicenum,@linkid,@siteid,@spid,@type,@addtime)");
            builder.Append(";select @@IDENTITY");
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@status", SqlDbType.VarChar, 5), new SqlParameter("@price", SqlDbType.Decimal, 9), new SqlParameter("@message", SqlDbType.VarChar, 100), new SqlParameter("@mobile", SqlDbType.VarChar, 12), new SqlParameter("@servicenum", SqlDbType.VarChar, 50), new SqlParameter("@linkid", SqlDbType.VarChar, 50), new SqlParameter("@siteid", SqlDbType.VarChar, 50), new SqlParameter("@spid", SqlDbType.VarChar, 50), new SqlParameter("@type", SqlDbType.VarChar, 2), new SqlParameter("@addtime", SqlDbType.DateTime) };
            commandParameters[0].Value = model.status;
            commandParameters[1].Value = model.price;
            commandParameters[2].Value = model.message;
            commandParameters[3].Value = model.mobile;
            commandParameters[4].Value = model.servicenum;
            commandParameters[5].Value = model.linkid;
            commandParameters[6].Value = model.siteid;
            commandParameters[7].Value = model.spid;
            commandParameters[8].Value = model.type;
            commandParameters[9].Value = model.addtime;
            DataBase.ExecuteNonQuery(CommandType.Text, builder.ToString(), commandParameters);
        }

        public static void Delete(string Id)
        {
            Comm.Delete("Smslog", "Sid=@Id", new SqlParameter[] { new SqlParameter("@Id", Id) });
        }

        public static DataTable GetList(int userid, int aid, int _sid, int pageindex, DateTime stime, DateTime etime, int status, out int total, out double money)
        {
            List<SqlParameter> list = new List<SqlParameter>();
            if (userid > 0)
            {
                list.Add(DataBase.MakeInParam("@uid", SqlDbType.Int, 10, userid));
            }
            if (_sid > 0)
            {
                list.Add(DataBase.MakeInParam("@sid", SqlDbType.Int, 10, _sid));
            }
            if (aid > 0)
            {
                list.Add(DataBase.MakeInParam("@aid", SqlDbType.Int, 10, aid));
            }
            list.Add(DataBase.MakeInParam("@stime", SqlDbType.DateTime, 8, stime));
            list.Add(DataBase.MakeInParam("@etime", SqlDbType.DateTime, 8, etime));
            list.Add(DataBase.MakeInParam("@Status", SqlDbType.TinyInt, 1, status));
            list.Add(DataBase.MakeInParam("@page", SqlDbType.Int, 10, pageindex));
            list.Add(DataBase.MakeInParam("@pagesize", SqlDbType.Int, 10, 40));
            SqlParameter item = DataBase.MakeOutParam("@totalmoney", SqlDbType.Decimal, 8);
            list.Add(item);
            SqlParameter parameter2 = DataBase.MakeOutParam("@total", SqlDbType.Int, 10);
            list.Add(parameter2);
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "User_PaySMS_Getlist", list.ToArray());
            DataTable table = null;
            money = 0.0;
            total = 0;
            if (set.Tables.Count != 0)
            {
                table = set.Tables[0];
                money = double.Parse(item.Value.ToString());
                total = int.Parse(parameter2.Value.ToString());
            }
            return table;
        }

        public static Smslog GetModel(string linkid)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select  top 1 sid,status,price,message,mobile,servicenum,linkid,siteid,spid,type,addtime from Smslog ");
            builder.Append(" where linkid=@linkid ");
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@linkid", SqlDbType.VarChar, 50) };
            commandParameters[0].Value = linkid;
            Smslog smslog = new Smslog();
            DataSet set = DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), commandParameters);
            if (set.Tables[0].Rows.Count > 0)
            {
                if (set.Tables[0].Rows[0]["sid"].ToString() != "")
                {
                    smslog.sid = int.Parse(set.Tables[0].Rows[0]["sid"].ToString());
                }
                smslog.status = set.Tables[0].Rows[0]["status"].ToString();
                if (set.Tables[0].Rows[0]["price"].ToString() != "")
                {
                    smslog.price = decimal.Parse(set.Tables[0].Rows[0]["price"].ToString());
                }
                smslog.message = set.Tables[0].Rows[0]["message"].ToString();
                smslog.mobile = set.Tables[0].Rows[0]["mobile"].ToString();
                smslog.servicenum = set.Tables[0].Rows[0]["servicenum"].ToString();
                smslog.linkid = set.Tables[0].Rows[0]["linkid"].ToString();
                smslog.siteid = set.Tables[0].Rows[0]["siteid"].ToString();
                smslog.spid = set.Tables[0].Rows[0]["spid"].ToString();
                smslog.type = set.Tables[0].Rows[0]["type"].ToString();
                if (set.Tables[0].Rows[0]["addtime"].ToString() != "")
                {
                    smslog.addtime = new DateTime?(DateTime.Parse(set.Tables[0].Rows[0]["addtime"].ToString()));
                }
                return smslog;
            }
            return smslog;
        }

        public static Smslog GetSMSId(string Id)
        {
            return Comm.SelectOne<Smslog>("Smslog", "Sid=@Id", new SqlParameter[] { new SqlParameter("@Id", Id) });
        }

        public static List<Smslog> List(string table, string filed, string condition, string fldname, int asc, int pageindex, int pageMax)
        {
            return Comm.Select<Smslog>(table, filed, condition, fldname, asc, pageindex, pageMax);
        }

        public static int SmsOrderComplete(string ServerId, decimal Amount, string LinkId)
        {
            int num = -2;
            SqlParameter parameter = DataBase.MakeOutParam("@result", SqlDbType.Int, 10);
            SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@serverId", SqlDbType.Int, 10, ServerId), DataBase.MakeInParam("@Amount", SqlDbType.Money, 8, Amount), DataBase.MakeInParam("@LinkId", SqlDbType.VarChar, 20, LinkId), parameter };
            DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "User_Pay_SMS_Complete", commandParameters);
            if (parameter != null)
            {
                num = (int) parameter.Value;
            }
            if (num == 1)
            {
                return 1;
            }
            return -1;
        }

        public static void Update(Smslog model)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("update Smslog set ");
            builder.Append("status=@status,");
            builder.Append("price=@price,");
            builder.Append("message=@message,");
            builder.Append("mobile=@mobile,");
            builder.Append("servicenum=@servicenum,");
            builder.Append("linkid=@linkid,");
            builder.Append("siteid=@siteid,");
            builder.Append("spid=@spid,");
            builder.Append("type=@type,");
            builder.Append("addtime=@addtime");
            builder.Append(" where sid=@sid ");
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@sid", SqlDbType.Int, 10), new SqlParameter("@status", SqlDbType.VarChar, 5), new SqlParameter("@price", SqlDbType.Decimal, 9), new SqlParameter("@message", SqlDbType.VarChar, 100), new SqlParameter("@mobile", SqlDbType.VarChar, 12), new SqlParameter("@servicenum", SqlDbType.VarChar, 50), new SqlParameter("@linkid", SqlDbType.VarChar, 50), new SqlParameter("@siteid", SqlDbType.VarChar, 50), new SqlParameter("@spid", SqlDbType.VarChar, 50), new SqlParameter("@type", SqlDbType.VarChar, 2), new SqlParameter("@addtime", SqlDbType.DateTime) };
            commandParameters[0].Value = model.sid;
            commandParameters[1].Value = model.status;
            commandParameters[2].Value = model.price;
            commandParameters[3].Value = model.message;
            commandParameters[4].Value = model.mobile;
            commandParameters[5].Value = model.servicenum;
            commandParameters[6].Value = model.linkid;
            commandParameters[7].Value = model.siteid;
            commandParameters[8].Value = model.spid;
            commandParameters[9].Value = model.type;
            commandParameters[10].Value = model.addtime;
            DataBase.ExecuteNonQuery(CommandType.Text, builder.ToString(), commandParameters);
        }
    }
}

