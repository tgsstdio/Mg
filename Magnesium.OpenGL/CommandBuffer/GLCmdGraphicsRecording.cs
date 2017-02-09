namespace Magnesium.OpenGL.Internals
{
    public class GLCmdGraphicsRecording
    {
        public GLCmdGraphicsGrid Grid { get; internal set; }
        public IGLCmdStateRenderer StateRenderer { get; internal set; }
    }
}