using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkSamplerCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public VkFilter magFilter { get; set; }
		public VkFilter minFilter { get; set; }
		public VkSamplerMipmapMode mipmapMode { get; set; }
		public VkSamplerAddressMode addressModeU { get; set; }
		public VkSamplerAddressMode addressModeV { get; set; }
		public VkSamplerAddressMode addressModeW { get; set; }
		public float mipLodBias { get; set; }
		public VkBool32 anisotropyEnable { get; set; }
		public float maxAnisotropy { get; set; }
		public VkBool32 compareEnable { get; set; }
		public VkCompareOp compareOp { get; set; }
		public float minLod { get; set; }
		public float maxLod { get; set; }
		public VkBorderColor borderColor { get; set; }
		public VkBool32 unnormalizedCoordinates { get; set; }
	}
}
