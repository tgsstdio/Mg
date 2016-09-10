using System;
using System.Runtime.InteropServices;

namespace Magnesium
{
    [StructLayout(LayoutKind.Sequential)]
	public struct MgVec3Ui
	{
		public UInt32 X { get; set; }
		public UInt32 Y { get; set; }
		public UInt32 Z { get; set; }
	}
}

