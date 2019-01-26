using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkPhysicalDeviceImageFormatInfo2
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public MgFormat format { get; set; }
		public VkImageType type { get; set; }
		public MgImageTiling tiling { get; set; }
		public MgImageUsageFlagBits usage { get; set; }
		public MgImageCreateFlagBits flags { get; set; }
	}
}
