using System;

namespace Magnesium
{
    public class MgSurfaceCapabilitiesKHR
	{
		public UInt32 MinImageCount { get; set; }
		public UInt32 MaxImageCount { get; set; }
		public MgExtent2D CurrentExtent { get; set; }
		public MgExtent2D MinImageExtent { get; set; }
		public MgExtent2D MaxImageExtent { get; set; }
		public UInt32 MaxImageArrayLayers { get; set; }
		public MgSurfaceTransformFlagBitsKHR SupportedTransforms { get; set; }
		public MgSurfaceTransformFlagBitsKHR CurrentTransform { get; set; }
		public MgCompositeAlphaFlagBitsKHR SupportedCompositeAlpha { get; set; }
		public MgImageUsageFlagBits SupportedUsageFlags { get; set; }
	}

}

