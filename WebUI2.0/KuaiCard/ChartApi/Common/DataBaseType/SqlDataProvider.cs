using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;



namespace General.DataProvider
{
	/// <summary>
	/// SqlDataProvider 
	/// </summary>
	public class SqlDataProvider:DataProvider
	{
		#region ================现接口定义的构造================
		/// <summary>
		/// 实现接口定义的构造
		/// </summary>
		/// <param name="connectionString"></param>
		public SqlDataProvider(string connectionString)
		{
			this.Conn.ConnectionString = connectionString;
			this.CommandType = CommandType.Text;			//SQLServer默认是Text
			this.ConnetString = connectionString;
		}

		/// <summary>
		/// 实现接口定义的构造
		/// </summary>
		public SqlDataProvider()
		{

		}


		~SqlDataProvider()
		{
		}
		#endregion



		//私有变量
		//连接变量不用.Dispose。只有在析构函数中销毁。
		private SqlConnection	Conn		= new SqlConnection();	//实例化
		//命令对象可以Dispose。可以重新连接，但是不能=null
		private SqlCommand		Cmd			= new SqlCommand();		//实例化
		private SqlTransaction	Tran;								//实例化
		private string ConnetString = string.Empty;			


		#region ===================公共方法=====================

		/// <summary>
		///	获得当前数据库连接状态
		/// </summary>
		/// <returns></returns>
		public override ConnectionState state()
		{
			return Conn.State;
		}

		/// <summary>
		/// 释放数据连接资源
		/// </summary>
		public override void Dispose()
		{
			if (this.Conn != null) 
				this.Conn = null;
			if (this.Cmd != null)
				this.Cmd = null;
		}


		#endregion


		#region ===================参数操作===================

		/// <summary>
		/// 为命令对象赋参数
		/// </summary>
		/// <param name="sqlCmd">命令对象</param>
        /// <param name="parameters">参数数组</param>
        private void SetUpParameters(SqlCommand sqlCmd, List<IDbDataParameter> parameters)
		{
			try
			{
				//附加参数
				if (parameters == null)
				{
					return;
				}
				sqlCmd.Parameters.Clear();
				foreach (SqlParameter p in parameters)
				{
					//检查输出参数是否是null
					if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
					{
						p.Value = DBNull.Value;
					}
					sqlCmd.Parameters.Add(p);
				}
			}
			catch (SqlException e)
			{
				throw new Exception(e.Message);
			}
			finally
			{
			}
		}


		#endregion


		#region ===============重写基类接口的定义===============

		#region Transaction

		/// <summary>
		/// 打开数据库连接
		/// </summary>
		public override void Open()
		{
			if( this.Conn == null )		//在关闭连接时候，释放了资源，所以要重新连接
				this.Conn.ConnectionString = this.ConnectionString;

			if( this.Conn.State == ConnectionState.Closed)
			{
				this.Conn.Open();

				this.ConnectTimes++;
			}
			else if( this.Conn.State == ConnectionState.Open)
			{

			}
		}


		/// <summary>
		/// 关闭数据库连接
		/// </summary>
		public override void Close()
		{
			if(this.Conn != null)
			{
				if( this.Conn.State == ConnectionState.Open )
				{
					this.Conn.Close();
				}
			}
		}


		/// <summary>
		/// 开启事务
		/// </summary>
		public override void BeginTrans()
		{
			if (Conn == null)
				return;
			this.Open();
			Tran = Conn.BeginTransaction();	//开事务
		}


		/// <summary>
		/// 提交事务
		/// </summary>
		public override void CommitTrans()
		{
			if (this.Tran == null)
				return;
			this.Tran.Commit();
			this.Tran.Dispose();			//释放资源，即释放连接，允许其他操作
		}


		/// <summary>
		/// 回滚事务
		/// </summary>
		public override void RollbackTrans()
		{
			if (this.Tran == null)
				return;
			this.Tran.Rollback();
			this.Tran.Dispose();			//释放资源，即释放连接，允许其他操作
		}


		#endregion


		#region NonQuery

