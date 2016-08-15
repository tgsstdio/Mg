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
		public void TestMethod()
		{
			var interfaces = new Type[] { typeof(IMgInstance) };

			var implementation = new List<VkContainerClass>();

			foreach (var i in interfaces)
			{
				var container = new VkContainerClass { Name = i.Name.Replace("IMg", "Vk") };
				foreach (var info in i.GetMethods())
				{
					
				}
				implementation.Add(container);
			}

			Assert.AreEqual(1, implementation.Count);
			Assert.AreEqual("VkInstance", implementation[0].Name);
		}
	}

	class VkContainerClass
	{
		public string Name { get; set;}
	}
}

