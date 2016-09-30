using System;
using System.Collections.Generic;
using System.Diagnostics;
using Metal;

namespace Magnesium.Metal
{
	public class AmtDevice : IMgDevice
	{
		private IMTLDevice mDevice; 
		private IAmtDeviceQuery mQuery;
		public AmtDevice(IMTLDevice systemDefault, IAmtDeviceQuery mQuery)
		{
			this.mDevice = systemDefault;
			this.mQuery = mQuery;
		}

		public Result AcquireNextImageKHR(IMgSwapchainKHR swapchain, ulong timeout, IMgSemaphore semaphore, IMgFence fence, out uint pImageIndex)
		{
			throw new NotImplementedException();
		}

		public Result AllocateCommandBuffers(MgCommandBufferAllocateInfo pAllocateInfo, IMgCommandBuffer[] pCommandBuffers)
		{
			if (pAllocateInfo == null)
				throw new ArgumentNullException(nameof(pAllocateInfo));

			if (pCommandBuffers == null)
				throw new ArgumentNullException(nameof(pCommandBuffers));

			if (pAllocateInfo.CommandBufferCount != pCommandBuffers.Length)
				throw new ArgumentOutOfRangeException(nameof(pAllocateInfo.CommandBufferCount) + " !=  " + nameof(pCommandBuffers.Length));

			var commandPool = (AmtCommandPool)pAllocateInfo.CommandPool;
			Debug.Assert(commandPool != null, nameof(pAllocateInfo.CommandPool) + " is null");

			var arraySize = pAllocateInfo.CommandBufferCount;



			for (var i = 0; i < arraySize; ++i)
			{
				var cmdBuf = commandPool.Queue.CommandBuffer();
				var instructions = new List<AmtCommandEncoderInstruction>();
				var computeBag = new AmtComputeEncoderItemBag();
				var compute = new AmtComputeEncoder(instructions, mDevice, computeBag);
				var graphicsBag = new AmtGraphicsEncoderItemBag();
				var graphics = new AmtGraphicsEncoder(instructions, graphicsBag, mDevice);
				var command = new AmtCommandEncoder(graphics, compute);
				pCommandBuffers[i] = new AmtCommandBuffer(cmdBuf, false, command);
			}


			return Result.SUCCESS;
		}

		public Result AllocateDescriptorSets(MgDescriptorSetAllocateInfo pAllocateInfo, out IMgDescriptorSet[] pDescriptorSets)
		{
			if (pAllocateInfo == null)
			{
				throw new ArgumentNullException(nameof(pAllocateInfo));
			}

			var pool = (AmtDescriptorPool) pAllocateInfo.DescriptorPool;
			if (pool == null)
			{
				throw new ArgumentNullException(nameof(pAllocateInfo.DescriptorPool));
			}

			var noOfSetsRequested = pAllocateInfo.SetLayouts.Length;
			if (pool.RemainingSets < noOfSetsRequested)
			{
				throw new InvalidOperationException();
			}

			pDescriptorSets = new AmtDescriptorSet[noOfSetsRequested];

			for (int i = 0; i < noOfSetsRequested; ++i)
			{
				var setLayout = (AmtDescriptorSetLayout) pAllocateInfo.SetLayouts[i];

				AmtDescriptorSet dSet;
				if (!pool.TryTake(out dSet))
				{
					throw new InvalidOperationException();
				}
				// copy here
				dSet.Initialize(setLayout);
				pDescriptorSets[i] = dSet;
			}

			return Result.SUCCESS;
		}

		public Result AllocateMemory(MgMemoryAllocateInfo pAllocateInfo, IMgAllocationCallbacks allocator, out IMgDeviceMemory pMemory)
		{
			throw new NotImplementedException();
		}

		public Result CreateBuffer(MgBufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBuffer pBuffer)
		{
			pBuffer = new AmtBuffer(mDevice, pCreateInfo);
			return Result.SUCCESS;
		}

		public Result CreateBufferView(MgBufferViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBufferView pView)
		{
			throw new NotImplementedException();
		}

		public Result CreateCommandPool(MgCommandPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgCommandPool pCommandPool)
		{
			var queue = mDevice.CreateCommandQueue(mQuery.NoOfCommandBufferSlots);
			pCommandPool = new AmtCommandPool(queue);
			return Result.SUCCESS;
		}

