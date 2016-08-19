using System;

namespace Magnesium.Vulkan
{
	internal enum VkQueryType : uint
	{
		QueryTypeOcclusion = 0,
		QueryTypePipelineStatistics = 1,
		QueryTypeTimestamp = 2,
	}
}
