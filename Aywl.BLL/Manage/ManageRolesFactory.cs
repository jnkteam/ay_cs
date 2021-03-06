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
    using System.Security.Cryptography;
    using System.IO;

    /// <summary>
    /// 角色管理
    /// </summary>
    public class ManageRolesFactory
    {


        internal static string SQL_TABLE = "manageRoles";
        internal static string SQL_TABLE_FIELD = "[id],[module],[type],[title],[description],[status],[rules],[menu]";
        internal static string SQL_TABLE_FIELD_INSERT = "[module],[type],[title],[description],[status]";
        const string KEY_64 = "EavicCpp";//密匙，8个字符，64位   
        const string IV_64  = "EavicCpp";


        /// <summary>
        /// 列表获取
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static DataSet GetList(string where)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select "+ SQL_TABLE_FIELD);
            builder.Append(" FROM  "+ SQL_TABLE);
            if (!string.IsNullOrEmpty(where))
            {
                builder.AppendFormat(" where {0}", where);
            }
            return DataBase.ExecuteDataset(CommandType.Text, builder.ToString());
        }

        /// <summary>
        /// 模型获取
        /// </summary>
        /// <param name="id"></param>
        /// 
        /// <returns></returns>
        public static ManageRoles GetModelById(int id)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select  " + SQL_TABLE_FIELD +" FROM  " + SQL_TABLE);
            builder.Append(" where id=@id  ");
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10)};
            commandParameters[0].Value = id;
            DataSet set = DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), commandParameters);
            if (set.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(set.Tables[0].Rows[0]);
            }
            return null;
        }

        public static ManageRoles DataRowToModel(DataRow row)
        {
            ManageRoles manageRoles = new ManageRoles();
            if (row != null)
            {
                if ((row["id"] != null) && (row["id"].ToString() != ""))
                {
                    manageRoles.Id = int.Parse(row["id"].ToString());
                }
                if ((row["module"] != null) && (row["module"].ToString() != ""))
                {
                    manageRoles.Module = row["module"].ToString();
                }
                if ((row["type"] != null) && (row["type"].ToString() != ""))
                {
                    manageRoles.Type = int.Parse(row["type"].ToString());
                }

                if ((row["title"] != null) && (row["title"].ToString() != ""))
                {
                    manageRoles.Title = row["title"].ToString();
                }
                if ((row["description"] != null) && (row["description"].ToString() != ""))
                {
                    manageRoles.Description = row["description"].ToString();
                }

                if ((row["status"] != null) && (row["status"].ToString() != ""))
                {
                    manageRoles.Status = int.Parse(row["status"].ToString());
                }

                if ((row["rules"] != null) && (row["rules"].ToString() != ""))
                {
                    manageRoles.Rules = row["rules"].ToString();
                }
                if ((row["menu"] != null) && (row["menu"].ToString() != ""))
                {
                    manageRoles.Menu = row["menu"].ToString();
                }
            }
            return manageRoles;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Add(ManageRoles model) { 
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("insert into "+ SQL_TABLE + "(");
                builder.Append(" "+ SQL_TABLE_FIELD_INSERT + ")");
                builder.Append(" values (");
                builder.Append("@module,@type,@title,@description,@status)");
                SqlParameter[] commandParameters = new SqlParameter[] {
                   
                    new SqlParameter("@module", SqlDbType.VarChar, 20),
                    new SqlParameter("@type", SqlDbType.Int, 10),
                    new SqlParameter("@title", SqlDbType.VarChar, 30),
                    new SqlParameter("@description", SqlDbType.VarChar, 80),
                    new SqlParameter("@status", SqlDbType.Int, 10),
                   
                };
               
                commandParameters[0].Value = model.Module;
                commandParameters[1].Value = model.Type;
                commandParameters[2].Value = model.Title;
                commandParameters[3].Value = model.Description;
                commandParameters[4].Value = model.Status;
                
               
                int result  = DataBase.ExecuteNonQuery(CommandType.Text, builder.ToString(), commandParameters);
                return  result;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Update(ManageRoles model)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("update " + SQL_TABLE + " set ");
                builder.Append("module=@module,");
                builder.Append("type=@type,");
                builder.Append("title=@title,");
                builder.Append("description=@description,");
                builder.Append("status=@status");
                builder.Append(" where id=@id  ");
                SqlParameter[] commandParameters = new SqlParameter[] {
                    new SqlParameter("@module", SqlDbType.VarChar, 20),
                    new SqlParameter("@type", SqlDbType.Int, 10),
                    new SqlParameter("@title", SqlDbType.VarChar, 30),
                    new SqlParameter("@description", SqlDbType.VarChar, 80),
                    new SqlParameter("@status", SqlDbType.Int, 10),
                    new SqlParameter("@id", SqlDbType.Int, 10),
                };
                   

                commandParameters[0].Value = model.Module;
                commandParameters[1].Value = model.Type;
                commandParameters[2].Value = model.Title;
                commandParameters[3].Value = model.Description;
                commandParameters[4].Value = model.Status;
                commandParameters[5].Value = model.Id;

                return (DataBase.ExecuteNonQuery(CommandType.Text, builder.ToString(), commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }
        /// <summary>
        /// 更新menu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool UpdateMenu(ManageRoles model)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("update " + SQL_TABLE + " set ");
                builder.Append("menu=@menu ");               
                builder.Append(" where id=@id  ");
                SqlParameter[] commandParameters = new SqlParameter[] {
                    new SqlParameter("@menu", SqlDbType.VarChar, 500),                  
                    new SqlParameter("@id", SqlDbType.Int, 10),
                };
               
                string menus = !string.IsNullOrEmpty(model.Menu.ToString()) ? EnCode(model.Menu.ToString()) : string.Empty;
                commandParameters[0].Value = menus;
                commandParameters[1].Value = model.Id;
                

                return (DataBase.ExecuteNonQuery(CommandType.Text, builder.ToString(), commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }
        public static bool UpdateRules(ManageRoles model)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("update " + SQL_TABLE + " set ");
                builder.Append("rules=@rules ");
                builder.Append(" where id=@id  ");
                SqlParameter[] commandParameters = new SqlParameter[] {
                    new SqlParameter("@rules", SqlDbType.VarChar, 500),
                    new SqlParameter("@id", SqlDbType.Int, 10),
                };

                string rules = !string.IsNullOrEmpty(model.Rules.ToString()) ? EnCode(model.Rules.ToString()) : string.Empty;
                commandParameters[0].Value = rules;
                commandParameters[1].Value = model.Id;


                return (DataBase.ExecuteNonQuery(CommandType.Text, builder.ToString(), commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }
        /// <summary>
        /// encode
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string EnCode(string data)
        {
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            int i = cryptoProvider.KeySize;
            MemoryStream ms = new MemoryStream();
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey,

    byIV), CryptoStreamMode.Write);

            StreamWriter sw = new StreamWriter(cst);
            sw.Write(data);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }
        /// <summary>
        /// decode
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string DeCode(string data)
        {
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(data);
            }
            catch
            {
                return null;
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream(byEnc);
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey,

    byIV), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cst);
            return sr.ReadToEnd();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Delete(int id)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("delete from " + SQL_TABLE + " ");
            builder.Append(" where id=@id ");
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10)};
            commandParameters[0].Value = id;
           
            return (DataBase.ExecuteNonQuery(CommandType.Text, builder.ToString(), commandParameters) > 0);
        }
    }
}

