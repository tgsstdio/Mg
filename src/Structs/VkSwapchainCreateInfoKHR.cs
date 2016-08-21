using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkSwapchainCreateInfoKHR
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public UInt64 surface { get; set; }
		public UInt32 minImageCount { get; set; }
		public VkFormat imageFormat { get; set; }
		public VkColorSpaceKhr imageColorSpace { get; set; }
		public MgExtent2D imageExtent { get; set; }
		public UInt32 imageArrayLayers { get; set; }
		public VkImageUsageFlags imageUsage { get; set; }
		public VkSharingMode imageSharingMode { get; set; }
		public UInt32 queueFamilyIndexCount { get; set; }
		public UInt32 pQueueFamilyIndices { get; set; }
		public VkSurfaceTransformFlagsKhr preTransform { get; set; }
		public VkCompositeAlphaFlagsKhr compositeAlpha { get; set; }
		public VkPresentModeKhr presentMode { get; set; }
		public VkBool32 clipped { get; set; }
		public IntPtr oldSwapchain { get; set; }
	}
}
