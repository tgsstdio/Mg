using System;
using Magnesium.Vulkan.Functions.Device;

namespace Magnesium.Vulkan
{
    public class VkDevice : IMgDevice
	{
        public VkDeviceInfo Info { get; }

        internal VkDevice(IntPtr handle)
		{
			Info = new VkDeviceInfo(handle);
		}

		public PFN_vkVoidFunction GetDeviceProcAddr(string pName)
		{
			return VkGetDeviceProcAddrSection.GetDeviceProcAddr(Info, pName);
		}

		public void DestroyDevice(IMgAllocationCallbacks allocator)
		{
            VkDestroyDeviceSection.DestroyDevice(Info, allocator);
		}

		public void GetDeviceQueue(uint queueFamilyIndex, uint queueIndex, out IMgQueue pQueue)
		{
            VkGetDeviceQueueSection.GetDeviceQueue(Info, queueFamilyIndex, queueIndex, out pQueue);
		}

		public MgResult DeviceWaitIdle()
		{
			return VkDeviceWaitIdleSection.DeviceWaitIdle(Info);
		}

		public MgResult AllocateMemory(MgMemoryAllocateInfo pAllocateInfo, IMgAllocationCallbacks allocator, out IMgDeviceMemory pMemory)
		{
            return VkAllocateMemorySection.AllocateMemory(Info, pAllocateInfo, allocator, out pMemory);
        }

		public MgResult FlushMappedMemoryRanges(MgMappedMemoryRange[] pMemoryRanges)
		{
            return VkFlushMappedMemoryRangesSection.FlushMappedMemoryRanges(Info, pMemoryRanges);
		}

		public MgResult InvalidateMappedMemoryRanges(MgMappedMemoryRange[] pMemoryRanges)
		{
            return VkInvalidateMappedMemoryRangesSection.InvalidateMappedMemoryRanges(Info, pMemoryRanges);
        }

		public void GetDeviceMemoryCommitment(IMgDeviceMemory memory, ref ulong pCommittedMemoryInBytes)
		{
            VkGetDeviceMemoryCommitmentSection.GetDeviceMemoryCommitment(Info, memory, ref pCommittedMemoryInBytes);
        }

		public void GetBufferMemoryRequirements(IMgBuffer buffer, out MgMemoryRequirements pMemoryRequirements)
		{
             VkGetBufferMemoryRequirementsSection.GetBufferMemoryRequirements(Info, buffer, out pMemoryRequirements);
		}

		public void GetImageMemoryRequirements(IMgImage image, out MgMemoryRequirements memoryRequirements)
        {
            VkGetImageMemoryRequirementsSection.GetImageMemoryRequirements(Info, image, out memoryRequirements);
        }

        public void GetImageSparseMemoryRequirements(IMgImage image, out MgSparseImageMemoryRequirements[] sparseMemoryRequirements)
        {
            VkGetImageSparseMemoryRequirementsSection.GetImageSparseMemoryRequirements(Info, image, out sparseMemoryRequirements);
        }

        public MgResult CreateFence(MgFenceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFence fence)
        {
            return VkCreateFenceSection.CreateFence(Info, pCreateInfo, allocator, out fence);
        }

        public MgResult ResetFences(IMgFence[] pFences)
        {
            return VkResetFencesSection.ResetFences(Info, pFences);
        }

        public MgResult GetFenceStatus(IMgFence fence)
        {
            return VkGetFenceStatusSection.GetFenceStatus(Info, fence);
        }

        public MgResult WaitForFences(IMgFence[] pFences, bool waitAll, ulong timeout)
        {
            return VkWaitForFencesSection.WaitForFences(Info, pFences, waitAll, timeout);
        }

        public MgResult CreateSemaphore(MgSemaphoreCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSemaphore pSemaphore)
        {
            return VkCreateSemaphoreSection.CreateSemaphore(Info, pCreateInfo, allocator, out pSemaphore);
        }

        public MgResult CreateEvent(MgEventCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgEvent @event)
        {
            return VkCreateEventSection.CreateEvent(Info, pCreateInfo, allocator, out @event);
        }

        public MgResult CreateQueryPool(MgQueryPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgQueryPool queryPool)
        {
            return VkCreateQueryPoolSection.CreateQueryPool(Info, pCreateInfo, allocator, out queryPool);
        }

        public MgResult GetQueryPoolResults(IMgQueryPool queryPool, uint firstQuery, uint queryCount, IntPtr dataSize, IntPtr pData, ulong stride, MgQueryResultFlagBits flags)
        {
            return VkGetQueryPoolResultsSection.GetQueryPoolResults(Info, queryPool, firstQuery, queryCount, dataSize, pData, stride, flags);
        }

