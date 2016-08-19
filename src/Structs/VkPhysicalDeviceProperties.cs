using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkPhysicalDeviceProperties
	{
		public UInt32 apiVersion { get; set; }
		public UInt32 driverVersion { get; set; }
		public UInt32 vendorID { get; set; }
		public UInt32 deviceID { get; set; }
		public VkPhysicalDeviceType deviceType { get; set; }
		public char deviceName { get; set; }
		public Byte pipelineCacheUUID { get; set; }
		public VkPhysicalDeviceLimits limits { get; set; }
		public VkPhysicalDeviceSparseProperties sparseProperties { get; set; }
	}
}
