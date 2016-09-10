using System;
using System.Runtime.InteropServices;

namespace Magnesium
{
	[StructLayout(LayoutKind.Sequential)]
    public struct MgBufferCopy
	{
		public UInt64 SrcOffset { get; set; }
		public UInt64 DstOffset { get; set; }
		public UInt64 Size { get; set; }
	}
}

