using System;
using NUnit.Framework;

namespace CommandGen.UnitTests
{
	[TestFixture ()]
	public class FunctionCallTests
	{
		[TestCase()]
		public void NoArguments_1()
		{
			var native = new VkNativeInterface { Name = "vkCreateInstance", ReturnType = "void" };
			var func = new VkFunctionCall { Call = native };

			Assert.AreEqual ("vkCreateInstance();", func.GetImplementation ());
		}

		[TestCase()]
		public void NoArguments_2()
		{
			var native = new VkNativeInterface { Name = "vkCreateInstance", ReturnType = "int" };
			var func = new VkFunctionCall { Call = native, Variable = "result", IsNew = true };

			Assert.AreEqual ("var result = vkCreateInstance();", func.GetImplementation ());
		}

		[TestCase()]
		public void NoArguments_3()
		{
			var native = new VkNativeInterface { Name = "vkCreateInstance", ReturnType = "int" };
			var func = new VkFunctionCall { Call = native, Variable = "result", IsNew = false };

			Assert.AreEqual ("result = vkCreateInstance();", func.GetImplementation ());
		}

		[TestCase()]
		public void OneArg_1()
		{
			var native = new VkNativeInterface { Name = "vkCreateInstance", ReturnType = "int" };
			var func = new VkFunctionCall { Call = native, Variable = "result", IsNew = false};

			func.Arguments.Add (new VkCallArgument{ Source = new VkFunctionArgument{Name = "pCount"}, IsNull = false});
			Assert.AreEqual (1, func.Arguments.Count);

			Assert.AreEqual ("result = vkCreateInstance(pCount);", func.GetImplementation ());
		}

		[TestCase()]
		public void OneArg_2()
		{
			var native = new VkNativeInterface { Name = "vkCreateInstance", ReturnType = "int" };
			var func = new VkFunctionCall { Call = native, Variable = "result", IsNew = false};

			func.Arguments.Add (new VkCallArgument{ Source = new VkFunctionArgument{Name = "pCount"}, IsNull = true});
			Assert.AreEqual (1, func.Arguments.Count);

			Assert.AreEqual ("result = vkCreateInstance(null);", func.GetImplementation ());
		}

		[TestCase()]
		public void OneArg_3()
		{
			var native = new VkNativeInterface { Name = "vkCreateInstance", ReturnType = "void" };
			var func = new VkFunctionCall { Call = native, Variable = "result", IsNew = false};

			func.Arguments.Add (new VkCallArgument{ Source = new VkFunctionArgument{Name = "pCount"}, IsNull = false});
			Assert.AreEqual (1, func.Arguments.Count);

			Assert.AreEqual ("vkCreateInstance(pCount);", func.GetImplementation ());
		}

		[TestCase()]
		public void OneArg_4()
		{
			var native = new VkNativeInterface { Name = "vkCreateInstance", ReturnType = "void" };
			var func = new VkFunctionCall { Call = native, Variable = "result", IsNew = false};

			func.Arguments.Add (new VkCallArgument{ Source = new VkFunctionArgument{Name = "pCount"}, IsNull = true});
			Assert.AreEqual (1, func.Arguments.Count);

			Assert.AreEqual ("vkCreateInstance(null);", func.GetImplementation ());
		}

		[TestCase()]
		public void TwoArgs_0()
		{
			var native = new VkNativeInterface { Name = "vkCreateInstance", ReturnType = "void" };
			var func = new VkFunctionCall { Call = native, Variable = "result", IsNew = false};

			func.Arguments.Add (new VkCallArgument{ Source = new VkFunctionArgument{Name = "pCount"}, IsNull = false});
			func.Arguments.Add (new VkCallArgument{ Source = new VkFunctionArgument{Name = "pArray"}, IsNull = true});
			Assert.AreEqual (2, func.Arguments.Count);

			Assert.AreEqual ("vkCreateInstance(pCount, null);", func.GetImplementation ());
		}

		[TestCase()]
		public void TwoArgs_1()
		{
			var native = new VkNativeInterface { Name = "vkCreateInstance", ReturnType = "void" };
			var func = new VkFunctionCall { Call = native, Variable = "result", IsNew = false};

			func.Arguments.Add (new VkCallArgument{ Source = new VkFunctionArgument{Name = "pCount"}, IsNull = false});
			func.Arguments.Add (new VkCallArgument{ Source = new VkFunctionArgument{Name = "pArray"}, IsNull = false});
			Assert.AreEqual (2, func.Arguments.Count);

			Assert.AreEqual ("vkCreateInstance(pCount, pArray);", func.GetImplementation ());
		}

		[TestCase()]
		public void TwoArgs_2()
		{
			var native = new VkNativeInterface { Name = "vkCreateInstance", ReturnType = "void" };
			var func = new VkFunctionCall { Call = native, Variable = "result", IsNew = false};

			func.Arguments.Add (new VkCallArgument{ Source = new VkFunctionArgument{Name = "pCount"}, IsNull = true});
			func.Arguments.Add (new VkCallArgument{ Source = new VkFunctionArgument{Name = "pArray"}, IsNull = true});
			Assert.AreEqual (2, func.Arguments.Count);

			Assert.AreEqual ("vkCreateInstance(null, null);", func.GetImplementation ());
		}
	}
}