		/// <summary>
		/// 执行指定的SQL语句（如添加、更新、删除等操作）
		/// </summary>
		/// <returns>影响的行数(存在外键问题返回-1、插入重复主键返回-2)</returns>
		public override int ExecuteNonQuery()
		{
			try
			{
				this.Open();
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandType = this.CommandType;
				this.Cmd.CommandText = this.CommandText;
				if (this.Cmd.Transaction == null || this.Tran != null)
				{
					this.Cmd.Transaction = this.Tran;
				}

				//设置参数
				SetUpParameters(this.Cmd,this.IParameters);
			
				//执行数据库操作
				int intResult = this.Cmd.ExecuteNonQuery();

				this._ExecuteTimes ++;				//执行命令次数加1

				return intResult;
			}
			catch( SqlException e )
			{
				switch (e.Number)
				{
					case 547:		//主键与外键
					{
						return -1;
					}
					case 2627:		//主键重复
					{
						return	-2;
					}
					default:
					{
						throw new Exception( e.Message + "\n SQL语句：" + this.CommandText );
					}
				}	
			}
			finally
			{
				//释放资源
				this.Cmd.Parameters.Clear();
				if (this.Tran == null)	//说明用到事务。如果事务对象为空，则释放命令资源
					this.Cmd.Dispose();	//SQLServer执行一条语句不用加事务，数据库自动加事务操作
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}


		#endregion


		#region DataSet
		
		/// <summary>
		/// 使用 DataAdapter 提取数据返回为 DataSet
		/// </summary>
		/// <returns></returns>
		public override DataSet ExecuteDataset()
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = this.CommandText;

				//设置参数
				SetUpParameters(this.Cmd,this.IParameters);
	
				/*	using 语句定义一个范围，在此范围的末尾将处理对象。
					在 using 语句中创建一个实例，确保退出 using 语句时在对象上调用 Dispose。
					当到达 using 语句的末尾，或者如果在语句结束之前引发异常并且控制离开语句
					块，都可以退出 using 语句。
				*/
				
				//这个地方不用显示打开连接，因为SqlDataAdapter会自动打开连接然后关闭。
				using ( SqlDataAdapter Adapter = new SqlDataAdapter(this.Cmd))
				{	
					using( DataSet ds = new DataSet())
					{
						Adapter.Fill(ds);

						this._ExecuteTimes ++;				//执行命令次数加1

						return ds;
					}
				}

			}
			catch( SqlException e )
			{
				this.Dispose();				//为什么释放资源？？？
				throw new Exception( e.Message + "\n SQL语句：" + this.CommandText );
			}
			finally
			{
				// 释放资源
				this.Cmd.Parameters.Clear();
				this.Cmd.Dispose();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}


		/// <summary>
		/// 使用 DataAdapter 提取数据返回为 DataSet(用于分页)
		/// </summary>
		/// <param name="startRecord">起始记录的索引(即从第几条开始显示)</param>
		/// <param name="maxRecord">显示最大的记录数</param>
		/// <returns></returns>
		public override DataSet ExecuteDataset(int startRecord, int maxRecord)
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = this.CommandText;

				//设置参数
				SetUpParameters(this.Cmd,this.IParameters);
	
				using ( SqlDataAdapter Adapter = new SqlDataAdapter(this.Cmd))
				{	
					using( DataSet ds = new DataSet())
					{
                        Adapter.Fill(ds, startRecord, maxRecord, "General");
						this._ExecuteTimes ++;				//执行命令次数加1

						return ds;
					}
				}				
			}
			catch( SqlException e )
			{
				throw new Exception( e.Message + "\n SQL语句：" + this.CommandText );
			}
			finally
			{
				// 释放资源
				this.Cmd.Parameters.Clear();
				this.Cmd.Dispose();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}


		#endregion


		#region DataView

		/// <summary>
		/// 返回DataView（用于排序）
		/// </summary>
		/// <returns></returns>
		public override DataView ExectuteDataView()
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = this.CommandText;

				//设置参数
				SetUpParameters(this.Cmd,this.IParameters);

