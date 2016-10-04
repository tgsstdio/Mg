using System;
namespace Magnesium.Metal
{
	public class AmtSwapchainCollection : IMgSwapchainCollection
	{
		public AmtSwapchainCollection()
		{
			Buffers = new MgSwapchainBuffer[]
			{
				new MgSwapchainBuffer
				{
					View = new AmtNullImageView(),
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
				throw new NotImplementedException();
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
