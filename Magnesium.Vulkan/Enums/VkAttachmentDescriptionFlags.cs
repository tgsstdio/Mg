using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkAttachmentDescriptionFlags : uint
	{
		AttachmentDescriptionMayAlias = 0x1,
	}
}