				using ( SqlDataAdapter Adapter = new SqlDataAdapter(this.Cmd))
				{	
					using( DataSet ds = new DataSet())
					{
						Adapter.Fill(ds);
						this._ExecuteTimes ++;				//执行命令次数加1
						return ds.Tables[0].DefaultView;
					}
				}

			}
			catch( SqlException e )
			{
				this.Dispose();		//为什么释放资源？
				throw new Exception( e.Message + "\n SQL语句：" + this.CommandText );
			}
			catch( Exception e )
			{
				this.Dispose();		//为什么释放资源？
				throw new Exception( e.Message + "\n SQL语句：" + this.CommandText );
			}
			finally
			{
				// 释放资源
				this.Cmd.Parameters.Clear();
				this.Cmd.Dispose();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}


		#endregion


		#region DataTable

		/// <summary>
		/// 使用 DataAdapter 提取数据返回为 DataTable
		/// </summary>
		/// <returns>DataTable</returns>
		public override DataTable ExecuteDataTable()
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = this.CommandText;
				this.Cmd.CommandType = this.CommandType;

				//设置参数
				SetUpParameters(this.Cmd,this.IParameters);

				using ( SqlDataAdapter Adapter = new SqlDataAdapter(this.Cmd))
				{	
					using( DataTable dt	= new DataTable())
					{
						Adapter.Fill(dt);
						this._ExecuteTimes ++;				//执行命令次数加1
						return dt;
					}
				}
			}
			catch (SqlException e)
			{
				throw new Exception(e.Message + "\n SQL语句：" + this.CommandText);
			}
			catch( Exception e )
			{
				throw new Exception( e.Message + "\n SQL语句：" + this.CommandText );
			}
			finally
			{
				// 释放资源
				this.Cmd.Dispose();
				this.Cmd.Parameters.Clear();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}


		/// <summary>
		/// 读取表数据结构信息
		/// </summary>
		/// <returns>DataTable</returns>
		public override DataTable GetSchemaTable()
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = this.CommandText;
				this.Cmd.CommandType = this.CommandType;

				//设置参数
				SetUpParameters(this.Cmd,this.IParameters);
            
				//打开连接
				this.Open();
				// 执行数据库操作
				using ( SqlDataReader Reader = this.Cmd.ExecuteReader())
				{
					if( Reader.Read() )
					{
						using ( DataTable dt = new DataTable())
						{
							DataRow dr;

							dt.Columns.Add("字段名称");
							dt.Columns.Add("字段类型");
							dt.Columns.Add("字段大小");
							dt.Columns.Add("允许空字符串");

							DataTable Dt = Reader.GetSchemaTable();						//获取表的架构

							for( int i = 0; i < Reader.FieldCount; i++ )				//获取当前行中的列数
							{
								dr = dt.NewRow();

								dr[0] = Reader.GetName(i);								//取得指定字段的字段名称
								dr[1] = Reader.GetDataTypeName(i);						//取得指定字段的数据类型
								dr[2] = Convert.ToString(Dt.Rows[i]["ColumnSize"]);		//取得指定行（也就是指定列）的字段大小
								dr[3] = Convert.ToString(Dt.Rows[i]["AllowDBNull"]);	//用来判断字段内是否为Null值

								dt.Rows.Add(dr);
							}
							this._ExecuteTimes ++;				//执行命令次数加1
							return dt;
						}
					}
					else
					{
						return null;
					}
				}
			}
			catch( SqlException e )
			{
				this.Dispose();
				throw new Exception( e.Message + "\n SQL语句：" + this.CommandText );
			}
			finally
			{
				// 释放资源
				this.Cmd.Parameters.Clear();
				this.Cmd.Dispose();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}		

		}


		/// <summary>
		/// 返回数据库名字集合。（SQLServer数据库使用）
		/// </summary>
		/// <returns></returns>
		public override DataTable GetDataBaseCollection()
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = "sp_databases";		//SQLServer系统存储过程，返回数据库名字集合
				this.Cmd.CommandType = CommandType.StoredProcedure;

				using ( SqlDataAdapter Adapter = new SqlDataAdapter(this.Cmd))
				{	
					using( DataTable dt	= new DataTable())
					{
						Adapter.Fill(dt);
						this._ExecuteTimes ++;				//执行命令次数加1
						return dt;
					}
				}
			}
			catch (SqlException e)
			{
				throw new Exception( e.Message + "\n SQL语句:" + this.CommandText); 
			}
			finally
			{
				// 释放资源
				this.Cmd.Dispose();
				this.Cmd.Parameters.Clear();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}


		/// <summary>
		/// 返回当前数据库里系统表集合。（SQLServer数据库使用，数据库由连接字符串决定）
		/// </summary>
		/// <returns></returns>
		public override DataTable GetSystemTableCollection()
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = "sp_tables";			//SQLServer系统存储过程，返回数据库里表名字集合。
				this.Cmd.CommandType = CommandType.StoredProcedure;

				//设置参数
				this.Cmd.Parameters.Add(new SqlParameter("@table_type",SqlDbType.VarChar,100,ParameterDirection.Input,false,0,0,"",DataRowVersion.Default,"'SYSTEM TABLE'"));

				using ( SqlDataAdapter Adapter = new SqlDataAdapter(this.Cmd))
				{	
					using( DataTable dt	= new DataTable())
					{
						Adapter.Fill(dt);
						this._ExecuteTimes ++;				//执行命令次数加1
						return dt;
					}
				}
			}
			catch (SqlException e)
			{
				throw new Exception( e.Message + "\n SQL语句:" + this.CommandText); 
			}
			finally
			{
				// 释放资源
				this.Cmd.Dispose();
				this.Cmd.Parameters.Clear();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}


		/// <summary>
		/// 返回当前数据库里用户表集合。（SQLServer数据库使用，数据库由连接字符串决定）
		/// </summary>
		/// <returns></returns>
		public override DataTable GetUserTableCollection()
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = "sp_tables";			//SQLServer系统存储过程，返回数据库里表名字集合。
				this.Cmd.CommandType = CommandType.StoredProcedure;

				//设置参数
				this.Cmd.Parameters.Add(new SqlParameter("@table_type",SqlDbType.VarChar,100,ParameterDirection.Input,false,0,0,"",DataRowVersion.Default,"'TABLE'"));

				using ( SqlDataAdapter Adapter = new SqlDataAdapter(this.Cmd))
				{	
					using( DataTable dt	= new DataTable())
					{
						Adapter.Fill(dt);
						this._ExecuteTimes ++;				//执行命令次数加1
						return dt;
					}
				}
			}
			catch (SqlException e)
			{
				throw new Exception( e.Message + "\n SQL语句:" + this.CommandText); 
			}
			finally
			{
				// 释放资源
				this.Cmd.Dispose();
				this.Cmd.Parameters.Clear();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}


		/// <summary>
		/// 返回当前数据库里视图集合表。（SQLServer数据库使用，数据库由连接字符串决定）
		/// </summary>
		/// <returns></returns>
		public override DataTable GetViewCollection()
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = "sp_tables";			//SQLServer系统存储过程，返回数据库里表名字集合。
				this.Cmd.CommandType = CommandType.StoredProcedure;

				//设置参数
				this.Cmd.Parameters.Add(new SqlParameter("@table_type",SqlDbType.VarChar,100,ParameterDirection.Input,false,0,0,"",DataRowVersion.Default,"'VIEW'"));

				using ( SqlDataAdapter Adapter = new SqlDataAdapter(this.Cmd))
				{	
					using( DataTable dt	= new DataTable())
					{
						Adapter.Fill(dt);
						this._ExecuteTimes ++;				//执行命令次数加1
						return dt;
					}
				}
			}
			catch (SqlException e)
			{
				throw new Exception( e.Message + "\n SQL语句:" + this.CommandText); 
			}
			finally
			{
				// 释放资源
				this.Cmd.Dispose();
				this.Cmd.Parameters.Clear();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}


		/// <summary>
		/// 返回当前数据库里某个表的列的集合表。（SQLServer数据库使用，数据库由连接字符串决定）
		/// </summary>
		/// <param name="TableName">待查询表</param>
		/// <returns>DataTable</returns>
		public override DataTable GetColumnCollection(string TableName)
		{
			if (TableName == string.Empty ) return null; 
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = "sp_columns";			//SQLServer系统存储过程，返回数据库里表名字集合。
				this.Cmd.CommandType = CommandType.StoredProcedure;

				//设置参数
				this.Cmd.Parameters.Add(new SqlParameter("@table_name",SqlDbType.VarChar,100,ParameterDirection.Input,false,0,0,"",DataRowVersion.Default,TableName));

				using ( SqlDataAdapter Adapter = new SqlDataAdapter(this.Cmd))
				{	
					using( DataTable dt	= new DataTable())
					{
						Adapter.Fill(dt);
						this._ExecuteTimes ++;				//执行命令次数加1
						return dt;
					}
				}
			}
			catch (SqlException e)
			{
				throw new Exception( e.Message + "\n SQL语句:" + this.CommandText); 
			}
			finally
			{
				// 释放资源
				this.Cmd.Dispose();
				this.Cmd.Parameters.Clear();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}


		#endregion


		#region DataRow 

		/// <summary>
		/// 查询并返回第一行数据的所有列的值，不存在则返回Null。
		/// </summary>
		/// <returns>DataRow</returns>
		public override DataRow ReadFirstDataRow()
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = this.CommandText;
				this.Cmd.CommandType = this.CommandType;

				//设置参数
				SetUpParameters(this.Cmd,this.IParameters);
	
				//打开连接
				this.Open();
				// 执行数据库操作
				using ( SqlDataReader Reader = this.Cmd.ExecuteReader(CommandBehavior.CloseConnection))
				{
					this._ExecuteTimes ++;				//执行命令次数加1
					if( Reader.Read() )
					{
						using ( DataTable dt = new DataTable())
						{
							DataRow dr;
							dr = dt.NewRow();

							for( int i = 0; i < Reader.FieldCount; i++ )
							{
								dt.Columns.Add(Reader.GetName(i)); 
								dr[i] = Reader[i];
							}
							dt.Rows.Add(dr);

							return dt.Rows[0];
						}
					}
					else
					{
						return null;
					}
				}
			}
			catch( SqlException e )
			{
				throw new Exception( e.Message + "\n SQL语句：" + this.CommandText );
			}
			finally
			{
				// 释放资源
				this.Cmd.Parameters.Clear();
				this.Cmd.Dispose();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}


		#endregion


		#region IDataReader

		/// <summary>
		/// 读取并返回数据集合
		/// </summary>
		/// <returns>IDataReader</returns>
		public override IDataReader ExecuteReader()
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = this.CommandText;
				this.Cmd.CommandType = this.CommandType;

				//设置参数
				SetUpParameters(this.Cmd,this.IParameters);
	
				//打开连接
				this.Open();
				// 执行数据库操作
				SqlDataReader Reader	= this.Cmd.ExecuteReader(CommandBehavior.CloseConnection);
				this._ExecuteTimes ++;				//执行命令次数加1
				return Reader;
			}
			catch( SqlException e )
			{
				throw new Exception( e.Message + "\n SQL语句：" + this.CommandText );
			}
			finally
			{
				// 释放资源(注意：SqlDataReader不能关闭连接,外界需要显示关闭)
				this.Cmd.Parameters.Clear();
				this.Cmd.Dispose();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}

		
		#endregion


		#region Other

		/// <summary>
		/// 返回结果集中第一行第一列信息。返回的是一个string，不存在则返回 String.Empty
		/// </summary>
		/// <returns>string</returns>
		public override string ExecuteScalar()
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = this.CommandText;
				this.Cmd.CommandType = this.CommandType;

				//设置参数
				SetUpParameters(this.Cmd,this.IParameters);
	
				//打开连接
				this.Open();
				// 执行数据库操作
				using (SqlDataReader Reader	= Cmd.ExecuteReader())
				{
					this._ExecuteTimes ++;				//执行命令次数加1
					if( Reader.Read() )
					{
						return Reader.GetValue(0).ToString();
					}
					else
					{
						return string.Empty;
					}
				}
			}
			catch( SqlException e )
			{
				this.Dispose();
				throw new Exception( e.Message + "\n SQL语句：" + this.CommandText );
			}
			finally
			{
				// 释放资源
				this.Cmd.Parameters.Clear();
				this.Cmd.Dispose();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}



		/// <summary>
		/// 执行查询语句，返回记录数(如: SELECT Count(*) FROM [xxx])
		/// </summary>
		/// <returns>没有有记录返回0</returns>
		public override int GetScalarSQL()
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = this.CommandText;
				this.Cmd.CommandType = this.CommandType;

				//设置参数
				SetUpParameters(this.Cmd,this.IParameters);
	
				//打开连接
				this.Open();
				// 执行数据库操作
				object obj = this.Cmd.ExecuteScalar();
				this._ExecuteTimes ++;				//执行命令次数加1

				if( Object.Equals(obj, null) )
				{
					return 0;
				}
				else
				{
					return (int)obj;
				}
			}
			catch( SqlException e )
			{
				this.Dispose();
				throw new Exception( e.Message + "\n SQL语句：" + this.CommandText );
			}
			finally
			{
				// 释放资源
				this.Cmd.Parameters.Clear();
				this.Cmd.Dispose();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}

		
		#endregion


		#endregion


	}
}
