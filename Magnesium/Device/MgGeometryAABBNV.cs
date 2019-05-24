using System;

namespace Magnesium
{
	public class MgGeometryAABBNV
	{
		public IMgBuffer AabbData { get; set; }
		public UInt32 NumAABBs { get; set; }
		///
		/// Stride in bytes between AABBs
		///
		public UInt32 Stride { get; set; }
		///
		/// Offset in bytes of the first AABB in aabbData
		///
		public UInt64 Offset { get; set; }
	}
}
