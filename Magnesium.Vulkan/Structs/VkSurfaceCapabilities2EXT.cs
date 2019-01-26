using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkSurfaceCapabilities2EXT
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		// Supported minimum number of images for the surface
		public UInt32 minImageCount { get; set; }
		// Supported maximum number of images for the surface, 0 for unlimited
		public UInt32 maxImageCount { get; set; }
		// Current image width and height for the surface, (0, 0) if undefined
		public MgExtent2D currentExtent { get; set; }
		// Supported minimum image width and height for the surface
		public MgExtent2D minImageExtent { get; set; }
		// Supported maximum image width and height for the surface
		public MgExtent2D maxImageExtent { get; set; }
		// Supported maximum number of image layers for the surface
		public UInt32 maxImageArrayLayers { get; set; }
		// 1 or more bits representing the transforms supported
		public MgSurfaceTransformFlagBitsKHR supportedTransforms { get; set; }
		// The surface's current transform relative to the device's natural orientation
		public MgSurfaceTransformFlagBitsKHR currentTransform { get; set; }
		// 1 or more bits representing the alpha compositing modes supported
		public MgCompositeAlphaFlagBitsKHR supportedCompositeAlpha { get; set; }
		// Supported image usage flags for the surface
		public MgImageUsageFlagBits supportedUsageFlags { get; set; }
		public MgSurfaceCounterFlagBitsEXT supportedSurfaceCounters { get; set; }
	}
}
