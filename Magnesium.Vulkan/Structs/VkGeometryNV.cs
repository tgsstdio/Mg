using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkGeometryNV
	{
		public VkStructureType sType;
		public IntPtr pNext;
		public MgGeometryTypeNV geometryType;
		public VkGeometryDataNV geometry;
		public MgGeometryFlagBitsNV flags;
	}
}
