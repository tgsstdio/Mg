using System;
using NUnit.Framework;

namespace CommandGen.UnitTests
{
	[TestFixture ()]
	public class VariableDeclarationTests
	{
		[Test ()]
		public void TestMethod1 ()
		{
			var source = new VkFunctionArgument{Name = "num", BaseCsType = "float" };
			var declare = new VkVariableDeclaration { Source = source };

			Assert.AreEqual ("var num = 0;", declare.GetImplementation());
		}

		[Test ()]
		public void FixedArray ()
		{
			var source = new VkFunctionArgument{Name = "num", BaseCsType = "UInt32", IsFixedArray = true, ArrayConstant = "10" };
			var declare = new VkVariableDeclaration { Source = source };

			Assert.AreEqual ("var num = new UInt32[10];", declare.GetImplementation());
		}
	}
}

