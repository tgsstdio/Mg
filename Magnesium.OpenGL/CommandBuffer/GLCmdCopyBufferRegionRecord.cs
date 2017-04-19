using System;

namespace Magnesium.OpenGL.Internals
{
    public class GLCmdCopyBufferRegionRecord
    {
        public IntPtr ReadOffset { get; internal set; }
        public int Size { get; internal set; }
        public IntPtr WriteOffset { get; internal set; }
    }
}