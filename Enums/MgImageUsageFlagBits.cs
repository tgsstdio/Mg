using System;

namespace Magnesium
{
    [Flags] 
	public enum MgImageUsageFlagBits : byte
	{
		// Can be used as a source of transfer operations
		TRANSFER_SRC_BIT = 1 << 0,
		// Can be used as a destination of transfer operations
		TRANSFER_DST_BIT = 1 << 1,
		// Can be sampled from (SAMPLED_IMAGE and COMBINED_IMAGE_SAMPLER descriptor types)
		SAMPLED_BIT = 1 << 2,
		// Can be used as storage image (STORAGE_IMAGE descriptor type)
		STORAGE_BIT = 1 << 3,
		// Can be used as framebuffer color attachment
		COLOR_ATTACHMENT_BIT = 1 << 4,
		// Can be used as framebuffer depth/stencil attachment
		DEPTH_STENCIL_ATTACHMENT_BIT = 1 << 5,
		// Image data not needed outside of rendering
		TRANSIENT_ATTACHMENT_BIT = 1 << 6,
		// Can be used as framebuffer input attachment
		INPUT_ATTACHMENT_BIT = 1 << 7,
	}
}

