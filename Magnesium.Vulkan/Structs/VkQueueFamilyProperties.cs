using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkQueueFamilyProperties
	{
		public VkQueueFlags queueFlags { get; set; }
		public UInt32 queueCount { get; set; }
		public UInt32 timestampValidBits { get; set; }
		public MgExtent3D minImageTransferGranularity { get; set; }
	}
}
