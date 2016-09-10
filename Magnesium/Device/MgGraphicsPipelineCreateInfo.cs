using System;

namespace Magnesium
{
    public class MgGraphicsPipelineCreateInfo
	{
		public MgPipelineCreateFlagBits Flags { get; set; }
		public MgPipelineShaderStageCreateInfo[] Stages { get; set; }
		public MgPipelineVertexInputStateCreateInfo VertexInputState { get; set; }
		public MgPipelineInputAssemblyStateCreateInfo InputAssemblyState { get; set; }
		/// <summary>
		/// OPTIONAL if the pipeline does not include a tessellation control shader stage and tessellation evaluation shader stage.
		/// </summary>
		/// <value>The state of the tessellation.</value>
		public MgPipelineTessellationStateCreateInfo TessellationState { get; set; }
		/// <summary>
		/// OPTIONAL
		/// </summary>
		/// <value>The state of the viewport.</value>
		public MgPipelineViewportStateCreateInfo ViewportState { get; set; }
		public MgPipelineRasterizationStateCreateInfo RasterizationState { get; set; }
		/// <summary>
		/// OPTIONAL if the pipeline has rasterization disabled.
		/// </summary>
		/// <value>The state of the multisample.</value>
		public MgPipelineMultisampleStateCreateInfo MultisampleState { get; set; }
		/// <summary>
		/// OPTIONAL if the pipeline has rasterization disabled or if the subpass of the render pass the pipeline is created against does not use a depth/stencil attachment.
		/// </summary>
		/// <value>The state of the depth stencil.</value>
		public MgPipelineDepthStencilStateCreateInfo DepthStencilState { get; set; }
		/// <summary>
		/// OPTIONAL if the pipeline has rasterization disabled or if the subpass of the render pass the pipeline is created against does not use any color attachments.
		/// </summary>
		/// <value>The state of the color blend.</value>
		public MgPipelineColorBlendStateCreateInfo ColorBlendState { get; set; }
		/// <summary>
		/// OPTIONAL. Which means no state in the pipeline is considered dynamic.
		/// </summary>
		/// <value>The state of the dynamic.</value>
		public MgPipelineDynamicStateCreateInfo DynamicState { get; set; }
		public IMgPipelineLayout Layout { get; set; }
		public IMgRenderPass RenderPass { get; set; }
		public UInt32 Subpass { get; set; }
		public IMgPipeline BasePipelineHandle { get; set; }
		public Int32 BasePipelineIndex { get; set; }
	}
}

