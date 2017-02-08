namespace Magnesium.OpenGL.Internals
{
    public class GLCmdCopyBufferRecord
    {
        public int Destination { get; internal set; }
        public GLCmdCopyBufferRegionRecord[] Regions { get; internal set; }
        public int Source { get; internal set; }
    }
}