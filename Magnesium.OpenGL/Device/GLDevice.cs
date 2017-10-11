using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Magnesium.OpenGL.Internals
{
    public class GLDevice : IMgDevice
	{
		#region IMgDevice implementation
		public IntPtr GetDeviceProcAddr (string pName)
		{
			throw new NotImplementedException ();		
		}

		public void DestroyDevice (IMgAllocationCallbacks allocator)
		{

		}

		private IGLQueue mQueue;
		private IGLDeviceEntrypoint mEntrypoint;
		public GLDevice (IGLQueue queue, IGLDeviceEntrypoint entrypoint)
		{
			mQueue = queue;
			mEntrypoint = entrypoint;
		}

		public void GetDeviceQueue (uint queueFamilyIndex, uint queueIndex, out IMgQueue pQueue)
		{
			pQueue = mQueue;
		}

		public Result DeviceWaitIdle ()
		{
			return Result.SUCCESS;
		}

		public Result AllocateMemory (MgMemoryAllocateInfo pAllocateInfo, IMgAllocationCallbacks allocator, out IMgDeviceMemory pMemory)
		{
			pMemory = mEntrypoint.DeviceMemory.CreateDeviceMemory(pAllocateInfo);
			return Result.SUCCESS;
		}

		public Result FlushMappedMemoryRanges (MgMappedMemoryRange[] pMemoryRanges)
		{
			throw new NotImplementedException ();
		}
		public Result InvalidateMappedMemoryRanges (MgMappedMemoryRange[] pMemoryRanges)
		{
			throw new NotImplementedException ();
		}
		public void GetDeviceMemoryCommitment (IMgDeviceMemory memory, ref ulong pCommittedMemoryInBytes)
		{
			throw new NotImplementedException ();
		}
		public void GetBufferMemoryRequirements (IMgBuffer buffer, out MgMemoryRequirements pMemoryRequirements)
		{
			var internalBuffer = buffer as IGLBuffer;
			if (internalBuffer == null)
			{
				throw new ArgumentException (nameof(buffer));
			}

            uint mask = DetermineBufferMemoryType(internalBuffer.Usage);
			pMemoryRequirements = new MgMemoryRequirements {
				Size = internalBuffer.RequestedSize,
				MemoryTypeBits = mask,
			};
		}

        private uint DetermineBufferMemoryType(MgBufferUsageFlagBits usage)
        {
            var flags = MgBufferUsageFlagBits.STORAGE_BUFFER_BIT;
            // 1st precedence
            if ((usage & flags) == flags)
            {
                return GLMemoryBufferType.SSBO.GetMask();
            }

            flags = MgBufferUsageFlagBits.VERTEX_BUFFER_BIT;
            if ((usage & flags) == flags)
            {
                return GLMemoryBufferType.VERTEX.GetMask();
            }

            flags = MgBufferUsageFlagBits.INDIRECT_BUFFER_BIT;
            if ((usage & flags) == flags)
            {
                return GLMemoryBufferType.INDIRECT.GetMask();
            }

            flags = MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT;
            if ((usage & flags) == flags)
            {
                return GLMemoryBufferType.UNIFORM.GetMask();
            }

            flags = MgBufferUsageFlagBits.INDEX_BUFFER_BIT;
            if ((usage & flags) == flags)
            {
                return GLMemoryBufferType.INDEX.GetMask();
            }

            flags = MgBufferUsageFlagBits.TRANSFER_DST_BIT;
            if ((usage & flags) == flags)
            {
                return GLMemoryBufferType.TRANSFER_DST.GetMask();
            }

            flags = MgBufferUsageFlagBits.TRANSFER_SRC_BIT;
            if ((usage & flags) == flags)
            {
                return GLMemoryBufferType.TRANSFER_SRC.GetMask();
            }
            else
            {
                throw new NotSupportedException("BufferMemoryType not supported");
            }
        }

        internal static int CalculateMipLevels(int width, int height = 0, int depth = 0)
		{
			int levels = 1;
			int size = Math.Max(Math.Max(width, height), depth);
			while (size > 1)
			{
				size = size / 2;
				levels++;
			}
			return levels;
		}

		public void GetImageMemoryRequirements (IMgImage image, out MgMemoryRequirements memoryRequirements)
		{
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

			var texture = (IGLImage) image;

			uint imageSize = 0;

			uint width = (uint) texture.Width;
			uint height = (uint) texture.Height;

			uint size = Math.Max(width, height);

			for(int i = 0; i < texture.Levels; ++i)
			{		
				Debug.Assert (size >= 1);

				var format = texture.Format;
				switch (format)
				{
				// FIXME : 
				//				//case SurfaceFormat.RgbPvrtc2Bpp:
				//				case SurfaceFormat.RgbaPvrtc2Bpp:
				//					imageSize = (Math.Max(this.width, 16) * Math.Max(this.height, 8) * 2 + 7) / 8;
				//					break;
				//				case SurfaceFormat.RgbPvrtc4Bpp:
				//				case SurfaceFormat.RgbaPvrtc4Bpp:
				//					imageSize = (Math.Max(this.width, 8) * Math.Max(this.height, 8) * 4 + 7) / 8;
				//					break;
				case MgFormat.BC1_RGB_UNORM_BLOCK:
				//case SurfaceFormat.Dxt1:
				case MgFormat.BC1_RGBA_UNORM_BLOCK:
				//case SurfaceFormat.Dxt1a:
				case MgFormat.BC1_RGB_SRGB_BLOCK:
				//case SurfaceFormat.Dxt1SRgb:
				case MgFormat.BC2_UNORM_BLOCK:
				//case SurfaceFormat.Dxt3:
				case MgFormat.BC2_SRGB_BLOCK:				
				//case SurfaceFormat.Dxt3SRgb:
				case MgFormat.BC3_UNORM_BLOCK:
				//case SurfaceFormat.Dxt5:
				case MgFormat.BC3_SRGB_BLOCK:
				//case SurfaceFormat.Dxt5SRgb:
				//case SurfaceFormat.RgbEtc1:
				//case SurfaceFormat.RgbaAtcExplicitAlpha:
				//case SurfaceFormat.RgbaAtcInterpolatedAlpha:
					imageSize += ((width + 3) / 4) * ((height + 3) / 4) * GetSize (format);
					break;
				default:
					imageSize += GetSize (format) * width * height;
					break;
					//return Result.ERROR_FEATURE_NOT_PRESENT;
				}

				if (width > 1)
					width = width / 2;

				if (height > 1)
					height = height / 2;
			}

			memoryRequirements = new MgMemoryRequirements {	
				// HOST ONLY OR DEVICE 
				MemoryTypeBits  = GLMemoryBufferType.IMAGE.GetMask(),
				Size = imageSize,
			};
		}


		public void GetImageSparseMemoryRequirements (IMgImage image, out MgSparseImageMemoryRequirements[] sparseMemoryRequirements)
		{
			throw new NotImplementedException ();
		}
		public Result CreateFence (MgFenceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFence fence)
		{
            fence = mEntrypoint.Fence.CreateFence();
            return Result.SUCCESS;
		}

		public Result ResetFences (IMgFence[] pFences)
		{
			foreach(var fence in pFences)
            {
                IGLFence bFence = (IGLFence) fence;
                bFence.Reset();
            }
            return Result.SUCCESS;                
		}
		public Result GetFenceStatus (IMgFence fence)
		{
            IGLFence bFence = (IGLFence)fence;
            return (bFence.IsSignalled) ? Result.SUCCESS : Result.NOT_READY;
        }

		public Result WaitForFences (IMgFence[] pFences, bool waitAll, ulong timeout)
		{
            if (timeout == 0)
            {
                // report current state only

                // NO WAITING AT ALL 
                foreach (var fence in pFences)
                {
                    IGLFence bFence = (IGLFence)fence;
                    // TODO MAYBE NON BLOCKING ONLY
                    if (!bFence.IsSignalled)
                    {
                        return Result.TIMEOUT;
                    }
                }
                return Result.SUCCESS;
            }
            else
            {
                Stopwatch timer = new Stopwatch();

                var noOfFences = pFences.Length;

                ulong elapsedInNanoSecs = 0UL;
                var remainingTime = timeout;

                var currentFence = 0;
                do
                {
                    IGLFence bFence = (IGLFence)pFences[currentFence];
                    timer.Start();
                    // ONE AT A TIME
                    var isSignalled = bFence.IsReady((long)remainingTime);

                    timer.Stop();
 
                    elapsedInNanoSecs = (ulong) ((timer.ElapsedTicks * 1000000000) / Stopwatch.Frequency);

                    // remove elapsed time from timeout
                    remainingTime -= elapsedInNanoSecs;
                    if (isSignalled)
                    {
                        currentFence += 1;
                    }

                    // COMPLETED LAST FENCE
                    if (currentFence >= noOfFences)
                    {
                        return Result.SUCCESS;
                    }
                }
                while (elapsedInNanoSecs < timeout);

                return Result.TIMEOUT;
            }            

		}
		public Result CreateSemaphore (MgSemaphoreCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSemaphore pSemaphore)
		{
			pSemaphore = mEntrypoint.Semaphore.CreateSemaphore();
			return Result.SUCCESS;
		}

		public Result CreateEvent (MgEventCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgEvent @event)
		{
			throw new NotImplementedException ();
		}
		public void DestroyEvent (IMgEvent @event, IMgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result GetEventStatus (IMgEvent @event)
		{
			throw new NotImplementedException ();
		}
		public Result SetEvent (IMgEvent @event)
		{
			throw new NotImplementedException ();
		}
		public Result ResetEvent (IMgEvent @event)
		{
			throw new NotImplementedException ();
		}
		public Result CreateQueryPool (MgQueryPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgQueryPool queryPool)
		{
			throw new NotImplementedException ();
		}
		public void DestroyQueryPool (IMgQueryPool queryPool, IMgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}
		public Result GetQueryPoolResults (IMgQueryPool queryPool, uint firstQuery, uint queryCount, IntPtr dataSize, IntPtr pData, ulong stride, MgQueryResultFlagBits flags)
		{
			throw new NotImplementedException ();
		}
		public Result CreateBuffer (MgBufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBuffer pBuffer)
		{
			pBuffer = mEntrypoint.Buffers.CreateBuffer(pCreateInfo);
			return Result.SUCCESS;
		}

		public Result CreateBufferView (MgBufferViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBufferView pView)
		{
			throw new NotImplementedException ();
		}
		public void DestroyBufferView (IMgBufferView bufferView, IMgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}

		private static uint GetSize(MgFormat surfaceFormat)
		{
			switch (surfaceFormat)
			{
			case MgFormat.BC1_RGB_UNORM_BLOCK:
			//case SurfaceFormat.Dxt1:
			case MgFormat.BC1_RGB_SRGB_BLOCK:
			//case SurfaceFormat.Dxt1SRgb:
			case MgFormat.BC1_RGBA_UNORM_BLOCK:
			//case SurfaceFormat.Dxt1a:
			//case SurfaceFormat.RgbPvrtc2Bpp:
			//case SurfaceFormat.RgbaPvrtc2Bpp:			
			//case SurfaceFormat.RgbEtc1:
				// One texel in DXT1, PVRTC 2bpp and ETC1 is a minimum 4x4 block, which is 8 bytes
				return 8;
			case MgFormat.BC2_UNORM_BLOCK:
			//case SurfaceFormat.Dxt3:
			case MgFormat.BC2_SRGB_BLOCK:
			//case SurfaceFormat.Dxt3SRgb:
			case MgFormat.BC3_UNORM_BLOCK:
			//case SurfaceFormat.Dxt5:
			case MgFormat.BC3_SRGB_BLOCK:
			//case SurfaceFormat.Dxt5SRgb:
			//case SurfaceFormat.RgbPvrtc4Bpp:
			//case SurfaceFormat.RgbaPvrtc4Bpp:
			//case SurfaceFormat.RgbaAtcExplicitAlpha:
			//case SurfaceFormat.RgbaAtcInterpolatedAlpha:
				// One texel in DXT3, DXT5 and PVRTC 4bpp is a minimum 4x4 block, which is 16 bytes
				return 16;
			case MgFormat.R8_UNORM:
			//case SurfaceFormat.Alpha8:
				return 1;
			case MgFormat.B5G6R5_UNORM_PACK16:
			//case SurfaceFormat.Bgr565:
			case MgFormat.B4G4R4A4_UNORM_PACK16:
			//case SurfaceFormat.Bgra4444:
			case MgFormat.B5G5R5A1_UNORM_PACK16:
			//case SurfaceFormat.Bgra5551:
			case MgFormat.R16_SFLOAT:
			//case SurfaceFormat.HalfSingle:
			case MgFormat.R16_UNORM:
			//case SurfaceFormat.NormalizedByte2:
				return 2;
			case MgFormat.R8G8B8A8_UINT:
				//case SurfaceFormat.Color:
			case MgFormat.R8G8B8A8_SRGB:
				//case SurfaceFormat.ColorSRgb:
			case MgFormat.R32_SFLOAT:
				//case SurfaceFormat.Single:
			case MgFormat.R16G16_UINT:
				//case SurfaceFormat.Rg32:
			case MgFormat.R16G16_SFLOAT:
				//case SurfaceFormat.HalfVector2:
			case MgFormat.R8G8B8A8_SNORM:
				//case SurfaceFormat.NormalizedByte4:
			case MgFormat.A2B10G10R10_UINT_PACK32:
				//case SurfaceFormat.Rgba1010102:
				//case SurfaceFormat.Bgra32:
				//case SurfaceFormat.Bgra32SRgb:
				//case SurfaceFormat.Bgr32:
				//case SurfaceFormat.Bgr32SRgb:
				return 4;
			case MgFormat.R16G16B16A16_SFLOAT:				
				//case SurfaceFormat.HalfVector4:
				//case SurfaceFormat.Rgba64:
			case MgFormat.R32G32_SFLOAT:
				//case SurfaceFormat.Vector2:
				return 8;
			case MgFormat.R32G32B32A32_SFLOAT:				
				//case SurfaceFormat.Vector4:
				return 16;
			case MgFormat.R8G8B8_SRGB:
			case MgFormat.R8G8B8_SSCALED:
			case MgFormat.R8G8B8_UINT:
			case MgFormat.R8G8B8_UNORM:
			case MgFormat.R8G8B8_USCALED:
			case MgFormat.R8G8B8_SINT:
			case MgFormat.R8G8B8_SNORM:
				return 3;
			default:
				throw new ArgumentException();
			}
		}

		public Result CreateImage (MgImageCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgImage pImage)
		{
			if (pCreateInfo == null)
			{
				throw new ArgumentNullException ("pCreateInfo");
			}

			// ARB_texture_storage
			int[] textureId = new int[1];

			int width = (int) pCreateInfo.Extent.Width;
			int height = (int) pCreateInfo.Extent.Height;
			int depth = (int) pCreateInfo.Extent.Depth;
			int levels = (int) pCreateInfo.MipLevels;
			int arrayLayers = (int)pCreateInfo.ArrayLayers;
			//var internalFormat = GetInternalFormat(pCreateInfo.Format);

			var imageType = pCreateInfo.ImageType;

			switch (pCreateInfo.ImageType)
			{
			case MgImageType.TYPE_1D:
//				GL.CreateTextures (TextureTarget.Texture1D, 1, textureId);
//				GL.Ext.TextureStorage1D (textureId [0], (ExtDirectStateAccess)All.Texture1D, levels, internalFormat, width);
				textureId[0] = mEntrypoint.Image.CreateTextureStorage1D (levels, pCreateInfo.Format, width);
				break;
			case MgImageType.TYPE_2D:
//				GL.CreateTextures (TextureTarget.Texture2D, 1, textureId);
//				GL.Ext.TextureStorage2D (textureId[0], (ExtDirectStateAccess)All.Texture2D, levels, internalFormat, width, height);
				textureId[0] = mEntrypoint.Image.CreateTextureStorage2D (levels, pCreateInfo.Format, width, height);
				break;
			case MgImageType.TYPE_3D:
//				GL.CreateTextures (TextureTarget.Texture3D, 1, textureId);
//				GL.Ext.TextureStorage3D (textureId [0], (ExtDirectStateAccess)All.Texture3D, levels, internalFormat, width, height, depth);
				textureId [0] = mEntrypoint.Image.CreateTextureStorage3D (levels, pCreateInfo.Format, width, height, depth);
				break;
			default:				
				throw new NotSupportedException ();
			}

			pImage = new GLImage(mEntrypoint.Image, textureId[0], imageType, pCreateInfo.Format, width, height, depth, levels, arrayLayers);
			return Result.SUCCESS;
		}

//		public void DestroyImage (MgImage image, IMgAllocationCallbacks allocator)
//		{
//			mImages [image.Key].Destroy ();
//		}

		public void GetImageSubresourceLayout (IMgImage image, MgImageSubresource pSubresource, out MgSubresourceLayout pLayout)
		{
			var internalImage = (IGLImage) image;

			if (internalImage != null
				&& pSubresource.ArrayLayer < internalImage.ArrayLayers.Length 
				&& pSubresource.MipLevel < internalImage.ArrayLayers[pSubresource.ArrayLayer].Levels.Length)
			{
				pLayout = internalImage.ArrayLayers [pSubresource.ArrayLayer].Levels [pSubresource.MipLevel].SubresourceLayout;
			}
			else
			{
				pLayout = new MgSubresourceLayout {};
			}
		}

		public Result CreateImageView (MgImageViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgImageView pView)
		{
			if (pCreateInfo == null)
			{
				throw new ArgumentNullException ("pCreateInfo");
			}

			if (pCreateInfo.Image == null)
			{
				throw new ArgumentNullException ("pCreateInfo.Image", "pCreateInfo.Image is null");
			}

			var originalImage = (IGLImage) pCreateInfo.Image;

			if (originalImage == null)
			{
				throw new InvalidCastException ("pCreateInfo.Image is not GLImage");
			}

			if (pCreateInfo.SubresourceRange == null)
			{
				throw new ArgumentNullException ("pCreateInfo.SubresourceRange", "pCreateInfo.SubresourceRange is null");
			}

			int textureId =  mEntrypoint.ImageView.CreateImageView (originalImage, pCreateInfo);
			pView = new GLImageView (textureId, mEntrypoint.ImageView);
			return Result.SUCCESS;
		}

		public Result CreateShaderModule (MgShaderModuleCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgShaderModule pShaderModule)
		{			
			pShaderModule = new GLShaderModule (pCreateInfo, mEntrypoint.ShaderModule);
			return Result.SUCCESS;
		}

		public Result CreatePipelineCache (MgPipelineCacheCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineCache pPipelineCache)
		{
			throw new NotImplementedException ();
		}

		public Result GetPipelineCacheData (IMgPipelineCache pipelineCache, out byte[] pData)
		{
			throw new NotImplementedException ();
		}
		public Result MergePipelineCaches (IMgPipelineCache dstCache, IMgPipelineCache[] pSrcCaches)
		{
			throw new NotImplementedException ();
		}

		public Result CreateGraphicsPipelines (IMgPipelineCache pipelineCache, MgGraphicsPipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines)
		{
			var output = new List<IMgPipeline> ();

			foreach (var info in pCreateInfos)
            {
                var bLayout = (IGLPipelineLayout)info.Layout;
                if (bLayout == null)
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

                var programId = mEntrypoint.GraphicsCompiler.Compile(info);

                var blocks = mEntrypoint.GraphicsCompiler.Inspect(programId);
                var arrayMapper = new GLInternalCacheArrayMapper(bLayout, blocks);
                var internalCache = new GLInternalCache(bLayout, blocks, arrayMapper);

                /// MAKE SURE ACTIVE UNIFORMS ARE AVAILABLE
                //int noOfActiveUniforms = mEntrypoint.GraphicsPipeline.GetActiveUniforms(programId);

                // var names = mEntrypoint.GraphicsPipeline.GetUniformBlocks(programId);

                //var binder = ConstructBinder(bLayout, programId, noOfActiveUniforms);

                var pipeline = new GLGraphicsPipeline(
                    mEntrypoint.GraphicsPipeline,
                    programId,
                    info,
                    internalCache,
                    bLayout
                );

                // TODO : BASE PIPELINE / CHILD

                output.Add(pipeline);
            }
            pPipelines = output.ToArray ();
			return Result.SUCCESS;
		}

        //private GLProgramUniformBinder ConstructBinder(IGLPipelineLayout bLayout, int programId)
        //{
        //
        //    int noOfActiveUniforms = mEntrypoint.GraphicsPipeline.GetActiveUniforms(programId);
        //    var notUniformBlock = ~(MgDescriptorType.UNIFORM_BUFFER | MgDescriptorType.UNIFORM_BUFFER_DYNAMIC);
        //    var uniqueLocations = new SortedDictionary<uint, GLVariableBind>();

        //    foreach (var binding in bLayout.Bindings)
        //    {
        //        bool uniformFound = false;


        //        if (noOfActiveUniforms > 0)
        //        {
        //            if (binding.Binding > int.MaxValue)
        //            {
        //                throw new ArgumentOutOfRangeException("Mg.GL: binding.Binding is > int.MaxValue");
        //            }

        //            // NOT SURE IF THIS IS STILL WORTH CHECKING
        //            if ((binding.DescriptorType & notUniformBlock) > 0)
        //            {
        //                int location = (int)binding.Binding;
        //                uniformFound = mEntrypoint.GraphicsPipeline.CheckUniformLocation(programId, location);
        //            }
        //        }

        //        // ONLY ACTIVE UNIFORMS
        //        // FIXME : input attachment
        //        var bind = new GLVariableBind
        //        {
        //            IsActive = (noOfActiveUniforms > 0 && uniformFound),
        //            Location = binding.Binding,
        //            DescriptorType = binding.DescriptorType
        //        };

        //        // WILL THROW ERROR HERE IF COLLISION
        //        uniqueLocations.Add(binding.Binding, bind);
        //    }

        //    // ASSUME NO GAPS ARE SUPPLIED
        //    var uniformBinder = new GLProgramUniformBinder(uniqueLocations.Values.Count);
        //    foreach (var bind in uniqueLocations.Values)
        //    {
        //        uniformBinder.Bindings[bind.Location] = bind;
        //    }
        //    return uniformBinder;
        //}

        public Result CreateComputePipelines (IMgPipelineCache pipelineCache, MgComputePipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines)
		{
			throw new NotImplementedException ();
		}

		public Result CreatePipelineLayout (MgPipelineLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineLayout pPipelineLayout)
		{
			if (pCreateInfo == null)
			{
				throw new ArgumentNullException ("pCreateInfo");
			}

			if (pCreateInfo.SetLayouts == null)
			{
				throw new ArgumentNullException ("pCreateInfo.SetLayouts");
			}

			if (pCreateInfo.SetLayouts.Length > 1)
			{
				throw new NotSupportedException ("DESKTOPGL - SetLayouts must be <= 1");
			}

			pPipelineLayout = new GLNextPipelineLayout (pCreateInfo);
			return Result.SUCCESS;
		}

		public Result CreateSampler (MgSamplerCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSampler pSampler)
		{
			pSampler = new GLSampler (mEntrypoint.Sampler.CreateSampler (), pCreateInfo, mEntrypoint.Sampler);
			return Result.SUCCESS;
		}

		public Result CreateDescriptorSetLayout (MgDescriptorSetLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorSetLayout pSetLayout)
		{
			if (pCreateInfo == null)
			{
				throw new ArgumentNullException ("pCreateInfo");
			}
			pSetLayout  = new GLDescriptorSetLayout (pCreateInfo); 
			return Result.SUCCESS;
		}

		public Result CreateDescriptorPool (MgDescriptorPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorPool pDescriptorPool)
		{
			pDescriptorPool = mEntrypoint.DescriptorPool.CreatePool (pCreateInfo);
			return Result.SUCCESS;
		}

		public Result ResetDescriptorPool (IMgDescriptorPool descriptorPool, uint flags)
		{
			throw new NotImplementedException ();
		}

    	public Result AllocateDescriptorSets (MgDescriptorSetAllocateInfo pAllocateInfo, out IMgDescriptorSet[] pDescriptorSets)
		{
            return mEntrypoint.DescriptorSet.Allocate(pAllocateInfo, out pDescriptorSets);
		}

		public Result FreeDescriptorSets (IMgDescriptorPool descriptorPool, IMgDescriptorSet[] pDescriptorSets)
		{
            return mEntrypoint.DescriptorSet.Free(descriptorPool, pDescriptorSets);
		}

		public void UpdateDescriptorSets (MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies)
		{
            mEntrypoint.DescriptorSet.Update(pDescriptorWrites, pDescriptorCopies);
		}

		public Result CreateFramebuffer (MgFramebufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFramebuffer pFramebuffer)
		{
			pFramebuffer = new GLFramebuffer ();
			return Result.SUCCESS;
		}

		public Result CreateRenderPass (MgRenderPassCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgRenderPass pRenderPass)
		{
			pRenderPass = new GLRenderPass (pCreateInfo.Attachments);
			return Result.SUCCESS;
		}

		public void GetRenderAreaGranularity (IMgRenderPass renderPass, out MgExtent2D pGranularity)
		{
			throw new NotImplementedException ();
		}
		public Result CreateCommandPool (MgCommandPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgCommandPool pCommandPool)
		{
			pCommandPool = new GLCommandPool (pCreateInfo.Flags);
			return Result.SUCCESS;
		}

		public Result AllocateCommandBuffers (MgCommandBufferAllocateInfo pAllocateInfo, IMgCommandBuffer[] pCommandBuffers)
		{			
			var cmdPool = pAllocateInfo.CommandPool as GLCommandPool;

			if (cmdPool == null)
			{
				throw new InvalidCastException ("pAllocateInfo.CommandPool");
			}

			for (uint i = 0; i < pAllocateInfo.CommandBufferCount; ++i)
			{
                // TODO : for now
                var sorter = new GLCmdIncrementalContextSorter();
                var dsBinder = new GLNextDescriptorSetBinder();
                var graphics = new GLCmdGraphicsEncoder(sorter, new GLCmdGraphicsBag(), mEntrypoint.VBO, dsBinder);
                var compute = new GLCmdComputeEncoder();
                var blit = new GLCmdBlitEncoder(sorter, new GLCmdBlitBag());
                var encoder = new GLCmdCommandEncoder(sorter, graphics, compute, blit);


				var buffer = new GLCmdCommandBuffer(true, encoder);
				cmdPool.Buffers.Add (buffer);
				pCommandBuffers [i] = buffer;
			}

			return Result.SUCCESS;
		}
		public void FreeCommandBuffers (IMgCommandPool commandPool, IMgCommandBuffer[] pCommandBuffers)
		{
			foreach (var item in pCommandBuffers)
			{
				var cmdBuf = (IGLCommandBuffer) item;
				cmdBuf.ResetAllData ();
			}
		}
		public Result CreateSharedSwapchainsKHR (MgSwapchainCreateInfoKHR[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgSwapchainKHR[] pSwapchains)
		{
			throw new NotImplementedException ();
		}
		public Result CreateSwapchainKHR (MgSwapchainCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSwapchainKHR pSwapchain)
		{
			throw new NotImplementedException ();
		}

		public Result GetSwapchainImagesKHR (IMgSwapchainKHR swapchain, out IMgImage[] pSwapchainImages)
		{
			throw new NotImplementedException ();
		}
		public Result AcquireNextImageKHR (IMgSwapchainKHR swapchain, ulong timeout, IMgSemaphore semaphore, IMgFence fence, out uint pImageIndex)
		{
			if (swapchain == null)
			{
				throw new ArgumentNullException ("swapchain");
			}

			var sc = swapchain as IGLSwapchainKHR;
			if (sc == null)
			{
				throw new InvalidCastException ("swapchain is not GLSwapchainKHR");
			}

			pImageIndex = sc.GetNextImage ();
			// TODO : fence stuff
			return Result.SUCCESS;
		}
		#endregion
		
	}
}

