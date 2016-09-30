using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtVertexBufferEncoderState
	{
		public uint FirstBinding { get; set; }
		public AmtVertexBufferBinding[] Bindings { get; set;}
	}
}