using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkBindAccelerationStructureMemoryInfoNV
	{
		public VkStructureType sType;
		public IntPtr pNext;
		public UInt64 accelerationStructure;
		public UInt64 memory;
		public UInt64 memoryOffset;
		public UInt32 deviceIndexCount;
		public IntPtr pDeviceIndices;
	}
}
