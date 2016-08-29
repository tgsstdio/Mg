using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkDescriptorPoolSize
	{
		public VkDescriptorType type { get; set; }
		public UInt32 descriptorCount { get; set; }
	}
}
