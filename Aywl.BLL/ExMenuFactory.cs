using OriginalStudio.DBAccess;
using OriginalStudio.Lib.ExceptionHandling;
using OriginalStudio.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace OriginalStudio.BLL
{
    public class ExMenuFactory
    {
        internal static string SQL_TABLE   = "ex_menu";
        internal static string TABLE_RULES = "manageRules";
        
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
        /// <summary>
        /// 获取菜单ID
        /// </summary>
        /// <returns></returns>
        public static int getExMenuIdByControl(string control)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("select top 1 id  from " + SQL_TABLE + " ");
                builder.Append(" where control = @control ");
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@control", SqlDbType.VarChar, 300) };
                commandParameters[0].Value = control;

                DataSet set = DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), commandParameters);

                DataTable table = null;
                if (set.Tables.Count != 0)
                {
                    table = set.Tables[0];
                    return int.Parse(table.Rows[0]["id"].ToString());
                }
                return 0;
            }
            catch (Exception exception)
            {
                //ExceptionHandler.HandleException(exception);
                return 0;
            }
        }
        /// <summary>
        /// 操作权限ID
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static int getRulesIdByControl(string control)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("select top 1 id  from " + TABLE_RULES + " ");
                builder.Append(" where name = @control ");
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@control", SqlDbType.VarChar, 300) };
                commandParameters[0].Value = control;

                DataSet set = DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), commandParameters);

                DataTable table = null;
                if (set.Tables.Count != 0)
                {
                    table = set.Tables[0];
                    return int.Parse(table.Rows[0]["id"].ToString());
                }
                return 0;
            }
            catch (Exception exception)
            {
                //ExceptionHandler.HandleException(exception);
                return 0;
            }
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
            //admin user auth 
           
            Manage manageModel = ManageFactory.GetModel(ManageFactory.GetCurrent());
            Model.ManageRoles rolesModle = ManageRolesFactory.GetModelById(manageModel.ManageRole);
            string menuText = rolesModle.Menu;
            menuText = !string.IsNullOrEmpty(menuText) ? ManageRolesFactory.DeCode(menuText) : string.Empty;
            string[] menuArr = !string.IsNullOrEmpty(menuText) ? menuText.Split(',') : new string[10];
            foreach (DataRow read in table.Rows)
            {
                if (menuArr.Contains(read["id"].ToString())) { 
                treeView += "<li class='treeview pli_" + read["id"] + "'>";
                treeView += "<a href = '#'>";
                treeView += "<i class='fa " + read["icons"] + "'></i> <span>" + read["title"] + "</span>";
                treeView += "<span class='pull-right-container'>";
                treeView += "<i class='fa fa-angle-left pull-right'></i>";
                treeView += "</span>";
                treeView += "</a>";
                treeView += "<ul class='treeview-menu'>";
                StringBuilder builderChild = new StringBuilder();
                builderChild.Append("select   * from " + SQL_TABLE + " where is_hide = 0 and pid = @cid  order by sort asc");
                SqlParameter[] commandParametersChild = new SqlParameter[] { new SqlParameter("@cid", SqlDbType.Int, 10) };
                commandParametersChild[0].Value = read["id"];
                DataSet setChild = DataBase.ExecuteDataset(CommandType.Text, builderChild.ToString(), commandParametersChild);
                DataTable tableChild = setChild.Tables.Count != 0 ? setChild.Tables[0] : null;

                if (tableChild != null)
                {
                    foreach (DataRow readChild in tableChild.Rows)
                    {
                        if (menuArr.Contains(readChild["id"].ToString())) { 
                        treeView += "<li class='li_" + readChild["id"] + "'><a href = '" + readChild["url"] + "sign=" + readChild["pid"] + "&menuId=" + readChild["id"] + "' ><i class='fa fa-circle-o'></i>" + readChild["title"] + "</a></li>";
                        }
                    }
                }

                treeView += "</ul>";
                treeView += "</li>";
                }
            }
            return treeView;
        }

        public static bool authContains(string id, string type) {
            Manage manageModel = ManageFactory.GetModel(ManageFactory.GetCurrent());
            Model.ManageRoles rolesModle = ManageRolesFactory.GetModelById(manageModel.ManageRole);
            string Text = type == "menu" ? rolesModle.Menu : rolesModle.Rules;
            Text = !string.IsNullOrEmpty(Text) ? ManageRolesFactory.DeCode(Text) : string.Empty;
            string[] Arr = !string.IsNullOrEmpty(Text) ? Text.Split(',') : new string[10];
            return Arr.Contains(id.ToString());
        }


        /// <summary>
        /// 获取权限菜单
        /// </summary>
        /// <param name="modle"></param>
        /// <returns></returns>
        public static string getRolesMenu(ManageRoles modle)
        {
            string menuText = modle.Menu;
            menuText = !string.IsNullOrEmpty(menuText) ? ManageRolesFactory.DeCode(menuText) : string.Empty;
           
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
                builderChild.Append("select   * from " + SQL_TABLE + " where is_hide = 0 and pid = @cid  order by sort asc");
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modle"></param>
        /// <returns></returns>
        public static string getRolesRules(ManageRoles modle)
        {
            string rulesText = modle.Rules;
            rulesText = !string.IsNullOrEmpty(rulesText) ? ManageRolesFactory.DeCode(rulesText) : string.Empty;
           
            string checkedText = string.Empty;
            string[] rulesArr = !string.IsNullOrEmpty(rulesText) ? rulesText.Split(',') : new string[10];
            string rolesRolus = string.Empty;

            StringBuilder builder = new StringBuilder();
            builder.Append("select   * from  "+TABLE_RULES+" ");
            builder.Append(" where status = 1 and pid = @id  order by sort asc");
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
            commandParameters[0].Value = 0;
            DataSet set = DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), commandParameters);
            DataTable table = set.Tables.Count != 0 ? set.Tables[0] : null;

            foreach (DataRow read in table.Rows)
            {
                checkedText = rulesArr.Contains(read["id"].ToString()) ? "checked" : string.Empty;

                rolesRolus += "<tbody>";
                rolesRolus += "<td align='center'>";
                rolesRolus += "<tr><td style = 'font-weight: bold;' align = 'left'> " + read["title"] + " </td></tr>";
                rolesRolus += "<tr><td>";
                StringBuilder builderChild = new StringBuilder();
                builderChild.Append("select   * from " + TABLE_RULES + " where status = 1 and pid = @cid  order by sort asc");
                SqlParameter[] commandParametersChild = new SqlParameter[] { new SqlParameter("@cid", SqlDbType.Int, 10) };
                commandParametersChild[0].Value = read["id"];
                DataSet setChild = DataBase.ExecuteDataset(CommandType.Text, builderChild.ToString(), commandParametersChild);
                DataTable tableChild = setChild.Tables.Count != 0 ? setChild.Tables[0] : null;

                if (tableChild != null)
                {
                    foreach (DataRow readChild in tableChild.Rows)
                    {
                        checkedText = rulesArr.Contains(readChild["id"].ToString()) ? "checked" : string.Empty;
                        rolesRolus += "<div class='sign1'>";
                        rolesRolus += "<input class='checkbox1'  name='rulesId[]' " + checkedText + "  value='" + readChild["id"] + "' type='checkbox'/><span class='textsign'>" + readChild["title"] + "</span></div>";
                        
                        
                    }
                }
                rolesRolus += " </td></tr>";
                rolesRolus += "</tbody>";
            }


            return rolesRolus;
        }
    }
}
