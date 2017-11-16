namespace KuaiCard.ETAPI
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Withdraw;
    using OriginalStudio.Model;
    using OriginalStudio.Model.Withdraw;
    using System;

    public class Withdraw
    {
        private static KuaiCard.BLL.Withdraw.channelwithdraw chalBLL = new KuaiCard.BLL.Withdraw.channelwithdraw();

        public static int Complete(int suppId, string trade_no, bool is_cancel, int status, string amount, string supp_trade_no, string message)
        {
            string str = string.Empty;
            int num = KuaiCard.BLL.distribution.Process(suppId, trade_no, is_cancel, status, amount, supp_trade_no, message, out str);
            if ((num == 0) && (trade_no.Substring(0, 1) == "2"))
            {
                new KuaiCard.BLL.Withdraw.settledAgent().DoNotify(str);
            }
            return num;
        }

        /// <summary>
        /// 提现非代理模式。
        /// </summary>
        /// <param name="itemInfo"></param>
        public static void InitDistribution(SettledInfo itemInfo)
        {
            KuaiCard.Model.distribution model = new KuaiCard.Model.distribution();
            model.trade_no = KuaiCard.BLL.distribution.GenerateTradeNo(1);
            model.mode = 1;     //!!!!!
            model.settledId = new int?(itemInfo.id);
            model.batchNo = 1;
            model.userid = itemInfo.userid;
            model.balance = 0M;
            model.bankCode = itemInfo.BankCode;        //itemInfo.PayeeBank是银行名称
            model.suppid = itemInfo.suppid;
            model.bankName = itemInfo.PayeeBank;
            model.bankBranch = itemInfo.Payeeaddress;
            model.bankAccountName = itemInfo.payeeName;
            model.bankAccount = itemInfo.Account;
            if (itemInfo.charges.HasValue)
            {
                //费用
                KuaiCard.Model.distribution distribution2 = model;
                decimal amount = itemInfo.amount;
                decimal num2 = itemInfo.charges.Value;
                decimal num3 = amount - num2;
                distribution2.amount = num3;
            }
            else
            {
                model.amount = itemInfo.amount;
            }
            KuaiCard.Model.distribution distribution3 = model;
            decimal num4 = itemInfo.charges.Value;
            distribution3.charges = num4;
            model.balance2 = 0;

            /**/
            //Step1:先记录数据库
            if (KuaiCard.BLL.distribution.Add(model) > 0)
            {
                //Step2：调用网关支付
                SellFactory.ReqDistribution(model);
            }
            
        }

        /// <summary>
        /// 提现代理模式
        /// </summary>
        /// <param name="itemInfo"></param>
        public static void InitDistribution2(KuaiCard.Model.Withdraw.settledAgent itemInfo)
        {
            KuaiCard.Model.distribution model = new KuaiCard.Model.distribution();
            model.trade_no = KuaiCard.BLL.distribution.GenerateTradeNo(2);
            model.suppid = itemInfo.suppid;
            model.mode = 2;
            model.settledId = new int?(itemInfo.id);
            model.batchNo = 1;
            model.userid = itemInfo.userid;
            model.balance = 0M;
            model.bankCode = itemInfo.bankCode;
            model.bankName = itemInfo.bankName;
            model.bankBranch = itemInfo.bankBranch;
            model.bankAccountName = itemInfo.bankAccountName;
            model.bankAccount = itemInfo.bankAccount;
            model.amount = itemInfo.amount;
            model.charges = itemInfo.charge;
            model.balance2 = 0;
            if (KuaiCard.BLL.distribution.Add(model) > 0)
            {
                SellFactory.ReqDistribution(model);
            }
        }


    }
}

