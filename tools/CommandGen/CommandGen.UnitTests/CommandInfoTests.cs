﻿using NUnit.Framework;
using System.Xml.Linq;
using System.Collections.Generic;

namespace CommandGen.UnitTests
{
	[TestFixture()]
	public class CommandInfoTests
	{
		[Test()]
		public void TestCase1()
		{
			const string xml = @"
		        <command successcodes=""VK_SUCCESS"" errorcodes=""VK_ERROR_OUT_OF_HOST_MEMORY,VK_ERROR_OUT_OF_DEVICE_MEMORY,VK_ERROR_INITIALIZATION_FAILED,VK_ERROR_LAYER_NOT_PRESENT,VK_ERROR_EXTENSION_NOT_PRESENT,VK_ERROR_INCOMPATIBLE_DRIVER"">
		            <proto><type>VkResult</type> <name>vkCreateInstance</name></proto>
		            <param>const <type>VkInstanceCreateInfo</type>* <name>pCreateInfo</name></param>
		            <param optional=""true"">const <type>VkAllocationCallbacks</type>* <name>pAllocator</name></param>
		            <param><type>VkInstance</type>* <name>pInstance</name></param>
		        </command>";

			XElement top = XElement.Parse(xml, LoadOptions.PreserveWhitespace);

			var inspector = new VkEntityInspector();

			inspector.Handles.Add("VkInstance", new VkHandleInfo { name = "VkInstance", csType = "IntPtr" });
			Assert.AreEqual(1, inspector.Handles.Keys.Count);

			var parser = new VkCommandParser(inspector);



			VkCommandInfo command;
			var actual = parser.Parse(top, out command);
			Assert.IsTrue(actual);
			Assert.IsNotNull(command);
			Assert.AreEqual("vkCreateInstance", command.Name);
			Assert.AreEqual("VkResult", command.CppReturnType);
			Assert.AreEqual("VkResult", command.CsReturnType);
			Assert.IsNull(command.FirstInstance);

			// NATIVE FUNCTION
			var function = command.NativeFunction;
			Assert.IsNotNull(function, "command.NativeFunction is not null");
			Assert.AreEqual("VkResult", function.ReturnType);
			Assert.AreEqual(3, function.Arguments.Count);

			{
				const int index = 0;
				var arg_0 = function.Arguments[index];
				Assert.IsNotNull(arg_0);
				Assert.AreEqual(index, arg_0.Index);
				Assert.AreEqual("pCreateInfo", arg_0.Name);
				Assert.AreEqual("VkInstanceCreateInfo", arg_0.BaseCppType);
				Assert.AreEqual("VkInstanceCreateInfo", arg_0.BaseCsType);
				Assert.AreEqual("VkInstanceCreateInfo*", arg_0.ArgumentCppType);
				Assert.AreEqual("VkInstanceCreateInfo", arg_0.ArgumentCsType);
				Assert.IsTrue(arg_0.IsConst);
				Assert.IsFalse(arg_0.IsOptional);
				Assert.IsFalse(arg_0.IsFixedArray);
				Assert.IsNull(arg_0.LengthVariable);
				Assert.IsNull(arg_0.ArrayConstant);
				Assert.IsFalse(arg_0.UseOut);
				Assert.IsFalse(arg_0.ByReference);
				Assert.IsNull(arg_0.Attribute);
			}

			{
				const int index = 1;
				var arg_1 = function.Arguments[index];
				Assert.IsNotNull(arg_1);
				Assert.AreEqual(index, arg_1.Index);
				Assert.AreEqual("pAllocator", arg_1.Name);
				Assert.AreEqual("VkAllocationCallbacks", arg_1.BaseCppType);
				Assert.AreEqual("VkAllocationCallbacks", arg_1.BaseCsType);
				Assert.AreEqual("VkAllocationCallbacks*", arg_1.ArgumentCppType);
				Assert.AreEqual("IntPtr", arg_1.ArgumentCsType);
				Assert.IsTrue(arg_1.IsConst);
				Assert.IsTrue(arg_1.IsOptional);
				Assert.IsFalse(arg_1.IsFixedArray);
				Assert.IsNull(arg_1.LengthVariable);
				Assert.IsNull(arg_1.ArrayConstant);
				Assert.IsFalse(arg_1.UseOut);
				Assert.IsFalse(arg_1.ByReference);
				Assert.IsNull(arg_1.Attribute);
			}

			{
				const int index = 2;
				var arg_2 = function.Arguments[index];
				Assert.IsNotNull(arg_2);
				Assert.AreEqual(index, arg_2.Index);
				Assert.AreEqual("pInstance", arg_2.Name);
				Assert.AreEqual("VkInstance", arg_2.BaseCppType);
				Assert.AreEqual("VkInstance", arg_2.BaseCsType);
				Assert.AreEqual("VkInstance*", arg_2.ArgumentCppType);
				Assert.AreEqual("IntPtr", arg_2.ArgumentCsType);
				Assert.IsFalse(arg_2.IsConst);
				Assert.IsFalse(arg_2.IsOptional);
				Assert.IsFalse(arg_2.IsFixedArray);
				Assert.IsNull(arg_2.LengthVariable);
				Assert.IsNull(arg_2.ArrayConstant);
				Assert.IsTrue(arg_2.UseOut);
				Assert.IsFalse(arg_2.ByReference);
				Assert.IsNull(arg_2.Attribute);
			}

			// METHOD SIGNATURE

			var method = command.MethodSignature;
			Assert.IsNotNull(method, "command.MethodSignature is not null");
			Assert.AreEqual("CreateInstance", method.Name);
			Assert.AreEqual("VkResult", method.ReturnType);
			Assert.AreEqual(3, method.Parameters.Count);

			{
				const int index = 0;
				var param_0 = method.Parameters[index];
				Assert.IsNotNull(param_0);
				Assert.AreEqual(0, param_0.Source.Index);
				Assert.AreEqual("pCreateInfo", param_0.Name);
				Assert.IsFalse(param_0.IsNullableType);
				Assert.AreEqual("VkInstanceCreateInfo", param_0.BaseCsType);
				Assert.IsFalse(param_0.UseRef);
				Assert.IsFalse(param_0.IsFixedArray);
			}

			{
				const int index = 1;
				var param_1 = method.Parameters[index];
				Assert.IsNotNull(param_1);
				Assert.AreEqual(1, param_1.Source.Index);
				Assert.AreEqual("pAllocator", param_1.Name);
				Assert.IsTrue(param_1.IsNullableType);
				Assert.AreEqual("VkAllocationCallbacks", param_1.BaseCsType);
				Assert.IsFalse(param_1.UseRef);
				Assert.IsFalse(param_1.IsFixedArray);
				Assert.IsFalse(param_1.IsArrayParameter);
			}

			{
				const int index = 2;
				var param_1 = method.Parameters[index];
				Assert.IsNotNull(param_1);
				Assert.AreEqual(2, param_1.Source.Index);
				Assert.AreEqual("pInstance", param_1.Name);
				Assert.IsFalse(param_1.IsNullableType);
				Assert.AreEqual("VkInstance", param_1.BaseCsType);
				Assert.IsTrue(param_1.UseRef);
				Assert.IsFalse(param_1.IsFixedArray);
				Assert.IsFalse(param_1.IsArrayParameter);
			}

			Assert.AreEqual(2, command.Lines.Count);
			Assert.AreEqual(1, command.Calls.Count);

			{
				var call = command.Calls[0];
				Assert.IsNotNull(call);
				Assert.AreEqual(3, call.Arguments.Count);
			}

		}