		public Result CreateComputePipelines(IMgPipelineCache pipelineCache, MgComputePipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines)
		{
			var output = new List<IMgPipeline>();

			foreach (var info in pCreateInfos)
			{
				var pipeline = new AmtComputePipeline(mDevice, info);
				output.Add(pipeline);
			}
			pPipelines = output.ToArray();
			return Result.SUCCESS;
		}

		public Result CreateDescriptorPool(MgDescriptorPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorPool pDescriptorPool)
		{
			pDescriptorPool = new AmtDescriptorPool(pCreateInfo);
			return Result.SUCCESS;
		}

		public Result CreateDescriptorSetLayout(MgDescriptorSetLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorSetLayout pSetLayout)
		{
			pSetLayout = new AmtDescriptorSetLayout(pCreateInfo);
			return Result.SUCCESS;
		}

		public Result CreateEvent(MgEventCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgEvent @event)
		{
			throw new NotImplementedException();
		}

		public Result CreateFence(MgFenceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFence fence)
		{
			throw new NotImplementedException();
		}

		public Result CreateFramebuffer(MgFramebufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFramebuffer pFramebuffer)
		{
			throw new NotImplementedException();
		}

		public Result CreateGraphicsPipelines(IMgPipelineCache pipelineCache, MgGraphicsPipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines)
		{
			if (pCreateInfos == null)
				throw new ArgumentNullException(nameof(pCreateInfos));

			var output = new List<IMgPipeline>();

			foreach (var info in pCreateInfos)
			{
				var pipeline = new AmtGraphicsPipeline(mDevice, info);
				output.Add(pipeline);

			}
			pPipelines = output.ToArray();
			return Result.SUCCESS;
		}

		private static MTLTextureType TranslateTextureType(MgImageType imageType)
		{
			switch (imageType)
			{
				case MgImageType.TYPE_1D:
					return MTLTextureType.k1D;
				case MgImageType.TYPE_2D:
					return MTLTextureType.k2D;
				case MgImageType.TYPE_3D:
					return MTLTextureType.k3D;
				default:
					throw new NotSupportedException();
			}
		}

		private MTLTextureUsage TranslateUsage(MgImageUsageFlagBits flags)
		{
			MTLTextureUsage output = 0;
			if ((flags & MgImageUsageFlagBits.COLOR_ATTACHMENT_BIT) == MgImageUsageFlagBits.COLOR_ATTACHMENT_BIT)
			{
				output |= MTLTextureUsage.RenderTarget;
			}
			if ((flags & MgImageUsageFlagBits.DEPTH_STENCIL_ATTACHMENT_BIT) == MgImageUsageFlagBits.DEPTH_STENCIL_ATTACHMENT_BIT)
			{
				output |= MTLTextureUsage.RenderTarget;
			}

			if ((flags & MgImageUsageFlagBits.TRANSFER_DST_BIT) == MgImageUsageFlagBits.TRANSFER_DST_BIT)
			{
				output |= MTLTextureUsage.Blit;
			}

			if ((flags & MgImageUsageFlagBits.TRANSFER_SRC_BIT) == MgImageUsageFlagBits.TRANSFER_SRC_BIT)
			{
				output |= MTLTextureUsage.PixelFormatView;
			}

			if ((flags & MgImageUsageFlagBits.SAMPLED_BIT) == MgImageUsageFlagBits.SAMPLED_BIT)
			{
				output |= MTLTextureUsage.ShaderRead;
			}

			if ((flags & MgImageUsageFlagBits.STORAGE_BIT) == MgImageUsageFlagBits.STORAGE_BIT)
			{
				output |= MTLTextureUsage.ShaderWrite;
			}

			return output;
		}

