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

		void SetupContext (MgGraphicsDeviceCreateInfo createInfo)
		{
            //GraphicsMode mode;
            //var wnd = mWindow.WindowInfo;
            //// Create an OpenGL compatibility context
            //var flags = GraphicsContextFlags.Default;
            //int major = 1;
            //int minor = 0;
            //if (Context == null || Context.IsDisposed)
            //{
            //	var color = GetColorFormat (createInfo.Color);
            //	var depthBit = GetDepthBit (createInfo.DepthStencil);
            //	var stencilBit = GetStencilBit (createInfo.DepthStencil);
            //	var samples = (int)createInfo.Samples;
            //	if (samples == 0)
            //	{
            //		// Use a default of 4x samples if PreferMultiSampling is enabled
            //		// without explicitly setting the desired MultiSampleCount.
            //		samples = 4;
            //	}
            //	mode = new GraphicsMode (color, depthBit, stencilBit, samples);
            //	try
            //	{
            //		Context = new GraphicsContext (mode, wnd, major, minor, flags);
            //	}
            //	catch (Exception e)
            //	{
            //		mLogger.Log (string.Format ("Failed to create OpenGL context, retrying. Error: {0}", e));
            //		major = 1;
            //		minor = 0;
            //		flags = GraphicsContextFlags.Default;
            //		Context = new GraphicsContext (mode, wnd, major, minor, flags);
            //	}
            //}
            //Context.MakeCurrent (wnd);
            //(Context as IGraphicsContextInternal).LoadAll ();
            ////Context.SwapInterval = mDeviceQuery.GetSwapInterval (mPresentation.PresentationInterval);
            //// TODO : background threading 
            //// Provide the graphics context for background loading
            //// Note: this context should use the same GraphicsMode,
            //// major, minor version and flags parameters as the main
            //// context. Otherwise, context sharing will very likely fail.
            ////			if (Threading.BackgroundContext == null)
            ////			{
            ////				Threading.BackgroundContext = new GraphicsContext(mode, wnd, major, minor, flags);
            ////				Threading.WindowInfo = wnd;
            ////				Threading.BackgroundContext.MakeCurrent(null);
            ////			}
            //Context.MakeCurrent (wnd);
            mBBContext.SetupContext(mWindow.WindowInfo, createInfo);

			mExtensions.Initialize ();
			//mCapabilities.Initialize ();
			//mGLPlatform.Initialize ();
			mSelector.Initialize();
			mQueueRenderer.Initialize ();
		}

		IGLRenderPass mRenderpass;
		void SetupRenderpass (MgGraphicsDeviceCreateInfo createInfo)
		{
			var attachmentDescriptions = new [] {
				new MgAttachmentDescription {
					Format = createInfo.Color,
					LoadOp = MgAttachmentLoadOp.CLEAR,
					StoreOp = MgAttachmentStoreOp.STORE,
					StencilLoadOp = MgAttachmentLoadOp.DONT_CARE,
					StencilStoreOp = MgAttachmentStoreOp.DONT_CARE,
				},
				new MgAttachmentDescription {
					Format = createInfo.DepthStencil,
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

			SetupContext (createInfo);
			SetupRenderpass (createInfo);

            // MANDATORY
            swapchainCollection.Create (setupCmdBuffer, createInfo.Width, createInfo.Height);
					
			SetupSwapchain (swapchainCollection, createInfo);

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

		void SetupSwapchain (IMgSwapchainCollection swapchainCollection, MgGraphicsDeviceCreateInfo createInfo)
		{
			if (swapchainCollection.Swapchain == null)
			{
				throw new ArgumentNullException (nameof(swapchainCollection));
			}
            var collection = (OpenTKSwapchainCollection) swapchainCollection;
            collection.Format = createInfo.Color;

            var sc = (IOpenTKSwapchainKHR) swapchainCollection.Swapchain;
            Debug.Assert(sc != null, nameof(swapchainCollection.Swapchain) + " is Not a IOpenTKSwapchainKHR type");
			sc.Initialize ((uint)swapchainCollection.Buffers.Length);
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

