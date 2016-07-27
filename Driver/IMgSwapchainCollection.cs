using System;

namespace Magnesium
{

	// CODE taken from vulkanswapchain.hpp by Sascha Willems 2016 (licensed under the MIT license)	
	// 
	public interface IMgSwapchainCollection : IDisposable
	{
		IMgSwapchainKHR Swapchain { get; }
		UInt32 Width { get; }
		UInt32 Height { get; }
		void Create(IMgCommandBuffer cmd, UInt32 width, UInt32 height);
		MgSwapchainBuffer[] Buffers { get; }
	}
}

