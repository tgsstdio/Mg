using System;

namespace Magnesium
{
    [Flags]
    public enum MgImageUsageFlagBits : UInt32
    {
        /// <summary> 
        /// Can be used as a source of transfer operations
        /// </summary> 
        TRANSFER_SRC_BIT = 0x1,
        /// <summary> 
        /// Can be used as a destination of transfer operations
        /// </summary> 
        TRANSFER_DST_BIT = 0x2,
        /// <summary> 
        /// Can be sampled from (SAMPLED_IMAGE and COMBINED_IMAGE_SAMPLER descriptor types)
        /// </summary> 
        SAMPLED_BIT = 0x4,
        /// <summary> 
        /// Can be used as storage image (STORAGE_IMAGE descriptor type)
        /// </summary> 
        STORAGE_BIT = 0x8,
        /// <summary> 
        /// Can be used as framebuffer color attachment
        /// </summary> 
        COLOR_ATTACHMENT_BIT = 0x10,
        /// <summary> 
        /// Can be used as framebuffer depth/stencil attachment
        /// </summary> 
        DEPTH_STENCIL_ATTACHMENT_BIT = 0x20,
        /// <summary> 
        /// Image data not needed outside of rendering
        /// </summary> 
        TRANSIENT_ATTACHMENT_BIT = 0x40,
        /// <summary> 
        /// Can be used as framebuffer input attachment
        /// </summary> 
        INPUT_ATTACHMENT_BIT = 0x80,
        SHADING_RATE_IMAGE_BIT_NV = 0x100,
    }
}
