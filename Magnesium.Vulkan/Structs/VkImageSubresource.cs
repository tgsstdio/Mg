using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkImageSubresource
	{
		public MgImageAspectFlagBits aspectMask { get; set; }
		public UInt32 mipLevel { get; set; }
		public UInt32 arrayLayer { get; set; }
	}
}
