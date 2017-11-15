using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using DBAccess;
using System.Data;

namespace KuaiCard.BLL
{
	public class ProductFactory
	{
        /// <summary>
        /// 获取价格附近的商品
        /// </summary>
        /// <param name="p_price"></param>
        /// <returns></returns>
        public KuaiCard.Model.ProductInfo GetRandomProductInfo(decimal p_price)
        {
            KuaiCard.Model.ProductInfo model = new Model.ProductInfo();

            SqlParameter[] commandParameters = new SqlParameter[] { 
                DataBase.MakeInParam("@p_price", SqlDbType.Money, 20, p_price) };
            DataSet ds = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_prod_get_random", commandParameters);

            if ((ds != null) && (ds.Tables.Count > 0))
            {
                DataTable dt = ds.Tables[0];

                model.ID = dt.Rows[0]["ID"].ToString();
                model.ProdName = dt.Rows[0]["ProdName"].ToString();
                model.ProdDesc = dt.Rows[0]["ProdDesc"].ToString();
                model.ShortName = dt.Rows[0]["ShortName"].ToString();
                model.Price = dt.Rows[0]["Price"].ToString();                
                model.DispUrl = dt.Rows[0]["DispUrl"].ToString();
                model.StoreName = dt.Rows[0]["StoreName"].ToString();
                model.StoreID = dt.Rows[0]["StoreID"].ToString();
                model.TerminalId = dt.Rows[0]["TerminalId"].ToString();
            }

            return model;
        }
	
        /// <summary>
        /// 随机生成电话号码
        /// </summary>
        /// <returns></returns>
        public string getRandomTel()
        {
            string[] telStarts = { "134", "135", "136", "137", "138", "139", "150", "151", "152", "157", "158", "159", "130", "131", "132", "155", "156", "133", "153", "180", "181", "182", "183", "185", "186", "176" };
            long tick = DateTime.Now.Ticks; 
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            int n = ran.Next(10, 1000);
            int index = ran.Next(0, telStarts.Length - 1);
            string first = telStarts[index];
            string second = (ran.Next(100, 888) + 10000).ToString().Substring(1);
            string thrid = (ran.Next(1, 9100) + 10000).ToString().Substring(1);
            return first + second + thrid;
        }
    }
}
