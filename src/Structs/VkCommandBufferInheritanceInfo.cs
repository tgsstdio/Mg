using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkCommandBufferInheritanceInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt64 renderPass { get; set; }
		public UInt32 subpass { get; set; }
		public UInt64 framebuffer { get; set; }
		public VkBool32 occlusionQueryEnable { get; set; }
		public VkQueryControlFlags queryFlags { get; set; }
		public VkQueryPipelineStatisticFlags pipelineStatistics { get; set; }
	}
}
