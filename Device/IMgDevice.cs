using System;

namespace Magnesium
{
    // Device
    public interface IMgDevice
	{
		PFN_vkVoidFunction GetDeviceProcAddr(string pName);
		void DestroyDevice(IMgAllocationCallbacks allocator);
		void GetDeviceQueue(UInt32 queueFamilyIndex, UInt32 queueIndex, out IMgQueue pQueue);
		Result DeviceWaitIdle();
		Result AllocateMemory(MgMemoryAllocateInfo pAllocateInfo, IMgAllocationCallbacks allocator, out IMgDeviceMemory pMemory);
		//void FreeMemory(MgDeviceMemory memory, MgAllocationCallbacks allocator);
		//Result MapMemory(MgDeviceMemory memory, UInt64 offset, UInt64 size, UInt32 flags, out IntPtr ppData);
		//void UnmapMemory(MgDeviceMemory memory);
		Result FlushMappedMemoryRanges(MgMappedMemoryRange[] pMemoryRanges);
		Result InvalidateMappedMemoryRanges(MgMappedMemoryRange[] pMemoryRanges);
		void GetDeviceMemoryCommitment(IMgDeviceMemory memory, ref UInt64 pCommittedMemoryInBytes);
		void GetBufferMemoryRequirements(IMgBuffer buffer, out MgMemoryRequirements pMemoryRequirements);
		//Result BindBufferMemory(IMgBuffer buffer, IMgDeviceMemory memory, UInt64 memoryOffset);
		void GetImageMemoryRequirements(IMgImage image, out MgMemoryRequirements memoryRequirements);
		//Result BindImageMemory(IMgImage image, IMgDeviceMemory memory, UInt64 memoryOffset);
		void GetImageSparseMemoryRequirements(IMgImage image, out MgSparseImageMemoryRequirements[] sparseMemoryRequirements);
		Result CreateFence(MgFenceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFence fence);
		//void DestroyFence(IMgFence fence, MgAllocationCallbacks allocator);
		Result ResetFences(IMgFence[] pFences);
		Result GetFenceStatus(IMgFence fence);
		Result WaitForFences(IMgFence[] pFences, bool waitAll, UInt64 timeout);
		Result CreateSemaphore(MgSemaphoreCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSemaphore pSemaphore);
		//void DestroySemaphore(MgSemaphore semaphore, MgAllocationCallbacks allocator);
		Result CreateEvent(MgEventCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgEvent @event);
		//void DestroyEvent(MgEvent @event, MgAllocationCallbacks allocator);
//		Result GetEventStatus(IMgEvent @event);
//		Result SetEvent(IMgEvent @event);
//		Result ResetEvent(IMgEvent @event);
		Result CreateQueryPool(MgQueryPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgQueryPool queryPool);
		//void DestroyQueryPool(MgQueryPool queryPool, MgAllocationCallbacks allocator);
		Result GetQueryPoolResults(IMgQueryPool queryPool, UInt32 firstQuery, UInt32 queryCount, IntPtr dataSize, IntPtr pData, UInt64 stride, MgQueryResultFlagBits flags);
		Result CreateBuffer(MgBufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBuffer pBuffer);
		//void DestroyBuffer(MgBuffer buffer, MgAllocationCallbacks allocator);
		Result CreateBufferView(MgBufferViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBufferView pView);
		//void DestroyBufferView(MgBufferView bufferView, MgAllocationCallbacks allocator);
		Result CreateImage(MgImageCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgImage pImage);
		//void DestroyImage(IMgImage image, MgAllocationCallbacks allocator);
		void GetImageSubresourceLayout(IMgImage image, MgImageSubresource pSubresource, out MgSubresourceLayout pLayout);
		Result CreateImageView(MgImageViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgImageView pView);
		//void DestroyImageView(MgImageView imageView, MgAllocationCallbacks allocator);
		Result CreateShaderModule(MgShaderModuleCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgShaderModule pShaderModule);
		//void DestroyShaderModule(MgShaderModule shaderModule, MgAllocationCallbacks allocator);
		Result CreatePipelineCache(MgPipelineCacheCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineCache pPipelineCache);
		//void DestroyPipelineCache(IMgPipelineCache pipelineCache, MgAllocationCallbacks allocator);
		Result GetPipelineCacheData(IMgPipelineCache pipelineCache, UIntPtr pDataSize, IntPtr pData);
		Result MergePipelineCaches(IMgPipelineCache dstCache, IMgPipelineCache[] pSrcCaches);
		Result CreateGraphicsPipelines(IMgPipelineCache pipelineCache, MgGraphicsPipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines);
		Result CreateComputePipelines(IMgPipelineCache pipelineCache, MgComputePipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines);
		//void DestroyPipeline(MgPipeline pipeline, MgAllocationCallbacks allocator);
		Result CreatePipelineLayout(MgPipelineLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineLayout pPipelineLayout);
		//void DestroyPipelineLayout(MgPipelineLayout pipelineLayout, MgAllocationCallbacks allocator);
		Result CreateSampler(MgSamplerCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSampler pSampler);
		//void DestroySampler(MgSampler sampler, MgAllocationCallbacks allocator);
		Result CreateDescriptorSetLayout(MgDescriptorSetLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorSetLayout pSetLayout);
		//void DestroyDescriptorSetLayout(MgDescriptorSetLayout descriptorSetLayout, MgAllocationCallbacks allocator);
		Result CreateDescriptorPool(MgDescriptorPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorPool pDescriptorPool);
		//void DestroyDescriptorPool(IMgDescriptorPool descriptorPool, MgAllocationCallbacks allocator);
		//Result ResetDescriptorPool(IMgDescriptorPool descriptorPool, UInt32 flags);
		Result AllocateDescriptorSets(MgDescriptorSetAllocateInfo pAllocateInfo, out IMgDescriptorSet[] pDescriptorSets);
		Result FreeDescriptorSets(IMgDescriptorPool descriptorPool, IMgDescriptorSet[] pDescriptorSets);
		void UpdateDescriptorSets(MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies);
		Result CreateFramebuffer(MgFramebufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFramebuffer pFramebuffer);
		//void DestroyFramebuffer(MgFramebuffer framebuffer, MgAllocationCallbacks allocator);
		Result CreateRenderPass(MgRenderPassCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgRenderPass pRenderPass);
//		void DestroyRenderPass(IMgRenderPass renderPass, MgAllocationCallbacks allocator);
		void GetRenderAreaGranularity(IMgRenderPass renderPass, out MgExtent2D pGranularity);
		Result CreateCommandPool(MgCommandPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgCommandPool pCommandPool);
		//void DestroyCommandPool(MgCommandPool commandPool, MgAllocationCallbacks allocator);
		//Result ResetCommandPool(MgCommandPool commandPool, MgCommandPoolResetFlagBits flags);
		Result AllocateCommandBuffers(MgCommandBufferAllocateInfo pAllocateInfo, IMgCommandBuffer[] pCommandBuffers);
		void FreeCommandBuffers(IMgCommandPool commandPool, IMgCommandBuffer[] pCommandBuffers);
		Result CreateSharedSwapchainsKHR(MgSwapchainCreateInfoKHR[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgSwapchainKHR[] pSwapchains);
		Result CreateSwapchainKHR(MgSwapchainCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSwapchainKHR pSwapchain);
		//void DestroySwapchainKHR(IMgSwapchainKHR swapchain, MgAllocationCallbacks allocator);
		Result GetSwapchainImagesKHR(IMgSwapchainKHR swapchain, out IMgImage[] pSwapchainImages);
		Result AcquireNextImageKHR(IMgSwapchainKHR swapchain, UInt64 timeout, IMgSemaphore semaphore, IMgFence fence, out UInt32 pImageIndex);
	}
}

