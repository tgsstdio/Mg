using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkMemoryType
	{
		public VkMemoryPropertyFlags propertyFlags { get; set; }
		public UInt32 heapIndex { get; set; }
	}
}
