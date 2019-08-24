using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkDescriptorSetLayoutBinding
	{
		public UInt32 binding { get; set; }
		public MgDescriptorType descriptorType { get; set; }
		public UInt32 descriptorCount { get; set; }
		public MgShaderStageFlagBits stageFlags { get; set; }
		public IntPtr pImmutableSamplers { get; set; }
	}
}