		public Result CreateImage(MgImageCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgImage pImage)
		{
			Debug.Assert(pCreateInfo != null);

			var depth = (nuint)pCreateInfo.Extent.Depth;
			var height = (nuint)pCreateInfo.Extent.Height;
			var width = (nuint) pCreateInfo.Extent.Width;
			var arrayLayers = (nuint)pCreateInfo.ArrayLayers;
			var mipLevels = (nuint) pCreateInfo.MipLevels;

			//TODO : Figure this out
			var storageMode = MTLStorageMode.Shared;
			var resourceOptions = MTLResourceOptions.CpuCacheModeDefault;
			var cpuCacheMode = MTLCpuCacheMode.DefaultCache;

			var descriptor = new MTLTextureDescriptor
			{ 
				ArrayLength = arrayLayers,
				PixelFormat = AmtFormatExtensions.GetPixelFormat(pCreateInfo.Format),
				SampleCount = AmtSampleCountFlagBitExtensions.TranslateSampleCount(pCreateInfo.Samples),
				TextureType = TranslateTextureType(pCreateInfo.ImageType),
				StorageMode = storageMode,
				Width = width,
				Height = height,
				Depth = depth,
				MipmapLevelCount = mipLevels,
				Usage = TranslateUsage(pCreateInfo.Usage),
				ResourceOptions =resourceOptions,
				CpuCacheMode = cpuCacheMode,
			};
			
			var texture = mDevice.CreateTexture(descriptor);
			pImage = new AmtImage(texture);
			return Result.SUCCESS;
		}

		public Result CreateImageView(MgImageViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgImageView pView)
		{
			pView = new AmtImageView(pCreateInfo);
			return Result.SUCCESS;
		}

		public Result CreatePipelineCache(MgPipelineCacheCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineCache pPipelineCache)
		{
			throw new NotImplementedException();
		}

		public Result CreatePipelineLayout(MgPipelineLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineLayout pPipelineLayout)
		{
			pPipelineLayout = new AmtPipelineLayout(pCreateInfo);
			return Result.SUCCESS;
		}

		public Result CreateQueryPool(MgQueryPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgQueryPool queryPool)
		{
			throw new NotImplementedException();
		}

		public Result CreateRenderPass(MgRenderPassCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgRenderPass pRenderPass)
		{
			pRenderPass = new AmtRenderPass(pCreateInfo);
			return Result.SUCCESS;
		}

		public Result CreateSampler(MgSamplerCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSampler pSampler)
		{
			pSampler = new AmtSampler(mDevice, pCreateInfo);
			return Result.SUCCESS;
		}

		public Result CreateSemaphore(MgSemaphoreCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSemaphore pSemaphore)
		{
			throw new NotImplementedException();
		}

		public Result CreateShaderModule(MgShaderModuleCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgShaderModule pShaderModule)
		{
			pShaderModule = new AmtShaderModule(pCreateInfo);
			return Result.SUCCESS;
		}

