namespace Magnesium.OpenGL.Internals
{
    public class GLCmdCopyBufferRecord
    {
        public uint Destination { get; internal set; }
        public GLCmdCopyBufferRegionRecord[] Regions { get; internal set; }
        public uint Source { get; internal set; }
    }
}