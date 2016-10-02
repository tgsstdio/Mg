using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtDrawIndirectRecord
	{
		public MTLPrimitiveType PrimitiveType { get; internal set; }

		public nuint Stride { get; internal set;}
		public IMTLBuffer IndirectBuffer { get; internal set; }
		public nuint IndirectBufferOffset { get; internal set; }

		public uint DrawCount { get; internal set; }
	}
}