using System;
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
			throw new NotImplementedException();
		}

		public Result AllocateDescriptorSets(MgDescriptorSetAllocateInfo pAllocateInfo, out IMgDescriptorSet[] pDescriptorSets)
		{
			throw new NotImplementedException();
		}

		public Result AllocateMemory(MgMemoryAllocateInfo pAllocateInfo, IMgAllocationCallbacks allocator, out IMgDeviceMemory pMemory)
		{
			throw new NotImplementedException();
		}

		public Result CreateBuffer(MgBufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBuffer pBuffer)
		{
			nuint length = 0;
			var buffer = mDevice.CreateBuffer(length, MTLResourceOptions.CpuCacheModeDefault);
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}

		public Result CreateDescriptorPool(MgDescriptorPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorPool pDescriptorPool)
		{
			throw new NotImplementedException();
		}

		public Result CreateDescriptorSetLayout(MgDescriptorSetLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorSetLayout pSetLayout)
		{
			throw new NotImplementedException();
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
			var output = new List<IMgPipeline>();

			foreach (var info in pCreateInfos)
			{
				var layout = (AmtPipelineLayout) info.Layout;
				if (layout == null)
				{
					throw new ArgumentException("pCreateInfos[].Layout");
				}

				if (info.VertexInputState == null)
				{
					throw new ArgumentNullException("pCreateInfos[].VertexInputState");
				}

				if (info.InputAssemblyState == null)
				{
					throw new ArgumentNullException("pCreateInfos[].InputAssemblyState");
				}

				if (info.RasterizationState == null)
				{
					throw new ArgumentNullException("pCreateInfos[].RasterizationState");
				}

				if (info.RenderPass == null)
				{
					throw new ArgumentNullException("pCreateInfos[].RenderPass");
				}

				var renderPass = (AmtRenderPass) info.RenderPass;

				var sampleCount = (info.MultisampleState != null) ? TranslateSampleCount(info.MultisampleState.RasterizationSamples) : 1; 

				// Create a reusable pipeline state
				var pipelineStateDescriptor = new MTLRenderPipelineDescriptor
				{
					SampleCount = sampleCount,
					VertexFunction = vertexProgram,
					FragmentFunction = fragmentProgram,
					VertexDescriptor = vertexDescriptor,
				};

				nint colorAttachmentIndex = 0;
				foreach (var attachment in renderPass.ClearAttachments)
				{
					if (attachment.Destination == AmtRenderPassAttachmentDestination.COLOR)
					{
						pipelineStateDescriptor.ColorAttachments[colorAttachmentIndex].PixelFormat = TranslateFormat(attachment.Format);

					}
					else if (attachment.Destination == AmtRenderPassAttachmentDestination.DEPTH_AND_STENCIL)
					{
						pipelineStateDescriptor.DepthAttachmentPixelFormat = TranslateFormat(attachment.Format);
						pipelineStateDescriptor.StencilAttachmentPixelFormat = TranslateFormat(attachment.Format);
					}
				}

				Foundation.NSError error;
				var pipelineState = mDevice.CreateRenderPipelineState(pipelineStateDescriptor, out error);
				if (pipelineState == null)
					Console.WriteLine("Failed to created pipeline state, error {0}", error);

				var depthStateDesc = new MTLDepthStencilDescriptor
				{
					DepthCompareFunction = MTLCompareFunction.Less,
					DepthWriteEnabled = true
				};

				depthState = device.CreateDepthStencilState(depthStateDesc);
			}
		}

		private static MTLPixelFormat TranslateFormat(MgFormat format)
		{
			switch (format)
			{
				default:
					throw new NotSupportedException();
				case MgFormat.UNDEFINED:
					return MTLPixelFormat.Invalid;
				//A8Unorm,
				case MgFormat.R8_SRGB:
					return MTLPixelFormat.R8Unorm_sRGB;
				case MgFormat.R8_UNORM:
					return MTLPixelFormat.R8Unorm;
				case MgFormat.R8_SNORM:
					return MTLPixelFormat.R8Snorm;
				case MgFormat.R8_UINT:
					return MTLPixelFormat.R8Uint;
				case MgFormat.R8_SINT:
					return MTLPixelFormat.R8Sint;
				case MgFormat.R16_UNORM:
					return MTLPixelFormat.R16Unorm;
				case MgFormat.R16_SNORM:
					return MTLPixelFormat.R16Snorm;
				case MgFormat.R16_UINT:
					return MTLPixelFormat.R16Uint;
				case MgFormat.R16_SINT:
					return MTLPixelFormat.R16Sint;
				case MgFormat.R16_SFLOAT:
					return MTLPixelFormat.R16Float;
				case MgFormat.R8G8_UNORM:
					return MTLPixelFormat.RG8Unorm;
				case MgFormat.R8G8_SNORM:
					return MTLPixelFormat.RG8Snorm;
				case MgFormat.R8G8_UINT:
					return MTLPixelFormat.RG8Uint;
				case MgFormat.R8G8_SINT:
					return MTLPixelFormat.RG8Sint;
				case MgFormat.R32_UINT:
					return MTLPixelFormat.R32Uint;
				case MgFormat.R32_SINT:
					return MTLPixelFormat.R32Sint;
				case MgFormat.R32_SFLOAT:
					return MTLPixelFormat.R32Float;
				case MgFormat.R16G16_UNORM:
					return MTLPixelFormat.RG16Unorm;
				case MgFormat.R16G16_SNORM:
					return MTLPixelFormat.RG16Snorm;
				case MgFormat.R16G16_UINT:
					return MTLPixelFormat.RG16Uint;
				case MgFormat.R16G16_SINT:
					return MTLPixelFormat.RG16Sint;
				case MgFormat.R16G16_SFLOAT:
					return MTLPixelFormat.RG16Float;
				case MgFormat.R8G8B8A8_UNORM:
					return MTLPixelFormat.RGBA8Unorm;
				case MgFormat.R8G8B8A8_SRGB:
					return MTLPixelFormat.RGBA8Unorm_sRGB;
				case MgFormat.R8G8B8A8_SNORM:
					return MTLPixelFormat.RGBA8Snorm;
				case MgFormat.R8G8B8A8_UINT:
					return MTLPixelFormat.RGBA8Uint;
				case MgFormat.R8G8B8A8_SINT:
					return MTLPixelFormat.RGBA8Sint;
				case MgFormat.B8G8R8A8_UNORM:
					return MTLPixelFormat.BGRA8Unorm;
				case MgFormat.B8G8R8A8_SRGB:
					return MTLPixelFormat.BGRA8Unorm_sRGB;
				case MgFormat.A2R10G10B10_UNORM_PACK32:
					return MTLPixelFormat.RGB10A2Unorm;
				case MgFormat.A2R10G10B10_UINT_PACK32:
					return MTLPixelFormat.RGB10A2Uint;
				case MgFormat.B10G11R11_UFLOAT_PACK32:
					return MTLPixelFormat.RG11B10Float;
				case MgFormat.E5B9G9R9_UFLOAT_PACK32:
					return MTLPixelFormat.RGB9E5Float;
				case MgFormat.R32G32_UINT:
					return MTLPixelFormat.RG32Uint;
				case MgFormat.R32G32_SINT:
					return MTLPixelFormat.RG32Sint;
				case MgFormat.R32G32_SFLOAT:
					return MTLPixelFormat.RG32Float;
				case MgFormat.R16G16B16A16_UNORM:
					return MTLPixelFormat.RGBA16Unorm;
				case MgFormat.R16G16B16A16_SNORM:
					return MTLPixelFormat.RGBA16Snorm;
				case MgFormat.R16G16B16A16_UINT:
					return MTLPixelFormat.RGBA16Uint;
				case MgFormat.R16G16B16A16_SINT:
					return MTLPixelFormat.RGBA16Sint;
				case MgFormat.R16G16B16A16_SFLOAT:
					return MTLPixelFormat.RGBA16Float;
				case MgFormat.R32G32B32A32_UINT:
					return MTLPixelFormat.RGBA32Uint;
				case MgFormat.R32G32B32A32_SINT:
					return MTLPixelFormat.RGBA32Sint;
				case MgFormat.R32G32B32A32_SFLOAT:
					return MTLPixelFormat.RGBA32Float;
				case MgFormat.BC1_RGBA_UNORM_BLOCK:
					return MTLPixelFormat.BC1RGBA;
				case MgFormat.BC1_RGBA_SRGB_BLOCK:
					return MTLPixelFormat.BC1_RGBA_sRGB;
				case MgFormat.BC2_UNORM_BLOCK:
					return MTLPixelFormat.BC2RGBA;
				case MgFormat.BC2_SRGB_BLOCK:
					return MTLPixelFormat.BC2_RGBA_sRGB;
				case MgFormat.BC3_UNORM_BLOCK:
					return MTLPixelFormat.BC3RGBA;
				case MgFormat.BC3_SRGB_BLOCK:
					return MTLPixelFormat.BC3_RGBA_sRGB;
				case MgFormat.BC4_UNORM_BLOCK:
					return MTLPixelFormat.BC4_RUnorm;
				case MgFormat.BC4_SNORM_BLOCK:
					return MTLPixelFormat.BC4_RSnorm;
				case MgFormat.BC5_UNORM_BLOCK:
					return MTLPixelFormat.BC5_RGUnorm;
				case MgFormat.BC5_SNORM_BLOCK:
					return MTLPixelFormat.BC5_RGSnorm;
				case MgFormat.BC6H_SFLOAT_BLOCK:
					return MTLPixelFormat.BC6H_RGBFloat;
				case MgFormat.BC6H_UFLOAT_BLOCK:
					return MTLPixelFormat.BC6H_RGBUFloat;
				case MgFormat.BC7_UNORM_BLOCK:
					return MTLPixelFormat.BC7_RGBAUnorm;
				case MgFormat.BC7_SRGB_BLOCK:
					return MTLPixelFormat.BC7_RGBAUnorm_sRGB;
				//GBGR422 = 240uL,
				//BGRG422,
				case MgFormat.D32_SFLOAT:
					return MTLPixelFormat.Depth32Float;
				case MgFormat.S8_UINT:
					return MTLPixelFormat.Stencil8;
				case MgFormat.D24_UNORM_S8_UINT:
					return MTLPixelFormat.Depth24Unorm_Stencil8;
				case MgFormat.D32_SFLOAT_S8_UINT:
					return MTLPixelFormat.Depth32Float_Stencil8;
			}
		}

		private static nuint TranslateSampleCount(MgSampleCountFlagBits count)
		{
			switch (count)
			{
				case MgSampleCountFlagBits.COUNT_1_BIT:
					return 1;
				case MgSampleCountFlagBits.COUNT_4_BIT:
					return 4;
				case MgSampleCountFlagBits.COUNT_2_BIT:
					return 2;
				case MgSampleCountFlagBits.COUNT_8_BIT:
					return 8;
				case MgSampleCountFlagBits.COUNT_16_BIT:
					return 16;
				case MgSampleCountFlagBits.COUNT_32_BIT:
					return 32;
				case MgSampleCountFlagBits.COUNT_64_BIT:
					return 64;
				default:
					throw new NotSupportedException();
			}
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
				PixelFormat = TranslateFormat(pCreateInfo.Format),
				SampleCount = TranslateSampleCount(pCreateInfo.Samples),
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
			throw new NotImplementedException();
		}

		public Result CreatePipelineCache(MgPipelineCacheCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineCache pPipelineCache)
		{
			throw new NotImplementedException();
		}

		public Result CreatePipelineLayout(MgPipelineLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineLayout pPipelineLayout)
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}

		public Result CreateSemaphore(MgSemaphoreCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSemaphore pSemaphore)
		{
			throw new NotImplementedException();
		}

		public Result CreateShaderModule(MgShaderModuleCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgShaderModule pShaderModule)
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}

		public Result WaitForFences(IMgFence[] pFences, bool waitAll, ulong timeout)
		{
			throw new NotImplementedException();
		}
	}
}
