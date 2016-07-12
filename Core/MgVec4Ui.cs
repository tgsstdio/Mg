using System;
using System.Runtime.InteropServices;

namespace Magnesium
{
    [StructLayout(LayoutKind.Sequential)]
	public struct MgVec4Ui
	{
		public MgVec4Ui (UInt32 x, UInt32 y, UInt32 z, UInt32 w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		public UInt32 X { get; set; }
		public UInt32 Y { get; set; }
		public UInt32 Z { get; set; }
		public UInt32 W { get; set; }
	}
}

