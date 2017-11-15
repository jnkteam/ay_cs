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
	/// DataProvider 多数据库操作的接口类。
	/// </summary>
	/// 该接口类提供了基本的数据库操作，以及可以无限拓展的数据库操作类型，目前已经支持的数据库类型有：
	/// <remarks>
	/// <ul>
	/// <li>Odbc： 操作以Odbc驱动连接的数据库，如Mysql等</li>
	/// <li>OleDb： 操作以OleDb驱动连接的数据库，如Access、Excel等</li>
	/// <li>SqlClient： 操作Sql Server数据库</li>
	/// <li>Oracle： 操作Oracle数据库</li>
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
	///			AdoProviderType apt = AdoProviderType.OLEDB;		// 设置数据库类型
	///			string connectionString = "Database/Global.mdb";	// 设置数据库的连接字符串
	///			
	///			// 实例化数据库操作对象
	///			dp = DataProvider.Instance(apt, connectionString);
	///			
	///			string strCmd	= "SELECT * FROM [UserInfo]";
	///			DataTable dt	= dp.DataTableSQL(strCmd);
	///			
	///			Response.Write(dt.Rows.Count);
	///			
	///			// 释放资源
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
			// TODO: 在此处添加构造函数逻辑
			//
		}

		/// <summary>
		/// 析构函数
		/// </summary>
		~DataProvider()
		{
			//自动执行
		}

		/*	该类更新：
		 *	070122在每个子类中使用using()语句块，可以自动释放资源，但是必须实现IDisposable接口
		 *	主要问题：什么时候要Close()  和 Dispose()
		 *	每次执行SQL语句完毕，不用Close()关闭数据库连接，除非显示关闭，这样可以在一条语句后面
		 *	再执行别的语句时候不用打开数据库连接，节省时间。
		 * 
		 *	底层操作捕获SQL语句错误
		 * */


		#region ====================私有变量======================

		private int					_QueryTimes			= 0;				//当前数据库对象执行查询的次数
		private int					_ConnectTimes		= 0;				//连接数据库的次数
		private string				_CommandText		= string.Empty;		//命令
		private Hashtable			_Parameter			= new Hashtable();
		private Hashtable			_ParameterReturn	= new Hashtable();
		private object[]			_ParameterValues;						//参数值数组
		private IDataParameter[]	_Parameters;			
		private CommandType			_CommandType		= CommandType.Text;	//命令对象类型(默认：Text)
		private string				_ConnectionString   = string.Empty;		//数据库连接字符串
		protected int				_ExecuteTimes		= 0;				//命令执行次数
        private List<IDbDataParameter> _IParameters = new List<IDbDataParameter>();		//参数值数组
		#endregion

		#region ====================静态方法======================

		/// <summary>
		/// 实例化派生类的接口
		/// </summary>
		/// <param name="dpt">数据驱动的类型</param>
		/// <param name="conncetionString">数据库连接的字符串</param>
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
				// 获取派生类的类型
				Type deriveType = Type.GetType(ProviderClassName);			//！！！(前提是该类在程序的中能找到)

				// 构建构造函数参数类型数组
				Type[] types = new Type[1];
				types[0] = typeof(string);

				// 获取指定参数派生类的构造函数
				ConstructorInfo deriveCI	=	 deriveType.GetConstructor(types);	
				//类型 Type 的空数组，用以获取不带参数的构造函数。 

				/* =====================ConstructorInfo类介绍: ===========================
				 * ConstructorInfo:发现类构造函数的特性并提供对构造函数元数据的访问权。
				 * 通过对 ConstructorInfo 调用 Invoke 来创建对象，其中 ConstructorInfo 是
				 * 由 Type 对象的 GetConstructors 或 GetConstructor 方法返回的。
				 * =======================================================================
				 * */

				// 指定派生类参数值
				object[] paramArray = new object[1];
				paramArray[0]		= conncetionString;

				return (DataProvider)deriveCI.Invoke(paramArray);  
			
				/* ======================补充==============================================
				 * ConstructorInfo.Invoke 方法:调用该实例反映的构造函数。
				 * 此处带参数:调用具有指定参数的实例所反映的构造函数，并为不常用的参数提供
				 * 默认值。
				 * paramArray:paramArray 数组中元素的个数、类型和顺序应与该实例所反映的构造
				 * 函数的参数的个数、类型和顺序相同。
				 * ========================================================================
				 * Invoke:调用*/
			}
			catch(Exception err)
			{
				throw new Exception("错误描述：" + err.Message.ToString());
			}
		}


		/// <summary>
		/// 得到实例化类的命名空间
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

		#region ====================公共属性======================
        
		/// <summary>
		/// 当前数据库对象执行查询的次数
		/// </summary>
		public int QueryTimes
		{
			get{	return this._QueryTimes;	}
			set{	this._QueryTimes = value;	}
		}


		/// <summary>
		/// 连接数据库的次数
		/// </summary>
		public int ConnectTimes
		{
			get { return this._ConnectTimes; }
			set { this._ConnectTimes = value; }
		}


		/// <summary>
		/// 命令执行次数。只读属性
		/// </summary>
		public int ExecuteTimes
		{
			get { return this._ExecuteTimes; }
		}


		/// <summary>
		/// 数据库操作语句或存储过程名
		/// </summary>
		public string CommandText
		{
			get{	return this._CommandText;	}
			set{	this._CommandText = value;	}
		}
		

		/// <summary>
		/// 当前的操作类型（存储过程或者SQL语句）
		/// </summary>
		public CommandType CommandType
		{
			get{	return this._CommandType;	}
			set{	this._CommandType = value;	}
		}


		/// <summary>
		/// 参数值数组（必须与命令对象的参数相对应）
		/// </summary>
		public Object[] ParameterValues
		{
			set {_ParameterValues = value;}
			get {return this._ParameterValues;}
		}


		/// <summary>
		/// 参数对象数组。已经实现IDataParameter接口。
		/// </summary>
        public List<IDbDataParameter> IParameters
		{
            set { this._IParameters = value; }
            get { return _IParameters; }
		}

        /// <summary>
        /// 参数对象数组。已经实现IDataParameter接口。
        /// </summary>
        //public IDataParameter[] Parameters
        //{
        //    set { _Parameters = value; }
        //    get { return _Parameters; }
        //}


		/// <summary>
		/// 数据库连接字符串(保护类型，只能子类继承)
		/// </summary>
		protected string ConnectionString
		{
			set { this._ConnectionString = value; }
			get { return this._ConnectionString; }
		}


		#endregion

		#region ===============用于派生类接口的实现===============
		

		#region 数据库操作
		/// <summary>
		/// 打开数据库连接
		/// </summary>
		public abstract void Open();

		/// <summary>
		/// 关闭数据库连接
		/// </summary>
		public abstract void Close();

		/// <summary>
		/// 开启事务
		/// </summary>
		public abstract void BeginTrans();

		/// <summary>
		/// 提交事务
		/// </summary>
		public abstract void CommitTrans();

		/// <summary>
		/// 回滚事务
		/// </summary>
		public abstract void RollbackTrans();

		/// <summary>
		/// 获取当前数据库的连接状态
		/// </summary>
		/// <returns>ConnectionState</returns>
		public abstract ConnectionState state();

		/// <summary>
		/// 释放数据连接资源
		/// </summary>
		public abstract void Dispose();

		/// <summary>
		/// 执行指定的SQL语句（如添加、更新、删除等操作）
		/// </summary>
		/// <returns>int:受影响的行数</returns>
		public abstract int ExecuteNonQuery();

		/// <summary>
		/// 提取记录数
		/// </summary>
		/// <returns>int</returns>
		public abstract int GetScalarSQL();

		/// <summary>
		/// 返回结果集中第一行第一列信息。返回的是一个string，不存在则返回 String.Empty
		/// </summary>
		/// <returns>string</returns>
		public abstract string ExecuteScalar();

		/// <summary>
		/// 查询并返回第一行数据的所有列的值，不存在则返回Null。
		/// </summary>
		/// <returns>DataRow</returns>
		public abstract DataRow ReadFirstDataRow();

		/// <summary>
		/// 读取并返回数据集合
		/// </summary>
		/// <returns>IDataReader</returns>
		public abstract IDataReader ExecuteReader();

		/// <summary>
		/// 读取表数据结构信息
		/// </summary>
		/// <returns>DataTable</returns>
		public abstract DataTable GetSchemaTable();

		/// <summary>
		/// 使用 DataAdapter 提取数据返回为 DataTable
		/// </summary>
		/// <returns>DataTable</returns>
		public abstract DataTable ExecuteDataTable();

		#region 针对SQLServer数据库

		/// <summary>
		/// 返回数据库名字集合。（SQLServer数据库使用）
		/// </summary>
		/// <returns>DataTable</returns>
		public abstract DataTable GetDataBaseCollection();

		/// <summary>
		/// 返回当前数据库里用户表集合。（SQLServer数据库使用，数据库由连接字符串决定）
		/// </summary>
		/// <returns></returns>
		public abstract DataTable GetUserTableCollection();

		/// <summary>
		/// 返回当前数据库里系统表集合。（SQLServer数据库使用，数据库由连接字符串决定）
		/// </summary>
		/// <returns>DataTable</returns>
		public abstract DataTable GetSystemTableCollection();

		/// <summary>
		/// 返回当前数据库里视图集合表。（SQLServer数据库使用，数据库由连接字符串决定）
		/// </summary>
		/// <returns>DataTable</returns>
		public abstract DataTable GetViewCollection();

		/// <summary>
		/// 返回当前数据库里某个表的列的集合表。（SQLServer数据库使用，数据库由连接字符串决定）
		/// </summary>
		/// <param name="TableName">待查询表</param>
		/// <returns>DataTable</returns>
		public abstract DataTable GetColumnCollection(string TableName);

		#endregion

		/// <summary>
		/// 使用 DataAdapter 提取数据返回为 DataSet
		/// </summary>
		/// <returns>DataSet</returns>
		public abstract DataSet ExecuteDataset();

		/// <summary>
		/// 使用 DataAdapter 提取数据返回为 DataSet(用于分页)
		/// </summary>
		/// <param name="startRecord">起始记录的索引(即从第几条开始显示)</param>
		/// <param name="maxRecord">显示最大的记录数</param>
		/// <returns></returns>
		public abstract DataSet ExecuteDataset(int startRecord,int maxRecord);

		/// <summary>
		/// 返回为 DataView(用于排序)
		/// </summary>
		/// <returns></returns>
		public abstract DataView ExectuteDataView();


		/// <summary>
		/// 向数据库里插入图像格式的字段
		/// </summary>
		/// <param name="strSQL"></param>
		/// <param name="fs"></param>
		///public abstract void ExecuteSqlInsertImg(string strSQL,byte[] fs);

		#endregion



		#endregion
	}
}