		[Test()]
		public void TestCase2()
		{
			const string xml = @"
		        <command successcodes=""VK_SUCCESS,VK_INCOMPLETE"" errorcodes=""VK_ERROR_OUT_OF_HOST_MEMORY,VK_ERROR_OUT_OF_DEVICE_MEMORY,VK_ERROR_INITIALIZATION_FAILED"">
		            <proto><type>VkResult</type> <name>vkEnumeratePhysicalDevices</name></proto>
		            <param><type>VkInstance</type> <name>instance</name></param>
		            <param optional=""false,true""><type>uint32_t</type>* <name>pPhysicalDeviceCount</name></param>
		            <param optional=""true"" len=""pPhysicalDeviceCount""><type>VkPhysicalDevice</type>* <name>pPhysicalDevices</name></param>
		        </command>";

			XElement top = XElement.Parse(xml, LoadOptions.PreserveWhitespace);

			var inspector = new VkEntityInspector();
			inspector.Handles.Add("VkPhysicalDevice", new VkHandleInfo { name = "VkPhysicalDevice", csType = "IntPtr" });
			inspector.Handles.Add("VkInstance", new VkHandleInfo { name = "VkInstance", csType = "IntPtr" });
			Assert.AreEqual(2, inspector.Handles.Keys.Count);

			var parser = new VkCommandParser(inspector);

			VkCommandInfo command;
			var actual = parser.Parse(top, out command);
			Assert.IsTrue(actual);
			Assert.IsNotNull(command);
			Assert.AreEqual("vkEnumeratePhysicalDevices", command.Name);
			Assert.AreEqual("VkResult", command.CppReturnType);
			Assert.AreEqual("VkResult", command.CsReturnType);
			Assert.IsNotNull(command.FirstInstance);

			// NATIVE FUNCTION
			var function = command.NativeFunction;
			Assert.IsNotNull(function, "command.NativeFunction is not null");
			Assert.AreEqual("VkResult", function.ReturnType);
			Assert.AreEqual(3, function.Arguments.Count);

			{
				const int index = 0;
				var arg_0 = function.Arguments[index];
				Assert.IsNotNull(arg_0);
				Assert.AreEqual(index, arg_0.Index);
				Assert.AreEqual("instance", arg_0.Name);
				Assert.AreEqual("VkInstance", arg_0.BaseCppType);
				Assert.AreEqual("VkInstance", arg_0.BaseCsType);
				Assert.AreEqual("VkInstance", arg_0.ArgumentCppType);
				Assert.AreEqual("IntPtr", arg_0.ArgumentCsType);
				Assert.IsFalse(arg_0.IsConst);
				Assert.IsFalse(arg_0.IsOptional);
				Assert.IsFalse(arg_0.IsFixedArray);
				Assert.IsNull(arg_0.LengthVariable);
				Assert.IsNull(arg_0.ArrayConstant);
				Assert.IsFalse(arg_0.UseOut);
				Assert.IsTrue(arg_0.IsBlittable);
				Assert.IsFalse(arg_0.ByReference);
				Assert.IsNull(arg_0.Attribute);
			}

			{
				const int index = 1;
				var arg_1 = function.Arguments[index];
				Assert.IsNotNull(arg_1);
				Assert.AreEqual(index, arg_1.Index);
				Assert.AreEqual("pPhysicalDeviceCount", arg_1.Name);
				Assert.AreEqual("uint32_t", arg_1.BaseCppType);
				Assert.AreEqual("UInt32", arg_1.BaseCsType);
				Assert.AreEqual("uint32_t*", arg_1.ArgumentCppType);
				Assert.AreEqual("UInt32", arg_1.ArgumentCsType);
				Assert.IsFalse(arg_1.IsConst);
				Assert.IsFalse(arg_1.IsOptional);
				Assert.IsFalse(arg_1.IsFixedArray);
				Assert.IsNull(arg_1.LengthVariable);
				Assert.IsNull(arg_1.ArrayConstant);
				Assert.IsTrue(arg_1.UseOut);
				Assert.IsTrue(arg_1.IsBlittable);
				Assert.IsTrue(arg_1.ByReference);
				Assert.IsNull(arg_1.Attribute);
			}

			{
				const int index = 2;
				var arg_2 = function.Arguments[index];
				Assert.IsNotNull(arg_2);
				Assert.AreEqual(index, arg_2.Index);
				Assert.AreEqual("pPhysicalDevices", arg_2.Name);
				Assert.AreEqual("VkPhysicalDevice", arg_2.BaseCppType);
				Assert.AreEqual("VkPhysicalDevice", arg_2.BaseCsType);
				Assert.AreEqual("VkPhysicalDevice*", arg_2.ArgumentCppType);
				Assert.AreEqual("IntPtr", arg_2.ArgumentCsType);
				Assert.IsFalse(arg_2.IsConst);
				Assert.IsTrue(arg_2.IsOptional);
				Assert.IsFalse(arg_2.IsFixedArray);
				Assert.AreEqual("pPhysicalDeviceCount", arg_2.LengthVariable);
				Assert.IsNull(arg_2.ArrayConstant);
				Assert.IsTrue(arg_2.UseOut);
				Assert.IsTrue(arg_2.IsBlittable);
				Assert.IsFalse(arg_2.ByReference);
				Assert.IsNull(arg_2.Attribute);
			}

			// METHOD SIGNATURE
			var method = command.MethodSignature;
			Assert.IsNotNull(method, "command.MethodSignature is not null");
			Assert.AreEqual("EnumeratePhysicalDevices", method.Name);
			Assert.AreEqual("VkResult", method.ReturnType);
			Assert.AreEqual(1, method.Parameters.Count);

			{
				const int index = 0;
				var param_0 = method.Parameters[index];
				Assert.IsNotNull(param_0);
				Assert.AreEqual(2, param_0.Source.Index);
				Assert.AreEqual("pPhysicalDevices", param_0.Name);
				Assert.IsTrue(param_0.IsNullableType);
				Assert.AreEqual("VkPhysicalDevice", param_0.BaseCsType);
				Assert.IsFalse(param_0.UseRef);
				Assert.IsFalse(param_0.IsFixedArray);
				Assert.IsTrue(param_0.IsArrayParameter);
			}

			Assert.AreEqual(5, command.Lines.Count);
			Assert.AreEqual(2, command.Calls.Count);

			{
				var call = command.Calls[0];
				Assert.IsNotNull(call);
				Assert.AreEqual(3, call.Arguments.Count);
			}

			{
				var call = command.Calls[1];
				Assert.IsNotNull(call);
				Assert.AreEqual(3, call.Arguments.Count);
			}
		}

