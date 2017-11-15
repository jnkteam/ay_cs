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
		#region ================�ֽӿڶ���Ĺ���================
		/// <summary>
		/// ʵ�ֽӿڶ���Ĺ���
		/// </summary>
		/// <param name="connectionString"></param>
		public SqlDataProvider(string connectionString)
		{
			this.Conn.ConnectionString = connectionString;
			this.CommandType = CommandType.Text;			//SQLServerĬ����Text
			this.ConnetString = connectionString;
		}

		/// <summary>
		/// ʵ�ֽӿڶ���Ĺ���
		/// </summary>
		public SqlDataProvider()
		{

		}


		~SqlDataProvider()
		{
		}
		#endregion



		//˽�б���
		//���ӱ�������.Dispose��ֻ�����������������١�
		private SqlConnection	Conn		= new SqlConnection();	//ʵ����
		//����������Dispose�������������ӣ����ǲ���=null
		private SqlCommand		Cmd			= new SqlCommand();		//ʵ����
		private SqlTransaction	Tran;								//ʵ����
		private string ConnetString = string.Empty;			


		#region ===================��������=====================

		/// <summary>
		///	��õ�ǰ���ݿ�����״̬
		/// </summary>
		/// <returns></returns>
		public override ConnectionState state()
		{
			return Conn.State;
		}

		/// <summary>
		/// �ͷ�����������Դ
		/// </summary>
		public override void Dispose()
		{
			if (this.Conn != null) 
				this.Conn = null;
			if (this.Cmd != null)
				this.Cmd = null;
		}


		#endregion


		#region ===================��������===================

		/// <summary>
		/// Ϊ������󸳲���
		/// </summary>
		/// <param name="sqlCmd">�������</param>
        /// <param name="parameters">��������</param>
        private void SetUpParameters(SqlCommand sqlCmd, List<IDbDataParameter> parameters)
		{
			try
			{
				//���Ӳ���
				if (parameters == null)
				{
					return;
				}
				sqlCmd.Parameters.Clear();
				foreach (SqlParameter p in parameters)
				{
					//�����������Ƿ���null
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


		#region ===============��д����ӿڵĶ���===============

		#region Transaction

		/// <summary>
		/// �����ݿ�����
		/// </summary>
		public override void Open()
		{
			if( this.Conn == null )		//�ڹر�����ʱ���ͷ�����Դ������Ҫ��������
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
		/// �ر����ݿ�����
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
		/// ��������
		/// </summary>
		public override void BeginTrans()
		{
			if (Conn == null)
				return;
			this.Open();
			Tran = Conn.BeginTransaction();	//������
		}


		/// <summary>
		/// �ύ����
		/// </summary>
		public override void CommitTrans()
		{
			if (this.Tran == null)
				return;
			this.Tran.Commit();
			this.Tran.Dispose();			//�ͷ���Դ�����ͷ����ӣ�������������
		}


		/// <summary>
		/// �ع�����
		/// </summary>
		public override void RollbackTrans()
		{
			if (this.Tran == null)
				return;
			this.Tran.Rollback();
			this.Tran.Dispose();			//�ͷ���Դ�����ͷ����ӣ�������������
		}


		#endregion


		#region NonQuery

		/// <summary>
		/// ִ��ָ����SQL��䣨����ӡ����¡�ɾ���Ȳ�����
		/// </summary>
		/// <returns>Ӱ�������(����������ⷵ��-1�������ظ���������-2)</returns>
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

				//���ò���
				SetUpParameters(this.Cmd,this.IParameters);
			
				//ִ�����ݿ����
				int intResult = this.Cmd.ExecuteNonQuery();

				this._ExecuteTimes ++;				//ִ�����������1

				return intResult;
			}
			catch( SqlException e )
			{
				switch (e.Number)
				{
					case 547:		//���������
					{
						return -1;
					}
					case 2627:		//�����ظ�
					{
						return	-2;
					}
					default:
					{
						throw new Exception( e.Message + "\n SQL��䣺" + this.CommandText );
					}
				}	
			}
			finally
			{
				//�ͷ���Դ
				this.Cmd.Parameters.Clear();
				if (this.Tran == null)	//˵���õ���������������Ϊ�գ����ͷ�������Դ
					this.Cmd.Dispose();	//SQLServerִ��һ����䲻�ü��������ݿ��Զ����������
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}


		#endregion


		#region DataSet
		
		/// <summary>
		/// ʹ�� DataAdapter ��ȡ���ݷ���Ϊ DataSet
		/// </summary>
		/// <returns></returns>
		public override DataSet ExecuteDataset()
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = this.CommandText;

				//���ò���
				SetUpParameters(this.Cmd,this.IParameters);
	
				/*	using ��䶨��һ����Χ���ڴ˷�Χ��ĩβ���������
					�� using ����д���һ��ʵ����ȷ���˳� using ���ʱ�ڶ����ϵ��� Dispose��
					������ using ����ĩβ�����������������֮ǰ�����쳣���ҿ����뿪���
					�飬�������˳� using ��䡣
				*/
				
				//����ط�������ʾ�����ӣ���ΪSqlDataAdapter���Զ�������Ȼ��رա�
				using ( SqlDataAdapter Adapter = new SqlDataAdapter(this.Cmd))
				{	
					using( DataSet ds = new DataSet())
					{
						Adapter.Fill(ds);

						this._ExecuteTimes ++;				//ִ�����������1

						return ds;
					}
				}

			}
			catch( SqlException e )
			{
				this.Dispose();				//Ϊʲô�ͷ���Դ������
				throw new Exception( e.Message + "\n SQL��䣺" + this.CommandText );
			}
			finally
			{
				// �ͷ���Դ
				this.Cmd.Parameters.Clear();
				this.Cmd.Dispose();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}


		/// <summary>
		/// ʹ�� DataAdapter ��ȡ���ݷ���Ϊ DataSet(���ڷ�ҳ)
		/// </summary>
		/// <param name="startRecord">��ʼ��¼������(���ӵڼ�����ʼ��ʾ)</param>
		/// <param name="maxRecord">��ʾ���ļ�¼��</param>
		/// <returns></returns>
		public override DataSet ExecuteDataset(int startRecord, int maxRecord)
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = this.CommandText;

				//���ò���
				SetUpParameters(this.Cmd,this.IParameters);
	
				using ( SqlDataAdapter Adapter = new SqlDataAdapter(this.Cmd))
				{	
					using( DataSet ds = new DataSet())
					{
                        Adapter.Fill(ds, startRecord, maxRecord, "General");
						this._ExecuteTimes ++;				//ִ�����������1

						return ds;
					}
				}				
			}
			catch( SqlException e )
			{
				throw new Exception( e.Message + "\n SQL��䣺" + this.CommandText );
			}
			finally
			{
				// �ͷ���Դ
				this.Cmd.Parameters.Clear();
				this.Cmd.Dispose();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}


		#endregion


		#region DataView

		/// <summary>
		/// ����DataView����������
		/// </summary>
		/// <returns></returns>
		public override DataView ExectuteDataView()
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = this.CommandText;

				//���ò���
				SetUpParameters(this.Cmd,this.IParameters);

				using ( SqlDataAdapter Adapter = new SqlDataAdapter(this.Cmd))
				{	
					using( DataSet ds = new DataSet())
					{
						Adapter.Fill(ds);
						this._ExecuteTimes ++;				//ִ�����������1
						return ds.Tables[0].DefaultView;
					}
				}

			}
			catch( SqlException e )
			{
				this.Dispose();		//Ϊʲô�ͷ���Դ��
				throw new Exception( e.Message + "\n SQL��䣺" + this.CommandText );
			}
			catch( Exception e )
			{
				this.Dispose();		//Ϊʲô�ͷ���Դ��
				throw new Exception( e.Message + "\n SQL��䣺" + this.CommandText );
			}
			finally
			{
				// �ͷ���Դ
				this.Cmd.Parameters.Clear();
				this.Cmd.Dispose();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}


		#endregion


		#region DataTable

		/// <summary>
		/// ʹ�� DataAdapter ��ȡ���ݷ���Ϊ DataTable
		/// </summary>
		/// <returns>DataTable</returns>
		public override DataTable ExecuteDataTable()
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = this.CommandText;
				this.Cmd.CommandType = this.CommandType;

				//���ò���
				SetUpParameters(this.Cmd,this.IParameters);

				using ( SqlDataAdapter Adapter = new SqlDataAdapter(this.Cmd))
				{	
					using( DataTable dt	= new DataTable())
					{
						Adapter.Fill(dt);
						this._ExecuteTimes ++;				//ִ�����������1
						return dt;
					}
				}
			}
			catch (SqlException e)
			{
				throw new Exception(e.Message + "\n SQL��䣺" + this.CommandText);
			}
			catch( Exception e )
			{
				throw new Exception( e.Message + "\n SQL��䣺" + this.CommandText );
			}
			finally
			{
				// �ͷ���Դ
				this.Cmd.Dispose();
				this.Cmd.Parameters.Clear();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}


		/// <summary>
		/// ��ȡ�����ݽṹ��Ϣ
		/// </summary>
		/// <returns>DataTable</returns>
		public override DataTable GetSchemaTable()
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = this.CommandText;
				this.Cmd.CommandType = this.CommandType;

				//���ò���
				SetUpParameters(this.Cmd,this.IParameters);
            
				//������
				this.Open();
				// ִ�����ݿ����
				using ( SqlDataReader Reader = this.Cmd.ExecuteReader())
				{
					if( Reader.Read() )
					{
						using ( DataTable dt = new DataTable())
						{
							DataRow dr;

							dt.Columns.Add("�ֶ�����");
							dt.Columns.Add("�ֶ�����");
							dt.Columns.Add("�ֶδ�С");
							dt.Columns.Add("������ַ���");

							DataTable Dt = Reader.GetSchemaTable();						//��ȡ��ļܹ�

							for( int i = 0; i < Reader.FieldCount; i++ )				//��ȡ��ǰ���е�����
							{
								dr = dt.NewRow();

								dr[0] = Reader.GetName(i);								//ȡ��ָ���ֶε��ֶ�����
								dr[1] = Reader.GetDataTypeName(i);						//ȡ��ָ���ֶε���������
								dr[2] = Convert.ToString(Dt.Rows[i]["ColumnSize"]);		//ȡ��ָ���У�Ҳ����ָ���У����ֶδ�С
								dr[3] = Convert.ToString(Dt.Rows[i]["AllowDBNull"]);	//�����ж��ֶ����Ƿ�ΪNullֵ

								dt.Rows.Add(dr);
							}
							this._ExecuteTimes ++;				//ִ�����������1
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
				throw new Exception( e.Message + "\n SQL��䣺" + this.CommandText );
			}
			finally
			{
				// �ͷ���Դ
				this.Cmd.Parameters.Clear();
				this.Cmd.Dispose();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}		

		}


		/// <summary>
		/// �������ݿ����ּ��ϡ���SQLServer���ݿ�ʹ�ã�
		/// </summary>
		/// <returns></returns>
		public override DataTable GetDataBaseCollection()
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = "sp_databases";		//SQLServerϵͳ�洢���̣��������ݿ����ּ���
				this.Cmd.CommandType = CommandType.StoredProcedure;

				using ( SqlDataAdapter Adapter = new SqlDataAdapter(this.Cmd))
				{	
					using( DataTable dt	= new DataTable())
					{
						Adapter.Fill(dt);
						this._ExecuteTimes ++;				//ִ�����������1
						return dt;
					}
				}
			}
			catch (SqlException e)
			{
				throw new Exception( e.Message + "\n SQL���:" + this.CommandText); 
			}
			finally
			{
				// �ͷ���Դ
				this.Cmd.Dispose();
				this.Cmd.Parameters.Clear();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}


		/// <summary>
		/// ���ص�ǰ���ݿ���ϵͳ���ϡ���SQLServer���ݿ�ʹ�ã����ݿ��������ַ���������
		/// </summary>
		/// <returns></returns>
		public override DataTable GetSystemTableCollection()
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = "sp_tables";			//SQLServerϵͳ�洢���̣��������ݿ�������ּ��ϡ�
				this.Cmd.CommandType = CommandType.StoredProcedure;

				//���ò���
				this.Cmd.Parameters.Add(new SqlParameter("@table_type",SqlDbType.VarChar,100,ParameterDirection.Input,false,0,0,"",DataRowVersion.Default,"'SYSTEM TABLE'"));

				using ( SqlDataAdapter Adapter = new SqlDataAdapter(this.Cmd))
				{	
					using( DataTable dt	= new DataTable())
					{
						Adapter.Fill(dt);
						this._ExecuteTimes ++;				//ִ�����������1
						return dt;
					}
				}
			}
			catch (SqlException e)
			{
				throw new Exception( e.Message + "\n SQL���:" + this.CommandText); 
			}
			finally
			{
				// �ͷ���Դ
				this.Cmd.Dispose();
				this.Cmd.Parameters.Clear();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}


		/// <summary>
		/// ���ص�ǰ���ݿ����û����ϡ���SQLServer���ݿ�ʹ�ã����ݿ��������ַ���������
		/// </summary>
		/// <returns></returns>
		public override DataTable GetUserTableCollection()
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = "sp_tables";			//SQLServerϵͳ�洢���̣��������ݿ�������ּ��ϡ�
				this.Cmd.CommandType = CommandType.StoredProcedure;

				//���ò���
				this.Cmd.Parameters.Add(new SqlParameter("@table_type",SqlDbType.VarChar,100,ParameterDirection.Input,false,0,0,"",DataRowVersion.Default,"'TABLE'"));

				using ( SqlDataAdapter Adapter = new SqlDataAdapter(this.Cmd))
				{	
					using( DataTable dt	= new DataTable())
					{
						Adapter.Fill(dt);
						this._ExecuteTimes ++;				//ִ�����������1
						return dt;
					}
				}
			}
			catch (SqlException e)
			{
				throw new Exception( e.Message + "\n SQL���:" + this.CommandText); 
			}
			finally
			{
				// �ͷ���Դ
				this.Cmd.Dispose();
				this.Cmd.Parameters.Clear();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}


		/// <summary>
		/// ���ص�ǰ���ݿ�����ͼ���ϱ���SQLServer���ݿ�ʹ�ã����ݿ��������ַ���������
		/// </summary>
		/// <returns></returns>
		public override DataTable GetViewCollection()
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = "sp_tables";			//SQLServerϵͳ�洢���̣��������ݿ�������ּ��ϡ�
				this.Cmd.CommandType = CommandType.StoredProcedure;

				//���ò���
				this.Cmd.Parameters.Add(new SqlParameter("@table_type",SqlDbType.VarChar,100,ParameterDirection.Input,false,0,0,"",DataRowVersion.Default,"'VIEW'"));

				using ( SqlDataAdapter Adapter = new SqlDataAdapter(this.Cmd))
				{	
					using( DataTable dt	= new DataTable())
					{
						Adapter.Fill(dt);
						this._ExecuteTimes ++;				//ִ�����������1
						return dt;
					}
				}
			}
			catch (SqlException e)
			{
				throw new Exception( e.Message + "\n SQL���:" + this.CommandText); 
			}
			finally
			{
				// �ͷ���Դ
				this.Cmd.Dispose();
				this.Cmd.Parameters.Clear();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}


		/// <summary>
		/// ���ص�ǰ���ݿ���ĳ������еļ��ϱ���SQLServer���ݿ�ʹ�ã����ݿ��������ַ���������
		/// </summary>
		/// <param name="TableName">����ѯ��</param>
		/// <returns>DataTable</returns>
		public override DataTable GetColumnCollection(string TableName)
		{
			if (TableName == string.Empty ) return null; 
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = "sp_columns";			//SQLServerϵͳ�洢���̣��������ݿ�������ּ��ϡ�
				this.Cmd.CommandType = CommandType.StoredProcedure;

				//���ò���
				this.Cmd.Parameters.Add(new SqlParameter("@table_name",SqlDbType.VarChar,100,ParameterDirection.Input,false,0,0,"",DataRowVersion.Default,TableName));

				using ( SqlDataAdapter Adapter = new SqlDataAdapter(this.Cmd))
				{	
					using( DataTable dt	= new DataTable())
					{
						Adapter.Fill(dt);
						this._ExecuteTimes ++;				//ִ�����������1
						return dt;
					}
				}
			}
			catch (SqlException e)
			{
				throw new Exception( e.Message + "\n SQL���:" + this.CommandText); 
			}
			finally
			{
				// �ͷ���Դ
				this.Cmd.Dispose();
				this.Cmd.Parameters.Clear();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}


		#endregion


		#region DataRow 

		/// <summary>
		/// ��ѯ�����ص�һ�����ݵ������е�ֵ���������򷵻�Null��
		/// </summary>
		/// <returns>DataRow</returns>
		public override DataRow ReadFirstDataRow()
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = this.CommandText;
				this.Cmd.CommandType = this.CommandType;

				//���ò���
				SetUpParameters(this.Cmd,this.IParameters);
	
				//������
				this.Open();
				// ִ�����ݿ����
				using ( SqlDataReader Reader = this.Cmd.ExecuteReader(CommandBehavior.CloseConnection))
				{
					this._ExecuteTimes ++;				//ִ�����������1
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
				throw new Exception( e.Message + "\n SQL��䣺" + this.CommandText );
			}
			finally
			{
				// �ͷ���Դ
				this.Cmd.Parameters.Clear();
				this.Cmd.Dispose();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}


		#endregion


		#region IDataReader

		/// <summary>
		/// ��ȡ���������ݼ���
		/// </summary>
		/// <returns>IDataReader</returns>
		public override IDataReader ExecuteReader()
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = this.CommandText;
				this.Cmd.CommandType = this.CommandType;

				//���ò���
				SetUpParameters(this.Cmd,this.IParameters);
	
				//������
				this.Open();
				// ִ�����ݿ����
				SqlDataReader Reader	= this.Cmd.ExecuteReader(CommandBehavior.CloseConnection);
				this._ExecuteTimes ++;				//ִ�����������1
				return Reader;
			}
			catch( SqlException e )
			{
				throw new Exception( e.Message + "\n SQL��䣺" + this.CommandText );
			}
			finally
			{
				// �ͷ���Դ(ע�⣺SqlDataReader���ܹر�����,�����Ҫ��ʾ�ر�)
				this.Cmd.Parameters.Clear();
				this.Cmd.Dispose();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}

		
		#endregion


		#region Other

		/// <summary>
		/// ���ؽ�����е�һ�е�һ����Ϣ�����ص���һ��string���������򷵻� String.Empty
		/// </summary>
		/// <returns>string</returns>
		public override string ExecuteScalar()
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = this.CommandText;
				this.Cmd.CommandType = this.CommandType;

				//���ò���
				SetUpParameters(this.Cmd,this.IParameters);
	
				//������
				this.Open();
				// ִ�����ݿ����
				using (SqlDataReader Reader	= Cmd.ExecuteReader())
				{
					this._ExecuteTimes ++;				//ִ�����������1
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
				throw new Exception( e.Message + "\n SQL��䣺" + this.CommandText );
			}
			finally
			{
				// �ͷ���Դ
				this.Cmd.Parameters.Clear();
				this.Cmd.Dispose();
				this.CommandText = string.Empty;
				this.CommandType = CommandType.Text;
			}
		}



		/// <summary>
		/// ִ�в�ѯ��䣬���ؼ�¼��(��: SELECT Count(*) FROM [xxx])
		/// </summary>
		/// <returns>û���м�¼����0</returns>
		public override int GetScalarSQL()
		{
			try
			{
				this.Cmd.Connection	 = this.Conn;
				this.Cmd.CommandText = this.CommandText;
				this.Cmd.CommandType = this.CommandType;

				//���ò���
				SetUpParameters(this.Cmd,this.IParameters);
	
				//������
				this.Open();
				// ִ�����ݿ����
				object obj = this.Cmd.ExecuteScalar();
				this._ExecuteTimes ++;				//ִ�����������1

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
				throw new Exception( e.Message + "\n SQL��䣺" + this.CommandText );
			}
			finally
			{
				// �ͷ���Դ
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
