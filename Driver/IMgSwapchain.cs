using System;

namespace Magnesium
{

	// CODE taken from vulkanswapchain.hpp by Sascha Willems 2016 (licensed under the MIT license)	
	// 
	public interface IMgSwapchain : IDisposable
	{
		void Setup();
		void Create(IMgCommandBuffer cmd, UInt32 width, UInt32 height);
	}
}

