using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkDedicatedAllocationMemoryAllocateInfoNV
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt64 image { get; set; }
		public UInt64 buffer { get; set; }
	}
}
