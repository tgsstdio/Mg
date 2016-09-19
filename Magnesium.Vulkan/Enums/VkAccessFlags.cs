using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkAccessFlags : uint
	{
		AccessIndirectCommandRead = 0x1,
		AccessIndexRead = 0x2,
		AccessVertexAttributeRead = 0x4,
		AccessUniformRead = 0x8,
		AccessInputAttachmentRead = 0x10,
		AccessShaderRead = 0x20,
		AccessShaderWrite = 0x40,
		AccessColorAttachmentRead = 0x80,
		AccessColorAttachmentWrite = 0x100,
		AccessDepthStencilAttachmentRead = 0x200,
		AccessDepthStencilAttachmentWrite = 0x400,
		AccessTransferRead = 0x800,
		AccessTransferWrite = 0x1000,
		AccessHostRead = 0x2000,
		AccessHostWrite = 0x4000,
		AccessMemoryRead = 0x8000,
		AccessMemoryWrite = 0x10000,
	}
}
