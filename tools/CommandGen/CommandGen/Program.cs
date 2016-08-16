using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace CommandGen
{
	class MainClass
	{
		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
		public struct LayerProperties
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=256)] public string layerName;
			public UInt32 specVersion;
			public UInt32 implementationVersion;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=256)] public string description;
		}

		[DllImport("vulkan-1", CallingConvention=CallingConvention.Winapi)]
		extern static unsafe UInt32 vkEnumerateInstanceLayerProperties(ref UInt32 pPropertyCount, [In, Out] LayerProperties[] pProperties);

		[DllImport("vulkan-1", CallingConvention = CallingConvention.Winapi)]
		extern static unsafe void vkCmdSetBlendConstants(IntPtr commandBuffer, [MarshalAs(UnmanagedType.LPArray, SizeConst = 4)] float[] blendConstants);

		public unsafe static void Main (string[] args)
		{
			//NativeVk();
			try
			{
				var doc = XDocument.Load("TestData/vk.xml", LoadOptions.PreserveWhitespace);

				var inspector = new VkEntityInspector();
				inspector.Inspect(doc.Root);

				var parser = new VkCommandParser(inspector);

				//parser.Handles.Add("Instance", new HandleInfo { name = "Instance", type = "VK_DEFINE_HANDLE" });

				var output = new List<VkCommandInfo>();
				foreach (var child in doc.Root.Descendants("command"))
				{
					VkCommandInfo command;
					if (parser.Parse(child, out command))
					{
						output.Add(command);
					}
				}

				int noOfUnsafe = 0;
				int totalNativeInterfaces = 0;

				using (var writer = new StreamWriter("Interfaces.txt", false))
				{
					foreach (var command in output)
					{
						++totalNativeInterfaces;
						if (command.NativeFunction.UseUnsafe)
							++noOfUnsafe;
						writer.WriteLine(command.NativeFunction.GetImplementation());
					}
				}

				Console.WriteLine("totalNativeInterfaces :" + totalNativeInterfaces);
				Console.WriteLine("noOfUnsafe :" + noOfUnsafe);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		static unsafe void NativeVk()
		{
			var p = Marshal.SizeOf(typeof(LayerProperties));

			Console.WriteLine("Hello World!" + p);

			UInt32 pPropertyCount = 0;
			var result = vkEnumerateInstanceLayerProperties(ref pPropertyCount, null);
			Console.WriteLine("Count!" + result + " : " + pPropertyCount);

			LayerProperties[] pProperties = new LayerProperties[pPropertyCount];
			var result2 = vkEnumerateInstanceLayerProperties(ref pPropertyCount, pProperties);

			Console.WriteLine("Count!" + result2 + " : " + pPropertyCount);

			foreach (var prop in pProperties)
			{
				Console.WriteLine(prop.layerName + " - " + prop.description);
			}
		}
}
}
