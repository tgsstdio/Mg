using System;
using System.Runtime.InteropServices;

namespace Magnesium
{
	[StructLayout(LayoutKind.Sequential)]
    public struct MgImageSubresource
	{
		public MgImageAspectFlagBits AspectMask { get; set; }
		public UInt32 MipLevel { get; set; }
		public UInt32 ArrayLayer { get; set; }
	}
}

