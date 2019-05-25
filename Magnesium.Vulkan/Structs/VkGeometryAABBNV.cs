using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkGeometryAABBNV
	{
		public VkStructureType sType;
		public IntPtr pNext;
		public UInt64 aabbData;
		public UInt32 numAABBs;
		// Stride in bytes between AABBs
		public UInt32 stride;
		// Offset in bytes of the first AABB in aabbData
		public UInt64 offset;
	}
}