		[Test()]
		public void TestCase3()
		{
			const string xml = @"
		        <command successcodes=""VK_SUCCESS,VK_INCOMPLETE"" errorcodes=""VK_ERROR_OUT_OF_HOST_MEMORY,VK_ERROR_OUT_OF_DEVICE_MEMORY"">
		            <proto><type>VkResult</type> <name>vkEnumerateDeviceLayerProperties</name></proto>
		            <param optional=""false,true""><type>VkPhysicalDevice</type> <name>physicalDevice</name></param>
		            <param optional=""false,true""><type>uint32_t</type>* <name>pPropertyCount</name></param>
		            <param optional=""true"" len=""pPropertyCount""><type>VkLayerProperties</type>* <name>pProperties</name></param>
		        </command>";

			XElement top = XElement.Parse(xml, LoadOptions.PreserveWhitespace);

			var inspector = new VkEntityInspector();

			inspector.Handles.Add("VkPhysicalDevice", new VkHandleInfo { name = "VkPhysicalDevice", csType = "IntPtr" });
			Assert.AreEqual(1, inspector.Handles.Keys.Count);

			inspector.Structs.Add("VkLayerProperties", new VkStructInfo { Name = "VkLayerProperties" });
			Assert.AreEqual(1, inspector.Structs.Keys.Count);

			var parser = new VkCommandParser(inspector);

			VkCommandInfo command;
			var actual = parser.Parse(top, out command);
			Assert.IsTrue(actual);
			Assert.IsNotNull(command);
			Assert.AreEqual("vkEnumerateDeviceLayerProperties", command.Name);
			Assert.AreEqual("VkResult", command.CppReturnType);
			Assert.AreEqual("VkResult", command.CsReturnType);
			Assert.IsNotNull(command.FirstInstance);

			// NATIVE FUNCTION
			var function = command.NativeFunction;
			Assert.IsNotNull(function, "command.NativeFunction is not null");
			Assert.AreEqual("vkEnumerateDeviceLayerProperties", function.Name);
			Assert.AreEqual("VkResult", function.ReturnType);
			Assert.AreEqual(3, function.Arguments.Count);

			{
				const int index = 0;
				var arg_0 = function.Arguments[index];
				Assert.IsNotNull(arg_0);
				Assert.AreEqual(index, arg_0.Index);
				Assert.AreEqual("physicalDevice", arg_0.Name);
				Assert.AreEqual("VkPhysicalDevice", arg_0.BaseCppType);
				Assert.AreEqual("VkPhysicalDevice", arg_0.BaseCsType);
				Assert.AreEqual("VkPhysicalDevice", arg_0.ArgumentCppType);
				Assert.AreEqual("IntPtr", arg_0.ArgumentCsType);
				Assert.IsFalse(arg_0.IsConst);
				Assert.IsFalse(arg_0.IsOptional);
				Assert.IsFalse(arg_0.IsFixedArray);
				Assert.IsNull(arg_0.LengthVariable);
				Assert.IsNull(arg_0.ArrayConstant);
				Assert.IsFalse(arg_0.UseOut);
				Assert.IsTrue(arg_0.ByReference);
				Assert.IsNull(arg_0.Attribute);
			}

			{
				const int index = 1;
				var arg_1 = function.Arguments[index];
				Assert.IsNotNull(arg_1);
				Assert.AreEqual(index, arg_1.Index);
				Assert.AreEqual("pPropertyCount", arg_1.Name);
				Assert.AreEqual("uint32_t", arg_1.BaseCppType);
				Assert.AreEqual("UInt32", arg_1.BaseCsType);
				Assert.AreEqual("uint32_t*", arg_1.ArgumentCppType);
				Assert.AreEqual("UInt32", arg_1.ArgumentCsType);
				Assert.IsFalse(arg_1.IsConst);
				Assert.IsFalse(arg_1.IsOptional);
				Assert.IsFalse(arg_1.IsFixedArray);
				Assert.IsNull(arg_1.LengthVariable);
				Assert.IsNull(arg_1.ArrayConstant);
				Assert.IsTrue(arg_1.UseOut);
				Assert.IsTrue(arg_1.ByReference);
				Assert.IsNull(arg_1.Attribute);
			}

			{
				const int index = 2;
				var arg_2 = function.Arguments[index];
				Assert.IsNotNull(arg_2);
				Assert.AreEqual(index, arg_2.Index);
				Assert.AreEqual("pProperties", arg_2.Name);
				Assert.AreEqual("VkLayerProperties", arg_2.BaseCppType);
				Assert.AreEqual("VkLayerProperties", arg_2.BaseCsType);
				Assert.AreEqual("VkLayerProperties*", arg_2.ArgumentCppType);
				Assert.AreEqual("VkLayerProperties", arg_2.ArgumentCsType);
				Assert.IsFalse(arg_2.IsConst);
				Assert.IsTrue(arg_2.IsOptional);
				Assert.IsFalse(arg_2.IsFixedArray);
				Assert.AreEqual("pPropertyCount", arg_2.LengthVariable);
				Assert.AreEqual("[In, Out]", arg_2.Attribute);
				Assert.IsNull(arg_2.ArrayConstant);
				Assert.IsTrue(arg_2.UseOut);
				Assert.IsFalse(arg_2.ByReference);
			}

			// METHOD SIGNATURE
			var method = command.MethodSignature;
			Assert.IsNotNull(method, "command.MethodSignature is not null");
			Assert.AreEqual("EnumerateDeviceLayerProperties", method.Name);
			Assert.AreEqual("VkResult", method.ReturnType);
			Assert.AreEqual(1, method.Parameters.Count);

			{
				const int index = 0;
				var param_0 = method.Parameters[index];
				Assert.IsNotNull(param_0);
				Assert.AreEqual(2, param_0.Source.Index);
				Assert.AreEqual("pProperties", param_0.Name);
				Assert.IsTrue(param_0.IsNullableType);
				Assert.AreEqual("VkLayerProperties", param_0.BaseCsType);
				Assert.IsFalse(param_0.UseRef);
				Assert.IsFalse(param_0.IsFixedArray);
				Assert.IsTrue(param_0.IsArrayParameter);
			}

			Assert.AreEqual(5, command.Lines.Count);
			Assert.AreEqual(2, command.Calls.Count);

			{
				var call = command.Calls[0];
				Assert.IsNotNull(call);
				Assert.AreEqual(3, call.Arguments.Count);
			}

			{
				var call = command.Calls[1];
				Assert.IsNotNull(call);
				Assert.AreEqual(3, call.Arguments.Count);
			}
		}

