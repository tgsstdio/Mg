using System;

namespace Magnesium.Vulkan
{
	internal enum VkAttachmentLoadOp : uint
	{
		AttachmentLoadOpLoad = 0,
		AttachmentLoadOpClear = 1,
		AttachmentLoadOpDontCare = 2,
	}
}
