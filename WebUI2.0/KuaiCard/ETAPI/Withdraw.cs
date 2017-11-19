namespace OriginalStudio.ETAPI
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Withdraw;
    using OriginalStudio.Model;
    using OriginalStudio.Model.Settled;
    using OriginalStudio.Model.Withdraw;
    using System;

    public class Withdraw
    {
        private static BLL.Withdraw.ChannelWithdraw chalBLL = new BLL.Withdraw.ChannelWithdraw();

        public static int Complete(int suppId, string trade_no, bool is_cancel, int status, string amount, string supp_trade_no, string message)
        {
            string str = string.Empty;
            int num = OriginalStudio.BLL.Settled.Distribution.Process(suppId, trade_no, is_cancel, status, amount, supp_trade_no, message, out str);
            if ((num == 0) && (trade_no.Substring(0, 1) == "2"))
            {
                new OriginalStudio.BLL.Withdraw.settledAgent().DoNotify(str);
            }
            return num;
        }

        /// <summary>
        /// 提现非代理模式。
        /// </summary>
        /// <param name="itemInfo"></param>
        public static void InitDistribution(SettledInfo itemInfo)
        {
            OriginalStudio.Model.Settled.Distribution model = new OriginalStudio.Model.Settled.Distribution();
            model.trade_no = OriginalStudio.BLL.Settled.Distribution.GenerateTradeNo(1);
            model.mode = 1;     //!!!!!
            model.settledId = new int?(itemInfo.ID);
            model.batchNo = 1;
            model.userid = itemInfo.UserID;
            model.balance = 0M;
            model.bankCode = itemInfo.BankCode;        //itemInfo.PayeeBank是银行名称
            model.suppid = itemInfo.Suppid;
            model.bankName = itemInfo.PayeeBank;
            model.bankBranch = itemInfo.PayeeAddress;
            model.bankAccountName = itemInfo.PayeeName;
            model.bankAccount = itemInfo.Account;
            if (itemInfo.Charges > 0)
            {
                //费用
                OriginalStudio.Model.Settled.Distribution distribution2 = model;
                decimal amount = itemInfo.Amount;
                decimal num2 = itemInfo.Charges;
                decimal num3 = amount - num2;
                distribution2.amount = num3;
            }
            else
            {
                model.amount = itemInfo.Amount;
            }
            OriginalStudio.Model.Settled.Distribution distribution3 = model;
            decimal num4 = itemInfo.Charges;
            distribution3.charges = num4;
            model.balance2 = 0;

            /**/
            //Step1:先记录数据库
            if (OriginalStudio.BLL.Settled.Distribution.Add(model) > 0)
            {
                //Step2：调用网关支付
                SellFactory.ReqDistribution(model);
            }
            
        }

        /// <summary>
        /// 提现代理模式
        /// </summary>
        /// <param name="itemInfo"></param>
        public static void InitDistribution2(OriginalStudio.Model.Withdraw.settledAgent itemInfo)
        {
            OriginalStudio.Model.Settled.Distribution model = new OriginalStudio.Model.Settled.Distribution();
            model.trade_no = OriginalStudio.BLL.Settled.Distribution.GenerateTradeNo(2);
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
            if (OriginalStudio.BLL.Settled.Distribution.Add(model) > 0)
            {
                SellFactory.ReqDistribution(model);
            }
        }


    }
}

