using System.Runtime.InteropServices;

namespace OffscreenDemo
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexT2fN3fV3f
    {
        public TkVector2 TexCoord;
        public TkVector3 Normal;
        public TkVector3 Position;
    }
    
}
