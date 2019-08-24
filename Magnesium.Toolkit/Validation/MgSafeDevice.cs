using System;
namespace Magnesium.Toolkit
{
	public class MgSafeDevice : IMgDevice
	{
		internal IMgDevice mImpl = null;
		internal MgSafeDevice(IMgDevice impl)
		{
			mImpl = impl;
		}

		public MgResult GetFenceFdKHR(MgFenceGetFdInfoKHR pGetFdInfo, out Int32 pFd) {
			Validation.Device.GetFenceFdKHR.Validate(pGetFdInfo);
			return mImpl.GetFenceFdKHR(pGetFdInfo, out pFd);
		}

		public MgResult GetImageDrmFormatModifierPropertiesEXT(IMgImage image, out MgImageDrmFormatModifierPropertiesEXT pProperties) {
			Validation.Device.GetImageDrmFormatModifierPropertiesEXT.Validate(image);
			return mImpl.GetImageDrmFormatModifierPropertiesEXT(image, out pProperties);
		}

		public MgResult GetMemoryFdKHR(MgMemoryGetFdInfoKHR pGetFdInfo, ref Int32 pFd) {
			Validation.Device.GetMemoryFdKHR.Validate(pGetFdInfo, ref pFd);
			return mImpl.GetMemoryFdKHR(pGetFdInfo, ref pFd);
		}

		public MgResult GetMemoryFdPropertiesKHR(MgExternalMemoryHandleTypeFlagBits handleType, Int32 fd, out MgMemoryFdPropertiesKHR pMemoryFdProperties) {
			Validation.Device.GetMemoryFdPropertiesKHR.Validate(handleType, fd);
			return mImpl.GetMemoryFdPropertiesKHR(handleType, fd, out pMemoryFdProperties);
		}

		public MgResult GetMemoryHostPointerPropertiesEXT(MgExternalMemoryHandleTypeFlagBits handleType, IntPtr pHostPointer, out MgMemoryHostPointerPropertiesEXT pMemoryHostPointerProperties) {
			Validation.Device.GetMemoryHostPointerPropertiesEXT.Validate(handleType, pHostPointer);
			return mImpl.GetMemoryHostPointerPropertiesEXT(handleType, pHostPointer, out pMemoryHostPointerProperties);
		}

		public MgResult GetRayTracingShaderGroupHandlesNV(IMgPipeline pipeline, UInt32 firstGroup, UInt32 groupCount, UIntPtr dataSize, IntPtr[] pData) {
			Validation.Device.GetRayTracingShaderGroupHandlesNV.Validate(pipeline, firstGroup, groupCount, dataSize, pData);
			return mImpl.GetRayTracingShaderGroupHandlesNV(pipeline, firstGroup, groupCount, dataSize, pData);
		}

		public MgResult GetSemaphoreFdKHR(MgSemaphoreGetFdInfoKHR pGetFdInfo, ref Int32 pFd) {
			Validation.Device.GetSemaphoreFdKHR.Validate(pGetFdInfo, ref pFd);
			return mImpl.GetSemaphoreFdKHR(pGetFdInfo, ref pFd);
		}

		public MgResult GetShaderInfoAMD(IMgPipeline pipeline, MgShaderStageFlagBits shaderStage, MgShaderInfoTypeAMD infoType, out UIntPtr pInfoSize, out IntPtr pInfo) {
			Validation.Device.GetShaderInfoAMD.Validate(pipeline, shaderStage, infoType);
			return mImpl.GetShaderInfoAMD(pipeline, shaderStage, infoType, out pInfoSize, out pInfo);
		}

		public MgResult GetSwapchainCounterEXT(IMgSwapchainKHR swapchain, MgSurfaceCounterFlagBitsEXT counter, ref UInt64 pCounterValue) {
			Validation.Device.GetSwapchainCounterEXT.Validate(swapchain, counter, ref pCounterValue);
			return mImpl.GetSwapchainCounterEXT(swapchain, counter, ref pCounterValue);
		}

