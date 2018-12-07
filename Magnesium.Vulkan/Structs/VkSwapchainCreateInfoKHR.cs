using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkSwapchainCreateInfoKHR
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public UInt64 surface { get; set; }
		public UInt32 minImageCount { get; set; }
		public MgFormat imageFormat { get; set; }
		public VkColorSpaceKhr imageColorSpace { get; set; }
		public MgExtent2D imageExtent { get; set; }
		public UInt32 imageArrayLayers { get; set; }
		public MgImageUsageFlagBits imageUsage { get; set; }
		public VkSharingMode imageSharingMode { get; set; }
		public UInt32 queueFamilyIndexCount { get; set; }
		public IntPtr pQueueFamilyIndices { get; set; }
		public VkSurfaceTransformFlagsKhr preTransform { get; set; }
		public VkCompositeAlphaFlagsKhr compositeAlpha { get; set; }
		public VkPresentModeKhr presentMode { get; set; }
		public VkBool32 clipped { get; set; }
		public UInt64 oldSwapchain { get; set; }
	}
}
