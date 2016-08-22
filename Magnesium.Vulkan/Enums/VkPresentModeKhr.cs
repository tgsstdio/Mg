using System;

namespace Magnesium.Vulkan
{
	internal enum VkPresentModeKhr : uint
	{
		PresentModeImmediate = 0,
		PresentModeMailbox = 1,
		PresentModeFifo = 2,
		PresentModeFifoRelaxed = 3,
	}
}
