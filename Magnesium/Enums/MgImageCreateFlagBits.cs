using System;

namespace Magnesium
{
    [Flags]
    public enum MgImageCreateFlagBits : UInt32
    {
        /// <summary> 
        /// Image should support sparse backing
        /// </summary> 
        SPARSE_BINDING_BIT = 0x1,
        /// <summary> 
        /// Image should support sparse backing with partial residency
        /// </summary> 
        SPARSE_RESIDENCY_BIT = 0x2,
        /// <summary> 
        /// Image should support constent data access to physical memory ranges mapped into multiple locations of sparse images
        /// </summary> 
        SPARSE_ALIASED_BIT = 0x4,
        /// <summary> 
        /// Allows image views to have different format than the base image
        /// </summary> 
        MUTABLE_FORMAT_BIT = 0x8,
        /// <summary> 
        /// Allows creating image views with cube type from the created image
        /// </summary> 
        CUBE_COMPATIBLE_BIT = 0x10,
        ALIAS_BIT = 0x400,
        /// <summary> 
        /// Allows using VkBindImageMemoryDeviceGroupInfo::pSplitInstanceBindRegions when binding memory to the image
        /// </summary> 
        SPLIT_INSTANCE_BIND_REGIONS_BIT = 0x40,
        /// <summary> 
        /// The 3D image can be viewed as a 2D or 2D array image
        /// </summary> 
        CREATE_2D_ARRAY_COMPATIBLE_BIT = 0x20,
        BLOCK_TEXEL_VIEW_COMPATIBLE_BIT = 0x80,
        EXTENDED_USAGE_BIT = 0x100,
        /// <summary> 
        /// Image requires protected memory
        /// </summary> 
        PROTECTED_BIT = 0x800,
        DISJOINT_BIT = 0x200,
        CORNER_SAMPLED_BIT_NV = 0x2000,
        SPLIT_INSTANCE_BIND_REGIONS_BIT_KHR = SPLIT_INSTANCE_BIND_REGIONS_BIT,
        CREATE_2D_ARRAY_COMPATIBLE_BIT_KHR = CREATE_2D_ARRAY_COMPATIBLE_BIT,
        BLOCK_TEXEL_VIEW_COMPATIBLE_BIT_KHR = BLOCK_TEXEL_VIEW_COMPATIBLE_BIT,
        EXTENDED_USAGE_BIT_KHR = EXTENDED_USAGE_BIT,
        SAMPLE_LOCATIONS_COMPATIBLE_DEPTH_BIT_EXT = 0x1000,
        DISJOINT_BIT_KHR = DISJOINT_BIT,
        ALIAS_BIT_KHR = ALIAS_BIT,
    }
}


