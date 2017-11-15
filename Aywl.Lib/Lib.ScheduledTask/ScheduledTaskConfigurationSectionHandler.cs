using OriginalStudio.Lib.Configuration;
using OriginalStudio.Lib.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace OriginalStudio.Lib.ScheduledTask
{
	public class ScheduledTaskConfigurationSectionHandler : IConfigurationSectionHandler
	{
		protected static Cache webCache = HttpRuntime.Cache;

		public object Create(object parent, object configContext, XmlNode section)
		{
			return ScheduledTaskConfigurationSectionHandler.Parse(section);
		}

		public static List<ScheduledTaskConfiguration> GetConfigs()
		{
			string filePath = ConfigHelper.FilePath;
			string xpath = string.Format("/configuration/scheduledTaskConfiguration", new object[0]);
			string key = filePath + "|" + xpath;
			List<ScheduledTaskConfiguration> result;
			if (ScheduledTaskConfigurationSectionHandler.webCache[key] != null)
			{
				List<ScheduledTaskConfiguration> list = ScheduledTaskConfigurationSectionHandler.webCache[key] as List<ScheduledTaskConfiguration>;
				if (list != null)
				{
					result = list;
					return result;
				}
			}
			try
			{
				if (File.Exists(filePath))
				{
					List<ScheduledTaskConfiguration> list = new List<ScheduledTaskConfiguration>();
					XmlDocument xmlDocument = ConfigHelper.GetXmlDocument(filePath);
					if (xmlDocument != null)
					{
						XmlNode section = xmlDocument.SelectSingleNode(xpath);
						if (section != null)
						{
							list = ScheduledTaskConfigurationSectionHandler.Parse(section);
						}
					}
					ScheduledTaskConfigurationSectionHandler.webCache.Add(key, list, new CacheDependency(filePath), DateTime.Now.AddDays(10.0), TimeSpan.Zero, CacheItemPriority.Normal, null);
					result = list;
					return result;
				}
			}
			catch (Exception ex)
			{
				ExceptionHandler.HandleException(ex);
			}
			result = new List<ScheduledTaskConfiguration>(0);
			return result;
		}

		public static List<ScheduledTaskConfiguration> Parse(XmlNode section)
		{
			List<ScheduledTaskConfiguration> list = new List<ScheduledTaskConfiguration>();
			foreach (XmlNode xmlNode in section.ChildNodes)
			{
				if (xmlNode.Name == "scheduledTask")
				{
					ScheduledTaskConfiguration taskConfiguration = new ScheduledTaskConfiguration();
					foreach (XmlAttribute xmlAttribute in xmlNode.Attributes)
					{
						string name = xmlAttribute.Name;
						if (name != null)
						{
							string str = string.IsInterned(name);
							if (str == "ScheduledTaskType")
							{
								taskConfiguration.ScheduledTaskType = xmlAttribute.Value;
							}
							else if (str == "ThreadSleepSecond")
							{
								taskConfiguration.ThreadSleepSecond = Convert.ToInt32(xmlAttribute.Value, 10);
							}
						}
					}
					foreach (XmlNode xmlNode2 in xmlNode.ChildNodes)
					{
						if (xmlNode2.Name == "execute")
						{
							foreach (XmlAttribute xmlAttribute in xmlNode2.Attributes)
							{
								if (xmlAttribute.Name == "type")
								{
									taskConfiguration.Executes.Add(xmlAttribute.Value);
									break;
								}
							}
						}
					}
					list.Add(taskConfiguration);
				}
			}
			return list;
		}
	}
}
