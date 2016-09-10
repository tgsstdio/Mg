using System;
using System.Runtime.InteropServices;

namespace Magnesium
{
    [StructLayout(LayoutKind.Sequential)]	
	public struct MgExtent3D
	{
		public UInt32 Width { get; set; }
		public UInt32 Height { get; set; }
		public UInt32 Depth { get; set; }
	}
}

