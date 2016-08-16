using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CommandGen.UnitTests
{
	class MockVkEntityInspector : IVkEntityInspector
	{
		public MockVkEntityInspector()
		{
		}

		public ISet<string> BlittableTypes
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public IDictionary<string, string> Translations
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		private Dictionary<string, VkHandleInfo> mHandles = new Dictionary<string, VkHandleInfo>();
		public IDictionary<string, VkHandleInfo> Handles
		{
			get
			{
				return mHandles;
			}
		}

		public IDictionary<string, VkEnumInfo> Enums
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public IDictionary<string, VkStructInfo> Structs
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public string GetTypeCsName(string name, string label)
		{
			throw new NotImplementedException();
		}

		public void Inspect(XElement root)
		{
			throw new NotImplementedException();
		}
	}
}