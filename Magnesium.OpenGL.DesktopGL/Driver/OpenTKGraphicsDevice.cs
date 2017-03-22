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

		void SetupContext (MgGraphicsDeviceCreateInfo createInfo, MgFormat colorPassFormat, MgFormat depthPassFormat)
		{
            mBBContext.SetupContext(mWindow.WindowInfo, colorPassFormat, depthPassFormat, createInfo);

			mExtensions.Initialize ();
			mSelector.Initialize();
			mQueueRenderer.Initialize ();
		}

		IGLRenderPass mRenderpass;
		void SetupRenderpass (MgFormat colorPassFormat, MgFormat depthPassFormat)
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

            // MANDATORY
            swapchainCollection.Create (setupCmdBuffer, createInfo.Width, createInfo.Height);

            var colorPassFormat = OverrideColorFormat(createInfo.Color, swapchainCollection.Format);
            var depthPassFormat = OverrideDepthStencilFormat(createInfo.DepthStencil);

            SetupContext(createInfo, colorPassFormat, depthPassFormat);
            SetupRenderpass(colorPassFormat, depthPassFormat);

           // SetupSwapchain (swapchainCollection, createInfo);

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

        private MgFormat OverrideDepthStencilFormat(MgFormat overrideColor)
        {
            if (overrideColor == MgFormat.UNDEFINED)
            {
                return MgFormat.D24_UNORM_S8_UINT;
            }
            else
            {
                return overrideColor;
            }
        }

        private MgFormat OverrideColorFormat(MgFormat overrideColor, MgFormat swapchainFormat)
        {
            if (overrideColor == MgFormat.UNDEFINED)
            {
                return swapchainFormat;
            }
            else
            {
                return overrideColor;
            }
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
    }
}

