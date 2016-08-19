using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkPipelineCreateFlags : uint
	{
		PipelineCreateDisableOptimization = 0x1,
		PipelineCreateAllowDerivatives = 0x2,
		PipelineCreateDerivative = 0x4,
	}
}
