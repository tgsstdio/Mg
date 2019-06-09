using Magnesium;
using System;
using System.Runtime.InteropServices;
namespace Magnesium.Vulkan
{
	internal static class Interops
	{
		public const string VULKAN_LIB = "vulkan-1";

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkCreateInstance(ref VkInstanceCreateInfo pCreateInfo, IntPtr pAllocator, ref IntPtr pInstance);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyInstance(IntPtr instance, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkEnumeratePhysicalDevices(IntPtr instance, ref UInt32 pPhysicalDeviceCount, [In, Out] IntPtr[] pPhysicalDevices);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static PFN_vkVoidFunction vkGetDeviceProcAddr(IntPtr device, [MarshalAs(UnmanagedType.LPStr)] string pName);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static PFN_vkVoidFunction vkGetInstanceProcAddr(IntPtr instance, [MarshalAs(UnmanagedType.LPStr)] string pName);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceProperties(IntPtr physicalDevice, ref VkPhysicalDeviceProperties pProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkGetPhysicalDeviceQueueFamilyProperties(IntPtr physicalDevice, UInt32* pQueueFamilyPropertyCount, VkQueueFamilyProperties* pQueueFamilyProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceMemoryProperties(IntPtr physicalDevice, ref VkPhysicalDeviceMemoryProperties pMemoryProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceFeatures(IntPtr physicalDevice, ref VkPhysicalDeviceFeatures pFeatures);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkGetPhysicalDeviceFormatProperties(IntPtr physicalDevice, MgFormat format, ref VkFormatProperties pFormatProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetPhysicalDeviceImageFormatProperties(IntPtr physicalDevice, MgFormat format, VkImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, ref VkImageFormatProperties pImageFormatProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkCreateDevice(IntPtr physicalDevice, ref VkDeviceCreateInfo pCreateInfo, IntPtr pAllocator, ref IntPtr pDevice);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static void vkDestroyDevice(IntPtr device, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkEnumerateInstanceLayerProperties(ref UInt32 pPropertyCount, [In, Out] VkLayerProperties[] pProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkEnumerateInstanceExtensionProperties(IntPtr pLayerName, ref UInt32 pPropertyCount, [In, Out] VkExtensionProperties[] pProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkEnumerateDeviceLayerProperties(IntPtr physicalDevice, ref UInt32 pPropertyCount, [In, Out] VkLayerProperties[] pProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkEnumerateDeviceExtensionProperties(IntPtr physicalDevice, IntPtr pLayerName, ref UInt32 pPropertyCount, [In, Out] VkExtensionProperties[] pProperties);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static void vkGetDeviceQueue(IntPtr device, UInt32 queueFamilyIndex, UInt32 queueIndex, ref IntPtr pQueue);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkQueueSubmit(IntPtr queue, UInt32 submitCount, VkSubmitInfo* pSubmits, UInt64 fence);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkQueueWaitIdle(IntPtr queue);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkDeviceWaitIdle(IntPtr device);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static unsafe MgResult vkAllocateMemory(IntPtr device, VkMemoryAllocateInfo* pAllocateInfo, IntPtr pAllocator, UInt64* pMemory);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkFreeMemory(IntPtr device, UInt64 memory, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkMapMemory(IntPtr device, UInt64 memory, UInt64 offset, UInt64 size, UInt32 flags, ref IntPtr ppData);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkUnmapMemory(IntPtr device, UInt64 memory);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static unsafe MgResult vkFlushMappedMemoryRanges(IntPtr device, UInt32 memoryRangeCount, VkMappedMemoryRange* pMemoryRanges);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static unsafe MgResult vkInvalidateMappedMemoryRanges(IntPtr device, UInt32 memoryRangeCount, VkMappedMemoryRange* pMemoryRanges);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static void vkGetDeviceMemoryCommitment(IntPtr device, UInt64 memory, ref UInt64 pCommittedMemoryInBytes);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static unsafe void vkGetBufferMemoryRequirements(IntPtr device, UInt64 buffer, Magnesium.MgMemoryRequirements* pMemoryRequirements);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkBindBufferMemory(IntPtr device, UInt64 buffer, UInt64 memory, UInt64 memoryOffset);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static unsafe void vkGetImageMemoryRequirements(IntPtr device, UInt64 image, Magnesium.MgMemoryRequirements* pMemoryRequirements);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkBindImageMemory(IntPtr device, UInt64 image, UInt64 memory, UInt64 memoryOffset);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static unsafe void vkGetImageSparseMemoryRequirements(IntPtr device, UInt64 image, UInt32* pSparseMemoryRequirementCount, Magnesium.MgSparseImageMemoryRequirements* pSparseMemoryRequirements);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceSparseImageFormatProperties(IntPtr physicalDevice, MgFormat format, VkImageType type, MgSampleCountFlagBits samples, MgImageUsageFlagBits usage, MgImageTiling tiling, ref UInt32 pPropertyCount, [In, Out] VkSparseImageFormatProperties[] pProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkQueueBindSparse(IntPtr queue, UInt32 bindInfoCount, VkBindSparseInfo* pBindInfo, UInt64 fence);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateFence(IntPtr device, ref VkFenceCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pFence);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyFence(IntPtr device, UInt64 fence, IntPtr pAllocator);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkResetFences(IntPtr device, UInt32 fenceCount, [In] UInt64[] pFences);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkGetFenceStatus(IntPtr device, UInt64 fence);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkWaitForFences(IntPtr device, UInt32 fenceCount, [In] UInt64[] pFences, VkBool32 waitAll, UInt64 timeout);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateSemaphore(IntPtr device, ref VkSemaphoreCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pSemaphore);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroySemaphore(IntPtr device, UInt64 semaphore, IntPtr pAllocator);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateEvent(IntPtr device, ref VkEventCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pEvent);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyEvent(IntPtr device, UInt64 @event, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetEventStatus(IntPtr device, UInt64 @event);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkSetEvent(IntPtr device, UInt64 @event);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkResetEvent(IntPtr device, UInt64 @event);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateQueryPool(IntPtr device, ref VkQueryPoolCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pQueryPool);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyQueryPool(IntPtr device, UInt64 queryPool, IntPtr pAllocator);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkGetQueryPoolResults(IntPtr device, UInt64 queryPool, UInt32 firstQuery, UInt32 queryCount, IntPtr dataSize, IntPtr pData, UInt64 stride, VkQueryResultFlags flags);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateBuffer(IntPtr device, ref VkBufferCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pBuffer);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyBuffer(IntPtr device, UInt64 buffer, IntPtr pAllocator);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateBufferView(IntPtr device, ref VkBufferViewCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pView);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyBufferView(IntPtr device, UInt64 bufferView, IntPtr pAllocator);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateImage(IntPtr device, ref VkImageCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pImage);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyImage(IntPtr device, UInt64 image, IntPtr pAllocator);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static void vkGetImageSubresourceLayout(IntPtr device, UInt64 image, [In] Magnesium.MgImageSubresource pSubresource, [In, Out] Magnesium.MgSubresourceLayout pLayout);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateImageView(IntPtr device, ref VkImageViewCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pView);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyImageView(IntPtr device, UInt64 imageView, IntPtr pAllocator);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateShaderModule(IntPtr device, ref VkShaderModuleCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pShaderModule);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyShaderModule(IntPtr device, UInt64 shaderModule, IntPtr pAllocator);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreatePipelineCache(IntPtr device, ref VkPipelineCacheCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pPipelineCache);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyPipelineCache(IntPtr device, UInt64 pipelineCache, IntPtr pAllocator);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkGetPipelineCacheData(IntPtr device, UInt64 pipelineCache, ref UIntPtr pDataSize, IntPtr pData);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkMergePipelineCaches(IntPtr device, UInt64 dstCache, UInt32 srcCacheCount, [In] UInt64[] pSrcCaches);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateGraphicsPipelines(IntPtr device, UInt64 pipelineCache, UInt32 createInfoCount, [In, Out] VkGraphicsPipelineCreateInfo[] pCreateInfos, IntPtr pAllocator, [In, Out] UInt64[] pPipelines);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateComputePipelines(IntPtr device, UInt64 pipelineCache, UInt32 createInfoCount, [In, Out] VkComputePipelineCreateInfo[] pCreateInfos, IntPtr pAllocator, [In, Out] UInt64[] pPipelines);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyPipeline(IntPtr device, UInt64 pipeline, IntPtr pAllocator);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreatePipelineLayout(IntPtr device, ref VkPipelineLayoutCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pPipelineLayout);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyPipelineLayout(IntPtr device, UInt64 pipelineLayout, IntPtr pAllocator);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateSampler(IntPtr device, ref VkSamplerCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pSampler);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroySampler(IntPtr device, UInt64 sampler, IntPtr pAllocator);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateDescriptorSetLayout(IntPtr device, ref VkDescriptorSetLayoutCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pSetLayout);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyDescriptorSetLayout(IntPtr device, UInt64 descriptorSetLayout, IntPtr pAllocator);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateDescriptorPool(IntPtr device, ref VkDescriptorPoolCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pDescriptorPool);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyDescriptorPool(IntPtr device, UInt64 descriptorPool, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkResetDescriptorPool(IntPtr device, UInt64 descriptorPool, UInt32 flags);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkAllocateDescriptorSets(IntPtr device, ref VkDescriptorSetAllocateInfo pAllocateInfo, [In, Out] UInt64[] pDescriptorSets);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkFreeDescriptorSets(IntPtr device, UInt64 descriptorPool, UInt32 descriptorSetCount, [In, Out] UInt64[] pDescriptorSets);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static unsafe void vkUpdateDescriptorSets(IntPtr device, UInt32 descriptorWriteCount, VkWriteDescriptorSet* pDescriptorWrites, UInt32 descriptorCopyCount, VkCopyDescriptorSet* pDescriptorCopies);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateFramebuffer(IntPtr device, ref VkFramebufferCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pFramebuffer);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyFramebuffer(IntPtr device, UInt64 framebuffer, IntPtr pAllocator);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateRenderPass(IntPtr device, ref VkRenderPassCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pRenderPass);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyRenderPass(IntPtr device, UInt64 renderPass, IntPtr pAllocator);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static unsafe void vkGetRenderAreaGranularity(IntPtr device, UInt64 renderPass, MgExtent2D* pGranularity);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateCommandPool(IntPtr device, ref VkCommandPoolCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pCommandPool);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyCommandPool(IntPtr device, UInt64 commandPool, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkResetCommandPool(IntPtr device, UInt64 commandPool, VkCommandPoolResetFlags flags);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static unsafe MgResult vkAllocateCommandBuffers(IntPtr device, VkCommandBufferAllocateInfo* pAllocateInfo, IntPtr* pCommandBuffers);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static void vkFreeCommandBuffers(IntPtr device, UInt64 commandPool, UInt32 commandBufferCount, [In] IntPtr[] pCommandBuffers);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkBeginCommandBuffer(IntPtr commandBuffer, ref VkCommandBufferBeginInfo pBeginInfo);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkEndCommandBuffer(IntPtr commandBuffer);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkResetCommandBuffer(IntPtr commandBuffer, VkCommandBufferResetFlags flags);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBindPipeline(IntPtr commandBuffer, MgPipelineBindPoint pipelineBindPoint, UInt64 pipeline);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdSetViewport(IntPtr commandBuffer, UInt32 firstViewport, UInt32 viewportCount, Magnesium.MgViewport* pViewports);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdSetScissor(IntPtr commandBuffer, UInt32 firstScissor, UInt32 scissorCount, Magnesium.MgRect2D* pScissors);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdSetLineWidth(IntPtr commandBuffer, float lineWidth);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdSetDepthBias(IntPtr commandBuffer, float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdSetBlendConstants(IntPtr commandBuffer, [MarshalAs(UnmanagedType.LPArray, SizeConst = 4)] float[] blendConstants);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdSetDepthBounds(IntPtr commandBuffer, float minDepthBounds, float maxDepthBounds);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdSetStencilCompareMask(IntPtr commandBuffer, VkStencilFaceFlags faceMask, UInt32 compareMask);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdSetStencilWriteMask(IntPtr commandBuffer, VkStencilFaceFlags faceMask, UInt32 writeMask);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdSetStencilReference(IntPtr commandBuffer, VkStencilFaceFlags faceMask, UInt32 reference);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBindDescriptorSets(IntPtr commandBuffer, MgPipelineBindPoint pipelineBindPoint, UInt64 layout, UInt32 firstSet, UInt32 descriptorSetCount, [In] UInt64[] pDescriptorSets, UInt32 dynamicOffsetCount, [In] UInt32[] pDynamicOffsets);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBindIndexBuffer(IntPtr commandBuffer, UInt64 buffer, UInt64 offset, MgIndexType indexType);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBindVertexBuffers(IntPtr commandBuffer, UInt32 firstBinding, UInt32 bindingCount, [In] UInt64[] pBuffers, [In] UInt64[] pOffsets);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdDraw(IntPtr commandBuffer, UInt32 vertexCount, UInt32 instanceCount, UInt32 firstVertex, UInt32 firstInstance);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdDrawIndexed(IntPtr commandBuffer, UInt32 indexCount, UInt32 instanceCount, UInt32 firstIndex, Int32 vertexOffset, UInt32 firstInstance);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdDrawIndirect(IntPtr commandBuffer, UInt64 buffer, UInt64 offset, UInt32 drawCount, UInt32 stride);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdDrawIndexedIndirect(IntPtr commandBuffer, UInt64 buffer, UInt64 offset, UInt32 drawCount, UInt32 stride);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdDispatch(IntPtr commandBuffer, UInt32 x, UInt32 y, UInt32 z);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdDispatchIndirect(IntPtr commandBuffer, UInt64 buffer, UInt64 offset);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdCopyBuffer(IntPtr commandBuffer, UInt64 srcBuffer, UInt64 dstBuffer, UInt32 regionCount, Magnesium.MgBufferCopy* pRegions);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdCopyImage(IntPtr commandBuffer, UInt64 srcImage, MgImageLayout srcImageLayout, UInt64 dstImage, MgImageLayout dstImageLayout, UInt32 regionCount, Magnesium.MgImageCopy* pRegions);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBlitImage(IntPtr commandBuffer, UInt64 srcImage, MgImageLayout srcImageLayout, UInt64 dstImage, MgImageLayout dstImageLayout, UInt32 regionCount, [In, Out] MgImageBlit[] pRegions, VkFilter filter);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdCopyBufferToImage(IntPtr commandBuffer, UInt64 srcBuffer, UInt64 dstImage, MgImageLayout dstImageLayout, UInt32 regionCount, MgBufferImageCopy* pRegions);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdCopyImageToBuffer(IntPtr commandBuffer, UInt64 srcImage, MgImageLayout srcImageLayout, UInt64 dstBuffer, UInt32 regionCount, MgBufferImageCopy* pRegions);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdUpdateBuffer(IntPtr commandBuffer, UInt64 dstBuffer, UInt64 dstOffset, UInt64 dataSize, IntPtr pData);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdFillBuffer(IntPtr commandBuffer, UInt64 dstBuffer, UInt64 dstOffset, UInt64 size, UInt32 data);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdClearColorImage(IntPtr commandBuffer, UInt64 image, MgImageLayout imageLayout, [In] MgClearColorValue pColor, UInt32 rangeCount, [In] Magnesium.MgImageSubresourceRange[] pRanges);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdClearDepthStencilImage(IntPtr commandBuffer, UInt64 image, MgImageLayout imageLayout, [In] MgClearDepthStencilValue pDepthStencil, UInt32 rangeCount, [In] Magnesium.MgImageSubresourceRange[] pRanges);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdClearAttachments(IntPtr commandBuffer, UInt32 attachmentCount, Magnesium.MgClearAttachment* pAttachments, UInt32 rectCount, Magnesium.MgClearRect* pRects);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdResolveImage(IntPtr commandBuffer, UInt64 srcImage, MgImageLayout srcImageLayout, UInt64 dstImage, MgImageLayout dstImageLayout, UInt32 regionCount, MgImageResolve* pRegions);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdSetEvent(IntPtr commandBuffer, UInt64 @event, MgPipelineStageFlagBits stageMask);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdResetEvent(IntPtr commandBuffer, UInt64 @event, MgPipelineStageFlagBits stageMask);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdWaitEvents(IntPtr commandBuffer, UInt32 eventCount, UInt64* pEvents, MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, UInt32 memoryBarrierCount, VkMemoryBarrier* pMemoryBarriers, UInt32 bufferMemoryBarrierCount, VkBufferMemoryBarrier* pBufferMemoryBarriers, UInt32 imageMemoryBarrierCount, VkImageMemoryBarrier* pImageMemoryBarriers);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdPipelineBarrier(IntPtr commandBuffer, MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgDependencyFlagBits dependencyFlags, UInt32 memoryBarrierCount, VkMemoryBarrier* pMemoryBarriers, UInt32 bufferMemoryBarrierCount, VkBufferMemoryBarrier* pBufferMemoryBarriers, UInt32 imageMemoryBarrierCount, VkImageMemoryBarrier* pImageMemoryBarriers);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBeginQuery(IntPtr commandBuffer, UInt64 queryPool, UInt32 query, VkQueryControlFlags flags);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdEndQuery(IntPtr commandBuffer, UInt64 queryPool, UInt32 query);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdResetQueryPool(IntPtr commandBuffer, UInt64 queryPool, UInt32 firstQuery, UInt32 queryCount);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdWriteTimestamp(IntPtr commandBuffer, MgPipelineStageFlagBits pipelineStage, UInt64 queryPool, UInt32 query);

        [DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdCopyQueryPoolResults(IntPtr commandBuffer, UInt64 queryPool, UInt32 firstQuery, UInt32 queryCount, UInt64 dstBuffer, UInt64 dstOffset, UInt64 stride, MgQueryResultFlagBits flags);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdPushConstants(IntPtr commandBuffer, UInt64 layout, VkShaderStageFlags stageFlags, UInt32 offset, UInt32 size, IntPtr pValues);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBeginRenderPass(IntPtr commandBuffer, ref VkRenderPassBeginInfo pRenderPassBegin, VkSubpassContents contents);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdNextSubpass(IntPtr commandBuffer, VkSubpassContents contents);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdEndRenderPass(IntPtr commandBuffer);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdExecuteCommands(IntPtr commandBuffer, UInt32 commandBufferCount, [In] IntPtr[] pCommandBuffers);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkCreateAndroidSurfaceKHR(IntPtr instance, ref VkAndroidSurfaceCreateInfoKHR pCreateInfo, IntPtr pAllocator, ref UInt64 pSurface);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetPhysicalDeviceDisplayPropertiesKHR(IntPtr physicalDevice, ref UInt32 pPropertyCount, [In, Out] VkDisplayPropertiesKHR[] pProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetPhysicalDeviceDisplayPlanePropertiesKHR(IntPtr physicalDevice, ref UInt32 pPropertyCount, [In, Out] VkDisplayPlanePropertiesKHR[] pProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetDisplayPlaneSupportedDisplaysKHR(IntPtr physicalDevice, UInt32 planeIndex, ref UInt32 pDisplayCount, [In, Out] UInt64[] pDisplays);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetDisplayModePropertiesKHR(IntPtr physicalDevice, UInt64 display, ref UInt32 pPropertyCount, [In, Out] VkDisplayModePropertiesKHR[] pProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkCreateDisplayModeKHR(IntPtr physicalDevice, UInt64 display, ref VkDisplayModeCreateInfoKHR pCreateInfo, IntPtr pAllocator, ref UInt64 pMode);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetDisplayPlaneCapabilitiesKHR(IntPtr physicalDevice, UInt64 mode, UInt32 planeIndex, ref VkDisplayPlaneCapabilitiesKHR pCapabilities);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkCreateDisplayPlaneSurfaceKHR(IntPtr instance, ref VkDisplaySurfaceCreateInfoKHR pCreateInfo, IntPtr pAllocator, ref UInt64 pSurface);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateSharedSwapchainsKHR(IntPtr device, UInt32 swapchainCount, [In] VkSwapchainCreateInfoKHR[] pCreateInfos, IntPtr pAllocator, [In, Out] UInt64[] pSwapchains);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroySurfaceKHR(IntPtr instance, UInt64 surface, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetPhysicalDeviceSurfaceSupportKHR(IntPtr physicalDevice, UInt32 queueFamilyIndex, UInt64 surface, ref VkBool32 pSupported);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetPhysicalDeviceSurfaceCapabilitiesKHR(IntPtr physicalDevice, UInt64 surface, ref VkSurfaceCapabilitiesKHR pSurfaceCapabilities);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetPhysicalDeviceSurfaceFormatsKHR(IntPtr physicalDevice, UInt64 surface, ref UInt32 pSurfaceFormatCount, [In, Out] VkSurfaceFormatKHR[] pSurfaceFormats);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetPhysicalDeviceSurfacePresentModesKHR(IntPtr physicalDevice, UInt64 surface, ref UInt32 pPresentModeCount, [In, Out] VkPresentModeKhr[] pPresentModes);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateSwapchainKHR(IntPtr device, ref VkSwapchainCreateInfoKHR pCreateInfo, IntPtr pAllocator, ref UInt64 pSwapchain);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroySwapchainKHR(IntPtr device, UInt64 swapchain, IntPtr pAllocator);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkGetSwapchainImagesKHR(IntPtr device, UInt64 swapchain, ref UInt32 pSwapchainImageCount, [In, Out] UInt64[] pSwapchainImages);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkAcquireNextImageKHR(IntPtr device, UInt64 swapchain, UInt64 timeout, UInt64 semaphore, UInt64 fence, ref UInt32 pImageIndex);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkQueuePresentKHR(IntPtr queue, ref VkPresentInfoKHR pPresentInfo);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateViSurfaceNN(IntPtr instance, [In, Out] VkViSurfaceCreateInfoNN pCreateInfo, IntPtr pAllocator, ref UInt64 pSurface);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateWaylandSurfaceKHR(IntPtr instance, ref VkWaylandSurfaceCreateInfoKHR pCreateInfo, IntPtr pAllocator, ref UInt64 pSurface);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static VkBool32 vkGetPhysicalDeviceWaylandPresentationSupportKHR(IntPtr physicalDevice, UInt32 queueFamilyIndex, ref wl_display display);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkCreateWin32SurfaceKHR(IntPtr instance, ref VkWin32SurfaceCreateInfoKHR pCreateInfo, IntPtr pAllocator, ref UInt64 pSurface);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkBool32 vkGetPhysicalDeviceWin32PresentationSupportKHR(IntPtr physicalDevice, UInt32 queueFamilyIndex);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateXlibSurfaceKHR(IntPtr instance, ref VkXlibSurfaceCreateInfoKHR pCreateInfo, IntPtr pAllocator, ref UInt64 pSurface);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static VkBool32 vkGetPhysicalDeviceXlibPresentationSupportKHR(IntPtr physicalDevice, UInt32 queueFamilyIndex, ref Display dpy, VisualID visualID);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateXcbSurfaceKHR(IntPtr instance, ref VkXcbSurfaceCreateInfoKHR pCreateInfo, IntPtr pAllocator, ref UInt64 pSurface);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkBool32 vkGetPhysicalDeviceXcbPresentationSupportKHR(IntPtr physicalDevice, UInt32 queueFamilyIndex, ref IntPtr connection, IntPtr visual_id);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateImagePipeSurfaceFUCHSIA(IntPtr instance, ref VkImagePipeSurfaceCreateInfoFUCHSIA pCreateInfo, IntPtr pAllocator, ref UInt64 pSurface);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkCreateDebugReportCallbackEXT(IntPtr instance, ref VkDebugReportCallbackCreateInfoEXT pCreateInfo, IntPtr pAllocator, ref UInt64 pCallback);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyDebugReportCallbackEXT(IntPtr instance, UInt64 callback, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDebugReportMessageEXT(IntPtr instance, VkDebugReportFlagsExt flags, VkDebugReportObjectTypeExt objectType, UInt64 @object, UIntPtr location, Int32 messageCode, string pLayerPrefix, string pMessage);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkDebugMarkerSetObjectNameEXT(IntPtr device, [In, Out] ref VkDebugMarkerObjectNameInfoExt pNameInfo);
        /**
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkDebugMarkerSetObjectTagEXT(IntPtr device, VkDebugMarkerObjectTagInfoEXT* pTagInfo);
        **/
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdDebugMarkerBeginEXT(IntPtr commandBuffer, ref VkDebugMarkerMarkerInfoEXT pMarkerInfo);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdDebugMarkerEndEXT(IntPtr commandBuffer);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdDebugMarkerInsertEXT(IntPtr commandBuffer, ref VkDebugMarkerMarkerInfoEXT pMarkerInfo);

        [DllImport(VULKAN_LIB, CallingConvention = CallingConvention.Winapi)]
        internal extern static void vkCmdBeginConditionalRenderingEXT(IntPtr commandBuffer, ref VkConditionalRenderingBeginInfoEXT pConditionalRenderingBegin);

        [DllImport(VULKAN_LIB, CallingConvention = CallingConvention.Winapi)]
        internal extern static void vkCmdEndConditionalRenderingEXT(IntPtr commandBuffer);

        [DllImport(VULKAN_LIB, CallingConvention = CallingConvention.Winapi)]
        internal extern static MgResult vkEnumerateInstanceVersion(ref UInt32 pApiVersion);

        [DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetPhysicalDeviceExternalImageFormatPropertiesNV(IntPtr physicalDevice, MgFormat format, VkImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, UInt32 externalHandleType, ref VkExternalImageFormatPropertiesNV pExternalImageFormatProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetMemoryWin32HandleNV(IntPtr device, UInt64 memory, UInt32 handleType, ref IntPtr pHandle);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdDrawIndirectCountAMD(IntPtr commandBuffer, UInt64 buffer, UInt64 offset, UInt64 countBuffer, UInt64 countBufferOffset, UInt32 maxDrawCount, UInt32 stride);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdDrawIndexedIndirectCountAMD(IntPtr commandBuffer, UInt64 buffer, UInt64 offset, UInt64 countBuffer, UInt64 countBufferOffset, UInt32 maxDrawCount, UInt32 stride);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdProcessCommandsNVX(IntPtr commandBuffer, VkCmdProcessCommandsInfoNVX pProcessCommandsInfo);

        /***
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdReserveSpaceForCommandsNVX(IntPtr commandBuffer, VkCmdReserveSpaceForCommandsInfoNVX pReserveSpaceInfo);
        ***/
		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateIndirectCommandsLayoutNVX(IntPtr device, ref VkIndirectCommandsLayoutCreateInfoNVX pCreateInfo, IntPtr pAllocator, ref UInt64 pIndirectCommandsLayout);
       
	//	[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
///		internal extern static void vkDestroyIndirectCommandsLayoutNVX(IntPtr device, UInt64 indirectCommandsLayout, IntPtr pAllocator);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateObjectTableNVX(IntPtr device, ref VkObjectTableCreateInfoNVX pCreateInfo, IntPtr pAllocator, ref UInt64 pObjectTable);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyObjectTableNVX(IntPtr device, UInt64 objectTable, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkRegisterObjectsNVX(IntPtr device, UInt64 objectTable, UInt32 objectCount, ref MgObjectTableEntryNVX[] ppObjectTableEntries, UInt32[] pObjectIndices);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkUnregisterObjectsNVX(IntPtr device, UInt64 objectTable, UInt32 objectCount, ref MgObjectTableEntryNVX[] pObjectEntryTypes, UInt32[] pObjectIndices);
        
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceGeneratedCommandsPropertiesNVX(IntPtr physicalDevice, ref VkDeviceGeneratedCommandsFeaturesNVX pFeatures, ref VkDeviceGeneratedCommandsLimitsNVX pLimits);
        
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceFeatures2(IntPtr physicalDevice, ref VkPhysicalDeviceFeatures2 pFeatures);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceProperties2(IntPtr physicalDevice, ref VkPhysicalDeviceProperties2 pProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceFormatProperties2(IntPtr physicalDevice, MgFormat format, ref VkFormatProperties2 pFormatProperties);
        
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetPhysicalDeviceImageFormatProperties2(IntPtr physicalDevice, ref VkPhysicalDeviceImageFormatInfo2 pImageFormatInfo, ref VkImageFormatProperties2 pImageFormatProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceQueueFamilyProperties2(IntPtr physicalDevice, ref UInt32 pQueueFamilyPropertyCount, [In, Out] VkQueueFamilyProperties2[] pQueueFamilyProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceMemoryProperties2(IntPtr physicalDevice, ref VkPhysicalDeviceMemoryProperties2 pMemoryProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceSparseImageFormatProperties2(IntPtr physicalDevice, ref VkPhysicalDeviceSparseImageFormatInfo2 pFormatInfo, ref UInt32 pPropertyCount, [In, Out] VkSparseImageFormatProperties2[] pProperties);
        /**
        [DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdPushDescriptorSetKHR(IntPtr commandBuffer, MgPipelineBindPoint pipelineBindPoint, UInt64 layout, UInt32 set, UInt32 descriptorWriteCount, [In, Out] VkWriteDescriptorSet[] pDescriptorWrites);

        [DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkTrimCommandPool(IntPtr device, UInt64 commandPool, VkCommandPoolTrimFlags flags);
        **/
        [DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceExternalBufferProperties(IntPtr physicalDevice, ref VkPhysicalDeviceExternalBufferInfo pExternalBufferInfo, ref VkExternalBufferProperties pExternalBufferProperties);
        /**
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetMemoryWin32HandleKHR(IntPtr device, VkMemoryGetWin32HandleInfoKHR pGetWin32HandleInfo, ref IntPtr pHandle);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetMemoryWin32HandlePropertiesKHR(IntPtr device, VkExternalMemoryHandleTypeFlags handleType, IntPtr handle, VkMemoryWin32HandlePropertiesKHR pMemoryWin32HandleProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetMemoryFdKHR(IntPtr device, VkMemoryGetFdInfoKHR pGetFdInfo, ref int pFd);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetMemoryFdPropertiesKHR(IntPtr device, VkExternalMemoryHandleTypeFlags handleType, int fd, VkMemoryFdPropertiesKHR pMemoryFdProperties);
        **/
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceExternalSemaphoreProperties(IntPtr physicalDevice, ref VkPhysicalDeviceExternalSemaphoreInfo pExternalSemaphoreInfo, ref VkExternalSemaphoreProperties pExternalSemaphoreProperties);
        /**
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetSemaphoreWin32HandleKHR(IntPtr device, VkSemaphoreGetWin32HandleInfoKHR pGetWin32HandleInfo, ref IntPtr pHandle);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkImportSemaphoreWin32HandleKHR(IntPtr device, [In, Out] VkImportSemaphoreWin32HandleInfoKHR pImportSemaphoreWin32HandleInfo);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetSemaphoreFdKHR(IntPtr device, VkSemaphoreGetFdInfoKHR pGetFdInfo, ref int pFd);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkImportSemaphoreFdKHR(IntPtr device, [In, Out] VkImportSemaphoreFdInfoKHR pImportSemaphoreFdInfo);
        **/
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceExternalFenceProperties(IntPtr physicalDevice, ref VkPhysicalDeviceExternalFenceInfo pExternalFenceInfo, ref VkExternalFenceProperties pExternalFenceProperties);
        /**
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetFenceWin32HandleKHR(IntPtr device, VkFenceGetWin32HandleInfoKHR pGetWin32HandleInfo, ref IntPtr pHandle);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkImportFenceWin32HandleKHR(IntPtr device, [In, Out] VkImportFenceWin32HandleInfoKHR pImportFenceWin32HandleInfo);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetFenceFdKHR(IntPtr device, VkFenceGetFdInfoKHR pGetFdInfo, ref int pFd);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkImportFenceFdKHR(IntPtr device, [In, Out] VkImportFenceFdInfoKHR pImportFenceFdInfo);
        **/
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkReleaseDisplayEXT(IntPtr physicalDevice, UInt64 display);
        /**
		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkAcquireXlibDisplayEXT(IntPtr physicalDevice, ref Display dpy, UInt64 display);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkGetRandROutputDisplayEXT(IntPtr physicalDevice, ref Display dpy, RROutput rrOutput, ref UInt64 pDisplay);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkDisplayPowerControlEXT(IntPtr device, UInt64 display, VkDisplayPowerInfoExt pDisplayPowerInfo);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkRegisterDeviceEventEXT(IntPtr device, VkDeviceEventInfoExt pDeviceEventInfo, IntPtr pAllocator, UInt64* pFence);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkRegisterDisplayEventEXT(IntPtr device, UInt64 display, VkDisplayEventInfoExt pDisplayEventInfo, IntPtr pAllocator, UInt64* pFence);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetSwapchainCounterEXT(IntPtr device, UInt64 swapchain, VkSurfaceCounterFlagsExt counter, ref UInt64 pCounterValue);
        **/
        [DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkGetPhysicalDeviceSurfaceCapabilities2EXT(IntPtr physicalDevice, UInt64 surface, ref VkSurfaceCapabilities2EXT pSurfaceCapabilities);

        [DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkEnumeratePhysicalDeviceGroups(IntPtr instance, ref UInt32 pPhysicalDeviceGroupCount, [In, Out] VkPhysicalDeviceGroupProperties[] pPhysicalDeviceGroupProperties);
        /**
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetDeviceGroupPeerMemoryFeatures(IntPtr device, UInt32 heapIndex, UInt32 localDeviceIndex, UInt32 remoteDeviceIndex, ref VkPeerMemoryFeatureFlags pPeerMemoryFeatures);
        **/
		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkBindBufferMemory2(IntPtr device, UInt32 bindInfoCount, [In] VkBindBufferMemoryInfo[] pBindInfos);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkBindImageMemory2(IntPtr device, UInt32 bindInfoCount, [In] VkBindImageMemoryInfo[] pBindInfos);
        /**
[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
internal extern static void vkCmdSetDeviceMask(IntPtr commandBuffer, UInt32 deviceMask);

[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
internal extern static MgResult vkGetDeviceGroupPresentCapabilitiesKHR(IntPtr device, [In, Out] VkDeviceGroupPresentCapabilitiesKHR pDeviceGroupPresentCapabilities);

[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
internal extern static MgResult vkGetDeviceGroupSurfacePresentModesKHR(IntPtr device, UInt64 surface, ref VkDeviceGroupPresentModeFlagsKHR pModes);
***/
  //      [DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkAcquireNextImage2KHR(IntPtr device, [In, Out] VkAcquireNextImageInfoKHR pAcquireInfo, ref UInt32 pImageIndex);
        /***
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdDispatchBase(IntPtr commandBuffer, UInt32 baseGroupX, UInt32 baseGroupY, UInt32 baseGroupZ, UInt32 groupCountX, UInt32 groupCountY, UInt32 groupCountZ);
        ***/
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkGetPhysicalDevicePresentRectanglesKHR(IntPtr physicalDevice, UInt64 surface, UInt32 pRectCount, MgRect2D* pRects);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateDescriptorUpdateTemplate(IntPtr device, [In, Out] VkDescriptorUpdateTemplateCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pDescriptorUpdateTemplate);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyDescriptorUpdateTemplate(IntPtr device, UInt64 descriptorUpdateTemplate, IntPtr pAllocator);
        /**
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkUpdateDescriptorSetWithTemplate(IntPtr device, UInt64 descriptorSet, UInt64 descriptorUpdateTemplate, IntPtr pData);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdPushDescriptorSetWithTemplateKHR(IntPtr commandBuffer, UInt64 descriptorUpdateTemplate, UInt64 layout, UInt32 set, IntPtr pData);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkSetHdrMetadataEXT(IntPtr device, UInt32 swapchainCount, UInt64* pSwapchains, VkHdrMetadataExt* pMetadata);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetSwapchainStatusKHR(IntPtr device, UInt64 swapchain);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static unsafe MgResult vkGetRefreshCycleDurationGOOGLE(IntPtr device, UInt64 swapchain, VkRefreshCycleDurationGOOGLE pDisplayTimingProperties);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static unsafe MgResult vkGetPastPresentationTimingGOOGLE(IntPtr device, UInt64 swapchain, UInt32* pPresentationTimingCount, VkPastPresentationTimingGOOGLE* pPresentationTimings);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateIOSSurfaceMVK(IntPtr instance, [In, Out] VkIOSSurfaceCreateInfoMVK pCreateInfo, IntPtr pAllocator, ref UInt64 pSurface);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateMacOSSurfaceMVK(IntPtr instance, [In, Out] VkMacOSSurfaceCreateInfoMVK pCreateInfo, IntPtr pAllocator, ref UInt64 pSurface);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdSetViewportWScalingNV(IntPtr commandBuffer, UInt32 firstViewport, UInt32 viewportCount, VkViewportWScalingNV* pViewportWScalings);
        **/
        [DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdSetDiscardRectangleEXT(IntPtr commandBuffer, UInt32 firstDiscardRectangle, UInt32 discardRectangleCount, MgRect2D* pDiscardRectangles);
        /**
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdSetSampleLocationsEXT(IntPtr commandBuffer, VkSampleLocationsInfoExt pSampleLocationsInfo);
        **/
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceMultisamplePropertiesEXT(IntPtr physicalDevice, MgSampleCountFlagBits samples, ref VkMultisamplePropertiesEXT pMultisampleProperties);
        
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetPhysicalDeviceSurfaceCapabilities2KHR(IntPtr physicalDevice, ref VkPhysicalDeviceSurfaceInfo2KHR pSurfaceInfo, ref VkSurfaceCapabilities2KHR pSurfaceCapabilities);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetPhysicalDeviceSurfaceFormats2KHR(IntPtr physicalDevice, ref VkPhysicalDeviceSurfaceInfo2KHR pSurfaceInfo, ref UInt32 pSurfaceFormatCount, [In, Out] VkSurfaceFormat2KHR[] pSurfaceFormats);

        [DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetPhysicalDeviceDisplayProperties2KHR(IntPtr physicalDevice, ref UInt32 pPropertyCount, [In, Out] VkDisplayProperties2KHR[] pProperties);

        [DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetPhysicalDeviceDisplayPlaneProperties2KHR(IntPtr physicalDevice, ref UInt32 pPropertyCount, [In, Out] VkDisplayPlaneProperties2KHR[] pProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetDisplayModeProperties2KHR(IntPtr physicalDevice, UInt64 display, ref UInt32 pPropertyCount, [In, Out] VkDisplayModeProperties2KHR[] pProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetDisplayPlaneCapabilities2KHR(IntPtr physicalDevice, ref VkDisplayPlaneInfo2KHR pDisplayPlaneInfo, ref VkDisplayPlaneCapabilities2KHR pCapabilities);
        /**
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetBufferMemoryRequirements2(IntPtr device, VkBufferMemoryRequirementsInfo2 pInfo, [In, Out] VkMemoryRequirements2 pMemoryRequirements);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetImageMemoryRequirements2(IntPtr device, VkImageMemoryRequirementsInfo2 pInfo, [In, Out] VkMemoryRequirements2 pMemoryRequirements);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkGetImageSparseMemoryRequirements2(IntPtr device, VkImageSparseMemoryRequirementsInfo2 pInfo, UInt32* pSparseMemoryRequirementCount, VkSparseImageMemoryRequirements2* pSparseMemoryRequirements);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkCreateSamplerYcbcrConversion(IntPtr device, VkSamplerYcbcrConversionCreateInfo pCreateInfo, IntPtr pAllocator, UInt64* pYcbcrConversion);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroySamplerYcbcrConversion(IntPtr device, UInt64 ycbcrConversion, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetDeviceQueue2(IntPtr device, [In, Out] VkDeviceQueueInfo2 pQueueInfo, ref IntPtr pQueue);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkCreateValidationCacheEXT(IntPtr device, [In, Out] VkValidationCacheCreateInfoExt pCreateInfo, IntPtr pAllocator, ref UInt64 pValidationCache);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyValidationCacheEXT(IntPtr device, UInt64 validationCache, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetValidationCacheDataEXT(IntPtr device, UInt64 validationCache, ref UIntPtr pDataSize, IntPtr[] pData);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkMergeValidationCachesEXT(IntPtr device, UInt64 dstCache, UInt32 srcCacheCount, UInt64[] pSrcCaches);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetDescriptorSetLayoutSupport(IntPtr device, [In, Out] VkDescriptorSetLayoutCreateInfo pCreateInfo, VkDescriptorSetLayoutSupport pSupport);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetSwapchainGrallocUsageANDROID(IntPtr device, MgFormat format, VkImageUsageFlags imageUsage, ref int grallocUsage);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkAcquireImageANDROID(IntPtr device, UInt64 image, int nativeFenceFd, UInt64 semaphore, UInt64 fence);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkQueueSignalReleaseImageANDROID(IntPtr queue, UInt32 waitSemaphoreCount, UInt64 pWaitSemaphores, UInt64 image, ref int pNativeFenceFd);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetShaderInfoAMD(IntPtr device, UInt64 pipeline, VkShaderStageFlags shaderStage, VkShaderInfoTypeAMD infoType, ref UIntPtr pInfoSize, IntPtr[] pInfo);
        **/
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetPhysicalDeviceCalibrateableTimeDomainsEXT(IntPtr physicalDevice, ref UInt32 pTimeDomainCount, [In, Out] MgTimeDomainEXT[] pTimeDomains);
        /**
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkGetCalibratedTimestampsEXT(IntPtr device, UInt32 timestampCount, VkCalibratedTimestampInfoExt* pTimestampInfos, UInt64* pTimestamps, UInt64* pMaxDeviation);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkSetDebugUtilsObjectNameEXT(IntPtr device, [In, Out] VkDebugUtilsObjectNameInfoExt pNameInfo);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkSetDebugUtilsObjectTagEXT(IntPtr device, VkDebugUtilsObjectTagInfoExt pTagInfo);
        **/
        [DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkQueueBeginDebugUtilsLabelEXT(IntPtr queue, ref VkDebugUtilsLabelEXT pLabelInfo);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkQueueEndDebugUtilsLabelEXT(IntPtr queue);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkQueueInsertDebugUtilsLabelEXT(IntPtr queue, ref VkDebugUtilsLabelEXT pLabelInfo);
        /**
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBeginDebugUtilsLabelEXT(IntPtr commandBuffer, [In, Out] VkDebugUtilsLabelExt pLabelInfo);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdEndDebugUtilsLabelEXT(IntPtr commandBuffer);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdInsertDebugUtilsLabelEXT(IntPtr commandBuffer, [In, Out] VkDebugUtilsLabelExt pLabelInfo);
        ***/
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkCreateDebugUtilsMessengerEXT(IntPtr instance, ref VkDebugUtilsMessengerCreateInfoEXT pCreateInfo, IntPtr pAllocator, ref UInt64 pMessenger);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyDebugUtilsMessengerEXT(IntPtr instance, UInt64 messenger, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkSubmitDebugUtilsMessageEXT(IntPtr instance, MgDebugUtilsMessageSeverityFlagBitsEXT messageSeverity, MgDebugUtilsMessageTypeFlagBitsEXT messageTypes, [In, Out] VkDebugUtilsMessengerCallbackDataEXT pCallbackData);
        /***
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkGetMemoryHostPointerPropertiesEXT(IntPtr device, VkExternalMemoryHandleTypeFlags handleType, IntPtr pHostPointer, VkMemoryHostPointerPropertiesExt* pMemoryHostPointerProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdWriteBufferMarkerAMD(IntPtr commandBuffer, VkPipelineStageFlags pipelineStage, UInt64 dstBuffer, UInt64 dstOffset, UInt32 marker);
        **/
		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateRenderPass2KHR(IntPtr device, ref VkRenderPassCreateInfo2KHR pCreateInfo, IntPtr pAllocator, ref UInt64 pRenderPass);
        /**
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBeginRenderPass2KHR(IntPtr commandBuffer, [In, Out] VkRenderPassBeginInfo pRenderPassBegin, VkSubpassBeginInfoKHR pSubpassBeginInfo);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdNextSubpass2KHR(IntPtr commandBuffer, VkSubpassBeginInfoKHR pSubpassBeginInfo, VkSubpassEndInfoKHR pSubpassEndInfo);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdEndRenderPass2KHR(IntPtr commandBuffer, VkSubpassEndInfoKHR pSubpassEndInfo);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkGetAndroidHardwareBufferPropertiesANDROID(IntPtr device, AHardwareBuffer[] buffer, VkAndroidHardwareBufferPropertiesANDROID pProperties);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkGetMemoryAndroidHardwareBufferANDROID(IntPtr device, VkMemoryGetAndroidHardwareBufferInfoANDROID pInfo, ref AHardwareBuffer pBuffer);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdDrawIndirectCountKHR(IntPtr commandBuffer, UInt64 buffer, UInt64 offset, UInt64 countBuffer, UInt64 countBufferOffset, UInt32 maxDrawCount, UInt32 stride);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdDrawIndexedIndirectCountKHR(IntPtr commandBuffer, UInt64 buffer, UInt64 offset, UInt64 countBuffer, UInt64 countBufferOffset, UInt32 maxDrawCount, UInt32 stride);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdSetCheckpointNV(IntPtr commandBuffer, IntPtr pCheckpointMarker);
        **/
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetQueueCheckpointDataNV(IntPtr queue, ref UInt32 pCheckpointDataCount, [In, Out] VkCheckpointDataNV[] pCheckpointData);
        /**
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBindTransformFeedbackBuffersEXT(IntPtr commandBuffer, UInt32 firstBinding, UInt32 bindingCount, UInt64[] pBuffers, UInt64[] pOffsets, UInt64[] pSizes);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBeginTransformFeedbackEXT(IntPtr commandBuffer, UInt32 firstCounterBuffer, UInt32 counterBufferCount, UInt64[] pCounterBuffers, UInt64[] pCounterBufferOffsets);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdEndTransformFeedbackEXT(IntPtr commandBuffer, UInt32 firstCounterBuffer, UInt32 counterBufferCount, UInt64[] pCounterBuffers, UInt64[] pCounterBufferOffsets);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBeginQueryIndexedEXT(IntPtr commandBuffer, UInt64 queryPool, UInt32 query, VkQueryControlFlags flags, UInt32 index);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdEndQueryIndexedEXT(IntPtr commandBuffer, UInt64 queryPool, UInt32 query, UInt32 index);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdDrawIndirectByteCountEXT(IntPtr commandBuffer, UInt32 instanceCount, UInt32 firstInstance, UInt64 counterBuffer, UInt64 counterBufferOffset, UInt32 counterOffset, UInt32 vertexStride);
        ***/
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdSetExclusiveScissorNV(IntPtr commandBuffer, UInt32 firstExclusiveScissor, UInt32 exclusiveScissorCount, MgRect2D* pExclusiveScissors);
        /***
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBindShadingRateImageNV(IntPtr commandBuffer, UInt64 imageView, MgImageLayout imageLayout);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdSetViewportShadingRatePaletteNV(IntPtr commandBuffer, UInt32 firstViewport, UInt32 viewportCount, VkShadingRatePaletteNV* pShadingRatePalettes);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdSetCoarseSampleOrderNV(IntPtr commandBuffer, VkCoarseSampleOrderTypeNV sampleOrderType, UInt32 customSampleOrderCount, VkCoarseSampleOrderCustomNV* pCustomSampleOrders);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdDrawMeshTasksNV(IntPtr commandBuffer, UInt32 taskCount, UInt32 firstTask);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdDrawMeshTasksIndirectNV(IntPtr commandBuffer, UInt64 buffer, UInt64 offset, UInt32 drawCount, UInt32 stride);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdDrawMeshTasksIndirectCountNV(IntPtr commandBuffer, UInt64 buffer, UInt64 offset, UInt64 countBuffer, UInt64 countBufferOffset, UInt32 maxDrawCount, UInt32 stride);
        **/
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkCompileDeferredNV(IntPtr device, UInt64 pipeline, UInt32 shader);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkCreateAccelerationStructureNV(IntPtr device, ref VkAccelerationStructureCreateInfoNV pCreateInfo, IntPtr pAllocator, ref UInt64 pAccelerationStructure);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyAccelerationStructureNV(IntPtr device, UInt64 accelerationStructure, IntPtr pAllocator);
        /**
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkGetAccelerationStructureMemoryRequirementsNV(IntPtr device, VkAccelerationStructureMemoryRequirementsInfoNV pInfo, VkMemoryRequirements2KHR* pMemoryRequirements);
        **/
		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkBindAccelerationStructureMemoryNV(IntPtr device, UInt32 bindInfoCount, [In, Out] VkBindAccelerationStructureMemoryInfoNV[] pBindInfos);
        /**
		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdCopyAccelerationStructureNV(IntPtr commandBuffer, UInt64 dst, UInt64 src, VkCopyAccelerationStructureModeNV mode);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdWriteAccelerationStructuresPropertiesNV(IntPtr commandBuffer, UInt32 accelerationStructureCount, UInt64[] pAccelerationStructures, VkQueryType queryType, UInt64 queryPool, UInt32 firstQuery);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBuildAccelerationStructureNV(IntPtr commandBuffer, [In, Out] VkAccelerationStructureInfoNV pInfo, UInt64 instanceData, UInt64 instanceOffset, VkBool32 update, UInt64 dst, UInt64 src, UInt64 scratch, UInt64 scratchOffset);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdTraceRaysNV(IntPtr commandBuffer, UInt64 raygenShaderBindingTableBuffer, UInt64 raygenShaderBindingOffset, UInt64 missShaderBindingTableBuffer, UInt64 missShaderBindingOffset, UInt64 missShaderBindingStride, UInt64 hitShaderBindingTableBuffer, UInt64 hitShaderBindingOffset, UInt64 hitShaderBindingStride, UInt64 callableShaderBindingTableBuffer, UInt64 callableShaderBindingOffset, UInt64 callableShaderBindingStride, UInt32 width, UInt32 height, UInt32 depth);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetRayTracingShaderGroupHandlesNV(IntPtr device, UInt64 pipeline, UInt32 firstGroup, UInt32 groupCount, UIntPtr dataSize, IntPtr[] pData);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetAccelerationStructureHandleNV(IntPtr device, UInt64 accelerationStructure, UIntPtr dataSize, IntPtr[] pData);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkCreateRayTracingPipelinesNV(IntPtr device, UInt64 pipelineCache, UInt32 createInfoCount, [In, Out] VkRayTracingPipelineCreateInfoNV[] pCreateInfos, IntPtr pAllocator, [In, Out] UInt64[] pPipelines);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkGetImageDrmFormatModifierPropertiesEXT(IntPtr device, UInt64 image, VkImageDrmFormatModifierPropertiesEXT pProperties);
        ***/
	}
}
