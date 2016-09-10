using System;
using System.Runtime.InteropServices;

namespace Magnesium
{
	[StructLayout(LayoutKind.Sequential)]
    public struct MgMemoryRequirements
	{
		public UInt64 Size { get; set; }
		public UInt64 Alignment { get; set; }
		public UInt32 MemoryTypeBits { get; set; }
	}
}

