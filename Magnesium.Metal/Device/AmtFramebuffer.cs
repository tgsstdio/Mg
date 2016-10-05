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

				var noOfSubpasses = bRenderpass.Subpasses.Length;
				Subpasses = new AmtFramebufferSubpassInfo[noOfSubpasses];

				for (var i = 0; i < noOfSubpasses; i++)
				{
					var srcSubpass = bRenderpass.Subpasses[i];
					Debug.Assert(srcSubpass.ColorAttachments != null);

					var noOfColorAttachments = srcSubpass.ColorAttachments.Length;

					var dstSubpass = new AmtFramebufferSubpassInfo
					{
						ColorAttachments = new IAmtImageView[noOfColorAttachments],
						DepthStencil = (srcSubpass.DepthStencil != null)
							? (IAmtImageView)createInfo.Attachments[srcSubpass.DepthStencil.Index]
							: null,
					};

					for (var j = 0; j < noOfColorAttachments; ++j)
					{
						var imageViewIndex = srcSubpass.ColorAttachments[j].Index;
						dstSubpass.ColorAttachments[j] = (IAmtImageView)createInfo.Attachments[imageViewIndex];
					}

					Subpasses[i] = dstSubpass;
				}
			}
			else
			{
				Subpasses = new AmtFramebufferSubpassInfo[] { };
			}

		}

		public AmtFramebufferSubpassInfo[] Subpasses { get; private set; }

		public void DestroyFramebuffer(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			
		}
	}
}
