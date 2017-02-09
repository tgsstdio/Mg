namespace Magnesium.OpenGL
{
	public struct GLActiveUniformBlockInfo
	{
		public MgShaderStageFlagBits Stage { get; set; }
		public int BindingIndex { get; set; }
		public int Stride { get; set; }
	}
}
