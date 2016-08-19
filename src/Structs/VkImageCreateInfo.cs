using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkImageCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public VkImageCreateFlags flags { get; set; }
		public VkImageType imageType { get; set; }
		public VkFormat format { get; set; }
		public VkExtent3D extent { get; set; }
		public UInt32 mipLevels { get; set; }
		public UInt32 arrayLayers { get; set; }
		public VkSampleCountFlags samples { get; set; }
		public VkImageTiling tiling { get; set; }
		public VkImageUsageFlags usage { get; set; }
		public VkSharingMode sharingMode { get; set; }
		public UInt32 queueFamilyIndexCount { get; set; }
		public UInt32 pQueueFamilyIndices { get; set; }
		public VkImageLayout initialLayout { get; set; }
	}
}