		public MgResult GetSwapchainStatusKHR(IMgSwapchainKHR swapchain) {
			Validation.Device.GetSwapchainStatusKHR.Validate(swapchain);
			return mImpl.GetSwapchainStatusKHR(swapchain);
		}

		public MgResult GetValidationCacheDataEXT(IMgValidationCacheEXT validationCache, ref UIntPtr pDataSize, IntPtr[] pData) {
			Validation.Device.GetValidationCacheDataEXT.Validate(validationCache, ref pDataSize, pData);
			return mImpl.GetValidationCacheDataEXT(validationCache, ref pDataSize, pData);
		}

		public MgResult ImportFenceFdKHR(MgImportFenceFdInfoKHR pImportFenceFdInfo) {
			Validation.Device.ImportFenceFdKHR.Validate(pImportFenceFdInfo);
			return mImpl.ImportFenceFdKHR(pImportFenceFdInfo);
		}

		public MgResult ImportSemaphoreFdKHR(MgImportSemaphoreFdInfoKHR pImportSemaphoreFdInfo) {
			Validation.Device.ImportSemaphoreFdKHR.Validate(pImportSemaphoreFdInfo);
			return mImpl.ImportSemaphoreFdKHR(pImportSemaphoreFdInfo);
		}

		public MgResult MergeValidationCachesEXT(IMgValidationCacheEXT dstCache, IMgValidationCacheEXT[] pSrcCaches) {
			Validation.Device.MergeValidationCachesEXT.Validate(dstCache, pSrcCaches);
			return mImpl.MergeValidationCachesEXT(dstCache, pSrcCaches);
		}

		public MgResult RegisterDeviceEventEXT(MgDeviceEventInfoEXT pDeviceEventInfo, IntPtr pAllocator, IMgFence pFence) {
			Validation.Device.RegisterDeviceEventEXT.Validate(pDeviceEventInfo, pAllocator, pFence);
			return mImpl.RegisterDeviceEventEXT(pDeviceEventInfo, pAllocator, pFence);
		}

		public MgResult RegisterDisplayEventEXT(IMgDisplayKHR display, MgDisplayEventInfoEXT pDisplayEventInfo, IMgAllocationCallbacks pAllocator, IMgFence pFence) {
			Validation.Device.RegisterDisplayEventEXT.Validate(display, pDisplayEventInfo, pAllocator, pFence);
			return mImpl.RegisterDisplayEventEXT(display, pDisplayEventInfo, pAllocator, pFence);
		}

		public MgResult SetDebugUtilsObjectNameEXT(MgDebugUtilsObjectNameInfoEXT pNameInfo) {
			Validation.Device.SetDebugUtilsObjectNameEXT.Validate(pNameInfo);
			return mImpl.SetDebugUtilsObjectNameEXT(pNameInfo);
		}

		public MgResult SetDebugUtilsObjectTagEXT(MgDebugUtilsObjectTagInfoEXT pTagInfo) {
			Validation.Device.SetDebugUtilsObjectTagEXT.Validate(pTagInfo);
			return mImpl.SetDebugUtilsObjectTagEXT(pTagInfo);
		}

		public void GetAccelerationStructureMemoryRequirementsNV(MgAccelerationStructureMemoryRequirementsInfoNV pInfo, out MgMemoryRequirements2 pMemoryRequirements) {
			Validation.Device.GetAccelerationStructureMemoryRequirementsNV.Validate(pInfo);
			mImpl.GetAccelerationStructureMemoryRequirementsNV(pInfo, out pMemoryRequirements);
		}

		public void GetBufferMemoryRequirements2(MgBufferMemoryRequirementsInfo2 pInfo, out MgMemoryRequirements2 pMemoryRequirements) {
			Validation.Device.GetBufferMemoryRequirements2.Validate(pInfo);
			mImpl.GetBufferMemoryRequirements2(pInfo, out pMemoryRequirements);
		}

