using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkSubresourceLayout
	{
		public UInt64 offset { get; set; }
		public UInt64 size { get; set; }
		public UInt64 rowPitch { get; set; }
		public UInt64 arrayPitch { get; set; }
		public UInt64 depthPitch { get; set; }
	}
}
