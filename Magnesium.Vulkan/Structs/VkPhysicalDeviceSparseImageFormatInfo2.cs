using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkPhysicalDeviceSparseImageFormatInfo2
	{
		public VkStructureType sType;
		public IntPtr pNext;
		public MgFormat format;
		public VkImageType type;
		public MgSampleCountFlagBits samples;
		public MgImageUsageFlagBits usage;
		public MgImageTiling tiling;
	}
}
