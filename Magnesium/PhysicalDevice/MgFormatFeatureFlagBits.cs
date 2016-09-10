using System;

namespace Magnesium
{
    [Flags] 
	public enum MgFormatFeatureFlagBits : ushort
	{
		// Format can be used for sampled images (SAMPLED_IMAGE and COMBINED_IMAGE_SAMPLER descriptor types)
		SAMPLED_IMAGE_BIT = 1 << 0,
		// Format can be used for storage images (STORAGE_IMAGE descriptor type)
		STORAGE_IMAGE_BIT = 1 << 1,
		// Format supports atomic operations in case it's used for storage images
		STORAGE_IMAGE_ATOMIC_BIT = 1 << 2,
		// Format can be used for uniform texel buffers (TBOs)
		UNIFORM_TEXEL_BUFFER_BIT = 1 << 3,
		// Format can be used for storage texel buffers (IBOs)
		STORAGE_TEXEL_BUFFER_BIT = 1 << 4,
		// Format supports atomic operations in case it's used for storage texel buffers
		STORAGE_TEXEL_BUFFER_ATOMIC_BIT = 1 << 5,
		// Format can be used for vertex buffers (VBOs)
		VERTEX_BUFFER_BIT = 1 << 6,
		// Format can be used for color attachment images
		COLOR_ATTACHMENT_BIT = 1 << 7,
		// Format supports blending in case it's used for color attachment images
		COLOR_ATTACHMENT_BLEND_BIT = 1 << 8,
		// Format can be used for depth/stencil attachment images
		DEPTH_STENCIL_ATTACHMENT_BIT = 1 << 9,
		// Format can be used as the source image of blits with vkCmdBlitImage
		BLIT_SRC_BIT = 1 << 10,
		// Format can be used as the destination image of blits with vkCmdBlitImage
		BLIT_DST_BIT = 1 << 11,
		// Format can be filtered with VK_FILTER_LINEAR when being sampled
		SAMPLED_IMAGE_FILTER_LINEAR_BIT = 1 << 12,
	}
}

