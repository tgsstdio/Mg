using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Magnesium
{
    public class MgDefaultGraphicsDevice : IMgGraphicsDevice
	{
		readonly IMgGraphicsConfiguration mGraphicsConfiguration;
		readonly IMgGraphicsDeviceContext mContext;
        readonly MgFramebufferCollection mFramebuffers;

        public IMgFramebuffer[] Framebuffers {
        	get {
        		return mFramebuffers.Framebuffers;
        	}
        }

        public MgDefaultGraphicsDevice (IMgGraphicsConfiguration configuration, IMgGraphicsDeviceContext context)
		{
            mGraphicsConfiguration = configuration;
            // SHOULD BE HIDDEN
            mFramebuffers = new MgFramebufferCollection(mGraphicsConfiguration);
            mContext = context;
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

		void ReleaseUnmanagedResources ()
		{
            mFramebuffers.Clear();
			if (mRenderpass != null)
			{
				mRenderpass.DestroyRenderPass (mGraphicsConfiguration.Partition.Device, null);
				mRenderpass = null;
			}
            mContext.ReleaseDepthStencil();
		}

        public void Create(IMgCommandBuffer setupCmdBuffer, IMgImageBufferCollection imageCollection, MgGraphicsDeviceCreateInfo createInfo)
        {
            if (setupCmdBuffer == null)
            {
                throw new ArgumentNullException(nameof(setupCmdBuffer));
            }

            if (createInfo == null)
            {
                throw new ArgumentNullException(nameof(createInfo));
            }

            if (imageCollection == null)
            {
                throw new ArgumentNullException(nameof(imageCollection));
            }

            mContext.Initialize(createInfo);

            ReleaseUnmanagedResources();
            mDeviceCreated = false;

            imageCollection.Create(setupCmdBuffer, createInfo.Swapchain.Color, createInfo.Swapchain.OverrideColor, createInfo.Width, createInfo.Height);
            var autoColorFormat = imageCollection.Format;

            var autoDepthStencilFormat = SelectDepthStencilFormat(createInfo.Swapchain.DepthStencil, createInfo.Swapchain.OverrideDepthStencil);
            var view = mContext.SetupDepthStencil(createInfo, setupCmdBuffer, autoDepthStencilFormat);
            mContext.SetupContext(createInfo, autoColorFormat, autoDepthStencilFormat);

            CreateRenderpass(createInfo, autoColorFormat, autoDepthStencilFormat);
            mFramebuffers.Create(imageCollection, mRenderpass, view, createInfo.Width, createInfo.Height);

            Scissor = new MgRect2D
            {
                Extent = new MgExtent2D { Width = createInfo.Width, Height = createInfo.Height },
                Offset = new MgOffset2D { X = 0, Y = 0 },
            };

            // initialize viewport
            Viewport = new MgViewport
            {
                Width = createInfo.Width,
                Height = createInfo.Height,
                X = 0,
                Y = 0,
                MinDepth = createInfo.MinDepth,
                MaxDepth = createInfo.MaxDepth,
            };
            mDeviceCreated = true;
        }

        public MgViewport Viewport {
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

        private MgRenderPassCreateInfo mRenderpassInfo;
        public MgRenderPassCreateInfo RenderpassInfo
        {
            get
            {
                return mRenderpassInfo;
            }
        }

        void CreateRenderpass(MgGraphicsDeviceCreateInfo createInfo, MgFormat autoColorFormat, MgFormat autoDepthStencilFormat)
        {
            var attachments = new List<MgAttachmentDescription>();

            var colorFormat = createInfo.RenderPass.Color == MgColorFormatOption.AUTO_DETECT
                ? autoColorFormat
                : createInfo.RenderPass.OverrideColor;

            var depthOption = createInfo.RenderPass.DepthStencil;

            var depthStencilFormat = createInfo.RenderPass.DepthStencil == MgDepthFormatOption.AUTO_DETECT
                ? autoDepthStencilFormat          
                // DOESN't MATTER IF NONE, WILL BE IGNORED
                : createInfo.RenderPass.OverrideDepthStencil;

            attachments.Add(			
				// Color attachment[0] 
				new MgAttachmentDescription{
					Format = colorFormat,
					// TODO : multisampling
					Samples = MgSampleCountFlagBits.COUNT_1_BIT,
					LoadOp =  MgAttachmentLoadOp.CLEAR,
					StoreOp = MgAttachmentStoreOp.STORE,
					StencilLoadOp = MgAttachmentLoadOp.DONT_CARE,
					StencilStoreOp = MgAttachmentStoreOp.DONT_CARE,
					InitialLayout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
					FinalLayout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
				}
            );

            if (depthOption != MgDepthFormatOption.USE_NONE)
            {
                attachments.Add(
                    // Depth attachment[1]
                    new MgAttachmentDescription
                    {
                        Format = depthStencilFormat,
                        // TODO : multisampling
                        Samples = MgSampleCountFlagBits.COUNT_1_BIT,
                        LoadOp = MgAttachmentLoadOp.CLEAR,
                        StoreOp = MgAttachmentStoreOp.STORE,

                        // TODO : activate stencil if needed
                        StencilLoadOp = MgAttachmentLoadOp.DONT_CARE,
                        StencilStoreOp = MgAttachmentStoreOp.DONT_CARE,

                        InitialLayout = MgImageLayout.DEPTH_STENCIL_ATTACHMENT_OPTIMAL,
                        FinalLayout = MgImageLayout.DEPTH_STENCIL_ATTACHMENT_OPTIMAL,
                    }
                );
            }

			var colorReference = new MgAttachmentReference
			{
				Attachment = 0,
				Layout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
			};

            MgAttachmentReference depthReference =
                (depthOption != MgDepthFormatOption.USE_NONE)
                ?   new MgAttachmentReference
                    {
                        Attachment = 1,
                        Layout = MgImageLayout.DEPTH_STENCIL_ATTACHMENT_OPTIMAL,
                    }
                : null;

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
				Attachments = attachments.ToArray(),
				Subpasses = new []{subpass},
				Dependencies = null,
			};

            Result err;

			IMgRenderPass renderPass;
			err = mGraphicsConfiguration.Partition.Device.CreateRenderPass(renderPassInfo, null, out renderPass);
			Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");
            mRenderpassInfo = renderPassInfo;
            mRenderpass = renderPass;
		}

        MgFormat SelectDepthStencilFormat(MgDepthFormatOption option, MgFormat overrideDepthStencil)
        {
            if (option == MgDepthFormatOption.USE_NONE)
            {
                /*
                 * left as null
                 * - mImage
                 * - mDepthStencilImageView
                 * - mDeviceMemory
                 */
                return MgFormat.UNDEFINED;
            }

            MgFormat depthFormat = MgFormat.UNDEFINED;

            if (option == MgDepthFormatOption.AUTO_DETECT)
            {
                // WILL ignore user-supplied depth 
                if (!GetSupportedDepthFormat(out depthFormat))
                {
                    throw new InvalidOperationException("No depth format available");
                }
            }
            else if (option == MgDepthFormatOption.USE_OVERRIDE)
            {
                depthFormat = overrideDepthStencil;
            }
            // TODO : NONE MEANS DISABLE VIEW CREATION

            return depthFormat;
        }

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

