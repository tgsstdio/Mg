using System;
using System.Runtime.InteropServices;

namespace Magnesium
{
    [StructLayout(LayoutKind.Sequential)]	
	public struct MgOffset3D
	{
		public Int32 X { get; set; }
		public Int32 Y { get; set; }
		public Int32 Z { get; set; }
	}
}

