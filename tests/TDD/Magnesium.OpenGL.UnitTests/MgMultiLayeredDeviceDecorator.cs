using System;

namespace Magnesium.OpenGL.UnitTests
{
    interface IMgDeviceValidationLayer
    {
        void GetDeviceProcAddr(IMgDevice element, string pName);
        void DestroyDevice(IMgDevice element, IMgAllocationCallbacks allocator);
        void GetDeviceQueue(IMgDevice element, UInt32 queueFamilyIndex, UInt32 queueIndex);
        void DeviceWaitIdle(IMgDevice element);
        void AllocateMemory(IMgDevice element, MgMemoryAllocateInfo pAllocateInfo, IMgAllocationCallbacks allocator);
        void FlushMappedMemoryRanges(IMgDevice element, MgMappedMemoryRange[] pMemoryRanges);
        void InvalidateMappedMemoryRanges(IMgDevice element, MgMappedMemoryRange[] pMemoryRanges);
        void GetDeviceMemoryCommitment(IMgDevice element, IMgDeviceMemory memory, ref UInt64 pCommittedMemoryInBytes);
        void GetBufferMemoryRequirements(IMgDevice element, IMgBuffer buffer);
        void GetImageMemoryRequirements(IMgDevice element, IMgImage image);
        void GetImageSparseMemoryRequirements(IMgDevice element, IMgImage image);
        void CreateFence(IMgDevice element, MgFenceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator);
        void ResetFences(IMgDevice element, IMgFence[] pFences);
        void GetFenceStatus(IMgDevice element, IMgFence fence);
        void WaitForFences(IMgDevice element, IMgFence[] pFences, bool waitAll, UInt64 timeout);
        void CreateSemaphore(IMgDevice element, MgSemaphoreCreateInfo pCreateInfo, IMgAllocationCallbacks allocator);
        void CreateEvent(IMgDevice element, MgEventCreateInfo pCreateInfo, IMgAllocationCallbacks allocator);
        void CreateQueryPool(IMgDevice element, MgQueryPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator);
        void GetQueryPoolResults(IMgDevice element, IMgQueryPool queryPool, UInt32 firstQuery, UInt32 queryCount, IntPtr dataSize, IntPtr pData, UInt64 stride, MgQueryResultFlagBits flags);
        void CreateBuffer(IMgDevice element, MgBufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator);
        void CreateBufferView(IMgDevice element, MgBufferViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator);
        void CreateImage(IMgDevice element, MgImageCreateInfo pCreateInfo, IMgAllocationCallbacks allocator);
        void GetImageSubresourceLayout(IMgDevice element, IMgImage image, MgImageSubresource pSubresource);
        void CreateImageView(IMgDevice element, MgImageViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator);
        void CreateShaderModule(IMgDevice element, MgShaderModuleCreateInfo pCreateInfo, IMgAllocationCallbacks allocator);
        void CreatePipelineCache(IMgDevice element, MgPipelineCacheCreateInfo pCreateInfo, IMgAllocationCallbacks allocator);
        void GetPipelineCacheData(IMgDevice element, IMgPipelineCache pipelineCache);
        void MergePipelineCaches(IMgDevice element, IMgPipelineCache dstCache, IMgPipelineCache[] pSrcCaches);
        void CreateGraphicsPipelines(IMgDevice element, IMgPipelineCache pipelineCache, MgGraphicsPipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator);
        void CreateComputePipelines(IMgDevice element, IMgPipelineCache pipelineCache, MgComputePipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator);
        void CreatePipelineLayout(IMgDevice element, MgPipelineLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator);
        void CreateSampler(IMgDevice element, MgSamplerCreateInfo pCreateInfo, IMgAllocationCallbacks allocator);
        void CreateDescriptorSetLayout(IMgDevice element, MgDescriptorSetLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator);
        void CreateDescriptorPool(IMgDevice element, MgDescriptorPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator);
        void AllocateDescriptorSets(IMgDevice element, MgDescriptorSetAllocateInfo pAllocateInfo);
        void FreeDescriptorSets(IMgDevice element, IMgDescriptorPool descriptorPool, IMgDescriptorSet[] pDescriptorSets);
        void UpdateDescriptorSets(IMgDevice element, MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies);
        void CreateFramebuffer(IMgDevice element, MgFramebufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator);
        void CreateRenderPass(IMgDevice element, MgRenderPassCreateInfo pCreateInfo, IMgAllocationCallbacks allocator);
        void GetRenderAreaGranularity(IMgDevice element, IMgRenderPass renderPass);
        void CreateCommandPool(IMgDevice element, MgCommandPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator);
        void AllocateCommandBuffers(IMgDevice element, MgCommandBufferAllocateInfo pAllocateInfo, IMgCommandBuffer[] pCommandBuffers);
        void FreeCommandBuffers(IMgDevice element, IMgCommandPool commandPool, IMgCommandBuffer[] pCommandBuffers);
        void CreateSharedSwapchainsKHR(IMgDevice element, MgSwapchainCreateInfoKHR[] pCreateInfos, IMgAllocationCallbacks allocator);
        void CreateSwapchainKHR(IMgDevice element, MgSwapchainCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator);
        void GetSwapchainImagesKHR(IMgDevice element, IMgSwapchainKHR swapchain);
        void AcquireNextImageKHR(IMgDevice element, IMgSwapchainKHR swapchain, UInt64 timeout, IMgSemaphore semaphore, IMgFence fence);
    }

