using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkDeviceQueueCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public UInt32 queueFamilyIndex { get; set; }
		public UInt32 queueCount { get; set; }
		public IntPtr pQueuePriorities { get; set; } // float[]
	}
}
