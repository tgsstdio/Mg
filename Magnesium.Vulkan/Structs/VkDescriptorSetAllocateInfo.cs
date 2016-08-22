using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkDescriptorSetAllocateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt64 descriptorPool { get; set; }
		public UInt32 descriptorSetCount { get; set; }
		public UInt64 pSetLayouts { get; set; }
	}
}
