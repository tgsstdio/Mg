using System;
using System.Runtime.InteropServices;

namespace Magnesium
{
	[StructLayout(LayoutKind.Sequential)]
    public class MgImageSubresourceRange
	{
		public MgImageAspectFlagBits AspectMask { get; set; }
		public UInt32 BaseMipLevel { get; set; }
		public UInt32 LevelCount { get; set; }
		public UInt32 BaseArrayLayer { get; set; }
		public UInt32 LayerCount { get; set; }
	}
}

