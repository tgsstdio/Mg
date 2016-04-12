using System;

namespace Magnesium
{
    public class MgSwapchainCreateInfoKHR
	{
		public UInt32 Flags { get; set; }
		public MgSurfaceKHR Surface { get; set; }
		public UInt32 MinImageCount { get; set; }
		public MgFormat ImageFormat { get; set; }
		public MgColorSpaceKHR ImageColorSpace { get; set; }
		public MgExtent2D ImageExtent { get; set; }
		public UInt32 ImageArrayLayers { get; set; }
		public MgImageUsageFlagBits ImageUsage { get; set; }
		public MgSharingMode ImageSharingMode { get; set; }
		public UInt32[] QueueFamilyIndices { get; set; }
		public MgSurfaceTransformFlagBitsKHR PreTransform { get; set; }
		public MgCompositeAlphaFlagBitsKHR CompositeAlpha { get; set; }
		public MgPresentModeKHR PresentMode { get; set; }
		public bool Clipped { get; set; }
		public MgSwapchainKHR OldSwapchain { get; set; }
	}

}

