using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkDeviceCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public UInt32 queueCreateInfoCount { get; set; }
		public IntPtr pQueueCreateInfos { get; set; } // VkDeviceQueueCreateInfo[]
		public UInt32 enabledLayerCount { get; set; }
		public IntPtr ppEnabledLayerNames { get; set; } // string[]
		public UInt32 enabledExtensionCount { get; set; }
		public IntPtr ppEnabledExtensionNames { get; set; } // string[]
		public IntPtr pEnabledFeatures { get; set; } // VkPhysicalDeviceFeatures
	}
}
