using System;
using System.Xml.Serialization;

namespace Aop.Api.Domain
{
    /// <summary>
    /// MybankCreditLoantradeLoanarCreateModel Data Structure.
    /// </summary>
    [Serializable]
    public class MybankCreditLoantradeLoanarCreateModel : AopObject
    {
        /// <summary>
        /// 收款方参与机构码
        /// </summary>
        [XmlElement("account_fin_code")]
        public string AccountFinCode { get; set; }

        /// <summary>
        /// 收款方机构名称
        /// </summary>
        [XmlElement("account_fin_name")]
        public string AccountFinName { get; set; }

        /// <summary>
        /// 对公对私
        /// </summary>
        [XmlElement("account_fin_type")]
        public string AccountFinType { get; set; }

        /// <summary>
        /// 收款方名称
        /// </summary>
        [XmlElement("account_name")]
        public string AccountName { get; set; }

        /// <summary>
        /// 收款方账户号
        /// </summary>
        [XmlElement("account_no")]
        public string AccountNo { get; set; }

        /// <summary>
        /// 收款方账户类型
        /// </summary>
        [XmlElement("account_type")]
        public string AccountType { get; set; }

        /// <summary>
        /// 2088开头的支付宝数字ID
        /// </summary>
        [XmlElement("alipay_id")]
        public string AlipayId { get; set; }

        /// <summary>
        /// 支用金额，默认人民币，精确到小数点两位，单位元
        /// </summary>
        [XmlElement("apply_amt")]
        public string ApplyAmt { get; set; }

        /// <summary>
        /// 业务单据号
        /// </summary>
        [XmlElement("bsn_no")]
        public string BsnNo { get; set; }

        /// <summary>
        /// 授信编号
        /// </summary>
        [XmlElement("credit_no")]
        public string CreditNo { get; set; }

        /// <summary>
        /// 客群
        /// </summary>
        [XmlElement("cust_group")]
        public string CustGroup { get; set; }

        /// <summary>
        /// 放款账户渠道
        /// </summary>
        [XmlElement("grant_channel")]
        public string GrantChannel { get; set; }

        /// <summary>
        /// 经营行业
        /// </summary>
        [XmlElement("industry")]
        public string Industry { get; set; }

        /// <summary>
        /// 参与者
        /// </summary>
        [XmlElement("ip_id")]
        public string IpId { get; set; }

        /// <summary>
        /// 参与者会员角色ID
        /// </summary>
        [XmlElement("ip_role_id")]
        public string IpRoleId { get; set; }

        /// <summary>
        /// BC政策码
        /// </summary>
        [XmlElement("loan_policy_code")]
        public string LoanPolicyCode { get; set; }

        /// <summary>
        /// 贷款期限
        /// </summary>
        [XmlElement("loan_term")]
        public long LoanTerm { get; set; }

        /// <summary>
        /// 贷款期限单位
        /// </summary>
        [XmlElement("loan_term_unit")]
        public string LoanTermUnit { get; set; }

        /// <summary>
        /// 销售产品编码
        /// </summary>
        [XmlElement("pd_code")]
        public string PdCode { get; set; }

        /// <summary>
        /// 销售产品版本号
        /// </summary>
        [XmlElement("pd_version")]
        public string PdVersion { get; set; }

        /// <summary>
        /// 还款方式
        /// </summary>
        [XmlElement("repay_mode")]
        public string RepayMode { get; set; }

        /// <summary>
        /// 请求流水号，用于幂等控制.以"ipRoleId_"开头
        /// </summary>
        [XmlElement("request_id")]
        public string RequestId { get; set; }

        /// <summary>
        /// 签名值
        /// </summary>
        [XmlElement("sign")]
        public string Sign { get; set; }

        /// <summary>
        /// 转账备注
        /// </summary>
        [XmlElement("trans_memo")]
        public string TransMemo { get; set; }
    }
}
