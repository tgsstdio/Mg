namespace Magnesium.OpenGL.Internals
{
    public class GLUniformBlockGroupInfo
    {
		public string Prefix { get;  set; }
        public int FirstBinding { get;  set; }
        public int Count { get; set; }
		public int ArrayStride { get; set; }
		public int HighestRow { get; set; }
		public int MatrixStride { get; set; }
		public int HighestLayer { get; set; }
		public int CubeStride { get; set; }
    }
}