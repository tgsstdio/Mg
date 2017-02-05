namespace Magnesium.OpenGL
{
	public struct GLActiveUniformBlockInfo
	{
		public MgShaderStageFlagBits Stage { get; set; }
		public uint BindingIndex { get; set; }
		public int Stride { get; set; }
	}
}
