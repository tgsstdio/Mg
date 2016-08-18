using System;
using NUnit.Framework;

namespace CommandGen.UnitTests
{
	[TestFixture]
	public class MethodSignatureTests
	{
		[TestCase]
		public void NoArguments_0 ()
		{
			var signature = new VkMethodSignature { Name = "CreateInstance", ReturnType = "void" };	
			Assert.AreEqual ("public void CreateInstance()", signature.GetImplementation());
		}

		[TestCase]
		public void NoArguments_1 ()
		{
			var signature = new VkMethodSignature { Name = "CreateInstance", ReturnType = "void", IsStatic = true };	
			Assert.AreEqual ("public static void CreateInstance()", signature.GetImplementation());
		}

		[TestCase]
		public void NoArguments_2 ()
		{
			var signature = new VkMethodSignature { Name = "CreateInstance", ReturnType = "Result" };	
			Assert.AreEqual ("public Result CreateInstance()", signature.GetImplementation());
		}

		[TestCase]
		public void NoArguments_3 ()
		{
			var signature = new VkMethodSignature { Name = "CreateInstance", ReturnType = "Result", IsStatic = true };	
			Assert.AreEqual ("public static Result CreateInstance()", signature.GetImplementation());
		}

		[TestCase]
		public void OneArgument_0 ()
		{
			var signature = new VkMethodSignature { Name = "CreateInstance", ReturnType = "void" };	
			signature.Parameters.Add (new VkMethodParameter{Name = "pCount", BaseCsType = "UInt32"});
			Assert.AreEqual (1, signature.Parameters.Count);
			Assert.AreEqual ("public void CreateInstance(UInt32 pCount)", signature.GetImplementation());
		}

		[TestCase]
		public void OneArgument_1_Out ()
		{
			var signature = new VkMethodSignature { Name = "CreateInstance", ReturnType = "void" };	
			signature.Parameters.Add (new VkMethodParameter{Name = "pCount", BaseCsType = "UInt32", UseOut = true});
			Assert.AreEqual (1, signature.Parameters.Count);
			Assert.AreEqual ("public void CreateInstance(out UInt32 pCount)", signature.GetImplementation());
		}

		[TestCase]
		public void OneArgument_1_Ref ()
		{
			var signature = new VkMethodSignature { Name = "CreateInstance", ReturnType = "void" };	
			signature.Parameters.Add (new VkMethodParameter{Name = "pCount", BaseCsType = "UInt32", UseRef = true});
			Assert.AreEqual (1, signature.Parameters.Count);
			Assert.AreEqual ("public void CreateInstance(ref UInt32 pCount)", signature.GetImplementation());
		}
	}
}

