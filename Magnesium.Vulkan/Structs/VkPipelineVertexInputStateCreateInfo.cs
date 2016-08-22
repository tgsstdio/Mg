using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkPipelineVertexInputStateCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public UInt32 vertexBindingDescriptionCount { get; set; }
		public VkVertexInputBindingDescription pVertexBindingDescriptions { get; set; }
		public UInt32 vertexAttributeDescriptionCount { get; set; }
		public VkVertexInputAttributeDescription pVertexAttributeDescriptions { get; set; }
	}
}
