﻿namespace OriginalStudio.BLL
{
    using DBAccess;
    using OriginalStudio.Model;
    using OriginalStudio.Lib.SysConfig;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.Lib.ExceptionHandling;
    using OriginalStudio.Lib.Security;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web;

    public class ManageFactory
    {
        internal const string MANAGE_CONTEXT_KEY = "{F25E0AC4-032C-42ba-B123-2289C6DBE4F1}";
        internal const string MANAGE_LOGIN_SESSIONID = "{90F37739-31E2-4b92-A35E-013313CE553D}";
        internal const string MANAGE_SECOND_SESSIONID = "{36147A08-17F3-477a-8449-75AC0EF9299F}";

        public static int Add(Manage model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@id", SqlDbType.Int, 10),
                    new SqlParameter("@username", SqlDbType.VarChar, 20),
                    new SqlParameter("@password", SqlDbType.VarChar, 100),
                    new SqlParameter("@role", SqlDbType.Int, 10),
                    new SqlParameter("@status", SqlDbType.Int, 10),
                    new SqlParameter("@relname", SqlDbType.NVarChar, 50),
                    new SqlParameter("@lastLoginIp", SqlDbType.VarChar, 50),
                    new SqlParameter("@lastLoginTime", SqlDbType.DateTime),
                    new SqlParameter("@sessionid", SqlDbType.VarChar, 100),
                    new SqlParameter("@secondpwd", SqlDbType.VarChar, 100),
                    new SqlParameter("@commissiontype", SqlDbType.TinyInt),
                    new SqlParameter("@commission", SqlDbType.Decimal, 9),
                    new SqlParameter("@cardcommission", SqlDbType.Decimal, 9),
                    new SqlParameter("@isSuperAdmin", SqlDbType.TinyInt, 1),
                    new SqlParameter("@isAgent", SqlDbType.TinyInt, 1),
                    new SqlParameter("@qq", SqlDbType.VarChar, 20), 
                    new SqlParameter("@tel", SqlDbType.VarChar, 20),
                    new SqlParameter("@manageRole", SqlDbType.Int, 10)
                 };
                commandParameters[0].Direction = ParameterDirection.Output;
                commandParameters[1].Value = model.username;
                commandParameters[2].Value = model.password;
                commandParameters[3].Value = model.role;
                commandParameters[4].Value = model.status;
                commandParameters[5].Value = model.relname;
                commandParameters[6].Value = model.lastLoginIp;
                commandParameters[7].Value = model.lastLoginTime;
                commandParameters[8].Value = model.sessionid;
                commandParameters[9].Value = model.secondpwd;
                commandParameters[10].Value = model.commissiontype;
                commandParameters[11].Value = model.commission;
                commandParameters[12].Value = model.cardcommission;
                commandParameters[13].Value = model.isSuperAdmin;
                commandParameters[14].Value = model.isAgent;
                commandParameters[15].Value = model.qq;
                commandParameters[0x10].Value = model.tel;
                commandParameters[17].Value = model.ManageRole;
                int num = DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_manage_add", commandParameters);
                return (int) commandParameters[0].Value;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        private static string BuilderWhere(List<SearchParam> param, List<SqlParameter> paramList)
        {
            StringBuilder builder = new StringBuilder(" 1 = 1");
            if ((param != null) && (param.Count > 0))
            {
                for (int i = 0; i < param.Count; i++)
                {
                    SqlParameter parameter;
                    SearchParam param2 = param[i];
                    switch (param2.ParamKey.Trim().ToLower())
                    {
                        case "manageid":
                            builder.Append(" AND [manageID] = @manageID");
                            parameter = new SqlParameter("@manageID", SqlDbType.Int);
                            parameter.Value = (int) param2.ParamValue;
                            paramList.Add(parameter);
                            break;

                        case "username":
                            builder.Append(" AND [userName] like @userName");
                            parameter = new SqlParameter("@userName", SqlDbType.VarChar, 20);
                            parameter.Value = "%" + SqlHelper.CleanString((string) param2.ParamValue, 100) + "%";
                            paramList.Add(parameter);
                            break;

                        case "starttime":
                            builder.Append(" AND [lastTime] > @starttime");
                            parameter = new SqlParameter("@starttime", SqlDbType.DateTime);
                            parameter.Value = Convert.ToDateTime(param2.ParamValue);
                            paramList.Add(parameter);
                            break;

                        case "endtime":
                            builder.Append(" AND [lastTime] < @endtime");
                            parameter = new SqlParameter("@endtime", SqlDbType.DateTime);
                            parameter.Value = Convert.ToDateTime(param2.ParamValue);
                            paramList.Add(parameter);
                            break;
                    }
                }
            }
            return builder.ToString();
        }

        public static bool CheckAdminPermission(bool isSupper, ManageRole allowPermission, ManageRole adminPermission)
        {
            return (isSupper || ((allowPermission & adminPermission) == allowPermission));
        }

        public static bool CheckCurrentPermission(bool shouldSupper, ManageRole allowPermission)
        {
            if (CurrentManage == null)
            {
                return false;
            }
            if (shouldSupper)
            {
                return (CurrentManage.isSuperAdmin > 0);
            }
            return CheckAdminPermission(CurrentManage.isSuperAdmin > 0, allowPermission, CurrentManage.role);
        }

        public static bool Delete(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] {
                    new SqlParameter("@id", SqlDbType.Int, 10)
                };
                commandParameters[0].Value = id;
                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_manage_del", commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static int GetCurrent()
        {
            try
            {
                object obj2 = HttpContext.Current.Session[MANAGE_LOGIN_SESSIONID];
                if (obj2 == null)
                {
                    return 0;
                }
                SqlParameter[] commandParameters = new SqlParameter[] {
                    DataBase.MakeInParam("@sessionId", SqlDbType.VarChar, 100, obj2)
                };
                object obj3 = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_manage_getIdBySession", commandParameters);
                if (obj3 == DBNull.Value)
                {
                    return 0;
                }
                return Convert.ToInt32(obj3);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        public static DataSet GetList(string where)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select * ");
            builder.Append(" FROM v_manage ");
            if (!string.IsNullOrEmpty(where))
            {
                builder.AppendFormat(" where {0}", where);
            }
            return DataBase.ExecuteDataset(CommandType.Text, builder.ToString());
        }

        public static bool GetManagePerformance(int id, DateTime begin, DateTime end, out decimal totalAmt, out decimal commission)
        {
            try
            {
                totalAmt = 0M;
                commission = 0M;
                SqlParameter[] commandParameters = new SqlParameter[] {
                    new SqlParameter("@id", SqlDbType.Int, 10),
                    new SqlParameter("@begin", SqlDbType.DateTime, 8),
                    new SqlParameter("@end", SqlDbType.DateTime, 8)
                };
                commandParameters[0].Value = id;
                commandParameters[1].Value = begin;
                commandParameters[2].Value = end;
                SqlDataReader reader = DataBase.ExecuteReader(CommandType.StoredProcedure, "proc_manage_orderAmt", commandParameters);
                if (reader.Read())
                {
                    totalAmt = Convert.ToDecimal(reader["totalAmt"]);
                    commission = Convert.ToDecimal(reader["commission"]);
                    return true;
                }
                return false;
            }
            catch (Exception exception)
            {
                totalAmt = 0M;
                commission = 0M;
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static string GetManageRoleView(ManageRole role)
        {
            string str = string.Empty;
            ManageRole role2 = role;
            if (role2 <= ManageRole.Orders)
            {
                switch (role2)
                {
                    case ManageRole.None:
                        return "未知";

                    case ManageRole.News:
                        return "新闻管理";

                    case ManageRole.System:
                        return "系统管理";

                    case (ManageRole.System | ManageRole.News):
                    case (ManageRole.Interfaces | ManageRole.News):
                    case (ManageRole.Interfaces | ManageRole.System):
                    case (ManageRole.Interfaces | ManageRole.System | ManageRole.News):
                        return str;

                    case ManageRole.Interfaces:
                        return "接口管理";

                    case ManageRole.Merchant:
                        return "商户管理";

                    case (ManageRole.Merchant | ManageRole.News):
                    case (ManageRole.Merchant | ManageRole.System):
                    case (ManageRole.Merchant | ManageRole.System | ManageRole.News):
                    case (ManageRole.Merchant | ManageRole.Interfaces):
                    case (ManageRole.Merchant | ManageRole.Interfaces | ManageRole.News):
                    case (ManageRole.Merchant | ManageRole.Interfaces | ManageRole.System):
                    case (ManageRole.Merchant | ManageRole.Interfaces | ManageRole.System | ManageRole.News):
                        return str;

                    case ManageRole.Orders:
                        return "订单管理";
                }
                return str;
            }
            if (role2 != ManageRole.Financial)
            {
                if (role2 != ManageRole.Report)
                {
                    return str;
                }
            }
            else
            {
                return "财务管理";
            }
            return "统计报表";
        }

        public static int GetManageUsers(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] {
                    new SqlParameter("@id", SqlDbType.Int, 10)
                };
                commandParameters[0].Value = id;
                return Convert.ToInt32(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_manage_getusers", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        public static Manage GetModel(int id)
        {
            SqlParameter[] commandParameters = new SqlParameter[] {
                new SqlParameter("@id", SqlDbType.Int, 10)
            };
            commandParameters[0].Value = id;
            Manage manage = new Manage();
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_manage_get", commandParameters);
            if (set.Tables[0].Rows.Count > 0)
            {
                if (set.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    manage.id = int.Parse(set.Tables[0].Rows[0]["id"].ToString());
                }
                manage.username = set.Tables[0].Rows[0]["username"].ToString();
                manage.password = set.Tables[0].Rows[0]["password"].ToString();
                manage.secondpwd = set.Tables[0].Rows[0]["secondpwd"].ToString();
               
                if (set.Tables[0].Rows[0]["role"].ToString() != "")
                {
                    manage.role = (ManageRole) int.Parse(set.Tables[0].Rows[0]["role"].ToString());
                }
                if (set.Tables[0].Rows[0]["status"].ToString() != "")
                {
                    manage.status = new int?(int.Parse(set.Tables[0].Rows[0]["status"].ToString()));
                }
                if (set.Tables[0].Rows[0]["manageRole"].ToString() != "")
                {
                    manage.ManageRole = int.Parse(set.Tables[0].Rows[0]["manageRole"].ToString());
                }
                manage.relname = set.Tables[0].Rows[0]["relname"].ToString();
                manage.lastLoginIp = set.Tables[0].Rows[0]["lastLoginIp"].ToString();
                if (set.Tables[0].Rows[0]["lastLoginTime"].ToString() != "")
                {
                    manage.lastLoginTime = new DateTime?(DateTime.Parse(set.Tables[0].Rows[0]["lastLoginTime"].ToString()));
                }
                manage.sessionid = set.Tables[0].Rows[0]["sessionid"].ToString();
                if (set.Tables[0].Rows[0]["commissiontype"].ToString() != "")
                {
                    manage.commissiontype = new int?(int.Parse(set.Tables[0].Rows[0]["commissiontype"].ToString()));
                }
                if (set.Tables[0].Rows[0]["commission"].ToString() != "")
                {
                    manage.commission = new decimal?(decimal.Parse(set.Tables[0].Rows[0]["commission"].ToString()));
                }
                if (set.Tables[0].Rows[0]["cardcommission"].ToString() != "")
                {
                    manage.cardcommission = new decimal?(decimal.Parse(set.Tables[0].Rows[0]["cardcommission"].ToString()));
                }
                if (set.Tables[0].Rows[0]["Balance"].ToString() != "")
                {
                    manage.balance = new decimal?(decimal.Parse(set.Tables[0].Rows[0]["Balance"].ToString()));
                }
                if (set.Tables[0].Rows[0]["isSuperAdmin"].ToString() != "")
                {
                    manage.isSuperAdmin = int.Parse(set.Tables[0].Rows[0]["isSuperAdmin"].ToString());
                }
                if (set.Tables[0].Rows[0]["isAgent"].ToString() != "")
                {
                    manage.isAgent = int.Parse(set.Tables[0].Rows[0]["isAgent"].ToString());
                }
                manage.qq = set.Tables[0].Rows[0]["qq"].ToString();
                manage.tel = set.Tables[0].Rows[0]["tel"].ToString();
                manage.DefaultThemes = set.Tables[0].Rows[0]["default_themes"].ToString(); 
                return manage;
            }
            return null;
        }

        public static bool IsSecondPwdValid()
        {
            if (HttpContext.Current.Session[MANAGE_SECOND_SESSIONID] == null)
            {
                return false;
            }
            return Convert.ToBoolean(HttpContext.Current.Session[MANAGE_SECOND_SESSIONID]);
        }

        public static bool LoginLogDel(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] {
                    new SqlParameter("@id", SqlDbType.Int, 10)
                };
                commandParameters[0].Value = id;
                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_manageLoginLog_del", commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "V_manageLoginLog";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "lastTime desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);
                string commandText = SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + SqlHelper.GetPageSelectSQL("[id]\r\n      ,[type]\r\n      ,[manageID]\r\n      ,[lastIP]\r\n      ,[address]\r\n      ,[remark]\r\n      ,[lastTime]\r\n      ,[sessionId]\r\n      ,[username]\r\n      ,[relname]", tables, wheres, orderby, key, pageSize, page, false);
                return DataBase.ExecuteDataset(CommandType.Text, commandText, paramList.ToArray());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return set;
            }
        }

        public static bool SecPwdVaild(string sedpwd)
        {
            if (!(string.IsNullOrEmpty(sedpwd) || !(Cryptography.MD5(sedpwd) == CurrentManage.secondpwd)))
            {
                HttpContext.Current.Session[MANAGE_SECOND_SESSIONID] = true;
                return true;
            }
            return false;
        }

        public static string SignIn(Manage manage)
        {
            string str = string.Empty;
            try
            {
                if (((manage == null) || string.IsNullOrEmpty(manage.username)) || string.IsNullOrEmpty(manage.password))
                {
                    return "请输入账号密码";
                }
                string str2 = Guid.NewGuid().ToString("b");
                SqlParameter[] commandParameters = new SqlParameter[] { 
                                DataBase.MakeInParam("@username", SqlDbType.VarChar, 50, manage.username), 
                                DataBase.MakeInParam("@password", SqlDbType.VarChar, 100, manage.password), 
                                DataBase.MakeInParam("@loginip", SqlDbType.VarChar, 50, manage.lastLoginIp), 
                                DataBase.MakeInParam("@logintime", SqlDbType.DateTime, 8, DateTime.Now), 
                                DataBase.MakeInParam("@sessionId", SqlDbType.VarChar, 100, str2), 
                                DataBase.MakeInParam("@address", SqlDbType.VarChar, 20, manage.LastLoginAddress),
                                DataBase.MakeInParam("@remark", SqlDbType.VarChar, 100, manage.LastLoginRemark) 
                };
                object obj2 = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_manage_Login", commandParameters);
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    manage.id = (int) obj2;
                    HttpContext.Current.Session[MANAGE_LOGIN_SESSIONID] = str2;
                    str = "登录成功";
                }
                else
                {
                    str = "用户名或者密码错误!";
                }
                return str;
            }
            catch (Exception exception)
            {
                str = "登录失败";
                ExceptionHandler.HandleException(exception);
                return str;
            }
        }

        public static void SignOut()
        {
            HttpContext.Current.Items[MANAGE_CONTEXT_KEY] = null;
            HttpContext.Current.Session[MANAGE_LOGIN_SESSIONID] = null;
            HttpContext.Current.Session[MANAGE_SECOND_SESSIONID] = null;
        }

        public static bool Update(Manage model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@username", SqlDbType.VarChar, 20), new SqlParameter("@password", SqlDbType.VarChar, 100), new SqlParameter("@role", SqlDbType.Int, 10), new SqlParameter("@status", SqlDbType.Int, 10), new SqlParameter("@relname", SqlDbType.NVarChar, 50), new SqlParameter("@lastLoginIp", SqlDbType.VarChar, 50), new SqlParameter("@lastLoginTime", SqlDbType.DateTime), new SqlParameter("@sessionid", SqlDbType.VarChar, 50), new SqlParameter("@secondpwd", SqlDbType.VarChar, 100), new SqlParameter("@commissiontype", SqlDbType.TinyInt), new SqlParameter("@commission", SqlDbType.Decimal, 9), new SqlParameter("@cardcommission", SqlDbType.Decimal, 9), new SqlParameter("@isSuperAdmin", SqlDbType.TinyInt, 1), new SqlParameter("@isAgent", SqlDbType.TinyInt, 1), new SqlParameter("@qq", SqlDbType.VarChar, 20), 
                    new SqlParameter("@tel", SqlDbType.VarChar, 20),
                    new SqlParameter("@manageRole", SqlDbType.Int, 10)
                 };
                commandParameters[0].Value = model.id;
                commandParameters[1].Value = model.username;
                commandParameters[2].Value = model.password;
                commandParameters[3].Value = model.role;
                commandParameters[4].Value = model.status;
                commandParameters[5].Value = model.relname;
                commandParameters[6].Value = model.lastLoginIp;
                commandParameters[7].Value = model.lastLoginTime;
                commandParameters[8].Value = model.sessionid;
                commandParameters[9].Value = model.secondpwd;
                commandParameters[10].Value = model.commissiontype;
                commandParameters[11].Value = model.commission;
                commandParameters[12].Value = model.cardcommission;
                commandParameters[13].Value = model.isSuperAdmin;
                commandParameters[14].Value = model.isAgent;
                commandParameters[15].Value = model.qq;
                commandParameters[0x10].Value = model.tel;
                commandParameters[17].Value = model.ManageRole;

                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_manage_Update", commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static Manage CurrentManage
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Items[MANAGE_CONTEXT_KEY] == null)
                    {
                        int current = GetCurrent();
                        if (current <= 0)
                        {
                            return null;
                        }
                        HttpContext.Current.Items[MANAGE_CONTEXT_KEY] = GetModel(current);
                    }
                    return (HttpContext.Current.Items[MANAGE_CONTEXT_KEY] as Manage);
                }
                return null;
            }
        }
    }
}

