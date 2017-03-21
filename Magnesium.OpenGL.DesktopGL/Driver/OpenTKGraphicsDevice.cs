using System;
using Magnesium;
using OpenTK.Graphics;
using OpenTK;
using Magnesium.OpenGL;
using System.Diagnostics;

namespace Magnesium.OpenGL.DesktopGL
{
	public class OpenTKGraphicsDevice : IMgGraphicsDevice
	{
		#region IMgDepthStencilBuffer implementation

		public IMgRenderPass Renderpass {
			get {
				return mRenderpass;
			}
		}

		GLNullImageView mView;

		INativeWindow mWindow;

		IGLExtensionLookup mExtensions;

		IMgGraphicsDeviceLogger mLogger;

		IMgGraphicsConfiguration mGraphicsConfiguration;

		IGLFramebufferHelperSelector mSelector;

		IGLRenderer mQueueRenderer;

		public OpenTKGraphicsDevice (
			INativeWindow window,
            IMgGraphicsConfiguration configuration,
			IGLFramebufferHelperSelector selector,
			IGLExtensionLookup extensions,
			IMgGraphicsDeviceLogger logger,
			IGLRenderer queueRenderer,
            IGLBackbufferContext bbContext
		)
		{
			mGraphicsConfiguration = configuration;
			mView = new GLNullImageView ();
			mWindow = window;
			mExtensions = extensions;
			mLogger = logger;
			mSelector = selector;
			mQueueRenderer = queueRenderer;
            mBBContext = bbContext;
            // SHOULD BE HIDDEN
            mFramebuffers = new MgFramebufferCollection(mGraphicsConfiguration);
        }

		public IMgImageView DepthStencilImageView {
			get {
				return mView;
			}
		}

		private readonly MgFramebufferCollection mFramebuffers;
		public IMgFramebuffer[] Framebuffers {
			get {
				return mFramebuffers.Framebuffers;
			}
		}

//		public void CreateDevice (IGraphicsAdapter adapter, GraphicsProfile graphicsProfile)
//		{
//			
//		}

		void SetupContext (MgGraphicsDeviceCreateInfo createInfo, MgFormat colorPassFormat, MgFormat depthPassFormat)
		{
            mBBContext.SetupContext(mWindow.WindowInfo, createInfo, colorPassFormat, depthPassFormat);

			mExtensions.Initialize ();
			//mCapabilities.Initialize ();
			//mGLPlatform.Initialize ();
			mSelector.Initialize();
			mQueueRenderer.Initialize ();
		}

        private MgRenderPassCreateInfo mRenderpassInfo;
        public MgRenderPassCreateInfo RenderpassInfo
        {
            get
            {
                return mRenderpassInfo;
            }
        }

        IGLRenderPass mRenderpass;
		void SetupRenderpass (MgGraphicsDeviceCreateInfo createInfo, MgFormat colorPassFormat, MgFormat depthPassFormat)
        {
			var attachmentDescriptions = new [] {
				new MgAttachmentDescription {
					Format = colorPassFormat,
					LoadOp = MgAttachmentLoadOp.CLEAR,
					StoreOp = MgAttachmentStoreOp.STORE,
					StencilLoadOp = MgAttachmentLoadOp.DONT_CARE,
					StencilStoreOp = MgAttachmentStoreOp.DONT_CARE,
				},
				new MgAttachmentDescription {
					Format = depthPassFormat,
					LoadOp = MgAttachmentLoadOp.CLEAR,
					StoreOp = MgAttachmentStoreOp.STORE,
					StencilLoadOp = MgAttachmentLoadOp.CLEAR,
					StencilStoreOp = MgAttachmentStoreOp.STORE,                    
				},
			};

            mRenderpassInfo = new MgRenderPassCreateInfo
            {
                Attachments = attachmentDescriptions,
                Subpasses = new []
                {
                    new MgSubpassDescription
                    {
                        PipelineBindPoint = MgPipelineBindPoint.GRAPHICS,
                        Flags = 0,
                        ColorAttachmentCount = 2,
                        ColorAttachments = new []
                        {
                            new MgAttachmentReference
                            {
                                Attachment = 0,
                                Layout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
                            },
                        },
                        DepthStencilAttachment = new MgAttachmentReference
                        {
                            Attachment = 1,
                            Layout = MgImageLayout.DEPTH_STENCIL_ATTACHMENT_OPTIMAL,
                        },
                    }
                },
            };

            mRenderpass = new GLRenderPass (attachmentDescriptions);
		}

        public void Create(IMgCommandBuffer setupCmdBuffer, IMgSwapchainCollection swapchainCollection, MgGraphicsDeviceCreateInfo createInfo)
        {
			if (createInfo == null)
			{
				throw new ArgumentNullException (nameof(createInfo));
			}

			if (setupCmdBuffer == null)
			{
				throw new ArgumentNullException (nameof(setupCmdBuffer));
			}

			if (swapchainCollection == null)
			{
				throw new ArgumentNullException (nameof(swapchainCollection));
			}

			ReleaseUnmanagedResources ();
			mDeviceCreated = false;

            swapchainCollection.Create (setupCmdBuffer, createInfo.Color, createInfo.Width, createInfo.Height);
            MgFormat colorPassFormat = swapchainCollection.Format;

            MgFormat depthPassFormat = OverrideDepthStencilFormat(createInfo);
            SetupContext(createInfo, colorPassFormat, depthPassFormat);

            // SetupSwapchain (swapchainCollection, createInfo);
            SetupRenderpass(createInfo, colorPassFormat, depthPassFormat);
            mFramebuffers.Create(swapchainCollection, mRenderpass, mView, createInfo.Width, createInfo.Height);

			Scissor = new MgRect2D { 
				Extent = new MgExtent2D{ Width = createInfo.Width, Height = createInfo.Height },
				Offset = new MgOffset2D{ X = 0, Y = 0 },
			};

			// initialize viewport
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

        private MgFormat OverrideDepthStencilFormat(MgGraphicsDeviceCreateInfo createInfo)
        {
            MgFormat depthPassFormat = createInfo.DepthStencil;
            if (depthPassFormat == MgFormat.UNDEFINED)
            {
                depthPassFormat = MgFormat.D24_UNORM_S8_UINT;
            }
            return depthPassFormat;
        }

        bool mDeviceCreated = false;
		public bool DeviceCreated ()
		{
			return mDeviceCreated;
		}

		public MgViewport CurrentViewport {
			get;
			set;
		}

		public MgRect2D Scissor {
			get;
			private set;
		}

		~OpenTKGraphicsDevice()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		void ReleaseUnmanagedResources ()
		{
            mFramebuffers.Clear();
			if (mRenderpass != null)
			{
				mRenderpass.DestroyRenderPass (mGraphicsConfiguration.Device, null);
			}
		}

		public bool IsDisposed ()
		{
			return mIsDisposed;
		}

		private bool mIsDisposed = false;
        private IGLBackbufferContext mBBContext;

        protected virtual void Dispose(bool isDisposing)
		{
			if (mIsDisposed)
				return;

			ReleaseUnmanagedResources ();

			mIsDisposed = true;
		}

        #endregion



    }
}

