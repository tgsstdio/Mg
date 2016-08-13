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

		IMgImageView View {
			get;
		}

		IMgRenderPass Renderpass {
			get;
		}

		IMgFramebuffer[] Framebuffers {
			get;
		}

		void Create(MgGraphicsDeviceCreateInfo createInfo);

		bool DeviceCreated();
		bool IsDisposed();
	}
}

