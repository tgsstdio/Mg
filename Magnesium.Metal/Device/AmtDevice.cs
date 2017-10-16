using System;
using System.Collections.Generic;
using System.Diagnostics;
using Foundation;
using Metal;

namespace Magnesium.Metal
{
	public class AmtDevice : IMgDevice
	{
		private IMTLDevice mDevice; 
		private IAmtDeviceQuery mQuery;
		private AmtQueue mQueue;
		private IAmtDeviceEntrypoint mEntrypoint;
		public AmtDevice(IMTLDevice systemDefault,
		                 IAmtDeviceQuery query,
		                 IAmtDeviceEntrypoint entrypoint,
		                 AmtQueue queue)
		{
			mDevice = systemDefault;
			mQuery = query;
			mEntrypoint = entrypoint;
			mQueue = queue;
		}

		public Result AcquireNextImageKHR(IMgSwapchainKHR swapchain, ulong timeout, IMgSemaphore semaphore, IMgFence fence, out uint pImageIndex)
		{
			if (swapchain == null)
				throw new ArgumentNullException(nameof(swapchain));

			// TODO : make it variable
			var bSwapchain = (AmtSwapchainKHR)swapchain;
			var nextIndex = bSwapchain.GetAvailableImageIndex();

			if (timeout == ulong.MaxValue)
			{
				bSwapchain.Images[nextIndex].Inflight.WaitOne();
			}
			else
			{
				var ticks = (long)timeout / 10000L;
				var timespan = TimeSpan.FromTicks(ticks);
				bSwapchain.Images[nextIndex].Inflight.WaitOne(timespan);
			}

			bSwapchain.RefreshImageView(nextIndex);
			pImageIndex = nextIndex;
			return Result.SUCCESS;
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
				var instructions = new AmtIncrementalChunkifier();
				var computeBag = new AmtComputeBag();
				var compute = new AmtComputeEncoder(instructions, computeBag);
				var graphicsBag = new AmtGraphicsBag();
				var graphics = new AmtGraphicsEncoder(instructions, mDevice, graphicsBag, commandPool.DepthCache);
				var blitBag = new AmtBlitBag();
				var blit = new AmtBlitEncoder(blitBag, instructions);

				var command = new AmtCommandEncoder(instructions, graphics, compute, blit);
				var cmdBuf = new AmtCommandBuffer(commandPool.CanIndividuallyReset, command);

				commandPool.Add(cmdBuf);

				pCommandBuffers[i] = cmdBuf;
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
			pMemory = new AmtDeviceMemory(mDevice, pAllocateInfo);
			return Result.SUCCESS;
		}

		public Result CreateBuffer(MgBufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBuffer pBuffer)
		{
			pBuffer = new AmtBuffer(pCreateInfo);
			return Result.SUCCESS;
		}

		public Result CreateBufferView(MgBufferViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBufferView pView)
		{
			throw new NotImplementedException();
		}

		public Result CreateCommandPool(MgCommandPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgCommandPool pCommandPool)
		{
			var queue = mDevice.CreateCommandQueue(mQuery.NoOfCommandBufferSlots);
			var depthCache = new AmtCmdDepthStencilCache();
			pCommandPool = new AmtCommandPool(queue, pCreateInfo, depthCache);
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
			fence = mEntrypoint.Fence.CreateFence();
			return Result.SUCCESS;
		}

		public Result CreateFramebuffer(MgFramebufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFramebuffer pFramebuffer)
		{
			// TODO : make sure everything is attached properly
			pFramebuffer = new AmtFramebuffer(pCreateInfo);
			return Result.SUCCESS;
		}

		public Result CreateGraphicsPipelines(IMgPipelineCache pipelineCache, MgGraphicsPipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines)
		{
			if (pCreateInfos == null)
				throw new ArgumentNullException(nameof(pCreateInfos));

			var output = new List<IMgPipeline>();

			foreach (var info in pCreateInfos)
			{
				var pipeline = new AmtGraphicsPipeline(mEntrypoint.LibraryLoader, mDevice, info);
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
			pSemaphore = mEntrypoint.Semaphore.CreateSemaphore();
			return Result.SUCCESS;
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

		}

		public Result FreeDescriptorSets(IMgDescriptorPool descriptorPool, IMgDescriptorSet[] pDescriptorSets)
		{
			throw new NotImplementedException();
		}

		public void GetBufferMemoryRequirements(IMgBuffer buffer, out MgMemoryRequirements pMemoryRequirements)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException(nameof(buffer));
			}

			var bBuffer = (AmtBuffer)buffer;

			var alignment = 16UL;

			// TODO: constant buffer alignment should be 256
			//if ((bBuffer.Usage & MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT) == MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT)
			//{
			//  https://developer.apple.com/reference/metal/mtlrendercommandencoder/1515829-setvertexbuffer
			//	For buffers in the device address space, the offset must be aligned to the data type consumed by the 
			//	vertex shader function (which is always less than or equal to 16 bytes).
			//  NOTE : VERTEX BUFFER data offset must be <= 16 bytes