		public void GetDescriptorSetLayoutSupport(MgDescriptorSetLayoutCreateInfo pCreateInfo, out MgDescriptorSetLayoutSupport pSupport) {
			Validation.Device.GetDescriptorSetLayoutSupport.Validate(pCreateInfo);
			mImpl.GetDescriptorSetLayoutSupport(pCreateInfo, out pSupport);
		}

		public void GetDeviceGroupPeerMemoryFeatures(UInt32 heapIndex, UInt32 localDeviceIndex, UInt32 remoteDeviceIndex, out MgPeerMemoryFeatureFlagBits pPeerMemoryFeatures) {
			Validation.Device.GetDeviceGroupPeerMemoryFeatures.Validate(heapIndex, localDeviceIndex, remoteDeviceIndex);
			mImpl.GetDeviceGroupPeerMemoryFeatures(heapIndex, localDeviceIndex, remoteDeviceIndex, out pPeerMemoryFeatures);
		}

		public void GetDeviceQueue2(MgDeviceQueueInfo2 pQueueInfo, out IMgQueue pQueue) {
			Validation.Device.GetDeviceQueue2.Validate(pQueueInfo);
			mImpl.GetDeviceQueue2(pQueueInfo, out IMgQueue tempQueue);

            pQueue = new MgSafeQueue(tempQueue);
        }

		public void GetImageMemoryRequirements2(MgImageMemoryRequirementsInfo2 pInfo, out MgMemoryRequirements2 pMemoryRequirements) {
			Validation.Device.GetImageMemoryRequirements2.Validate(pInfo);
			mImpl.GetImageMemoryRequirements2(pInfo, out pMemoryRequirements);
		}

		public void GetImageSparseMemoryRequirements2(MgImageSparseMemoryRequirementsInfo2 pInfo, out MgSparseImageMemoryRequirements2[] pSparseMemoryRequirements) {
			Validation.Device.GetImageSparseMemoryRequirements2.Validate(pInfo);
			mImpl.GetImageSparseMemoryRequirements2(pInfo, out pSparseMemoryRequirements);
		}

		public void SetHdrMetadataEXT(IMgSwapchainKHR[] pSwapchains, MgHdrMetadataEXT pMetadata) {
			Validation.Device.SetHdrMetadataEXT.Validate(pSwapchains, pMetadata);
			mImpl.SetHdrMetadataEXT(pSwapchains, pMetadata);
		}

		public void TrimCommandPool(IMgCommandPool commandPool, UInt32 flags) {
			Validation.Device.TrimCommandPool.Validate(commandPool, flags);
			mImpl.TrimCommandPool(commandPool, flags);
		}

		public void UpdateDescriptorSetWithTemplate(IMgDescriptorSet descriptorSet, IMgDescriptorUpdateTemplate descriptorUpdateTemplate, IntPtr pData) {
			Validation.Device.UpdateDescriptorSetWithTemplate.Validate(descriptorSet, descriptorUpdateTemplate, pData);
			mImpl.UpdateDescriptorSetWithTemplate(descriptorSet, descriptorUpdateTemplate, pData);
		}

		public IntPtr GetDeviceProcAddr(string pName) {
			Validation.Device.GetDeviceProcAddr.Validate(pName);
			return mImpl.GetDeviceProcAddr(pName);
		}

		public void DestroyDevice(IMgAllocationCallbacks allocator) {
			Validation.Device.DestroyDevice.Validate(allocator);
			mImpl.DestroyDevice(allocator);
		}

		public void GetDeviceQueue(UInt32 queueFamilyIndex, UInt32 queueIndex, out IMgQueue pQueue) {
			Validation.Device.GetDeviceQueue.Validate(queueFamilyIndex, queueIndex);
			mImpl.GetDeviceQueue(queueFamilyIndex, queueIndex, out IMgQueue tempQueue);
            pQueue = new MgSafeQueue(tempQueue);
		}

		public MgResult DeviceWaitIdle() {
			return mImpl.DeviceWaitIdle();
		}

