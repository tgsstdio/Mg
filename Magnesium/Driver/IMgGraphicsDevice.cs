using System;

namespace Magnesium
{
	// TODO : rename this class 
	public interface IMgGraphicsDevice : IDisposable
	{
		MgRect2D Scissor {
			get;
		}

		MgViewport CurrentViewport
		{
			get;
		}

		IMgImageView DepthStencilImageView {
			get;
		}

		IMgRenderPass Renderpass {
			get;
		}

		IMgFramebuffer[] Framebuffers {
			get;
		}


		bool DeviceCreated();
		bool IsDisposed();
        void Create(IMgCommandBuffer setupCmdBuffer, IMgSwapchainCollection mSwapchainCollection, MgGraphicsDeviceCreateInfo dsCreateInfo);
    }
}

