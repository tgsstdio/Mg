using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkImageCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public MgImageCreateFlagBits flags { get; set; }
		public VkImageType imageType { get; set; }
		public MgFormat format { get; set; }
		public MgExtent3D extent { get; set; }
		public UInt32 mipLevels { get; set; }
		public UInt32 arrayLayers { get; set; }
		public MgSampleCountFlagBits samples { get; set; }
		public MgImageTiling tiling { get; set; }
		public MgImageUsageFlagBits usage { get; set; }
		public VkSharingMode sharingMode { get; set; }
		public UInt32 queueFamilyIndexCount { get; set; }
		public IntPtr pQueueFamilyIndices { get; set; }
		public MgImageLayout initialLayout { get; set; }
	}
}
