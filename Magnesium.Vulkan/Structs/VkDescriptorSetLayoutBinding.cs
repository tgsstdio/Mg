using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkDescriptorSetLayoutBinding
	{
		public UInt32 binding { get; set; }
		public VkDescriptorType descriptorType { get; set; }
		public UInt32 descriptorCount { get; set; }
		public VkShaderStageFlags stageFlags { get; set; }
		public IntPtr pImmutableSamplers { get; set; }
	}
}
