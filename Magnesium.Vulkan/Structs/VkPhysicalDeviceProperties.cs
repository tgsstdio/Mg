using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkPhysicalDeviceProperties
	{
		public UInt32 apiVersion;
		public UInt32 driverVersion;
		public UInt32 vendorID;
		public UInt32 deviceID;
		public VkPhysicalDeviceType deviceType;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public byte[] deviceName;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst= 16)]
		public Byte[] pipelineCacheUUID;
		public VkPhysicalDeviceLimits limits;
		public VkPhysicalDeviceSparseProperties sparseProperties;
	}
}
