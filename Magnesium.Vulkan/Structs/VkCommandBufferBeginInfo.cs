using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkCommandBufferBeginInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public VkCommandBufferUsageFlags flags { get; set; }
		public IntPtr pInheritanceInfo { get; set; }
	}
}
