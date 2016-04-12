using System;

namespace Magnesium
{
    public class MgImageSubresourceLayers
	{
		public MgImageAspectFlagBits AspectMask { get; set; }
		public UInt32 MipLevel { get; set; }
		public UInt32 BaseArrayLayer { get; set; }
		public UInt32 LayerCount { get; set; }
	}
}

