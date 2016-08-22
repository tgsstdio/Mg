using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkInstanceCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public IntPtr pApplicationInfo { get; set; }
		public UInt32 enabledLayerCount { get; set; }
		public IntPtr ppEnabledLayerNames { get; set; }
		public UInt32 enabledExtensionCount { get; set; }
		public IntPtr ppEnabledExtensionNames { get; set; }
	}
}
