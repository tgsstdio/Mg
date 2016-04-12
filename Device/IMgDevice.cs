using System;

namespace Magnesium
{
    // Device
    public interface IMgDevice
	{
		PFN_vkVoidFunction GetDeviceProcAddr(string pName);
		void DestroyDevice(MgAllocationCallbacks allocator);
		void GetDeviceQueue(UInt32 queueFamilyIndex, UInt32 queueIndex, out IMgQueue pQueue);
		Result DeviceWaitIdle();
		Result AllocateMemory(MgMemoryAllocateInfo pAllocateInfo, MgAllocationCallbacks allocator, out MgDeviceMemory pMemory);
		void FreeMemory(MgDeviceMemory memory, MgAllocationCallbacks allocator);
		Result MapMemory(MgDeviceMemory memory, UInt64 offset, UInt64 size, UInt32 flags, IntPtr ppData);
		void UnmapMemory(MgDeviceMemory memory);
		Result FlushMappedMemoryRanges(MgMappedMemoryRange[] pMemoryRanges);
		Result InvalidateMappedMemoryRanges(MgMappedMemoryRange[] pMemoryRanges);
		void GetDeviceMemoryCommitment(MgDeviceMemory memory, ref UInt64 pCommittedMemoryInBytes);
		void GetBufferMemoryRequirements(MgBuffer buffer, out MgMemoryRequirements pMemoryRequirements);
		Result BindBufferMemory(MgBuffer buffer, MgDeviceMemory memory, UInt64 memoryOffset);
		void GetImageMemoryRequirements(MgImage image, out MgMemoryRequirements memoryRequirements);
		Result BindImageMemory(MgImage image, MgDeviceMemory memory, UInt64 memoryOffset);
		void GetImageSparseMemoryRequirements(MgImage image, out MgSparseImageMemoryRequirements[] sparseMemoryRequirements);
		Result CreateFence(MgFenceCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgFence fence);
		void DestroyFence(MgFence fence, MgAllocationCallbacks allocator);
		Result ResetFences(MgFence[] pFences);
		Result GetFenceStatus(MgFence fence);
		Result WaitForFences(MgFence[] pFences, bool waitAll, UInt64 timeout);
		Result CreateSemaphore(MgSemaphoreCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgSemaphore pSemaphore);
		void DestroySemaphore(MgSemaphore semaphore, MgAllocationCallbacks allocator);
		Result CreateEvent(MgEventCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgEvent @event);
		void DestroyEvent(MgEvent @event, MgAllocationCallbacks allocator);
		Result GetEventStatus(MgEvent @event);
		Result SetEvent(MgEvent @event);
		Result ResetEvent(MgEvent @event);
		Result CreateQueryPool(MgQueryPoolCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgQueryPool queryPool);
		void DestroyQueryPool(MgQueryPool queryPool, MgAllocationCallbacks allocator);
		Result GetQueryPoolResults(MgQueryPool queryPool, UInt32 firstQuery, UInt32 queryCount, IntPtr dataSize, IntPtr pData, UInt64 stride, MgQueryResultFlagBits flags);
		Result CreateBuffer(MgBufferCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgBuffer pBuffer);
		void DestroyBuffer(MgBuffer buffer, MgAllocationCallbacks allocator);
		Result CreateBufferView(MgBufferViewCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgBufferView pView);
		void DestroyBufferView(MgBufferView bufferView, MgAllocationCallbacks allocator);
		Result CreateImage(MgImageCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgImage pImage);
		void DestroyImage(MgImage image, MgAllocationCallbacks allocator);
		void GetImageSubresourceLayout(MgImage image, MgImageSubresource pSubresource, out MgSubresourceLayout pLayout);
		Result CreateImageView(MgImageViewCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgImageView pView);
		void DestroyImageView(MgImageView imageView, MgAllocationCallbacks allocator);
		Result CreateShaderModule(MgShaderModuleCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgShaderModule pShaderModule);
		void DestroyShaderModule(MgShaderModule shaderModule, MgAllocationCallbacks allocator);
		Result CreatePipelineCache(MgPipelineCacheCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgPipelineCache pPipelineCache);
		void DestroyPipelineCache(MgPipelineCache pipelineCache, MgAllocationCallbacks allocator);
		Result GetPipelineCacheData(MgPipelineCache pipelineCache, UIntPtr pDataSize, IntPtr pData);
		Result MergePipelineCaches(MgPipelineCache dstCache, MgPipelineCache[] pSrcCaches);
		Result CreateGraphicsPipelines(MgPipelineCache pipelineCache, MgGraphicsPipelineCreateInfo[] pCreateInfos, MgAllocationCallbacks allocator, out MgPipeline[] pPipelines);
		Result CreateComputePipelines(MgPipelineCache pipelineCache, MgComputePipelineCreateInfo[] pCreateInfos, MgAllocationCallbacks allocator, out MgPipeline[] pPipelines);
		void DestroyPipeline(MgPipeline pipeline, MgAllocationCallbacks allocator);
		Result CreatePipelineLayout(MgPipelineLayoutCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgPipelineLayout pPipelineLayout);
		void DestroyPipelineLayout(MgPipelineLayout pipelineLayout, MgAllocationCallbacks allocator);
		Result CreateSampler(MgSamplerCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgSampler pSampler);
		void DestroySampler(MgSampler sampler, MgAllocationCallbacks allocator);
		Result CreateDescriptorSetLayout(MgDescriptorSetLayoutCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgDescriptorSetLayout pSetLayout);
		void DestroyDescriptorSetLayout(MgDescriptorSetLayout descriptorSetLayout, MgAllocationCallbacks allocator);
		Result CreateDescriptorPool(MgDescriptorPoolCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgDescriptorPool pDescriptorPool);
		void DestroyDescriptorPool(MgDescriptorPool descriptorPool, MgAllocationCallbacks allocator);
		Result ResetDescriptorPool(MgDescriptorPool descriptorPool, UInt32 flags);
		Result AllocateDescriptorSets(MgDescriptorSetAllocateInfo pAllocateInfo, MgDescriptorSet[] pDescriptorSets);
		Result FreeDescriptorSets(MgDescriptorPool descriptorPool, MgDescriptorSet[] pDescriptorSets);
		void UpdateDescriptorSets(MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies);
		Result CreateFramebuffer(MgFramebufferCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgFramebuffer pFramebuffer);
		void DestroyFramebuffer(MgFramebuffer framebuffer, MgAllocationCallbacks allocator);
		Result CreateRenderPass(MgRenderPassCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgRenderPass pRenderPass);
		void DestroyRenderPass(MgRenderPass renderPass, MgAllocationCallbacks allocator);
		void GetRenderAreaGranularity(MgRenderPass renderPass, out MgExtent2D pGranularity);
		Result CreateCommandPool(MgCommandPoolCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out MgCommandPool pCommandPool);
		void DestroyCommandPool(MgCommandPool commandPool, MgAllocationCallbacks allocator);
		Result ResetCommandPool(MgCommandPool commandPool, MgCommandPoolResetFlagBits flags);
		Result AllocateCommandBuffers(MgCommandBufferAllocateInfo pAllocateInfo, IMgCommandBuffer[] pCommandBuffers);
		void FreeCommandBuffers(MgCommandPool commandPool, IMgCommandBuffer[] pCommandBuffers);
		Result CreateSharedSwapchainsKHR(MgSwapchainCreateInfoKHR[] pCreateInfos, MgAllocationCallbacks allocator, out MgSwapchainKHR[] pSwapchains);
		Result CreateSwapchainKHR(MgSwapchainCreateInfoKHR pCreateInfo, MgAllocationCallbacks allocator, out MgSwapchainKHR pSwapchain);
		void DestroySwapchainKHR(MgSwapchainKHR swapchain, MgAllocationCallbacks allocator);
		Result GetSwapchainImagesKHR(MgSwapchainKHR swapchain, out MgImage[] pSwapchainImages);
		Result AcquireNextImageKHR(MgSwapchainKHR swapchain, UInt64 timeout, MgSemaphore semaphore, MgFence fence, out UInt32 pImageIndex);
	}
}

