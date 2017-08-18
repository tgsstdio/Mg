using Magnesium;
using System;
using System.Runtime.InteropServices;
namespace Magnesium.Vulkan
{
	internal static class Interops
	{
		const string VULKAN_LIB = "vulkan-1";

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreateInstance(ref VkInstanceCreateInfo pCreateInfo, IntPtr pAllocator, ref IntPtr pInstance);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyInstance(IntPtr instance, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkEnumeratePhysicalDevices(IntPtr instance, ref UInt32 pPhysicalDeviceCount, [In, Out] IntPtr[] pPhysicalDevices);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static PFN_vkVoidFunction vkGetDeviceProcAddr(IntPtr device, [MarshalAs(UnmanagedType.LPStr)] string pName);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static IntPtr vkGetInstanceProcAddr(IntPtr instance, [MarshalAs(UnmanagedType.LPStr)] string pName);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceProperties(IntPtr physicalDevice, ref VkPhysicalDeviceProperties pProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkGetPhysicalDeviceQueueFamilyProperties(IntPtr physicalDevice, UInt32* pQueueFamilyPropertyCount, VkQueueFamilyProperties* pQueueFamilyProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceMemoryProperties(IntPtr physicalDevice, ref VkPhysicalDeviceMemoryProperties pMemoryProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceFeatures(IntPtr physicalDevice, ref VkPhysicalDeviceFeatures pFeatures);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkGetPhysicalDeviceFormatProperties(IntPtr physicalDevice, VkFormat format, ref VkFormatProperties pFormatProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkGetPhysicalDeviceImageFormatProperties(IntPtr physicalDevice, VkFormat format, VkImageType type, VkImageTiling tiling, VkImageUsageFlags usage, VkImageCreateFlags flags, ref VkImageFormatProperties pImageFormatProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreateDevice(IntPtr physicalDevice, ref VkDeviceCreateInfo pCreateInfo, IntPtr pAllocator, ref IntPtr pDevice);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyDevice(IntPtr device, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		public extern static Result vkEnumerateInstanceLayerProperties(ref UInt32 pPropertyCount, [In, Out] VkLayerProperties[] pProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkEnumerateInstanceExtensionProperties(IntPtr pLayerName, ref UInt32 pPropertyCount, [In, Out] VkExtensionProperties[] pProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkEnumerateDeviceLayerProperties(IntPtr physicalDevice, ref UInt32 pPropertyCount, [In, Out] VkLayerProperties[] pProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkEnumerateDeviceExtensionProperties(IntPtr physicalDevice, IntPtr pLayerName, ref UInt32 pPropertyCount, [In, Out] VkExtensionProperties[] pProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetDeviceQueue(IntPtr device, UInt32 queueFamilyIndex, UInt32 queueIndex, ref IntPtr pQueue);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe Result vkQueueSubmit(IntPtr queue, UInt32 submitCount, VkSubmitInfo* pSubmits, UInt64 fence);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkQueueWaitIdle(IntPtr queue);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkDeviceWaitIdle(IntPtr device);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe Result vkAllocateMemory(IntPtr device, VkMemoryAllocateInfo* pAllocateInfo, IntPtr pAllocator, UInt64* pMemory);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkFreeMemory(IntPtr device, UInt64 memory, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkMapMemory(IntPtr device, UInt64 memory, UInt64 offset, UInt64 size, UInt32 flags, ref IntPtr ppData);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkUnmapMemory(IntPtr device, UInt64 memory);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe Result vkFlushMappedMemoryRanges(IntPtr device, UInt32 memoryRangeCount, VkMappedMemoryRange* pMemoryRanges);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe Result vkInvalidateMappedMemoryRanges(IntPtr device, UInt32 memoryRangeCount, VkMappedMemoryRange* pMemoryRanges);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetDeviceMemoryCommitment(IntPtr device, UInt64 memory, ref UInt64 pCommittedMemoryInBytes);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkGetBufferMemoryRequirements(IntPtr device, UInt64 buffer, Magnesium.MgMemoryRequirements* pMemoryRequirements);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkBindBufferMemory(IntPtr device, UInt64 buffer, UInt64 memory, UInt64 memoryOffset);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkGetImageMemoryRequirements(IntPtr device, UInt64 image, Magnesium.MgMemoryRequirements* pMemoryRequirements);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkBindImageMemory(IntPtr device, UInt64 image, UInt64 memory, UInt64 memoryOffset);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkGetImageSparseMemoryRequirements(IntPtr device, UInt64 image, UInt32* pSparseMemoryRequirementCount, Magnesium.MgSparseImageMemoryRequirements* pSparseMemoryRequirements);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetPhysicalDeviceSparseImageFormatProperties(IntPtr physicalDevice, VkFormat format, VkImageType type, VkSampleCountFlags samples, VkImageUsageFlags usage, VkImageTiling tiling, ref UInt32 pPropertyCount, [In, Out] VkSparseImageFormatProperties[] pProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe Result vkQueueBindSparse(IntPtr queue, UInt32 bindInfoCount, VkBindSparseInfo* pBindInfo, UInt64 fence);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreateFence(IntPtr device, ref VkFenceCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pFence);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyFence(IntPtr device, UInt64 fence, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkResetFences(IntPtr device, UInt32 fenceCount, [In] UInt64[] pFences);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkGetFenceStatus(IntPtr device, UInt64 fence);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkWaitForFences(IntPtr device, UInt32 fenceCount, [In] UInt64[] pFences, VkBool32 waitAll, UInt64 timeout);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreateSemaphore(IntPtr device, ref VkSemaphoreCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pSemaphore);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroySemaphore(IntPtr device, UInt64 semaphore, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreateEvent(IntPtr device, ref VkEventCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pEvent);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyEvent(IntPtr device, UInt64 @event, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkGetEventStatus(IntPtr device, UInt64 @event);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkSetEvent(IntPtr device, UInt64 @event);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkResetEvent(IntPtr device, UInt64 @event);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreateQueryPool(IntPtr device, ref VkQueryPoolCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pQueryPool);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyQueryPool(IntPtr device, UInt64 queryPool, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkGetQueryPoolResults(IntPtr device, UInt64 queryPool, UInt32 firstQuery, UInt32 queryCount, IntPtr dataSize, IntPtr pData, UInt64 stride, VkQueryResultFlags flags);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreateBuffer(IntPtr device, ref VkBufferCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pBuffer);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyBuffer(IntPtr device, UInt64 buffer, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreateBufferView(IntPtr device, ref VkBufferViewCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pView);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyBufferView(IntPtr device, UInt64 bufferView, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreateImage(IntPtr device, ref VkImageCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pImage);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyImage(IntPtr device, UInt64 image, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetImageSubresourceLayout(IntPtr device, UInt64 image, [In] Magnesium.MgImageSubresource pSubresource, [In, Out] Magnesium.MgSubresourceLayout pLayout);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreateImageView(IntPtr device, ref VkImageViewCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pView);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyImageView(IntPtr device, UInt64 imageView, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreateShaderModule(IntPtr device, ref VkShaderModuleCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pShaderModule);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyShaderModule(IntPtr device, UInt64 shaderModule, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreatePipelineCache(IntPtr device, ref VkPipelineCacheCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pPipelineCache);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyPipelineCache(IntPtr device, UInt64 pipelineCache, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkGetPipelineCacheData(IntPtr device, UInt64 pipelineCache, ref UIntPtr pDataSize, IntPtr pData);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkMergePipelineCaches(IntPtr device, UInt64 dstCache, UInt32 srcCacheCount, [In] UInt64[] pSrcCaches);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreateGraphicsPipelines(IntPtr device, UInt64 pipelineCache, UInt32 createInfoCount, [In, Out] VkGraphicsPipelineCreateInfo[] pCreateInfos, IntPtr pAllocator, [In, Out] UInt64[] pPipelines);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreateComputePipelines(IntPtr device, UInt64 pipelineCache, UInt32 createInfoCount, [In, Out] VkComputePipelineCreateInfo[] pCreateInfos, IntPtr pAllocator, [In, Out] UInt64[] pPipelines);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyPipeline(IntPtr device, UInt64 pipeline, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreatePipelineLayout(IntPtr device, ref VkPipelineLayoutCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pPipelineLayout);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyPipelineLayout(IntPtr device, UInt64 pipelineLayout, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreateSampler(IntPtr device, ref VkSamplerCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pSampler);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroySampler(IntPtr device, UInt64 sampler, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreateDescriptorSetLayout(IntPtr device, ref VkDescriptorSetLayoutCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pSetLayout);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyDescriptorSetLayout(IntPtr device, UInt64 descriptorSetLayout, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreateDescriptorPool(IntPtr device, ref VkDescriptorPoolCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pDescriptorPool);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyDescriptorPool(IntPtr device, UInt64 descriptorPool, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkResetDescriptorPool(IntPtr device, UInt64 descriptorPool, UInt32 flags);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkAllocateDescriptorSets(IntPtr device, ref VkDescriptorSetAllocateInfo pAllocateInfo, [In, Out] UInt64[] pDescriptorSets);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkFreeDescriptorSets(IntPtr device, UInt64 descriptorPool, UInt32 descriptorSetCount, [In, Out] UInt64[] pDescriptorSets);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkUpdateDescriptorSets(IntPtr device, UInt32 descriptorWriteCount, VkWriteDescriptorSet* pDescriptorWrites, UInt32 descriptorCopyCount, VkCopyDescriptorSet* pDescriptorCopies);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreateFramebuffer(IntPtr device, ref VkFramebufferCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pFramebuffer);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyFramebuffer(IntPtr device, UInt64 framebuffer, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreateRenderPass(IntPtr device, ref VkRenderPassCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pRenderPass);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyRenderPass(IntPtr device, UInt64 renderPass, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkGetRenderAreaGranularity(IntPtr device, UInt64 renderPass, MgExtent2D* pGranularity);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreateCommandPool(IntPtr device, ref VkCommandPoolCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pCommandPool);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyCommandPool(IntPtr device, UInt64 commandPool, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkResetCommandPool(IntPtr device, UInt64 commandPool, VkCommandPoolResetFlags flags);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe Result vkAllocateCommandBuffers(IntPtr device, VkCommandBufferAllocateInfo* pAllocateInfo, IntPtr* pCommandBuffers);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkFreeCommandBuffers(IntPtr device, UInt64 commandPool, UInt32 commandBufferCount, [In] IntPtr[] pCommandBuffers);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkBeginCommandBuffer(IntPtr commandBuffer, ref VkCommandBufferBeginInfo pBeginInfo);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkEndCommandBuffer(IntPtr commandBuffer);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkResetCommandBuffer(IntPtr commandBuffer, VkCommandBufferResetFlags flags);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBindPipeline(IntPtr commandBuffer, VkPipelineBindPoint pipelineBindPoint, UInt64 pipeline);

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
		internal extern static void vkCmdBindDescriptorSets(IntPtr commandBuffer, VkPipelineBindPoint pipelineBindPoint, UInt64 layout, UInt32 firstSet, UInt32 descriptorSetCount, [In] UInt64[] pDescriptorSets, UInt32 dynamicOffsetCount, [In] UInt32[] pDynamicOffsets);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBindIndexBuffer(IntPtr commandBuffer, UInt64 buffer, UInt64 offset, VkIndexType indexType);

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
		internal extern static unsafe void vkCmdCopyImage(IntPtr commandBuffer, UInt64 srcImage, VkImageLayout srcImageLayout, UInt64 dstImage, VkImageLayout dstImageLayout, UInt32 regionCount, Magnesium.MgImageCopy* pRegions);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBlitImage(IntPtr commandBuffer, UInt64 srcImage, MgImageLayout srcImageLayout, UInt64 dstImage, MgImageLayout dstImageLayout, UInt32 regionCount, [In] MgImageBlit[] pRegions, VkFilter filter);

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
		internal extern static unsafe void vkCmdClearDepthStencilImage(IntPtr commandBuffer, UInt64 image, MgImageLayout imageLayout, [In] MgClearDepthStencilValue pDepthStencil, UInt32 rangeCount, [In] Magnesium.MgImageSubresourceRange[] pRanges);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdClearAttachments(IntPtr commandBuffer, UInt32 attachmentCount, Magnesium.MgClearAttachment* pAttachments, UInt32 rectCount, Magnesium.MgClearRect* pRects);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdResolveImage(IntPtr commandBuffer, UInt64 srcImage, MgImageLayout srcImageLayout, UInt64 dstImage, MgImageLayout dstImageLayout, UInt32 regionCount, MgImageResolve* pRegions);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdSetEvent(IntPtr commandBuffer, UInt64 @event, VkPipelineStageFlags stageMask);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdResetEvent(IntPtr commandBuffer, UInt64 @event, VkPipelineStageFlags stageMask);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdWaitEvents(IntPtr commandBuffer, UInt32 eventCount, UInt64* pEvents, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, UInt32 memoryBarrierCount, VkMemoryBarrier* pMemoryBarriers, UInt32 bufferMemoryBarrierCount, VkBufferMemoryBarrier* pBufferMemoryBarriers, UInt32 imageMemoryBarrierCount, VkImageMemoryBarrier* pImageMemoryBarriers);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkCmdPipelineBarrier(IntPtr commandBuffer, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, VkDependencyFlags dependencyFlags, UInt32 memoryBarrierCount, VkMemoryBarrier* pMemoryBarriers, UInt32 bufferMemoryBarrierCount, VkBufferMemoryBarrier* pBufferMemoryBarriers, UInt32 imageMemoryBarrierCount, VkImageMemoryBarrier* pImageMemoryBarriers);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdBeginQuery(IntPtr commandBuffer, UInt64 queryPool, UInt32 query, VkQueryControlFlags flags);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdEndQuery(IntPtr commandBuffer, UInt64 queryPool, UInt32 query);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdResetQueryPool(IntPtr commandBuffer, UInt64 queryPool, UInt32 firstQuery, UInt32 queryCount);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdWriteTimestamp(IntPtr commandBuffer, VkPipelineStageFlags pipelineStage, UInt64 queryPool, UInt32 query);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdCopyQueryPoolResults(IntPtr commandBuffer, UInt64 queryPool, UInt32 firstQuery, UInt32 queryCount, UInt64 dstBuffer, UInt64 dstOffset, UInt64 stride, VkQueryResultFlags flags);

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
		internal extern static Result vkCreateAndroidSurfaceKHR(IntPtr instance, ref VkAndroidSurfaceCreateInfoKHR pCreateInfo, IntPtr pAllocator, ref UInt64 pSurface);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkGetPhysicalDeviceDisplayPropertiesKHR(IntPtr physicalDevice, ref UInt32 pPropertyCount, [In, Out] VkDisplayPropertiesKHR[] pProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkGetPhysicalDeviceDisplayPlanePropertiesKHR(IntPtr physicalDevice, ref UInt32 pPropertyCount, [In, Out] VkDisplayPlanePropertiesKHR[] pProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkGetDisplayPlaneSupportedDisplaysKHR(IntPtr physicalDevice, UInt32 planeIndex, ref UInt32 pDisplayCount, [In, Out] UInt64[] pDisplays);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkGetDisplayModePropertiesKHR(IntPtr physicalDevice, UInt64 display, ref UInt32 pPropertyCount, [In, Out] VkDisplayModePropertiesKHR[] pProperties);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreateDisplayModeKHR(IntPtr physicalDevice, UInt64 display, ref VkDisplayModeCreateInfoKHR pCreateInfo, IntPtr pAllocator, ref UInt64 pMode);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkGetDisplayPlaneCapabilitiesKHR(IntPtr physicalDevice, UInt64 mode, UInt32 planeIndex, ref VkDisplayPlaneCapabilitiesKHR pCapabilities);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreateDisplayPlaneSurfaceKHR(IntPtr instance, ref VkDisplaySurfaceCreateInfoKHR pCreateInfo, IntPtr pAllocator, ref UInt64 pSurface);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreateSharedSwapchainsKHR(IntPtr device, UInt32 swapchainCount, [In] VkSwapchainCreateInfoKHR[] pCreateInfos, IntPtr pAllocator, [In, Out] UInt64[] pSwapchains);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreateMirSurfaceKHR(IntPtr instance, ref VkMirSurfaceCreateInfoKHR pCreateInfo, IntPtr pAllocator, ref UInt64 pSurface);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkBool32 vkGetPhysicalDeviceMirPresentationSupportKHR(IntPtr physicalDevice, UInt32 queueFamilyIndex, ref IntPtr connection);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroySurfaceKHR(IntPtr instance, UInt64 surface, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkGetPhysicalDeviceSurfaceSupportKHR(IntPtr physicalDevice, UInt32 queueFamilyIndex, UInt64 surface, ref VkBool32 pSupported);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkGetPhysicalDeviceSurfaceCapabilitiesKHR(IntPtr physicalDevice, UInt64 surface, ref VkSurfaceCapabilitiesKHR pSurfaceCapabilities);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkGetPhysicalDeviceSurfaceFormatsKHR(IntPtr physicalDevice, UInt64 surface, ref UInt32 pSurfaceFormatCount, [In, Out] VkSurfaceFormatKHR[] pSurfaceFormats);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkGetPhysicalDeviceSurfacePresentModesKHR(IntPtr physicalDevice, UInt64 surface, ref UInt32 pPresentModeCount, [In, Out] VkPresentModeKhr[] pPresentModes);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreateSwapchainKHR(IntPtr device, ref VkSwapchainCreateInfoKHR pCreateInfo, IntPtr pAllocator, ref UInt64 pSwapchain);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroySwapchainKHR(IntPtr device, UInt64 swapchain, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkGetSwapchainImagesKHR(IntPtr device, UInt64 swapchain, ref UInt32 pSwapchainImageCount, [In, Out] UInt64[] pSwapchainImages);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkAcquireNextImageKHR(IntPtr device, UInt64 swapchain, UInt64 timeout, UInt64 semaphore, UInt64 fence, ref UInt32 pImageIndex);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkQueuePresentKHR(IntPtr queue, ref VkPresentInfoKHR pPresentInfo);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static Result vkCreateWaylandSurfaceKHR(IntPtr instance, ref VkWaylandSurfaceCreateInfoKhr pCreateInfo, IntPtr pAllocator, ref UInt64 pSurface);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static VkBool32 vkGetPhysicalDeviceWaylandPresentationSupportKHR(IntPtr physicalDevice, UInt32 queueFamilyIndex, ref wl_display display);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static Result vkCreateWin32SurfaceKHR(IntPtr instance, ref VkWin32SurfaceCreateInfoKHR pCreateInfo, IntPtr pAllocator, ref UInt64 pSurface);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkBool32 vkGetPhysicalDeviceWin32PresentationSupportKHR(IntPtr physicalDevice, UInt32 queueFamilyIndex);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static Result vkCreateXlibSurfaceKHR(IntPtr instance, ref VkXlibSurfaceCreateInfoKhr pCreateInfo, IntPtr pAllocator, ref UInt64 pSurface);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static VkBool32 vkGetPhysicalDeviceXlibPresentationSupportKHR(IntPtr physicalDevice, UInt32 queueFamilyIndex, ref Display dpy, VisualID visualID);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static Result vkCreateXcbSurfaceKHR(IntPtr instance, ref VkXcbSurfaceCreateInfoKhr pCreateInfo, IntPtr pAllocator, ref UInt64 pSurface);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkBool32 vkGetPhysicalDeviceXcbPresentationSupportKHR(IntPtr physicalDevice, UInt32 queueFamilyIndex, ref IntPtr connection, IntPtr visual_id);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static Result vkCreateDebugReportCallbackEXT(IntPtr instance, ref VkDebugReportCallbackCreateInfoEXT pCreateInfo, IntPtr pAllocator, ref UInt64 pCallback);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDestroyDebugReportCallbackEXT(IntPtr instance, UInt64 callback, IntPtr pAllocator);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkDebugReportMessageEXT(IntPtr instance, VkDebugReportFlagsExt flags, VkDebugReportObjectTypeExt objectType, UInt64 @object, UIntPtr location, Int32 messageCode, string pLayerPrefix, string pMessage);

		//[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static Result vkDebugMarkerSetObjectNameEXT(IntPtr device, [In, Out] ref VkDebugMarkerObjectNameInfoExt pNameInfo);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe Result vkDebugMarkerSetObjectTagEXT(IntPtr device, VkDebugMarkerObjectTagInfoEXT* pTagInfo);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdDebugMarkerBeginEXT(IntPtr commandBuffer, ref VkDebugMarkerMarkerInfoEXT pMarkerInfo);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdDebugMarkerEndEXT(IntPtr commandBuffer);

		[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkCmdDebugMarkerInsertEXT(IntPtr commandBuffer, ref VkDebugMarkerMarkerInfoEXT pMarkerInfo);

	}
}
