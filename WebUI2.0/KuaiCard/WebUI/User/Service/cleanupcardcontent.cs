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

    public class cleanupcardcontent : UserHandlerBase
    {
        private string Cleanup(string content)
        {
            StringBuilder builder = new StringBuilder();
            string pattern = "[0-9a-zA-Z]{6,20}";
            MatchCollection matchs = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase).Matches(content);
            int num = 0;
            foreach (Match match in matchs)
            {
                if (!match.Value.StartsWith("DEP"))
                {
                    num++;
                    if ((num % 2) == 0)
                    {
                        builder.AppendFormat("{0}\r\n", match.Value);
                    }
                    else
                    {
                        builder.AppendFormat("{0} ", match.Value);
                    }
                }
            }
            return builder.ToString().Trim();
        }

        public override void OnLoad(HttpContext context)
        {
            AjaxResult model = new AjaxResult();
            try
            {
                string formString = WebBase.GetFormString("cardcontent", "");
                if (!string.IsNullOrEmpty(formString))
                {
                    model.result = "ok";
                    model.msg = this.Cleanup(formString);
                }
            }
            catch (Exception exception)
            {
                model.msg = exception.Message;
            }
            string s = JSON.SerializeObject(model);
            context.Response.ContentType = "application/json";
            context.Response.Write(s);
        }
    }
}

