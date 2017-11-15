namespace OriginalStudio.Model
{
    using System;

    /// <summary>
    /// 产品类
    /// </summary>
    [Serializable]
    public class ProductInfo
    {
        public string ID { get; set; }

        public string ProdName { get; set; }

        public string ProdDesc { get; set; }

        public string Price { get; set; }

        public string ShortName { get; set; }

        public string DispUrl { get; set; }

        public string StoreName { get; set; }

        public string StoreID { get; set; }

        public string TerminalId { get; set; }
    }
}

