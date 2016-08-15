using NUnit.Framework;

namespace CommandGen.UnitTests
{
	[TestFixture]
	public class CallArgumentTests
	{
		[TestCase]
		public void IsNullTest ()
		{
			var arg = new VkFunctionArgument { Name = "pCount" };
			var call_arg = new VkCallArgument { Source = arg , IsNull = true};
			Assert.AreEqual ("null", call_arg.GetImplementation ());
		}

		[TestCase]
		public void IsNotNullTest ()
		{
			var arg = new VkFunctionArgument { Name = "pCount" };
			var call_arg = new VkCallArgument  { Source = arg , IsNull = false};
			Assert.AreEqual ("pCount", call_arg.GetImplementation ());
		}
	}
}

