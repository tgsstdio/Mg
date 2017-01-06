using System;
namespace Magnesium.OpenGL
{
	public class AmtCommandBufferRecord
	{
		public AmtEncoderContext[] Contexts { get; set; }
		public AmtRecordInstruction[] Instructions { get; set; }
		public AmtGraphicsGrid GraphicsGrid { get; set; }
		public AmtComputeGrid ComputeGrid { get; set; }
		public AmtBlitGrid BlitGrid { get; set; }
	}
}
