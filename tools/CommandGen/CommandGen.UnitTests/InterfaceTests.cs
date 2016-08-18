using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Magnesium;
using NUnit.Framework;

namespace CommandGen.UnitTests
{
	[TestFixture ()]
	public class InterfaceTests
	{
		public static string GetAssemblyDirectory(string fileName)
		{
			string codeBase = Assembly.GetExecutingAssembly().CodeBase;
			UriBuilder uri = new UriBuilder(codeBase);
			string path = Uri.UnescapeDataString(uri.Path);
			return System.IO.Path.Combine(Path.GetDirectoryName(path), fileName);
		}

		[TestCase]
		public void NoOfInterfaces()
		{
			var collection = new VkInterfaceCollection();

			Assert.AreEqual(29, collection.Interfaces.Count);
			var internalClass = collection.Interfaces[0];
			Assert.AreEqual("VkInstance", internalClass.Name);
			Assert.AreEqual("IMgInstance", internalClass.InterfaceName);
		}


	}

}