		[Test()]
		public void TestCase4()
		{
			const string xml = @"
		        <command successcodes=""VK_SUCCESS,VK_INCOMPLETE"" errorcodes=""VK_ERROR_OUT_OF_HOST_MEMORY,VK_ERROR_OUT_OF_DEVICE_MEMORY"">
		            <proto><type>VkResult</type> <name>vkEnumerateInstanceLayerProperties</name></proto>
		            <param optional=""false,true""><type>uint32_t</type>* <name>pPropertyCount</name></param>
		            <param optional=""true"" len=""pPropertyCount""><type>VkLayerProperties</type>* <name>pProperties</name></param>
		        </command>";

			XElement top = XElement.Parse(xml, LoadOptions.PreserveWhitespace);

			var inspector = new VkEntityInspector();
			Assert.AreEqual(0, inspector.Handles.Keys.Count);

			inspector.Structs.Add("VkLayerProperties", new VkStructInfo { Name = "VkLayerProperties" });
			Assert.AreEqual(1, inspector.Structs.Keys.Count);

			// LayerProperties IS NOT BLITTABLE
			Assert.IsFalse(inspector.BlittableTypes.Contains("VkLayerProperties"));

			var parser = new VkCommandParser(inspector);

			VkCommandInfo command;
			var actual = parser.Parse(top, out command);
			Assert.IsTrue(actual);
			Assert.IsNotNull(command);
			Assert.AreEqual("vkEnumerateInstanceLayerProperties", command.Name);
			Assert.AreEqual("VkResult", command.CppReturnType);
			Assert.AreEqual("VkResult", command.CsReturnType);
			Assert.IsNull(command.FirstInstance);

			// NATIVE FUNCTION
			var function = command.NativeFunction;
			Assert.IsNotNull(function, "command.NativeFunction is not null");
			Assert.AreEqual("VkResult", function.ReturnType);
			Assert.AreEqual(2, function.Arguments.Count);
			Assert.IsFalse(function.UseUnsafe);

			{
				const int index = 0;
				var arg_0 = function.Arguments[index];
				Assert.IsNotNull(arg_0);
				Assert.AreEqual(index, arg_0.Index);
				Assert.AreEqual("pPropertyCount", arg_0.Name);
				Assert.AreEqual("uint32_t", arg_0.BaseCppType);
				Assert.AreEqual("UInt32", arg_0.BaseCsType);
				Assert.AreEqual("uint32_t*", arg_0.ArgumentCppType);
				Assert.AreEqual("UInt32", arg_0.ArgumentCsType);
				Assert.IsFalse(arg_0.IsConst);
				Assert.IsFalse(arg_0.IsOptional);
				Assert.IsFalse(arg_0.IsFixedArray);
				Assert.IsNull(arg_0.LengthVariable);
				Assert.IsNull(arg_0.ArrayConstant);
				Assert.IsTrue(arg_0.UseOut);
				Assert.IsTrue(arg_0.ByReference);
				Assert.IsNull(arg_0.Attribute);
			}

			{
				const int index = 1;
				var arg_1 = function.Arguments[index];
				Assert.IsNotNull(arg_1);
				Assert.AreEqual(index, arg_1.Index);
				Assert.AreEqual("pProperties", arg_1.Name);
				Assert.AreEqual("VkLayerProperties", arg_1.BaseCppType);
				Assert.AreEqual("VkLayerProperties", arg_1.BaseCsType);
				Assert.AreEqual("VkLayerProperties*", arg_1.ArgumentCppType);
				Assert.AreEqual("VkLayerProperties", arg_1.ArgumentCsType);
				Assert.IsFalse(arg_1.IsConst);
				Assert.IsTrue(arg_1.IsOptional);
				Assert.IsFalse(arg_1.IsFixedArray);
				Assert.AreEqual("pPropertyCount", arg_1.LengthVariable);
				Assert.AreEqual("[In, Out]", arg_1.Attribute);
				Assert.IsNull(arg_1.ArrayConstant);
				Assert.IsTrue(arg_1.UseOut);
				Assert.IsFalse(arg_1.ByReference);
			}

			// METHOD SIGNATURE
			var method = command.MethodSignature;
			Assert.IsNotNull(method, "command.MethodSignature is not null");
			Assert.AreEqual("EnumerateInstanceLayerProperties", method.Name);
			Assert.AreEqual("VkResult", method.ReturnType);
			Assert.AreEqual(1, method.Parameters.Count);

			{
				const int index = 0;
				var param_0 = method.Parameters[index];
				Assert.IsNotNull(param_0);
				Assert.AreEqual(1, param_0.Source.Index);
				Assert.AreEqual("pProperties", param_0.Name);
				Assert.IsTrue(param_0.IsNullableType);
				Assert.AreEqual("VkLayerProperties", param_0.BaseCsType);
				Assert.IsFalse(param_0.UseRef);
				Assert.IsFalse(param_0.IsFixedArray);
				Assert.IsTrue(param_0.IsArrayParameter);
			}

			Assert.AreEqual(5, command.Lines.Count);
			Assert.AreEqual(2, command.Calls.Count);

			{
				var call = command.Calls[0];
				Assert.IsNotNull(call);
				Assert.AreEqual(2, call.Arguments.Count);
			}

			{
				var call = command.Calls[1];
				Assert.IsNotNull(call);
				Assert.AreEqual(2, call.Arguments.Count);
			}
		}

		[Test()]
		public void TestCase5()
		{
			const string xml = @"
		        <command queues=""graphics"" renderpass=""both"" cmdbufferlevel=""primary,secondary"">
		            <proto><type>void</type> <name>vkCmdSetBlendConstants</name></proto>
		            <param externsync=""true""><type>VkCommandBuffer</type> <name>commandBuffer</name></param>
		            <param>const <type>float</type> <name>blendConstants</name>[4]</param>
		        </command>";

			XElement top = XElement.Parse(xml, LoadOptions.PreserveWhitespace);

			var inspector = new VkEntityInspector();
			inspector.Handles.Add("VkCommandBuffer", new VkHandleInfo { name = "VkCommandBuffer", csType = "IntPtr" });
			Assert.AreEqual(1, inspector.Handles.Keys.Count);

			var parser = new VkCommandParser(inspector);

			VkCommandInfo command;
			bool actual = parser.Parse(top, out command);
			Assert.IsTrue(actual);
			Assert.IsNotNull(command);
			Assert.AreEqual("vkCmdSetBlendConstants", command.Name);
			Assert.AreEqual("void", command.CppReturnType);
			Assert.AreEqual("void", command.CsReturnType);
			Assert.IsNotNull(command.FirstInstance);

			// NATIVE FUNCTION
			var function = command.NativeFunction;
			Assert.IsNotNull(function, "command.NativeFunction is not null");
			Assert.AreEqual("vkCmdSetBlendConstants", function.Name);
			Assert.AreEqual("void", function.ReturnType);
			Assert.AreEqual(2, function.Arguments.Count);
			Assert.IsFalse(function.UseUnsafe);

			{
				const int index = 0;
				var arg_0 = function.Arguments[index];
				Assert.IsNotNull(arg_0);
				Assert.AreEqual(index, arg_0.Index);
				Assert.AreEqual("commandBuffer", arg_0.Name);
				Assert.AreEqual("VkCommandBuffer", arg_0.BaseCppType);
				Assert.AreEqual("VkCommandBuffer", arg_0.BaseCsType);
				Assert.AreEqual("VkCommandBuffer", arg_0.ArgumentCppType);
				Assert.AreEqual("IntPtr", arg_0.ArgumentCsType);
				Assert.IsFalse(arg_0.IsConst);
				Assert.IsFalse(arg_0.IsOptional);
				Assert.IsFalse(arg_0.IsFixedArray);
				Assert.IsNull(arg_0.LengthVariable);
				Assert.IsNull(arg_0.ArrayConstant);
				Assert.IsFalse(arg_0.UseOut);
				Assert.IsFalse(arg_0.ByReference);
				Assert.IsNull(arg_0.Attribute);
			}

			{
				const int index = 1;
				var arg_1 = function.Arguments[index];
				Assert.IsNotNull(arg_1);
				Assert.AreEqual(index, arg_1.Index);
				Assert.AreEqual("blendConstants", arg_1.Name);
				Assert.AreEqual("float", arg_1.BaseCppType);
				Assert.AreEqual("float", arg_1.BaseCsType);
				Assert.AreEqual("float", arg_1.ArgumentCppType);
				Assert.AreEqual("float", arg_1.ArgumentCsType);
				Assert.IsTrue(arg_1.IsConst);
				Assert.IsFalse(arg_1.IsOptional);
				Assert.IsTrue(arg_1.IsFixedArray);
				Assert.IsNull(arg_1.LengthVariable);
				Assert.AreEqual("4", arg_1.ArrayConstant);
				Assert.IsTrue(arg_1.IsBlittable);
				Assert.AreEqual("[MarshalAs(UnmanagedType.LPArray, SizeConst = 4)]", arg_1.Attribute);
				Assert.IsFalse(arg_1.UseOut);
				Assert.IsFalse(arg_1.ByReference);
			}

			// METHOD SIGNATURE
			var method = command.MethodSignature;
			Assert.IsNotNull(method, "command.MethodSignature is not null");
			Assert.AreEqual("CmdSetBlendConstants", method.Name);
			Assert.AreEqual(1, method.Parameters.Count);

			{
				const int index = 0;
				var param_0 = method.Parameters[index];
				Assert.IsNotNull(param_0);
				Assert.AreEqual(1, param_0.Source.Index);
				Assert.AreEqual("blendConstants", param_0.Name);
				Assert.IsFalse(param_0.IsNullableType);
				Assert.AreEqual("float", param_0.BaseCsType);
				Assert.IsFalse(param_0.UseRef);
				Assert.IsTrue(param_0.IsFixedArray);
				Assert.IsFalse(param_0.IsArrayParameter);
			}

			Assert.AreEqual(1, command.Lines.Count);
			Assert.AreEqual(1, command.Calls.Count);

			{
				var call = command.Calls[0];
				Assert.IsNotNull(call);
				Assert.AreEqual(2, call.Arguments.Count);
			}
		}

