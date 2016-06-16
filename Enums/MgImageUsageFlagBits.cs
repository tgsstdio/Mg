using System;

namespace Magnesium
{
    [Flags] 
	public enum MgImageUsageFlagBits : byte
	{
		/// <summary>
		/// Can be used as a source of transfer operations
		/// </summary>
		TRANSFER_SRC_BIT = 1 << 0,
		/// <summary>
		/// Can be used as a destination of transfer operations
		/// </summary>
		TRANSFER_DST_BIT = 1 << 1,
		/// <summary>
		/// Can be sampled from (SAMPLED_IMAGE and COMBINED_IMAGE_SAMPLER descriptor types)
		/// </summary>
		SAMPLED_BIT = 1 << 2,
		/// <summary>
		/// Can be used as storage image (STORAGE_IMAGE descriptor type)
		/// </summary>
		STORAGE_BIT = 1 << 3,
		/// <summary>
		/// Can be used as framebuffer color attachment
		/// </summary>
		COLOR_ATTACHMENT_BIT = 1 << 4,
		/// <summary>
		/// Can be used as framebuffer depth/stencil attachment
		/// </summary>
		DEPTH_STENCIL_ATTACHMENT_BIT = 1 << 5,
		/// <summary>
		/// Image data not needed outside of rendering
		/// </summary>
		TRANSIENT_ATTACHMENT_BIT = 1 << 6,
		/// <summary>
		/// Can be used as framebuffer input attachment
		/// </summary>
		INPUT_ATTACHMENT_BIT = 1 << 7,
	}
}

