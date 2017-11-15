using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// API类型
/// </summary>
public enum PayApi
{
    订单日线 = 1,       //已用
    每日汇总 = 2,
    客户汇总 = 4,       //每个客户订单汇总
    分时成交 = 9,       //已用
    客户列表 = 10,       //已用
    订单提交汇总 = 12,     //看提交订单的情况
    通道使用量统计 = 14    //通道使用量统计
}