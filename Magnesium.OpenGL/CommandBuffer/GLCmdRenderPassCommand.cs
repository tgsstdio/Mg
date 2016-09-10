using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	public class GLCmdRenderPassCommand
	{
		public GLCmdRenderPassCommand ()
		{
			DrawCommands = new List<GLCmdDrawCommand> ();
			ComputeCommands = new List<GLCmdComputeCommand> ();
		}

		public MgSubpassContents Contents;
		public MgClearValue[] ClearValues;
		public IMgRenderPass Origin;
		public List<GLCmdDrawCommand> DrawCommands;
		public List<GLCmdComputeCommand> ComputeCommands;
	}
}

