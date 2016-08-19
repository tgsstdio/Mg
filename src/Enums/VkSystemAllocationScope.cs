using System;

namespace Magnesium.Vulkan
{
	internal enum VkSystemAllocationScope : uint
	{
		SystemAllocationScopeCommand = 0,
		SystemAllocationScopeObject = 1,
		SystemAllocationScopeCache = 2,
		SystemAllocationScopeDevice = 3,
		SystemAllocationScopeInstance = 4,
	}
}