		public MgResult AllocateMemory(MgMemoryAllocateInfo pAllocateInfo, IMgAllocationCallbacks allocator, out IMgDeviceMemory pMemory) {
			Validation.Device.AllocateMemory.Validate(pAllocateInfo, allocator);
			return mImpl.AllocateMemory(pAllocateInfo, allocator, out pMemory);
		}

		public MgResult FlushMappedMemoryRanges(MgMappedMemoryRange[] pMemoryRanges) {
			Validation.Device.FlushMappedMemoryRanges.Validate(pMemoryRanges);
			return mImpl.FlushMappedMemoryRanges(pMemoryRanges);
		}

		public MgResult InvalidateMappedMemoryRanges(MgMappedMemoryRange[] pMemoryRanges) {
			Validation.Device.InvalidateMappedMemoryRanges.Validate(pMemoryRanges);
			return mImpl.InvalidateMappedMemoryRanges(pMemoryRanges);
		}

		public void GetDeviceMemoryCommitment(IMgDeviceMemory memory, ref UInt64 pCommittedMemoryInBytes) {
			Validation.Device.GetDeviceMemoryCommitment.Validate(memory, ref pCommittedMemoryInBytes);
			mImpl.GetDeviceMemoryCommitment(memory, ref pCommittedMemoryInBytes);
		}

		public void GetBufferMemoryRequirements(IMgBuffer buffer, out MgMemoryRequirements pMemoryRequirements) {
			Validation.Device.GetBufferMemoryRequirements.Validate(buffer);
			mImpl.GetBufferMemoryRequirements(buffer, out pMemoryRequirements);
		}

		public void GetImageMemoryRequirements(IMgImage image, out MgMemoryRequirements memoryRequirements) {
			Validation.Device.GetImageMemoryRequirements.Validate(image);
			mImpl.GetImageMemoryRequirements(image, out memoryRequirements);
		}

		public void GetImageSparseMemoryRequirements(IMgImage image, out MgSparseImageMemoryRequirements[] sparseMemoryRequirements) {
			Validation.Device.GetImageSparseMemoryRequirements.Validate(image);
			mImpl.GetImageSparseMemoryRequirements(image, out sparseMemoryRequirements);
		}

		public MgResult CreateFence(MgFenceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFence fence) {
			Validation.Device.CreateFence.Validate(pCreateInfo, allocator);
			return mImpl.CreateFence(pCreateInfo, allocator, out fence);
		}

		public MgResult ResetFences(IMgFence[] pFences) {
			Validation.Device.ResetFences.Validate(pFences);
			return mImpl.ResetFences(pFences);
		}

		public MgResult GetFenceStatus(IMgFence fence) {
			Validation.Device.GetFenceStatus.Validate(fence);
			return mImpl.GetFenceStatus(fence);
		}

		public MgResult WaitForFences(IMgFence[] pFences, Boolean waitAll, UInt64 timeout) {
			Validation.Device.WaitForFences.Validate(pFences, waitAll, timeout);
			return mImpl.WaitForFences(pFences, waitAll, timeout);
		}

		public MgResult CreateSemaphore(MgSemaphoreCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSemaphore pSemaphore) {
			Validation.Device.CreateSemaphore.Validate(pCreateInfo, allocator);
			return mImpl.CreateSemaphore(pCreateInfo, allocator, out pSemaphore);
		}

		public MgResult CreateEvent(MgEventCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgEvent @event) {
			Validation.Device.CreateEvent.Validate(pCreateInfo, allocator);
			return mImpl.CreateEvent(pCreateInfo, allocator, out @event);
		}

		public MgResult CreateQueryPool(MgQueryPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgQueryPool queryPool) {
			Validation.Device.CreateQueryPool.Validate(pCreateInfo, allocator);
			return mImpl.CreateQueryPool(pCreateInfo, allocator, out queryPool);
		}

		public MgResult GetQueryPoolResults(IMgQueryPool queryPool, UInt32 firstQuery, UInt32 queryCount, IntPtr dataSize, IntPtr pData, UInt64 stride, MgQueryResultFlagBits flags) {
			Validation.Device.GetQueryPoolResults.Validate(queryPool, firstQuery, queryCount, dataSize, pData, stride, flags);
			return mImpl.GetQueryPoolResults(queryPool, firstQuery, queryCount, dataSize, pData, stride, flags);
		}

