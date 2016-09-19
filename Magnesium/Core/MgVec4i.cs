using System;
using System.Runtime.InteropServices;

namespace Magnesium
{
    [StructLayout(LayoutKind.Sequential)]
	public struct MgVec4i
	{
		public MgVec4i (int x, int y, int z, int w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		public Int32 X { get; set; }
		public Int32 Y { get; set; }
		public Int32 Z { get; set; }
		public Int32 W { get; set; }
	}
}

