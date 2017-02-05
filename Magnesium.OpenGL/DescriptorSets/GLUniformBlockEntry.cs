namespace Magnesium.OpenGL
{
	public class GLUniformBlockEntry
	{
		public int ActiveIndex { get; internal set; }
		public string BlockName { get; internal set; }
        public uint FirstBinding { get; internal set; }
        public int Stride { get; internal set; }
		public GLUniformBlockInfo Token { get; internal set; }
	}
}