using System;

namespace Magnesium.Vulkan
{
	internal enum VkImageLayout : uint
	{
		ImageLayoutUndefined = 0,
		ImageLayoutGeneral = 1,
		ImageLayoutColorAttachmentOptimal = 2,
		ImageLayoutDepthStencilAttachmentOptimal = 3,
		ImageLayoutDepthStencilReadOnlyOptimal = 4,
		ImageLayoutShaderReadOnlyOptimal = 5,
		ImageLayoutTransferSrcOptimal = 6,
		ImageLayoutTransferDstOptimal = 7,
		ImageLayoutPreinitialized = 8,
		ImageLayoutPresentSrcKhr = 1000001002,
	}
}
