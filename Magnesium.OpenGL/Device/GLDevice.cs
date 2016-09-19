using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace Magnesium.OpenGL
{
	public class GLDevice : IMgDevice
	{
		#region IMgDevice implementation
		public PFN_vkVoidFunction GetDeviceProcAddr (string pName)
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
//		public void FreeMemory (IMgDeviceMemory memory, IMgAllocationCallbacks allocator)
//		{
//			throw new NotImplementedException ();
//		}
//		public Result MapMemory (IMgDeviceMemory memory, ulong offset, ulong size, uint flags, out IntPtr ppData)
//		{
//			throw new NotImplementedException ();
//		}
//		public void UnmapMemory (IMgDeviceMemory memory)
//		{
//			throw new NotImplementedException ();
//		}
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
				throw new ArgumentException ("buffer");
			}

			pMemoryRequirements = new MgMemoryRequirements {
				Size = internalBuffer.RequestedSize,
				MemoryTypeBits = internalBuffer.BufferType.GetMask (),
			};
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
			var texture = image as GLImage;

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

//		public Result BindImageMemory (IMgImage image, IMgDeviceMemory memory, ulong memoryOffset)
//		{
//			throw new NotImplementedException ();
//		}
		public void GetImageSparseMemoryRequirements (IMgImage image, out MgSparseImageMemoryRequirements[] sparseMemoryRequirements)
		{
			throw new NotImplementedException ();
		}
		public Result CreateFence (MgFenceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFence fence)
		{
			throw new NotImplementedException ();
		}
//		public void DestroyFence (MgFence fence, IMgAllocationCallbacks allocator)
//		{
//			throw new NotImplementedException ();
//		}
		public Result ResetFences (IMgFence[] pFences)
		{
			throw new NotImplementedException ();
		}
		public Result GetFenceStatus (IMgFence fence)
		{
			throw new NotImplementedException ();
		}
		public Result WaitForFences (IMgFence[] pFences, bool waitAll, ulong timeout)
		{
			throw new NotImplementedException ();
		}
		public Result CreateSemaphore (MgSemaphoreCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSemaphore pSemaphore)
		{
			pSemaphore = mEntrypoint.Semaphore.CreateSemaphore();
			return Result.SUCCESS;
		}
//		public void DestroySemaphore (IMgSemaphore semaphore, IMgAllocationCallbacks allocator)
//		{
//			throw new NotImplementedException ();
//		}
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
//		public void DestroyBuffer (IMgBuffer buffer, IMgAllocationCallbacks allocator)
//		{
//			throw new NotImplementedException ();
//		}
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
			var internalImage = image as GLImage;

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

			var originalImage = pCreateInfo.Image as GLImage;

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

//		public void DestroyImageView (IMgImageView imageView, IMgAllocationCallbacks allocator)
//		{
//			mImageViews [imageView.Key].Destroy ();
//		}

		//private List<GLShaderModule> mShaderModules = new List<GLShaderModule>();
		public Result CreateShaderModule (MgShaderModuleCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgShaderModule pShaderModule)
		{			
			pShaderModule = new GLShaderModule (pCreateInfo, mEntrypoint.ShaderModule);
			return Result.SUCCESS;
		}

//		public void DestroyShaderModule (IMgShaderModule shaderModule, IMgAllocationCallbacks allocator)
//		{
//			mShaderModules[shaderModule.Key].Destroy();
//		}

		public Result CreatePipelineCache (MgPipelineCacheCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineCache pPipelineCache)
		{
			throw new NotImplementedException ();
		}
//		public void DestroyPipelineCache (IMgPipelineCache pipelineCache, IMgAllocationCallbacks allocator)
//		{
//			throw new NotImplementedException ();
//		}
		public Result GetPipelineCacheData (IMgPipelineCache pipelineCache, out byte[] pData)
		{
			throw new NotImplementedException ();
		}
		public Result MergePipelineCaches (IMgPipelineCache dstCache, IMgPipelineCache[] pSrcCaches)
		{
			throw new NotImplementedException ();
		}
		//private List<GLGraphicsPipeline> mPipelines = new List<GLGraphicsPipeline> ();

//		int CompileShaderModules (MgGraphicsPipelineCreateInfo info)
//		{
//			var modules = new List<int> ();
//			foreach (var stage in info.Stages)
//			{
//				var shaderType = ShaderType.VertexShader;
//				if (stage.Stage == MgShaderStageFlagBits.FRAGMENT_BIT)
//				{
//					shaderType = ShaderType.FragmentShader;
//				}
//				else if (stage.Stage == MgShaderStageFlagBits.VERTEX_BIT)
//				{
//					shaderType = ShaderType.VertexShader;
//				}
//				else if (stage.Stage == MgShaderStageFlagBits.COMPUTE_BIT)
//				{
//					shaderType = ShaderType.ComputeShader;
//				}
//				else if (stage.Stage == MgShaderStageFlagBits.GEOMETRY_BIT)
//				{
//					shaderType = ShaderType.GeometryShader;
//				}
//				var module = stage.Module as GLShaderModule;
//				if (module != null &&  module.ShaderId.HasValue)
//				{
//					modules.Add (module.ShaderId.Value);
//				}
//				else
//				{
//					using (var ms = new MemoryStream ())
//					{
//						module.Info.Code.CopyTo (ms, (int)module.Info.CodeSize.ToUInt32 ());
//						ms.Seek (0, SeekOrigin.Begin);
//						// FIXME : Encoding type 
//						using (var sr = new StreamReader (ms))
//						{
//							string fileContents = sr.ReadToEnd ();
//							module.ShaderId = GLSLTextShader.CompileShader (shaderType, fileContents, string.Empty);
//							modules.Add (module.ShaderId.Value);
//						}
//					}
//				}
//			}
//			return GLSLTextShader.LinkShaders (modules.ToArray ());
//		}
		public Result CreateGraphicsPipelines (IMgPipelineCache pipelineCache, MgGraphicsPipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines)
		{
			var output = new List<IMgPipeline> ();

			foreach (var info in pCreateInfos)
			{
				var layout = info.Layout as GLPipelineLayout;
				if (layout == null)
				{
					throw new ArgumentException ("pCreateInfos[].Layout");
				}

				if (info.VertexInputState == null)
				{
					throw new ArgumentNullException ("pCreateInfos[].VertexInputState");
				}

				if (info.InputAssemblyState == null)
				{
					throw new ArgumentNullException ("pCreateInfos[].InputAssemblyState");
				}

				if (info.RasterizationState == null)
				{
					throw new ArgumentNullException ("pCreateInfos[].RasterizationState");
				}

				var programId = mEntrypoint.GraphicsCompiler.Compile (info);

				/// MAKE SURE ACTIVE UNIFORMS ARE AVAILABLE
				int noOfActiveUniforms = mEntrypoint.GraphicsPipeline.GetActiveUniforms(programId);

				var uniqueLocations = new SortedDictionary<int, GLVariableBind> ();
				foreach (var binding in layout.Bindings)
				{
					bool uniformFound = false;

					if (noOfActiveUniforms > 0)
					{
						uniformFound = mEntrypoint.GraphicsPipeline.CheckUniformLocation (programId, binding.Location);
					}

					// ONLY ACTIVE UNIFORMS
					// FIXME : input attachment
					var bind = new GLVariableBind{
						IsActive = (noOfActiveUniforms > 0 && uniformFound), 
						Location = binding.Location,
						DescriptorType = binding.DescriptorType };

					// WILL THROW ERROR HERE IF COLLISION
					uniqueLocations.Add(binding.Location, bind);
				}

				// ASSUME NO GAPS ARE SUPPLIED
				var uniformBinder = new GLProgramUniformBinder (uniqueLocations.Values.Count);
				foreach (var bind in uniqueLocations.Values)
				{
					uniformBinder.Bindings[bind.Location] = bind;
				}

				var pipeline = new GLGraphicsPipeline (
					mEntrypoint.GraphicsPipeline,
					programId,
					info,
					uniformBinder
				);

				// TODO : BASE PIPELINE / CHILD

				output.Add (pipeline);
			}
			pPipelines = output.ToArray ();
			return Result.SUCCESS;
		}
		public Result CreateComputePipelines (IMgPipelineCache pipelineCache, MgComputePipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines)
		{
			throw new NotImplementedException ();
		}
//		public void DestroyPipeline (IMgPipeline pipeline, IMgAllocationCallbacks allocator)
//		{
//			mPipelines [pipeline.Key].Destroy ();	
//		}

		//private List<GLPipelineLayout> mPipelineLayouts = new List<GLPipelineLayout> ();
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

			pPipelineLayout = new GLPipelineLayout (pCreateInfo);
			return Result.SUCCESS;
		}
