using System;

namespace Magnesium
{
    // Device
    public interface IMgDevice
	{
		PFN_vkVoidFunction GetDeviceProcAddr(string pName);
		void DestroyDevice(IMgAllocationCallbacks allocator);
		void GetDeviceQueue(UInt32 queueFamilyIndex, UInt32 queueIndex, out IMgQueue pQueue);
		MgResult DeviceWaitIdle();
		MgResult AllocateMemory(MgMemoryAllocateInfo pAllocateInfo, IMgAllocationCallbacks allocator, out IMgDeviceMemory pMemory);
		//void FreeMemory(MgDeviceMemory memory, MgAllocationCallbacks allocator);
		//MgResult MapMemory(MgDeviceMemory memory, UInt64 offset, UInt64 size, UInt32 flags, out IntPtr ppData);
		//void UnmapMemory(MgDeviceMemory memory);
		MgResult FlushMappedMemoryRanges(MgMappedMemoryRange[] pMemoryRanges);
		MgResult InvalidateMappedMemoryRanges(MgMappedMemoryRange[] pMemoryRanges);
		void GetDeviceMemoryCommitment(IMgDeviceMemory memory, ref UInt64 pCommittedMemoryInBytes);
		void GetBufferMemoryRequirements(IMgBuffer buffer, out MgMemoryRequirements pMemoryRequirements);
		//MgResult BindBufferMemory(IMgBuffer buffer, IMgDeviceMemory memory, UInt64 memoryOffset);
		void GetImageMemoryRequirements(IMgImage image, out MgMemoryRequirements memoryRequirements);
		//MgResult BindImageMemory(IMgImage image, IMgDeviceMemory memory, UInt64 memoryOffset);
		void GetImageSparseMemoryRequirements(IMgImage image, out MgSparseImageMemoryRequirements[] sparseMemoryRequirements);
		MgResult CreateFence(MgFenceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFence fence);
		//void DestroyFence(IMgFence fence, MgAllocationCallbacks allocator);
		MgResult ResetFences(IMgFence[] pFences);
		MgResult GetFenceStatus(IMgFence fence);
		MgResult WaitForFences(IMgFence[] pFences, bool waitAll, UInt64 timeout);
		MgResult CreateSemaphore(MgSemaphoreCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSemaphore pSemaphore);
		//void DestroySemaphore(MgSemaphore semaphore, MgAllocationCallbacks allocator);
		MgResult CreateEvent(MgEventCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgEvent @event);
		//void DestroyEvent(MgEvent @event, MgAllocationCallbacks allocator);
//		MgResult GetEventStatus(IMgEvent @event);
//		MgResult SetEvent(IMgEvent @event);
//		MgResult ResetEvent(IMgEvent @event);
		MgResult CreateQueryPool(MgQueryPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgQueryPool queryPool);
		//void DestroyQueryPool(MgQueryPool queryPool, MgAllocationCallbacks allocator);
		MgResult GetQueryPoolResults(IMgQueryPool queryPool, UInt32 firstQuery, UInt32 queryCount, IntPtr dataSize, IntPtr pData, UInt64 stride, MgQueryResultFlagBits flags);
		MgResult CreateBuffer(MgBufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBuffer pBuffer);
		//void DestroyBuffer(MgBuffer buffer, MgAllocationCallbacks allocator);
		MgResult CreateBufferView(MgBufferViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBufferView pView);
		//void DestroyBufferView(MgBufferView bufferView, MgAllocationCallbacks allocator);
		MgResult CreateImage(MgImageCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgImage pImage);
		//void DestroyImage(IMgImage image, MgAllocationCallbacks allocator);
		void GetImageSubresourceLayout(IMgImage image, MgImageSubresource pSubresource, out MgSubresourceLayout pLayout);
		MgResult CreateImageView(MgImageViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgImageView pView);
		//void DestroyImageView(MgImageView imageView, MgAllocationCallbacks allocator);
		MgResult CreateShaderModule(MgShaderModuleCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgShaderModule pShaderModule);
		//void DestroyShaderModule(MgShaderModule shaderModule, MgAllocationCallbacks allocator);
		MgResult CreatePipelineCache(MgPipelineCacheCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineCache pPipelineCache);
		//void DestroyPipelineCache(IMgPipelineCache pipelineCache, MgAllocationCallbacks allocator);
		MgResult GetPipelineCacheData(IMgPipelineCache pipelineCache, out byte[] pData);
		MgResult MergePipelineCaches(IMgPipelineCache dstCache, IMgPipelineCache[] pSrcCaches);
		MgResult CreateGraphicsPipelines(IMgPipelineCache pipelineCache, MgGraphicsPipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines);
		MgResult CreateComputePipelines(IMgPipelineCache pipelineCache, MgComputePipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines);
		//void DestroyPipeline(MgPipeline pipeline, MgAllocationCallbacks allocator);
		MgResult CreatePipelineLayout(MgPipelineLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineLayout pPipelineLayout);
		//void DestroyPipelineLayout(MgPipelineLayout pipelineLayout, MgAllocationCallbacks allocator);
		MgResult CreateSampler(MgSamplerCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSampler pSampler);
		//void DestroySampler(MgSampler sampler, MgAllocationCallbacks allocator);
		MgResult CreateDescriptorSetLayout(MgDescriptorSetLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorSetLayout pSetLayout);
		//void DestroyDescriptorSetLayout(MgDescriptorSetLayout descriptorSetLayout, MgAllocationCallbacks allocator);
		MgResult CreateDescriptorPool(MgDescriptorPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorPool pDescriptorPool);
		//void DestroyDescriptorPool(IMgDescriptorPool descriptorPool, MgAllocationCallbacks allocator);
		//MgResult ResetDescriptorPool(IMgDescriptorPool descriptorPool, UInt32 flags);
		MgResult AllocateDescriptorSets(MgDescriptorSetAllocateInfo pAllocateInfo, out IMgDescriptorSet[] pDescriptorSets);
		MgResult FreeDescriptorSets(IMgDescriptorPool descriptorPool, IMgDescriptorSet[] pDescriptorSets);
		void UpdateDescriptorSets(MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies);
		MgResult CreateFramebuffer(MgFramebufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFramebuffer pFramebuffer);
		//void DestroyFramebuffer(MgFramebuffer framebuffer, MgAllocationCallbacks allocator);
		MgResult CreateRenderPass(MgRenderPassCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgRenderPass pRenderPass);
//		void DestroyRenderPass(IMgRenderPass renderPass, MgAllocationCallbacks allocator);
		void GetRenderAreaGranularity(IMgRenderPass renderPass, out MgExtent2D pGranularity);
		MgResult CreateCommandPool(MgCommandPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgCommandPool pCommandPool);
		//void DestroyCommandPool(MgCommandPool commandPool, MgAllocationCallbacks allocator);
		//MgResult ResetCommandPool(MgCommandPool commandPool, MgCommandPoolResetFlagBits flags);
		MgResult AllocateCommandBuffers(MgCommandBufferAllocateInfo pAllocateInfo, IMgCommandBuffer[] pCommandBuffers);
		void FreeCommandBuffers(IMgCommandPool commandPool, IMgCommandBuffer[] pCommandBuffers);
		MgResult CreateSharedSwapchainsKHR(MgSwapchainCreateInfoKHR[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgSwapchainKHR[] pSwapchains);
		MgResult CreateSwapchainKHR(MgSwapchainCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSwapchainKHR pSwapchain);
		//void DestroySwapchainKHR(IMgSwapchainKHR swapchain, MgAllocationCallbacks allocator);
		MgResult GetSwapchainImagesKHR(IMgSwapchainKHR swapchain, out IMgImage[] pSwapchainImages);
		MgResult AcquireNextImageKHR(IMgSwapchainKHR swapchain, UInt64 timeout, IMgSemaphore semaphore, IMgFence fence, out UInt32 pImageIndex);
	}
}

