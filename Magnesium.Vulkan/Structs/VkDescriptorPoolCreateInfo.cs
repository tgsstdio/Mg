using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkDescriptorPoolCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public VkDescriptorPoolCreateFlags flags { get; set; }
		public UInt32 maxSets { get; set; }
		public UInt32 poolSizeCount { get; set; }
		public IntPtr pPoolSizes { get; set; } // VkDescriptorPoolSize
	}
}
