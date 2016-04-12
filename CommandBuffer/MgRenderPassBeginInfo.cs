namespace Magnesium
{
    public class MgRenderPassBeginInfo
	{
		public MgRenderPass RenderPass { get; set; }
		public MgFramebuffer Framebuffer { get; set; }
		public MgRect2D RenderArea { get; set; }
		public MgClearValue[] ClearValues { get; set; }
	}
}

