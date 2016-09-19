using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkSampleCountFlags : uint
	{
		SampleCount1 = 0x1,
		SampleCount2 = 0x2,
		SampleCount4 = 0x4,
		SampleCount8 = 0x8,
		SampleCount16 = 0x10,
		SampleCount32 = 0x20,
		SampleCount64 = 0x40,
	}
}
