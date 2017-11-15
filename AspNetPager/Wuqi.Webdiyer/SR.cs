using System;
using System.Resources;

namespace Wuqi.Webdiyer
{
	internal sealed class SR
	{
		private ResourceManager _rm;

		private static SR _loader = null;

		private static object _lock = new object();

		private ResourceManager Resources
		{
			get
			{
				return this._rm;
			}
		}

		private SR()
		{
			this._rm = new ResourceManager("Wuqi.Webdiyer.AspNetPager", base.GetType().Assembly);
		}

		private static SR GetLoader()
		{
			if (SR._loader == null)
			{
				lock (SR._lock)
				{
					if (SR._loader == null)
					{
						SR._loader = new SR();
					}
				}
			}
			return SR._loader;
		}

		public static string GetString(string name)
		{
			SR loader = SR.GetLoader();
			string result = null;
			if (loader != null)
			{
				result = loader.Resources.GetString(name, null);
			}
			return result;
		}
	}
}
