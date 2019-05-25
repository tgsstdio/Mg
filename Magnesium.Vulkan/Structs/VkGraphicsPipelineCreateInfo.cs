using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkGraphicsPipelineCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public MgPipelineCreateFlagBits flags { get; set; }
		public UInt32 stageCount { get; set; }
		public IntPtr pStages { get; set; } // VkPipelineShaderStageCreateInfo[]
		public IntPtr pVertexInputState { get; set; } // VkPipelineVertexInputStateCreateInfo
		public IntPtr pInputAssemblyState { get; set; } // VkPipelineInputAssemblyStateCreateInfo
		public IntPtr pTessellationState { get; set; } // VkPipelineTessellationStateCreateInfo
		public IntPtr pViewportState { get; set; } // VkPipelineViewportStateCreateInfo
		public IntPtr pRasterizationState { get; set; } // VkPipelineRasterizationStateCreateInfo
		public IntPtr pMultisampleState { get; set; } // VkPipelineMultisampleStateCreateInfo
		public IntPtr pDepthStencilState { get; set; } // VkPipelineDepthStencilStateCreateInfo
		public IntPtr pColorBlendState { get; set; } // VkPipelineColorBlendStateCreateInfo
		public IntPtr pDynamicState { get; set; } // VkPipelineDynamicStateCreateInfo
		public UInt64 layout { get; set; }
		public UInt64 renderPass { get; set; }
		public UInt32 subpass { get; set; }
		public UInt64 basePipelineHandle { get; set; }
		public Int32 basePipelineIndex { get; set; }
	}
}
