namespace KuaiCard.WebUI.User.Service
{
    using KuaiCard.BLL.User;
    using KuaiCard.Model.User;
    using System;
    using System.Web;
    using System.Web.SessionState;
    using System.IO;
    using System.Drawing;

    public class realname : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        private UserInfo _currentUser = null;

        public void ProcessRequest(HttpContext context)
        {
            //KuaiCardLib.Logging.LogHelper.Write("realname0");

            try
            {
                string s = "";
                if (this.currentUser == null)
                {
                    s = "{\"result\":\"false\",\"text\":\"未登录\",\"ok\":\"true\"}";
                    context.Response.ContentType = "application/json";
                    context.Response.Write(s);
                    return;
                }
                //KuaiCardLib.Logging.LogHelper.Write("realname1");
                string str2 = context.Request["auth_Name2"];
                string str3 = context.Request["attachments1"];
                string str4 = context.Request["attachments2"];
                string str5 = context.Request["auth_PaperworkNumb"];
                if (string.IsNullOrEmpty(str2))
                {
                    s = "{\"result\":\"false\",\"text\":\"姓名不能为空！\",\"ok\":\"true\"}";
                }
                else if (string.IsNullOrEmpty(str5))
                {
                    s = "{\"result\":\"false\",\"text\":\"身份证号码不能为空！\",\"ok\":\"true\"}";
                }
                else if (string.IsNullOrEmpty(str3))
                {
                    s = "{\"result\":\"false\",\"text\":\"请上传身份证正面照！\",\"ok\":\"true\"}";
                }
                else if (string.IsNullOrEmpty(str4))
                {
                    s = "{\"result\":\"false\",\"text\":\"请上传身份证反面照\",\"ok\":\"true\"}";
                }
                else
                {
                    //KuaiCardLib.Logging.LogHelper.Write("str2" + str2);

                    this.currentUser.full_name = str2;
                    this.currentUser.versoPic = str4;
                    this.currentUser.frontPic = str3;
                    this.currentUser.IdCard = str5;
                    this.currentUser.IsRealNamePass = 2;
                    this.currentUser.msn = DateTime.Now.ToLongDateString().ToString();
                    //if (UserFactory.Update(UserFactory.CurrentMember, null))
                    if (UserFactory.Update(this.currentUser, null))
                    {
                        //=====16.12.5把图片字节流写入数据库=========
                        usersIdImageInfo imgInfo = new usersIdImageInfo();
                        imgInfo.userId = Convert.ToInt32(this.currentUser.ID);
                        imgInfo.addtime = DateTime.Now;
                        if (!string.IsNullOrEmpty(str4))
                        {
                            //身份证反面扫描件
                            string tmpFile = context.Server.MapPath(str4);
                            if (File.Exists(tmpFile))
                            {
                                byte[] bts = imageToByteArray(context.Server.MapPath(str4));
                                imgInfo.image_down = bts;
                                imgInfo.filesize1 = bts.Length;
                                imgInfo.ptype1 = "2";
                                imgInfo.status = IdImageStatus.审核中;
                            }
                        }
                        if (!string.IsNullOrEmpty(str3))
                        {
                            //身份证正面扫描件
                            string tmpFile = context.Server.MapPath(str3);
                            if (File.Exists(tmpFile))
                            {
                                byte[] bts = imageToByteArray(context.Server.MapPath(str3));
                                imgInfo.image_on = bts;
                                imgInfo.filesize = bts.Length;
                                imgInfo.ptype1 = "1";
                                imgInfo.status = IdImageStatus.审核中;
                            }
                        }
                        new KuaiCard.BLL.User.usersIdImage().Add(imgInfo);

                        //=====16.12.5把图片字节流写入数据库=========

                        s = "{\"result\":\"true\",\"text\":\"实名认证申请已经受理，请等待审核！\",\"time\":2,\"url\":\"/user/realname/\"}";
                    }
                    else
                    {
                        s = "{\"result\":\"false\",\"text\":\"提交失败，请联系管理人员\",\"ok\":\"true\"}";
                    }
                }
                //KuaiCardLib.Logging.LogHelper.Write("s:" + s);
                context.Response.ContentType = "application/json";
                context.Response.Write(s);

            }
            catch (Exception err)
            {
                KuaiCardLib.Logging.LogHelper.Write("实名认证realname错误:" + err.Message.ToString());
            }
        }

        public UserInfo currentUser
        {
            get
            {
                if (this._currentUser == null)
                {
                    this._currentUser = UserFactory.CurrentMember;
                }
                return this._currentUser;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 图片转为Byte字节数组
        /// </summary>
        /// <param name="FilePath">路径</param>
        /// <returns>字节数组</returns>
        private byte[] imageToByteArray(string FilePath)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Image imageIn = Image.FromFile(FilePath))
                {
                    using (Bitmap bmp = new Bitmap(imageIn))
                    {
                        bmp.Save(ms, imageIn.RawFormat);
                    }
                }
                return ms.ToArray();
            }
        }
    }
}

