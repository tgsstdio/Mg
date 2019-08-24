using System;

namespace Magnesium
{
    [Flags]
    public enum MgFormatFeatureFlagBits : UInt32
    {
        /// <summary> 
        /// Format can be used for sampled images (SAMPLED_IMAGE and COMBINED_IMAGE_SAMPLER descriptor types)
        /// </summary> 
        SAMPLED_IMAGE_BIT = 0x1,
        /// <summary> 
        /// Format can be used for storage images (STORAGE_IMAGE descriptor type)
        /// </summary> 
        STORAGE_IMAGE_BIT = 0x2,
        /// <summary> 
        /// Format supports atomic operations in case it is used for storage images
        /// </summary> 
        STORAGE_IMAGE_ATOMIC_BIT = 0x4,
        /// <summary> 
        /// Format can be used for uniform texel buffers (TBOs)
        /// </summary> 
        UNIFORM_TEXEL_BUFFER_BIT = 0x8,
        /// <summary> 
        /// Format can be used for storage texel buffers (IBOs)
        /// </summary> 
        STORAGE_TEXEL_BUFFER_BIT = 0x10,
        /// <summary> 
        /// Format supports atomic operations in case it is used for storage texel buffers
        /// </summary> 
        STORAGE_TEXEL_BUFFER_ATOMIC_BIT = 0x20,
        /// <summary> 
        /// Format can be used for vertex buffers (VBOs)
        /// </summary> 
        VERTEX_BUFFER_BIT = 0x40,
        /// <summary> 
        /// Format can be used for color attachment images
        /// </summary> 
        COLOR_ATTACHMENT_BIT = 0x80,
        /// <summary> 
        /// Format supports blending in case it is used for color attachment images
        /// </summary> 
        COLOR_ATTACHMENT_BLEND_BIT = 0x100,
        /// <summary> 
        /// Format can be used for depth/stencil attachment images
        /// </summary> 
        DEPTH_STENCIL_ATTACHMENT_BIT = 0x200,
        /// <summary> 
        /// Format can be used as the source image of blits with vkCmdBlitImage
        /// </summary> 
        BLIT_SRC_BIT = 0x400,
        /// <summary> 
        /// Format can be used as the destination image of blits with vkCmdBlitImage
        /// </summary> 
        BLIT_DST_BIT = 0x800,
        /// <summary> 
        /// Format can be filtered with VK_FILTER_LINEAR when being sampled
        /// </summary> 
        SAMPLED_IMAGE_FILTER_LINEAR_BIT = 0x1000,
        /// <summary> 
        /// Format can be used as the source image of image transfer commands
        /// </summary> 
        TRANSFER_SRC_BIT = 0x4000,
        /// <summary> 
        /// Format can be used as the destination image of image transfer commands
        /// </summary> 
        TRANSFER_DST_BIT = 0x8000,
        /// <summary> 
        /// Format can have midpoint rather than cosited chroma samples
        /// </summary> 
        MIDPOINT_CHROMA_SAMPLES_BIT = 0x20000,
        /// <summary> 
        /// Format can be used with linear filtering whilst color conversion is enabled
        /// </summary> 
        SAMPLED_IMAGE_YCBCR_CONVERSION_LINEAR_FILTER_BIT = 0x40000,
        /// <summary> 
        /// Format can have different chroma, min and mag filters
        /// </summary> 
        SAMPLED_IMAGE_YCBCR_CONVERSION_SEPARATE_RECONSTRUCTION_FILTER_BIT = 0x80000,
        SAMPLED_IMAGE_YCBCR_CONVERSION_CHROMA_RECONSTRUCTION_EXPLICIT_BIT = 0x100000,
        SAMPLED_IMAGE_YCBCR_CONVERSION_CHROMA_RECONSTRUCTION_EXPLICIT_FORCEABLE_BIT = 0x200000,
        /// <summary> 
        /// Format supports disjoint planes
        /// </summary> 
        DISJOINT_BIT = 0x400000,
        /// <summary> 
        /// Format can have cosited rather than midpoint chroma samples
        /// </summary> 
        COSITED_CHROMA_SAMPLES_BIT = 0x800000,
        /// <summary> 
        /// Format can be filtered with VK_FILTER_CUBIC_IMG when being sampled
        /// </summary> 
        SAMPLED_IMAGE_FILTER_CUBIC_BIT_IMG = 0x2000,
        TRANSFER_SRC_BIT_KHR = TRANSFER_SRC_BIT,
        TRANSFER_DST_BIT_KHR = TRANSFER_DST_BIT,
        /// <summary> 
        /// Format can be used with min/max reduction filtering
        /// </summary> 
        SAMPLED_IMAGE_FILTER_MINMAX_BIT_EXT = 0x10000,
        MIDPOINT_CHROMA_SAMPLES_BIT_KHR = MIDPOINT_CHROMA_SAMPLES_BIT,
        SAMPLED_IMAGE_YCBCR_CONVERSION_LINEAR_FILTER_BIT_KHR = SAMPLED_IMAGE_YCBCR_CONVERSION_LINEAR_FILTER_BIT,
        SAMPLED_IMAGE_YCBCR_CONVERSION_SEPARATE_RECONSTRUCTION_FILTER_BIT_KHR = SAMPLED_IMAGE_YCBCR_CONVERSION_SEPARATE_RECONSTRUCTION_FILTER_BIT,
        SAMPLED_IMAGE_YCBCR_CONVERSION_CHROMA_RECONSTRUCTION_EXPLICIT_BIT_KHR = SAMPLED_IMAGE_YCBCR_CONVERSION_CHROMA_RECONSTRUCTION_EXPLICIT_BIT,
        SAMPLED_IMAGE_YCBCR_CONVERSION_CHROMA_RECONSTRUCTION_EXPLICIT_FORCEABLE_BIT_KHR = SAMPLED_IMAGE_YCBCR_CONVERSION_CHROMA_RECONSTRUCTION_EXPLICIT_FORCEABLE_BIT,
        DISJOINT_BIT_KHR = DISJOINT_BIT,
        COSITED_CHROMA_SAMPLES_BIT_KHR = COSITED_CHROMA_SAMPLES_BIT,
    }
}