		[Test()]
		public void TestCase6()
		{
			const string xml = @"
	        <command successcodes=""VK_SUCCESS"" errorcodes=""VK_ERROR_OUT_OF_HOST_MEMORY,VK_ERROR_OUT_OF_DEVICE_MEMORY"">
	            <proto><type>VkResult</type> <name>vkCreateQueryPool</name></proto>
	            <param><type>VkDevice</type> <name>device</name></param>
	            <param>const <type>VkQueryPoolCreateInfo</type>* <name>pCreateInfo</name></param>
	            <param optional=""true"">const <type>VkAllocationCallbacks</type>* <name>pAllocator</name></param>
	            <param><type>VkQueryPool</type>* <name>pQueryPool</name></param>
	        </command>";

			XElement top = XElement.Parse(xml, LoadOptions.PreserveWhitespace);

			var inspector = new VkEntityInspector();

			inspector.Handles.Add("VkDevice", new VkHandleInfo { name = "VkDevice", csType = "IntPtr" });
			inspector.Handles.Add("VkCommandBuffer", new VkHandleInfo { name = "VkCommandBuffer", csType = "IntPtr" });
			inspector.Handles.Add("VkQueryPool", new VkHandleInfo { name = "VkQueryPool", csType = "UInt64" });
			Assert.AreEqual(3, inspector.Handles.Keys.Count);

			var parser = new VkCommandParser(inspector);

			VkCommandInfo command;
			bool actual = parser.Parse(top, out command);
			Assert.IsTrue(actual);
			Assert.IsNotNull(command);
			Assert.AreEqual("vkCreateQueryPool", command.Name);
			Assert.AreEqual("VkResult", command.CppReturnType);
			Assert.AreEqual("VkResult", command.CsReturnType);
			Assert.IsNotNull(command.FirstInstance);

			// NATIVE FUNCTION
			var function = command.NativeFunction;
			Assert.IsNotNull(function, "command.NativeFunction is not null");
			Assert.AreEqual("vkCreateQueryPool", function.Name);
			Assert.AreEqual("VkResult", function.ReturnType);
			Assert.AreEqual(4, function.Arguments.Count);
			Assert.IsFalse(function.UseUnsafe);

			{
				const int index = 0;
				var arg_0 = function.Arguments[index];
				Assert.IsNotNull(arg_0);
				Assert.AreEqual(index, arg_0.Index);
				Assert.AreEqual("device", arg_0.Name);
				Assert.AreEqual("VkDevice", arg_0.BaseCppType);
				Assert.AreEqual("VkDevice", arg_0.BaseCsType);
				Assert.AreEqual("VkDevice", arg_0.ArgumentCppType);
				Assert.AreEqual("IntPtr", arg_0.ArgumentCsType);
				Assert.IsFalse(arg_0.IsConst);
				Assert.IsFalse(arg_0.IsOptional);
				Assert.IsFalse(arg_0.IsFixedArray);
				Assert.IsNull(arg_0.LengthVariable);
				Assert.IsNull(arg_0.ArrayConstant);
				Assert.IsFalse(arg_0.UseOut);
				Assert.IsFalse(arg_0.ByReference);
			}

			{
				const int index = 1;
				var arg_1 = function.Arguments[index];
				Assert.IsNotNull(arg_1);
				Assert.AreEqual(index, arg_1.Index);
				Assert.AreEqual("pCreateInfo", arg_1.Name);
				Assert.AreEqual("VkQueryPoolCreateInfo", arg_1.BaseCppType);
				Assert.AreEqual("VkQueryPoolCreateInfo", arg_1.BaseCsType);
				Assert.AreEqual("VkQueryPoolCreateInfo*", arg_1.ArgumentCppType);
				Assert.AreEqual("VkQueryPoolCreateInfo", arg_1.ArgumentCsType);
				Assert.IsTrue(arg_1.IsConst);
				Assert.IsFalse(arg_1.IsOptional);
				Assert.IsFalse(arg_1.IsFixedArray);
				Assert.IsNull(arg_1.LengthVariable);
				Assert.IsNull(arg_1.ArrayConstant);
				Assert.IsFalse(arg_1.UseOut);
				Assert.IsFalse(arg_1.ByReference);
			}

			{
				const int index = 2;
				var arg_2 = function.Arguments[index];
				Assert.IsNotNull(arg_2);
				Assert.AreEqual(index, arg_2.Index);
				Assert.AreEqual("pAllocator", arg_2.Name);
				Assert.AreEqual("VkAllocationCallbacks", arg_2.BaseCppType);
				Assert.AreEqual("VkAllocationCallbacks", arg_2.BaseCsType);
				Assert.AreEqual("VkAllocationCallbacks*", arg_2.ArgumentCppType);
				Assert.AreEqual("IntPtr", arg_2.ArgumentCsType);
				Assert.IsTrue(arg_2.IsConst);
				Assert.IsTrue(arg_2.IsOptional);
				Assert.IsFalse(arg_2.IsFixedArray);
				Assert.IsNull(arg_2.LengthVariable);
				Assert.IsNull(arg_2.ArrayConstant);
				Assert.IsFalse(arg_2.UseOut);
				Assert.IsFalse(arg_2.ByReference);
			}

			{
				const int index = 3;
				var arg_3 = function.Arguments[index];
				Assert.IsNotNull(arg_3);
				Assert.AreEqual(index, arg_3.Index);
				Assert.AreEqual("pQueryPool", arg_3.Name);
				Assert.AreEqual("VkQueryPool", arg_3.BaseCppType);
				Assert.AreEqual("VkQueryPool", arg_3.BaseCsType);
				Assert.AreEqual("VkQueryPool*", arg_3.ArgumentCppType);
				Assert.AreEqual("UInt64", arg_3.ArgumentCsType);
				Assert.IsFalse(arg_3.IsConst);
				Assert.IsFalse(arg_3.IsOptional);
				Assert.IsFalse(arg_3.IsFixedArray);
				Assert.IsNull(arg_3.LengthVariable);
				Assert.IsNull(arg_3.ArrayConstant);
				Assert.IsTrue(arg_3.UseOut);
				Assert.IsFalse(arg_3.ByReference);
			}

			Assert.AreEqual(2, command.Lines.Count);
			Assert.AreEqual(1, command.Calls.Count);

			{
				var call = command.Calls[0];
				Assert.IsNotNull(call);
				Assert.AreEqual(4, call.Arguments.Count);
			}
		}

