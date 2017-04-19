namespace Magnesium.OpenGL
{
	public class GLUniformBlockEntry
	{
		public int ActiveIndex { get; set; }
		public string BlockName { get; set; }
        public int FirstBinding { get; set; }
        public int Stride { get; set; }
		public GLUniformBlockInfo Token { get; set; }
	}
}