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
			Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

			var writeCount = 0U;
			if (pDescriptorWrites != null)
			{
				writeCount = (uint)pDescriptorWrites.Length;
			}

			var copyCount = 0U;
			if (pDescriptorCopies != null)
			{
				copyCount = (uint)pDescriptorCopies.Length;
			}

			var attachedItems = new List<IntPtr>();

			try
			{
				unsafe
				{
					VkWriteDescriptorSet* writes = null;
					VkCopyDescriptorSet* copies = null;

					if (writeCount > 0)
					{
						var bWriteSets = stackalloc VkWriteDescriptorSet[(int)writeCount];

						for (var i = 0; i < writeCount; ++i)
						{
							var currentWrite = pDescriptorWrites[i];
							var bDstSet = (VkDescriptorSet)currentWrite.DstSet;
							Debug.Assert(bDstSet != null);

							var descriptorCount = (int) currentWrite.DescriptorCount;

							var pImageInfo = IntPtr.Zero;
							if (currentWrite.ImageInfo != null)
							{
								if (descriptorCount > 0)
								{
									pImageInfo = VkInteropsUtility.AllocateHGlobalArray( 
										currentWrite.ImageInfo,
										(srcInfo) =>
										{
											var bSampler = (VkSampler) srcInfo.Sampler;
											Debug.Assert(bSampler != null);

											var bImageView = (VkImageView) srcInfo.ImageView;
											Debug.Assert(bImageView != null);

											return new VkDescriptorImageInfo
											{
												sampler = bSampler.Handle,
												imageView = bImageView.Handle,
												imageLayout = srcInfo.ImageLayout,
											};
										});				
									attachedItems.Add(pImageInfo);								
								}
							}

							var pBufferInfo = IntPtr.Zero;
							if (currentWrite.BufferInfo != null)
							{
								if (descriptorCount > 0)
								{
									pBufferInfo = VkInteropsUtility.AllocateHGlobalArray(
										currentWrite.BufferInfo,
										(src) =>
										{
											var bBuffer = (VkBuffer) src.Buffer;
											Debug.Assert(bBuffer != null);

											return new VkDescriptorBufferInfo
											{
												buffer = bBuffer.Handle,
												offset = src.Offset,
												range = src.Range,
											};
										}
									);
									attachedItems.Add(pBufferInfo);
								}
							}

							var pTexelBufferView = IntPtr.Zero;
							if (currentWrite.TexelBufferView != null)
							{
								if (descriptorCount > 0)
								{
									pTexelBufferView = VkInteropsUtility.ExtractUInt64HandleArray(currentWrite.TexelBufferView,
										(tbv) =>
										{
											var bBufferView = (VkBufferView) tbv;
											Debug.Assert(bBufferView != null);
											return bBufferView.Handle;
										}
										);
									attachedItems.Add(pTexelBufferView);
								}
							}							

							bWriteSets[i] = new VkWriteDescriptorSet
							{
								sType = VkStructureType.StructureTypeWriteDescriptorSet,
								pNext = IntPtr.Zero,
								dstSet = bDstSet.Handle,
								dstBinding = currentWrite.DstBinding,
								dstArrayElement = currentWrite.DstArrayElement,
								descriptorCount = currentWrite.DescriptorCount,
								descriptorType = currentWrite.DescriptorType,
								pImageInfo = pImageInfo,
								pBufferInfo = pBufferInfo,
								pTexelBufferView = pTexelBufferView,
							};
						}

						writes = bWriteSets;
					}

					if (copyCount > 0)
					{
						var bCopySets = stackalloc VkCopyDescriptorSet[(int)copyCount];

						for (var j = 0; j < copyCount; ++j)
						{
							var currentCopy = pDescriptorCopies[j];

							var bSrcSet = (VkDescriptorSet)currentCopy.SrcSet;
							Debug.Assert(bSrcSet != null);

							var bDstSet = (VkDescriptorSet)currentCopy.DstSet;
							Debug.Assert(bDstSet != null);

							bCopySets[j] = new VkCopyDescriptorSet
							{
								sType = VkStructureType.StructureTypeCopyDescriptorSet,
								pNext = IntPtr.Zero,
								srcSet = bSrcSet.Handle,
								srcBinding = currentCopy.SrcBinding,
								srcArrayElement = currentCopy.SrcArrayElement,
								dstSet = bDstSet.Handle,
								dstBinding = currentCopy.DstBinding,
								dstArrayElement = currentCopy.DstArrayElement,
								descriptorCount = currentCopy.DescriptorCount,
							};
						}

						copies = bCopySets;
					}

					Interops.vkUpdateDescriptorSets(info.Handle, writeCount, writes, copyCount, copies);
				}			
			}
			finally
			{
				foreach(var item in attachedItems)
				{
					Marshal.FreeHGlobal(item);
				}
			}
		}

		public MgResult CreateFramebuffer(MgFramebufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFramebuffer pFramebuffer)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

			var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(allocator);

			var bRenderPass = (VkRenderPass) pCreateInfo.RenderPass;
			Debug.Assert(bRenderPass != null);

			var attachmentCount = 0U;
			var pAttachments = IntPtr.Zero;

			try
			{
				if (pCreateInfo.Attachments != null)
				{
					attachmentCount = (uint) pCreateInfo.Attachments.Length;
					if (attachmentCount > 0)
					{									
						pAttachments = VkInteropsUtility.ExtractUInt64HandleArray(pCreateInfo.Attachments, 
							(a) => 
							{
								var bImageView = (VkImageView) a;
								Debug.Assert(bImageView != null);
								return bImageView.Handle;
							});
					}
				}

				var createInfo = new VkFramebufferCreateInfo
				{
					sType = VkStructureType.StructureTypeFramebufferCreateInfo,
					pNext = IntPtr.Zero,
					flags = pCreateInfo.Flags,
					renderPass = bRenderPass.Handle,
					attachmentCount = attachmentCount,
					pAttachments = pAttachments,
					width = pCreateInfo.Width,
					height = pCreateInfo.Height,
					layers = pCreateInfo.Layers,
				};

				var internalHandle = 0UL;
				var result = Interops.vkCreateFramebuffer(info.Handle, ref createInfo, allocatorPtr, ref internalHandle);
				pFramebuffer = new VkFramebuffer(internalHandle);
				return result;
			}
			finally
			{
				if (pAttachments != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(pAttachments);
				}
			}
		}

		public MgResult CreateRenderPass(MgRenderPassCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgRenderPass pRenderPass)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

			var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(allocator);

			var attachedItems = new List<IntPtr>();

			try
			{
				var pAttachments = IntPtr.Zero;
				uint attachmentCount = pCreateInfo.Attachments != null ? (uint)pCreateInfo.Attachments.Length : 0U;
				if (attachmentCount > 0)
				{	
					pAttachments = VkInteropsUtility.AllocateHGlobalArray(
						pCreateInfo.Attachments,
						(attachment) =>
						{
							return new VkAttachmentDescription
							{
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
					attachedItems.Add(pAttachments);
				}

				var attReferenceSize = Marshal.SizeOf(typeof(VkAttachmentReference));

				uint subpassCount = pCreateInfo.Subpasses != null ? (uint)pCreateInfo.Subpasses.Length : 0U;
				var pSubpasses = IntPtr.Zero;
				if (subpassCount > 0)
				{
					var subPassDescriptionSize = Marshal.SizeOf(typeof(VkSubpassDescription));
					pSubpasses = Marshal.AllocHGlobal((int)(subPassDescriptionSize * subpassCount));
					attachedItems.Add(pSubpasses);

					var subpassOffset = 0;
					foreach (var currentSubpass in pCreateInfo.Subpasses)
					{
						var depthStencil = IntPtr.Zero;
						if (currentSubpass.DepthStencilAttachment != null)
						{
							depthStencil = Marshal.AllocHGlobal(attReferenceSize);
							var attachment = new VkAttachmentReference
							{
								attachment = currentSubpass.DepthStencilAttachment.Attachment,
								layout = currentSubpass.DepthStencilAttachment.Layout,
							};
							Marshal.StructureToPtr(attachment, depthStencil, false);
							attachedItems.Add(depthStencil);
						}

						var pInputAttachments = IntPtr.Zero;
						var inputAttachmentCount = 
                            currentSubpass.InputAttachments != null 
                            ? (uint) currentSubpass.InputAttachments.Length
                            : 0;

						if (inputAttachmentCount > 0)
						{
							pInputAttachments = VkInteropsUtility.AllocateHGlobalArray(
								currentSubpass.InputAttachments,
								(input) =>
								{
									return new VkAttachmentReference
									{
										attachment = input.Attachment,
										layout = input.Layout,
									};
								});
							attachedItems.Add(pInputAttachments);					
						}

						var colorAttachmentCount = 
                            currentSubpass.ColorAttachments != null 
                            ? (uint)currentSubpass.ColorAttachments.Length
                            : 0;

						var pColorAttachments = IntPtr.Zero;
						var pResolveAttachments = IntPtr.Zero;

						if (colorAttachmentCount > 0)
						{
							pColorAttachments = VkInteropsUtility.AllocateHGlobalArray(
								currentSubpass.ColorAttachments,
								(color) =>
								{
									return new VkAttachmentReference
									{
										attachment = color.Attachment,
										layout = color.Layout,
									};
								});
							attachedItems.Add(pColorAttachments);

							if (currentSubpass.ResolveAttachments != null)
							{
								pResolveAttachments = VkInteropsUtility.AllocateHGlobalArray(
									currentSubpass.ResolveAttachments,
									(resolve) =>
									{
										return new VkAttachmentReference
										{
											attachment = resolve.Attachment,
											layout = resolve.Layout,
										};
									});
								attachedItems.Add(pResolveAttachments);
							}
						}

						var preserveAttachmentCount = 
                            currentSubpass.PreserveAttachments != null 
                            ? (uint) currentSubpass.PreserveAttachments.Length 
                            : 0U;
						var pPreserveAttachments = IntPtr.Zero;

						if (preserveAttachmentCount > 0)
						{
							pPreserveAttachments = VkInteropsUtility.AllocateUInt32Array(currentSubpass.PreserveAttachments);
							attachedItems.Add(pPreserveAttachments);
						}	

						var description = new VkSubpassDescription
						{
							flags = currentSubpass.Flags,
							pipelineBindPoint = currentSubpass.PipelineBindPoint,
							inputAttachmentCount = inputAttachmentCount,
							pInputAttachments = pInputAttachments,// VkAttachmentReference
							colorAttachmentCount = colorAttachmentCount, 
							pColorAttachments = pColorAttachments, // VkAttachmentReference
							pResolveAttachments = pResolveAttachments,
							pDepthStencilAttachment = depthStencil,
							preserveAttachmentCount = preserveAttachmentCount,
							pPreserveAttachments = pPreserveAttachments, // uint
						};

						var dest = IntPtr.Add(pSubpasses, subpassOffset);
						Marshal.StructureToPtr(description, dest, false);
						subpassOffset += subPassDescriptionSize;
					}
				}

				uint dependencyCount = pCreateInfo.Dependencies != null ? (uint) pCreateInfo.Dependencies.Length : 0U;
				var pDependencies = IntPtr.Zero;
				if (dependencyCount > 0)
				{
					pDependencies = VkInteropsUtility.AllocateHGlobalArray(
						pCreateInfo.Dependencies,
                     	(src) => {
						return new VkSubpassDependency
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
					attachedItems.Add(pDependencies);
				}

				var createInfo = new VkRenderPassCreateInfo
				{
					sType = VkStructureType.StructureTypeRenderPassCreateInfo,
					pNext = IntPtr.Zero,
					flags = pCreateInfo.Flags,
					attachmentCount = attachmentCount,
					pAttachments = pAttachments,
					subpassCount = subpassCount,
					pSubpasses = pSubpasses,
					dependencyCount = dependencyCount,
					pDependencies = pDependencies,
				};

				ulong internalHandle = 0;
				var result = Interops.vkCreateRenderPass(info.Handle, ref createInfo, allocatorPtr, ref internalHandle);
				pRenderPass = new VkRenderPass(internalHandle);
				return result;
			}
			finally
			{
				foreach (var ptr in attachedItems)
				{
					Marshal.FreeHGlobal(ptr);
				}
			}
		}

		public void GetRenderAreaGranularity(IMgRenderPass renderPass, out MgExtent2D pGranularity)
		{
			Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

			var bRenderPass = (VkRenderPass)renderPass;
			Debug.Assert(bRenderPass != null);

			unsafe
			{
				var grans = stackalloc MgExtent2D[1];
				Interops.vkGetRenderAreaGranularity(info.Handle, bRenderPass.Handle, grans);
				pGranularity = grans[0];
			}
		}

		public MgResult CreateCommandPool(MgCommandPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgCommandPool pCommandPool)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

			var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(allocator);

			ulong internalHandle = 0UL;
			var createInfo = new VkCommandPoolCreateInfo
			{
				sType = VkStructureType.StructureTypeCommandPoolCreateInfo,
				pNext = IntPtr.Zero,
				flags = (VkCommandPoolCreateFlags)pCreateInfo.Flags,
				queueFamilyIndex = pCreateInfo.QueueFamilyIndex,
			};
			var result = Interops.vkCreateCommandPool(info.Handle, ref createInfo, allocatorPtr, ref internalHandle);
			pCommandPool = new VkCommandPool(internalHandle);
			return result;
		}

		public MgResult AllocateCommandBuffers(MgCommandBufferAllocateInfo pAllocateInfo, IMgCommandBuffer[] pCommandBuffers)
		{
			if (pAllocateInfo == null)
				throw new ArgumentNullException(nameof(pAllocateInfo));

			if (pCommandBuffers == null)
				throw new ArgumentNullException(nameof(pCommandBuffers));

			if (pAllocateInfo.CommandBufferCount != pCommandBuffers.Length)
				throw new ArgumentOutOfRangeException(nameof(pAllocateInfo.CommandBufferCount) + " !=  " + nameof(pCommandBuffers.Length));

			Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

			var bCommandPool = (VkCommandPool)pAllocateInfo.CommandPool;
			Debug.Assert(bCommandPool != null);

			unsafe
			{
				var arraySize = (int)pAllocateInfo.CommandBufferCount;

				var pBufferHandle = stackalloc IntPtr[arraySize];

				var allocateInfo = stackalloc VkCommandBufferAllocateInfo[1];

				allocateInfo[0] = new VkCommandBufferAllocateInfo
				{
					sType = VkStructureType.StructureTypeCommandBufferAllocateInfo,
					pNext = IntPtr.Zero,
					commandBufferCount = pAllocateInfo.CommandBufferCount,
					commandPool = bCommandPool.Handle,
					level = (VkCommandBufferLevel)pAllocateInfo.Level,
				};

				var result = Interops.vkAllocateCommandBuffers(info.Handle, allocateInfo, pBufferHandle);

				for (var i = 0; i < arraySize; ++i)
				{
					pCommandBuffers[i] = new VkCommandBuffer(pBufferHandle[i]);
				}
				return result;
			}
		}

		public void FreeCommandBuffers(IMgCommandPool commandPool, IMgCommandBuffer[] pCommandBuffers)
		{
			Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

			var bCommandPool = (VkCommandPool) commandPool;
			Debug.Assert(bCommandPool != null);

			var commandBufferCount = pCommandBuffers != null ? (uint) pCommandBuffers.Length : 0U;

			if (commandBufferCount > 0)
			{
				var bufferHandles = new IntPtr[commandBufferCount];
				for (var i = 0; i < commandBufferCount; ++i)
				{
					var bCommandBuffer = (VkCommandBuffer)pCommandBuffers[i];
					bufferHandles[i] = bCommandBuffer.Handle;
				}

				Interops.vkFreeCommandBuffers(info.Handle, bCommandPool.Handle, commandBufferCount, bufferHandles);
			}
		}

		public MgResult CreateSharedSwapchainsKHR(MgSwapchainCreateInfoKHR[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgSwapchainKHR[] pSwapchains)
		{
			if (pCreateInfos == null)
				throw new ArgumentNullException(nameof(pCreateInfos));

			Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

			var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(allocator);

			var attachedItems = new List<IntPtr>();

			try
			{

				var createInfoStructSize = Marshal.SizeOf(typeof(VkSwapchainCreateInfoKHR));
				var swapChainCount = 0U;

				if (pCreateInfos != null)
				{
					swapChainCount = (uint)pCreateInfos.Length;
				}

				var swapChainCreateInfos = new VkSwapchainCreateInfoKHR[swapChainCount];
				for (var i = 0; i < swapChainCount; ++i)
				{
					swapChainCreateInfos[i] = GenerateSwapchainCreateInfoKHR(pCreateInfos[i], attachedItems);
				}

				var sharedSwapchains = new ulong[swapChainCount];
				var result = Interops.vkCreateSharedSwapchainsKHR(info.Handle, swapChainCount, swapChainCreateInfos, allocatorPtr, sharedSwapchains);

				pSwapchains = new VkSwapchainKHR[swapChainCount];
				for (var i = 0; i < swapChainCount; ++i)
				{
					pSwapchains[i] = new VkSwapchainKHR(sharedSwapchains[i]);
				}
				return result;
			}
			finally
			{
				foreach (var handle in attachedItems)
				{
					Marshal.FreeHGlobal(handle);
				}
			}
		}

		public MgResult CreateSwapchainKHR(MgSwapchainCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSwapchainKHR pSwapchain)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

			var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(allocator);

			var attachedItems = new List<IntPtr>();

			try
			{
				var createInfo = GenerateSwapchainCreateInfoKHR(pCreateInfo, attachedItems);

				ulong internalHandle = 0;
				var result = Interops.vkCreateSwapchainKHR(info.Handle, ref createInfo, allocatorPtr, ref internalHandle);
				pSwapchain = new VkSwapchainKHR(internalHandle);
				return result;
			}
			finally
			{
				foreach (var handle in attachedItems)
				{
					Marshal.FreeHGlobal(handle);
				}
			}
		}

		static VkSwapchainCreateInfoKHR GenerateSwapchainCreateInfoKHR(MgSwapchainCreateInfoKHR pCreateInfo, List<IntPtr> attachedItems)
		{
			var bSurface = (VkSurfaceKHR)pCreateInfo.Surface;
			var bSurfacePtr = bSurface != null ? bSurface.Handle : 0UL;

			var bOldSwapchain = (VkSwapchainKHR)pCreateInfo.OldSwapchain;
			var bOldSwapchainPtr = bOldSwapchain != null ? bOldSwapchain.Handle : 0UL;

			var pQueueFamilyIndices = IntPtr.Zero;
			var queueFamilyIndexCount =  0U;


			if (pCreateInfo.QueueFamilyIndices != null)
			{
				queueFamilyIndexCount = (uint)pCreateInfo.QueueFamilyIndices.Length;	
				pQueueFamilyIndices = VkInteropsUtility.AllocateUInt32Array(pCreateInfo.QueueFamilyIndices); 
				attachedItems.Add(pQueueFamilyIndices);
			}

			var createInfo = new VkSwapchainCreateInfoKHR
			{
				sType = VkStructureType.StructureTypeSwapchainCreateInfoKhr,
				pNext = IntPtr.Zero,
				flags = pCreateInfo.Flags,
				surface = bSurfacePtr,
				minImageCount = pCreateInfo.MinImageCount,
				imageFormat = pCreateInfo.ImageFormat,
				imageColorSpace = pCreateInfo.ImageColorSpace,
				imageExtent = pCreateInfo.ImageExtent,
				imageArrayLayers = pCreateInfo.ImageArrayLayers,
				imageUsage = pCreateInfo.ImageUsage,
				imageSharingMode = (VkSharingMode)pCreateInfo.ImageSharingMode,
				queueFamilyIndexCount = queueFamilyIndexCount,
				pQueueFamilyIndices = pQueueFamilyIndices,
				preTransform = (VkSurfaceTransformFlagsKhr)pCreateInfo.PreTransform,
				compositeAlpha = (VkCompositeAlphaFlagsKhr)pCreateInfo.CompositeAlpha,
				presentMode = (VkPresentModeKhr)pCreateInfo.PresentMode,
				clipped = VkBool32.ConvertTo(pCreateInfo.Clipped),
				oldSwapchain = bOldSwapchainPtr
			};
			return createInfo;

		}

		public MgResult GetSwapchainImagesKHR(IMgSwapchainKHR swapchain, out IMgImage[] pSwapchainImages)
		{
			Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

			var bSwapchain = (VkSwapchainKHR)swapchain;
			Debug.Assert(bSwapchain != null);

			uint noOfImages = 0;
			var first = Interops.vkGetSwapchainImagesKHR(info.Handle, bSwapchain.Handle, ref noOfImages, null);

			if (first != MgResult.SUCCESS)
			{
				pSwapchainImages = null;
				return first;
			}

			var images = new ulong[noOfImages];
			var final = Interops.vkGetSwapchainImagesKHR(info.Handle, bSwapchain.Handle, ref noOfImages, images);

			pSwapchainImages = new VkImage[noOfImages];
			for (var i = 0; i < noOfImages; ++i)
			{
				pSwapchainImages[i] = new VkImage(images[i]);
			}

			return final;
		}

		public MgResult AcquireNextImageKHR(IMgSwapchainKHR swapchain, ulong timeout, IMgSemaphore semaphore, IMgFence fence, out uint pImageIndex)
		{
			Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

			var bSwapchain = (VkSwapchainKHR)swapchain;
			Debug.Assert(bSwapchain != null);

			var bSemaphore = (VkSemaphore)semaphore;
			var bSemaphorePtr = bSemaphore != null ? bSemaphore.Handle : 0UL;

			var bFence = (VkFence)fence;
			var bFencePtr = bFence != null ? bFence.Handle : 0UL;

			uint imageIndex = 0;
			var result = Interops.vkAcquireNextImageKHR(info.Handle, bSwapchain.Handle, timeout, bSemaphorePtr, bFencePtr, ref imageIndex);
			pImageIndex = imageIndex;
			return result;
		}

        public MgResult CreateObjectTableNVX(MgObjectTableCreateInfoNVX pCreateInfo, IMgAllocationCallbacks allocator, out IMgObjectTableNVX pObjectTable)
        {
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));

            if (pCreateInfo.Entries == null)
                throw new ArgumentNullException(nameof(pCreateInfo.Entries));

            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(allocator);

            var pObjectEntryTypes = IntPtr.Zero;
            var pObjectEntryCounts = IntPtr.Zero;
            var pObjectEntryUsageFlags = IntPtr.Zero;

            try
            {
                var objectCount = (UInt32) pCreateInfo.Entries.Length;

                if (objectCount > 0)
                {
                    var counts = new UInt32[objectCount];
                    var types = new UInt32[objectCount];
                    var flags = new UInt32[objectCount];

                    for (var i = 0; i < objectCount; i += 1)
                    {
                        var current = pCreateInfo.Entries[i];

                        counts[i] = current.ObjectEntryCount;
                        types[i] = (UInt32)current.ObjectEntryType;
                        flags[i] = (UInt32)current.UsageFlag;
                    }

                    pObjectEntryCounts = VkInteropsUtility.AllocateUInt32Array(counts);
                    pObjectEntryTypes = VkInteropsUtility.AllocateUInt32Array(types);
                    pObjectEntryUsageFlags = VkInteropsUtility.AllocateUInt32Array(flags);
                }

                var bCreateInfo = new VkObjectTableCreateInfoNVX
                {
                    sType = VkStructureType.StructureTypeObjectTableCreateInfoNvx,
                    pNext = IntPtr.Zero,
                    objectCount = objectCount,
                    pObjectEntryTypes = pObjectEntryTypes,
                    pObjectEntryCounts = pObjectEntryCounts,
                    pObjectEntryUsageFlags = pObjectEntryUsageFlags,
                    maxPipelineLayouts = pCreateInfo.MaxPipelineLayouts,
                    maxSampledImagesPerDescriptor = pCreateInfo.MaxSampledImagesPerDescriptor,
                    maxStorageBuffersPerDescriptor = pCreateInfo.MaxStorageBuffersPerDescriptor,
                    maxStorageImagesPerDescriptor = pCreateInfo.MaxStorageImagesPerDescriptor,
                    maxUniformBuffersPerDescriptor = pCreateInfo.MaxUniformBuffersPerDescriptor,
                };

                ulong handle = 0UL;
                var result = Interops.vkCreateObjectTableNVX(info.Handle, ref bCreateInfo, allocatorPtr, ref handle);

                pObjectTable = new VkObjectTableNVX(handle);
                return result;
            }
            finally
            {
                if (pObjectEntryTypes != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pObjectEntryTypes);
                }

                if (pObjectEntryCounts != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pObjectEntryCounts);
                }

                if (pObjectEntryUsageFlags != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pObjectEntryUsageFlags);
                }
            }
        }

        public MgResult CreateIndirectCommandsLayoutNVX(MgIndirectCommandsLayoutCreateInfoNVX pCreateInfo, IMgAllocationCallbacks allocator, out IMgIndirectCommandsLayoutNVX pIndirectCommandsLayout)
        {
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));

            if (pCreateInfo.Tokens == null)
                throw new ArgumentNullException(nameof(pCreateInfo.Tokens));

            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(allocator);

            var pTokens = IntPtr.Zero;

            try
            {
                var tokenCount = (UInt32) pCreateInfo.Tokens.Length;

                pTokens = VkInteropsUtility.AllocateHGlobalStructArray(pCreateInfo.Tokens);

                var createInfo = new VkIndirectCommandsLayoutCreateInfoNVX
                {
                    sType = VkStructureType.StructureTypeIndirectCommandsLayoutCreateInfoNvx,
                    pNext = IntPtr.Zero,
                    pipelineBindPoint = pCreateInfo.PipelineBindPoint,
                    flags = pCreateInfo.Flags,
                    tokenCount = tokenCount,
                    pTokens = pTokens,
                };

                ulong bHandle = 0UL;
                var result = Interops.vkCreateIndirectCommandsLayoutNVX(info.Handle, ref createInfo, allocatorPtr, ref bHandle);

                pIndirectCommandsLayout = new VkIndirectCommandsLayoutNVX(bHandle);
                return result;
            }
            finally
            {
                if (pTokens != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pTokens);
                }
            }
        }

        public MgResult AcquireNextImage2KHR(MgAcquireNextImageInfoKHR pAcquireInfo, ref uint pImageIndex)
        {
            if (pAcquireInfo == null)
                throw new ArgumentNullException(nameof(pAcquireInfo));

            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var bSwapchain = (VkSwapchainKHR)pAcquireInfo.Swapchain;
            Debug.Assert(bSwapchain != null);

            var bSemaphore = (VkSemaphore) pAcquireInfo.Semaphore;
            var bSemaphorePtr = bSemaphore != null ? bSemaphore.Handle : 0UL;

            var bFence = (VkFence) pAcquireInfo.Fence;
            var bFencePtr = bFence != null ? bFence.Handle : 0UL;

            var bAcquireInfo = new VkAcquireNextImageInfoKHR
            {
                sType = VkStructureType.StructureTypeAcquireNextImageInfoKhr,
                // TODO: extensible
                pNext = IntPtr.Zero,
                fence = bFencePtr,
                semaphore = bSemaphorePtr,
                swapchain = bSemaphorePtr,
                timeout = pAcquireInfo.Timeout,
                deviceMask = pAcquireInfo.DeviceMask,                
            };

            uint imageIndex = 0;
            var result = Interops.vkAcquireNextImage2KHR(info.Handle, bAcquireInfo, ref imageIndex);
            pImageIndex = imageIndex;
            return result;
        }

        public MgResult BindAccelerationStructureMemoryNV(MgBindAccelerationStructureMemoryInfoNV[] pBindInfos)
        {
            if (pBindInfos == null)
                throw new ArgumentNullException(nameof(pBindInfos));

            var attachedItems = new List<IntPtr>();

            try
            {
                var bindInfoCount = (UInt32) pBindInfos.Length;

                var bBindInfos = new VkBindAccelerationStructureMemoryInfoNV[bindInfoCount];

                for (var i = 0; i < bindInfoCount; i += 1)
                {
                    var currentInfo = pBindInfos[i];

                    var bAccelerationStructure = (VkAccelerationStructureNV) currentInfo.AccelerationStructure;
                    var bAccelerationStructurePtr = bAccelerationStructure != null ? bAccelerationStructure.Handle : 0UL;

                    var bDeviceMemory = (VkDeviceMemory)currentInfo.Memory;
                    var bDeviceMemoryPtr = bDeviceMemory != null ? bDeviceMemory.Handle : 0UL;

                    var pDeviceIndices = VkInteropsUtility.AllocateUInt32Array(currentInfo.DeviceIndices);
                    if (pDeviceIndices != IntPtr.Zero)
                        attachedItems.Add(pDeviceIndices);

                    var deviceIndexCount = currentInfo.DeviceIndices != null ? (uint) currentInfo.DeviceIndices.Length : 0U;

                    bBindInfos[i] = new VkBindAccelerationStructureMemoryInfoNV
                    {
                        sType = VkStructureType.StructureTypeBindAccelerationStructureMemoryInfoNv,
                        // TODO: extensible
                        pNext = IntPtr.Zero,
                        accelerationStructure = bAccelerationStructurePtr,
                        memory = bDeviceMemoryPtr,
                        memoryOffset = currentInfo.MemoryOffset,
                        deviceIndexCount = deviceIndexCount,
                        pDeviceIndices = pDeviceIndices,                        
                    };
                }

                return Interops.vkBindAccelerationStructureMemoryNV(info.Handle, bindInfoCount, bBindInfos);
            }
            finally
            {
                foreach (var handle in attachedItems)
                {
                    Marshal.FreeHGlobal(handle);
                }
            }
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
