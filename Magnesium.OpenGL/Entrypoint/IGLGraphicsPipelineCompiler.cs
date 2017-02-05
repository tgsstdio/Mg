namespace Magnesium.OpenGL
{
	public interface IGLGraphicsPipelineCompiler
	{
		int Compile(MgGraphicsPipelineCreateInfo info);

        GLUniformBlockEntry[] Inspect(int programId);
    }
}

