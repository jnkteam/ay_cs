using OriginalStudio.DBAccess;
using OriginalStudio.Model;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace OriginalStudio.BLL
{
    public class ExMenuFactory
    {
        internal static string SQL_TABLE = "ex_menu";
        /// <summary>
        /// 读取菜单
        /// </summary>
        /// <returns></returns>
        public static DataTable getExMenuList()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select   * from " + SQL_TABLE + " ");
            builder.Append(" where id>0 ");
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
            commandParameters[0].Value = 5;

            DataSet set = DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), commandParameters);

            DataTable table = null;
            if (set.Tables.Count != 0)
            {
                table = set.Tables[0];
            }
            return table;
        }
        //获取简易二级分类菜单样式   递归效率低下
        /**
         * @paragram pid  起始点
         * @return String css 树状图
         * */
        public static string getTreeView(int pid)
        {
            string treeView = string.Empty;
            StringBuilder builder = new StringBuilder();
            builder.Append("select   * from " + SQL_TABLE + " ");
            builder.Append(" where is_hide = 0 and pid = @id  order by sort asc");
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
            commandParameters[0].Value = pid;
            DataSet set = DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), commandParameters);
            DataTable table = set.Tables.Count != 0 ? set.Tables[0] : null;
            foreach (DataRow read in table.Rows)
            {
                treeView += "<li class='treeview pli_" + read["id"] + "'>";
                treeView += "<a href = '#'>";
                treeView += "<i class='fa " + read["icons"] + "'></i> <span>" + read["title"] + "</span>";
                treeView += "<span class='pull-right-container'>";
                treeView += "<i class='fa fa-angle-left pull-right'></i>";
                treeView += "</span>";
                treeView += "</a>";
                treeView += "<ul class='treeview-menu'>";
                StringBuilder builderChild = new StringBuilder();
                builderChild.Append("select   * from ex_menu where is_hide = 0 and pid = @cid  order by sort asc");
                SqlParameter[] commandParametersChild = new SqlParameter[] { new SqlParameter("@cid", SqlDbType.Int, 10) };
                commandParametersChild[0].Value = read["id"];
                DataSet setChild = DataBase.ExecuteDataset(CommandType.Text, builderChild.ToString(), commandParametersChild);
                DataTable tableChild = setChild.Tables.Count != 0 ? setChild.Tables[0] : null;

                if (tableChild != null)
                {
                    foreach (DataRow readChild in tableChild.Rows)
                    {
                        treeView += "<li class='li_" + readChild["id"] + "'><a href = '" + readChild["url"] + "sign=" + readChild["pid"] + "&menuId=" + readChild["id"] + "' ><i class='fa fa-circle-o'></i>" + readChild["title"] + "</a></li>";
                    }
                }

                treeView += "</ul>";
                treeView += "</li>";
            }
            return treeView;
        }
        /// <summary>
        /// 获取权限菜单
        /// </summary>
        /// <param name="modle"></param>
        /// <returns></returns>
        public static string getRolesMenu(ManageRoles modle)
        {
            string menuText = modle.Menu;
            string checkedText = string.Empty;
            string[] menuArr = !string.IsNullOrEmpty(menuText) ? menuText.Split(',') : new string[10];
            string rolesMenu = string.Empty;

            StringBuilder builder = new StringBuilder();
            builder.Append("select   * from " + SQL_TABLE + " ");
            builder.Append(" where is_hide = 0 and pid = @id  order by sort asc");
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
            commandParameters[0].Value = 0;
            DataSet set = DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), commandParameters);
            DataTable table = set.Tables.Count != 0 ? set.Tables[0] : null;

            foreach (DataRow read in table.Rows)
            {
                checkedText = menuArr.Contains(read["id"].ToString()) ? "checked" : string.Empty;

                rolesMenu += "<tr><td align='center'>" + read["id"] + "</td>";
                rolesMenu += "<td align='center'>";
                rolesMenu += "<input class='checkbox' name='menuId[]' " + checkedText + " value='" + read["id"] + "' type='checkbox'/></td>";
                rolesMenu += "<td align='left'>" + read["title"] + "</td></tr>";
                StringBuilder builderChild = new StringBuilder();
                builderChild.Append("select   * from ex_menu where is_hide = 0 and pid = @cid  order by sort asc");
                SqlParameter[] commandParametersChild = new SqlParameter[] { new SqlParameter("@cid", SqlDbType.Int, 10) };
                commandParametersChild[0].Value = read["id"];
                DataSet setChild = DataBase.ExecuteDataset(CommandType.Text, builderChild.ToString(), commandParametersChild);
                DataTable tableChild = setChild.Tables.Count != 0 ? setChild.Tables[0] : null;

                if (tableChild != null)
                {
                    foreach (DataRow readChild in tableChild.Rows)
                    {
                        checkedText = menuArr.Contains(readChild["id"].ToString()) ? "checked" : string.Empty;
                        rolesMenu += "<tr><td align='center'>" + readChild["id"] + "</td>";
                        rolesMenu += "<td align='center'>";
                        rolesMenu += "<input class='checkbox' name='menuId[]' " + checkedText + " value='" + readChild["id"] + "' type='checkbox'/></td>";
                        rolesMenu += "<td align='left'><div id='sign'>&nbsp;</div><div id = 'sign1'>" + readChild["title"] + "</div></td></tr>";
                    }
                }
            }


            return rolesMenu;
        }


        public static string getRolesRules(ManageRoles modle)
        {
            return "string";
        }
    }
}
