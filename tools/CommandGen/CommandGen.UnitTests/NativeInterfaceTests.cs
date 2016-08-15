using System;
using Magnesium;
using NUnit.Framework;

namespace CommandGen.UnitTests
{
	[TestFixture ()]
	public class NativeInterfaceTests
	{
		[TestCase]
		public void TestMethod ()
		{
			var interfaces = new Type[] { typeof(IMgInstance) };

			foreach (var i in interfaces)
			{
				var infos = i.GetMethods();
			}
		}
	}
}

