using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkCopyDescriptorSet
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt64 srcSet { get; set; }
		public UInt32 srcBinding { get; set; }
		public UInt32 srcArrayElement { get; set; }
		public UInt64 dstSet { get; set; }
		public UInt32 dstBinding { get; set; }
		public UInt32 dstArrayElement { get; set; }
		public UInt32 descriptorCount { get; set; }
	}
}
