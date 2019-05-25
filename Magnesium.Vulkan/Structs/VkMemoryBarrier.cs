using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkMemoryBarrier
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public MgAccessFlagBits srcAccessMask { get; set; }
		public MgAccessFlagBits dstAccessMask { get; set; }
	}
}
