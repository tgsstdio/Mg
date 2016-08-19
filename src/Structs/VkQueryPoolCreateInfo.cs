using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkQueryPoolCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public VkQueryType queryType { get; set; }
		public UInt32 queryCount { get; set; }
		public VkQueryPipelineStatisticFlags pipelineStatistics { get; set; }
	}
}
