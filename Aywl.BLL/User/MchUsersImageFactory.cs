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

    /// <summary>
    /// 商户图片管理
    /// </summary>
    public class MchUsersImageFactory
    {
        #region 增删改

        public static int UploadUserImage(MchUserImageInfo model)
        {
            return 1;
        }

        public static int DeleteUserImage(MchUserImageInfo model)
        {
            return 1;
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
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataSet GetUserImages(int userId)
        {
            return new DataSet();
        }

        public static MchUserImageInfo GetModel(int id)
        {
            SqlParameter[] commandParameters = new SqlParameter[] {
                new SqlParameter("@userId", SqlDbType.Int, 10)
            };
            commandParameters[0].Value = id;
            MchUserImageInfo info = new MchUserImageInfo();
            return info;
        }

        public static MchUserImageInfo GetModelFromDs(DataSet ds)
        {
            MchUserImageInfo info = new MchUserImageInfo();
            return info;
        }

        #endregion

        #region 审核

        /// <summary>
        /// 审核用户图片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="checkUser"></param>
        /// <param name="checkStatus"></param>
        /// <returns></returns>
        public static int CheckUserImage(int id , int checkUser,int checkStatus)
        {
            return 1;
        }

        #endregion
    }
}

