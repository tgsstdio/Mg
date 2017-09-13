using System;
using System.Diagnostics;
using Metal;
using MetalKit;

namespace Magnesium.Metal
{
	public class AmtGraphicsDevice : IMgGraphicsDevice
	{
		private readonly MTKView mApplicationView;

		private readonly IMgGraphicsConfiguration mConfiguration;

		private IMgRenderPass mRenderpass;

		private MgFramebufferCollection mFramebuffers;

		private bool mDeviceCreated = false;

		public AmtGraphicsDevice(MTKView view, IMgGraphicsConfiguration config)
		{
			mApplicationView = view;
			mConfiguration = config;
			mFramebuffers = new MgFramebufferCollection(config);
		}

		public MgViewport CurrentViewport
		{
			get;
			private set;
		}

		public IMgFramebuffer[] Framebuffers
		{
			get
			{
				return mFramebuffers.Framebuffers;
			}
		}

		public IMgRenderPass Renderpass
		{
			get
			{
				return mRenderpass;
			}
		}

		public MgRect2D Scissor
		{
			get;
			private set;
		}

		public IMgImageView DepthStencilImageView
		{
			get
			{
				return mDepthStencilView;
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

		void ReleaseUnmanagedResources()
		{
			mRenderpass = null;
			mDepthStencilView = null;
			mFramebuffers.Clear();
		}

		public void Create(IMgCommandBuffer setupCmdBuffer, IMgSwapchainCollection swapchainCollection, MgGraphicsDeviceCreateInfo dsCreateInfo)
		{

			if (dsCreateInfo == null)
			{
				throw new ArgumentNullException(nameof(dsCreateInfo));
			}

			if (swapchainCollection == null)
			{
				throw new ArgumentNullException(nameof(swapchainCollection));
			}
			mDeviceCreated = false;

            // USE DEFAULT 
            var colorPassFormat =
                (dsCreateInfo.Color == MgColorFormatOption.USE_OVERRIDE)
                ? dsCreateInfo.OverrideColor
                : MgFormat.B8G8R8A8_UNORM;
;			var colorFormat = AmtFormatExtensions.GetPixelFormat(colorPassFormat);

            // USE DEFAULT
            var depthPassFormat =
                (dsCreateInfo.DepthStencil == MgDepthFormatOption.USE_OVERRIDE)
                ? dsCreateInfo.OverrideDepthStencil
                : MgFormat.D32_SFLOAT_S8_UINT;
			var depthFormat = AmtFormatExtensions.GetPixelFormat(depthPassFormat);

			var sampleCount = AmtSampleCountFlagBitExtensions.TranslateSampleCount(dsCreateInfo.Samples);

			ReleaseUnmanagedResources();

			mApplicationView.ColorPixelFormat = colorFormat;
            mApplicationView.DepthStencilPixelFormat = depthFormat;
			mApplicationView.SampleCount = sampleCount;

			CreateDepthStencilImageView();
			CreateRenderpass(dsCreateInfo, colorPassFormat, depthPassFormat);

			var bSwapchainCollection = (AmtSwapchainCollection)swapchainCollection;
			bSwapchainCollection.Format = colorPassFormat;
            bSwapchainCollection.Create(
                setupCmdBuffer, dsCreateInfo.Color,
                dsCreateInfo.OverrideColor, dsCreateInfo.Width,
                dsCreateInfo.Height);

			mFramebuffers.Create(
				swapchainCollection,
				mRenderpass,
				mDepthStencilView,
				dsCreateInfo.Width,
				dsCreateInfo.Height);

			Scissor = new MgRect2D
			{
				Extent = new MgExtent2D { Width = dsCreateInfo.Width, Height = dsCreateInfo.Height },
				Offset = new MgOffset2D { X = 0, Y = 0 },
			};

			// initialise viewport
			CurrentViewport = new MgViewport
			{
				Width = dsCreateInfo.Width,
				Height = dsCreateInfo.Height,
				X = 0,
				Y = 0,
				MinDepth = 0f,
				MaxDepth = 1f,
			};
			mDeviceCreated = true;
		}

		private IMgImageView mDepthStencilView;
		void CreateDepthStencilImageView()
		{
			Debug.Assert(mApplicationView != null);
			mDepthStencilView = new AmtImageView(mApplicationView.DepthStencilTexture);
		}

		void CreateRenderpass(MgGraphicsDeviceCreateInfo createInfo, MgFormat colorFormat, MgFormat depthStencilFormat)
		{
			bool isStencilFormat = AmtFormatExtensions.IsStencilFormat(depthStencilFormat);

			var attachments = new[]
			{
				// Color attachment[0] 
				new MgAttachmentDescription{
					Format = colorFormat,
					// TODO : multisampling
					Samples = createInfo.Samples,
					LoadOp =  MgAttachmentLoadOp.CLEAR,
					StoreOp = MgAttachmentStoreOp.STORE,
					StencilLoadOp = MgAttachmentLoadOp.DONT_CARE,
					StencilStoreOp = MgAttachmentStoreOp.DONT_CARE,
					InitialLayout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
					FinalLayout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
				},
				// Depth attachment[1]
				new MgAttachmentDescription{
					Format = depthStencilFormat,
					// TODO : multisampling
					Samples = createInfo.Samples,
					LoadOp = MgAttachmentLoadOp.CLEAR,
					StoreOp = MgAttachmentStoreOp.STORE,

					//  activate stencil if needed
					StencilLoadOp = isStencilFormat ? MgAttachmentLoadOp.CLEAR : MgAttachmentLoadOp.DONT_CARE,
					StencilStoreOp = isStencilFormat ? MgAttachmentStoreOp.STORE : MgAttachmentStoreOp.DONT_CARE,

					InitialLayout = MgImageLayout.DEPTH_STENCIL_ATTACHMENT_OPTIMAL,
					FinalLayout = MgImageLayout.DEPTH_STENCIL_ATTACHMENT_OPTIMAL,
				}
			};

			var colorReference = new MgAttachmentReference
			{
				Attachment = 0,
				Layout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
			};

			var depthReference = new MgAttachmentReference
			{
				Attachment = 1,
				Layout = MgImageLayout.DEPTH_STENCIL_ATTACHMENT_OPTIMAL,
			};

			var subpass = new MgSubpassDescription
			{
				PipelineBindPoint = MgPipelineBindPoint.GRAPHICS,
				Flags = 0,
				InputAttachments = null,
				ColorAttachmentCount = 1,
				ColorAttachments = new[] { colorReference },
				ResolveAttachments = null,
				DepthStencilAttachment = depthReference,
				PreserveAttachments = null,
			};

			var renderPassInfo = new MgRenderPassCreateInfo
			{
				Attachments = attachments,
				Subpasses = new[] { subpass },
				Dependencies = null,
			};

			Result err;

			IMgRenderPass renderPass;
			err = mConfiguration.Device.CreateRenderPass(renderPassInfo, null, out renderPass);
			Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");
			mRenderpass = renderPass;
			mRenderpassInfo = renderPassInfo;
		}

		public bool DeviceCreated()
		{
			return mDeviceCreated;
		}

		private bool mIsDisposed = false;
		public void Dispose()
		{
			if (mIsDisposed)
				return;

			ReleaseUnmanagedResources();

			mIsDisposed = true;
		}

		public bool IsDisposed()
		{
			return mIsDisposed;
		}
	}
}
