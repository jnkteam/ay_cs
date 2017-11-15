namespace KuaiCard.WebUI.User.Service
{
    using KuaiCard.BLL;
    using KuaiCard.BLL.User;
    using KuaiCard.Cache;
    using KuaiCard.Model;
    using KuaiCard.Model.User;
    using System;
    using System.Web;
    using System.Web.SessionState;

    public class Code : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        private WebInfo _webinfo = null;

        public void ProcessRequest(HttpContext context)
        {
            string msg = "";            //返回消息
            string ico = "";            //显示图标success，error
            string result = "";         //结果ok，no
            string url = "";            //跳转url

            if (this.currentUser == null)
            {
                //未登录
                msg = "未登录!";
                result = "no";
                ico = "error";
                goto Label_Exit;
            }

            string code = context.Request["code"];
            string reqName = context.Request["name"];
            string email = context.Request["email"];
            string Phone = context.Request["Phone"];
            string paypwd = context.Request["paypwd"];          //支付密码
            string imgCode = context.Request["imgcode"];        //验证码参数，有的传递，使用前判断是否为空null

            //2017.8.18 增加使用用户判断条件
            string siteuser = KuaiCard.SysConfig.RuntimeSetting.SiteUser;

            if (String.IsNullOrEmpty(reqName) == false)
            {
                string str;
                string str2;
                if (reqName == "newtel")
                {
                    //绑定手机
                    str = "PHONE_VALID_" + Phone;
                    str2 = (string)WebCache.GetCacheService().RetrieveObject(str);
                    if (code != str2)
                    {
                        msg = "手机验证码不正确!";
                        result = "no";
                        ico = "error";
                        goto Label_Exit;
                    }
                    else
                    {
                        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                        if (siteuser == "zft")
                        {
                            //支付通 加上 验证码图片模式
                            if ((context.Session["CCode"] == null) || (imgCode ==null))
                            {
                                msg = "验证码失效!";
                                result = "no";
                                ico = "error";
                                goto Label_Exit;
                            }
                            else if (context.Session["CCode"].ToString().ToUpper() != imgCode.ToUpper())
                            {
                                msg = "验证码不正确!";
                                result = "no";
                                ico = "error";
                                goto Label_Exit;
                            }
                        }
                        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

                        //0n2.pay模式
                        this.currentUser.IsPhonePass = 1;
                        this.currentUser.Tel = Phone;
                        if (UserFactory.Update(this.currentUser, null))
                        {
                            msg = "验证通过！";
                            result = "ok";
                            ico = "success";
                        }
                        else
                        {
                            this.currentUser.IsPhonePass = 0;
                            this.currentUser.Tel = string.Empty;
                            msg = "修改失败";
                        }

                    }
                }
                else if (reqName == "modifyShouji")
                {
                    //修改手机
                    str = "PHONE_VALID_" + UserFactory.CurrentMember.Tel;
                    str2 = (string)WebCache.GetCacheService().RetrieveObject(str);
                    if (code != str2)
                    {
                        msg = "手机验证码不正确!";
                        result = "no";
                        ico = "error";
                    }
                    else
                    {
                        this.currentUser.IsEmailPass = 0;
                        if (UserFactory.Update(this.currentUser, null))
                        {
                            msg = "验证通过！";
                            result = "ok";
                            ico = "success";
                            url = "/user/validate/set.aspx";
                        }
                        else
                        {
                            this.currentUser.IsEmailPass = 1;
                            this.currentUser.Tel = string.Empty;
                            msg = "修改失败";
                        }
                    }
                }
                else if (reqName == "mobilecode")
                {
                    //手机验证码
                    str = "PHONE_VALID_" + UserFactory.CurrentMember.Tel;
                    str2 = (string)WebCache.GetCacheService().RetrieveObject(str);
                    if (code != str2)
                    {
                        msg = "手机验证码不正确!";
                        result = "no";
                        ico = "error";
                    }
                    else
                    {
                        this.currentUser.IsPhonePass = 0;
                        if (UserFactory.Update(this.currentUser, null))
                        {
                            msg = "验证通过！";
                            result = "ok";
                            ico = "success";
                            url = "/user/validate/tel.aspx";
                        }
                        else
                        {
                            this.currentUser.IsPhonePass = 1;
                            this.currentUser.Tel = string.Empty;
                            msg = "修改失败";
                        }
                    }
                }
                else if (reqName == "modifyEmail")
                {
                    //修改邮件
                    str = "PHONE_VALID_" + UserFactory.CurrentMember.Email;
                    str2 = (string)WebCache.GetCacheService().RetrieveObject(str);
                    if (code != str2)
                    {
                        msg = "邮件验证码不正确!";
                        result = "no";
                        ico = "error";
                    }
                    else
                    {
                        this.currentUser.IsEmailPass = 0;
                        if (UserFactory.Update(this.currentUser, null))
                        {
                            msg = "验证通过！";
                            result = "ok";
                            ico = "success";
                            url = "/user/validate/set.aspx";
                        }
                        else
                        {
                            this.currentUser.IsEmailPass = 1;
                            this.currentUser.Tel = string.Empty;
                            msg = "修改失败";
                        }
                    }
                }
                else if (reqName == "newmail")
                {
                    if (siteuser == "zft")
                    {
                        //支付通 加上 验证码图片模式
                        if ((context.Session["CCode"] == null) || (imgCode == null))
                        {
                            msg = "验证码失效!";
                            result = "no";
                            ico = "error";
                            goto Label_Exit;
                        }
                        else if (context.Session["CCode"].ToString().ToUpper() != imgCode.ToUpper())
                        {
                            msg = "验证码不正确!";
                            result = "no";
                            ico = "error";
                            goto Label_Exit;
                        }
                    }

                    //新邮件
                    str = "PHONE_VALID_" + email;
                    str2 = (string)WebCache.GetCacheService().RetrieveObject(str);
                    if (code != str2)
                    {
                        msg = "邮件验证码不正确!";
                        result = "no";
                        ico = "error";
                    }
                    else
                    {
                        this.currentUser.IsEmailPass = 1;
                        this.currentUser.Email = email;
                        if (UserFactory.Update(this.currentUser, null))
                        {
                            msg = "验证通过！";
                            result = "ok";
                            ico = "success";
                        }
                        else
                        {
                            this.currentUser.IsEmailPass = 0;
                            this.currentUser.Tel = string.Empty;
                            msg = "修改失败";
                        }
                    }
                }
                else if (reqName == "showkey")
                {
                    //显示密钥
                    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                    if (siteuser == "zft")
                    {
                        if (string.IsNullOrEmpty(paypwd) || string.IsNullOrEmpty(imgCode))
                        {
                            msg = "内容不全!";
                            result = "no";
                            ico = "error";
                            goto Label_Exit;
                        }
                        //支付通 加上 验证码图片模式
                        if ((context.Session["CCode"] == null) || (imgCode == null))
                        {
                            msg = "验证码失效!";
                            result = "no";
                            ico = "error";
                            goto Label_Exit;
                        }
                        else if (context.Session["CCode"].ToString().ToUpper() != imgCode.ToUpper())
                        {
                            msg = "验证码不正确!";
                            result = "no";
                            ico = "error";
                            goto Label_Exit;
                        }
                        //验证支付密码是否准确
                        if (this.currentUser.Password2 == paypwd)
                        {
                            msg = this.currentUser.APIKey; //返回的密钥
                            result = "ok";
                            ico = "success";
                            goto Label_Exit;
                        }
                        else
                        {
                            msg = "支付密码错误！";
                            result = "no";
                            ico = "error";
                            goto Label_Exit;
                        }
                    }
                    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                }
                else if (reqName == "getkey")
                { 
                    //产生随机密钥
                    long tick = DateTime.Now.Ticks;
                    Random random = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
                    string tmp = KuaiCardLib.Security.Cryptography.MD5(random.Next(100000000).ToString("00000000"));
                    msg = tmp; //返回的密钥
                    result = "ok";
                    ico = "success";
                    goto Label_Exit;
                }
                else if (reqName == "upkey")
                {
                    //修改密钥
                    if (siteuser == "zft")
                    {
                        string newkey = context.Request["newkey"];
                        if (string.IsNullOrEmpty(paypwd) || string.IsNullOrEmpty(newkey) || string.IsNullOrEmpty(imgCode))
                        {
                            msg = "内容不全!";
                            result = "no";
                            ico = "error";
                            goto Label_Exit;
                        }
                        //支付通 加上 验证码图片模式
                        if ((context.Session["CCode"] == null) || (imgCode == null))
                        {
                            msg = "验证码失效!";
                            result = "no";
                            ico = "error";
                            goto Label_Exit;
                        }
                        else if (context.Session["CCode"].ToString().ToUpper() != imgCode.ToUpper())
                        {
                            msg = "验证码不正确!";
                            result = "no";
                            ico = "error";
                            goto Label_Exit;
                        }
                        //验证支付密码是否准确
                        if (this.currentUser.Password2 == paypwd)
                        {
                            UserFactory.ChangeUserApiKey(this.currentUser.ID, newkey);
                            //修改数据库操作
                            msg = ""; //
                            result = "ok";
                            ico = "success";
                            goto Label_Exit;
                        }
                        else
                        {
                            msg = "支付密码错误！";
                            result = "no";
                            ico = "error";
                            goto Label_Exit;
                        }
                    }
                }
                else if (reqName == "newip")
                {
                    string newip = context.Request["newip"];
                    string loginip = context.Request["loginip"];
                    string noticeip = context.Request["noticeip"];

                    if (string.IsNullOrEmpty(paypwd) 
                        || string.IsNullOrEmpty(newip) 
                        || string.IsNullOrEmpty(imgCode)
                        || string.IsNullOrEmpty(loginip)
                         || string.IsNullOrEmpty(noticeip))
                    {
                        msg = "内容不全!";
                        result = "no";
                        ico = "error";
                        goto Label_Exit;
                    }
                    //支付通 加上 验证码图片模式
                    if ((context.Session["CCode"] == null) || (imgCode == null))
                    {
                        msg = "验证码失效!";
                        result = "no";
                        ico = "error";
                        goto Label_Exit;
                    }
                    else if (context.Session["CCode"].ToString().ToUpper() != imgCode.ToUpper())
                    {
                        msg = "验证码不正确!";
                        result = "no";
                        ico = "error";
                        goto Label_Exit;
                    }
                    if (this.currentUser.Password2 == paypwd)
                    {
                        //数据库操作
                        if (loginip == "1")
                            UserFactory.SetUserBindIp(this.currentUser.ID, 1, newip);
                        if (noticeip == "1")
                            UserFactory.SetUserBindIp(this.currentUser.ID, 2, newip);
                        msg = "绑定成功！"; //
                        result = "ok";
                        ico = "success";
                        goto Label_Exit;
                    }
                    else
                    {
                        msg = "支付密码错误！";
                        result = "no";
                        ico = "error";
                        goto Label_Exit;
                    }
                }
                else if (reqName == "delip")
                {
                    string recid = context.Request["rec"];

                    if (string.IsNullOrEmpty(recid)
                        || string.IsNullOrEmpty(imgCode))
                    {
                        msg = "内容不全!";
                        result = "no";
                        ico = "error";
                        goto Label_Exit;
                    }
                    //新邮件
                    str = "PHONE_VALID_" + email;
                    str2 = (string)WebCache.GetCacheService().RetrieveObject(str);
                    if (code != str2)
                    {
                        msg = "邮件验证码不正确!";
                        result = "no";
                        ico = "error";
                        goto Label_Exit;
                    }

                    //支付通 加上 验证码图片模式
                    if ((context.Session["CCode"] == null) || (imgCode == null))
                    {
                        msg = "验证码失效!";
                        result = "no";
                        ico = "error";
                        goto Label_Exit;
                    }
                    else if (context.Session["CCode"].ToString().ToUpper() != imgCode.ToUpper())
                    {
                        msg = "验证码不正确!";
                        result = "no";
                        ico = "error";
                        goto Label_Exit;
                    }
                    //数据库操作
                    UserFactory.DeleteUserBindIp(Convert.ToInt32(recid));
                    msg = "删除成功！"; //
                    result = "ok";
                    ico = "success";
                    goto Label_Exit;

                }
            }

        Label_Exit:
            context.Response.ContentType = "application/json";
            context.Response.Write("{\"result\":\"" + result + "\",\"ico\":\"" + ico + "\",\"msg\":\"" + msg + "\",\"url\":\"" + url + "\"}");
        }

        public UserInfo currentUser
        {
            get
            {
                return UserFactory.CurrentMember;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
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

