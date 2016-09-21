using System;
using Metal;

namespace Magnesium.Metal
{

	public struct AmtVertexLayoutBinding
	{
		public nint Index { get; set; }
		public MTLVertexStepFunction StepFunction { get; set; }
		public nuint Stride { get; set; }
	}
}
