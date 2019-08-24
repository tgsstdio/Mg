using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkBindBufferMemoryInfo
	{
		public VkStructureType sType;
		public IntPtr pNext;
		public UInt64 buffer;
		public UInt64 memory;
		public UInt64 memoryOffset;
	}
}
