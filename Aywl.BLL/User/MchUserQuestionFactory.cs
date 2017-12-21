using OriginalStudio.DBAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace OriginalStudio.BLL.User
{
    public class MchUserQuestionFactory
    {
        /// <summary>
        /// 设置问题答案
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="qusetionId"></param>
        /// <param name="qusetionType"></param>
        /// <param name="answer"></param>
        public static int SetQuestion(int userId, int qusetionId, int qusetionType, string answer)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@userid",SqlDbType.Int),
                new SqlParameter("@questionid",SqlDbType.Int),
                new SqlParameter("@qtype",SqlDbType.Int),
                new SqlParameter("@answer",SqlDbType.VarChar,4000)
            };
            parameters[0].Value = userId;
            parameters[1].Value = qusetionId;
            parameters[2].Value = qusetionType;
            parameters[3].Value = answer;

            return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_mch_user_question_set", parameters);
        }
    }
}
