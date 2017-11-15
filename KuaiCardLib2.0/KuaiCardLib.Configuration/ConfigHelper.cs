using KuaiCardLib.ExceptionHandling;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace KuaiCardLib.Configuration
{
	public sealed class ConfigHelper
	{
		private static readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configurations\\runtimeconfiguration.config");

		private static readonly CultureInfo _culture = CultureInfo.CreateSpecificCulture("zh-CN");

		public static string FilePath
		{
			get
			{
				return ConfigHelper._filePath;
			}
		}

		public static CultureInfo DefaultCulture
		{
			get
			{
				return ConfigHelper._culture;
			}
		}

		private ConfigHelper()
		{
		}

        public static string GetConfig(object settingGroup, string v)
        {
            throw new NotImplementedException();
        }

        public static string GetConfig(string group, string key)
		{
			return ConfigHelper.GetConfig(null, group, key);
		}

		public static string GetConfig(string path, string group, string key)
		{
			string result;
			if (key != null && key.Length != 0)
			{
				NameValueCollection configs = ConfigHelper.GetConfigs(path, group);
				if (configs != null && configs[key] != null)
				{
					result = configs[key];
					return result;
				}
			}
			result = string.Empty;
			return result;
		}

		public static NameValueCollection GetConfigs(string group)
		{
			return ConfigHelper.GetConfigs(null, group);
		}

		public static NameValueCollection GetConfigs(string path, string group)
		{
			NameValueCollection result;
			if (group != null && group.Length != 0)
			{
				if (path == null || path.Trim().Length == 0)
				{
					path = ConfigHelper.FilePath;
				}
				string key = group + "_" + path;
				if (HttpRuntime.Cache.Get(key) != null)
				{
					NameValueCollection nameValueCollection = HttpRuntime.Cache.Get(key) as NameValueCollection;
					if (nameValueCollection != null)
					{
						result = nameValueCollection;
						return result;
					}
				}
				try
				{
					if (File.Exists(path))
					{
						NameValueCollection nameValueCollection = new NameValueCollection();
						XmlDocument xmlDocument = ConfigHelper.GetXmlDocument(path);
						if (xmlDocument != null)
						{
							foreach (XmlNode xmlNode in xmlDocument.SelectNodes("/configuration/" + group + "/add"))
							{
								nameValueCollection.Add(xmlNode.Attributes["key"].Value, xmlNode.Attributes["value"].Value);
							}
						}
						CacheDependency dependencies = new CacheDependency(path, DateTime.Now);
						HttpRuntime.Cache.Insert(key, nameValueCollection, dependencies);
						result = nameValueCollection;
						return result;
					}
				}
				catch (Exception ex)
				{
					ExceptionHandler.HandleException(ex);
				}
			}
			result = new NameValueCollection(0);
			return result;
		}

		public static XmlDocument GetXmlDocument(string path)
		{
			XmlTextReader xmlTextReader = null;
			XmlDocument result;
			try
			{
				xmlTextReader = new XmlTextReader(path);
				int num = (int)xmlTextReader.MoveToContent();
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xmlTextReader.ReadOuterXml());
				result = xmlDocument;
				return result;
			}
			catch (Exception ex)
			{
				ExceptionHandler.HandleException(ex);
			}
			finally
			{
				if (xmlTextReader != null)
				{
					xmlTextReader.Close();
				}
			}
			result = null;
			return result;
		}

		public static bool WriteConfig(string group, NameValueCollection list)
		{
			return ConfigHelper.WriteConfig(null, group, list);
		}

		public static bool WriteConfig(string path, string group, NameValueCollection list)
		{
			if (path == null || path.Trim().Length == 0)
			{
				path = ConfigHelper.FilePath;
			}
			XmlDocument xmlDocument = ConfigHelper.GetXmlDocument(path);
			if (xmlDocument == null)
			{
				xmlDocument = new XmlDocument();
				xmlDocument.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><configuration></configuration>");
			}
			XmlNode xmlNode = xmlDocument.DocumentElement;
			XmlNode newChild = xmlDocument.SelectSingleNode("configuration/" + group);
			if (newChild == null)
			{
				newChild = xmlDocument.CreateElement(group);
				xmlNode.AppendChild(newChild);
			}
			XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/configuration/" + group + "/add");
			for (int index = 0; index < list.AllKeys.Length; index++)
			{
				bool flag = false;
				foreach (XmlElement xmlElement in xmlNodeList)
				{
					if (xmlElement.Attributes["key"].Value == list.AllKeys[index])
					{
						xmlElement.SetAttribute("value", list[list.AllKeys[index]]);
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					XmlElement element = xmlDocument.CreateElement("add");
					element.SetAttribute("key", list.AllKeys[index]);
					element.SetAttribute("value", list[list.AllKeys[index]]);
					newChild.AppendChild(element);
				}
			}
			string directoryName = Path.GetDirectoryName(path);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.UTF8))
			{
				streamWriter.Write(xmlDocument.OuterXml);
				streamWriter.Close();
			}
			return true;
		}
	}
}
