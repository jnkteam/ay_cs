using System;
using System.Collections;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Reflection;
using System.Security;
using System.Xml;
using System.Collections.Generic;



namespace General.DataProvider
{
	/// <summary>
	/// DataProvider �����ݿ�����Ľӿ��ࡣ
	/// </summary>
	/// �ýӿ����ṩ�˻��������ݿ�������Լ�����������չ�����ݿ�������ͣ�Ŀǰ�Ѿ�֧�ֵ����ݿ������У�
	/// <remarks>
	/// <ul>
	/// <li>Odbc�� ������Odbc�������ӵ����ݿ⣬��Mysql��</li>
	/// <li>OleDb�� ������OleDb�������ӵ����ݿ⣬��Access��Excel��</li>
	/// <li>SqlClient�� ����Sql Server���ݿ�</li>
	/// <li>Oracle�� ����Oracle���ݿ�</li>
	/// </ul>
	/// </remarks>
	/// <example>
	/// <code>
	/// <![CDATA[
	/// using System;
	/// using System.Data;
	/// using Seaskyer.AdoProvider;
	/// 
	/// public class Class1
	/// {
	///		public void GetDataTable()
	///		{
	///			DataProvider dp = null;
	///			
	///			AdoProviderType apt = AdoProviderType.OLEDB;		// �������ݿ�����
	///			string connectionString = "Database/Global.mdb";	// �������ݿ�������ַ���
	///			
	///			// ʵ�������ݿ��������
	///			dp = DataProvider.Instance(apt, connectionString);
	///			
	///			string strCmd	= "SELECT * FROM [UserInfo]";
	///			DataTable dt	= dp.DataTableSQL(strCmd);
	///			
	///			Response.Write(dt.Rows.Count);
	///			
	///			// �ͷ���Դ
	///			dt.Clear();
	///			dt.Dispose();
	///			dp.Dispose();
	///		}
	/// }
	/// ]]>
	/// </code>
	/// </example>
	public abstract class DataProvider:IDisposable
	{
		public DataProvider()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		/// <summary>
		/// ��������
		/// </summary>
		~DataProvider()
		{
			//�Զ�ִ��
		}

		/*	������£�
		 *	070122��ÿ��������ʹ��using()���飬�����Զ��ͷ���Դ�����Ǳ���ʵ��IDisposable�ӿ�
		 *	��Ҫ���⣺ʲôʱ��ҪClose()  �� Dispose()
		 *	ÿ��ִ��SQL�����ϣ�����Close()�ر����ݿ����ӣ�������ʾ�رգ�����������һ��������
		 *	��ִ�б�����ʱ���ô����ݿ����ӣ���ʡʱ�䡣
		 * 
		 *	�ײ��������SQL������
		 * */


		#region ====================˽�б���======================

		private int					_QueryTimes			= 0;				//��ǰ���ݿ����ִ�в�ѯ�Ĵ���
		private int					_ConnectTimes		= 0;				//�������ݿ�Ĵ���
		private string				_CommandText		= string.Empty;		//����
		private Hashtable			_Parameter			= new Hashtable();
		private Hashtable			_ParameterReturn	= new Hashtable();
		private object[]			_ParameterValues;						//����ֵ����
		private IDataParameter[]	_Parameters;			
		private CommandType			_CommandType		= CommandType.Text;	//�����������(Ĭ�ϣ�Text)
		private string				_ConnectionString   = string.Empty;		//���ݿ������ַ���
		protected int				_ExecuteTimes		= 0;				//����ִ�д���
        private List<IDbDataParameter> _IParameters = new List<IDbDataParameter>();		//����ֵ����
		#endregion

		#region ====================��̬����======================

