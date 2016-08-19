using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkPhysicalDeviceMemoryProperties
	{
		public UInt32 memoryTypeCount { get; set; }
		public VkMemoryType memoryTypes { get; set; }
		public UInt32 memoryHeapCount { get; set; }
		public VkMemoryHeap memoryHeaps { get; set; }
	}
}
