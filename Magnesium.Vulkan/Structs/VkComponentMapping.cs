using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkComponentMapping
	{
		public VkComponentSwizzle r { get; set; }
		public VkComponentSwizzle g { get; set; }
		public VkComponentSwizzle b { get; set; }
		public VkComponentSwizzle a { get; set; }
	}
}
