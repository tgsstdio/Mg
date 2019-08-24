using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkAccelerationStructureCreateInfoNV
	{
		public VkStructureType sType;
		public IntPtr pNext;
		public UInt64 compactedSize;
		public VkAccelerationStructureInfoNV info;
	}
}
