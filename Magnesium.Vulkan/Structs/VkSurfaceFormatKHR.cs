using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkSurfaceFormatKHR
	{
		public VkFormat format { get; set; }
		public VkColorSpaceKhr colorSpace { get; set; }
	}
}
