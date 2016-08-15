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

		private Dictionary<string, HandleInfo> mHandles = new Dictionary<string, HandleInfo>();
		public IDictionary<string, HandleInfo> Handles
		{
			get
			{
				return mHandles;
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