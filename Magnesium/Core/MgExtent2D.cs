using System;
using System.Runtime.InteropServices;

namespace Magnesium
{
    [StructLayout(LayoutKind.Sequential)]	
	public struct MgExtent2D
	{
		public UInt32 Width { get; set; }
		public UInt32 Height { get; set; }
	}
}