		[Test()]
		public void TestCase7()
		{
			const string xml = @"
			<command successcodes=""VK_SUCCESS,VK_SUBOPTIMAL_KHR"" errorcodes=""VK_ERROR_OUT_OF_HOST_MEMORY,VK_ERROR_OUT_OF_DEVICE_MEMORY,VK_ERROR_DEVICE_LOST,VK_ERROR_OUT_OF_DATE_KHR,VK_ERROR_SURFACE_LOST_KHR"">
	            <proto><type>VkResult</type> <name>vkQueuePresentKHR</name></proto>
		            <param externsync=""true""><type>VkQueue</type> <name>queue</name></param>
		            <param externsync=""pPresentInfo.pWaitSemaphores[],pPresentInfo.pSwapchains[]"">const <type>VkPresentInfoKHR</type>* <name>pPresentInfo</name></param>
	            <validity>
               	 <usage>Any given element of pname:pSwapchains member of pname:pPresentInfo must: be a swapchain that is created for a surface for which presentation is supported from pname:queue as determined using a call to fname:vkGetPhysicalDeviceSurfaceSupportKHR</usage>
            	</validity>
        	</command>";

			XElement top = XElement.Parse(xml, LoadOptions.PreserveWhitespace);

			var inspector = new VkEntityInspector();

			inspector.Handles.Add("VkQueue", new VkHandleInfo { name = "VkQueue", csType = "IntPtr" });
			Assert.AreEqual(1, inspector.Handles.Keys.Count);

			inspector.Structs.Add("VkPresentInfoKhr", new VkStructInfo { Name = "VkPresentInfoKhr" });
			Assert.AreEqual(1, inspector.Structs.Keys.Count);

			var parser = new VkCommandParser(inspector);

			VkCommandInfo command;
			bool actual = parser.Parse(top, out command);
			Assert.IsTrue(actual);
			Assert.IsNotNull(command);
			Assert.AreEqual("vkQueuePresentKHR", command.Name);
			Assert.AreEqual("VkResult", command.CppReturnType);
			Assert.AreEqual("VkResult", command.CsReturnType);
			Assert.IsNotNull(command.FirstInstance);

			// NATIVE FUNCTION
			var function = command.NativeFunction;
			Assert.IsNotNull(function, "command.NativeFunction is not null");
			Assert.AreEqual("vkQueuePresentKHR", function.Name);
			Assert.AreEqual("VkResult", function.ReturnType);
			Assert.AreEqual(2, function.Arguments.Count);
			Assert.IsFalse(function.UseUnsafe);

			{
				const int index = 0;
				var arg_0 = function.Arguments[index];
				Assert.IsNotNull(arg_0);
				Assert.AreEqual(index, arg_0.Index);
				Assert.AreEqual("queue", arg_0.Name);
				Assert.AreEqual("VkQueue", arg_0.BaseCppType);
				Assert.AreEqual("VkQueue", arg_0.BaseCsType);
				Assert.AreEqual("VkQueue", arg_0.ArgumentCppType);
				Assert.AreEqual("IntPtr", arg_0.ArgumentCsType);
				Assert.IsFalse(arg_0.IsConst);
				Assert.IsFalse(arg_0.IsOptional);
				Assert.IsFalse(arg_0.IsFixedArray);
				Assert.IsNull(arg_0.LengthVariable);
				Assert.IsNull(arg_0.ArrayConstant);
				Assert.IsFalse(arg_0.UseOut);
				Assert.IsFalse(arg_0.ByReference);
				Assert.IsNull(arg_0.Attribute);
			}

			{
				const int index = 1;
				var arg_1 = function.Arguments[index];
				Assert.IsNotNull(arg_1);
				Assert.AreEqual(index, arg_1.Index);
				Assert.AreEqual("pPresentInfo", arg_1.Name);
				Assert.AreEqual("VkPresentInfoKHR", arg_1.BaseCppType);
				Assert.AreEqual("VkPresentInfoKHR", arg_1.BaseCsType);
				Assert.AreEqual("VkPresentInfoKHR*", arg_1.ArgumentCppType);
				Assert.AreEqual("VkPresentInfoKHR", arg_1.ArgumentCsType);
				Assert.IsTrue(arg_1.IsConst);
				Assert.IsFalse(arg_1.IsOptional);
				Assert.IsFalse(arg_1.IsFixedArray);
				Assert.IsNull(arg_1.LengthVariable);
				Assert.IsNull(arg_1.ArrayConstant);
				Assert.IsFalse(arg_1.UseOut);
				Assert.IsFalse(arg_1.ByReference);
				Assert.IsNull(arg_1.Attribute);
			}

			// METHOD SIGNATURE
			var method = command.MethodSignature;
			Assert.IsNotNull(method, "command.MethodSignature is not null");
			Assert.AreEqual("QueuePresentKHR", method.Name);
			Assert.AreEqual(1, method.Parameters.Count);

			{
				const int index = 0;
				var param_0 = method.Parameters[index];
				Assert.IsNotNull(param_0);
				Assert.AreEqual(1, param_0.Source.Index);
				Assert.AreEqual("pPresentInfo", param_0.Name);
				Assert.IsFalse(param_0.IsNullableType);
				Assert.AreEqual("VkPresentInfoKHR", param_0.BaseCsType);
				Assert.IsFalse(param_0.UseRef);
				Assert.IsFalse(param_0.IsFixedArray);
				Assert.IsFalse(param_0.IsArrayParameter);
			}

			Assert.AreEqual(1, command.Lines.Count);
			Assert.AreEqual(1, command.Calls.Count);

			{
				var call = command.Calls[0];
				Assert.IsNotNull(call);
				Assert.AreEqual(2, call.Arguments.Count);
			}
		}

