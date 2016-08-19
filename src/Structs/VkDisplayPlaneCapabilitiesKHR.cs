using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkDisplayPlaneCapabilitiesKHR
	{
		public VkDisplayPlaneAlphaFlagsKhr supportedAlpha { get; set; }
		public VkOffset2D minSrcPosition { get; set; }
		public VkOffset2D maxSrcPosition { get; set; }
		public VkExtent2D minSrcExtent { get; set; }
		public VkExtent2D maxSrcExtent { get; set; }
		public VkOffset2D minDstPosition { get; set; }
		public VkOffset2D maxDstPosition { get; set; }
		public VkExtent2D minDstExtent { get; set; }
		public VkExtent2D maxDstExtent { get; set; }
	}
}
