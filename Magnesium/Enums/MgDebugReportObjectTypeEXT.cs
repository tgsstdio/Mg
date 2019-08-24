using System;

namespace Magnesium
{
    public enum MgDebugReportObjectTypeEXT : UInt32
    {
        UNKNOWN_EXT = 0,
        INSTANCE_EXT = 1,
        PHYSICAL_DEVICE_EXT = 2,
        DEVICE_EXT = 3,
        QUEUE_EXT = 4,
        SEMAPHORE_EXT = 5,
        COMMAND_BUFFER_EXT = 6,
        FENCE_EXT = 7,
        DEVICE_MEMORY_EXT = 8,
        BUFFER_EXT = 9,
        IMAGE_EXT = 10,
        EVENT_EXT = 11,
        QUERY_POOL_EXT = 12,
        BUFFER_VIEW_EXT = 13,
        IMAGE_VIEW_EXT = 14,
        SHADER_MODULE_EXT = 15,
        PIPELINE_CACHE_EXT = 16,
        PIPELINE_LAYOUT_EXT = 17,
        RENDER_PASS_EXT = 18,
        PIPELINE_EXT = 19,
        DESCRIPTOR_SET_LAYOUT_EXT = 20,
        SAMPLER_EXT = 21,
        DESCRIPTOR_POOL_EXT = 22,
        DESCRIPTOR_SET_EXT = 23,
        FRAMEBUFFER_EXT = 24,
        COMMAND_POOL_EXT = 25,
        SURFACE_KHR_EXT = 26,
        SWAPCHAIN_KHR_EXT = 27,
        DEBUG_REPORT_CALLBACK_EXT_EXT = 28,
        /// <summary> 
        /// Backwards-compatible alias containing a typo
        /// </summary> 
        DEBUG_REPORT_EXT = DEBUG_REPORT_CALLBACK_EXT_EXT,
        DISPLAY_KHR_EXT = 29,
        DISPLAY_MODE_KHR_EXT = 30,
        OBJECT_TABLE_NVX_EXT = 31,
        INDIRECT_COMMANDS_LAYOUT_NVX_EXT = 32,
        VALIDATION_CACHE_EXT_EXT = 33,
        /// <summary> 
        /// Backwards-compatible alias containing a typo
        /// </summary> 
        VALIDATION_CACHE_EXT = VALIDATION_CACHE_EXT_EXT,
        SAMPLER_YCBCR_CONVERSION_EXT = 1000156000,
        SAMPLER_YCBCR_CONVERSION_KHR_EXT = SAMPLER_YCBCR_CONVERSION_EXT,
        DESCRIPTOR_UPDATE_TEMPLATE_EXT = 1000085000,
        DESCRIPTOR_UPDATE_TEMPLATE_KHR_EXT = DESCRIPTOR_UPDATE_TEMPLATE_EXT,
        ACCELERATION_STRUCTURE_NV_EXT = 1000165000,
    }
}

