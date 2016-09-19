namespace Magnesium
{
    public class MgRenderPassBeginInfo
	{
		public IMgRenderPass RenderPass { get; set; }
		public IMgFramebuffer Framebuffer { get; set; }
		public MgRect2D RenderArea { get; set; }
		public MgClearValue[] ClearValues { get; set; }
	}
}