//		public void DestroyPipelineLayout (IMgPipelineLayout pipelineLayout, IMgAllocationCallbacks allocator)
//		{
//			mPipelineLayouts [pipelineLayout.Key].Destroy ();
//		}

		public Result CreateSampler (MgSamplerCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSampler pSampler)
		{
			pSampler = new GLSampler (mEntrypoint.Sampler.CreateSampler (), pCreateInfo, mEntrypoint.Sampler);
			return Result.SUCCESS;
		}
//		public void DestroySampler (IMgSampler sampler, IMgAllocationCallbacks allocator)
//		{
//			mSamplers [sampler.Key].Destroy ();	
//		}

		//private List<GLDescriptorSetLayout> mDescriptorSetLayouts = new List<GLDescriptorSetLayout> ();
		public Result CreateDescriptorSetLayout (MgDescriptorSetLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorSetLayout pSetLayout)
		{
			if (pCreateInfo == null)
			{
				throw new ArgumentNullException ("pCreateInfo");
			}
			pSetLayout  = new GLDescriptorSetLayout (pCreateInfo); 
			return Result.SUCCESS;
		}
//		public void DestroyDescriptorSetLayout (IMgDescriptorSetLayout descriptorSetLayout, IMgAllocationCallbacks allocator)
//		{
//			mDescriptorSetLayouts [descriptorSetLayout.Key].Destroy ();
//		}

		public Result CreateDescriptorPool (MgDescriptorPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorPool pDescriptorPool)
		{
			pDescriptorPool = mEntrypoint.DescriptorPool.CreatePool (pCreateInfo);
			return Result.SUCCESS;
		}

