namespace Magnesium.OpenGL.Internals
{
    public class GLCmdBeginRenderpassRecord
    {
       // public GLCmdClearValuesParameter ClearState { get; internal set; }
        public GLNextFramebuffer Framebuffer { get; internal set; }
        public bool IsCompatible { get; internal set; }
        public GLNextRenderPass Renderpass { get; internal set; }
        public GLCmdClearValueArrayItem[] ClearValues { get; internal set; }
        public uint Subpass { get; internal set; }
        public MgRect2D RenderArea { get; internal set; }
    }
}