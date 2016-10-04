using System;
using System.Diagnostics;

namespace Magnesium.Metal
{
	public class AmtFramebuffer : IMgFramebuffer
	{
		public uint Width { get; private set; }

		public uint Height { get; private set; }

		public uint Layers { get; private set; }

		public AmtFramebuffer(MgFramebufferCreateInfo createInfo)
		{
			if (createInfo == null)
			{
				throw new ArgumentNullException(nameof(createInfo));
			}

			if (createInfo.RenderPass == null)
			{
				throw new ArgumentNullException(nameof(createInfo.RenderPass));
			}

			Width = createInfo.Width;
			Height = createInfo.Height;
			Layers = createInfo.Layers;

			if (createInfo.Attachments != null)
			{
				var bRenderpass = (AmtRenderPass)createInfo.RenderPass;


			}

		}

		public IMgImageView[] ColorTexture { get; set; }
		public IMgImageView DepthTexture { get; set; }

		public void DestroyFramebuffer(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			
		}
	}
}