			//	For buffers in the constant address space, the offset must be aligned to 256 bytes in macOS. In 
			//	iOS, the offset must be aligned to the maximum of either the data type consumed by the vertex shader 
			//	function, or 4 bytes.A 16 - byte alignment is always safe in iOS if you do not need to worry 
			//	about the data type.
			// NOTE : constant/uniform (i.e. readonly) buffer on macOS should be in 256 byte increments, constant on iOS
			// must be >= 16 bytes or max 
			//}

			pMemoryRequirements = new MgMemoryRequirements
			{
				Size = (ulong) bBuffer.Length,
				MemoryTypeBits = 1 << 0, 
				Alignment = alignment,
			};
		}

		public void GetDeviceMemoryCommitment(IMgDeviceMemory memory, ref ulong pCommittedMemoryInBytes)
		{
			throw new NotImplementedException();
		}

		public IntPtr GetDeviceProcAddr(string pName)
		{
			throw new NotImplementedException();
		}

		public void GetDeviceQueue(uint queueFamilyIndex, uint queueIndex, out IMgQueue pQueue)
		{
			pQueue = mQueue;
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
			foreach (var fence in pFences)
			{
				var bFence = (IAmtFence)fence;
				if (bFence != null)
				{
					bFence.Reset(0);
				}
			}

			return Result.SUCCESS;
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

					int arrayElement = (int)desc.DstArrayElement;

					// TODO: what do we do about multiple descriptors
					var count = (int)desc.DescriptorCount;

					AmtDescriptorSetUpdateKey result;
					if (localSet.Locator.TryGetValue(desc.DstBinding, out result))
					{
						foreach (var mask in new[] {
							MgShaderStageFlagBits.COMPUTE_BIT,
							MgShaderStageFlagBits.VERTEX_BIT,
							MgShaderStageFlagBits.FRAGMENT_BIT })
						{
							AmtDescriptorSetBindingMap map = null;
							uint bindingOffset = 0;
							uint samplerOffset = 0;
							if ((result.Stage & mask) == MgShaderStageFlagBits.COMPUTE_BIT)
							{
								map = localSet.Compute;
								bindingOffset = result.ComputeOffset;
								samplerOffset = result.ComputeSamplerIndex;
							}
							else if ((result.Stage & mask) == MgShaderStageFlagBits.VERTEX_BIT)
							{
								map = localSet.Vertex;
								bindingOffset = result.VertexOffset;
								samplerOffset = result.VertexSamplerIndex;
							}
							else if ((result.Stage & mask) == MgShaderStageFlagBits.FRAGMENT_BIT)
							{
								map = localSet.Fragment;
								bindingOffset = result.FragmentOffset;
								samplerOffset = result.FragmentSamplerIndex;
							}

							if (map != null)
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
											var localView = (IAmtImageView)info.ImageView;

											var imageIndex = bindingOffset + i;
											map.Textures[imageIndex].Texture = localView.GetTexture();

											var samplerIndex = samplerOffset + i;
											map.SamplerStates[samplerIndex].Sampler = localSampler.Sampler;
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

											ulong totalOffset = buf.BoundMemoryOffset + info.Offset;

											Debug.Assert(totalOffset <= nuint.MaxValue);

											var index = bindingOffset + i;
											map.Buffers[index].Buffer = buf.VertexBuffer;
											map.Buffers[index].BoundMemoryOffset = (nuint)totalOffset;

										}
										break;
									default:
										throw new NotSupportedException();
								}
							}
						}
					}

				}
			}
		}

		public Result WaitForFences(IMgFence[] pFences, bool waitAll, ulong timeout)
		{
			var fences = new List<IAmtFence>();
			var noOfFencesRequired = 0;
			if (waitAll)
			{
				foreach (var fence in pFences)
				{
					var bFence = (IAmtFence)fence;
					if (bFence != null)
					{
						fences.Add(bFence);;
					}
				}
				noOfFencesRequired = fences.Count;
			}
			else
			{
				noOfFencesRequired = 1;
			}

			var timer = new Stopwatch();
			var elapsedInNanoSecs = 0UL;

			do
			{
				var noOfFencesCompleted = 0;
				timer.Start();
				foreach (var fence in fences)
				{
					if (fence.AlreadySignalled)
					{
						noOfFencesCompleted += 1;
					}
				}
				timer.Stop();

			 	elapsedInNanoSecs = (ulong)((timer.ElapsedTicks * 1000000000) / Stopwatch.Frequency);

				if (noOfFencesCompleted >= noOfFencesRequired)
				{
					return Result.SUCCESS;
				}
			}
			while (elapsedInNanoSecs < timeout);

			return Result.TIMEOUT;
		}
	}
}
