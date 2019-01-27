using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkSurfaceFormatKHR
	{
		public MgFormat format { get; set; }
		public MgColorSpaceKHR colorSpace { get; set; }
	}
}