		public MgResult CreateBuffer(MgBufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBuffer pBuffer) {
			Validation.Device.CreateBuffer.Validate(pCreateInfo, allocator);
			return mImpl.CreateBuffer(pCreateInfo, allocator, out pBuffer);
		}

		public MgResult CreateBufferView(MgBufferViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBufferView pView) {
			Validation.Device.CreateBufferView.Validate(pCreateInfo, allocator);
			return mImpl.CreateBufferView(pCreateInfo, allocator, out pView);
		}

		public MgResult CreateImage(MgImageCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgImage pImage) {
			Validation.Device.CreateImage.Validate(pCreateInfo, allocator);
			return mImpl.CreateImage(pCreateInfo, allocator, out pImage);
		}

		public void GetImageSubresourceLayout(IMgImage image, MgImageSubresource pSubresource, out MgSubresourceLayout pLayout) {
			Validation.Device.GetImageSubresourceLayout.Validate(image, pSubresource);
			mImpl.GetImageSubresourceLayout(image, pSubresource, out pLayout);
		}

		public MgResult CreateImageView(MgImageViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgImageView pView) {
			Validation.Device.CreateImageView.Validate(pCreateInfo, allocator);
			return mImpl.CreateImageView(pCreateInfo, allocator, out pView);
		}

		public MgResult CreateShaderModule(MgShaderModuleCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgShaderModule pShaderModule) {
			Validation.Device.CreateShaderModule.Validate(pCreateInfo, allocator);
			return mImpl.CreateShaderModule(pCreateInfo, allocator, out pShaderModule);
		}

		public MgResult CreatePipelineCache(MgPipelineCacheCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineCache pPipelineCache) {
			Validation.Device.CreatePipelineCache.Validate(pCreateInfo, allocator);
			return mImpl.CreatePipelineCache(pCreateInfo, allocator, out pPipelineCache);
		}

		public MgResult GetPipelineCacheData(IMgPipelineCache pipelineCache, out Byte[] pData) {
			Validation.Device.GetPipelineCacheData.Validate(pipelineCache);
			return mImpl.GetPipelineCacheData(pipelineCache, out pData);
		}

		public MgResult MergePipelineCaches(IMgPipelineCache dstCache, IMgPipelineCache[] pSrcCaches) {
			Validation.Device.MergePipelineCaches.Validate(dstCache, pSrcCaches);
			return mImpl.MergePipelineCaches(dstCache, pSrcCaches);
		}

		public MgResult CreateGraphicsPipelines(IMgPipelineCache pipelineCache, MgGraphicsPipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines) {
			Validation.Device.CreateGraphicsPipelines.Validate(pipelineCache, pCreateInfos, allocator);
			return mImpl.CreateGraphicsPipelines(pipelineCache, pCreateInfos, allocator, out pPipelines);
		}

		public MgResult CreateComputePipelines(IMgPipelineCache pipelineCache, MgComputePipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines) {
			Validation.Device.CreateComputePipelines.Validate(pipelineCache, pCreateInfos, allocator);
			return mImpl.CreateComputePipelines(pipelineCache, pCreateInfos, allocator, out pPipelines);
		}

		public MgResult CreatePipelineLayout(MgPipelineLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineLayout pPipelineLayout) {
			Validation.Device.CreatePipelineLayout.Validate(pCreateInfo, allocator);
			return mImpl.CreatePipelineLayout(pCreateInfo, allocator, out pPipelineLayout);
		}

		public MgResult CreateSampler(MgSamplerCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSampler pSampler) {
			Validation.Device.CreateSampler.Validate(pCreateInfo, allocator);
			return mImpl.CreateSampler(pCreateInfo, allocator, out pSampler);
		}

