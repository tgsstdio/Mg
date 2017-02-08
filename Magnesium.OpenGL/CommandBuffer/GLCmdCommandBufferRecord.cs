using System;
namespace Magnesium.OpenGL.Internals
{
	public class GLCmdCommandBufferRecord
	{
		public GLCmdEncoderContext[] Contexts { get; set; }
		public GLCmdRecordInstruction[] Instructions { get; set; }
		public GLCmdGraphicsGrid GraphicsGrid { get; set; }
		public GLCmdComputeGrid ComputeGrid { get; set; }
		public GLCmdBlitGrid BlitGrid { get; set; }
	}
}
