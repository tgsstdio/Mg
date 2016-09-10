using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	public interface ICmdBufferInstructionSetComposer
	{
		CmdBufferInstructionSet Compose (IGLCmdBufferRepository repository, IEnumerable<GLCmdRenderPassCommand> passes);
	}
}

