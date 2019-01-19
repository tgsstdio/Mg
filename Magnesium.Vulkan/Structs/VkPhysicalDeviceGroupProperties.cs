using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
    [StructLayout(LayoutKind.Sequential)]
	internal struct VkPhysicalDeviceGroupProperties
	{
        public VkStructureType sType;

        public IntPtr pNext;

        public UInt32 physicalDeviceCount;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public IntPtr[] physicalDevices; // VK_MAX_DEVICE_GROUP_SIZE

        public VkBool32 subsetAllocation;
	}
}
