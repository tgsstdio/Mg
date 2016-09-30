using System;
using Metal;

namespace Magnesium.Metal
{
	public struct AmtVertexBufferBinding
	{
		public IMTLBuffer VertexBuffer { get; set; }
		public nuint VertexOffset { get; set; }
	}
}
