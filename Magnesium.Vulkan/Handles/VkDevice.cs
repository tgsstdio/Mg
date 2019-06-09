using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections.Generic;
using Magnesium.Vulkan.Functions.Device;

namespace Magnesium.Vulkan
{
    public class VkDevice : IMgDevice
	{
		readonly VkDeviceInfo info;
        public VkDeviceInfo Info { get => info; }

        internal VkDevice(IntPtr handle)
		{
			info = new VkDeviceInfo(handle);
		}

		public PFN_vkVoidFunction GetDeviceProcAddr(string pName)
		{
			return VkGetDeviceProcAddrSection.GetDeviceProcAddr(info, pName);
		}

		public void DestroyDevice(IMgAllocationCallbacks allocator)
		{
            VkDestroyDeviceSection.DestroyDevice(info, allocator);
		}

		public void GetDeviceQueue(uint queueFamilyIndex, uint queueIndex, out IMgQueue pQueue)
		{
            VkGetDeviceQueueSection.GetDeviceQueue(info, queueFamilyIndex, queueIndex, out pQueue);
		}

		public MgResult DeviceWaitIdle()
		{
			return VkDeviceWaitIdleSection.DeviceWaitIdle(info);
		}

		public MgResult AllocateMemory(MgMemoryAllocateInfo pAllocateInfo, IMgAllocationCallbacks allocator, out IMgDeviceMemory pMemory)
		{
            return VkAllocateMemorySection.AllocateMemory(info, pAllocateInfo, allocator, out pMemory);
        }

		public MgResult FlushMappedMemoryRanges(MgMappedMemoryRange[] pMemoryRanges)
		{
            return VkFlushMappedMemoryRangesSection.FlushMappedMemoryRanges(info, pMemoryRanges);
		}

		public MgResult InvalidateMappedMemoryRanges(MgMappedMemoryRange[] pMemoryRanges)
		{
            return VkInvalidateMappedMemoryRangesSection.InvalidateMappedMemoryRanges(info, pMemoryRanges);
        }

		public void GetDeviceMemoryCommitment(IMgDeviceMemory memory, ref ulong pCommittedMemoryInBytes)
		{
            VkGetDeviceMemoryCommitmentSection.GetDeviceMemoryCommitment(info, memory, ref pCommittedMemoryInBytes);
        }

		public void GetBufferMemoryRequirements(IMgBuffer buffer, out MgMemoryRequirements pMemoryRequirements)
		{
             VkGetBufferMemoryRequirementsSection.GetBufferMemoryRequirements(info, buffer, out pMemoryRequirements);
		}

		public void GetImageMemoryRequirements(IMgImage image, out MgMemoryRequirements memoryRequirements)
        {
            VkGetImageMemoryRequirementsSection.GetImageMemoryRequirements(info, image, out memoryRequirements);
        }

        public void GetImageSparseMemoryRequirements(IMgImage image, out MgSparseImageMemoryRequirements[] sparseMemoryRequirements)
        {
            VkGetImageSparseMemoryRequirementsSection.GetImageSparseMemoryRequirements(info, image, out sparseMemoryRequirements);
        }

        public MgResult CreateFence(MgFenceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFence fence)
        {
            return VkCreateFenceSection.CreateFence(info, pCreateInfo, allocator, out fence);
        }

        public MgResult ResetFences(IMgFence[] pFences)
        {
            return VkResetFencesSection.ResetFences(info, pFences);
        }

        public MgResult GetFenceStatus(IMgFence fence)
        {
            return VkGetFenceStatusSection.GetFenceStatus(info, fence);
        }

        public MgResult WaitForFences(IMgFence[] pFences, bool waitAll, ulong timeout)
        {
            return VkWaitForFencesSection.WaitForFences(info, pFences, waitAll, timeout);
        }

        public MgResult CreateSemaphore(MgSemaphoreCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSemaphore pSemaphore)
        {
            return VkCreateSemaphoreSection.CreateSemaphore(info, pCreateInfo, allocator, out pSemaphore);
        }

