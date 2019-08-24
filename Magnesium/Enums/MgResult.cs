using System;

namespace Magnesium
{
    public enum MgResult : Int32
    {
        /// <summary> 
        /// Command completed successfully
        /// </summary> 
        SUCCESS = 0,
        /// <summary> 
        /// A fence or query has not yet completed
        /// </summary> 
        NOT_READY = 1,
        /// <summary> 
        /// A wait operation has not completed in the specified time
        /// </summary> 
        TIMEOUT = 2,
        /// <summary> 
        /// An event is signaled
        /// </summary> 
        EVENT_SET = 3,
        /// <summary> 
        /// An event is unsignaled
        /// </summary> 
        EVENT_RESET = 4,
        /// <summary> 
        /// A return array was too small for the result
        /// </summary> 
        INCOMPLETE = 5,
        /// <summary> 
        /// A host memory allocation has failed
        /// </summary> 
        ERROR_OUT_OF_HOST_MEMORY = -1,
        /// <summary> 
        /// A device memory allocation has failed
        /// </summary> 
        ERROR_OUT_OF_DEVICE_MEMORY = -2,
        /// <summary> 
        /// Initialization of a object has failed
        /// </summary> 
        ERROR_INITIALIZATION_FAILED = -3,
        /// <summary> 
        /// The logical device has been lost. See <<devsandqueues-lost-device>>
        /// </summary> 
        ERROR_DEVICE_LOST = -4,
        /// <summary> 
        /// Mapping of a memory object has failed
        /// </summary> 
        ERROR_MEMORY_MAP_FAILED = -5,
        /// <summary> 
        /// Layer specified does not exist
        /// </summary> 
        ERROR_LAYER_NOT_PRESENT = -6,
        /// <summary> 
        /// Extension specified does not exist
        /// </summary> 
        ERROR_EXTENSION_NOT_PRESENT = -7,
        /// <summary> 
        /// Requested feature is not available on this device
        /// </summary> 
        ERROR_FEATURE_NOT_PRESENT = -8,
        /// <summary> 
        /// Unable to find a Vulkan driver
        /// </summary> 
        ERROR_INCOMPATIBLE_DRIVER = -9,
        /// <summary> 
        /// Too many objects of the type have already been created
        /// </summary> 
        ERROR_TOO_MANY_OBJECTS = -10,
        /// <summary> 
        /// Requested format is not supported on this device
        /// </summary> 
        ERROR_FORMAT_NOT_SUPPORTED = -11,
        /// <summary> 
        /// A requested pool allocation has failed due to fragmentation of the pool's memory
        /// </summary> 
        ERROR_FRAGMENTED_POOL = -12,
        ERROR_OUT_OF_POOL_MEMORY = -1000069000,
        ERROR_INVALID_EXTERNAL_HANDLE = -1000072003,
        ERROR_SURFACE_LOST_KHR = -1000000000,
        ERROR_NATIVE_WINDOW_IN_USE_KHR = -1000000001,
        SUBOPTIMAL_KHR = 1000001003,
        ERROR_OUT_OF_DATE_KHR = -1000001004,
        ERROR_INCOMPATIBLE_DISPLAY_KHR = -1000003001,
        ERROR_VALIDATION_FAILED_EXT = -1000011001,
        ERROR_INVALID_SHADER_NV = -1000012000,
        ERROR_OUT_OF_POOL_MEMORY_KHR = ERROR_OUT_OF_POOL_MEMORY,
        ERROR_INVALID_EXTERNAL_HANDLE_KHR = ERROR_INVALID_EXTERNAL_HANDLE,
        ERROR_INVALID_DRM_FORMAT_MODIFIER_PLANE_LAYOUT_EXT = -1000158000,
        ERROR_FRAGMENTATION_EXT = -1000161000,
        ERROR_NOT_PERMITTED_EXT = -1000174001,
    }
}
