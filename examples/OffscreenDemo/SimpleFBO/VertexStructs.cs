using System;

using OpenTK;
using System.Runtime.InteropServices;

namespace Examples.Shapes
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexT2dN3dV3d
    {
        public Vector2d TexCoord;
        public Vector3d Normal;
        public Vector3d Position;

        public VertexT2dN3dV3d( Vector2d texcoord, Vector3d normal, Vector3d position )
        {
            TexCoord = texcoord;
            Normal = normal;
            Position = position;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VertexT2fN3fV3f
    {
        public Vector2 TexCoord;
        public Vector3 Normal;
        public Vector3 Position;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VertexT2hN3hV3h
    {
        public Vector2h TexCoord;
        public Vector3h Normal;
        public Vector3h Position;
    }

   
}