		public MgResult CreateDescriptorSetLayout(MgDescriptorSetLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorSetLayout pSetLayout) {
			Validation.Device.CreateDescriptorSetLayout.Validate(pCreateInfo, allocator);
			return mImpl.CreateDescriptorSetLayout(pCreateInfo, allocator, out pSetLayout);
		}

		public MgResult CreateDescriptorPool(MgDescriptorPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorPool pDescriptorPool) {
			Validation.Device.CreateDescriptorPool.Validate(pCreateInfo, allocator);
			return mImpl.CreateDescriptorPool(pCreateInfo, allocator, out pDescriptorPool);
		}

		public MgResult AllocateDescriptorSets(MgDescriptorSetAllocateInfo pAllocateInfo, out IMgDescriptorSet[] pDescriptorSets) {
			Validation.Device.AllocateDescriptorSets.Validate(pAllocateInfo);
			return mImpl.AllocateDescriptorSets(pAllocateInfo, out pDescriptorSets);
		}

		public MgResult FreeDescriptorSets(IMgDescriptorPool descriptorPool, IMgDescriptorSet[] pDescriptorSets) {
			Validation.Device.FreeDescriptorSets.Validate(descriptorPool, pDescriptorSets);
			return mImpl.FreeDescriptorSets(descriptorPool, pDescriptorSets);
		}

		public void UpdateDescriptorSets(MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies) {
			Validation.Device.UpdateDescriptorSets.Validate(pDescriptorWrites, pDescriptorCopies);
			mImpl.UpdateDescriptorSets(pDescriptorWrites, pDescriptorCopies);
		}

		public MgResult CreateFramebuffer(MgFramebufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFramebuffer pFramebuffer) {
			Validation.Device.CreateFramebuffer.Validate(pCreateInfo, allocator);
			return mImpl.CreateFramebuffer(pCreateInfo, allocator, out pFramebuffer);
		}

		public MgResult CreateRenderPass(MgRenderPassCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgRenderPass pRenderPass) {
			Validation.Device.CreateRenderPass.Validate(pCreateInfo, allocator);
			return mImpl.CreateRenderPass(pCreateInfo, allocator, out pRenderPass);
		}

		public void GetRenderAreaGranularity(IMgRenderPass renderPass, out MgExtent2D pGranularity) {
			Validation.Device.GetRenderAreaGranularity.Validate(renderPass);
			mImpl.GetRenderAreaGranularity(renderPass, out pGranularity);
		}

		public MgResult CreateCommandPool(MgCommandPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgCommandPool pCommandPool) {
			Validation.Device.CreateCommandPool.Validate(pCreateInfo, allocator);
			return mImpl.CreateCommandPool(pCreateInfo, allocator, out pCommandPool);
		}

		public MgResult AllocateCommandBuffers(MgCommandBufferAllocateInfo pAllocateInfo, IMgCommandBuffer[] pCommandBuffers) {
			Validation.Device.AllocateCommandBuffers.Validate(pAllocateInfo, pCommandBuffers);

            var count = pCommandBuffers.Length;
            var tempCommandBuffers = new IMgCommandBuffer[count];
            var result = mImpl.AllocateCommandBuffers(pAllocateInfo, tempCommandBuffers);

            if (result != MgResult.SUCCESS)
            {
                return result;
            }

            for (var i = 0; i < count; i += 1)
            {
                pCommandBuffers[i] = new MgSafeCommandBuffer(tempCommandBuffers[i]);
            }
            return result;
		}

		public void FreeCommandBuffers(IMgCommandPool commandPool, IMgCommandBuffer[] pCommandBuffers) {
			Validation.Device.FreeCommandBuffers.Validate(commandPool, pCommandBuffers);
			mImpl.FreeCommandBuffers(commandPool, pCommandBuffers);
		}

