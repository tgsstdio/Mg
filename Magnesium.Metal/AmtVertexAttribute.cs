using System;
using Metal;

namespace Magnesium.Metal
{
	public struct AmtVertexAttribute
	{
		public nint Index { get; set; }
		public nuint Offset { get; internal set; }
		public nuint BufferIndex { get; internal set; }
		public MTLVertexFormat Format { get; internal set; }
	}
}
