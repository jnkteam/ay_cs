namespace KuaiCard.BLL.Tools
{
    using KuaiCardLib.Web;
    using System;
    using System.Text;
    using System.Xml;

    public sealed class idcards
    {
        public static IdcardInfo GetIdCardInfo(string id)
        {
            IdcardInfo info = new IdcardInfo();
            try
            {
                Encoding encoding = Encoding.GetEncoding("GBK");
                string postData = "type=id&q=" + id;
                string url = "http://www.youdao.com/smartresult-xml/search.s";
                string xml = WebClientHelper.GetString(url, postData, "GET", encoding, 0x1388);
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);
                string innerText = document.GetElementsByTagName("code")[0].InnerText;
                string str5 = document.GetElementsByTagName("location")[0].InnerText;
                string str6 = document.GetElementsByTagName("birthday")[0].InnerText;
                string str7 = document.GetElementsByTagName("gender")[0].InnerText;
                info.code = innerText;
                info.location = str5;
                info.birthday = str6;
                info.gender = (str7 == "m") ? "男" : "女";
                return info;
            }
            catch
            {
                return null;
            }
        }
    }
}

