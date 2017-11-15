using System;
using System.Collections.Generic;
using System.Text;

namespace KuaiCard.Model.User
{
    /// <summary>
    /// 平安银行商户资料
    /// </summary>
    public class PayhSubmerchant
    {
        public string external_id { get; set; }
        public string name { get; set; }
        public string alias_name { get; set; }
        public string service_phone { get; set; }
        public string contact_name { get; set; }
        public string contact_phone { get; set; }
        public string contact_mobile { get; set; }
        public string contact_email { get; set; }
        public string category_id { get; set; }
        public string memo { get; set; }
        public string id_card_name { get; set; }
        public string id_card_num { get; set; }
        public string store_address { get; set; }
        public string id_card_hand_img_url { get; set; }
        public string store_front_img_url { get; set; }
        public string business_license_img_url { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string card_no { get; set; }
        public string business_license { get; set; }
        public string business_license_type { get; set; }
        public string contact_type { get; set; }
    }
}
