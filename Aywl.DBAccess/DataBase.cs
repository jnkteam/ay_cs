namespace OriginalStudio.DBAccess
{
    using OriginalStudio.Cache;
    using OriginalStudio.Lib.SysConfig;
    using OriginalStudio.Lib.ExceptionHandling;
    using OriginalStudio.Lib.Security;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;
    using System.Web;
    using System.Xml;

    public sealed class DataBase
    {
        private DataBase()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return RuntimeSetting.ConnectString;
            }
        }

        private static void AddParameters(SqlCommand command, IDictionary<string, object> parameters)
        {
            command.Parameters.Clear();
            foreach (KeyValuePair<string, object> pair in parameters)
            {
                string key = pair.Key;
                if (!pair.Key.StartsWith("@"))
                {
                    key = "@" + key;
                }
                command.Parameters.Add(new SqlParameter(key, pair.Value));
            }
        }

        public static SqlDependency AddSqlDependency(string cacheKey, string strSql, IDictionary<string, object> parameters)
        {
            OnChangeEventHandler handler = null;
            SqlDependency dependency2;
            try
            {
                string connectionString = ConnectionString;
                SqlDependency.Start(connectionString);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(strSql, connection);
                    if (parameters != null)
                    {
                        AddParameters(command, parameters);
                    }
                    SqlDependency dependency = new SqlDependency(command);
                    if (handler == null)
                    {
                        handler = delegate(object sender, SqlNotificationEventArgs e)
                        {
                            if (e.Info != SqlNotificationInfo.Invalid)
                            {
                            }
                            WebCache.GetCacheService().RemoveObject(cacheKey);
                        };
                    }
                    dependency.OnChange += handler;
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    command.ExecuteNonQuery();
                    dependency2 = dependency;
                }
            }
            catch (SqlException exception)
            {
                ExceptionHandler.HandleException(exception);
                dependency2 = null;
            }
            catch (Exception exception2)
            {
                ExceptionHandler.HandleException(exception2);
                dependency2 = null;
            }
            return dependency2;
        }

        public static SqlDependency AddSqlDependency(string cacheKey, string tableName, string column, string where, IDictionary<string, object> parameters)
        {
            string strSql = string.Format("select {0} from {1}.{2}", column, "dbo", tableName);
            if (!string.IsNullOrEmpty(where))
            {
                strSql = strSql + " where " + where;
            }
            return AddSqlDependency(cacheKey, strSql, parameters);
        }

        public static SqlDependency AddSqlDependency(string cacheKey, string dbOwner, string tableName, string column, string where, IDictionary<string, object> parameters)
        {
            string strSql = string.Format("select {0} from {1}.{2}", column, dbOwner, tableName);
            if (!string.IsNullOrEmpty(where))
            {
                strSql = strSql + " where " + where;
            }
            return AddSqlDependency(cacheKey, strSql, parameters);
        }

        private static void AssignParameterValues(SqlParameter[] commandParameters, DataRow dataRow)
        {
            if ((commandParameters != null) && (dataRow != null))
            {
                int num = 0;
                foreach (SqlParameter parameter in commandParameters)
                {
                    if ((parameter.ParameterName == null) || (parameter.ParameterName.Length <= 1))
                    {
                        throw new Exception(string.Format("Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: '{1}'.", num, parameter.ParameterName));
                    }
                    if (dataRow.Table.Columns.IndexOf(parameter.ParameterName.Substring(1)) != -1)
                    {
                        parameter.Value = dataRow[parameter.ParameterName.Substring(1)];
                    }
                    num++;
                }
            }
        }

        public static void AssignParameterValues(SqlParameter[] commandParameters, params object[] parameterValues)
        {
            if ((commandParameters != null) && (parameterValues != null))
            {
                if (commandParameters.Length != parameterValues.Length)
                {
                    throw new ArgumentException("Parameter count does not match Parameter Value count.");
                }
                int index = 0;
                int length = commandParameters.Length;
                while (index < length)
                {
                    if (parameterValues[index] is IDbDataParameter)
                    {
                        IDbDataParameter parameter = (IDbDataParameter) parameterValues[index];
                        if (parameter.Value == null)
                        {
                            commandParameters[index].Value = DBNull.Value;
                        }
                        else
                        {
                            commandParameters[index].Value = parameter.Value;
                        }
                    }
                    else if (parameterValues[index] == null)
                    {
                        commandParameters[index].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[index].Value = parameterValues[index];
                    }
                    index++;
                }
            }
        }

        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (commandParameters != null)
            {
                foreach (SqlParameter parameter in commandParameters)
                {
                    if (parameter != null)
                    {
                        if (((parameter.Direction == ParameterDirection.InputOutput) || (parameter.Direction == ParameterDirection.Input)) && (parameter.Value == null))
                        {
                            parameter.Value = DBNull.Value;
                        }
                        command.Parameters.Add(parameter);
                    }
                }
            }
        }

        public static SqlCommand CreateCommand(SqlConnection connection, string spName, params string[] sourceColumns)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            SqlCommand command = new SqlCommand(spName, connection);
            command.CommandType = CommandType.StoredProcedure;
            if ((sourceColumns != null) && (sourceColumns.Length > 0))
            {
                SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(connection, spName);
                for (int i = 0; i < sourceColumns.Length; i++)
                {
                    spParameterSet[i].SourceColumn = sourceColumns[i];
                }
                AttachParameters(command, spParameterSet);
            }
            return command;
        }

        public static DataSet ExecuteDataset(CommandType commandType, string commandText)
        {
            return ExecuteDataset(ConnectionString, commandType, commandText, null);
        }

        public static DataSet ExecuteDataset(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            return ExecuteDataset(ConnectionString, commandType, commandText, commandParameters);
        }

        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteDataset(connection, commandType, commandText, null);
        }

        public static DataSet ExecuteDataset(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues == null) || (parameterValues.Length <= 0))
            {
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(connection, spName);
            AssignParameterValues(spParameterSet, parameterValues);
            return ExecuteDataset(connection, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteDataset(transaction, commandType, commandText, null);
        }

        public static DataSet ExecuteDataset(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues == null) || (parameterValues.Length <= 0))
            {
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(transaction.Connection, spName);
            AssignParameterValues(spParameterSet, parameterValues);
            return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteDataset(connectionString, commandType, commandText, null);
        }

        public static DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues == null) || (parameterValues.Length <= 0))
            {
                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(connectionString, spName);
            AssignParameterValues(spParameterSet, parameterValues);
            return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, connection, null, commandType, commandText, commandParameters, out mustCloseConnection);
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                command.Parameters.Clear();
                if (mustCloseConnection)
                {
                    connection.Close();
                }
                return dataSet;
            }
        }

        public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                command.Parameters.Clear();
                return dataSet;
            }
        }

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return ExecuteDataset(connection, commandType, commandText, commandParameters);
            }
        }

        public static DataSet ExecuteDatasetTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow == null) || (dataRow.ItemArray.Length <= 0))
            {
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(connection, spName);
            AssignParameterValues(spParameterSet, dataRow);
            return ExecuteDataset(connection, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static DataSet ExecuteDatasetTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow == null) || (dataRow.ItemArray.Length <= 0))
            {
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(transaction.Connection, spName);
            AssignParameterValues(spParameterSet, dataRow);
            return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static DataSet ExecuteDatasetTypedParams(string connectionString, string spName, DataRow dataRow)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow == null) || (dataRow.ItemArray.Length <= 0))
            {
                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(connectionString, spName);
            AssignParameterValues(spParameterSet, dataRow);
            return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(ConnectionString, commandType, commandText, null);
        }

        public static int ExecuteNonQuery(string commandText, params SqlParameter[] commandParameters)
        {
            return ExecuteNonQuery(CommandType.Text, commandText, commandParameters);
        }

        public static int ExecuteNonQuery(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            return ExecuteNonQuery(ConnectionString, commandType, commandText, commandParameters);
        }

        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connection, commandType, commandText, null);
        }

        public static int ExecuteNonQuery(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues == null) || (parameterValues.Length <= 0))
            {
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(connection, spName);
            AssignParameterValues(spParameterSet, parameterValues);
            return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(transaction, commandType, commandText, null);
        }

        public static int ExecuteNonQuery(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues == null) || (parameterValues.Length <= 0))
            {
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(transaction.Connection, spName);
            AssignParameterValues(spParameterSet, parameterValues);
            return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connectionString, commandType, commandText, null);
        }

        public static int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues == null) || (parameterValues.Length <= 0))
            {
                return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(connectionString, spName);
            AssignParameterValues(spParameterSet, parameterValues);
            return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, connection, null, commandType, commandText, commandParameters, out mustCloseConnection);
            int num = command.ExecuteNonQuery();
            command.Parameters.Clear();
            if (mustCloseConnection)
            {
                connection.Close();
            }
            return num;
        }

        public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
            int num = command.ExecuteNonQuery();
            command.Parameters.Clear();
            return num;
        }

        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
            }
        }

        public static int ExecuteNonQueryTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow == null) || (dataRow.ItemArray.Length <= 0))
            {
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(connection, spName);
            AssignParameterValues(spParameterSet, dataRow);
            return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static int ExecuteNonQueryTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow == null) || (dataRow.ItemArray.Length <= 0))
            {
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(transaction.Connection, spName);
            AssignParameterValues(spParameterSet, dataRow);
            return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static int ExecuteNonQueryTypedParams(string connectionString, string spName, DataRow dataRow)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow == null) || (dataRow.ItemArray.Length <= 0))
            {
                return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(connectionString, spName);
            AssignParameterValues(spParameterSet, dataRow);
            return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static SqlDataReader ExecuteReader(CommandType commandType, string commandText)
        {
            return ExecuteReader(ConnectionString, commandType, commandText, null);
        }

        public static SqlDataReader ExecuteReader(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            return ExecuteReader(ConnectionString, commandType, commandText, commandParameters);
        }

        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteReader(connection, commandType, commandText, null);
        }

        public static SqlDataReader ExecuteReader(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues == null) || (parameterValues.Length <= 0))
            {
                return ExecuteReader(connection, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(connection, spName);
            AssignParameterValues(spParameterSet, parameterValues);
            return ExecuteReader(connection, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteReader(transaction, commandType, commandText, null);
        }

        public static SqlDataReader ExecuteReader(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues == null) || (parameterValues.Length <= 0))
            {
                return ExecuteReader(transaction, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(transaction.Connection, spName);
            AssignParameterValues(spParameterSet, parameterValues);
            return ExecuteReader(transaction, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteReader(connectionString, commandType, commandText, null);
        }

        public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues == null) || (parameterValues.Length <= 0))
            {
                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(connectionString, spName);
            AssignParameterValues(spParameterSet, parameterValues);
            return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            return ExecuteReader(connection, null, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }

        public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }

        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlDataReader reader;
            if ((connectionString == null) || (connectionString.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                reader = ExecuteReader(connection, null, commandType, commandText, commandParameters, SqlConnectionOwnership.Internal);
            }
            catch
            {
                if (connection != null)
                {
                    connection.Close();
                }
                throw;
            }
            return reader;
        }

        private static SqlDataReader ExecuteReader(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlConnectionOwnership connectionOwnership)
        {
            SqlDataReader reader;
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            bool mustCloseConnection = false;
            SqlCommand command = new SqlCommand();
            try
            {
                PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
                SqlDataReader reader2 = (connectionOwnership != SqlConnectionOwnership.External) ? command.ExecuteReader(CommandBehavior.CloseConnection) : command.ExecuteReader(CommandBehavior.CloseConnection);
                bool flag2 = true;
                foreach (DbParameter parameter in command.Parameters)
                {
                    if (parameter.Direction != ParameterDirection.Input)
                    {
                        flag2 = false;
                    }
                }
                if (flag2)
                {
                    command.Parameters.Clear();
                }
                reader = reader2;
            }
            catch
            {
                if (mustCloseConnection)
                {
                    connection.Close();
                }
                throw;
            }
            return reader;
        }

        public static SqlDataReader ExecuteReaderTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow == null) || (dataRow.ItemArray.Length <= 0))
            {
                return ExecuteReader(connection, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(connection, spName);
            AssignParameterValues(spParameterSet, dataRow);
            return ExecuteReader(connection, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static SqlDataReader ExecuteReaderTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow == null) || (dataRow.ItemArray.Length <= 0))
            {
                return ExecuteReader(transaction, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(transaction.Connection, spName);
            AssignParameterValues(spParameterSet, dataRow);
            return ExecuteReader(transaction, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static SqlDataReader ExecuteReaderTypedParams(string connectionString, string spName, DataRow dataRow)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow == null) || (dataRow.ItemArray.Length <= 0))
            {
                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(connectionString, spName);
            AssignParameterValues(spParameterSet, dataRow);
            return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static object ExecuteScalar(CommandType commandType, string commandText)
        {
            return ExecuteScalar(ConnectionString, commandType, commandText, null);
        }

        public static object ExecuteScalar(string commandText, params SqlParameter[] parameterValues)
        {
            return ExecuteScalar(CommandType.Text, commandText, parameterValues);
        }

        public static object ExecuteScalar(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            return ExecuteScalar(ConnectionString, commandType, commandText, commandParameters);
        }

        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteScalar(connection, commandType, commandText, null);
        }

        public static object ExecuteScalar(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues == null) || (parameterValues.Length <= 0))
            {
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(connection, spName);
            AssignParameterValues(spParameterSet, parameterValues);
            return ExecuteScalar(connection, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteScalar(transaction, commandType, commandText, null);
        }

        public static object ExecuteScalar(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues == null) || (parameterValues.Length <= 0))
            {
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(transaction.Connection, spName);
            AssignParameterValues(spParameterSet, parameterValues);
            return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteScalar(connectionString, commandType, commandText, null);
        }

        public static object ExecuteScalar(string connectionString, string spName, params object[] parameterValues)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues == null) || (parameterValues.Length <= 0))
            {
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(connectionString, spName);
            AssignParameterValues(spParameterSet, parameterValues);
            return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, connection, null, commandType, commandText, commandParameters, out mustCloseConnection);
            object obj2 = command.ExecuteScalar();
            command.Parameters.Clear();
            if (mustCloseConnection)
            {
                connection.Close();
            }
            return obj2;
        }

        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
            object obj2 = command.ExecuteScalar();
            command.Parameters.Clear();
            return obj2;
        }

        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return ExecuteScalar(connection, commandType, commandText, commandParameters);
            }
        }

        public static string ExecuteScalarToStr(CommandType commandType, string commandText)
        {
            object obj2 = ExecuteScalar(ConnectionString, commandType, commandText);
            if (obj2 == null)
            {
                return "";
            }
            return obj2.ToString();
        }

        public static string ExecuteScalarToStr(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            object obj2 = ExecuteScalar(ConnectionString, commandType, commandText, commandParameters);
            if (obj2 == null)
            {
                return "";
            }
            return obj2.ToString();
        }

        public static string ExecuteScalarToStr(string connectionString, CommandType commandType, string commandText)
        {
            object obj2 = ExecuteScalar(ConnectionString, commandType, commandText);
            if (obj2 == null)
            {
                return "";
            }
            return obj2.ToString();
        }

        public static string ExecuteScalarToStr(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            object obj2 = ExecuteScalar(ConnectionString, commandType, commandText, commandParameters);
            if (obj2 == null)
            {
                return "";
            }
            return obj2.ToString();
        }

        public static object ExecuteScalarTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow == null) || (dataRow.ItemArray.Length <= 0))
            {
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(connection, spName);
            AssignParameterValues(spParameterSet, dataRow);
            return ExecuteScalar(connection, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static object ExecuteScalarTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow == null) || (dataRow.ItemArray.Length <= 0))
            {
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(transaction.Connection, spName);
            AssignParameterValues(spParameterSet, dataRow);
            return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static object ExecuteScalarTypedParams(string connectionString, string spName, DataRow dataRow)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow == null) || (dataRow.ItemArray.Length <= 0))
            {
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(connectionString, spName);
            AssignParameterValues(spParameterSet, dataRow);
            return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteXmlReader(connection, commandType, commandText, null);
        }

        public static XmlReader ExecuteXmlReader(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues == null) || (parameterValues.Length <= 0))
            {
                return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(connection, spName);
            AssignParameterValues(spParameterSet, parameterValues);
            return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteXmlReader(transaction, commandType, commandText, null);
        }

        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues == null) || (parameterValues.Length <= 0))
            {
                return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(transaction.Connection, spName);
            AssignParameterValues(spParameterSet, parameterValues);
            return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            XmlReader reader;
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            bool mustCloseConnection = false;
            SqlCommand command = new SqlCommand();
            try
            {
                PrepareCommand(command, connection, null, commandType, commandText, commandParameters, out mustCloseConnection);
                XmlReader reader2 = command.ExecuteXmlReader();
                command.Parameters.Clear();
                reader = reader2;
            }
            catch
            {
                if (mustCloseConnection)
                {
                    connection.Close();
                }
                throw;
            }
            return reader;
        }

        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
            XmlReader reader = command.ExecuteXmlReader();
            command.Parameters.Clear();
            return reader;
        }

        public static XmlReader ExecuteXmlReaderTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow == null) || (dataRow.ItemArray.Length <= 0))
            {
                return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(connection, spName);
            AssignParameterValues(spParameterSet, dataRow);
            return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static XmlReader ExecuteXmlReaderTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow == null) || (dataRow.ItemArray.Length <= 0))
            {
                return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
            }
            SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(transaction.Connection, spName);
            AssignParameterValues(spParameterSet, dataRow);
            return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static void FillDataset(SqlConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            FillDataset(connection, commandType, commandText, dataSet, tableNames, null);
        }

        public static void FillDataset(SqlConnection connection, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(connection, spName);
                AssignParameterValues(spParameterSet, parameterValues);
                FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames, spParameterSet);
            }
            else
            {
                FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames);
            }
        }

        public static void FillDataset(SqlTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            FillDataset(transaction, commandType, commandText, dataSet, tableNames, null);
        }

        public static void FillDataset(SqlTransaction transaction, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = ParamsCache.GetSpParameterSet(transaction.Connection, spName);
                AssignParameterValues(spParameterSet, parameterValues);
                FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames, spParameterSet);
            }
            else
            {
                FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames);
            }
        }

        public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                FillDataset(connection, commandType, commandText, dataSet, tableNames);
            }
        }

        public static void FillDataset(string connectionString, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                FillDataset(connection, spName, dataSet, tableNames, parameterValues);
            }
        }

        public static void FillDataset(SqlConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
        {
            FillDataset(connection, null, commandType, commandText, dataSet, tableNames, commandParameters);
        }

        public static void FillDataset(SqlTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
        {
            FillDataset(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames, commandParameters);
        }

        public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                FillDataset(connection, commandType, commandText, dataSet, tableNames, commandParameters);
            }
        }

        private static void FillDataset(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }
            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                if ((tableNames != null) && (tableNames.Length > 0))
                {
                    string sourceTable = "Table";
                    for (int i = 0; i < tableNames.Length; i++)
                    {
                        if ((tableNames[i] == null) || (tableNames[i].Length == 0))
                        {
                            throw new ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames");
                        }
                        adapter.TableMappings.Add(sourceTable, tableNames[i]);
                        sourceTable = sourceTable + ((i + 1)).ToString();
                    }
                }
                adapter.Fill(dataSet);
                command.Parameters.Clear();
            }
            if (mustCloseConnection)
            {
                connection.Close();
            }
        }

        public static SqlParameter MakeInParam(string ParamName, SqlDbType DbType, int Size, object Value)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }

        public static SqlParameter MakeOutParam(string ParamName, SqlDbType DbType, int Size)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, null);
        }

        public static SqlParameter MakeParam(string ParamName, SqlDbType DbType, int Size, ParameterDirection Direction, object Value)
        {
            SqlParameter parameter = (Size <= 0) ? new SqlParameter(ParamName, DbType) : new SqlParameter(ParamName, DbType, Size);
            parameter.Direction = Direction;
            if ((Direction != ParameterDirection.Output) || (Value != null))
            {
                parameter.Value = Value;
            }
            return parameter;
        }

        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, out bool mustCloseConnection)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if ((commandText == null) || (commandText.Length == 0))
            {
                throw new ArgumentNullException("commandText");
            }
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }
            command.CommandTimeout = 180;
            command.Connection = connection;
            command.CommandText = commandText;
            if (transaction != null)
            {
                if (transaction.Connection == null)
                {
                    throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                }
                command.Transaction = transaction;
            }
            command.CommandType = commandType;
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
        }

        public static int RunProc(string procName)
        {
            return ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, procName, null);
        }

        public static int RunProc(string procName, SqlParameter[] prams)
        {
            return ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, procName, prams);
        }

        public static void RunProc(string procName, out DataSet ds)
        {
            ds = ExecuteDataset(ConnectionString, CommandType.StoredProcedure, procName, null);
        }

        public static void RunProc(string procName, out SqlDataReader reader)
        {
            reader = ExecuteReader(ConnectionString, CommandType.StoredProcedure, procName, null);
        }

        public static void RunProc(string procName, out object obj)
        {
            obj = ExecuteScalar(ConnectionString, CommandType.StoredProcedure, procName, null);
        }

        public static void RunProc(string procName, out XmlReader xmlReader)
        {
            xmlReader = ExecuteXmlReader(new SqlConnection(ConnectionString), CommandType.StoredProcedure, procName, null);
        }

        public static void RunProc(string procName, SqlParameter[] prams, out DataSet ds)
        {
            ds = ExecuteDataset(ConnectionString, CommandType.StoredProcedure, procName, prams);
        }

        public static void RunProc(string procName, SqlParameter[] prams, out SqlDataReader reader)
        {
            reader = ExecuteReader(ConnectionString, CommandType.StoredProcedure, procName, prams);
        }

        public static void RunProc(string procName, SqlParameter[] prams, out object obj)
        {
            obj = ExecuteScalar(ConnectionString, CommandType.StoredProcedure, procName, prams);
        }

        public static void RunProc(string procName, SqlParameter[] prams, out XmlReader xmlReader)
        {
            xmlReader = ExecuteXmlReader(new SqlConnection(ConnectionString), CommandType.StoredProcedure, procName, prams);
        }

        public static void UpdateDataset(SqlCommand insertCommand, SqlCommand deleteCommand, SqlCommand updateCommand, DataSet dataSet, string tableName)
        {
            if (insertCommand == null)
            {
                throw new ArgumentNullException("insertCommand");
            }
            if (deleteCommand == null)
            {
                throw new ArgumentNullException("deleteCommand");
            }
            if (updateCommand == null)
            {
                throw new ArgumentNullException("updateCommand");
            }
            if ((tableName == null) || (tableName.Length == 0))
            {
                throw new ArgumentNullException("tableName");
            }
            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                adapter.UpdateCommand = updateCommand;
                adapter.InsertCommand = insertCommand;
                adapter.DeleteCommand = deleteCommand;
                adapter.Update(dataSet, tableName);
                dataSet.AcceptChanges();
            }
        }

        private enum SqlConnectionOwnership
        {
            External = 2,
            Internal = 1
        }
    }
}

