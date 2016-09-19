using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkCommandPoolCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public VkCommandPoolCreateFlags flags { get; set; }
		public UInt32 queueFamilyIndex { get; set; }
	}
}