        public MgResult CreateEvent(MgEventCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgEvent @event)
        {
            return VkCreateEventSection.CreateEvent(info, pCreateInfo, allocator, out @event);
        }

        public MgResult CreateQueryPool(MgQueryPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgQueryPool queryPool)
        {
            return VkCreateQueryPoolSection.CreateQueryPool(info, pCreateInfo, allocator, out queryPool);
        }

        public MgResult GetQueryPoolResults(IMgQueryPool queryPool, uint firstQuery, uint queryCount, IntPtr dataSize, IntPtr pData, ulong stride, MgQueryResultFlagBits flags)
        {
            return VkGetQueryPoolResultsSection.GetQueryPoolResults(info, queryPool, firstQuery, queryCount, dataSize, pData, stride, flags);
        }

        public MgResult CreateBuffer(MgBufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBuffer pBuffer)
        {
            return VkCreateBufferSection.CreateBuffer(info, pCreateInfo, allocator, out pBuffer);
        }

        public MgResult CreateBufferView(MgBufferViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBufferView pView)
        {
            return VkCreateBufferViewSection.CreateBufferView(info, pCreateInfo, allocator, out pView);
        }

        public MgResult CreateImage(MgImageCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgImage pImage)
        {
            return VkCreateImageSection.CreateImage(info, pCreateInfo, allocator, out pImage);
        }

        public void GetImageSubresourceLayout(IMgImage image, MgImageSubresource pSubresource, out MgSubresourceLayout pLayout)
        {
            VkGetImageSubresourceLayoutSection.GetImageSubresourceLayout(info, image, pSubresource, out pLayout);
        }

        public MgResult CreateImageView(MgImageViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgImageView pView)
        {
            return VkCreateImageViewSection.CreateImageView(info, pCreateInfo, allocator, out pView);
        }

        public MgResult CreateShaderModule(MgShaderModuleCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgShaderModule pShaderModule)
        {
            return VkCreateShaderModuleSection.CreateShaderModule(info, pCreateInfo, allocator, out pShaderModule);
        }

        public MgResult CreatePipelineCache(MgPipelineCacheCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineCache pPipelineCache)
        {
            return VkCreatePipelineCacheSection.CreatePipelineCache(info, pCreateInfo, allocator, out pPipelineCache);
        }

        public MgResult GetPipelineCacheData(IMgPipelineCache pipelineCache, out byte[] pData)
        {
            return VkGetPipelineCacheDataSection.GetPipelineCacheData(info, pipelineCache, out pData);
        }

        public MgResult MergePipelineCaches(IMgPipelineCache dstCache, IMgPipelineCache[] pSrcCaches)
        {
            return VkMergePipelineCachesSection.MergePipelineCaches(info, dstCache, pSrcCaches);
        }

        public MgResult CreateGraphicsPipelines(IMgPipelineCache pipelineCache, MgGraphicsPipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines)
        {
            return VkCreateGraphicsPipelinesSection.CreateGraphicsPipelines(info, pipelineCache, pCreateInfos, allocator, out pPipelines);
        }

        public MgResult CreateComputePipelines(IMgPipelineCache pipelineCache, MgComputePipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines)
        {
            return VkCreateComputePipelinesSection.CreateComputePipelines(info, pipelineCache, pCreateInfos, allocator, out pPipelines);
        }

        public MgResult CreatePipelineLayout(MgPipelineLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineLayout pPipelineLayout)
        {
            return VkCreatePipelineLayoutSection.CreatePipelineLayout(info, pCreateInfo, allocator, out pPipelineLayout);
        }

        public MgResult CreateSampler(MgSamplerCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSampler pSampler)
        {
            return VkCreateSamplerSection.CreateSampler(info, pCreateInfo, allocator, out pSampler);
        }

        public MgResult CreateDescriptorSetLayout(MgDescriptorSetLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorSetLayout pSetLayout)
        {
            return VkCreateDescriptorSetLayoutSection.CreateDescriptorSetLayout(info, pCreateInfo, allocator, out pSetLayout);
        }