		/// <summary>
		/// ʵ����������Ľӿ�
		/// </summary>
		/// <param name="dpt">��������������</param>
		/// <param name="conncetionString">���ݿ����ӵ��ַ���</param>
		/// <returns></returns>
		public static DataProvider Instance(DataProviderType dpt,string conncetionString)
		{
			string ProviderClassName = string.Empty;

			switch( dpt )
			{
				case DataProviderType.ODBC			:
					ProviderClassName = "General.DataProvider.OdbcDataProvider";
					
					break;

				case DataProviderType.SQLCLIENT		:
					ProviderClassName = "General.DataProvider.SqlDataProvider";

					break;

				case DataProviderType.OLEDB			:
					ProviderClassName = "General.DataProvider.OleDbDataProvider";

					break;

				case DataProviderType.ORACLE		:
					ProviderClassName = "General.DataProvider.OracleDataProvider";

					break;

			}

			try
			{
				// ��ȡ�����������
				Type deriveType = Type.GetType(ProviderClassName);			//������(ǰ���Ǹ����ڳ���������ҵ�)

				// �������캯��������������
				Type[] types = new Type[1];
				types[0] = typeof(string);

				// ��ȡָ������������Ĺ��캯��
				ConstructorInfo deriveCI	=	 deriveType.GetConstructor(types);	
				//���� Type �Ŀ����飬���Ի�ȡ���������Ĺ��캯���� 

				/* =====================ConstructorInfo�����: ===========================
				 * ConstructorInfo:�����๹�캯�������Բ��ṩ�Թ��캯��Ԫ���ݵķ���Ȩ��
				 * ͨ���� ConstructorInfo ���� Invoke �������������� ConstructorInfo ��
				 * �� Type ����� GetConstructors �� GetConstructor �������صġ�
				 * =======================================================================
				 * */

				// ָ�����������ֵ
				object[] paramArray = new object[1];
				paramArray[0]		= conncetionString;

				return (DataProvider)deriveCI.Invoke(paramArray);  
			
				/* ======================����==============================================
				 * ConstructorInfo.Invoke ����:���ø�ʵ����ӳ�Ĺ��캯����
				 * �˴�������:���þ���ָ��������ʵ������ӳ�Ĺ��캯������Ϊ�����õĲ����ṩ
				 * Ĭ��ֵ��
				 * paramArray:paramArray ������Ԫ�صĸ��������ͺ�˳��Ӧ���ʵ������ӳ�Ĺ���
				 * �����Ĳ����ĸ��������ͺ�˳����ͬ��
				 * ========================================================================
				 * Invoke:����*/
			}
			catch(Exception err)
			{
				throw new Exception("����������" + err.Message.ToString());
			}
		}


		/// <summary>
		/// �õ�ʵ������������ռ�
		/// </summary>
		/// <param name="dpt"></param>
		/// <returns></returns>
		public static string GetSubClassName(DataProviderType dpt)
		{
			string strResult = string.Empty;

			switch (dpt)
			{
				case DataProviderType.ODBC		:
					strResult = "General.DataBaseProvider.OdbcDataProvider";
					break;

				case DataProviderType.OLEDB		:
					strResult = "General.DataBaseProvider.OleDbDataProvider";
					break;

				case DataProviderType.SQLCLIENT	:
					strResult = "General.DataBaseProvider.SqlDataProvider";
					break;

				case DataProviderType.ORACLE	:
					strResult = "General.DataBaseProvider.OracleDataProvider";
					break;

				case DataProviderType.XML		:
					strResult = "General.DataBaseProvider.XmlDataProvider";
					break;

				default							:
					strResult = "General.DataProvider.OleDbDataProvider";
					break;
			}

			return strResult;
		}



		#endregion

		#region ====================��������======================
        
		/// <summary>
		/// ��ǰ���ݿ����ִ�в�ѯ�Ĵ���
		/// </summary>
		public int QueryTimes
		{
			get{	return this._QueryTimes;	}
			set{	this._QueryTimes = value;	}
		}


		/// <summary>
		/// �������ݿ�Ĵ���
		/// </summary>
		public int ConnectTimes
		{
			get { return this._ConnectTimes; }
			set { this._ConnectTimes = value; }
		}


		/// <summary>
		/// ����ִ�д�����ֻ������
		/// </summary>
		public int ExecuteTimes
		{
			get { return this._ExecuteTimes; }
		}


		/// <summary>
		/// ���ݿ��������洢������
		/// </summary>
		public string CommandText
		{
			get{	return this._CommandText;	}
			set{	this._CommandText = value;	}
		}
		

		/// <summary>
		/// ��ǰ�Ĳ������ͣ��洢���̻���SQL��䣩
		/// </summary>
		public CommandType CommandType
		{
			get{	return this._CommandType;	}
			set{	this._CommandType = value;	}
		}


		/// <summary>
		/// ����ֵ���飨�������������Ĳ������Ӧ��
		/// </summary>
		public Object[] ParameterValues
		{
			set {_ParameterValues = value;}
			get {return this._ParameterValues;}
		}


		/// <summary>
		/// �����������顣�Ѿ�ʵ��IDataParameter�ӿڡ�
		/// </summary>
        public List<IDbDataParameter> IParameters
		{
            set { this._IParameters = value; }
            get { return _IParameters; }
		}

        /// <summary>
        /// �����������顣�Ѿ�ʵ��IDataParameter�ӿڡ�
        /// </summary>
        //public IDataParameter[] Parameters
        //{
        //    set { _Parameters = value; }
        //    get { return _Parameters; }
        //}


		/// <summary>
		/// ���ݿ������ַ���(�������ͣ�ֻ������̳�)
		/// </summary>
		protected string ConnectionString
		{
			set { this._ConnectionString = value; }
			get { return this._ConnectionString; }
		}


		#endregion

		#region ===============����������ӿڵ�ʵ��===============
		

