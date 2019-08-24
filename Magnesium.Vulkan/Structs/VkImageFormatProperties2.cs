using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkImageFormatProperties2
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public VkImageFormatProperties imageFormatProperties { get; set; }
	}
}
