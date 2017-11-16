namespace KuaiCard.WebUI.webservice
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model;
    using OriginalStudio.Model.User;
    using OriginalStudio.WebComponents;
    using OriginalStudio.Lib.Security;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Text;
    using System.Web;
    using System.Web.SessionState;

    public class Register : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        private WebInfo _webinfo = null;

        private string GetCheckEmailUrl(string parms)
        {
            return (this.webInfo.Domain + "Merchant/Ajax/mailCheckReceive.ashx?parms=" + parms);
        }

        public void ProcessRequest(HttpContext context)
        {
            string str = "";
            string str2 = "";
            string str3 = context.Request["LoginName"];
            string str4 = context.Request["LoginPassword"];
            string str5 = context.Request["SndCode"];
            string str6 = context.Request["ReLoginPassword"];
            string str7 = context.Request["MerchantName"];
            string str8 = context.Request["IdCardNo"];
            string str9 = context.Request["MobilePhone"];
            string str10 = context.Request["Email"];
            string str11 = context.Request["ContactQQ"];
            string str12 = context.Request["CryptoguardQuestion"];
            string str13 = context.Request["CryptoguardAnswer"];
            string str14 = HttpContext.Current.Session["CCode"].ToString();
            if (string.IsNullOrEmpty(str3))
            {
                str = "用户名不能为空!";
            }
            else if (string.IsNullOrEmpty(str4))
            {
                str = "密码不能为空!";
            }
            else if (string.IsNullOrEmpty(str6))
            {
                str = "确认密码不能为空!";
            }
            else if (str4 != str6)
            {
                str = "两次密码不一致!";
            }
            else if (string.IsNullOrEmpty(str7))
            {
                str = "用户名不能为空!";
            }
            else if (string.IsNullOrEmpty(str8))
            {
                str = "身份证号码不能为空!";
            }
            else if (string.IsNullOrEmpty(str9))
            {
                str = "手机号码不能为空!";
            }
            else if (string.IsNullOrEmpty(str10))
            {
                str = "邮箱不能为空!";
            }
            else if (string.IsNullOrEmpty(str11))
            {
                str = "QQ不能为空!";
            }
            else if (string.IsNullOrEmpty(str12))
            {
                str = "密保问题不能为空!";
            }
            else if (string.IsNullOrEmpty(str13))
            {
                str = "密保答案不能为空!";
            }
            else if (HttpContext.Current.Session["CCode"] == null)
            {
                str = "验证码失效!请刷新页面";
            }
            else if (str14.ToUpper() != str5.ToUpper())
            {
                str = "验证码不正确";
            }
            else if (str == "")
            {
                UserInfo info = new UserInfo();
                info.UserName = str3;
                info.Password = str4;
                info.Email = str10;
                info.QQ = str11;
                info.Tel = str9;
                info.IdCard = str8;
                info.CPSDrate = SysConfig.DefaultCPSDrate;
                info.PMode = 1;
                info.LinkMan = str7;
                info.full_name = str7;
                info.Status = SysConfig.IsAudit ? 1 : 2;
                info.LastLoginIp = ServerVariables.TrueIP;
                info.LastLoginTime = DateTime.Now;
                info.RegTime = DateTime.Now;
                info.Settles = SysConfig.DefaultSettledMode;
                info.CPSDrate = SysConfig.DefaultCPSDrate;
                info.AgentId = 0;
                info.APIAccount = 0L;
                info.APIKey = Guid.NewGuid().ToString("N");
                info.question = str12;
                info.answer = str13;
                if (this.m_Id > 0)
                {
                    info.manageId = new int?(this.m_Id);
                }
                int userId = UserFactory.Add(info);
                if (userId <= 0)
                {
                    str = "注册失败！";
                }
                else
                {
                    if (this.agent_Id > 1)
                    {
                        PromotionUserInfo promUser = new PromotionUserInfo();
                        promUser.PID = this.agent_Id;
                        promUser.Prices = 0.5M;
                        promUser.RegId = userId;
                        promUser.PromTime = DateTime.Now;
                        promUser.PromStatus = 1;
                        PromotionUserFactory.Insert(promUser);
                    }
                    if (!SysConfig.RegistrationActivationByEmail)
                    {
                        if (!SysConfig.IsAudit)
                        {
                            str = "注册成功,无须审核请登陆后直接使用！";
                            str2 = "/";
                        }
                        else
                        {
                            str = "注册成功,请等待管理员审核！";
                        }
                    }
                    else
                    {
                        string str15 = this.SendMail(userId, str10);
                        if (str15 != null)
                        {
                            if (!(str15 == "ok"))
                            {
                                if (str15 == "no")
                                {
                                    str = "邮件发送失败，请联系管理员";
                                }
                            }
                            else
                            {
                                str = "注册成功,请登录邮箱激活账号!";
                                str2 = "/";
                            }
                        }
                    }
                }
            }
            context.Response.ContentType = "application/json";
            context.Response.Write("{\"result\":\"" + str + "\",\"msg\":\"" + str + "\",\"url\":\"" + str2 + "\"}");
        }

        public string SendMail(int UserId, string email)
        {
            string str = string.Empty;
            EmailCheckInfo model = new EmailCheckInfo();
            model.userid = UserId;
            model.status = EmailCheckStatus.提交中;
            model.addtime = new DateTime?(DateTime.Now);
            model.checktime = new DateTime?(DateTime.Now);
            model.email = email;
            model.typeid = EmailCheckType.注册;
            model.Expired = DateTime.Now.AddDays(7.0);
            int num = new EmailCheck().Add(model);
            if (num <= 0)
            {
                return str;
            }
            string domain = this.webInfo.Domain;
            string name = this.webInfo.Name;
            string kfqq = this.webInfo.Kfqq;
            string phone = this.webInfo.Phone;
            string parms = HttpUtility.UrlEncode(Cryptography.RijndaelEncrypt(string.Format("id={0}&", num)));
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("<p>亲爱的{0}:<p>", email);
            builder.AppendFormat("<p style=\"font-size:14px\">感谢您注册{0}，您需要在一周之内激活您的账号。 您只需点击下面的确认链接，即可完成帐号激活。</p><p>如果不是您本人的操作，可能是有用户误输入您的Email地址，您可以忽略此邮件或与{0}客服联系。</p>", this.SiteName);
            builder.AppendFormat("<p><a href=\"{0}\" style=\"color:#003300\">{0}</a></p>", this.GetCheckEmailUrl(parms));
            builder.Append("<p style=\"color:#999;font-size:12px\">如果无法点击该URL链接地址，请将它复制并粘帖到浏览器的地址输入框，然后单击回车即可。");
            builder.Append("<p><p>————————————————————————————————");
            builder.AppendFormat("<p style=\"font-size:14px;line-height:150%\">{1} {0} 如有疑问加我们的客服QQ {2} 或者来电咨询 {3}", new object[] { domain, name, kfqq, phone });
            builder.AppendFormat("<p><img src=\"http://{0}/style/index/images/logo.png\"  />  <!-- --><style>#mailContentContainer .txt {{height:auto;}}</style>", domain);
            EmailHelper helper = new EmailHelper(string.Empty, email, email + "账号激活", builder.ToString(), true, Encoding.GetEncoding("gbk"));
            if (helper.Send())
            {
                return "ok";
            }
            return "no";
        }

        public int agent_Id
        {
            get
            {
                return WebBase.GetQueryStringInt32("registered", 0);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public int m_Id
        {
            get
            {
                return WebBase.GetQueryStringInt32("s", 0);
            }
        }

        public string SiteName
        {
            get
            {
                if (this.webInfo == null)
                {
                    return string.Empty;
                }
                return this.webInfo.Name;
            }
        }

        public WebInfo webInfo
        {
            get
            {
                if (this._webinfo == null)
                {
                    this._webinfo = WebInfoFactory.CurrentWebInfo;
                }
                return this._webinfo;
            }
        }
    }
}

