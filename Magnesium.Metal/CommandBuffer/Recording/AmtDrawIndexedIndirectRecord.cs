using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtDrawIndexedIndirectRecord
	{
		public uint DrawCount { get; internal set; }
		public nuint Stride { get; internal set; }

		public MTLPrimitiveType PrimitiveType { get; internal set; }
		public MTLIndexType IndexType { get; internal set; }
		public IMTLBuffer IndexBuffer { get; internal set; }
		public nuint IndexBufferOffset { get; internal set; }
		public IMTLBuffer IndirectBuffer { get; internal set; }
		public nuint IndirectBufferOffset { get; internal set; }
	}
}