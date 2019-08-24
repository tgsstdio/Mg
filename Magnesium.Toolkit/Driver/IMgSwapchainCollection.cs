using System;

namespace Magnesium.Toolkit
{
	// CODE taken from vulkanswapchain.hpp by Sascha Willems 2016 (licensed under the MIT license)	
	// 
	public interface IMgSwapchainCollection : IDisposable
    {
        MgFormat Format { get; }
        UInt32 Width { get; }
        UInt32 Height { get; }
        void Create(IMgCommandBuffer cmd, MgColorFormatOption option, MgFormat overrideFormat, UInt32 width, UInt32 height);
        MgSwapchainBuffer[] Buffers { get; }
        IMgSwapchainKHR Swapchain { get; }
	}
}

