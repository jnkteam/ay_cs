namespace OriginalStudio.BLL.User
{
    using DBAccess;
    using OriginalStudio.Model.User;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using OriginalStudio.Lib.Utils;

    /// <summary>
    /// 商户图片管理
    /// </summary>
    public class MchUsersImageFactory
    {
        #region 增删改

        public static int UploadUserImage(MchUserImageInfo model)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@userid",SqlDbType.Int),
                new SqlParameter("@imagefile",SqlDbType.VarChar,1000),
                new SqlParameter("@imagetype",SqlDbType.Int),
                new SqlParameter("@imagedesc",SqlDbType.VarChar,1000)
            };
            parameters[0].Value = model.UserID;
            parameters[1].Value = model.ImageFile;
            parameters[2].Value = model.ImageType;
            parameters[3].Value = model.ImageDesc;

            return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_mch_userIdImage_add", parameters);
        }

        public static int DeleteUserImage(int imgId)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@id",SqlDbType.Int)
            };
            parameters[0].Value = imgId;

            return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_mch_userIdImage_delete", parameters);

        }

        #endregion

        #region 集合对象

        public static DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            return new DataSet();
        }

        /// <summary>
        /// 商户图片列表。
        /// </summary>
        /// <param name="userId">0显示所有用户图片;>1显示指定用户图片</param>
        /// <param name="status">0显示所有用户图片;  其余状态 1,2,4</param>
        /// <returns></returns>
        public DataSet GetUserImages(int userId, int status = 0)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@userId",SqlDbType.Int),
                new SqlParameter("@status",SqlDbType.Int)
            };
            parameters[0].Value = userId;
            parameters[1].Value = status;

            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_mch_userIdImage_GetByUser", parameters);

        }

        public static MchUserImageInfo GetModel(int imgId)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@id",SqlDbType.Int)
            };
            parameters[0].Value = imgId;

            DataSet ds = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_mch_userIdImage_get", parameters);

            return GetModelFromDs(ds);
        }

        public static MchUserImageInfo GetModelFromDs(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }

            DataRow dr = ds.Tables[0].Rows[0];

            MchUserImageInfo modle = new MchUserImageInfo();
            modle.ID = Convert.ToInt32(dr["id"]);
            modle.UserID = Convert.ToInt32(dr["userid"]);
            modle.ImageFile = Convert.ToString(dr["imagefile"]);
            //modle.ImageStream = ;
            modle.ImageType = (IdImagTypeEnum)(Utils.StrToInt(dr["imagetype"].ToString(), 0));
            modle.ImageDesc = Convert.ToString(dr["imagedesc"]);
            modle.Status = (IdImageStatusEnum)(Utils.StrToInt(dr["status"].ToString(), 0));
            modle.AddTime = Utils.StrToDateTime(dr["addTime"].ToString()) ;
            modle.CheckUser = Utils.StrToInt(dr["checkuser"].ToString(), 0);
            modle.CheckTime = Utils.StrToDateTime(dr["CheckTime"].ToString()); ;

            return modle;
        }

        #endregion

        #region 审核

        /// <summary>
        /// 审核用户图片
        /// </summary>
        /// <param name="id">图片序号</param>
        /// <param name="checkUser">审核人</param>
        /// <param name="checkStatus">审核结果.  2:成功   4:失败</param>
        /// <returns></returns>
        public static int CheckUserImage(int imgId , int checkUser,int checkStatus)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@id",SqlDbType.Int),
                new SqlParameter("@status",SqlDbType.Int),
                new SqlParameter("@checkuser",SqlDbType.Int)
            };
            parameters[0].Value = imgId;
            parameters[1].Value = checkStatus;
            parameters[2].Value = checkUser;

            return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_mch_userIdImage_check", parameters);

        }

        #endregion
    }
}

