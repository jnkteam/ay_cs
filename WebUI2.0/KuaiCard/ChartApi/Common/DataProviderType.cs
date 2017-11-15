using System;


namespace General.DataProvider
{
	/// <summary>
	/// ���ݿ����͡�
	/// </summary>
	public enum DataProviderType:int
	{
		/// <summary>
		/// SQLCLIENT�������ݿ�
		/// </summary>
		SQLCLIENT = 1,


		/// <summary>
		/// OLEDB�������ݿ�
		/// </summary>
		OLEDB = 2,


		/// <summary>
		/// ODBC�������ݿ�
		/// </summary>
		ODBC = 3,

		/// <summary>
		/// ORACLE�������ݿ�
		/// </summary>
		ORACLE = 4,

		/// <summary>
		/// XML����
		/// </summary>
		XML = 5		

	}
}