        public MgResult CreateDescriptorPool(MgDescriptorPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorPool pDescriptorPool)
        {
            return VkCreateDescriptorPoolSection.CreateDescriptorPool(info, pCreateInfo, allocator, out pDescriptorPool);
        }

        public MgResult AllocateDescriptorSets(MgDescriptorSetAllocateInfo pAllocateInfo, out IMgDescriptorSet[] pDescriptorSets)
        {
            return VkAllocateDescriptorSetsSection.AllocateDescriptorSets(info, pAllocateInfo, out pDescriptorSets);
        }

        public MgResult FreeDescriptorSets(IMgDescriptorPool descriptorPool, IMgDescriptorSet[] pDescriptorSets)
        {
            return VkFreeDescriptorSetsSection.FreeDescriptorSets(info, descriptorPool, pDescriptorSets);
        }

        public void UpdateDescriptorSets(MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies)
        {
            VkUpdateDescriptorSetsSection.UpdateDescriptorSets(info, pDescriptorWrites, pDescriptorCopies);
        }

        public MgResult CreateFramebuffer(MgFramebufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFramebuffer pFramebuffer)
        {
            return VkCreateFramebufferSection.CreateFramebuffer(info, pCreateInfo, allocator, out pFramebuffer);
        }

        public MgResult CreateRenderPass(MgRenderPassCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgRenderPass pRenderPass)
        {
            return VkCreateRenderPassSection.CreateRenderPass(info, pCreateInfo, allocator, out pRenderPass);
        }

        public void GetRenderAreaGranularity(IMgRenderPass renderPass, out MgExtent2D pGranularity)
        {
            VkGetRenderAreaGranularitySection.GetRenderAreaGranularity(info, renderPass, out pGranularity);
        }

        public MgResult CreateCommandPool(MgCommandPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgCommandPool pCommandPool)
        {
            return VkCreateCommandPoolSection.CreateCommandPool(info, pCreateInfo, allocator, out pCommandPool);
        }

        public MgResult AllocateCommandBuffers(MgCommandBufferAllocateInfo pAllocateInfo, IMgCommandBuffer[] pCommandBuffers)
        {
            return VkAllocateCommandBuffersSection.AllocateCommandBuffers(info, pAllocateInfo, pCommandBuffers);
        }

        public void FreeCommandBuffers(IMgCommandPool commandPool, IMgCommandBuffer[] pCommandBuffers)
        {
            VkFreeCommandBuffersSection.FreeCommandBuffers(info, commandPool, pCommandBuffers);
        }

