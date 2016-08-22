using System;

namespace Magnesium.Vulkan
{
	internal enum VkPhysicalDeviceType : uint
	{
		PhysicalDeviceTypeOther = 0,
		PhysicalDeviceTypeIntegratedGpu = 1,
		PhysicalDeviceTypeDiscreteGpu = 2,
		PhysicalDeviceTypeVirtualGpu = 3,
		PhysicalDeviceTypeCpu = 4,
	}
}
