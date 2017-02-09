using System;
namespace Magnesium.OpenGL.Internals
{
	public struct GLCmdRecordInstruction
	{
		public uint Index { get; set; }
		public Action<GLCmdCommandRecording, uint> Operation { get; set; }

		public void Perform(GLCmdCommandRecording recording)
		{
			Operation(recording, Index);
		}
	}
}
