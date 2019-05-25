using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkBindImageMemoryInfo
	{
		public VkStructureType sType;
		public IntPtr pNext;
		public UInt64 image;
		public UInt64 memory;
		public UInt64 memoryOffset;
	}
}
