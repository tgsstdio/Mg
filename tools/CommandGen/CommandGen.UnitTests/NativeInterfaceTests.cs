using System;
using Magnesium;
using NUnit.Framework;

namespace CommandGen.UnitTests
{
	[TestFixture()]
	public class NativeInterfaceTests
	{
		[TestCase]
		public void NoArguments()
		{
			var native = new VkNativeInterface { Name = "vkCreateInstance", ReturnType = "void" };
			native.UseUnsafe = false;

			Assert.AreEqual("internal external static void vkCreateInstance();", native.GetImplementation());
		}

		[TestCase]
		public void UsePointer()
		{
			// USE POINTER / unsafe IF
			// ALL ARGUMENTS ARE BLITTABLE
			// >= 1 ARGUMENTS

			var native = new VkNativeInterface { Name = "vkCreateInstance", ReturnType = "void" };
			native.UseUnsafe = true;
			native.Arguments.Add(
				new VkFunctionArgument
				{
					Name = "instance",
					ArgumentCsType = "IntPtr",
					UseOut = true,
				}
			);
			Assert.AreEqual("internal external static unsafe void vkCreateInstance(IntPtr* instance);", native.GetImplementation());
		}

		[TestCase]
		public void UseSafePointer()
		{
			// USE POINTER / unsafe ONLY IF
			// ALL ARGUMENTS ARE BLITTABLE
			// >= 1 ARGUMENTS

			var native = new VkNativeInterface { Name = "vkCreateInstance", ReturnType = "void" };
			native.UseUnsafe = false;
			native.Arguments.Add(
				new VkFunctionArgument
				{
					Name = "createInfo",
					ArgumentCsType = "InstanceCreateInfo",
					IsPointer = true,
					IsConst = true,
				}
			);
			native.Arguments.Add(
				new VkFunctionArgument
				{
					Name = "instance",
					ArgumentCsType = "IntPtr",
					UseOut = true,
					IsBlittable = true,
				}
			);
			Assert.AreEqual("internal external static void vkCreateInstance(InstanceCreateInfo createInfo, out IntPtr instance);", native.GetImplementation());
		}

		[TestCase]
		public void OutArgument()
		{
			var native = new VkNativeInterface { Name = "vkCreateInstance", ReturnType = "void" };
			native.UseUnsafe = false;
			native.Arguments.Add(
				new VkFunctionArgument
				{
					Name = "instance",
					ArgumentCsType = "IntPtr",
					UseOut = true,
				}
			);

			Assert.AreEqual("internal external static void vkCreateInstance(out IntPtr instance);", native.GetImplementation());
		}

		[TestCase]
		public void OutUnsafeUInt64Argument()
		{
			var native = new VkNativeInterface { Name = "vkCreateInstance", ReturnType = "void" };
			native.UseUnsafe = true;
			native.Arguments.Add(
				new VkFunctionArgument
				{
					Name = "instance",
					ArgumentCsType = "UInt64",
					UseOut = true,
				}
			);

			Assert.AreEqual("internal external static unsafe void vkCreateInstance(UInt64* instance);", native.GetImplementation());
		}

		[TestCase]
		public void Example_1()
		{
			var native = new VkNativeInterface { Name = "vkEnumeratePhysicalDevices", ReturnType = "Result" };
			native.UseUnsafe = false;
			native.Arguments.Add(
				new VkFunctionArgument
				{
					Name = "instance",
					ArgumentCsType = "IntPtr",
					UseOut = false,
				});
			native.Arguments.Add(
				new VkFunctionArgument
				{
					Name = "pPhysicalDeviceCount",
					ArgumentCsType = "UInt32",
					ByReference = true,
					UseOut = true,
				});
			native.Arguments.Add(
				new VkFunctionArgument
				{
					Name = "pPhysicalDevices",
					ArgumentCsType = "IntPtr",
					ByReference = false,
					UseOut = true,
				}
			);

			Assert.AreEqual("internal external static Result vkEnumeratePhysicalDevices(IntPtr instance, ref UInt32 pPhysicalDeviceCount, out IntPtr pPhysicalDevices);", native.GetImplementation());
		}

		[TestCase]
		public void Example_2()
		{
			var native = new VkNativeInterface { Name = "vkEnumerateInstanceLayerProperties", ReturnType = "UInt32" };
			native.UseUnsafe = false;
			native.Arguments.Add(
				new VkFunctionArgument
				{
					Name = "pPropertyCount",
					ArgumentCsType = "UInt32",
					ByReference = true,
					UseOut = true,
				});
			native.Arguments.Add(
				new VkFunctionArgument
				{
					Name = "pProperties",
					Attribute = "[In, Out]",
					ArgumentCsType = "LayerProperties",
					LengthVariable = "pPropertyCount",
					ByReference = false,
					UseOut = true,
				}
			);

			Assert.AreEqual("internal external static UInt32 vkEnumerateInstanceLayerProperties(ref UInt32 pPropertyCount, [In, Out] LayerProperties[] pProperties);", native.GetImplementation());
		}

	}
}

