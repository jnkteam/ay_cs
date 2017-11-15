namespace KuaiCard.BLL.Financial
{
    using DBAccess;
    using KuaiCard.Model.Financial;
    using KuaiCardLib.ExceptionHandling;
    using KuaiCardLib.Web;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    public class tenpay_batch_trans_head
    {
        public int Complete(string package_id, int status, string retcode, string message, string retcontext, int success, int fail, int uncertain)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@package_id", SqlDbType.VarChar, 20),
                    new SqlParameter("@status", SqlDbType.Int, 10), 
                    new SqlParameter("@completetime", SqlDbType.DateTime), 
                    new SqlParameter("@retcode", SqlDbType.VarChar, 50), 
                    new SqlParameter("@message", SqlDbType.VarChar, 200), 
                    new SqlParameter("@retcontext", SqlDbType.VarChar, 0x1f40), 
                    new SqlParameter("@success", SqlDbType.Int, 10), 
                    new SqlParameter("@fail", SqlDbType.Int, 10), 
                    new SqlParameter("@uncertain", SqlDbType.Int, 10) 
                };
                commandParameters[0].Value = package_id;
                commandParameters[1].Value = status;
                commandParameters[2].Value = DateTime.Now;
                commandParameters[3].Value = retcode;
                commandParameters[4].Value = message;
                commandParameters[5].Value = retcontext;
                commandParameters[6].Value = success;
                commandParameters[7].Value = fail;
                commandParameters[8].Value = uncertain;
                object obj2 = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_tenpay_batch_trans_head_complete", commandParameters);
                if (obj2 != DBNull.Value)
                {
                    return Convert.ToInt32(obj2);
                }
                return 0;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        public KuaiCard.Model.Financial.tenpay_batch_trans_head InitInfo(int settleid, decimal tranamt, int userid, string account, string rec_name, string op_user)
        {
            string str = new Random().Next(100, 0x3e7).ToString() + DateTime.Now.ToString("yyyyMMddHHmmss");
            KuaiCard.Model.Financial.tenpay_batch_trans_detail item = new KuaiCard.Model.Financial.tenpay_batch_trans_detail();
            item.settleid = settleid;
            item.package_id = str;
            item.hid = 0;
            item.id = 0;
            item.message = string.Empty;
            item.pay_amt = tranamt;
            item.rec_acc = account;
            item.rec_name = rec_name;
            item.remark = string.Empty;
            item.serial = 1;
            item.status = 1;
            item.succ_amt = 0;
            item.trans_id = string.Empty;
            item.userid = userid;
            item.balance = 0M;
            List<KuaiCard.Model.Financial.tenpay_batch_trans_detail> list = new List<KuaiCard.Model.Financial.tenpay_batch_trans_detail>();
            list.Add(item);
            KuaiCard.Model.Financial.tenpay_batch_trans_head _head = new KuaiCard.Model.Financial.tenpay_batch_trans_head();
            _head.client_ip = ServerVariables.TrueIP;
            _head.completetime = new DateTime?(DateTime.Now);
            _head.fail = 0;
            _head.success = 0;
            _head.uncertain = 0;
            _head.id = 0;
            _head.message = string.Empty;
            _head.op_time = DateTime.Now;
            _head.op_user = op_user;
            _head.package_id = str;
            _head.status = 1;
            _head.total_amt = tranamt;
            _head.total_num = 1;
            _head.version = "2";
            _head.items = list;
            return _head;
        }

        public int Insert(KuaiCard.Model.Financial.tenpay_batch_trans_head model)
        {
            int num3;
            using (SqlConnection connection = new SqlConnection(DataBase.ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@package_id", SqlDbType.VarChar, 20), new SqlParameter("@total_num", SqlDbType.Int, 10), new SqlParameter("@total_amt", SqlDbType.Decimal, 9), new SqlParameter("@status", SqlDbType.Int, 10), new SqlParameter("@version", SqlDbType.VarChar, 10), new SqlParameter("@client_ip", SqlDbType.VarChar, 20), new SqlParameter("@op_user", SqlDbType.VarChar, 30), new SqlParameter("@op_time", SqlDbType.DateTime), new SqlParameter("@completetime", SqlDbType.DateTime), new SqlParameter("@retcode", SqlDbType.VarChar, 50), new SqlParameter("@message", SqlDbType.VarChar, 200), new SqlParameter("@retcontext", SqlDbType.VarChar, 0x1f40), new SqlParameter("@success", SqlDbType.Int, 10), new SqlParameter("@fail", SqlDbType.Int, 10), new SqlParameter("@uncertain", SqlDbType.Int, 10) };
                    parameterArray[0].Direction = ParameterDirection.Output;
                    parameterArray[1].Value = model.package_id;
                    parameterArray[2].Value = model.total_num;
                    parameterArray[3].Value = model.total_amt;
                    parameterArray[4].Value = model.status;
                    parameterArray[5].Value = model.version;
                    parameterArray[6].Value = model.client_ip;
                    parameterArray[7].Value = model.op_user;
                    parameterArray[8].Value = model.op_time;
                    parameterArray[9].Value = model.completetime;
                    parameterArray[10].Value = model.retcode;
                    parameterArray[11].Value = model.message;
                    parameterArray[12].Value = model.retcontext;
                    parameterArray[13].Value = model.success;
                    parameterArray[14].Value = model.fail;
                    parameterArray[15].Value = model.uncertain;
                    int num2 = Convert.ToInt32(DataBase.ExecuteScalar(transaction, "proc_tenpay_batch_trans_head_add", (object[]) parameterArray));
                    if (num2 > 0)
                    {
                        foreach (KuaiCard.Model.Financial.tenpay_batch_trans_detail _detail in model.items)
                        {
                            SqlParameter[] parameterArray2 = new SqlParameter[] { 
                                new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@settleid", SqlDbType.Int, 10), new SqlParameter("@hid", SqlDbType.Int, 10), new SqlParameter("@package_id", SqlDbType.VarChar, 20), new SqlParameter("@serial", SqlDbType.Int, 10), new SqlParameter("@userid", SqlDbType.Int, 10), new SqlParameter("@balance", SqlDbType.Decimal, 9), new SqlParameter("@status", SqlDbType.TinyInt, 1), new SqlParameter("@rec_acc", SqlDbType.VarChar, 20), new SqlParameter("@rec_name", SqlDbType.VarChar, 20), new SqlParameter("@cur_type", SqlDbType.VarChar, 20), new SqlParameter("@pay_amt", SqlDbType.Decimal, 9), new SqlParameter("@succ_amt", SqlDbType.Decimal, 9), new SqlParameter("@remark", SqlDbType.VarChar, 200), new SqlParameter("@trans_id", SqlDbType.VarChar, 50), new SqlParameter("@message", SqlDbType.VarChar, 50), 
                                new SqlParameter("@completetime", SqlDbType.DateTime)
                             };
                            parameterArray2[0].Direction = ParameterDirection.Output;
                            parameterArray2[1].Value = _detail.settleid;
                            parameterArray2[2].Value = num2;
                            parameterArray2[3].Value = _detail.package_id;
                            parameterArray2[4].Value = _detail.serial;
                            parameterArray2[5].Value = _detail.userid;
                            parameterArray2[6].Value = _detail.balance;
                            parameterArray2[7].Value = _detail.status;
                            parameterArray2[8].Value = _detail.rec_acc;
                            parameterArray2[9].Value = _detail.rec_name;
                            parameterArray2[10].Value = _detail.cur_type;
                            parameterArray2[11].Value = _detail.pay_amt;
                            parameterArray2[12].Value = _detail.succ_amt;
                            parameterArray2[13].Value = _detail.remark;
                            parameterArray2[14].Value = _detail.trans_id;
                            parameterArray2[15].Value = _detail.message;
                            parameterArray2[0x10].Value = DateTime.Now;
                            DataBase.ExecuteScalar(transaction, "proc_tenpay_batch_trans_detail_ADD", (object[]) parameterArray2);
                        }
                        transaction.Commit();
                        connection.Close();
                        return num2;
                    }
                    transaction.Rollback();
                    connection.Close();
                    num3 = 0;
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    ExceptionHandler.HandleException(exception);
                    num3 = 0;
                }
                finally
                {
                    if (transaction != null)
                    {
                        transaction.Dispose();
                    }
                }
            }
            return num3;
        }
    }
}

