using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkWriteDescriptorSet
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt64 dstSet { get; set; }
		public UInt32 dstBinding { get; set; }
		public UInt32 dstArrayElement { get; set; }
		public UInt32 descriptorCount { get; set; }
		public VkDescriptorType descriptorType { get; set; }
		public VkDescriptorImageInfo pImageInfo { get; set; }
		public VkDescriptorBufferInfo pBufferInfo { get; set; }
		public UInt64 pTexelBufferView { get; set; }
	}
}
