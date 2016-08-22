using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkDeviceCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public UInt32 queueCreateInfoCount { get; set; }
		public VkDeviceQueueCreateInfo pQueueCreateInfos { get; set; }
		public UInt32 enabledLayerCount { get; set; }
		public string ppEnabledLayerNames { get; set; }
		public UInt32 enabledExtensionCount { get; set; }
		public string ppEnabledExtensionNames { get; set; }
		public VkPhysicalDeviceFeatures pEnabledFeatures { get; set; }
	}
}