		#region ���ݿ����
		/// <summary>
		/// �����ݿ�����
		/// </summary>
		public abstract void Open();

		/// <summary>
		/// �ر����ݿ�����
		/// </summary>
		public abstract void Close();

		/// <summary>
		/// ��������
		/// </summary>
		public abstract void BeginTrans();

		/// <summary>
		/// �ύ����
		/// </summary>
		public abstract void CommitTrans();

		/// <summary>
		/// �ع�����
		/// </summary>
		public abstract void RollbackTrans();

		/// <summary>
		/// ��ȡ��ǰ���ݿ������״̬
		/// </summary>
		/// <returns>ConnectionState</returns>
		public abstract ConnectionState state();

		/// <summary>
		/// �ͷ�����������Դ
		/// </summary>
		public abstract void Dispose();

		/// <summary>
		/// ִ��ָ����SQL��䣨����ӡ����¡�ɾ���Ȳ�����
		/// </summary>
		/// <returns>int:��Ӱ�������</returns>
		public abstract int ExecuteNonQuery();

		/// <summary>
		/// ��ȡ��¼��
		/// </summary>
		/// <returns>int</returns>
		public abstract int GetScalarSQL();

		/// <summary>
		/// ���ؽ�����е�һ�е�һ����Ϣ�����ص���һ��string���������򷵻� String.Empty
		/// </summary>
		/// <returns>string</returns>
		public abstract string ExecuteScalar();

		/// <summary>
		/// ��ѯ�����ص�һ�����ݵ������е�ֵ���������򷵻�Null��
		/// </summary>
		/// <returns>DataRow</returns>
		public abstract DataRow ReadFirstDataRow();

		/// <summary>
		/// ��ȡ���������ݼ���
		/// </summary>
		/// <returns>IDataReader</returns>
		public abstract IDataReader ExecuteReader();

		/// <summary>
		/// ��ȡ�����ݽṹ��Ϣ
		/// </summary>
		/// <returns>DataTable</returns>
		public abstract DataTable GetSchemaTable();

		/// <summary>
		/// ʹ�� DataAdapter ��ȡ���ݷ���Ϊ DataTable
		/// </summary>
		/// <returns>DataTable</returns>
		public abstract DataTable ExecuteDataTable();

		#region ���SQLServer���ݿ�

		/// <summary>
		/// �������ݿ����ּ��ϡ���SQLServer���ݿ�ʹ�ã�
		/// </summary>
		/// <returns>DataTable</returns>
		public abstract DataTable GetDataBaseCollection();

		/// <summary>
		/// ���ص�ǰ���ݿ����û����ϡ���SQLServer���ݿ�ʹ�ã����ݿ��������ַ���������
		/// </summary>
		/// <returns></returns>
		public abstract DataTable GetUserTableCollection();

		/// <summary>
		/// ���ص�ǰ���ݿ���ϵͳ���ϡ���SQLServer���ݿ�ʹ�ã����ݿ��������ַ���������
		/// </summary>
		/// <returns>DataTable</returns>
		public abstract DataTable GetSystemTableCollection();

		/// <summary>
		/// ���ص�ǰ���ݿ�����ͼ���ϱ���SQLServer���ݿ�ʹ�ã����ݿ��������ַ���������
		/// </summary>
		/// <returns>DataTable</returns>
		public abstract DataTable GetViewCollection();

		/// <summary>
		/// ���ص�ǰ���ݿ���ĳ������еļ��ϱ���SQLServer���ݿ�ʹ�ã����ݿ��������ַ���������
		/// </summary>
		/// <param name="TableName">����ѯ��</param>
		/// <returns>DataTable</returns>
		public abstract DataTable GetColumnCollection(string TableName);

		#endregion

		/// <summary>
		/// ʹ�� DataAdapter ��ȡ���ݷ���Ϊ DataSet
		/// </summary>
		/// <returns>DataSet</returns>
		public abstract DataSet ExecuteDataset();

		/// <summary>
		/// ʹ�� DataAdapter ��ȡ���ݷ���Ϊ DataSet(���ڷ�ҳ)
		/// </summary>
		/// <param name="startRecord">��ʼ��¼������(���ӵڼ�����ʼ��ʾ)</param>
		/// <param name="maxRecord">��ʾ���ļ�¼��</param>
		/// <returns></returns>
		public abstract DataSet ExecuteDataset(int startRecord,int maxRecord);

		/// <summary>
		/// ����Ϊ DataView(��������)
		/// </summary>
		/// <returns></returns>
		public abstract DataView ExectuteDataView();


		/// <summary>
		/// �����ݿ������ͼ���ʽ���ֶ�
		/// </summary>
		/// <param name="strSQL"></param>
		/// <param name="fs"></param>
		///public abstract void ExecuteSqlInsertImg(string strSQL,byte[] fs);

		#endregion



		#endregion
	}
}
