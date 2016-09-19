using System;

namespace Magnesium.Vulkan
{
	internal enum VkCompareOp : uint
	{
		CompareOpNever = 0,
		CompareOpLess = 1,
		CompareOpEqual = 2,
		CompareOpLessOrEqual = 3,
		CompareOpGreater = 4,
		CompareOpNotEqual = 5,
		CompareOpGreaterOrEqual = 6,
		CompareOpAlways = 7,
	}
}
