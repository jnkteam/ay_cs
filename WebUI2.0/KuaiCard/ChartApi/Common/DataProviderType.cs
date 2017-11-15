using System;


namespace General.DataProvider
{
	/// <summary>
	/// 数据库类型。
	/// </summary>
	public enum DataProviderType:int
	{
		/// <summary>
		/// SQLCLIENT类型数据库
		/// </summary>
		SQLCLIENT = 1,


		/// <summary>
		/// OLEDB类型数据库
		/// </summary>
		OLEDB = 2,


		/// <summary>
		/// ODBC类型数据库
		/// </summary>
		ODBC = 3,

		/// <summary>
		/// ORACLE类型数据库
		/// </summary>
		ORACLE = 4,

		/// <summary>
		/// XML类型
		/// </summary>
		XML = 5		

	}
}
