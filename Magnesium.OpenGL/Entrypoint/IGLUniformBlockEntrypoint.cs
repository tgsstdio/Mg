using System;

namespace Magnesium.OpenGL
{
	public interface IGLUniformBlockEntrypoint
	{
		int GetNoOfActiveUniformBlocks(int programId);
		string GetActiveUniformBlockName(int programId, int activeIndex);
		GLActiveUniformBlockInfo GetActiveUniformBlockInfo(int programId, int activeIndex);
    }
}
