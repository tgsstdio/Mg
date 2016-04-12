using System;

namespace Magnesium
{
    public class MgGraphicsPipelineCreateInfo
	{
		public MgPipelineCreateFlagBits Flags { get; set; }
		public MgPipelineShaderStageCreateInfo[] Stages { get; set; }
		public MgPipelineVertexInputStateCreateInfo VertexInputState { get; set; }
		public MgPipelineInputAssemblyStateCreateInfo InputAssemblyState { get; set; }
		public MgPipelineTessellationStateCreateInfo TessellationState { get; set; }
		public MgPipelineViewportStateCreateInfo ViewportState { get; set; }
		public MgPipelineRasterizationStateCreateInfo RasterizationState { get; set; }
		public MgPipelineMultisampleStateCreateInfo MultisampleState { get; set; }
		public MgPipelineDepthStencilStateCreateInfo DepthStencilState { get; set; }
		public MgPipelineColorBlendStateCreateInfo ColorBlendState { get; set; }
		public MgPipelineDynamicStateCreateInfo DynamicState { get; set; }
		public MgPipelineLayout Layout { get; set; }
		public MgRenderPass RenderPass { get; set; }
		public UInt32 Subpass { get; set; }
		public MgPipeline BasePipelineHandle { get; set; }
		public Int32 BasePipelineIndex { get; set; }
	}
}

