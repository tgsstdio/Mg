using System;

namespace Magnesium.Vulkan
{
	internal enum VkDebugReportObjectTypeExt : uint
	{
		DebugReportObjectTypeUnknown = 0,
		DebugReportObjectTypeInstance = 1,
		DebugReportObjectTypePhysicalDevice = 2,
		DebugReportObjectTypeDevice = 3,
		DebugReportObjectTypeQueue = 4,
		DebugReportObjectTypeSemaphore = 5,
		DebugReportObjectTypeCommandBuffer = 6,
		DebugReportObjectTypeFence = 7,
		DebugReportObjectTypeDeviceMemory = 8,
		DebugReportObjectTypeBuffer = 9,
		DebugReportObjectTypeImage = 10,
		DebugReportObjectTypeEvent = 11,
		DebugReportObjectTypeQueryPool = 12,
		DebugReportObjectTypeBufferView = 13,
		DebugReportObjectTypeImageView = 14,
		DebugReportObjectTypeShaderModule = 15,
		DebugReportObjectTypePipelineCache = 16,
		DebugReportObjectTypePipelineLayout = 17,
		DebugReportObjectTypeRenderPass = 18,
		DebugReportObjectTypePipeline = 19,
		DebugReportObjectTypeDescriptorSetLayout = 20,
		DebugReportObjectTypeSampler = 21,
		DebugReportObjectTypeDescriptorPool = 22,
		DebugReportObjectTypeDescriptorSet = 23,
		DebugReportObjectTypeFramebuffer = 24,
		DebugReportObjectTypeCommandPool = 25,
		DebugReportObjectTypeSurfaceKhr = 26,
		DebugReportObjectTypeSwapchainKhr = 27,
		DebugReportObjectTypeDebugReport = 28,
	}
}
