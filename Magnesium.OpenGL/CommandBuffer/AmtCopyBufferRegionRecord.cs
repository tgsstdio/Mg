using System;

namespace Magnesium.OpenGL.Internals
{
    public class AmtCopyBufferRegionRecord
    {
        public IntPtr ReadOffset { get; internal set; }
        public int Size { get; internal set; }
        public IntPtr WriteOffset { get; internal set; }
    }
}