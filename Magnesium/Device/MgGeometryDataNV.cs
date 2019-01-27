using System;
using System.Runtime.InteropServices;

namespace Magnesium
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct MgGeometryDataNV
	{
		public MgGeometryTrianglesNV triangles;
		public MgGeometryAABBNV aabbs;
	}
}
