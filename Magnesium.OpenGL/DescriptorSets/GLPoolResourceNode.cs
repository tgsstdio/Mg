namespace Magnesium.OpenGL
{
    public class GLPoolResourceNode
    {
        public uint First { get; set; }
        public uint Last { get; set; }
        public uint Count { get; set; }
        public GLPoolResourceNode Next { get; set; }
    }
}
