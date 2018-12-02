using System;

namespace Magnesium
{
    public enum MgResult : Int32
    {
        // Command completed successfully
        SUCCESS = 0,
        // A fence or query has not yet completed
        NOT_READY = 1,
        // A wait operation has not completed in the specified time
        TIMEOUT = 2,
        // An event is signaled
        EVENT_SET = 3,
        // An event is unsignaled
        EVENT_RESET = 4,
        // A return array was too small for the result
        INCOMPLETE = 5,
        // A host memory allocation has failed
        ERROR_OUT_OF_HOST_MEMORY = -1,
        // A device memory allocation has failed
        ERROR_OUT_OF_DEVICE_MEMORY = -2,
        // Initialization of a object has failed
        ERROR_INITIALIZATION_FAILED = -3,
        // The logical device has been lost. See <<devsandqueues-lost-device>>
        ERROR_DEVICE_LOST = -4,
        // Mapping of a memory object has failed
        ERROR_MEMORY_MAP_FAILED = -5,
        // Layer specified does not exist
        ERROR_LAYER_NOT_PRESENT = -6,
        // Extension specified does not exist
        ERROR_EXTENSION_NOT_PRESENT = -7,
        // Requested feature is not available on this device
        ERROR_FEATURE_NOT_PRESENT = -8,
        // Unable to find a Vulkan driver
        ERROR_INCOMPATIBLE_DRIVER = -9,
        // Too many objects of the type have already been created
        ERROR_TOO_MANY_OBJECTS = -10,
        // Requested format is not supported on this device
        ERROR_FORMAT_NOT_SUPPORTED = -11,
        // <summary>
        // A requested pool allocation has failed due to fragmentation of the pool's memory
        // </summary>
        ERROR_FRAGMENTED_POOL = -12,
        ERROR_SURFACE_LOST_KHR = -1000000000,
        ERROR_NATIVE_WINDOW_IN_USE_KHR = -1000000001,
        SUBOPTIMAL_KHR = 1000001003,
        ERROR_OUT_OF_DATE_KHR = -1000001004,
        ERROR_INCOMPATIBLE_DISPLAY_KHR = -1000003001,
        ERROR_VALIDATION_FAILED_EXT = -1000011001,
        ERROR_INVALID_SHADER_NV = -1000012000,
        ERROR_OUT_OF_POOL_MEMORY_KHR = 1000069000,
        ERROR_OUT_OF_POOL_MEMORY = ERROR_OUT_OF_POOL_MEMORY_KHR,
        ERROR_INVALID_EXTERNAL_HANDLE_KHR = 1000072000,
        ERROR_INVALID_EXTERNAL_HANDLE = ERROR_INVALID_EXTERNAL_HANDLE_KHR,
        ERROR_INVALID_DRM_FORMAT_MODIFIER_PLANE_LAYOUT_EXT = -1000158000,
        ERROR_FRAGMENTATION_EXT = -1000161000,
        ERROR_NOT_PERMITTED_EXT = -1000174001,
    }
}

