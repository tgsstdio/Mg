using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkPipelineDepthStencilStateCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public VkBool32 depthTestEnable { get; set; }
		public VkBool32 depthWriteEnable { get; set; }
		public VkCompareOp depthCompareOp { get; set; }
		public VkBool32 depthBoundsTestEnable { get; set; }
		public VkBool32 stencilTestEnable { get; set; }
		public VkStencilOpState front { get; set; }
		public VkStencilOpState back { get; set; }
		public float minDepthBounds { get; set; }
		public float maxDepthBounds { get; set; }
	}
}
