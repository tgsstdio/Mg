using System;
using MetalKit;

namespace Magnesium.Metal
{
	public class AmtSwapchainCollection : IMgSwapchainCollection
	{
		private AmtSwapchainKHR mSwapchain;
		public AmtSwapchainCollection(MTKView view)
		{
			var depthStencil = new AmtNullImageView();
			var color = new AmtNullImageView();
			mSwapchain = new AmtSwapchainKHR(view, color);

			Buffers = new MgSwapchainBuffer[]
			{
				new MgSwapchainBuffer
				{
					View = color,
				},
			};
		}

		public MgSwapchainBuffer[] Buffers
		{
			get;
			private set;
		}

		public MgFormat Format
		{
			get;
			private set;
		}

		public uint Height
		{
			get;
			private set;
		}

		public IMgSwapchainKHR Swapchain
		{
			get
			{
				return mSwapchain;
			}
		}

		public uint Width
		{
			get;
			private set;
		}

		public void Create(IMgCommandBuffer cmd, uint width, uint height)
		{
			Width = width;
			Height = height;
		}

		public void Dispose()
		{
	
		}
	}
}
