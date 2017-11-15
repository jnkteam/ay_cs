namespace KuaiCard.BLL
{
    using KuaiCard.DAL;
    using KuaiCard.Model;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using KuaiCardLib.Data;

    public class SettledFactory
    {
        private static settled dal = new settled();

        public static int Add(SettledInfo model)
        {
            try
            {
                return dal.Add(model);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        public static bool Allfails()
        {
            try
            {
                return dal.Allfails();
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static bool AllPass(string batchNo)
        {
            try
            {
                return dal.AllPass(batchNo);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static bool AllSettle()
        {
            try
            {
                return dal.AllSettle();
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        /// <summary>
        /// 提交代付申请。
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Apply(SettledInfo model)
        {
            try
            {
                return dal.Apply(model);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        /// <summary>
        /// 执行代付付款。
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Pay(SettledInfo model)
        {
            try
            {
                return dal.Pay(model);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 99;
            }
        }

        public static bool Audit(int id, int status)
        {
            try
            {
                return dal.Audit(id, status);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static bool AutoSettled(decimal balance)
        {
            try
            {
                return dal.AutoSettled(balance);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static bool BatchPass(string ids, string batchNo, out DataTable withdrawListByApi)
        {
            withdrawListByApi = null;
            try
            {
                return dal.BatchPass(ids, batchNo, out withdrawListByApi);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static bool BatchSettle(string ids)
        {
            try
            {
                return dal.BatchSettle(ids);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static bool Cancel(int id)
        {
            try
            {
                return dal.Cancel(id);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static List<SettledInfo> DataTableToList(DataTable dt)
        {
            List<SettledInfo> list = new List<SettledInfo>();
            int count = dt.Rows.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    SettledInfo item = dal.DataRowToModel(dt.Rows[i]);
                    if (item != null)
                    {
                        list.Add(item);
                    }
                }
            }
            return list;
        }

        public static bool Delete(DateTime etime)
        {
            try
            {
                return dal.Delete(etime);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static DataTable Export(string ids)
        {
            try
            {
                return dal.Export(ids);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static DataTable GetListWithdrawByApi(string batchNo)
        {
            try
            {
                return dal.GetListWithdrawByApi(batchNo).Tables[0];
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static SettledInfo GetModel(int id)
        {
            try
            {
                return dal.GetModel(id);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static decimal GetPayDayMoney(int uid)
        {
            try
            {
                return dal.GetPayDayMoney(uid);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0M;
            }
        }

        public static decimal Getpayingmoney(int uid)
        {
            try
            {
                return dal.Getpayingmoney(uid);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0M;
            }
        }

        public static string GetSettleBankName(string code)
        {
            string str = code;
            switch (code)
            {
                case "0002":
                    return "支付宝";

                case "0003":
                    return "财付通";

                case "1002":
                    return "工商银行";

                case "1005":
                    return "农业银行";

                case "1003":
                    return "建设银行";

                case "1026":
                    return "中国银行";

                case "1001":
                    return "招商银行";

                case "1006":
                    return "民生银行";

                case "1020":
                    return "交通银行";

                case "1025":
                    return "华夏银行";

                case "1009":
                    return "兴业银行";

                case "1027":
                    return "广发银行";

                case "1004":
                    return "浦发银行";

                case "1022":
                    return "光大银行";

                case "1021":
                    return "中信银行";

                case "1010":
                    return "平安银行";

                case "1066":
                    return "邮政储蓄银行";
            }
            return str;
        }

        public static decimal GetUserDaySettledAmt(int userid, string day)
        {
            try
            {
                return dal.GetUserDaySettledAmt(userid, day);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0M;
            }
        }

        public static int GetUserDaySettledTimes(int userid, string day)
        {
            try
            {
                return dal.GetUserDaySettledTimes(userid, day);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        public static DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            try
            {
                return dal.PageSearch(searchParams, pageSize, page, orderby);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }


        public static bool Update(SettledInfo model)
        {
            try
            {
                return dal.Update(model);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }
    }
}

