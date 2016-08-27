using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkPhysicalDeviceProperties
	{
		public UInt32 apiVersion;
		public UInt32 driverVersion;
		public UInt32 vendorID;
		public UInt32 deviceID;
		public VkPhysicalDeviceType deviceType;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string deviceName;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst= 16)]
		public Byte[] pipelineCacheUUID;
		public VkPhysicalDeviceLimits limits;
		public VkPhysicalDeviceSparseProperties sparseProperties;
	}
}
