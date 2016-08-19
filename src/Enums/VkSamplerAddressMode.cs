using System;

namespace Magnesium.Vulkan
{
	internal enum VkSamplerAddressMode : uint
	{
		SamplerAddressModeRepeat = 0,
		SamplerAddressModeMirroredRepeat = 1,
		SamplerAddressModeClampToEdge = 2,
		SamplerAddressModeClampToBorder = 3,
		SamplerAddressModeMirrorClampToEdge = 4,
	}
}