		[Test()]
		public void TestCase8()
		{
			const string xml = @"
		        <command>
		            <proto><type>void</type> <name>vkGetPhysicalDeviceProperties</name></proto>
		            <param><type>VkPhysicalDevice</type> <name>physicalDevice</name></param>
		            <param><type>VkPhysicalDeviceProperties</type>* <name>pProperties</name></param>
		        </command>";

			XElement top = XElement.Parse(xml, LoadOptions.PreserveWhitespace);

			var inspector = new VkEntityInspector();

			inspector.Handles.Add("VkPhysicalDevice", new VkHandleInfo { name = "VkPhysicalDevice", csType = "IntPtr" });
			Assert.AreEqual(1, inspector.Handles.Keys.Count);

			inspector.Structs.Add("VkPhysicalDeviceProperties", new VkStructInfo { Name = "VkPhysicalDeviceProperties", returnedonly = true });
			Assert.AreEqual(1, inspector.Structs.Keys.Count);

			var parser = new VkCommandParser(inspector);

			VkCommandInfo command;
			bool actual = parser.Parse(top, out command);
			Assert.IsTrue(actual);
			Assert.IsNotNull(command);
			Assert.AreEqual("vkGetPhysicalDeviceProperties", command.Name);
			Assert.AreEqual("void", command.CppReturnType);
			Assert.AreEqual("void", command.CsReturnType);
			Assert.IsNotNull(command.FirstInstance);

			// NATIVE FUNCTION
			var function = command.NativeFunction;
			Assert.IsNotNull(function, "command.NativeFunction is not null");
			Assert.AreEqual("vkGetPhysicalDeviceProperties", function.Name);
			Assert.AreEqual("void", function.ReturnType);
			Assert.AreEqual(2, function.Arguments.Count);
			Assert.IsFalse(function.UseUnsafe);

			{
				const int index = 0;
				var arg_0 = function.Arguments[index];
				Assert.IsNotNull(arg_0);
				Assert.AreEqual(index, arg_0.Index);
				Assert.AreEqual("physicalDevice", arg_0.Name);
				Assert.AreEqual("VkPhysicalDevice", arg_0.BaseCppType);
				Assert.AreEqual("VkPhysicalDevice", arg_0.BaseCsType);
				Assert.AreEqual("VkPhysicalDevice", arg_0.ArgumentCppType);
				Assert.AreEqual("IntPtr", arg_0.ArgumentCsType);
				Assert.IsFalse(arg_0.IsConst);
				Assert.IsFalse(arg_0.IsOptional);
				Assert.IsFalse(arg_0.IsFixedArray);
				Assert.IsNull(arg_0.LengthVariable);
				Assert.IsNull(arg_0.ArrayConstant);
				Assert.IsFalse(arg_0.UseOut);
				Assert.IsFalse(arg_0.ByReference);
				Assert.IsNull(arg_0.Attribute);
			}

			{
				const int index = 1;
				var arg_1 = function.Arguments[index];
				Assert.IsNotNull(arg_1);
				Assert.AreEqual(index, arg_1.Index);
				Assert.AreEqual("pProperties", arg_1.Name);
				Assert.AreEqual("VkPhysicalDeviceProperties", arg_1.BaseCppType);
				Assert.AreEqual("VkPhysicalDeviceProperties", arg_1.BaseCsType);
				Assert.AreEqual("VkPhysicalDeviceProperties*", arg_1.ArgumentCppType);
				Assert.AreEqual("VkPhysicalDeviceProperties", arg_1.ArgumentCsType);
				Assert.IsFalse(arg_1.IsConst);
				Assert.IsFalse(arg_1.IsOptional);
				Assert.IsFalse(arg_1.IsFixedArray);
				Assert.IsNull(arg_1.LengthVariable);
				Assert.IsNull(arg_1.ArrayConstant);
				Assert.IsFalse(arg_1.UseOut);
				Assert.IsFalse(arg_1.ByReference);
				Assert.AreEqual("[In, Out]", arg_1.Attribute);
			}

			// METHOD SIGNATURE
			var method = command.MethodSignature;
			Assert.IsNotNull(method, "command.MethodSignature is not null");
			Assert.AreEqual("GetPhysicalDeviceProperties", method.Name);
			Assert.AreEqual(1, method.Parameters.Count);

			{
				const int index = 0;
				var param_0 = method.Parameters[index];
				Assert.IsNotNull(param_0);
				Assert.AreEqual(1, param_0.Source.Index);
				Assert.AreEqual("pProperties", param_0.Name);
				Assert.IsFalse(param_0.IsNullableType);
				Assert.AreEqual("VkPhysicalDeviceProperties", param_0.BaseCsType);
				Assert.IsFalse(param_0.UseRef);
				Assert.IsFalse(param_0.IsFixedArray);
				Assert.IsFalse(param_0.IsArrayParameter);
			}
		}


