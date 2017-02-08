using System;

namespace Magnesium.OpenGL.Internals
{
    public class GLCmdEncodingInstruction
	{
		public uint Index { get; set; }
		public GLCmdEncoderCategory Category { get; internal set; }
 		public Action<GLCmdCommandRecording, uint> Operation { get; set;}
	}
}
