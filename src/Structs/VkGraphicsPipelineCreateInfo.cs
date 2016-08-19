using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkGraphicsPipelineCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public VkPipelineCreateFlags flags { get; set; }
		public UInt32 stageCount { get; set; }
		public VkPipelineShaderStageCreateInfo pStages { get; set; }
		public VkPipelineVertexInputStateCreateInfo pVertexInputState { get; set; }
		public VkPipelineInputAssemblyStateCreateInfo pInputAssemblyState { get; set; }
		public VkPipelineTessellationStateCreateInfo pTessellationState { get; set; }
		public VkPipelineViewportStateCreateInfo pViewportState { get; set; }
		public VkPipelineRasterizationStateCreateInfo pRasterizationState { get; set; }
		public VkPipelineMultisampleStateCreateInfo pMultisampleState { get; set; }
		public VkPipelineDepthStencilStateCreateInfo pDepthStencilState { get; set; }
		public VkPipelineColorBlendStateCreateInfo pColorBlendState { get; set; }
		public VkPipelineDynamicStateCreateInfo pDynamicState { get; set; }
		public UInt64 layout { get; set; }
		public UInt64 renderPass { get; set; }
		public UInt32 subpass { get; set; }
		public UInt64 basePipelineHandle { get; set; }
		public Int32 basePipelineIndex { get; set; }
	}
}