        public MgResult CreateBuffer(MgBufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBuffer pBuffer)
        {
            return VkCreateBufferSection.CreateBuffer(Info, pCreateInfo, allocator, out pBuffer);
        }

        public MgResult CreateBufferView(MgBufferViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBufferView pView)
        {
            return VkCreateBufferViewSection.CreateBufferView(Info, pCreateInfo, allocator, out pView);
        }

        public MgResult CreateImage(MgImageCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgImage pImage)
        {
            return VkCreateImageSection.CreateImage(Info, pCreateInfo, allocator, out pImage);
        }

        public void GetImageSubresourceLayout(IMgImage image, MgImageSubresource pSubresource, out MgSubresourceLayout pLayout)
        {
            VkGetImageSubresourceLayoutSection.GetImageSubresourceLayout(Info, image, pSubresource, out pLayout);
        }

        public MgResult CreateImageView(MgImageViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgImageView pView)
        {
            return VkCreateImageViewSection.CreateImageView(Info, pCreateInfo, allocator, out pView);
        }

        public MgResult CreateShaderModule(MgShaderModuleCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgShaderModule pShaderModule)
        {
            return VkCreateShaderModuleSection.CreateShaderModule(Info, pCreateInfo, allocator, out pShaderModule);
        }

        public MgResult CreatePipelineCache(MgPipelineCacheCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineCache pPipelineCache)
        {
            return VkCreatePipelineCacheSection.CreatePipelineCache(Info, pCreateInfo, allocator, out pPipelineCache);
        }

        public MgResult GetPipelineCacheData(IMgPipelineCache pipelineCache, out byte[] pData)
        {
            return VkGetPipelineCacheDataSection.GetPipelineCacheData(Info, pipelineCache, out pData);
        }

        public MgResult MergePipelineCaches(IMgPipelineCache dstCache, IMgPipelineCache[] pSrcCaches)
        {
            return VkMergePipelineCachesSection.MergePipelineCaches(Info, dstCache, pSrcCaches);
        }

        public MgResult CreateGraphicsPipelines(IMgPipelineCache pipelineCache, MgGraphicsPipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines)
        {
            return VkCreateGraphicsPipelinesSection.CreateGraphicsPipelines(Info, pipelineCache, pCreateInfos, allocator, out pPipelines);
        }

        public MgResult CreateComputePipelines(IMgPipelineCache pipelineCache, MgComputePipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines)
        {
            return VkCreateComputePipelinesSection.CreateComputePipelines(Info, pipelineCache, pCreateInfos, allocator, out pPipelines);
        }

        public MgResult CreatePipelineLayout(MgPipelineLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineLayout pPipelineLayout)
        {
            return VkCreatePipelineLayoutSection.CreatePipelineLayout(Info, pCreateInfo, allocator, out pPipelineLayout);
        }

        public MgResult CreateSampler(MgSamplerCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSampler pSampler)
        {
            return VkCreateSamplerSection.CreateSampler(Info, pCreateInfo, allocator, out pSampler);
        }

        public MgResult CreateDescriptorSetLayout(MgDescriptorSetLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorSetLayout pSetLayout)
        {
            return VkCreateDescriptorSetLayoutSection.CreateDescriptorSetLayout(Info, pCreateInfo, allocator, out pSetLayout);
        }

        public MgResult CreateDescriptorPool(MgDescriptorPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorPool pDescriptorPool)
        {
            return VkCreateDescriptorPoolSection.CreateDescriptorPool(Info, pCreateInfo, allocator, out pDescriptorPool);
        }

        public MgResult AllocateDescriptorSets(MgDescriptorSetAllocateInfo pAllocateInfo, out IMgDescriptorSet[] pDescriptorSets)
        {
            return VkAllocateDescriptorSetsSection.AllocateDescriptorSets(Info, pAllocateInfo, out pDescriptorSets);
        }

        public MgResult FreeDescriptorSets(IMgDescriptorPool descriptorPool, IMgDescriptorSet[] pDescriptorSets)
        {
            return VkFreeDescriptorSetsSection.FreeDescriptorSets(Info, descriptorPool, pDescriptorSets);
        }

        public void UpdateDescriptorSets(MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies)
        {
            VkUpdateDescriptorSetsSection.UpdateDescriptorSets(Info, pDescriptorWrites, pDescriptorCopies);
        }