    class MgMultiLayeredDeviceDecorator : IMgDevice
    {
        private IMgDevice mElement;
        private IMgDeviceValidationLayer[] mLayers;
        private IMgDestroyElementLogger mLogger;
        private bool mIsDestroyed;

        public MgMultiLayeredDeviceDecorator(
            IMgDevice element,
            IMgDeviceValidationLayer[] layers,
            IMgDestroyElementLogger logger
        )
        {
            mElement = element;
            mLayers = layers;
            mLogger = logger;
            mIsDestroyed = false;
        }

        public Result AcquireNextImageKHR(IMgSwapchainKHR swapchain, ulong timeout, IMgSemaphore semaphore, IMgFence fence, out uint pImageIndex)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.AcquireNextImageKHR(mElement, swapchain, timeout, semaphore, fence);
                }
            }
            return mElement.AcquireNextImageKHR(swapchain, timeout, semaphore, fence, out pImageIndex);
        }

        public Result AllocateCommandBuffers(MgCommandBufferAllocateInfo pAllocateInfo, IMgCommandBuffer[] pCommandBuffers)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.AllocateCommandBuffers(mElement, pAllocateInfo, pCommandBuffers);
                }
            }            
            return mElement.AllocateCommandBuffers(pAllocateInfo, pCommandBuffers);
        }

        public Result AllocateDescriptorSets(MgDescriptorSetAllocateInfo pAllocateInfo, out IMgDescriptorSet[] pDescriptorSets)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.AllocateDescriptorSets(mElement, pAllocateInfo);
                }
            }
            return mElement.AllocateDescriptorSets(pAllocateInfo, out pDescriptorSets);
        }

        public Result AllocateMemory(MgMemoryAllocateInfo pAllocateInfo, IMgAllocationCallbacks allocator, out IMgDeviceMemory pMemory)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.AllocateMemory(mElement, pAllocateInfo, allocator);
                }
            }
            return mElement.AllocateMemory(pAllocateInfo, allocator, out pMemory);
        }

        public Result CreateBuffer(MgBufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBuffer pBuffer)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.CreateBuffer(mElement, pCreateInfo, allocator);
                }
            }
            return mElement.CreateBuffer(pCreateInfo, allocator, out pBuffer);
        }

        public Result CreateBufferView(MgBufferViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBufferView pView)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.CreateBufferView(mElement, pCreateInfo, allocator);
                }
            }
            return mElement.CreateBufferView(pCreateInfo, allocator, out pView);
        }

        public Result CreateCommandPool(MgCommandPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgCommandPool pCommandPool)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.CreateCommandPool(mElement, pCreateInfo, allocator);
                }
            }
            return mElement.CreateCommandPool(pCreateInfo, allocator, out pCommandPool);
        }

        public Result CreateComputePipelines(IMgPipelineCache pipelineCache, MgComputePipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.CreateComputePipelines(mElement, pipelineCache, pCreateInfos, allocator);
                }
            }
            return mElement.CreateComputePipelines(pipelineCache, pCreateInfos, allocator, out pPipelines);
        }

        public Result CreateDescriptorPool(MgDescriptorPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorPool pDescriptorPool)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.CreateDescriptorPool(mElement, pCreateInfo, allocator);
                }
            }
            return mElement.CreateDescriptorPool(pCreateInfo, allocator, out pDescriptorPool);
        }

        public Result CreateDescriptorSetLayout(MgDescriptorSetLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorSetLayout pSetLayout)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.CreateDescriptorSetLayout(mElement, pCreateInfo, allocator);
                }
            }
            return mElement.CreateDescriptorSetLayout(pCreateInfo, allocator, out pSetLayout);
        }

        public Result CreateEvent(MgEventCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgEvent @event)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.CreateEvent(mElement, pCreateInfo, allocator);
                }
            }
            return mElement.CreateEvent(pCreateInfo, allocator, out @event);
        }

        public Result CreateFence(MgFenceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFence fence)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.CreateFence(mElement, pCreateInfo, allocator);
                }
            }
            return mElement.CreateFence(pCreateInfo, allocator, out fence);
        }

        public Result CreateFramebuffer(MgFramebufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFramebuffer pFramebuffer)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.CreateFramebuffer(mElement, pCreateInfo, allocator);
                }
            }
            return mElement.CreateFramebuffer(pCreateInfo, allocator, out pFramebuffer);
        }

        public Result CreateGraphicsPipelines(IMgPipelineCache pipelineCache, MgGraphicsPipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.CreateGraphicsPipelines(mElement, pipelineCache, pCreateInfos, allocator);
                }
            }
            return mElement.CreateGraphicsPipelines(pipelineCache, pCreateInfos, allocator, out pPipelines);
        }

        public Result CreateImage(MgImageCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgImage pImage)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.CreateImage(mElement, pCreateInfo, allocator);
                }
            }
            return mElement.CreateImage(pCreateInfo, allocator, out pImage);
        }

        public Result CreateImageView(MgImageViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgImageView pView)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.CreateImageView(mElement, pCreateInfo, allocator);
                }
            }
            return mElement.CreateImageView(pCreateInfo, allocator, out pView);
        }

        public Result CreatePipelineCache(MgPipelineCacheCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineCache pPipelineCache)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.CreatePipelineCache(mElement, pCreateInfo, allocator);
                }
            }
            return mElement.CreatePipelineCache(pCreateInfo, allocator, out pPipelineCache);
        }

        public Result CreatePipelineLayout(MgPipelineLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineLayout pPipelineLayout)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.CreatePipelineLayout(mElement, pCreateInfo, allocator);
                }
            }
            return mElement.CreatePipelineLayout(pCreateInfo, allocator, out pPipelineLayout);
        }

        public Result CreateQueryPool(MgQueryPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgQueryPool queryPool)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.CreateQueryPool(mElement, pCreateInfo, allocator);
                }
            }
            return mElement.CreateQueryPool(pCreateInfo, allocator, out queryPool);
        }

        public Result CreateRenderPass(MgRenderPassCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgRenderPass pRenderPass)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.CreateRenderPass(mElement, pCreateInfo, allocator);
                }
            }
            return mElement.CreateRenderPass(pCreateInfo, allocator, out pRenderPass);
        }

        public Result CreateSampler(MgSamplerCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSampler pSampler)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.CreateSampler(mElement, pCreateInfo, allocator);
                }
            }
            return mElement.CreateSampler(pCreateInfo, allocator, out pSampler);
        }

        public Result CreateSemaphore(MgSemaphoreCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSemaphore pSemaphore)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.CreateSemaphore(mElement, pCreateInfo, allocator);
                }
            }
            return mElement.CreateSemaphore(pCreateInfo, allocator, out pSemaphore);
        }

        public Result CreateShaderModule(MgShaderModuleCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgShaderModule pShaderModule)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.CreateShaderModule(mElement, pCreateInfo, allocator);
                }
            }
            return mElement.CreateShaderModule(pCreateInfo, allocator, out pShaderModule);
        }

        public Result CreateSharedSwapchainsKHR(MgSwapchainCreateInfoKHR[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgSwapchainKHR[] pSwapchains)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.CreateSharedSwapchainsKHR(mElement, pCreateInfos, allocator);
                }
            }
            return mElement.CreateSharedSwapchainsKHR(pCreateInfos, allocator, out pSwapchains);
        }

        public Result CreateSwapchainKHR(MgSwapchainCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSwapchainKHR pSwapchain)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.CreateSwapchainKHR(mElement, pCreateInfo, allocator);
                }
            }
            return mElement.CreateSwapchainKHR(pCreateInfo, allocator, out pSwapchain);
        }

        public void DestroyDevice(IMgAllocationCallbacks allocator)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.DestroyDevice(mElement, allocator);
                }
            }
            mElement.DestroyDevice(allocator);
            mIsDestroyed = true;
        }

        public Result DeviceWaitIdle()
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.DeviceWaitIdle(mElement);
                }
            }
            return mElement.DeviceWaitIdle();
        }

        public Result FlushMappedMemoryRanges(MgMappedMemoryRange[] pMemoryRanges)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.FlushMappedMemoryRanges(mElement, pMemoryRanges);
                }
            }
            return mElement.FlushMappedMemoryRanges(pMemoryRanges);
        }

        public void FreeCommandBuffers(IMgCommandPool commandPool, IMgCommandBuffer[] pCommandBuffers)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.FreeCommandBuffers(mElement, commandPool, pCommandBuffers);
                }
            }
            mElement.FreeCommandBuffers(commandPool, pCommandBuffers);
        }

        public Result FreeDescriptorSets(IMgDescriptorPool descriptorPool, IMgDescriptorSet[] pDescriptorSets)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.FreeDescriptorSets(mElement, descriptorPool, pDescriptorSets);
                }
            }
            return mElement.FreeDescriptorSets(descriptorPool, pDescriptorSets);
        }

        public void GetBufferMemoryRequirements(IMgBuffer buffer, out MgMemoryRequirements pMemoryRequirements)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetBufferMemoryRequirements(mElement, buffer);
                }
            }
            mElement.GetBufferMemoryRequirements(buffer, out pMemoryRequirements);
        }

        public void GetDeviceMemoryCommitment(IMgDeviceMemory memory, ref ulong pCommittedMemoryInBytes)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetDeviceMemoryCommitment(mElement, memory, ref pCommittedMemoryInBytes);
                }
            }
            mElement.GetDeviceMemoryCommitment(memory, ref pCommittedMemoryInBytes);
        }

        public IntPtr GetDeviceProcAddr(string pName)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetDeviceProcAddr(mElement, pName);
                }
            }
            return mElement.GetDeviceProcAddr(pName);
        }

        public void GetDeviceQueue(uint queueFamilyIndex, uint queueIndex, out IMgQueue pQueue)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetDeviceQueue(mElement, queueFamilyIndex, queueIndex);
                }
            }
            mElement.GetDeviceQueue(queueFamilyIndex, queueIndex, out pQueue);
        }

        public Result GetFenceStatus(IMgFence fence)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetFenceStatus(mElement, fence);
                }
            }
            return mElement.GetFenceStatus(fence);
        }

        public void GetImageMemoryRequirements(IMgImage image, out MgMemoryRequirements memoryRequirements)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetImageMemoryRequirements(mElement, image);
                }
            }
            mElement.GetImageMemoryRequirements(image, out memoryRequirements);
        }

        public void GetImageSparseMemoryRequirements(IMgImage image, out MgSparseImageMemoryRequirements[] sparseMemoryRequirements)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetImageSparseMemoryRequirements(mElement, image);
                }
            }
            mElement.GetImageSparseMemoryRequirements(image, out sparseMemoryRequirements);
        }

        public void GetImageSubresourceLayout(IMgImage image, MgImageSubresource pSubresource, out MgSubresourceLayout pLayout)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetImageSubresourceLayout(mElement, image, pSubresource);
                }
            }
            mElement.GetImageSubresourceLayout(image, pSubresource, out pLayout);
        }

        public Result GetPipelineCacheData(IMgPipelineCache pipelineCache, out byte[] pData)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetPipelineCacheData(mElement, pipelineCache);
                }
            }
            return mElement.GetPipelineCacheData(pipelineCache, out pData);
        }

        public Result GetQueryPoolResults(IMgQueryPool queryPool, uint firstQuery, uint queryCount, IntPtr dataSize, IntPtr pData, ulong stride, MgQueryResultFlagBits flags)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetQueryPoolResults(mElement, queryPool, firstQuery, queryCount, dataSize, pData, stride, flags);
                }
            }
            return mElement.GetQueryPoolResults(queryPool, firstQuery, queryCount, dataSize, pData, stride, flags);
        }

        public void GetRenderAreaGranularity(IMgRenderPass renderPass, out MgExtent2D pGranularity)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetRenderAreaGranularity(mElement, renderPass);
                }
            }
            mElement.GetRenderAreaGranularity(renderPass, out pGranularity);
        }

        public Result GetSwapchainImagesKHR(IMgSwapchainKHR swapchain, out IMgImage[] pSwapchainImages)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetSwapchainImagesKHR(mElement, swapchain);
                }
            }
            return mElement.GetSwapchainImagesKHR(swapchain, out pSwapchainImages);
        }

        public Result InvalidateMappedMemoryRanges(MgMappedMemoryRange[] pMemoryRanges)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.InvalidateMappedMemoryRanges(mElement, pMemoryRanges);
                }
            }
            return mElement.InvalidateMappedMemoryRanges(pMemoryRanges);
        }

        public Result MergePipelineCaches(IMgPipelineCache dstCache, IMgPipelineCache[] pSrcCaches)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.MergePipelineCaches(mElement, dstCache, pSrcCaches);
                }
            }
            return mElement.MergePipelineCaches(dstCache, pSrcCaches);
        }

        public Result ResetFences(IMgFence[] pFences)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.ResetFences(mElement, pFences);
                }
            }
            return mElement.ResetFences(pFences);
        }

        public void UpdateDescriptorSets(MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.UpdateDescriptorSets(mElement, pDescriptorWrites, pDescriptorCopies);
                }
            }
            mElement.UpdateDescriptorSets(pDescriptorWrites, pDescriptorCopies);
        }

        public Result WaitForFences(IMgFence[] pFences, bool waitAll, ulong timeout)
        {
            if (mLogger != null)
                mLogger.Debug(mIsDestroyed);
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.WaitForFences(mElement, pFences, waitAll, timeout);
                }
            }
            return mElement.WaitForFences(pFences, waitAll, timeout);
        }
    }
}
