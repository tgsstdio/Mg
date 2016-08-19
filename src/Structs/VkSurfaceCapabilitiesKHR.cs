using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkSurfaceCapabilitiesKHR
	{
		public UInt32 minImageCount { get; set; }
		public UInt32 maxImageCount { get; set; }
		public VkExtent2D currentExtent { get; set; }
		public VkExtent2D minImageExtent { get; set; }
		public VkExtent2D maxImageExtent { get; set; }
		public UInt32 maxImageArrayLayers { get; set; }
		public VkSurfaceTransformFlagsKhr supportedTransforms { get; set; }
		public VkSurfaceTransformFlagsKhr currentTransform { get; set; }
		public VkCompositeAlphaFlagsKhr supportedCompositeAlpha { get; set; }
		public VkImageUsageFlags supportedUsageFlags { get; set; }
	}
}
