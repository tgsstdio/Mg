namespace Magnesium.OpenGL.Internals
{
    public class GLCmdBeginRenderpassRecord
    {
        public GLQueueClearBufferMask Bitmask { get; internal set; }
        public GLCmdClearValuesParameter ClearState { get; internal set; }
        public GLNextFramebuffer Framebuffer { get; internal set; }
    }
}