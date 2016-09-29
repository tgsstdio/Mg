using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtDrawIndexedEncoderState
	{
		public nuint IndexCount { get; internal set; }
		public MTLIndexType IndexType { get; internal set; }
		public MTLPrimitiveType PrimitiveType { get; internal set; }
		public IMTLBuffer IndexBuffer { get; internal set; }
		public nuint BufferOffset { get; internal set; }
		public nuint InstanceCount { get; internal set; }
		public nint VertexOffset { get; internal set; }
		public nuint FirstInstance { get; internal set; }
	}
}