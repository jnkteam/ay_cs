namespace KuaiCard.WebUI.User.Service
{
    using OriginalStudio.Model.Json;
    using OriginalStudio.WebComponents;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    public class changethemes : UserHandlerBase
    {
        public override void OnLoad(HttpContext context)
        {
            AjaxResult model = new AjaxResult();
            if (this.CurrentUser == null)
            {
                model.result = "error";
                model.msg = "未登录";
            }
            else
            {
                try
                {
                    int userid = this.CurrentUser.ID;
                    string defaultthemes = WebBase.GetQueryStringString("themes", "");
                    if (string.IsNullOrEmpty(defaultthemes) || userid == 0)
                    {
                        model.result = "error";
                        model.msg = "参数为空";
                    }
                    else
                    {
                        KuaiCard.BLL.User.UserFactory.ChangeUserDefaultThemes(userid, defaultthemes);
                        model.result = "ok";
                        model.msg = "皮肤修改成功";
                    }
                }
                catch (Exception exception)
                {
                    model.result = "error";
                    model.msg = exception.Message;
                }
            }
            string s = JSON.SerializeObject(model);
            context.Response.ContentType = "application/json";
            context.Response.Write(s);
        }
    }
}

