using System;
using System.Runtime.InteropServices;
using Magnesium;
using Magnesium.Vulkan;

namespace InstanceDemo
{
	class MainClass
	{
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public struct LayerProperties
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string layerName;
			public UInt32 specVersion;
			public UInt32 implementationVersion;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string description;
		}

		[DllImport("vulkan-1", CallingConvention = CallingConvention.Winapi)]
		extern static unsafe UInt32 vkEnumerateInstanceLayerProperties(ref UInt32 pPropertyCount, [In, Out] LayerProperties[] pProperties);


		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			var entrypoint = new VkEntrypoint();

			IMgInstance instance;
			var createInfo = new Magnesium.MgInstanceCreateInfo
			{ 
				
			};
			var result = entrypoint.CreateInstance(createInfo, null, out instance);
			instance.DestroyInstance(null);

			UInt32 pPropertyCount = 0;
			var results = vkEnumerateInstanceLayerProperties(ref pPropertyCount, null);
			Console.WriteLine("Count FIRST!" + results + " : " + pPropertyCount);
		}
	}
}
