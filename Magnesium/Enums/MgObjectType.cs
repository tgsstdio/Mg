using System;

namespace Magnesium
{
	public enum MgObjectType : UInt32
	{
		UNKNOWN = 0,
		/// <summary> 
		/// VkInstance
		/// </summary> 
		INSTANCE = 1,
		/// <summary> 
		/// VkPhysicalDevice
		/// </summary> 
		PHYSICAL_DEVICE = 2,
		/// <summary> 
		/// VkDevice
		/// </summary> 
		DEVICE = 3,
		/// <summary> 
		/// VkQueue
		/// </summary> 
		QUEUE = 4,
		/// <summary> 
		/// VkSemaphore
		/// </summary> 
		SEMAPHORE = 5,
		/// <summary> 
		/// VkCommandBuffer
		/// </summary> 
		COMMAND_BUFFER = 6,
		/// <summary> 
		/// VkFence
		/// </summary> 
		FENCE = 7,
		/// <summary> 
		/// VkDeviceMemory
		/// </summary> 
		DEVICE_MEMORY = 8,
		/// <summary> 
		/// VkBuffer
		/// </summary> 
		BUFFER = 9,
		/// <summary> 
		/// VkImage
		/// </summary> 
		IMAGE = 10,
		/// <summary> 
		/// VkEvent
		/// </summary> 
		EVENT = 11,
		/// <summary> 
		/// VkQueryPool
		/// </summary> 
		QUERY_POOL = 12,
		/// <summary> 
		/// VkBufferView
		/// </summary> 
		BUFFER_VIEW = 13,
		/// <summary> 
		/// VkImageView
		/// </summary> 
		IMAGE_VIEW = 14,
		/// <summary> 
		/// VkShaderModule
		/// </summary> 
		SHADER_MODULE = 15,
		/// <summary> 
		/// VkPipelineCache
		/// </summary> 
		PIPELINE_CACHE = 16,
		/// <summary> 
		/// VkPipelineLayout
		/// </summary> 
		PIPELINE_LAYOUT = 17,
		/// <summary> 
		/// VkRenderPass
		/// </summary> 
		RENDER_PASS = 18,
		/// <summary> 
		/// VkPipeline
		/// </summary> 
		PIPELINE = 19,
		/// <summary> 
		/// VkDescriptorSetLayout
		/// </summary> 
		DESCRIPTOR_SET_LAYOUT = 20,
		/// <summary> 
		/// VkSampler
		/// </summary> 
		SAMPLER = 21,
		/// <summary> 
		/// VkDescriptorPool
		/// </summary> 
		DESCRIPTOR_POOL = 22,
		/// <summary> 
		/// VkDescriptorSet
		/// </summary> 
		DESCRIPTOR_SET = 23,
		/// <summary> 
		/// VkFramebuffer
		/// </summary> 
		FRAMEBUFFER = 24,
		/// <summary> 
		/// VkCommandPool
		/// </summary> 
		COMMAND_POOL = 25,
		SAMPLER_YCBCR_CONVERSION = 1000156000,
		DESCRIPTOR_UPDATE_TEMPLATE = 1000085000,
		/// <summary> 
		/// VkSurfaceKHR
		/// </summary> 
		SURFACE_KHR = 1000000000,
		/// <summary> 
		/// VkSwapchainKHR
		/// </summary> 
		SWAPCHAIN_KHR = 1000001000,
		/// <summary> 
		/// VkDisplayKHR
		/// </summary> 
		DISPLAY_KHR = 1000002000,
		/// <summary> 
		/// VkDisplayModeKHR
		/// </summary> 
		DISPLAY_MODE_KHR = 1000002001,
		/// <summary> 
		/// VkDebugReportCallbackEXT
		/// </summary> 
		DEBUG_REPORT_CALLBACK_EXT = 1000011000,
		DESCRIPTOR_UPDATE_TEMPLATE_KHR = DESCRIPTOR_UPDATE_TEMPLATE,
		/// <summary> 
		/// VkobjectTableNVX
		/// </summary> 
		OBJECT_TABLE_NVX = 1000086000,
		/// <summary> 
		/// VkIndirectCommandsLayoutNVX
		/// </summary> 
		INDIRECT_COMMANDS_LAYOUT_NVX = 1000086001,
		/// <summary> 
		/// VkDebugUtilsMessengerEXT
		/// </summary> 
		DEBUG_UTILS_MESSENGER_EXT = 1000128000,
		SAMPLER_YCBCR_CONVERSION_KHR = SAMPLER_YCBCR_CONVERSION,
		/// <summary> 
		/// VkValidationCacheEXT
		/// </summary> 
		VALIDATION_CACHE_EXT = 1000160000,
		ACCELERATION_STRUCTURE_NV = 1000165000,
	}
}