		public Result CreateSharedSwapchainsKHR(MgSwapchainCreateInfoKHR[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgSwapchainKHR[] pSwapchains)
		{
			throw new NotImplementedException();
		}

		public Result CreateSwapchainKHR(MgSwapchainCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSwapchainKHR pSwapchain)
		{
			throw new NotImplementedException();
		}

		public void DestroyDevice(IMgAllocationCallbacks allocator)
		{

		}

		public Result DeviceWaitIdle()
		{
			throw new NotImplementedException();
		}

		public Result FlushMappedMemoryRanges(MgMappedMemoryRange[] pMemoryRanges)
		{
			throw new NotImplementedException();
		}

		public void FreeCommandBuffers(IMgCommandPool commandPool, IMgCommandBuffer[] pCommandBuffers)
		{
			throw new NotImplementedException();
		}

		public Result FreeDescriptorSets(IMgDescriptorPool descriptorPool, IMgDescriptorSet[] pDescriptorSets)
		{
			throw new NotImplementedException();
		}

		public void GetBufferMemoryRequirements(IMgBuffer buffer, out MgMemoryRequirements pMemoryRequirements)
		{
			throw new NotImplementedException();
		}

		public void GetDeviceMemoryCommitment(IMgDeviceMemory memory, ref ulong pCommittedMemoryInBytes)
		{
			throw new NotImplementedException();
		}

		public PFN_vkVoidFunction GetDeviceProcAddr(string pName)
		{
			throw new NotImplementedException();
		}

		public void GetDeviceQueue(uint queueFamilyIndex, uint queueIndex, out IMgQueue pQueue)
		{
			throw new NotImplementedException();
		}

		public Result GetFenceStatus(IMgFence fence)
		{
			throw new NotImplementedException();
		}

		public void GetImageMemoryRequirements(IMgImage image, out MgMemoryRequirements memoryRequirements)
		{
			throw new NotImplementedException();
		}

		public void GetImageSparseMemoryRequirements(IMgImage image, out MgSparseImageMemoryRequirements[] sparseMemoryRequirements)
		{
			throw new NotImplementedException();
		}

		public void GetImageSubresourceLayout(IMgImage image, MgImageSubresource pSubresource, out MgSubresourceLayout pLayout)
		{
			throw new NotImplementedException();
		}

		public Result GetPipelineCacheData(IMgPipelineCache pipelineCache, out byte[] pData)
		{
			throw new NotImplementedException();
		}

		public Result GetQueryPoolResults(IMgQueryPool queryPool, uint firstQuery, uint queryCount, IntPtr dataSize, IntPtr pData, ulong stride, MgQueryResultFlagBits flags)
		{
			throw new NotImplementedException();
		}

		public void GetRenderAreaGranularity(IMgRenderPass renderPass, out MgExtent2D pGranularity)
		{
			throw new NotImplementedException();
		}

		public Result GetSwapchainImagesKHR(IMgSwapchainKHR swapchain, out IMgImage[] pSwapchainImages)
		{
			throw new NotImplementedException();
		}

		public Result InvalidateMappedMemoryRanges(MgMappedMemoryRange[] pMemoryRanges)
		{
			throw new NotImplementedException();
		}

		public Result MergePipelineCaches(IMgPipelineCache dstCache, IMgPipelineCache[] pSrcCaches)
		{
			throw new NotImplementedException();
		}

		public Result ResetFences(IMgFence[] pFences)
		{
			throw new NotImplementedException();
		}

		public void UpdateDescriptorSets(MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies)
		{
			if (pDescriptorWrites != null)
			{
				foreach (var desc in pDescriptorWrites)
				{
					var localSet = (AmtDescriptorSet) desc.DstSet;
					if (localSet == null)
					{
						throw new ArgumentNullException(nameof(desc.DstSet));
					}

					var x = desc.DstBinding; // SHOULD ALWAYS BE ZERO
					if (x != 0)
					{
						throw new ArgumentException(nameof(desc.DstBinding));
					}

					int offset = (int)desc.DstArrayElement;
					int count = (int)desc.DescriptorCount;

					// HOPEFULLY DESCRIPTOR SETS ARE GROUPED BY COMMON TYPES
					foreach (var map in new[] { localSet.Compute, localSet.Fragment, localSet.Vertex })
					{
						switch (desc.DescriptorType)
						{
							//case MgDescriptorType.SAMPLER:
							case MgDescriptorType.COMBINED_IMAGE_SAMPLER:
							case MgDescriptorType.SAMPLED_IMAGE:
								
								for (int i = 0; i < count; ++i)
								{
									MgDescriptorImageInfo info = desc.ImageInfo[i];

									var localSampler = (AmtSampler)info.Sampler;
									var localView = (AmtImageView)info.ImageView;

									var index = offset + i;
									map.Textures[index].Texture = localView.Image;
									map.SamplerStates[index].Sampler = localSampler.Sampler;
								}

								break;
							case MgDescriptorType.UNIFORM_BUFFER:
							case MgDescriptorType.UNIFORM_BUFFER_DYNAMIC:
							case MgDescriptorType.STORAGE_BUFFER:
							case MgDescriptorType.STORAGE_BUFFER_DYNAMIC:
								// HOPEFULLY DESCRIPTOR SETS ARE GROUPED BY COMMON TYPES
								for (int i = 0; i < count; ++i)
								{
									var info = desc.BufferInfo[i];

									var buf = (AmtBuffer)info.Buffer;

									var index = offset + i;
									map.Buffers[index].Buffer = buf.VertexBuffer;
								}
								break;
							default:
								throw new NotSupportedException();
						}
					}

				}
			}
		}

		public Result WaitForFences(IMgFence[] pFences, bool waitAll, ulong timeout)
		{
			throw new NotImplementedException();
		}
	}
}
