namespace KuaiCard.Model.Order
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Text;

    [Serializable]
    public class OrderCardInfo : OrderBase
    {
        private decimal _faceValue = 0m;

        private byte _cardversion = 1;

        public string cardNo
        {
            get;
            set;
        }

        public int cardnum
        {
            get;
            set;
        }

        public string cardPwd
        {
            get;
            set;
        }

        public int cardType
        {
            get;
            set;
        }

        public byte cardversion
        {
            get
            {
                return this._cardversion;
            }
            set
            {
                this._cardversion = value;
            }
        }

        public string Desc
        {
            get;
            set;
        }

        public decimal faceValue
        {
            get
            {
                return this._faceValue;
            }
            set
            {
                this._faceValue = value;
            }
        }

        public int ismulticard
        {
            get;
            set;
        }

        public byte makeup
        {
            get;
            set;
        }

        public byte method
        {
            get;
            set;
        }

        public string ovalue
        {
            get;
            set;
        }

        public string resultcode
        {
            get;
            set;
        }

        public string returnopstate
        {
            get
            {
                string str3;
                if (!string.IsNullOrEmpty(base.opstate))
                {
                    StringBuilder _returnop = new StringBuilder();
                    string str = base.opstate;
                    char[] chrArray = new char[]
					{
						'|'
					};
                    string[] arr = str.Split(chrArray);
                    for (int i = 0; i < arr.Length; i++)
                    {
                        string str2 = arr[i];
                        chrArray = new char[]
						{
							':'
						};
                        string[] arr2 = str2.Split(chrArray);
                        if (arr2.Length == 2)
                        {
                            if (_returnop.Length != 0)
                            {
                                _returnop.AppendFormat(",opstate={0}", arr2[1]);
                            }
                            else
                            {
                                _returnop.AppendFormat("opstate={0}", arr2[1]);
                            }
                        }
                    }
                    str3 = _returnop.ToString();
                }
                else
                {
                    str3 = "opstate=-1";
                }
                return str3;
            }
        }

        public string returnovalue
        {
            get
            {
                string empty;
                if (!string.IsNullOrEmpty(base.opstate))
                {
                    empty = ((!this.ovalue.EndsWith(",")) ? this.ovalue : this.ovalue.Substring(0, this.ovalue.Length - 1));
                }
                else
                {
                    empty = string.Empty;
                }
                return empty;
            }
        }

        public string userViewMsg
        {
            get;
            set;
        }

        public byte withhold_type
        {
            get;
            set;
        }

        public decimal withholdAmt
        {
            get;
            set;
        }

        public OrderCardInfo()
        {
            this.makeup = 0;
        }

        public OrderCardInfo(string serverId, string userId, string chanel)
        {
            Random random = new Random(this.GetRandomSeed(serverId + userId + chanel));
            string str = DateTime.Now.ToString("yyMMddHHmmssff");
            base.orderid = str + "0" + random.Next(1000).ToString("0000");
        }

        private int GetRandomSeed(string factor)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(factor);
            new RNGCryptoServiceProvider().GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}

