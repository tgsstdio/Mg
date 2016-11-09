using System;
using System.Diagnostics;

namespace Magnesium
{
	public class MgDefaultGraphicsDevice : IMgGraphicsDevice
	{
		readonly IMgGraphicsConfiguration mGraphicsConfiguration;
		readonly IMgImageTools mImageTools;
        readonly MgFramebufferCollection mFramebuffers;

        public IMgFramebuffer[] Framebuffers {
        	get {
        		return mFramebuffers.Framebuffers;
        	}
        }

        public MgDefaultGraphicsDevice (IMgGraphicsConfiguration configuration, IMgImageTools imageTools)
		{
            mGraphicsConfiguration = configuration;
            // SHOULD BE HIDDEN
            mFramebuffers = new MgFramebufferCollection(mGraphicsConfiguration);
			mImageTools = imageTools;
		}

		MgPhysicalDeviceProperties mProperties;
		private void Setup ()
		{
			MgPhysicalDeviceProperties prop;
            Debug.Assert(mGraphicsConfiguration.Partition != null);
            mGraphicsConfiguration.Partition.PhysicalDevice.GetPhysicalDeviceProperties (out prop);
			mProperties = prop;
		}

		private IMgImageView mDepthStencilImageView;

		public IMgImageView DepthStencilImageView {
			get {
				return mDepthStencilImageView;
			}
		}

		bool GetSupportedDepthFormat(out MgFormat depthFormat)
		{
			// Since all depth formats may be optional, we need to find a suitable depth format to use
			// Start with the highest precision packed format
			MgFormat[] depthFormats = { 
				MgFormat.D32_SFLOAT_S8_UINT, 
				MgFormat.D32_SFLOAT,
				MgFormat.D24_UNORM_S8_UINT, 
				MgFormat.D16_UNORM_S8_UINT, 
				MgFormat.D16_UNORM 
			};

            Debug.Assert(mGraphicsConfiguration.Partition != null);

            foreach (var format in depthFormats)
			{
				MgFormatProperties formatProps;
                mGraphicsConfiguration.Partition.PhysicalDevice.GetPhysicalDeviceFormatProperties(format, out formatProps);
				// Format must support depth stencil attachment for optimal tiling
				if ((formatProps.OptimalTilingFeatures & MgFormatFeatureFlagBits.DEPTH_STENCIL_ATTACHMENT_BIT) == MgFormatFeatureFlagBits.DEPTH_STENCIL_ATTACHMENT_BIT)
				{
					depthFormat = format;
					return true;
				}
			}

			depthFormat = MgFormat.UNDEFINED;
			return false;
		}

		private IMgImage mImage;
		private IMgDeviceMemory mDeviceMemory;
		void ReleaseUnmanagedResources ()
		{
            mFramebuffers.Clear();
			if (mRenderpass != null)
			{
				mRenderpass.DestroyRenderPass (mGraphicsConfiguration.Partition.Device, null);
				mRenderpass = null;
			}
			if (mDepthStencilImageView != null)
			{
				mDepthStencilImageView.DestroyImageView (mGraphicsConfiguration.Partition.Device, null);
				mDepthStencilImageView = null;
			}
			if (mImage != null)
			{
				mImage.DestroyImage (mGraphicsConfiguration.Partition.Device, null);
				mImage = null;
			}
			if (mDeviceMemory != null)
			{
				mDeviceMemory.FreeMemory (mGraphicsConfiguration.Partition.Device, null);
				mDeviceMemory = null;
			}
		}

        public void Create(IMgCommandBuffer setupCmdBuffer, IMgSwapchainCollection swapchainCollection, MgGraphicsDeviceCreateInfo createInfo)
        {
            if (setupCmdBuffer == null)
            {
                throw new ArgumentNullException(nameof(setupCmdBuffer));
            }

            if (createInfo == null)
			{
				throw new ArgumentNullException (nameof(createInfo));
			}

			if (swapchainCollection == null)
			{
				throw new ArgumentNullException (nameof(swapchainCollection));
			}

            Setup();

            // Check if device supports requested sample count for color and depth frame buffer
            if (
				(mProperties.Limits.FramebufferColorSampleCounts < createInfo.Samples)
				|| (mProperties.Limits.FramebufferDepthSampleCounts < createInfo.Samples))
			{
				throw new ArgumentOutOfRangeException ("createInfo.Samples",
					"MgDefaultDepthStencilBuffer : This physical device cannot fulfil the requested sample count for BOTH color and depth frame buffer");
			}

			ReleaseUnmanagedResources ();
			mDeviceCreated = false;

			CreateDepthStencil (setupCmdBuffer, createInfo);
			CreateRenderpass (createInfo);
            swapchainCollection.Create (setupCmdBuffer, createInfo.Width, createInfo.Height);
			mFramebuffers.Create(swapchainCollection, mRenderpass, mDepthStencilImageView, createInfo.Width, createInfo.Height);

            Scissor = new MgRect2D { 
				Extent = new MgExtent2D{ Width = createInfo.Width, Height = createInfo.Height },
				Offset = new MgOffset2D{ X = 0, Y = 0 },
			};

			// initialise viewport
			CurrentViewport = new MgViewport {
				Width = createInfo.Width,
				Height = createInfo.Height,
				X = 0,
				Y = 0,
				MinDepth = 0f,
				MaxDepth = 1f,
			};
			mDeviceCreated = true;
		}

