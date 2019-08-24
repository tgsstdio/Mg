using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkDisplayModeProperties2KHR
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public VkDisplayModePropertiesKHR displayModeProperties { get; set; }
	}
}
