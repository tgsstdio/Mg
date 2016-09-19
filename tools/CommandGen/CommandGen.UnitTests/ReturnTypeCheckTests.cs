using System;
using NUnit.Framework;

namespace CommandGen.UnitTests
{
	[TestFixture ()]
	public class ReturnTypeCheckTests
	{
		[TestCase]
		public void TestCase1 ()
		{
			var check = new VkReturnTypeCheck{ ReturnType = "Result", Variable = "first_call" };
			Assert.AreEqual ("if ( first_call == Result.Success ) return first_call;", check.GetImplementation ());
		}

		[TestCase]
		public void TestCase2 ()
		{
			var check = new VkReturnTypeCheck{ ReturnType = "object", Variable = "first_call" };
			Assert.AreEqual ("if ( first_call == null ) return first_call;", check.GetImplementation ());
		}

		[TestCase]
		public void TestCase3 ()
		{
			var check = new VkReturnTypeCheck{ ReturnType = "UInt32", Variable = "first_call" };
			Assert.AreEqual ("if ( first_call == 0 ) return first_call;", check.GetImplementation ());
		}
	}
}

