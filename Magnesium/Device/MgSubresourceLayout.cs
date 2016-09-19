using System;
using System.Runtime.InteropServices;

namespace Magnesium
{
	[StructLayout(LayoutKind.Sequential)]
    public struct MgSubresourceLayout
	{
		public UInt64 Offset { get; set; }
		public UInt64 Size { get; set; }
		public UInt64 RowPitch { get; set; }
		public UInt64 ArrayPitch { get; set; }
		public UInt64 DepthPitch { get; set; }
	}
}

