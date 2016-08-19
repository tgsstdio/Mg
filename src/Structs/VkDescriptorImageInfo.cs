using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkDescriptorImageInfo
	{
		public UInt64 sampler { get; set; }
		public UInt64 imageView { get; set; }
		public VkImageLayout imageLayout { get; set; }
	}
}
