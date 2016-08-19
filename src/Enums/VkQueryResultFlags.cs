using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkQueryResultFlags : uint
	{
		QueryResult64 = 0x1,
		QueryResultWait = 0x2,
		QueryResultWithAvailability = 0x4,
		QueryResultPartial = 0x8,
	}
}
