using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkGeometryTrianglesNV
	{
		public VkStructureType sType;
		public IntPtr pNext;
		public UInt64 vertexData;
		public UInt64 vertexOffset;
		public UInt32 vertexCount;
		public UInt64 vertexStride;
		public MgFormat vertexFormat;
		public UInt64 indexData;
		public UInt64 indexOffset;
		public UInt32 indexCount;
		public MgIndexType indexType;
		// Optional reference to array of floats representing a 3x4 row major affine transformation matrix.
		public UInt64 transformData;
		public UInt64 transformOffset;
	}
}
