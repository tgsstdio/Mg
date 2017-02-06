using System;

namespace Magnesium.OpenGL
{
    public static class GLMemoryBufferExtensions
    {
        public static uint GetMask(this GLMemoryBufferType bufferType)
        {
            switch (bufferType)
            {
                case GLMemoryBufferType.SSBO:
                    return 1 << 0;
                case GLMemoryBufferType.INDIRECT:
                    return 1 << 1;
                case GLMemoryBufferType.VERTEX:
                    return 1 << 2;
                case GLMemoryBufferType.INDEX:
                    return 1 << 3;
                case GLMemoryBufferType.IMAGE:
                    return 1 << 4;
                case GLMemoryBufferType.TRANSFER_SRC:
                    return 1 << 5;
                case GLMemoryBufferType.TRANSFER_DST:
                    return 1 << 6;
                case GLMemoryBufferType.UNIFORM:
                    return 1 << 7;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