        public MgResult CreateFramebuffer(MgFramebufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFramebuffer pFramebuffer)
        {
            return VkCreateFramebufferSection.CreateFramebuffer(Info, pCreateInfo, allocator, out pFramebuffer);
        }

        public MgResult CreateRenderPass(MgRenderPassCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgRenderPass pRenderPass)
        {
            return VkCreateRenderPassSection.CreateRenderPass(Info, pCreateInfo, allocator, out pRenderPass);
        }

        public void GetRenderAreaGranularity(IMgRenderPass renderPass, out MgExtent2D pGranularity)
        {
            VkGetRenderAreaGranularitySection.GetRenderAreaGranularity(Info, renderPass, out pGranularity);
        }

        public MgResult CreateCommandPool(MgCommandPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgCommandPool pCommandPool)
        {
            return VkCreateCommandPoolSection.CreateCommandPool(Info, pCreateInfo, allocator, out pCommandPool);
        }

        public MgResult AllocateCommandBuffers(MgCommandBufferAllocateInfo pAllocateInfo, IMgCommandBuffer[] pCommandBuffers)
        {
            return VkAllocateCommandBuffersSection.AllocateCommandBuffers(Info, pAllocateInfo, pCommandBuffers);
        }

        public void FreeCommandBuffers(IMgCommandPool commandPool, IMgCommandBuffer[] pCommandBuffers)
        {
            VkFreeCommandBuffersSection.FreeCommandBuffers(Info, commandPool, pCommandBuffers);
        }

