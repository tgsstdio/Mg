using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkSurfaceFormatKHR
	{
		public VkFormat format { get; set; }
		public VkColorSpaceKhr colorSpace { get; set; }
	}
}