//		public void DestroyDescriptorPool (IMgDescriptorPool descriptorPool, IMgAllocationCallbacks allocator)
//		{
//			mPools [descriptorPool.Key].Destroy ();
//		}

		public Result ResetDescriptorPool (IMgDescriptorPool descriptorPool, uint flags)
		{
			throw new NotImplementedException ();
		}

		//private ConcurrentDictionary<int, GLDescriptorSet> mDescriptorSets = new ConcurrentDictionary<int, GLDescriptorSet>();
		public Result AllocateDescriptorSets (MgDescriptorSetAllocateInfo pAllocateInfo, out IMgDescriptorSet[] pDescriptorSets)
		{
			if (pAllocateInfo == null)
			{	
				throw new ArgumentNullException ("pAllocateInfo");
			}

			var pool = pAllocateInfo.DescriptorPool as IGLDescriptorPool;
			if (pool == null)
			{
				throw new ArgumentNullException ("pAllocateInfo.DescriptorPool");
			}

			var noOfSetsRequested = pAllocateInfo.SetLayouts.Length;
			if (pool.NoOfSets < noOfSetsRequested)
			{
				throw new InvalidOperationException ();
			}

			pDescriptorSets = new GLDescriptorSet[noOfSetsRequested];

			for (int i = 0; i < noOfSetsRequested; ++i)
			{
				var setLayout = pAllocateInfo.SetLayouts[i] as GLDescriptorSetLayout;

				GLDescriptorSet dSet;
				if (!pool.TryTake (out dSet))
				{
					throw new InvalidOperationException ();
				}
				// copy here
				dSet.Populate (setLayout);
				pDescriptorSets[i] = dSet;
			}

			return Result.SUCCESS;
		}

		public Result FreeDescriptorSets (IMgDescriptorPool descriptorPool, IMgDescriptorSet[] pDescriptorSets)
		{
			if (descriptorPool == null)
			{	
				throw new ArgumentNullException ("descriptorPool");
			}

			var localPool = descriptorPool as IGLDescriptorPool;

			foreach (var dSet in pDescriptorSets)
			{
				var localSet = dSet as GLDescriptorSet;
				if (localSet != null)
				{
					localSet.Destroy ();
					localPool.Add (localSet);
				}
			}
			return Result.SUCCESS;

		}

		public void UpdateDescriptorSets (MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies)
		{
			if (pDescriptorWrites != null)
			{
				foreach (var desc in pDescriptorWrites)
				{
					var localSet = desc.DstSet as GLDescriptorSet;
					if (localSet == null)
					{
						throw new NotSupportedException ();
					}

					var x = desc.DstBinding; // SHOULD ALWAYS BE ZERO

					int offset = (int)desc.DstArrayElement;
					int count = (int)desc.DescriptorCount;

					var lastIndex = localSet.Bindings.Length - 1;
					var right = offset + count - 1;
					if (right > lastIndex)
					{
						// VULKAN WOULD CONTINUE ONTO WRITE ADDITIONAL VALUES TO NEXT BINDING
						// ONLY ONE SET OF BINDING USED
						throw new IndexOutOfRangeException ();
					}

					switch (desc.DescriptorType)
					{
					case MgDescriptorType.SAMPLER:
					case MgDescriptorType.COMBINED_IMAGE_SAMPLER:
					case MgDescriptorType.SAMPLED_IMAGE:

						// HOPEFULLY DESCRIPTOR SETS ARE GROUPED BY COMMON TYPES
						for (int i = 0; i < count; ++i)
						{
							MgDescriptorImageInfo info = desc.ImageInfo [i];						

							var localSampler = info.Sampler as GLSampler;
							var localView = info.ImageView as GLImageView;	

							// Generate bindless texture handle 
							// FIXME : messy as F***

							var internalBinding = localSet.Bindings [offset + i];

							if (internalBinding != null)
							{
								var texHandle = mEntrypoint.ImageDescriptor.CreateHandle (localView.TextureId, localSampler.SamplerId); 

								var imageDesc = internalBinding.ImageDesc;
								imageDesc.Replace (texHandle);
							}
						}					
						break;
					case MgDescriptorType.STORAGE_BUFFER:
					case MgDescriptorType.STORAGE_BUFFER_DYNAMIC:
						// HOPEFULLY DESCRIPTOR SETS ARE GROUPED BY COMMON TYPES
						for (int i = 0; i < count; ++i)
						{
							var info = desc.BufferInfo [i];

							var buf = info.Buffer as IGLBuffer;

							if (buf != null && buf.BufferType == GLMemoryBufferType.SSBO)
							{
								var bufferDesc = localSet.Bindings [offset + i].BufferDesc;
								bufferDesc.BufferId = buf.BufferId;
							}
						}
						break;
					default:
						throw new NotSupportedException ();					
					}

				}
			}
		}
		public Result CreateFramebuffer (MgFramebufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFramebuffer pFramebuffer)
		{
			pFramebuffer = new GLFramebuffer ();
			return Result.SUCCESS;
		}

