namespace Magnesium.OpenGL.Internals
{
    public class AmtBeginRenderpassRecord
    {
        public GLQueueClearBufferMask Bitmask { get; internal set; }
        public GLCmdClearValuesParameter ClearState { get; internal set; }
    }
}