        public MgResult CreateSharedSwapchainsKHR(MgSwapchainCreateInfoKHR[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgSwapchainKHR[] pSwapchains)
        {
            return VkCreateSharedSwapchainsKHRSection.CreateSharedSwapchainsKHR(Info, pCreateInfos, allocator, out pSwapchains);
        }

        public MgResult CreateSwapchainKHR(MgSwapchainCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSwapchainKHR pSwapchain)
        {
            return VkCreateSwapchainKHRSection.CreateSwapchainKHR(Info, pCreateInfo, allocator, out pSwapchain);
        }

        public MgResult GetSwapchainImagesKHR(IMgSwapchainKHR swapchain, out IMgImage[] pSwapchainImages)
        {
            return VkGetSwapchainImagesKHRSection.GetSwapchainImagesKHR(Info, swapchain, out pSwapchainImages);
        }

        public MgResult AcquireNextImageKHR(IMgSwapchainKHR swapchain, ulong timeout, IMgSemaphore semaphore, IMgFence fence, out uint pImageIndex)
        {
            return VkAcquireNextImageKHRSection.AcquireNextImageKHR(Info, swapchain, timeout, semaphore, fence, out pImageIndex);
        }

        public MgResult CreateObjectTableNVX(MgObjectTableCreateInfoNVX pCreateInfo, IMgAllocationCallbacks allocator, out IMgObjectTableNVX pObjectTable)
        {
            return VkCreateObjectTableNVXSection.CreateObjectTableNVX(Info, pCreateInfo, allocator, out pObjectTable);
        }

        public MgResult CreateIndirectCommandsLayoutNVX(MgIndirectCommandsLayoutCreateInfoNVX pCreateInfo, IMgAllocationCallbacks allocator, out IMgIndirectCommandsLayoutNVX pIndirectCommandsLayout)
        {
            return VkCreateIndirectCommandsLayoutNVXSection.CreateIndirectCommandsLayoutNVX(Info, pCreateInfo, allocator, out pIndirectCommandsLayout);
        }

        public MgResult AcquireNextImage2KHR(MgAcquireNextImageInfoKHR pAcquireInfo, ref uint pImageIndex)
        {
            return VkAcquireNextImage2KHRSection.AcquireNextImage2KHR(Info, pAcquireInfo, ref pImageIndex);
        }

        public MgResult BindAccelerationStructureMemoryNV(MgBindAccelerationStructureMemoryInfoNV[] pBindInfos)
        {
            return VkBindAccelerationStructureMemoryNVSection.BindAccelerationStructureMemoryNV(Info, pBindInfos);
        }

        public MgResult BindBufferMemory2(MgBindBufferMemoryInfo[] pBindInfos)
        {
            return VkBindBufferMemory2Section.BindBufferMemory2(Info, pBindInfos);
        }

        public MgResult BindImageMemory2(MgBindImageMemoryInfo[] pBindInfos)
        {
            return VkBindImageMemory2Section.BindImageMemory2(Info, pBindInfos);
        }

        public MgResult CreateAccelerationStructureNV(MgAccelerationStructureCreateInfoNV pCreateInfo, IMgAllocationCallbacks pAllocator, out IMgAccelerationStructureNV pAccelerationStructure)
        {
            return VkCreateAccelerationStructureNVSection.CreateAccelerationStructureNV(Info, pCreateInfo, pAllocator, out pAccelerationStructure);
        }

        public MgResult CreateDescriptorUpdateTemplate(MgDescriptorUpdateTemplateCreateInfo pCreateInfo, IMgAllocationCallbacks pAllocator, out IMgDescriptorUpdateTemplate pDescriptorUpdateTemplate)
        {
            return VkCreateDescriptorUpdateTemplateSection.CreateDescriptorUpdateTemplate(Info, pCreateInfo, pAllocator, out pDescriptorUpdateTemplate);
        }

        public MgResult CreateRayTracingPipelinesNV(IMgPipelineCache pipelineCache, MgRayTracingPipelineCreateInfoNV[] pCreateInfos, IMgAllocationCallbacks pAllocator, out IMgPipeline[] pPipelines)
        {
            return VkCreateRayTracingPipelinesNVSection.CreateRayTracingPipelinesNV(Info, pipelineCache, pCreateInfos, pAllocator, out pPipelines);
        }

        public MgResult CreateRenderPass2KHR(MgRenderPassCreateInfo2KHR pCreateInfo, IMgAllocationCallbacks pAllocator, out IMgRenderPass pRenderPass)
        {
            return VkCreateRenderPass2KHRSection.CreateRenderPass2KHR(Info, pCreateInfo, pAllocator, out pRenderPass);
        }

        public MgResult CreateSamplerYcbcrConversion(MgSamplerYcbcrConversionCreateInfo pCreateInfo, IMgAllocationCallbacks pAllocator, IMgSamplerYcbcrConversion pYcbcrConversion)
        {
            throw new NotImplementedException();
        }

        public MgResult CreateValidationCacheEXT(MgValidationCacheCreateInfoEXT pCreateInfo, IMgAllocationCallbacks pAllocator, IMgValidationCacheEXT pValidationCache)
        {
            throw new NotImplementedException();
        }

        public MgResult DisplayPowerControlEXT(IMgDisplayKHR display, out MgDisplayPowerInfoEXT pDisplayPowerInfo)
        {
            throw new NotImplementedException();
        }

        public MgResult GetAccelerationStructureHandleNV(IMgAccelerationStructureNV accelerationStructure, UIntPtr dataSize, out IntPtr pData)
        {
            throw new NotImplementedException();
        }

        public MgResult GetCalibratedTimestampsEXT(MgCalibratedTimestampInfoEXT[] pTimestampInfos, out ulong[] pTimestamps, out ulong pMaxDeviation)
        {
            throw new NotImplementedException();
        }

        public MgResult GetDeviceGroupPresentCapabilitiesKHR(out MgDeviceGroupPresentCapabilitiesKHR pDeviceGroupPresentCapabilities)
        {
            throw new NotImplementedException();
        }

        public MgResult GetDeviceGroupSurfacePresentModesKHR(IMgSurfaceKHR surface, out MgDeviceGroupPresentModeFlagBitsKHR pModes)
        {
            throw new NotImplementedException();
        }

        public MgResult GetFenceFdKHR(MgFenceGetFdInfoKHR pGetFdInfo, out int pFd)
        {
            throw new NotImplementedException();
        }

        public MgResult GetImageDrmFormatModifierPropertiesEXT(IMgImage image, out MgImageDrmFormatModifierPropertiesEXT pProperties)
        {
            throw new NotImplementedException();
        }

        public MgResult GetMemoryFdKHR(MgMemoryGetFdInfoKHR pGetFdInfo, ref int pFd)
        {
            throw new NotImplementedException();
        }

        public MgResult GetMemoryFdPropertiesKHR(MgExternalMemoryHandleTypeFlagBits handleType, int fd, out MgMemoryFdPropertiesKHR pMemoryFdProperties)
        {
            throw new NotImplementedException();
        }

        public MgResult GetMemoryHostPointerPropertiesEXT(MgExternalMemoryHandleTypeFlagBits handleType, IntPtr pHostPointer, out MgMemoryHostPointerPropertiesEXT pMemoryHostPointerProperties)
        {
            throw new NotImplementedException();
        }

        public MgResult GetRayTracingShaderGroupHandlesNV(IMgPipeline pipeline, uint firstGroup, uint groupCount, UIntPtr dataSize, IntPtr[] pData)
        {
            throw new NotImplementedException();
        }

        public MgResult GetSemaphoreFdKHR(MgSemaphoreGetFdInfoKHR pGetFdInfo, ref int pFd)
        {
            throw new NotImplementedException();
        }

        public MgResult GetShaderInfoAMD(IMgPipeline pipeline, MgShaderStageFlagBits shaderStage, MgShaderInfoTypeAMD infoType, out UIntPtr pInfoSize, out IntPtr pInfo)
        {
            throw new NotImplementedException();
        }

        public MgResult GetSwapchainCounterEXT(IMgSwapchainKHR swapchain, MgSurfaceCounterFlagBitsEXT counter, ref ulong pCounterValue)
        {
            throw new NotImplementedException();
        }

        public MgResult GetSwapchainStatusKHR(IMgSwapchainKHR swapchain)
        {
            throw new NotImplementedException();
        }

        public MgResult GetValidationCacheDataEXT(IMgValidationCacheEXT validationCache, ref UIntPtr pDataSize, IntPtr[] pData)
        {
            throw new NotImplementedException();
        }

        public MgResult ImportFenceFdKHR(MgImportFenceFdInfoKHR pImportFenceFdInfo)
        {
            throw new NotImplementedException();
        }

        public MgResult ImportSemaphoreFdKHR(MgImportSemaphoreFdInfoKHR pImportSemaphoreFdInfo)
        {
            throw new NotImplementedException();
        }

        public MgResult MergeValidationCachesEXT(IMgValidationCacheEXT dstCache, IMgValidationCacheEXT[] pSrcCaches)
        {
            throw new NotImplementedException();
        }

        public MgResult RegisterDeviceEventEXT(MgDeviceEventInfoEXT pDeviceEventInfo, IntPtr pAllocator, IMgFence pFence)
        {
            throw new NotImplementedException();
        }

        public MgResult RegisterDisplayEventEXT(IMgDisplayKHR display, MgDisplayEventInfoEXT pDisplayEventInfo, IMgAllocationCallbacks pAllocator, IMgFence pFence)
        {
            throw new NotImplementedException();
        }

        public MgResult SetDebugUtilsObjectNameEXT(MgDebugUtilsObjectNameInfoEXT pNameInfo)
        {
            throw new NotImplementedException();
        }

        public MgResult SetDebugUtilsObjectTagEXT(MgDebugUtilsObjectTagInfoEXT pTagInfo)
        {
            throw new NotImplementedException();
        }

        public void GetAccelerationStructureMemoryRequirementsNV(MgAccelerationStructureMemoryRequirementsInfoNV pInfo, out MgMemoryRequirements2 pMemoryRequirements)
        {
            throw new NotImplementedException();
        }

        public void GetBufferMemoryRequirements2(MgBufferMemoryRequirementsInfo2 pInfo, out MgMemoryRequirements2 pMemoryRequirements)
        {
            throw new NotImplementedException();
        }

        public void GetDescriptorSetLayoutSupport(MgDescriptorSetLayoutCreateInfo pCreateInfo, out MgDescriptorSetLayoutSupport pSupport)
        {
            throw new NotImplementedException();
        }

        public void GetDeviceGroupPeerMemoryFeatures(uint heapIndex, uint localDeviceIndex, uint remoteDeviceIndex, out MgPeerMemoryFeatureFlagBits pPeerMemoryFeatures)
        {
            throw new NotImplementedException();
        }

        public void GetDeviceQueue2(MgDeviceQueueInfo2 pQueueInfo, out IMgQueue pQueue)
        {
            throw new NotImplementedException();
        }

        public void GetImageMemoryRequirements2(MgImageMemoryRequirementsInfo2 pInfo, out MgMemoryRequirements2 pMemoryRequirements)
        {
            throw new NotImplementedException();
        }

        public void GetImageSparseMemoryRequirements2(MgImageSparseMemoryRequirementsInfo2 pInfo, out MgSparseImageMemoryRequirements2[] pSparseMemoryRequirements)
        {
            throw new NotImplementedException();
        }

        public void SetHdrMetadataEXT(IMgSwapchainKHR[] pSwapchains, MgHdrMetadataEXT pMetadata)
        {
            throw new NotImplementedException();
        }

        public void TrimCommandPool(IMgCommandPool commandPool, uint flags)
        {
            throw new NotImplementedException();
        }

        public void UpdateDescriptorSetWithTemplate(IMgDescriptorSet descriptorSet, IMgDescriptorUpdateTemplate descriptorUpdateTemplate, IntPtr pData)
        {
            throw new NotImplementedException();
        }
    }
}
