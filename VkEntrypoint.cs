using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Magnesium.Vulkan
{
	public class VkEntrypoint333
	{
		#region IMgEntrypoint implementation

		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
		internal struct ApplicationCreateInfo
		{
			public UInt32 sType;
			public IntPtr pNext;
			[MarshalAs(UnmanagedType.LPStr)] public string pApplicationName;
			public UInt32 applicationVersion;
			[MarshalAs(UnmanagedType.LPStr)] public string pEngineName;
			public UInt32 engineVersion;
			public UInt32 apiVersion;
		}

		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
		internal class InstanceCreateInfo
		{
			public UInt32 sType;
			public IntPtr pNext;
			public UInt32 flags;
			public ApplicationCreateInfo pApplicationInfo;
			public UInt32 enabledLayerCount;
			public IntPtr ppEnabledLayerNames;
			public UInt32 enabledExtensionCount;
			public IntPtr ppEnabledExtensionNames;
		}

		[DllImport("vulkan-1", CallingConvention=CallingConvention.Winapi)]
		extern static Result vkCreateInstance([In] InstanceCreateInfo createInfo, IntPtr allocator, out IntPtr instance);

	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
		internal struct LayerProperties
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=256)] public string layerName;
			public UInt32 specVersion;
			public UInt32 implementationVersion;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=256)] public string description;
		}

		[DllImport("vulkan-1", CallingConvention=CallingConvention.Winapi)]
		extern static Result vkEnumerateInstanceLayerProperties(ref UInt32 pPropertyCount, [In, Out] LayerProperties[] pProperties);

		#endregion


	}
}

