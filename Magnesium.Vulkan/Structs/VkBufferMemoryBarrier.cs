using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkBufferMemoryBarrier
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public VkAccessFlags srcAccessMask { get; set; }
		public VkAccessFlags dstAccessMask { get; set; }
		public UInt32 srcQueueFamilyIndex { get; set; }
		public UInt32 dstQueueFamilyIndex { get; set; }
		public UInt64 buffer { get; set; }
		public UInt64 offset { get; set; }
		public UInt64 size { get; set; }
	}
}