		public MgViewport CurrentViewport {
			get;
			private set;
		}

		public MgRect2D Scissor {
			get;
			private set;
		}

		IMgRenderPass mRenderpass;
		public IMgRenderPass Renderpass {
			get {
				return mRenderpass;
			}
		}

		void CreateRenderpass (MgGraphicsDeviceCreateInfo createInfo)
		{
			var attachments = new []
			{
				// Color attachment[0] 
				new MgAttachmentDescription{
					Format = createInfo.Color,
					// TODO : multisampling
					Samples = MgSampleCountFlagBits.COUNT_1_BIT,
					LoadOp =  MgAttachmentLoadOp.CLEAR,
					StoreOp = MgAttachmentStoreOp.STORE,
					StencilLoadOp = MgAttachmentLoadOp.DONT_CARE,
					StencilStoreOp = MgAttachmentStoreOp.DONT_CARE,
					InitialLayout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
					FinalLayout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
				},
				// Depth attachment[1]
				new MgAttachmentDescription{
					Format = createInfo.DepthStencil,
					// TODO : multisampling
					Samples = MgSampleCountFlagBits.COUNT_1_BIT,
					LoadOp = MgAttachmentLoadOp.CLEAR,
					StoreOp = MgAttachmentStoreOp.STORE,

					// TODO : activate stencil if needed
					StencilLoadOp =  MgAttachmentLoadOp.DONT_CARE,
					StencilStoreOp =  MgAttachmentStoreOp.DONT_CARE,

					InitialLayout = MgImageLayout.DEPTH_STENCIL_ATTACHMENT_OPTIMAL,
					FinalLayout = MgImageLayout.DEPTH_STENCIL_ATTACHMENT_OPTIMAL,
				}
			};

			var colorReference = new MgAttachmentReference
			{
				Attachment = 0,
				Layout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
			};

			var depthReference = new MgAttachmentReference{
				Attachment = 1,
				Layout = MgImageLayout.DEPTH_STENCIL_ATTACHMENT_OPTIMAL,
			};

			var subpass = new MgSubpassDescription
			{
				PipelineBindPoint = MgPipelineBindPoint.GRAPHICS,
				Flags = 0,
				InputAttachments = null,
				ColorAttachments = new []{colorReference},
				ResolveAttachments = null,
				DepthStencilAttachment = depthReference,
				PreserveAttachments = null,
			};

			var renderPassInfo = new MgRenderPassCreateInfo{
				Attachments = attachments,
				Subpasses = new []{subpass},
				Dependencies = null,
			};

			Result err;

			IMgRenderPass renderPass;
			err = mGraphicsConfiguration.Partition.Device.CreateRenderPass(renderPassInfo, null, out renderPass);
			Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");
			mRenderpass = renderPass;
		}

