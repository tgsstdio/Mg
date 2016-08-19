using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkBufferImageCopy
	{
		public UInt64 bufferOffset { get; set; }
		public UInt32 bufferRowLength { get; set; }
		public UInt32 bufferImageHeight { get; set; }
		public VkImageSubresourceLayers imageSubresource { get; set; }
		public VkOffset3D imageOffset { get; set; }
		public VkExtent3D imageExtent { get; set; }
	}
}