        public MgResult CreateSharedSwapchainsKHR(MgSwapchainCreateInfoKHR[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgSwapchainKHR[] pSwapchains)
        {
            return VkCreateSharedSwapchainsKHRSection.CreateSharedSwapchainsKHR(info, pCreateInfos, allocator, out pSwapchains);
        }

        public MgResult CreateSwapchainKHR(MgSwapchainCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSwapchainKHR pSwapchain)
        {
            return VkCreateSwapchainKHRSection.CreateSwapchainKHR(info, pCreateInfo, allocator, out pSwapchain);
        }

        public MgResult GetSwapchainImagesKHR(IMgSwapchainKHR swapchain, out IMgImage[] pSwapchainImages)
        {
            return VkGetSwapchainImagesKHRSection.GetSwapchainImagesKHR(info, swapchain, out pSwapchainImages);
        }

        public MgResult AcquireNextImageKHR(IMgSwapchainKHR swapchain, ulong timeout, IMgSemaphore semaphore, IMgFence fence, out uint pImageIndex)
        {
            return VkAcquireNextImageKHRSection.AcquireNextImageKHR(info, swapchain, timeout, semaphore, fence, out pImageIndex);
        }

        public MgResult CreateObjectTableNVX(MgObjectTableCreateInfoNVX pCreateInfo, IMgAllocationCallbacks allocator, out IMgObjectTableNVX pObjectTable)
        {
            return VkCreateObjectTableNVXSection.CreateObjectTableNVX(info, pCreateInfo, allocator, out pObjectTable);
        }

        public MgResult CreateIndirectCommandsLayoutNVX(MgIndirectCommandsLayoutCreateInfoNVX pCreateInfo, IMgAllocationCallbacks allocator, out IMgIndirectCommandsLayoutNVX pIndirectCommandsLayout)
        {
            return VkCreateIndirectCommandsLayoutNVXSection.CreateIndirectCommandsLayoutNVX(info, pCreateInfo, allocator, out pIndirectCommandsLayout);
        }

        public MgResult AcquireNextImage2KHR(MgAcquireNextImageInfoKHR pAcquireInfo, ref uint pImageIndex)
        {
            return VkAcquireNextImage2KHRSection.AcquireNextImage2KHR(info, pAcquireInfo, ref pImageIndex);
        }

        public MgResult BindAccelerationStructureMemoryNV(MgBindAccelerationStructureMemoryInfoNV[] pBindInfos)
        {
            return VkBindAccelerationStructureMemoryNVSection.BindAccelerationStructureMemoryNV(info, pBindInfos);
        }

        public MgResult BindBufferMemory2(MgBindBufferMemoryInfo[] pBindInfos)
        {
            if (pBindInfos == null)
                throw new ArgumentNullException(nameof(pBindInfos));

            var bindInfoCount = (UInt32)pBindInfos.Length;

            var bBindInfos = new VkBindBufferMemoryInfo[bindInfoCount];

            for (var i = 0; i < bindInfoCount; i += 1)
            {
                var currentInfo = pBindInfos[i];

                var bBuffer = (VkBuffer)currentInfo.Memory;
                var bBufferPtr = bBuffer != null ? bBuffer.Handle : 0UL;

                var bDeviceMemory = (VkDeviceMemory)currentInfo.Memory;
                var bDeviceMemoryPtr = bDeviceMemory != null ? bDeviceMemory.Handle : 0UL;

                bBindInfos[i] = new VkBindBufferMemoryInfo
                {
                    sType = VkStructureType.StructureTypeBindBufferMemoryInfo,
                    // TODO: extensible
                    pNext = IntPtr.Zero,
                    buffer = bBufferPtr,
                    memory = bDeviceMemoryPtr,
                    memoryOffset = currentInfo.MemoryOffset,
                };
            }

            return Interops.vkBindBufferMemory2(info.Handle, bindInfoCount, bBindInfos);
        }

        public MgResult BindImageMemory2(MgBindImageMemoryInfo[] pBindInfos)
        {
            if (pBindInfos == null)
                throw new ArgumentNullException(nameof(pBindInfos));

            var bindInfoCount = (UInt32)pBindInfos.Length;

            var bBindInfos = new VkBindImageMemoryInfo[bindInfoCount];

            for (var i = 0; i < bindInfoCount; i += 1)
            {
                var currentInfo = pBindInfos[i];

                var bImage = (VkImage)currentInfo.Image;
                var bImagePtr = bImage != null ? bImage.Handle : 0UL;

                var bDeviceMemory = (VkDeviceMemory)currentInfo.Memory;
                var bDeviceMemoryPtr = bDeviceMemory != null ? bDeviceMemory.Handle : 0UL;

                bBindInfos[i] = new VkBindImageMemoryInfo
                {
                    sType = VkStructureType.StructureTypeBindImageMemoryInfo,
                    // TODO: extensible
                    pNext = IntPtr.Zero,
                    image = bImagePtr,
                    memory = bDeviceMemoryPtr,
                    memoryOffset = currentInfo.MemoryOffset,
                }; 
            }

            return Interops.vkBindImageMemory2(info.Handle, bindInfoCount, bBindInfos);
        }

        public MgResult CreateAccelerationStructureNV(MgAccelerationStructureCreateInfoNV pCreateInfo, IMgAllocationCallbacks pAllocator, out IMgAccelerationStructureNV pAccelerationStructure)
        {
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));

            if (pCreateInfo.Info == null)
                throw new ArgumentNullException(nameof(pCreateInfo.Info));

            var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(pAllocator);

