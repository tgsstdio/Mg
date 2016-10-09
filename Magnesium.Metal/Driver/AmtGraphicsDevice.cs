using System;
using System.Diagnostics;
using MetalKit;

namespace Magnesium.Metal
{
	public class AmtGraphicsDevice : IMgGraphicsDevice
	{
		private readonly MTKView mApplicationView;

		private readonly IMgLogicalDevice mLogicalDevice;

		private IMgRenderPass mRenderpass;

		private IMgFramebuffer[] mFramebuffers;

		private bool mDeviceCreated = false;

		public AmtGraphicsDevice(MTKView view, IMgLogicalDevice device)
		{
			mApplicationView = view;
			mLogicalDevice = device;
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
				return mFramebuffers;
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

			var colorFormat = AmtFormatExtensions.GetPixelFormat(dsCreateInfo.Color);
			var depthFormat = AmtFormatExtensions.GetPixelFormat(dsCreateInfo.DepthStencil);
			var sampleCount = AmtSampleCountFlagBitExtensions.TranslateSampleCount(dsCreateInfo.Samples);

			mApplicationView.ColorPixelFormat = colorFormat;
			mApplicationView.DepthStencilPixelFormat = depthFormat;
			mApplicationView.SampleCount = sampleCount;

			mFramebuffers = null;
			mRenderpass = null;
			mDepthStencilView = null;

			CreateDepthStencilImageView();
			CreateRenderpass(dsCreateInfo);
			swapchainCollection.Create(setupCmdBuffer, dsCreateInfo.Width, dsCreateInfo.Height);
			CreateFramebuffers(swapchainCollection, dsCreateInfo);

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

		void CreateRenderpass(MgGraphicsDeviceCreateInfo createInfo)
		{
			bool isStencilFormat = AmtFormatExtensions.IsStencilFormat(createInfo.DepthStencil);

			var attachments = new[]
			{
				// Color attachment[0] 
				new MgAttachmentDescription{
					Format = createInfo.Color,
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
					Format = createInfo.DepthStencil,
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
			err = mLogicalDevice.Device.CreateRenderPass(renderPassInfo, null, out renderPass);
			Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");
			mRenderpass = renderPass;
		}

		void CreateFramebuffers(IMgSwapchainCollection swapchains, MgGraphicsDeviceCreateInfo createInfo)
		{
			Debug.Assert(mDepthStencilView != null);
			// Create frame buffers for every swap chain image
			var frameBuffers = new IMgFramebuffer[swapchains.Buffers.Length];
			for (uint i = 0; i < frameBuffers.Length; i++)
			{
				var frameBufferCreateInfo = new MgFramebufferCreateInfo
				{
					RenderPass = mRenderpass,
					Attachments = new[]
					{
						swapchains.Buffers[i].View,
						// Depth/Stencil attachment is the same for all frame buffers
						mDepthStencilView,
					},
					Width = createInfo.Width,
					Height = createInfo.Height,
					Layers = 1,
				};

				var err = mLogicalDevice.Device.CreateFramebuffer(frameBufferCreateInfo, null, out frameBuffers[i]);
				Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");
			}

			mFramebuffers = frameBuffers;
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

			mIsDisposed = true;
		}

		public bool IsDisposed()
		{
			return mIsDisposed;
		}
	}
}
