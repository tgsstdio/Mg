using System;

namespace Magnesium
{
	public class MgGeometryTrianglesNV
	{
		public UInt64 VertexData { get; set; }
		public UInt64 VertexOffset { get; set; }
		public UInt32 VertexCount { get; set; }
		public UInt64 VertexStride { get; set; }
		public MgFormat VertexFormat { get; set; }
		public UInt64 IndexData { get; set; }
		public UInt64 IndexOffset { get; set; }
		public UInt32 IndexCount { get; set; }
		public MgIndexType IndexType { get; set; }
		///
		/// Optional reference to array of floats representing a 3x4 row major affine transformation matrix.
		///
		public UInt64 TransformData { get; set; }
		public UInt64 TransformOffset { get; set; }
	}
}
