namespace Magnesium.OpenGL.Internals
{
    public class GLCmdComputeRecording
    {
        public GLCmdComputeGrid Grid { get; internal set; }
        public GLCmdComputeEncoder Encoder { get; set; }
    }
}