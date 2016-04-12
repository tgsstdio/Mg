using System;

namespace Magnesium
{
    public enum Result : Int32
	{
		// Command completed successfully
		SUCCESS = 0,
		// A fence or query has not yet completed
		NOT_READY = 1,
		// A wait operation has not completed in the specified time
		TIMEOUT = 2,
		// An event is signaled
		EVENT_SET = 3,
		// An event is unsignalled
		EVENT_RESET = 4,
		// A return array was too small for the resul
		INCOMPLETE = 5,
		// A host memory allocation has failed
		ERROR_OUT_OF_HOST_MEMORY = -1,
		// A device memory allocation has failed
		ERROR_OUT_OF_DEVICE_MEMORY = -2,
		// The logical device has been lost. See <<devsandqueues-lost-device>>
		ERROR_INITIALIZATION_FAILED = -3,
		// Initialization of a object has failed
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
	}

}