		void CreateDepthStencil (IMgCommandBuffer setupCmdBuffer, MgGraphicsDeviceCreateInfo createInfo)
		{
			var image = new MgImageCreateInfo {
				ImageType = MgImageType.TYPE_2D,
				Format = createInfo.DepthStencil,
				Extent = new MgExtent3D {
					Width = createInfo.Width,
					Height = createInfo.Height,
					Depth = 1
				},
				MipLevels = 1,
				ArrayLayers = 1,
				Samples = createInfo.Samples,
				Tiling = MgImageTiling.OPTIMAL,
				// TODO : multisampled uses MgImageUsageFlagBits.TRANSIENT_ATTACHMENT_BIT | MgImageUsageFlagBits.DEPTH_STENCIL_ATTACHMENT_BIT;
				Usage = MgImageUsageFlagBits.DEPTH_STENCIL_ATTACHMENT_BIT | MgImageUsageFlagBits.TRANSFER_SRC_BIT,
				Flags = 0,
			};
			var mem_alloc = new MgMemoryAllocateInfo {
				AllocationSize = 0,
				MemoryTypeIndex = 0,
			};
			MgMemoryRequirements memReqs;

            Debug.Assert(mGraphicsConfiguration.Partition != null);

            Result err;
			{
				IMgImage dsImage;
				err = mGraphicsConfiguration.Partition.Device.CreateImage (image, null, out dsImage);
				Debug.Assert (err == Result.SUCCESS, err + " != Result.SUCCESS");
				mImage = dsImage;
			}
            mGraphicsConfiguration.Partition.Device.GetImageMemoryRequirements (mImage, out memReqs);
			mem_alloc.AllocationSize = memReqs.Size;
			uint memTypeIndex;
			bool memoryTypeFound = mGraphicsConfiguration.Partition.GetMemoryType (memReqs.MemoryTypeBits, MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT, out memTypeIndex);
			Debug.Assert (memoryTypeFound);
			mem_alloc.MemoryTypeIndex = memTypeIndex;
			{
				IMgDeviceMemory dsDeviceMemory;
				err = mGraphicsConfiguration.Partition.Device.AllocateMemory (mem_alloc, null, out dsDeviceMemory);
				Debug.Assert (err == Result.SUCCESS, err + " != Result.SUCCESS");
				mDeviceMemory = dsDeviceMemory;
			}
			err = mImage.BindImageMemory (mGraphicsConfiguration.Partition.Device, mDeviceMemory, 0);
			Debug.Assert (err == Result.SUCCESS, err + " != Result.SUCCESS");
			mImageTools.SetImageLayout (setupCmdBuffer, mImage, MgImageAspectFlagBits.DEPTH_BIT | MgImageAspectFlagBits.STENCIL_BIT, MgImageLayout.UNDEFINED, MgImageLayout.DEPTH_STENCIL_ATTACHMENT_OPTIMAL);
			var depthStencilView = new MgImageViewCreateInfo {
				Image = mImage,
				ViewType = MgImageViewType.TYPE_2D,
				Format = createInfo.DepthStencil,
				Flags = 0,
				SubresourceRange = new MgImageSubresourceRange {
					AspectMask = MgImageAspectFlagBits.DEPTH_BIT | MgImageAspectFlagBits.STENCIL_BIT,
					BaseMipLevel = 0,
					LevelCount = 1,
					BaseArrayLayer = 0,
					LayerCount = 1,
				},
			};
			{
				IMgImageView dsView;
				err = mGraphicsConfiguration.Partition.Device.CreateImageView (depthStencilView, null, out dsView);
				Debug.Assert (err == Result.SUCCESS, err + " != Result.SUCCESS");
				mDepthStencilImageView = dsView;
			}
		}

		//private IMgFramebuffer[] mFramebuffers;
		//public IMgFramebuffer[] Framebuffers {
		//	get {
		//		return mFramebuffers;
		//	}
		//}

		//void CreateFramebuffers (IMgSwapchainCollection swapchains, MgGraphicsDeviceCreateInfo createInfo)
		//{
  //          IMgFramebuffer[] SetupFrameBuffers(IMgRenderPass renderPass, IMgDepthStencilBuffer depthStencil, IMgSwapchainCollection swapChain, uint width, uint height)
  //          {
  //          Debug.Assert(mGraphicsConfiguration.Partition != null);

  //           Create frame buffers for every swap chain image
  //          var frameBuffers = new IMgFramebuffer[swapchains.Buffers.Length];
		//	for (uint i = 0; i < frameBuffers.Length; i++)
		//	{
		//		var frameBufferCreateInfo = new MgFramebufferCreateInfo
		//		{
		//			RenderPass = mRenderpass,
		//			Attachments = new []
		//			{
  //                      swapchains.Buffers[i].View,
		//				 Depth/Stencil attachment is the same for all frame buffers
		//				mDepthStencilImageView,
		//			},
		//			Width = createInfo.Width,
		//			Height = createInfo.Height,
		//			Layers = 1,
		//		};

		//		var err = mGraphicsConfiguration.Partition.Device.CreateFramebuffer(frameBufferCreateInfo, null, out frameBuffers[i]);
		//		Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");
		//	}

		//	mFramebuffers = frameBuffers;
		//}

		private bool mDeviceCreated = false;
		public bool DeviceCreated ()
		{
			return mDeviceCreated;
		}

		~MgDefaultGraphicsDevice()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		public bool IsDisposed()
		{
			return mIsDisposed;
		}

		private bool mIsDisposed = false;
        protected virtual void Dispose(bool isDisposing)
		{
			if (mIsDisposed)
				return;

			ReleaseUnmanagedResources ();

			mIsDisposed = true;
		}
    }
}

