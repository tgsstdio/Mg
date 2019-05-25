using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkAccelerationStructureInfoNV
	{
		public VkStructureType sType;
		public IntPtr pNext;
		public MgAccelerationStructureTypeNV type;
		public MgBuildAccelerationStructureFlagBitsNV flags;
		public UInt32 instanceCount;
		public UInt32 geometryCount;
		public IntPtr pGeometries;
	}
}