		[TestCase]
		public void TestCase9()
		{
			const string xml = @"
		        <command successcodes=""VK_SUCCESS"" errorcodes=""VK_ERROR_OUT_OF_HOST_MEMORY,VK_ERROR_OUT_OF_DEVICE_MEMORY,VK_ERROR_MEMORY_MAP_FAILED"">
		            <proto><type>VkResult</type> <name>vkMapMemory</name></proto>
		            <param><type>VkDevice</type> <name>device</name></param>
		            <param externsync=""true""><type>VkDeviceMemory</type> <name>memory</name></param>
		            <param><type>VkDeviceSize</type> <name>offset</name></param>
		            <param><type>VkDeviceSize</type> <name>size</name></param>
		            <param optional=""true""><type>VkMemoryMapFlags</type> <name>flags</name></param>
		            <param><type>void</type>** <name>ppData</name></param>
		            <validity>
		                <usage>pname:memory mustnot: currently be mapped</usage>
		                <usage>pname:offset must: be less than the size of pname:memory</usage>
		                <usage>If pname:size is not equal to ename:VK_WHOLE_SIZE, pname:size must: be greater than `0`</usage>
		                <usage>If pname:size is not equal to ename:VK_WHOLE_SIZE, pname:size must: be less than or equal to the size of the pname:memory minus pname:offset</usage>
		                <usage>pname:memory must: have been created with a memory type that reports ename:VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT</usage>
		            </validity>
		        </command>";

			XElement top = XElement.Parse(xml, LoadOptions.PreserveWhitespace);

			var inspector = new VkEntityInspector();

			inspector.Handles.Add("VkDevice", new VkHandleInfo { name = "VkDevice", csType = "IntPtr" });
			inspector.Handles.Add("VkDeviceMemory", new VkHandleInfo { name = "VkDeviceMemory", csType = "IntPtr" });
			Assert.AreEqual(2, inspector.Handles.Keys.Count);

			inspector.Structs.Add("VkDeviceSize", new VkStructInfo { Name = "VkDeviceSize" });
			Assert.AreEqual(1, inspector.Structs.Keys.Count);

			inspector.Enums.Add("VkMemoryMapFlags", new VkEnumInfo {  name = "VkMemoryMapFlags" });
			Assert.AreEqual(1, inspector.Enums.Keys.Count);

			var parser = new VkCommandParser(inspector);

			VkCommandInfo command;
			bool actual = parser.Parse(top, out command);
			Assert.IsTrue(actual);
			Assert.IsNotNull(command);
			Assert.AreEqual("vkMapMemory", command.Name);
			Assert.AreEqual("VkResult", command.CppReturnType);
			Assert.AreEqual("VkResult", command.CsReturnType);
			Assert.IsNotNull(command.FirstInstance);

			// NATIVE FUNCTION
			var function = command.NativeFunction;
			Assert.IsNotNull(function, "command.NativeFunction is not null");
			Assert.AreEqual("vkMapMemory", function.Name);
			Assert.AreEqual("VkResult", function.ReturnType);
			Assert.AreEqual(6, function.Arguments.Count);
			Assert.IsFalse(function.UseUnsafe);

			{
				const int index = 0;
				var arg_0 = function.Arguments[index];
				Assert.IsNotNull(arg_0);
				Assert.AreEqual(index, arg_0.Index);
				Assert.AreEqual("device", arg_0.Name);
				Assert.AreEqual("VkDevice", arg_0.BaseCppType);
				Assert.AreEqual("VkDevice", arg_0.BaseCsType);
				Assert.AreEqual("VkDevice", arg_0.ArgumentCppType);
				Assert.AreEqual("IntPtr", arg_0.ArgumentCsType);
				Assert.IsFalse(arg_0.IsConst);
				Assert.IsFalse(arg_0.IsOptional);
				Assert.IsFalse(arg_0.IsFixedArray);
				Assert.IsNull(arg_0.LengthVariable);
				Assert.IsNull(arg_0.ArrayConstant);
				Assert.IsFalse(arg_0.UseOut);
				Assert.IsFalse(arg_0.ByReference);
				Assert.IsNull(arg_0.Attribute);
			}

			{
				const int index = 1;
				var arg_1 = function.Arguments[index];
				Assert.IsNotNull(arg_1);
				Assert.AreEqual(index, arg_1.Index);
				Assert.AreEqual("memory", arg_1.Name);
				Assert.AreEqual("VkDeviceMemory", arg_1.BaseCppType);
				Assert.AreEqual("VkDeviceMemory", arg_1.BaseCsType);
				Assert.AreEqual("VkDeviceMemory", arg_1.ArgumentCppType);
				Assert.AreEqual("IntPtr", arg_1.ArgumentCsType);
				Assert.IsFalse(arg_1.IsConst);
				Assert.IsFalse(arg_1.IsOptional);
				Assert.IsFalse(arg_1.IsFixedArray);
				Assert.IsNull(arg_1.LengthVariable);
				Assert.IsNull(arg_1.ArrayConstant);
				Assert.IsFalse(arg_1.UseOut);
				Assert.IsFalse(arg_1.ByReference);
				Assert.IsNull(arg_1.Attribute);
			}

			{
				const int index = 2;
				var arg_1 = function.Arguments[index];
				Assert.IsNotNull(arg_1);
				Assert.AreEqual(index, arg_1.Index);
				Assert.AreEqual("offset", arg_1.Name);
				Assert.AreEqual("VkDeviceSize", arg_1.BaseCppType);
				Assert.AreEqual("VkDeviceSize", arg_1.BaseCsType);
				Assert.AreEqual("VkDeviceSize", arg_1.ArgumentCppType);
				Assert.AreEqual("VkDeviceSize", arg_1.ArgumentCsType);
				Assert.IsFalse(arg_1.IsConst);
				Assert.IsFalse(arg_1.IsOptional);
				Assert.IsFalse(arg_1.IsFixedArray);
				Assert.IsNull(arg_1.LengthVariable);
				Assert.IsNull(arg_1.ArrayConstant);
				Assert.IsFalse(arg_1.UseOut);
				Assert.IsFalse(arg_1.ByReference);
				Assert.IsNull(arg_1.Attribute);
			}

			{
				const int index = 3;
				var arg_1 = function.Arguments[index];
				Assert.IsNotNull(arg_1);
				Assert.AreEqual(index, arg_1.Index);
				Assert.AreEqual("size", arg_1.Name);
				Assert.AreEqual("VkDeviceSize", arg_1.BaseCppType);
				Assert.AreEqual("VkDeviceSize", arg_1.BaseCsType);
				Assert.AreEqual("VkDeviceSize", arg_1.ArgumentCppType);
				Assert.AreEqual("VkDeviceSize", arg_1.ArgumentCsType);
				Assert.IsFalse(arg_1.IsConst);
				Assert.IsFalse(arg_1.IsOptional);
				Assert.IsFalse(arg_1.IsFixedArray);
				Assert.IsNull(arg_1.LengthVariable);
				Assert.IsNull(arg_1.ArrayConstant);
				Assert.IsFalse(arg_1.UseOut);
				Assert.IsFalse(arg_1.ByReference);
				Assert.IsNull(arg_1.Attribute);
			}

			{
				const int index = 4;
				var arg_1 = function.Arguments[index];
				Assert.IsNotNull(arg_1);
				Assert.AreEqual(index, arg_1.Index);
				Assert.AreEqual("flags", arg_1.Name);
				Assert.AreEqual("VkMemoryMapFlags", arg_1.BaseCppType);
				Assert.AreEqual("VkMemoryMapFlags", arg_1.BaseCsType);
				Assert.AreEqual("VkMemoryMapFlags", arg_1.ArgumentCppType);
				Assert.AreEqual("VkMemoryMapFlags", arg_1.ArgumentCsType);
				Assert.IsFalse(arg_1.IsConst);
				Assert.IsTrue(arg_1.IsOptional);
				Assert.IsFalse(arg_1.IsFixedArray);
				Assert.IsNull(arg_1.LengthVariable);
				Assert.IsNull(arg_1.ArrayConstant);
				Assert.IsFalse(arg_1.UseOut);
				Assert.IsFalse(arg_1.ByReference);
				Assert.IsNull(arg_1.Attribute);
			}

			{
				const int index = 5;
				var arg_1 = function.Arguments[index];
				Assert.IsNotNull(arg_1);
				Assert.AreEqual(index, arg_1.Index);
				Assert.AreEqual("ppData", arg_1.Name);
				Assert.AreEqual("void", arg_1.BaseCppType);
				Assert.AreEqual("void", arg_1.BaseCsType);
				Assert.AreEqual("void**", arg_1.ArgumentCppType);
				Assert.AreEqual("IntPtr", arg_1.ArgumentCsType);
				Assert.IsFalse(arg_1.IsConst);
				Assert.IsFalse(arg_1.IsOptional);
				Assert.IsFalse(arg_1.IsFixedArray);
				Assert.IsNull(arg_1.LengthVariable);
				Assert.IsNull(arg_1.ArrayConstant);
				Assert.IsTrue(arg_1.UseOut);
				Assert.IsFalse(arg_1.ByReference);
				Assert.IsNull(arg_1.Attribute);
			}

			// FIXME: figure out what required

			// METHOD SIGNATURE
			var method = command.MethodSignature;
			Assert.IsNotNull(method, "command.MethodSignature is not null");
			Assert.AreEqual("MapMemory", method.Name);
			Assert.AreEqual(5, method.Parameters.Count);

			{
				const int index = 0;
				var param_0 = method.Parameters[index];
				Assert.IsNotNull(param_0);
				Assert.AreEqual(1, param_0.Source.Index);
				Assert.AreEqual("memory", param_0.Name);
				Assert.IsFalse(param_0.IsNullableType);
				Assert.AreEqual("VkDeviceMemory", param_0.BaseCsType);
				Assert.IsFalse(param_0.UseRef);
				Assert.IsFalse(param_0.IsFixedArray);
				Assert.IsFalse(param_0.IsArrayParameter);
			}

			{
				const int index = 1;
				var param_0 = method.Parameters[index];
				Assert.IsNotNull(param_0);
				Assert.AreEqual(2, param_0.Source.Index);
				Assert.AreEqual("offset", param_0.Name);
				Assert.IsFalse(param_0.IsNullableType);
				Assert.AreEqual("VkDeviceSize", param_0.BaseCsType);
				Assert.IsFalse(param_0.UseRef);
				Assert.IsFalse(param_0.IsFixedArray);
				Assert.IsFalse(param_0.IsArrayParameter);
			}

			{
				const int index = 2;
				var param_0 = method.Parameters[index];
				Assert.IsNotNull(param_0);
				Assert.AreEqual(3, param_0.Source.Index);
				Assert.AreEqual("size", param_0.Name);
				Assert.IsFalse(param_0.IsNullableType);
				Assert.AreEqual("VkDeviceSize", param_0.BaseCsType);
				Assert.IsFalse(param_0.UseRef);
				Assert.IsFalse(param_0.IsFixedArray);
				Assert.IsFalse(param_0.IsArrayParameter);
			}

			{
				const int index = 3;
				var param_0 = method.Parameters[index];
				Assert.IsNotNull(param_0);
				Assert.AreEqual(4, param_0.Source.Index);
				Assert.AreEqual("flags", param_0.Name);
				Assert.IsFalse(param_0.IsNullableType);
				Assert.AreEqual("VkMemoryMapFlags", param_0.BaseCsType);
				Assert.IsFalse(param_0.UseRef);
				Assert.IsFalse(param_0.IsFixedArray);
				Assert.IsFalse(param_0.IsArrayParameter);
			}

			{
				const int index = 4;
				var param_0 = method.Parameters[index];
				Assert.IsNotNull(param_0);
				Assert.AreEqual(5, param_0.Source.Index);
				Assert.AreEqual("ppData", param_0.Name);
				Assert.IsFalse(param_0.IsNullableType);
				Assert.AreEqual("void", param_0.BaseCsType);
				Assert.IsTrue(param_0.UseRef);
				Assert.IsFalse(param_0.IsFixedArray);
				Assert.IsFalse(param_0.IsArrayParameter);
			}
		}
	}
}

