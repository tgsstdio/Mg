using System;
using Metal;

namespace Magnesium.Metal
{
	public struct AmtVertexBufferBindingRecord
	{
		public IMTLBuffer VertexBuffer { get; set; }
		public nuint VertexOffset { get; set; }
	}
}
