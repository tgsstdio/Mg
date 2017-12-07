using System.Runtime.InteropServices;

namespace OffscreenDemo
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexT2fN3fV3f
    {
        public Vector2 TexCoord;
        public Vector3 Normal;
        public Vector3 Position;
    }
    
}
