namespace Magnesium.OpenGL.Internals
{
    public class GLCmdBlitRecording
    {
        public GLCmdBlitGrid Grid { get; internal set; }
        public IGLBlitOperationEntrypoint Entrypoint { get; set; }
    }
}