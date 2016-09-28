using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtDispatchIndirectEncoderState
	{
		public IMTLBuffer IndirectBuffer { get; internal set; }
		public nuint Offset { get; internal set; }
		public MTLSize ThreadsPerGroupSize { get; internal set; }
	}
}