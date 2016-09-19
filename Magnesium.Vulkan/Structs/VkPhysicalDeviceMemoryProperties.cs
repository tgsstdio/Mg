using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkPhysicalDeviceMemoryProperties
	{
		public UInt32 memoryTypeCount;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public VkMemoryType[] memoryTypes; // [VK_MAX_MEMORY_TYPES]
		public UInt32 memoryHeapCount;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public VkMemoryHeap[] memoryHeaps; // [VK_MAX_MEMORY_HEAPS]
	}
}
