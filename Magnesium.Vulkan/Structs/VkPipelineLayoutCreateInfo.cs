using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkPipelineLayoutCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public UInt32 setLayoutCount { get; set; }
		public IntPtr pSetLayouts { get; set; } // UInt64
		public UInt32 pushConstantRangeCount { get; set; }
		public IntPtr pPushConstantRanges { get; set; } // VkPushConstantRange
	}
}
