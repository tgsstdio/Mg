using System;
using System.Runtime.InteropServices;

namespace Magnesium
{
	[StructLayout(LayoutKind.Sequential)]
    public struct MgClearAttachment
	{
		public MgImageAspectFlagBits AspectMask { get; set; }
		public UInt32 ColorAttachment { get; set; }
		public MgClearValue ClearValue { get; set; }
	}
}

