namespace KuaiCard.Model
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class UserSupplierInfo
    {
        public UserSupplierInfo()
        {
            this.ChannelTypeList = new List<string>();
        }

        public int ID { get; set; }

        public int UserID { get; set; }

        public int SupplierCode { get; set; }

        public string Name { get; set; }

        public string PUserID { get; set; }

        public string PUserKey { get; set; }

        public string PUserName { get; set; }

        public string PUserID1 { get; set; }

        public string PUserKey1 { get; set; }

        public string PUserID2 { get; set; }

        public string PUserKey2 { get; set; }

        public string PostBankUrl { get; set; }

        public string JumpUrl { get; set; }

        public string distributionUrl { get; set; }

        public string Desc { get; set; }

        public int Sort { get; set; }

        public int Active { get; set; }

        public string ExtParm1 { get; set; }

        public string ExtParm2 { get; set; }

        public string ExtParm3 { get; set; }

        public string ExtParm4 { get; set; }

        public List<String> ChannelTypeList { get; set; }
    }
}