//		public void DestroyFramebuffer (IMgFramebuffer framebuffer, IMgAllocationCallbacks allocator)
//		{
//			throw new NotImplementedException ();
//		}

		public Result CreateRenderPass (MgRenderPassCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgRenderPass pRenderPass)
		{
			pRenderPass = new GLRenderPass (pCreateInfo.Attachments);
			return Result.SUCCESS;
		}
//		public void DestroyRenderPass (IMgRenderPass renderPass, IMgAllocationCallbacks allocator)
//		{
//			throw new NotImplementedException ();
//		}
		public void GetRenderAreaGranularity (IMgRenderPass renderPass, out MgExtent2D pGranularity)
		{
			throw new NotImplementedException ();
		}
		public Result CreateCommandPool (MgCommandPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgCommandPool pCommandPool)
		{
			pCommandPool = new GLCommandPool (pCreateInfo.Flags);
			return Result.SUCCESS;
		}
//		public void DestroyCommandPool (IMgCommandPool commandPool, IMgAllocationCallbacks allocator)
//		{
//			throw new NotImplementedException ();
//		}
//		public Result ResetCommandPool (IMgCommandPool commandPool, MgCommandPoolResetFlagBits flags)
//		{
//			throw new NotImplementedException ();
//		}
		public Result AllocateCommandBuffers (MgCommandBufferAllocateInfo pAllocateInfo, IMgCommandBuffer[] pCommandBuffers)
		{			
			var cmdPool = pAllocateInfo.CommandPool as GLCommandPool;

			if (cmdPool == null)
			{
				throw new InvalidCastException ("pAllocateInfo.CommandPool");
			}

//			{
//				var error = GL.GetError ();
//				Debug.WriteLineIf (error != ErrorCode.NoError, "AllocateCommandBuffers (BEFORE) : " + error);
//			}

			for (uint i = 0; i < pAllocateInfo.CommandBufferCount; ++i)
			{
				// TODO : for now
				var buffer = new GLCommandBuffer (true, new GLCmdBufferRepository(), mEntrypoint.VBO, mEntrypoint.ImageFormat);
				cmdPool.Buffers.Add (buffer);
				pCommandBuffers [i] = buffer;
			}

//			{
//				var error = GL.GetError ();
//				Debug.WriteLineIf (error != ErrorCode.NoError, "AllocateCommandBuffers (BEFORE) : " + error);
//			}

			return Result.SUCCESS;
		}
		public void FreeCommandBuffers (IMgCommandPool commandPool, IMgCommandBuffer[] pCommandBuffers)
		{
			foreach (var item in pCommandBuffers)
			{
				var cmdBuf = item as GLCommandBuffer;
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
//		public void DestroySwapchainKHR (IMgSwapchainKHR swapchain, IMgAllocationCallbacks allocator)
//		{
//			throw new NotImplementedException ();
//		}
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

