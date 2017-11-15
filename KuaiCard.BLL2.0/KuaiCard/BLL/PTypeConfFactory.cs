namespace KuaiCard.BLL
{
    using DBAccess;
    using KuaiCard.Model;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class PTypeConfFactory
    {
        public int Add(PTypeConf model)
        {
            SqlParameter[] prams = new SqlParameter[] { new SqlParameter("@ID", SqlDbType.Int, 10), new SqlParameter("@GoodType", SqlDbType.TinyInt, 1), new SqlParameter("@GM_ID", SqlDbType.Int, 10), new SqlParameter("@PayType", SqlDbType.Int, 10), new SqlParameter("@IsUse", SqlDbType.Bit, 1) };
            prams[0].Direction = ParameterDirection.Output;
            prams[1].Value = model.GoodType;
            prams[2].Value = model.GM_ID;
            prams[3].Value = model.PayType;
            prams[4].Value = model.IsUse;
            DataBase.RunProc("PTypeConf_ADD", prams);
            return (int) prams[0].Value;
        }

        public bool Delete(int ID)
        {
            SqlParameter[] prams = new SqlParameter[] { new SqlParameter("@ID", SqlDbType.Int, 10) };
            prams[0].Value = ID;
            return (DataBase.RunProc("PTypeConf_Delete", prams) > 0);
        }

        public bool DeleteList(string IDlist)
        {
            string str = string.Empty;
            foreach (string str2 in IDlist.Split(new char[] { ',' }))
            {
                int result = 0;
                if (int.TryParse(str2, out result))
                {
                    str = str + str2 + ",";
                }
            }
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Substring(0, str.Length - 1);
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("delete from PTypeConf ");
            builder.Append(" where ID in (" + str + ")  ");
            return (DataBase.ExecuteNonQuery(CommandType.Text, builder.ToString()) > 0);
        }

        public bool Exists(int ID)
        {
            SqlParameter[] prams = new SqlParameter[] { new SqlParameter("@ID", SqlDbType.Int, 10) };
            prams[0].Value = ID;
            return (DataBase.RunProc("PTypeConf_Exists", prams) == 1);
        }

        public DataSet GetBankList(int userid)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select ID,GoodType,GM_ID,PayType,IsUse,isDisable ");
            builder.Append(" FROM PTypeConf where GM_ID=@GM_ID AND (PayType=100 or PayType=101 or PayType =102)");
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(DataBase.MakeInParam("@GM_ID", SqlDbType.Int, 10, userid));
            return DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), list.ToArray());
        }

        public DataSet GetCardCloseList(int userid)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select ID,GoodType,GM_ID,PayType,IsUse,isDisable ");
            builder.Append(" FROM PTypeConf where GM_ID=@GM_ID and GoodType=2 and PayType <> 300 and (IsUse=0 or isDisable = 1)");
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(DataBase.MakeInParam("@GM_ID", SqlDbType.Int, 10, userid));
            return DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), list.ToArray());
        }

        public DataSet GetGMCloseList(int userid)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select ID,GoodType,GM_ID,PayType,IsUse,isDisable ");
            builder.Append(" FROM PTypeConf where GM_ID=@GM_ID and GoodType=1 and PayType <> 300 and (IsUse=0 or isDisable = 1)");
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(DataBase.MakeInParam("@GM_ID", SqlDbType.Int, 10, userid));
            return DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), list.ToArray());
        }

        public DataSet GetList(int userid, int type)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select ID,GoodType,GM_ID,PayType,IsUse,isDisable ");
            builder.Append(" FROM PTypeConf where GM_ID=@GM_ID AND GoodType=@type");
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(DataBase.MakeInParam("@GM_ID", SqlDbType.Int, 10, userid));
            list.Add(DataBase.MakeInParam("@type", SqlDbType.Int, 10, type));
            return DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), list.ToArray());
        }

        public PTypeConf GetModel(int ID)
        {
            SqlParameter[] prams = new SqlParameter[] { new SqlParameter("@ID", SqlDbType.Int, 10) };
            prams[0].Value = ID;
            PTypeConf conf = new PTypeConf();
            DataSet ds = null;
            DataBase.RunProc("PTypeConf_GetModel", prams, out ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ID"].ToString() != "")
                {
                    conf.ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["GoodType"].ToString() != "")
                {
                    conf.GoodType = int.Parse(ds.Tables[0].Rows[0]["GoodType"].ToString());
                }
                if (ds.Tables[0].Rows[0]["GM_ID"].ToString() != "")
                {
                    conf.GM_ID = int.Parse(ds.Tables[0].Rows[0]["GM_ID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["PayType"].ToString() != "")
                {
                    conf.PayType = int.Parse(ds.Tables[0].Rows[0]["PayType"].ToString());
                }
                if (ds.Tables[0].Rows[0]["IsUse"].ToString() != "")
                {
                    conf.IsUse = Convert.ToInt32(ds.Tables[0].Rows[0]["IsUse"].ToString());
                }
                return conf;
            }
            return null;
        }

        public PTypeConf GetModelByUser(int UserID, int gdtype)
        {
            SqlParameter[] prams = new SqlParameter[] { new SqlParameter("@userID", SqlDbType.Int, 10), new SqlParameter("@gdType", SqlDbType.Int, 10) };
            prams[0].Value = UserID;
            prams[1].Value = gdtype;
            PTypeConf conf = new PTypeConf();
            DataSet ds = null;
            DataBase.RunProc("PTypeConf_GetModelByUserID", prams, out ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int result = 0;
                int num2 = 0;
                int num3 = 0;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    int.TryParse(row["PayType"].ToString(), out result);
                    int.TryParse(row["IsUse"].ToString(), out num2);
                    int.TryParse(row["isDisable"].ToString(), out num3);
                    if ((num2 == 0) || (num3 == 1))
                    {
                        switch (result)
                        {
                            case 100:
                                conf.PayAlipay = false;
                                break;

                            case 0x65:
                                conf.PayTanPay = false;
                                break;

                            case 0x66:
                                conf.PayBank = false;
                                break;

                            case 0x67:
                                conf.Pay103 = false;
                                break;

                            case 0x68:
                                conf.Pay104 = false;
                                break;

                            case 0x69:
                                conf.Pay105 = false;
                                break;

                            case 0x6a:
                                conf.Pay106 = false;
                                break;

                            case 0x6b:
                                conf.Pay107 = false;
                                break;

                            case 0x6c:
                                conf.Pay108 = false;
                                break;

                            case 0x6d:
                                conf.Pay109 = false;
                                break;

                            case 110:
                                conf.Pay110 = false;
                                break;

                            case 0x6f:
                                conf.Pay111 = false;
                                break;

                            case 0x70:
                                conf.Pay112 = false;
                                break;
                        }
                    }
                }
            }
            return conf;
        }

        public bool Update(PTypeConf model)
        {
            object obj2 = null;
            SqlParameter[] prams = new SqlParameter[] { new SqlParameter("@GoodType", SqlDbType.TinyInt, 1), new SqlParameter("@GM_ID", SqlDbType.Int, 10), new SqlParameter("@PayType", SqlDbType.Int, 10), new SqlParameter("@IsUse", SqlDbType.TinyInt, 1) };
            prams[0].Value = model.GoodType;
            prams[1].Value = model.GM_ID;
            prams[2].Value = model.PayType;
            prams[3].Value = model.IsUse;
            DataBase.RunProc("PTypeConf_Update", prams, out obj2);
            return ((obj2 != null) && (obj2 != DBNull.Value));
        }
    }
}

