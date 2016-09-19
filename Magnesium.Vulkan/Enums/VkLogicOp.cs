using System;

namespace Magnesium.Vulkan
{
	internal enum VkLogicOp : uint
	{
		LogicOpClear = 0,
		LogicOpAnd = 1,
		LogicOpAndReverse = 2,
		LogicOpCopy = 3,
		LogicOpAndInverted = 4,
		LogicOpNoOp = 5,
		LogicOpXor = 6,
		LogicOpOr = 7,
		LogicOpNor = 8,
		LogicOpEquivalent = 9,
		LogicOpInvert = 10,
		LogicOpOrReverse = 11,
		LogicOpCopyInverted = 12,
		LogicOpOrInverted = 13,
		LogicOpNand = 14,
		LogicOpSet = 15,
	}
}
