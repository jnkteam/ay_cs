using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

/// <summary>
/// SQLServerHelper 的摘要说明
/// </summary>
public class SQLServerHelper
{
	public SQLServerHelper()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 静态连接字符串。
    /// </summary>
    public static readonly string connString = ConfigurationSettings.AppSettings["paycon"].ToString();
        
}