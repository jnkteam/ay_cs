using OriginalStudio.DBAccess;
using OriginalStudio.Model.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace OriginalStudio.BLL.User
{
    /// <summary>
    /// 商户自定义通道账号配置。
    /// </summary>
    public class MchUserSupplierFactory
    {

        /// <summary>
        /// 获取商户独立通道列表
        /// </summary>
        /// <param name="userId">商户ID。0：全部</param>
        /// <param name="supplierCode">通道代码。0：全部</param>
        /// <returns></returns>
        public static DataSet GetUserSupplierList(int userId, int supplierCode)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@userid",SqlDbType.Int),
                    new SqlParameter("@suppliercode",SqlDbType.Int)
                };
            parameters[0].Value = userId;
            parameters[1].Value = supplierCode;
            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_mch_user_supplier_list", parameters);
        }

        /// <summary>
        /// 新增或编辑商户独立通道
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int EditUserSupplier(MchUserSupplier model)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@userid",SqlDbType.Int),
                new SqlParameter("@suppliercode",SqlDbType.Int),
                new SqlParameter("@puserid",SqlDbType.VarChar,100),
                new SqlParameter("@puserkey",SqlDbType.VarChar,100),
                new SqlParameter("@pusername",SqlDbType.VarChar,100),
                new SqlParameter("@puserparm1",SqlDbType.VarChar,100),
                new SqlParameter("@puserparm2",SqlDbType.VarChar,100),
                new SqlParameter("@issingle",SqlDbType.Int),
                new SqlParameter("@istransfer",SqlDbType.Int),
                new SqlParameter("@transferurl",SqlDbType.VarChar,1000)
            };
            parameters[0].Value = model.UserID;
            parameters[1].Value = model.SupplierCode;
            parameters[2].Value = model.PUserID;
            parameters[3].Value = model.PUserKey;
            parameters[4].Value = model.PUserName;
            parameters[5].Value = model.PUserParm1;
            parameters[6].Value = model.PUserParm2;
            parameters[7].Value = model.IsSingle;
            parameters[8].Value = model.IsTransfer;
            parameters[9].Value = model.TransferUrl;

            return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_mch_user_supplier_edit", parameters);

        }

        /// <summary>
        /// 删除自定义通道
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static DataSet DeleteUserSupplier(int Id)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@id",SqlDbType.Int)
                };
            parameters[0].Value = Id;
            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_mch_user_supplier_delete", parameters);
        }


        /// <summary>
        /// 显示自定义通道
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static DataSet GetUserSupplier(int Id)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@id",SqlDbType.Int)
                };
            parameters[0].Value = Id;

            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_mch_user_supplier_get", parameters);
        }
    }
}
