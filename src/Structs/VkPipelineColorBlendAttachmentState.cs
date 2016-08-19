using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkPipelineColorBlendAttachmentState
	{
		public VkBool32 blendEnable { get; set; }
		public VkBlendFactor srcColorBlendFactor { get; set; }
		public VkBlendFactor dstColorBlendFactor { get; set; }
		public VkBlendOp colorBlendOp { get; set; }
		public VkBlendFactor srcAlphaBlendFactor { get; set; }
		public VkBlendFactor dstAlphaBlendFactor { get; set; }
		public VkBlendOp alphaBlendOp { get; set; }
		public VkColorComponentFlags colorWriteMask { get; set; }
	}
}