            var geometryCount = (pCreateInfo.Info.Geometries != null) 
                ? (uint) pCreateInfo.Info.Geometries.Length
                : 0U;

            var pGeometries = IntPtr.Zero;

            try
            {

                pGeometries = VkInteropsUtility.AllocateHGlobalArray<MgGeometryNV, VkGeometryNV>(
                        pCreateInfo.Info.Geometries,
                        (src) =>
                        {
                            return new VkGeometryNV
                            {
                                sType = VkStructureType.StructureTypeGeometryNv,
                                pNext = IntPtr.Zero,
                                flags = src.flags,
                                geometry = new VkGeometryDataNV
                                {
                                    aabbs = ExtractAabbs(src.geometry.aabbs),
                                    triangles = ExtractTriangleData(src.geometry.triangles)
                                },
                                geometryType = src.geometryType,
                            };
                        }
                    );

                var bCreateInfo = new VkAccelerationStructureCreateInfoNV
                {
                    sType = VkStructureType.StructureTypeAccelerationStructureCreateInfoNv,
                    // TODO: extensible
                    pNext = IntPtr.Zero,
                    compactedSize = pCreateInfo.CompactedSize,
                    info = new VkAccelerationStructureInfoNV
                    {
                        sType = VkStructureType.StructureTypeAccelerationStructureInfoNv,
                        // TODO: extensible
                        pNext = IntPtr.Zero,
                        type = pCreateInfo.Info.Type,
                        flags = pCreateInfo.Info.Flags,
                        instanceCount = pCreateInfo.Info.InstanceCount,
                        geometryCount = geometryCount,
                        pGeometries = pGeometries,
                    },
                };

                var pHandle = 0UL;
                var result = Interops.vkCreateAccelerationStructureNV(info.Handle, ref bCreateInfo, allocatorPtr, ref pHandle);
                pAccelerationStructure = new VkAccelerationStructureNV(pHandle);
                return result;
            }
            finally
            {
                if (pGeometries != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pGeometries);
                }
            }
        }

        private static VkGeometryTrianglesNV ExtractTriangleData(MgGeometryTrianglesNV src)
        {
            var bVertexData = (VkBuffer) src.VertexData;

            var bIndexData = (VkBuffer)src.IndexData;

            var bTransformData = (VkBuffer)src.TransformData;

            return new VkGeometryTrianglesNV
            {
                sType = VkStructureType.StructureTypeGeometryTrianglesNv,
                pNext = IntPtr.Zero,
                vertexData = bVertexData.Handle,
                vertexOffset = src.VertexOffset,
                vertexCount = src.VertexCount,
                vertexStride = src.VertexStride,
                vertexFormat = src.VertexFormat,
                indexData = bIndexData.Handle,
                indexOffset = src.IndexOffset,
                indexCount = src.IndexCount,
                indexType = src.IndexType,
                
                transformData = bTransformData.Handle,
                transformOffset = src.TransformOffset,
            };
        }

        private static VkGeometryAABBNV ExtractAabbs(MgGeometryAABBNV aabbs)
        {
            var bAabbData = (VkBuffer) aabbs.AabbData;

            return new VkGeometryAABBNV
            {
                sType = VkStructureType.StructureTypeGeometryAabbNv,
                pNext = IntPtr.Zero,
                aabbData = bAabbData.Handle,
                numAABBs = aabbs.NumAABBs,
                offset = aabbs.Offset,
                stride = aabbs.Stride,
            };
        }

        public MgResult CreateDescriptorUpdateTemplate(MgDescriptorUpdateTemplateCreateInfo pCreateInfo, IMgAllocationCallbacks pAllocator, out IMgDescriptorUpdateTemplate pDescriptorUpdateTemplate)
        {
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));

            if (pCreateInfo.DescriptorUpdateEntries == null)
                throw new ArgumentNullException(nameof(pCreateInfo.DescriptorUpdateEntries));


            var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(pAllocator);

            var descriptorUpdateEntryCount = (UInt32) pCreateInfo.DescriptorUpdateEntries.Length;

            var pDescriptorUpdateEntries = IntPtr.Zero;

            try
            {
                pDescriptorUpdateEntries = VkInteropsUtility.AllocateHGlobalArray(
                    pCreateInfo.DescriptorUpdateEntries,
                    (src) =>
                    {
                        return new VkDescriptorUpdateTemplateEntry
                        {
                            dstBinding = src.DstBinding,
                            dstArrayElement = src.DstArrayElement,
                            descriptorCount = src.DescriptorCount,
                            descriptorType = src.DescriptorType,
                            offset = src.Offset,
                            stride = src.Stride,
                        };
                    }
                );

                var bSetLayout = (VkDescriptorSetLayout)pCreateInfo.DescriptorSetLayout;
                var bSetLayoutPtr = bSetLayout != null ? bSetLayout.Handle : 0UL;

                var bPipelineLayout = (VkPipelineLayout)pCreateInfo.PipelineLayout;
                var bPipelineLayoutPtr = bPipelineLayout != null ? bPipelineLayout.Handle : 0UL;

                var bCreateInfo = new VkDescriptorUpdateTemplateCreateInfo
                {
                    sType = VkStructureType.StructureTypeDescriptorUpdateTemplateCreateInfo,
                    pNext = IntPtr.Zero,
                    flags = pCreateInfo.Flags,
                    descriptorUpdateEntryCount = descriptorUpdateEntryCount,
                    pDescriptorUpdateEntries = pDescriptorUpdateEntries,
                    templateType = pCreateInfo.TemplateType,
                    descriptorSetLayout = bSetLayoutPtr,
                    pipelineBindPoint = pCreateInfo.PipelineBindPoint,
                    pipelineLayout = bPipelineLayoutPtr,
                    set = pCreateInfo.Set,
                };

                var pHandle = 0UL;
                var result = Interops.vkCreateDescriptorUpdateTemplate(info.Handle, bCreateInfo, allocatorPtr, ref pHandle);

                pDescriptorUpdateTemplate = new VkDescriptorUpdateTemplate(pHandle);
                return result;
            }
            finally
            {
                if (pDescriptorUpdateEntries != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pDescriptorUpdateEntries);
                }
            }            
        }

        public MgResult CreateRayTracingPipelinesNV(IMgPipelineCache pipelineCache, MgRayTracingPipelineCreateInfoNV[] pCreateInfos, IMgAllocationCallbacks pAllocator, out IMgPipeline[] pPipelines)
        {
            return VkCreateRayTracingPipelinesNVSection.CreateRayTracingPipelinesNV(info, pipelineCache, pCreateInfos, pAllocator, out pPipelines);
        }

        public MgResult CreateRenderPass2KHR(MgRenderPassCreateInfo2KHR pCreateInfo, IMgAllocationCallbacks pAllocator, out IMgRenderPass pRenderPass)
        {
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));

            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(pAllocator);

            var allocatedItems = new List<IntPtr>();

            try
            {
                var pAttachments = IntPtr.Zero;
                var attachmentCount = pCreateInfo.Attachments != null
                    ? (uint)pCreateInfo.Attachments.Length 
                    : 0U;
                if (attachmentCount > 0)
                {
                    pAttachments = VkInteropsUtility.AllocateHGlobalArray(
                        pCreateInfo.Attachments,
                        (attachment) =>
                        {
                            return new VkAttachmentDescription2KHR
                            {
                                sType = VkStructureType.StructureTypeAttachmentDescription2Khr,
                                // TODO: extension here
                                pNext = IntPtr.Zero,
                                flags = attachment.Flags,
                                format = attachment.Format,
                                samples = attachment.Samples,
                                loadOp = (VkAttachmentLoadOp)attachment.LoadOp,
                                storeOp = (VkAttachmentStoreOp)attachment.StoreOp,
                                stencilLoadOp = (VkAttachmentLoadOp)attachment.StencilLoadOp,
                                stencilStoreOp = (VkAttachmentStoreOp)attachment.StencilStoreOp,
                                initialLayout = attachment.InitialLayout,
                                finalLayout = attachment.FinalLayout
                            };
                        });
                    allocatedItems.Add(pAttachments);
                }

                var subpassCount = pCreateInfo.Subpasses != null
                    ? (uint)pCreateInfo.Subpasses.Length 
                    : 0U;
                var pSubpasses = IntPtr.Zero;
                if (subpassCount > 0)
                {
                    pSubpasses = VkInteropsUtility.AllocateHGlobalArray
                        <MgSubpassDescription2KHR, VkSubpassDescription2KHR>
                        (
                            pCreateInfo.Subpasses,
                            (src) =>
                            {
                                var inputAttachmentCount =
                                    src.InputAttachments != null
                                    ? (uint)src.InputAttachments.Length
                                    : 0U;

                                var pInputAttachments = IntPtr.Zero;
                                if (inputAttachmentCount > 0)
                                {
                                    pInputAttachments = VkInteropsUtility.AllocateHGlobalArray
                                        <MgAttachmentReference2KHR, VkAttachmentReference2KHR>
                                        (
                                            src.InputAttachments,
                                            (input) =>
                                            {
                                                return new VkAttachmentReference2KHR
                                                {
                                                    sType = VkStructureType.StructureTypeAttachmentReference2Khr,
                                                    // TODO: extension here
                                                    pNext = IntPtr.Zero,
                                                    attachment = input.Attachment,
                                                    layout = input.Layout,
                                                    aspectMask = input.AspectMask,
                                                };
                                            }
                                        );
                                    allocatedItems.Add(pInputAttachments);
                                }

                                var colorAttachmentCount =
                                    src.ColorAndResolveAttachments != null
                                    ? (uint)src.ColorAndResolveAttachments.Length
                                    : 0U;
                                var pColorAttachments = IntPtr.Zero;
                                var pResolveAttachments = IntPtr.Zero;

                                if (colorAttachmentCount > 0)
                                {
                                    pColorAttachments = VkInteropsUtility.AllocateHGlobalArray
                                        <MgColorAndResolveAttachmentInfo, VkAttachmentReference2KHR>
                                        (
                                            src.ColorAndResolveAttachments,
                                            (cnr) =>
                                            {
                                                return new VkAttachmentReference2KHR
                                                {
                                                    sType = VkStructureType.StructureTypeAttachmentReference2Khr,
                                                    // TODO: extension here
                                                    pNext = IntPtr.Zero,
                                                    attachment = cnr.Color.Attachment,
                                                    layout = cnr.Color.Layout,
                                                    aspectMask = cnr.Color.AspectMask,
                                                };
                                            }
                                        );
                                    allocatedItems.Add(pColorAttachments);

                                    pResolveAttachments = VkInteropsUtility.AllocateHGlobalArray
                                        <MgColorAndResolveAttachmentInfo, VkAttachmentReference2KHR>
                                        (
                                            src.ColorAndResolveAttachments,
                                            (cnr) =>
                                            {
                                                return new VkAttachmentReference2KHR
                                                {
                                                    sType = VkStructureType.StructureTypeAttachmentReference2Khr,
                                                    // TODO: extension here
                                                    pNext = IntPtr.Zero,
                                                    attachment = cnr.Resolve.Attachment,
                                                    layout = cnr.Resolve.Layout,
                                                    aspectMask = cnr.Resolve.AspectMask,
                                                };
                                            }
                                        );
                                    allocatedItems.Add(pResolveAttachments);
                                }

                                var preserveAttachmentCount = src.PreserveAttachments != null
                                    ? (uint)src.PreserveAttachments.Length
                                    : 0U;
                                var pPreserveAttachments = IntPtr.Zero;

                                if (preserveAttachmentCount > 0)
                                {
                                    pPreserveAttachments = VkInteropsUtility.AllocateUInt32Array(src.PreserveAttachments);
                                    allocatedItems.Add(pPreserveAttachments);
                                }

                                return new VkSubpassDescription2KHR
                                {
                                    sType = VkStructureType.StructureTypeSubpassDescription2Khr,
                                    // TODO: extension here
                                    pNext = IntPtr.Zero,
                                    flags = src.Flags,
                                    pipelineBindPoint = src.PipelineBindPoint,
                                    viewMask = src.ViewMask,
                                    inputAttachmentCount = inputAttachmentCount,
                                    pInputAttachments = pInputAttachments,
                                    colorAttachmentCount = colorAttachmentCount,
                                    pColorAttachments = pColorAttachments,
                                    pResolveAttachments = pResolveAttachments,
                                    pDepthStencilAttachment = ExtractDepthStencilAttachment2KHR(
                                        allocatedItems,
                                        src.DepthStencilAttachment),
                                    preserveAttachmentCount = preserveAttachmentCount,
                                    pPreserveAttachments = pPreserveAttachments,
                                };
                            }
                        );
                    }

                var dependencyCount = pCreateInfo.Dependencies != null ? 
                    (uint) pCreateInfo.Dependencies.Length : 0U;
                var pDependencies = IntPtr.Zero;
                if (dependencyCount > 0)
                {
                    pDependencies = VkInteropsUtility.AllocateHGlobalArray(
                        pCreateInfo.Dependencies,
                         (src) => {
                             return new VkSubpassDependency2KHR
                             {
                                 srcSubpass = src.SrcSubpass,
                                 dstSubpass = src.DstSubpass,
                                 srcStageMask = src.SrcStageMask,
                                 dstStageMask = src.DstStageMask,
                                 srcAccessMask = src.SrcAccessMask,
                                 dstAccessMask = src.DstAccessMask,
                                 dependencyFlags = src.DependencyFlags,
                             };
                         });
                    allocatedItems.Add(pDependencies);
                }

                var correlatedViewMaskCount = pCreateInfo.CorrelatedViewMasks != null 
                    ? (uint)pCreateInfo.CorrelatedViewMasks.Length
                    : 0U;

                var pCorrelatedViewMasks = IntPtr.Zero;

                if (correlatedViewMaskCount > 0)
                {
                    pCorrelatedViewMasks = VkInteropsUtility.AllocateUInt32Array(pCreateInfo.CorrelatedViewMasks);
                    allocatedItems.Add(pCorrelatedViewMasks);
                }

                var createInfo = new VkRenderPassCreateInfo2KHR
                {
                    sType = VkStructureType.StructureTypeRenderPassCreateInfo2Khr,
                    pNext = IntPtr.Zero,
                    flags = pCreateInfo.Flags,
                    attachmentCount = attachmentCount,
                    pAttachments = pAttachments,
                    subpassCount = subpassCount,
                    pSubpasses = pSubpasses,
                    dependencyCount = dependencyCount,
                    pDependencies = pDependencies,
                    correlatedViewMaskCount = correlatedViewMaskCount,
                    pCorrelatedViewMasks = pCorrelatedViewMasks,
                };

                ulong internalHandle = 0;
                var result = Interops.vkCreateRenderPass2KHR(info.Handle, ref createInfo, allocatorPtr, ref internalHandle);
                pRenderPass = new VkRenderPass(internalHandle);
                return result;
            }
            finally
            {
                foreach (var ptr in allocatedItems)
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

        private static IntPtr ExtractDepthStencilAttachment2KHR(List<IntPtr> allocatedItems, MgAttachmentReference2KHR src)
        {
            var result = IntPtr.Zero;

            if (src != null)
            {
                result = VkInteropsUtility.AllocateHGlobalStructArray(
                    new[]
                    {
                        new VkAttachmentReference2KHR
                        {
                            sType = VkStructureType.StructureTypeAttachmentReference2Khr,
                            // TODO: extension here
                            pNext = IntPtr.Zero,
                            attachment = src.Attachment,
                            layout = src.Layout,
                            aspectMask = src.AspectMask,

                        }
                    }
                );
                allocatedItems.Add(result);
            }

            return result;
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

        public void GetDeviceQueue2(MgDeviceQueueInfo2 pQueueInfo, IMgQueue pQueue)
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
