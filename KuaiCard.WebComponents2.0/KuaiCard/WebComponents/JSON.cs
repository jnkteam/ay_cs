namespace KuaiCard.WebComponents
{
    using KuaiCardLib.ExceptionHandling;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Reflection;

    public class JSON
    {
        private string _json;

        public JSON(string json)
        {
            this._json = json;
        }

        public T Convert<T>(T NewT)
        {
            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                object obj2 = this.GetValue(info.Name);
                if (obj2 == null)
                {
                    NewT = default(T);
                    return NewT;
                }
                info.SetValue(NewT, obj2, null);
            }
            return NewT;
        }

        public object GetValue(string name)
        {
            try
            {
                return this._json.Split(new string[] { name + "\":\"" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { "\"" }, StringSplitOptions.RemoveEmptyEntries)[0];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string GetValue(string json, string key)
        {
            try
            {
                return JObject.Parse(json)[key].ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string SerializeObject(object model)
        {
            try
            {
                JsonConverter[] converters = new JsonConverter[1];
                IsoDateTimeConverter converter = new IsoDateTimeConverter();
                converter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                converters[0] = converter;
                return JsonConvert.SerializeObject(model, Formatting.Indented, converters);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return string.Empty;
            }
        }
    }
}

