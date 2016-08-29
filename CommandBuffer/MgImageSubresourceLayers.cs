using System;
using System.Runtime.InteropServices;

namespace Magnesium
{
	[StructLayout(LayoutKind.Sequential)]
    public struct MgImageSubresourceLayers
	{
		public MgImageAspectFlagBits AspectMask { get; set; }
		public UInt32 MipLevel { get; set; }
		public UInt32 BaseArrayLayer { get; set; }
		public UInt32 LayerCount { get; set; }
	}
}

