using System;
using Magnesium.Metal.Internals;
using MetalKit;

namespace Magnesium.Metal
{
	// initialises connection from MTKView disposable drawables as 
	// swapchain image buffers
	public class AmtSwapchainCollection : IMgSwapchainCollection
	{
		private AmtSwapchainKHR mSwapchain;

		private MTKView mApplicationView;

		public AmtSwapchainCollection(MTKView view)
		{
			mApplicationView = view;
			var color = new AmtDynamicBoundedImageView();
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
			internal set;
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

        public void Create(IMgCommandBuffer cmd,
                           MgColorFormatOption option, 
                           MgFormat overrideColor, 
                           uint width, uint height)
		{
			Width = width;
			Height = height;

            Format =  (option == MgColorFormatOption.USE_OVERRIDE)
                ? overrideColor
                : MgFormat.B8G8R8A8_UNORM;   
		}

		public void Dispose()
		{
	
		}
	}
}
