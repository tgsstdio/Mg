using System;

namespace Magnesium
{
    public class MgCommandBufferInheritanceInfo
	{
		public IMgRenderPass RenderPass { get; set; }
		public UInt32 Subpass { get; set; }
		public IMgFramebuffer Framebuffer { get; set; }
		public bool OcclusionQueryEnable { get; set; }
		public MgQueryControlFlagBits QueryFlags { get; set; }
		public MgQueryPipelineStatisticFlagBits PipelineStatistics { get; set; }
	}
}