		public MgResult CreateSharedSwapchainsKHR(MgSwapchainCreateInfoKHR[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgSwapchainKHR[] pSwapchains) {
			Validation.Device.CreateSharedSwapchainsKHR.Validate(pCreateInfos, allocator);
			return mImpl.CreateSharedSwapchainsKHR(pCreateInfos, allocator, out pSwapchains);
		}

		public MgResult CreateSwapchainKHR(MgSwapchainCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSwapchainKHR pSwapchain) {
			Validation.Device.CreateSwapchainKHR.Validate(pCreateInfo, allocator);
			return mImpl.CreateSwapchainKHR(pCreateInfo, allocator, out pSwapchain);
		}

		public MgResult GetSwapchainImagesKHR(IMgSwapchainKHR swapchain, out IMgImage[] pSwapchainImages) {
			Validation.Device.GetSwapchainImagesKHR.Validate(swapchain);
			return mImpl.GetSwapchainImagesKHR(swapchain, out pSwapchainImages);
		}

		public MgResult AcquireNextImageKHR(IMgSwapchainKHR swapchain, UInt64 timeout, IMgSemaphore semaphore, IMgFence fence, out UInt32 pImageIndex) {
			Validation.Device.AcquireNextImageKHR.Validate(swapchain, timeout, semaphore, fence);
			return mImpl.AcquireNextImageKHR(swapchain, timeout, semaphore, fence, out pImageIndex);
		}

		public MgResult CreateObjectTableNVX(MgObjectTableCreateInfoNVX pCreateInfo, IMgAllocationCallbacks allocator, out IMgObjectTableNVX pObjectTable) {
			Validation.Device.CreateObjectTableNVX.Validate(pCreateInfo, allocator);
			return mImpl.CreateObjectTableNVX(pCreateInfo, allocator, out pObjectTable);
		}

		public MgResult CreateIndirectCommandsLayoutNVX(MgIndirectCommandsLayoutCreateInfoNVX pCreateInfo, IMgAllocationCallbacks pAllocator, out IMgIndirectCommandsLayoutNVX pIndirectCommandsLayout) {
			Validation.Device.CreateIndirectCommandsLayoutNVX.Validate(pCreateInfo, pAllocator);
			return mImpl.CreateIndirectCommandsLayoutNVX(pCreateInfo, pAllocator, out pIndirectCommandsLayout);
		}

		public MgResult AcquireNextImage2KHR(MgAcquireNextImageInfoKHR pAcquireInfo, ref UInt32 pImageIndex) {
			Validation.Device.AcquireNextImage2KHR.Validate(pAcquireInfo, ref pImageIndex);
			return mImpl.AcquireNextImage2KHR(pAcquireInfo, ref pImageIndex);
		}

		public MgResult BindAccelerationStructureMemoryNV(MgBindAccelerationStructureMemoryInfoNV[] pBindInfos) {
			Validation.Device.BindAccelerationStructureMemoryNV.Validate(pBindInfos);
			return mImpl.BindAccelerationStructureMemoryNV(pBindInfos);
		}

		public MgResult BindBufferMemory2(MgBindBufferMemoryInfo[] pBindInfos) {
			Validation.Device.BindBufferMemory2.Validate(pBindInfos);
			return mImpl.BindBufferMemory2(pBindInfos);
		}

		public MgResult BindImageMemory2(MgBindImageMemoryInfo[] pBindInfos) {
			Validation.Device.BindImageMemory2.Validate(pBindInfos);
			return mImpl.BindImageMemory2(pBindInfos);
		}

		public MgResult CreateAccelerationStructureNV(MgAccelerationStructureCreateInfoNV pCreateInfo, IMgAllocationCallbacks pAllocator, out IMgAccelerationStructureNV pAccelerationStructure) {
			Validation.Device.CreateAccelerationStructureNV.Validate(pCreateInfo, pAllocator);
			return mImpl.CreateAccelerationStructureNV(pCreateInfo, pAllocator, out pAccelerationStructure);
		}

		public MgResult CreateDescriptorUpdateTemplate(MgDescriptorUpdateTemplateCreateInfo pCreateInfo, IMgAllocationCallbacks pAllocator, out IMgDescriptorUpdateTemplate pDescriptorUpdateTemplate) {
			Validation.Device.CreateDescriptorUpdateTemplate.Validate(pCreateInfo, pAllocator);
			return mImpl.CreateDescriptorUpdateTemplate(pCreateInfo, pAllocator, out pDescriptorUpdateTemplate);
		}

		public MgResult CreateRayTracingPipelinesNV(IMgPipelineCache pipelineCache, MgRayTracingPipelineCreateInfoNV[] pCreateInfos, IMgAllocationCallbacks pAllocator, out IMgPipeline[] pPipelines) {
			Validation.Device.CreateRayTracingPipelinesNV.Validate(pipelineCache, pCreateInfos, pAllocator);
			return mImpl.CreateRayTracingPipelinesNV(pipelineCache, pCreateInfos, pAllocator, out pPipelines);
		}

		public MgResult CreateRenderPass2KHR(MgRenderPassCreateInfo2KHR pCreateInfo, IMgAllocationCallbacks pAllocator, out IMgRenderPass pRenderPass) {
			Validation.Device.CreateRenderPass2KHR.Validate(pCreateInfo, pAllocator);
			return mImpl.CreateRenderPass2KHR(pCreateInfo, pAllocator, out pRenderPass);
		}

		public MgResult CreateSamplerYcbcrConversion(MgSamplerYcbcrConversionCreateInfo pCreateInfo, IMgAllocationCallbacks pAllocator, IMgSamplerYcbcrConversion pYcbcrConversion) {
			Validation.Device.CreateSamplerYcbcrConversion.Validate(pCreateInfo, pAllocator, pYcbcrConversion);
			return mImpl.CreateSamplerYcbcrConversion(pCreateInfo, pAllocator, pYcbcrConversion);
		}

		public MgResult CreateValidationCacheEXT(MgValidationCacheCreateInfoEXT pCreateInfo, IMgAllocationCallbacks pAllocator, IMgValidationCacheEXT pValidationCache) {
			Validation.Device.CreateValidationCacheEXT.Validate(pCreateInfo, pAllocator, pValidationCache);
			return mImpl.CreateValidationCacheEXT(pCreateInfo, pAllocator, pValidationCache);
		}

		public MgResult DisplayPowerControlEXT(IMgDisplayKHR display, out MgDisplayPowerInfoEXT pDisplayPowerInfo) {
			Validation.Device.DisplayPowerControlEXT.Validate(display);
			return mImpl.DisplayPowerControlEXT(display, out pDisplayPowerInfo);
		}

		public MgResult GetAccelerationStructureHandleNV(IMgAccelerationStructureNV accelerationStructure, UIntPtr dataSize, out IntPtr pData) {
			Validation.Device.GetAccelerationStructureHandleNV.Validate(accelerationStructure, dataSize);
			return mImpl.GetAccelerationStructureHandleNV(accelerationStructure, dataSize, out pData);
		}

		public MgResult GetCalibratedTimestampsEXT(MgCalibratedTimestampInfoEXT[] pTimestampInfos, out UInt64[] pTimestamps, out UInt64 pMaxDeviation) {
			Validation.Device.GetCalibratedTimestampsEXT.Validate(pTimestampInfos);
			return mImpl.GetCalibratedTimestampsEXT(pTimestampInfos, out pTimestamps, out pMaxDeviation);
		}

		public MgResult GetDeviceGroupPresentCapabilitiesKHR(out MgDeviceGroupPresentCapabilitiesKHR pDeviceGroupPresentCapabilities) {
			return mImpl.GetDeviceGroupPresentCapabilitiesKHR(out pDeviceGroupPresentCapabilities);
		}

		public MgResult GetDeviceGroupSurfacePresentModesKHR(IMgSurfaceKHR surface, out MgDeviceGroupPresentModeFlagBitsKHR pModes) {
			Validation.Device.GetDeviceGroupSurfacePresentModesKHR.Validate(surface);
			return mImpl.GetDeviceGroupSurfacePresentModesKHR(surface, out pModes);
		}

	}
}
