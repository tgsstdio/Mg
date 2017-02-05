using System;

namespace Magnesium.OpenGL
{
	public interface IGLUniformBlockEntrypoint
	{
		int GetMaxNoOfBindingPoints();
		int GetNoOfActiveUniformBlocks(int programId);
		string GetActiveUniformBlockName(int programId, uint activeIndex);
		GLActiveUniformBlockInfo GetActiveUniformBlockInfo(int programId, uint activeIndex);
	}
}
